namespace KRFTemplateApi.App.Injection
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using KRFTemplateApi.App.DatabaseQueries;
    using KRFTemplateApi.Infrastructure.Database.Context;

    public static class AppDBContextInjection
    {
        public static void InjectDBContext(IServiceCollection services, string connectionString, string migrationAssembly, bool useLocalDb = false, string apiDbFolder = null)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<SampleDBContext>(opt =>
            {
                opt.UseSqlServer(useLocalDb && !string.IsNullOrEmpty(apiDbFolder) 
                    ? connectionString.Replace(apiDbFolder, string.Concat(Environment.CurrentDirectory, "\\", apiDbFolder) ) 
                    : connectionString, x =>
                {
                    x.MigrationsAssembly(migrationAssembly);
                });
            });

            services.AddScoped( x => new Lazy<ISampleDatabaseQuery>( () => new SampleDatabaseQuery(x.GetService<SampleDBContext>()) ));
        }

        public static void ConfigureDBContext( IApplicationBuilder app )
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<SampleDBContext>().Database.Migrate();
            }
        }
    }
}
