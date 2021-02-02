namespace KRFTemplateApi.Domain.Database.Sample
{
    using System.ComponentModel.DataAnnotations;

    public class SampleTable
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public int TemperatureMin { get; set; }
        public int TemperatureMax { get; set; }
    }
}
