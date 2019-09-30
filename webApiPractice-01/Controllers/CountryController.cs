using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_SqlServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace webApiPractice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ntpContext _ctx;

        public CountryController(ntpContext ctx)
        {
            this._ctx = ctx;
        }

        public async Task<IEnumerable<Countries>> Get()
        {
            return await _ctx.Countries.ToListAsync();
        }

        [HttpGet("{CountryId:int}")]
        public async Task<Countries> GetById(int CountryId)
        {
            return await _ctx.Countries.FirstOrDefaultAsync(r => r.CountryId == CountryId);
        }


        [HttpGet("{CountryCode}")]
        public async Task<Countries> GetByCode(string CountryCode)
        {
            return await _ctx.Countries.FirstOrDefaultAsync(r => r.CountryCode == CountryCode);
        }

    }
}