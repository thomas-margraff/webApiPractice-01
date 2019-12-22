using DAL_SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_SqlServer.Repository
{
    public class CountriesRepository<TDbContext> : ICountriesRepository where TDbContext : DbContext
    {
        protected TDbContext dbContext;

        public CountriesRepository(TDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<Countries>> GetActive(bool? includeIndicators = false)
        {
            if (includeIndicators.Value)
            {
                return await this.dbContext.Set<Countries>()
                    .Include(r => r.EconomicIndicators)
                    .Where(r => r.Active).ToListAsync();
            }
            
            return await this.dbContext.Set<Countries>().Where(r => r.Active).ToListAsync();

        }

        public async Task<List<Countries>> GetAll()
        {
            return await this.dbContext.Set<Countries>().ToListAsync();
        }

        public async Task<Countries> GetByCode(string code)
        {
            return await this.dbContext.Set<Countries>()
                .Include(r => r.EconomicIndicators)
                .Where(r => r.CountryCode == code).FirstOrDefaultAsync();
        }

        public async Task<Countries> GetById(int id)
        {
            return await this.dbContext.Set<Countries>()
                .Include(r => r.EconomicIndicators)
                .Where(r => r.CountryId == id).FirstOrDefaultAsync();
        }
    }
}
