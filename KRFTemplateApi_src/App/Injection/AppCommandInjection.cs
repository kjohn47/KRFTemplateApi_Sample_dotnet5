namespace KRFTemplateApi.App.Injection
{


    using KRFCommon.CQRS.Command;

    using KRFTemplateApi.App.CQRS.Sample.Command;
    using KRFTemplateApi.Domain.CQRS.Sample.Command;

    using Microsoft.Extensions.DependencyInjection;

    public static class AppCommandInjection
    {
        public static void InjectCommand( IServiceCollection services )
        {
            services.AddTransient<ICommand<SampleCommandInput, SampleCommandOutput>, PostSampleData>();
        }
    }
}
