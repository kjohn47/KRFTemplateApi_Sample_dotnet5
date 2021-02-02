namespace KRFTemplateApi.Domain.CQRS.Sample.Query
{
    using System;

    public class SampleOutputItem
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public string UserData { get; set; }
    }
}
