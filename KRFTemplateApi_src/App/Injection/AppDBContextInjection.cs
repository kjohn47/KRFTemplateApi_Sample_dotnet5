namespace KRFTemplateApi.App.Injection
{
    using System;
    using System.Linq;

    using KRFCommon.Database;

    using KRFTemplateApi.App.DatabaseQueries;
    using KRFTemplateApi.Infrastructure.Database.Context;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class AppDBContextInjection
    {
        public static void InjectDBContext( IServiceCollection services, KRFDatabases databaseSettings = null )
        {
            if ( databaseSettings != null && databaseSettings.Databases != null && databaseSettings.Databases.Any() )
            {
                KRFDbContextInjectHelper.InjectDBContext<SampleDBContext>( services, databaseSettings.Databases.ElementAt( 0 ), databaseSettings.MigrationAssembly );

                services.AddScoped( x => new Lazy<ISampleDatabaseQuery>( () => new SampleDatabaseQuery( x.GetService<SampleDBContext>() ) ) );
            }
        }

        public static void ConfigureDBContext( IApplicationBuilder app, KRFDatabases databaseSettings = null )
        {
            if ( databaseSettings != null && databaseSettings.EnableAutomaticMigration && databaseSettings.Databases != null )
            {
                using ( var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope() )
                {
                    KRFDbContextInjectHelper.ConfigureAutomaticMigrations<SampleDBContext>( serviceScope );
                }
            }
        }
    }
}