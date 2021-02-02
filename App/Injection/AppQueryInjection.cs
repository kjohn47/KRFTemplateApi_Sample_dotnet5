namespace KRFTemplateApi.App.Injection
{
    using Microsoft.Extensions.DependencyInjection;

    using KRFCommon.CQRS.Query;

    using KRFTemplateApi.App.CQRS.Sample.Query;
    using KRFTemplateApi.Domain.CQRS.Sample.Query;

    public static class AppQueryInjection
    {      
        public static void InjectQuery( IServiceCollection services )
        {
            services.AddTransient<IQuery<SampleInput, SampleOutput[]>, GetSampleData>();
            services.AddTransient<IQuery<ListSampleInput, ListSampleOutput>, ListaAllSample>();
        }
    }
}
