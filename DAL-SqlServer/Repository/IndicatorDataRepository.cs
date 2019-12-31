using DAL_SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_SqlServer.Repository
{
    public class IndicatorDataRepository<TDbContext> : IIndicatorDataRepository where TDbContext : DbContext
    {
        protected TDbContext dbContext;

        public IndicatorDataRepository(TDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<IndicatorData>> GetByCurrency(string currency)
        {
            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.Currency == currency)
                .OrderByDescending(r => r.ReleaseDateTime)
                .Take(300)
                .ToListAsync();
        }

        public async Task<List<IndicatorData>> GetIndicatorsForDate(DateTime dt)
        {
            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.ReleaseDateTime >= dt && r.ReleaseDateTime < dt.AddDays(1).AddSeconds(-1))
                .OrderBy(r => r.ReleaseDateTime)
                .ToListAsync();
        }

        public IndicatorData Create(IndicatorData data)
        {
            throw new NotImplementedException();
        }

        public void Create(List<IndicatorData> data)
        {

        }

        public List<IndicatorData> BulkUpdate(List<IndicatorData> recs)
        {
            List<IndicatorData> updRecs = new List<IndicatorData>();

            foreach (var rec in recs)
            {
                var exist = this.dbContext.Set<IndicatorData>().Where(r => r.EventId == rec.EventId).FirstOrDefault();
                if (exist == null)
                {
                    rec.CreateDate = DateTime.Now;
                    this.dbContext.Set<IndicatorData>().Add(rec);
                }
                else
                {
                    exist.ModifyDate = DateTime.Now;
                    exist.Actual = rec.Actual;
                    exist.Forecast = rec.Forecast;
                    exist.Indicator = rec.Indicator;
                    exist.Previous = rec.Previous;
                    exist.ReleaseDate = rec.ReleaseDate;
                    exist.ReleaseDateTime = rec.ReleaseDateTime;
                    exist.ReleaseTime = rec.ReleaseTime;

                    this.dbContext.Entry(exist).State = EntityState.Modified;
                    updRecs.Add(exist);
                }
            }
            this.dbContext.SaveChanges();

            return updRecs;
        }

        public async Task<List<vwCountryIndicator>> GetCurrencyIndicators()
        {
            return await this.dbContext.Set<vwCountryIndicator>()
                .OrderBy(r => r.Currency).ThenBy(r => r.Indicator)
                .ToListAsync();
        }

        public async Task<List<vwCountryIndicator>> GetCurrencyIndicatorsByCcy(string currency)
        {
            return await this.dbContext.Set<vwCountryIndicator>()
                .Where(r => r.Currency == currency)
                .OrderBy(r => r.Currency).ThenBy(r => r.Indicator)
                .ToListAsync();
        }

        public async Task<List<IndicatorData>> ThisWeek()
        {
            var dtToday = DateTime.Now;
            var dayOfWeek = Convert.ToInt16(dtToday.DayOfWeek);
            var sun = dtToday.AddDays(dayOfWeek * -1);
            var sat = sun.AddDays(6);

            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.ReleaseDateTime >= sun && r.ReleaseDateTime <= sat)
                .OrderBy(r => r.ReleaseDateTime)
                .ToListAsync();
        }
    }
}
