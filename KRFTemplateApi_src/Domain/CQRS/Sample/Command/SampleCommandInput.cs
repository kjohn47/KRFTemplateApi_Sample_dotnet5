namespace KRFTemplateApi.Domain.CQRS.Sample.Command
{
    using KRFCommon.CQRS.Command;

    public class SampleCommandInput : ICommandRequest
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
