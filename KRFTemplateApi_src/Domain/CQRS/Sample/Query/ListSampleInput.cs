namespace KRFTemplateApi.Domain.CQRS.Sample.Query
{
    using KRFCommon.CQRS.Query;

    public class ListSampleInput: IQueryRequest
    {
        public string Code { get; set; }
    }
}
