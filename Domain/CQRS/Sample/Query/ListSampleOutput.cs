namespace KRFTemplateApi.Domain.CQRS.Sample.Query
{
    using System.Collections.Generic;

    using KRFTemplateApi.Domain.Database.Sample;

    public class ListSampleOutput
    {
        public IEnumerable<SampleTable> Samples { get; set; }
    }
}
