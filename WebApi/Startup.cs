namespace KRFTemplateApi.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using KRFCommon.Context;
    using KRFCommon.Handler;
    using KRFCommon.Swagger;

    using KRFTemplateApi.App.Constants;
    using KRFTemplateApi.App.Injection;

    public class Startup
    {       
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this._apiName = configuration.GetValue(AppConstants.AppName_Key, string.Empty);
            this._tokenIdentifier = configuration.GetValue(AppConstants.TokenIdentifier_Key, string.Empty);
            this._tokenKey = configuration.GetValue(AppConstants.TokenKey_Key, string.Empty);
            this.HostingEnvironment = env;
        }

        private readonly string _apiName;
        private readonly string _tokenIdentifier;
        private readonly string _tokenKey;

        public IWebHostEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add logger config
            services.AddLogging(l =>
            {
                var config = this.Configuration.GetSection(AppConstants.Logging);
                l.ClearProviders();
                l.AddConfiguration(config);
                l.AddConsole();
                l.AddDebug();
                l.AddEventLog();
                l.AddEventSourceLogger();
            });

            InjectUserContext.InjectContext(services, this._tokenIdentifier, this._tokenKey);

            services.AddControllers();

            SwaggerInit.ServiceInit(services, this._apiName, this._tokenIdentifier);

            //Database settings
            var connStr = this.Configuration.GetConnectionString(AppConstants.DefaultConStr);
            var localDb = this.Configuration.GetValue(AppConstants.UseLocalDB, false);
            var migAssemb = this.Configuration.GetValue(AppConstants.MigrationAssembly, string.Empty);
            var apiDbFolder = this.Configuration.GetValue(AppConstants.ApiDBFolder, string.Empty);

            //Dependency injection
            AppDBContextInjection.InjectDBContext(services, connStr, migAssemb, localDb, apiDbFolder);
            AppQueryInjection.InjectQuery(services);
            AppCommandInjection.InjectCommand(services);
            AppProxyInjection.InjectProxy(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //server config settings
            bool enableLogs = this.Configuration.GetValue(AppConstants.LogsOnPrd, false);
            bool reqCtxEnableRead = this.Configuration.GetValue(AppConstants.ReqCtxEnableRead, false);
            bool reqCtxMemBufferOnly = this.Configuration.GetValue(AppConstants.ReqCtxMemBufferOnly, false);
            int reqCtxBufferSize = this.Configuration.GetValue(AppConstants.ReqCtxBufferSize, 30000);

            if (this.HostingEnvironment.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
                enableLogs = true;
            }                                     

            if (enableLogs && reqCtxEnableRead)
            {                
                app.UseMiddleware<KRFBodyRewindMiddleware>(reqCtxBufferSize, reqCtxMemBufferOnly);
            }

            if (reqCtxEnableRead && reqCtxMemBufferOnly)
            {
                KRFExceptionHandlerMiddleware.Configure(app, loggerFactory, enableLogs, this._apiName, this._tokenIdentifier, reqCtxBufferSize);
            }
            else
            {
                KRFExceptionHandlerMiddleware.Configure(app, loggerFactory, enableLogs, this._apiName, this._tokenIdentifier);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            AuthConfigure.Configure(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SwaggerInit.Configure(app, this._apiName);

            AppDBContextInjection.ConfigureDBContext(app);
        }
    }
}