namespace KRFTemplateApi.App.DatabaseQueries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KRFCommon.Database;

    using KRFTemplateApi.Domain.Database.Sample;

    public interface ISampleDatabaseQuery
    {
        Task<IEnumerable<SampleTable>> GetSampleListAsync(string code = null);

        Task<SampleQueryByTemperature> GetSampleFromTemperatureAsync(int temperature);

        Task<IQueryCommand> AddTemperatureRangeAsync(int min, int max, string code, string description);
    }
}
