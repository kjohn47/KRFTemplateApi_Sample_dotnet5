namespace KRFTemplateApi.Domain.CQRS.Sample.Command
{
    using KRFCommon.CQRS.Command;

    public class SampleCommandOutput: ICommandResponse
    {
        public string Result { get; set; }
    }
}
