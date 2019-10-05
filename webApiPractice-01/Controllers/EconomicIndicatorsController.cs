using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL_SqlServer;
using Microsoft.EntityFrameworkCore;

namespace webApiPractice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EconomicIndicatorsController : ControllerBase
    {
        private readonly ntpContext _ctx;

        public EconomicIndicatorsController(ntpContext ctx)
        {
            this._ctx = ctx;
        }

        [HttpGet()]
        public async Task<IEnumerable<VwIndicatorCountry>>Get()
        {
            var recs = await _ctx.VwIndicatorCountry.ToListAsync();
            return recs;
        }

        [HttpGet("GetByCountry/{id:int}")]
        public async Task<IEnumerable<VwIndicatorCountry>>GetByCountry(int id)
        {
            var recs = await _ctx.VwIndicatorCountry.Where(x => x.CountryId == id).ToListAsync();
            return recs;
        }

        [HttpGet("GetByIndicator/{id:int}")]
        public async Task<IEnumerable<VwIndicatorCountry>> GetByIndicator(int id)
        {
            return await _ctx.VwIndicatorCountry.Where(x => x.IndicatorId == id).ToListAsync();
        }
    }
}