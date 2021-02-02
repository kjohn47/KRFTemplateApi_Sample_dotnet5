namespace KRFTemplateApi.Infrastructure.Database.Context
{
    using Microsoft.EntityFrameworkCore;

    using KRFTemplateApi.Domain.Database.Sample;
    using KRFTemplateApi.Infrastructure.Database.Configuration;
    using KRFTemplateApi.Infrastructure.Database.DataSeed;

    public class SampleDBContext : DbContext
    {
        public SampleDBContext(DbContextOptions options) : base(options)
        {
        }

        //context tables
        public DbSet<SampleTable> SampleTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure table and seed initial data
            modelBuilder.Entity<SampleTable>(e => {
                SampleTableConfiguration.Configure(e);
                SampleTableSeeder.Seed(e);
            });
        }
    }
}
