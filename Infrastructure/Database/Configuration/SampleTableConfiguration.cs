namespace KRFTemplateApi.Infrastructure.Database.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using KRFTemplateApi.Domain.Database.Sample;    

    public static class SampleTableConfiguration
    {
        public static void Configure(EntityTypeBuilder<SampleTable> entity)
        {
            entity.ToTable("SampleTable");
            entity.Property(x => x.Code).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).IsRequired();
            entity.Property(x => x.TemperatureMin).IsRequired();
            entity.Property(x => x.TemperatureMax).IsRequired();

        }
    }
}
