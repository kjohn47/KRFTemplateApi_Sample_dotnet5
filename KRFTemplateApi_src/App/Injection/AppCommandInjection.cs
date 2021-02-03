namespace KRFTemplateApi.App.Injection
{

    using Microsoft.Extensions.DependencyInjection;
    
    using KRFCommon.CQRS.Command;
    
    using KRFTemplateApi.App.CQRS.Sample.Command;
    using KRFTemplateApi.Domain.CQRS.Sample.Command;

    public static class AppCommandInjection
    {
        public static void InjectCommand(IServiceCollection services)
        {
            services.AddTransient<ICommand<SampleCommandInput, SampleCommandOutput>, PostSampleData>();
        }
    }
}
