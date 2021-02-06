namespace KRFTemplateApi.Infrastructure.Database.Configuration
{
    using KRFTemplateApi.Domain.Database.Sample;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SampleTableConfiguration
    {
        public static void Configure( EntityTypeBuilder<SampleTable> entity )
        {
            entity.ToTable( "SampleTable" );
            entity.HasKey( s => s.Code ).HasName( "PK_SAMPLE" );
            entity.Property( x => x.Code ).HasMaxLength( 100 ).IsRequired();
            entity.Property( x => x.Description ).HasMaxLength( 200 ).IsRequired();
            entity.Property( x => x.TemperatureMin ).IsRequired();
            entity.Property( x => x.TemperatureMax ).IsRequired();

        }
    }
}
