namespace KRFTemplateApi.App.CQRS.Sample.Query
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using KRFCommon.CQRS.Common;
    using KRFCommon.CQRS.Query;
    using KRFCommon.MemoryCache;

    using KRFTemplateApi.App.Constants;
    using KRFTemplateApi.Infrastructure.Database.Queries;
    using KRFTemplateApi.Domain.CQRS.Sample.Query;
    using KRFCommon.Constants;

    public class ListaAllSample : IQuery<ListSampleInput, ListSampleOutput>
    {
        private ISampleDatabaseQuery _sampleDB;
        private readonly IKRFMemoryCache _memoryCache;

        public ListaAllSample( Lazy<ISampleDatabaseQuery> sampleDB, IKRFMemoryCache memoryCache )
        {
            this._sampleDB = sampleDB.Value;
            this._memoryCache = memoryCache;
        }

        public async Task<IResponseOut<ListSampleOutput>> QueryAsync( ListSampleInput request )
        {
            var result = await this._memoryCache.GetCachedItemWithHandlerAsync(
                AppConstants.SampleCacheKey,
                () => this._sampleDB.GetSampleListAsync( request?.Code ),
                !string.IsNullOrEmpty( request?.Code ),
                x => ( !string.IsNullOrEmpty( request?.Code ) && !x.CacheMiss )
                    ? x.Result.Where( x => x.Code.Contains( request.Code, StringComparison.InvariantCultureIgnoreCase ) )
                    : x.Result );

            if ( result == null || !result.Any() )
            {
                return ResponseOut<ListSampleOutput>.GenerateFault( new ErrorOut( System.Net.HttpStatusCode.BadRequest, "Error Ocurred: no results", ResponseErrorType.Database ) );
            }

            return ResponseOut<ListSampleOutput>.GenerateResult( new ListSampleOutput
            {
                Samples = result
            } );
        }
    }
}