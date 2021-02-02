namespace KRFTemplateApi.Domain.CQRS.Sample.Command
{
    public class SampleCommandInput
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
