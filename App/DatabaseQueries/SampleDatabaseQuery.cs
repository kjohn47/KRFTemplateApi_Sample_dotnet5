namespace KRFTemplateApi.App.DatabaseQueries
{
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    using KRFTemplateApi.Domain.Database.Sample;
    using KRFTemplateApi.Infrastructure.Database.Context;

    public class SampleDatabaseQuery: ISampleDatabaseQuery
    {
        private readonly SampleDBContext _sampleDBContext;
        public SampleDatabaseQuery(SampleDBContext sampleDBContext)
        {
            this._sampleDBContext = sampleDBContext;
        }

        public async Task<IEnumerable<SampleTable>> GetSampleListAsync(string code = null)
        {
            if(string.IsNullOrEmpty(code))
                return await this._sampleDBContext.SampleTable.AsNoTracking().OrderBy(x => x.TemperatureMin).ToListAsync();

            return await this._sampleDBContext.SampleTable.AsNoTracking().Where(x => x.Code.Contains(code)).OrderBy(x => x.TemperatureMin).ToListAsync();
        }

        public async Task<SampleQueryByTemperature> GetSampleFromTemperatureAsync(int temperature)
        {
            return await this._sampleDBContext.SampleTable.AsNoTracking()
                .Where(q => q.TemperatureMin <= temperature && q.TemperatureMax >= temperature )
                .Select(r => new SampleQueryByTemperature(r.Code, r.Description))
                .FirstOrDefaultAsync();
        }

        public async Task<string> AddTemperatureRangeAsync(int min, int max, string code, string description)
        {
            var db_range = await this._sampleDBContext.SampleTable.AsNoTracking()
                        .Where(q => (min >= q.TemperatureMin && min <= q.TemperatureMax) ||
                                    (max >= q.TemperatureMin && max <= q.TemperatureMax))
                        .CountAsync();

            if (db_range > 0)
            {
                return "Range overlaps with existent data";
            }

            var db_code = await this._sampleDBContext.SampleTable.AsNoTracking()
                        .Where(q => q.Code.Equals(code))
                        .CountAsync();

            if (db_code > 0)
            {
                return "Code already exists";
            }

            var newData = new SampleTable
            {
                Code = code,
                TemperatureMin = min,
                TemperatureMax = max,
                Description = description
            };

            await this._sampleDBContext.SampleTable.AddAsync(newData);
            await this._sampleDBContext.SaveChangesAsync();

            return string.Empty;
        }
    }
}