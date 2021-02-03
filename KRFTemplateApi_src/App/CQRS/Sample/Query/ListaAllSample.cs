namespace KRFTemplateApi.App.CQRS.Sample.Query
{
    using System;
    using System.Threading.Tasks;
    using System.Linq;
    
    using KRFCommon.CQRS.Query;
    using KRFCommon.CQRS.Common;

    using KRFTemplateApi.Domain.CQRS.Sample.Query;
    using KRFTemplateApi.App.DatabaseQueries;

    public class ListaAllSample : IQuery<ListSampleInput, ListSampleOutput>
    {
        private ISampleDatabaseQuery _sampleDB;
        public ListaAllSample( Lazy<ISampleDatabaseQuery> sampleDB )
        {
            this._sampleDB = sampleDB.Value;
        }

        public async Task<IResponseOut<ListSampleOutput>> QueryAsync(ListSampleInput request)
        {
            var result = await this._sampleDB.GetSampleListAsync(request?.Code);
            
            if(result == null || !result.Any())
            {
                return ResponseOut<ListSampleOutput>.GenerateFault(new ErrorOut(System.Net.HttpStatusCode.BadRequest, "Error Ocurred: no results", ResponseErrorType.Database));
            }

            return ResponseOut<ListSampleOutput>.GenerateResult(new ListSampleOutput
            {
                Samples = result
            });
        }
    }
}
