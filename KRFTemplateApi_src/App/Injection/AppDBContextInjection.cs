namespace KRFTemplateApi.App.Injection
{
    using System;

    using KRFCommon.Database;

    using KRFTemplateApi.App.DatabaseQueries;
    using KRFTemplateApi.Infrastructure.Database.Context;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class AppDBContextInjection
    {
        public static void InjectAppDBContext( this IServiceCollection services, KRFDatabases databaseSettings = null )
        {
            //Context inject
            services.InjectDBContext<SampleDBContext>( databaseSettings );

            //Inject database query handlers
            services.AddScoped( x => new Lazy<ISampleDatabaseQuery>( () => new SampleDatabaseQuery( x.GetService<SampleDBContext>() ) ) );
        }

        public static void ConfigureAppDBContext( this IApplicationBuilder app, KRFDatabases databaseSettings = null )
        {
            //Inject Migration Automation
            if ( databaseSettings != null && databaseSettings.EnableAutomaticMigration )
            {
                using ( var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope() )
                {
                    serviceScope.ConfigureAutomaticMigrations<SampleDBContext>();
                }
            }
        }
    }
}