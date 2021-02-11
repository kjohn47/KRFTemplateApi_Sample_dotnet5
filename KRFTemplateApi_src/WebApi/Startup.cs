namespace KRFTemplateApi.WebApi
{
    using KRFCommon.Api;
    using KRFCommon.Constants;
    using KRFCommon.Context;
    using KRFCommon.Database;
    using KRFCommon.Handler;
    using KRFCommon.MemoryCache;
    using KRFCommon.Swagger;

    using KRFTemplateApi.App.Injection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup( IConfiguration configuration, IWebHostEnvironment env )
        {
            this.Configuration = configuration;
            this._apiSettings = configuration.GetSection( KRFApiSettings.AppConfiguration_Key ).Get<AppConfiguration>();
            this._requestContext = configuration.GetSection( KRFApiSettings.RequestContext_Key ).Get<RequestContext>();
            this._databases = configuration.GetSection( KRFApiSettings.KRFDatabases_Key ).Get<KRFDatabases>();
            this._enableLogs = configuration.GetValue( KRFApiSettings.LogsOnPrd_Key, false );
            this._cacheMemorySettings = configuration.GetSection( KRFApiSettings.MemoryCacheSettings_Key ).Get<KRFMemoryCacheSettings>() ?? new KRFMemoryCacheSettings();

            this.HostingEnvironment = env;
        }

        private readonly AppConfiguration _apiSettings;
        private readonly RequestContext _requestContext;
        private readonly KRFDatabases _databases;
        private readonly bool _enableLogs;
        private readonly KRFMemoryCacheSettings _cacheMemorySettings;

        public IWebHostEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            //Add logger config
            services.AddLogging( l =>
            {
                var config = this.Configuration.GetSection( KRFApiSettings.Logging_Key );
                l.ClearProviders();
                l.AddConfiguration( config );
                l.AddConsole();
                l.AddDebug();
                l.AddEventLog();
                l.AddEventSourceLogger();
            } );

            services.InjectUserContext( this._apiSettings.TokenIdentifier, this._apiSettings.TokenKey );

            services.AddControllers();

            services.SwaggerInit( this._apiSettings.ApiName, this._apiSettings.TokenKey );

            services.AddKRFMemoryCache( this._cacheMemorySettings );

            //Dependency injection
            services.InjectAppDBContext( this._databases );
            services.InjectAppQueries();
            services.InjectAppCommands();
            services.InjectAppProxies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, ILoggerFactory loggerFactory )
        {
            //server config settings
            var isDev = this.HostingEnvironment.IsDevelopment();

            if ( isDev )
            {
                app.UseDeveloperExceptionPage();
            }

            app.KRFExceptionHandlerMiddlewareConfigure(
                loggerFactory,
                ( this._enableLogs || isDev ),
                this._apiSettings.ApiName,
                this._apiSettings.TokenIdentifier,
                this._requestContext.EnableRead,
                this._requestContext.MemBufferOnly,
                this._requestContext.BufferSize );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.AuthConfigure();

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers();
            } );

            app.SwaggerConfigure( this._apiSettings.ApiName );

            app.ConfigureAppDBContext( this._databases );
        }
    }
}