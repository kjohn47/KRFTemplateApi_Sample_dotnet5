namespace KRFTemplateApi.WebApi
{
    using KRFCommon.Api;
    using KRFCommon.Constants;
    using KRFCommon.Context;
    using KRFCommon.Database;
    using KRFCommon.Handler;
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
                 var config = this.Configuration.GetSection( KRFApiSettings.Logging_Key );
                 l.ClearProviders();
                 l.AddConfiguration( config );
                 l.AddConsole();
                 l.AddDebug();
                 l.AddEventLog();
                 l.AddEventSourceLogger();
             } );

            InjectUserContext.InjectContext( services, this._apiSettings.TokenIdentifier, this._apiSettings.TokenKey );

            services.AddControllers();

            SwaggerInit.ServiceInit( services, this._apiSettings.ApiName, this._apiSettings.TokenKey );

            //Dependency injection
            AppDBContextInjection.InjectDBContext( services, this._databases );
            AppQueryInjection.InjectQuery( services );
            AppCommandInjection.InjectCommand( services );
            AppProxyInjection.InjectProxy( services );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, ILoggerFactory loggerFactory )
        {
            //server config settings
            var enableLogs = this._enableLogs;


            if ( this.HostingEnvironment.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
                enableLogs = true;
            }

            if ( enableLogs && this._requestContext.EnableRead )
            {
                app.UseMiddleware<KRFBodyRewindMiddleware>( this._requestContext.BufferSize, this._requestContext.MemBufferOnly );
            }

            if ( this._requestContext.EnableRead && this._requestContext.MemBufferOnly )
            {
                KRFExceptionHandlerMiddleware.Configure( app, loggerFactory, enableLogs, this._apiSettings.ApiName, this._apiSettings.TokenIdentifier, this._requestContext.BufferSize );
            }
            else
            {
                KRFExceptionHandlerMiddleware.Configure( app, loggerFactory, enableLogs, this._apiSettings.ApiName, this._apiSettings.TokenIdentifier );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            AuthConfigure.Configure( app );

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllers();
             } );

            SwaggerInit.Configure( app, this._apiSettings.ApiName );

            AppDBContextInjection.ConfigureDBContext( app, this._databases );
        }
    }
}