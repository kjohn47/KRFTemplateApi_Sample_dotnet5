namespace KRFTemplateApi.App.CQRS.Sample.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using KRFCommon.CQRS.Common;

    using KRFCommon.CQRS.Query;
    using KRFCommon.MemoryCache;

    using KRFTemplateApi.App.Constants;
    using KRFTemplateApi.App.DatabaseQueries;
    using KRFTemplateApi.Domain.CQRS.Sample.Query;
    using KRFTemplateApi.Domain.Database.Sample;

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
            IEnumerable<SampleTable> result = null;

            var cacheResult = await this._memoryCache.GetCachedItemWithMissReturnAsync(
                AppConstants.SampleCacheKey,
                () => this._sampleDB.GetSampleListAsync( request?.Code ),
                null,
                !string.IsNullOrEmpty( request?.Code ) );

            if ( !cacheResult.CacheMiss && !string.IsNullOrEmpty( request?.Code ) )
            {
                result = cacheResult.Result.Where( x => x.Code.Contains( request.Code, StringComparison.InvariantCultureIgnoreCase ) );
            }
            else
            {
                result = cacheResult.Result;
            }

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