namespace KRFTemplateApi.Domain.CQRS.Sample.Query
{  
    using System.Collections.Generic;

    using KRFCommon.CQRS.Query;

    public class SampleOutput : IQueryResponse
    {
        public IEnumerable<SampleOutputItem> Output { get; set; }
    }
}
