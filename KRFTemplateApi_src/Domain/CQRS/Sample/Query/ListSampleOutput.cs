namespace KRFTemplateApi.Domain.CQRS.Sample.Query
{
    using System.Collections.Generic;

    using KRFCommon.CQRS.Query;

    using KRFTemplateApi.Domain.Database.Sample;

    public class ListSampleOutput: IQueryResponse
    {
        public IEnumerable<SampleTable> Samples { get; set; }
    }
}
