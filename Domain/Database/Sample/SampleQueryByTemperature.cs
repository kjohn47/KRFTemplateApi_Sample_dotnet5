namespace KRFTemplateApi.Domain.Database.Sample
{
    public class SampleQueryByTemperature
    {
        public SampleQueryByTemperature(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
