namespace KRFTemplateApi.WebApi
{
    using KRFCommon.Api;
    using KRFCommon.Constants;
    using KRFCommon.Context;
    using KRFCommon.Database;
    using KRFCommon.Middleware;
    using KRFCommon.Logger;
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
            this.HostingEnvironment = env;
        }

        private readonly AppConfiguration _apiSettings;
        private readonly RequestContext _requestContext;
        private readonly KRFDatabases _databases;
        private readonly bool _enableLogs;

        public IWebHostEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            //Add logger config
            services.AddLogging( l =>
            {
                l.ClearProviders();
                l.AddConfiguration( this.Configuration.GetSection( KRFApiSettings.Logging_Key ) );
                l.AddConsole();
                l.AddDebug();
                l.AddEventLog();
                l.AddEventSourceLogger();
                l.AddKRFLogToFileLogger( this.Configuration.GetSection( KRFApiSettings.KRFLoggerConfig_Key ) );
            } );

            services.InjectUserContext( this._apiSettings.TokenIdentifier, this._apiSettings.TokenKey );

            services.AddControllers();

            services.SwaggerInit( this._apiSettings.ApiName, this._apiSettings.TokenIdentifier );

            services.AddKRFMemoryCache( this.Configuration.GetSection( KRFApiSettings.MemoryCacheSettings_Key ) );

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

            app.KRFLogAndExceptionHandlerConfigure(
                loggerFactory,
                ( this._enableLogs || isDev ),
                this._apiSettings.ApiName,
                this._apiSettings.TokenIdentifier,
                this._requestContext.EnableRequestRead );

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