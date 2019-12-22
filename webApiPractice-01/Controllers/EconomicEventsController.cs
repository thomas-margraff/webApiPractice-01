using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EconomicEventsController : ControllerBase
    {
        private readonly ntpContext _ctx;

        public EconomicEventsController(ntpContext ctx)
        {
            this._ctx = ctx;
        }

        public async Task<IEnumerable<VwEconomicIndicators>>Get()
        {
            var rec = await _ctx.VwEconomicIndicators.Where(r => r.CountryId == 9).ToListAsync();
            return rec.Take(100);
        }

    }
}