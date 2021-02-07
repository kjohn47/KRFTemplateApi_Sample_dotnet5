namespace KRFTemplateApi.App.Injection
{
    using KRFCommon.CQRS.Query;

    using KRFTemplateApi.App.CQRS.Sample.Query;
    using KRFTemplateApi.Domain.CQRS.Sample.Query;

    using Microsoft.Extensions.DependencyInjection;

    public static class AppQueryInjection
    {
        public static void InjectAppQueries( this IServiceCollection services )
        {
            services.AddTransient<IQuery<SampleInput, SampleOutput>, GetSampleData>();
            services.AddTransient<IQuery<ListSampleInput, ListSampleOutput>, ListaAllSample>();
        }
    }
}
