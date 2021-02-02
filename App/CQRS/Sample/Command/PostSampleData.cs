namespace KRFTemplateApi.App.CQRS.Sample.Command
{
    using System;
    using System.Threading.Tasks;

    using KRFCommon.CQRS.Command;
    using KRFCommon.CQRS.Common;
    using KRFCommon.CQRS.Validator;

    using KRFTemplateApi.App.DatabaseQueries;
    using KRFTemplateApi.Domain.CQRS.Sample.Command;

    public class PostSampleData : ICommand<SampleCommandInput, SampleCommandOutput>
    {
        private ISampleDatabaseQuery _sampleDB;
        public PostSampleData(Lazy<ISampleDatabaseQuery> sampleDB)
        {
            this._sampleDB = sampleDB.Value;
        }

        public async Task<CommandValidationError> ExecuteValidationAsync(SampleCommandInput request)
        {
            IKRFValidator<SampleCommandInput> validator = new PostSampleDataValidator();
            return await validator.CheckValidationAsync(request);
        }

        public async Task<IResponseOut<SampleCommandOutput>> ExecuteCommandAsync(SampleCommandInput request)
        {
            var result = await this._sampleDB.AddTemperatureRangeAsync(request.Min, request.Max, request.Code, request.Description);
            if(!string.IsNullOrEmpty(result))
            {
                return ResponseOut<SampleCommandOutput>.GenerateFault( new ErrorOut( System.Net.HttpStatusCode.BadRequest, result, ResponseErrorType.Database ) );
            }

            return ResponseOut<SampleCommandOutput>.GenerateResult( new SampleCommandOutput { Result = "Added new sample with success" } );
        }
    }
}
