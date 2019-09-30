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
    public class NtpController : ControllerBase
    {
        private readonly ntpContext _ctx;

        public NtpController(ntpContext ctx)
        {
            this._ctx = ctx;
        }

        public async Task<IEnumerable<VwIndicatorCountry>> Get()
        {
            return await _ctx.VwIndicatorCountry.ToListAsync();
        }

        //[HttpGet("{countrylist}")]
        //public async Task<IEnumerable<Countries>> GountryList()
        //{
        //    return await _ctx.Countries.ToListAsync();
        //}

        [HttpGet("{indicatorlist}")]
        public async Task<IEnumerable<EconomicIndicators>> IndicatorsList()
        {
            return await _ctx.EconomicIndicators.ToListAsync();
        }
    }
}