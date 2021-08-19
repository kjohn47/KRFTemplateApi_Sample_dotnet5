namespace KRFTemplateApi.WebApi
{
    using KRFCommon.Api;
    using KRFCommon.Constants;
    using KRFCommon.Context;
    using KRFCommon.Database;
    using KRFCommon.Logger;
    using KRFCommon.MemoryCache;
    using KRFCommon.Swagger;
    using KRFCommon.Controller;

    using KRFTemplateApi.App.Injection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using KRFCommon.Proxy;

    public class Startup
    {
        public Startup( IConfiguration configuration, IWebHostEnvironment env )
        {
            this.Configuration = configuration;
            this._apiSettings = configuration.GetSection( KRFApiSettings.AppConfiguration_Key ).Get<AppConfiguration>();
            this._databases = configuration.GetSection( KRFApiSettings.KRFDatabases_Key ).Get<KRFDatabases>();
            this._externalServices = configuration.GetSection( KRFApiSettings.KRFExternalServices_Key ).Get<KRFExternalServices>();
            this.HostingEnvironment = env;
        }

        private readonly AppConfiguration _apiSettings;
        private readonly KRFDatabases _databases;
        private readonly KRFExternalServices _externalServices;
        public IWebHostEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            //Add logger config
            services.AddLogging( l =>
            {
                l.AddKRFLogger( this.Configuration );
            } );

            services.AddUserBearerContext( this._apiSettings );

            services.AddKRFController();

            services.SwaggerInit( this._apiSettings );

            services.AddKRFMemoryCache( this.Configuration );

            //Dependency injection
            services.InjectAppDBContext( this._databases );
            services.InjectAppQueries();
            services.InjectAppCommands();
            services.InjectAppProxies( this._externalServices );
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

            app.ApiConfigure( this._apiSettings, loggerFactory, isDev );

            app.SwaggerConfigure( this._apiSettings.ApiName );

            app.ConfigureAppDBContext( this._databases );
        }
    }
}