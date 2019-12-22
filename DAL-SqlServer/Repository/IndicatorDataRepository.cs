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

        public IndicatorData Create(IndicatorData data)
        {
            throw new NotImplementedException();
        }

        public void Create(List<IndicatorData> data)
        {
            
        }

    }
}
