namespace KRFTemplateApi.Infrastructure.Database.DataSeed
{
    using KRFTemplateApi.Domain.Database.Sample;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SampleTableSeeder
    {
        public static void Seed( EntityTypeBuilder<SampleTable> entity )
        {
            entity.HasData( new[] {
                new SampleTable {
                    Code = "Freezing",
                    Description = "It's freezing today",
                    TemperatureMin = -15,
                    TemperatureMax = -5
                },
                new SampleTable {
                    Code = "Bracing",
                    Description = "It's Bracing",
                    TemperatureMin = -4,
                    TemperatureMax = 0
                },
                new SampleTable {
                    Code = "Chilly",
                    Description = "It's Chilly tonight",
                    TemperatureMin = 1,
                    TemperatureMax = 5
                },
                new SampleTable {
                    Code = "Cool",
                    Description = "Cool day",
                    TemperatureMin = 6,
                    TemperatureMax = 10
                },
                new SampleTable {
                    Code = "Mild",
                    Description = "Mild mornigng",
                    TemperatureMin = 11,
                    TemperatureMax = 15
                },
                new SampleTable {
                    Code = "Warm",
                    Description = "The afternoon will be Warm",
                    TemperatureMin = 16,
                    TemperatureMax = 20
                },
                new SampleTable {
                    Code = "Balmy",
                    Description = "Balmy it is",
                    TemperatureMin = 21,
                    TemperatureMax = 25
                },
                new SampleTable {
                    Code = "Hot",
                    Description = "The day is too Hot",
                    TemperatureMin = 26,
                    TemperatureMax = 30
                },
                new SampleTable {
                    Code = "Sweltering",
                    Description = "Sweltering, i need a pool",
                    TemperatureMin = 31,
                    TemperatureMax = 35
                },
                new SampleTable {
                    Code = "Scorching",
                    Description = "It's Scorching, i'm going to the beach and swim all day",
                    TemperatureMin = 36,
                    TemperatureMax = 45
                }
            } );
        }
    }
}