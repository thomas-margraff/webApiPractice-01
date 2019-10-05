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
    // comment

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ntpContext _ctx;

        public CountryController(ntpContext ctx)
        {
            this._ctx = ctx;
        }

        [HttpGet("GetByStatus/{active:bool?}")]
        public async Task<IEnumerable<Countries>> GetByStatus(bool? active)
        {
            //return await _ctx.Countries.ToListAsync();
            if (active == null)
            {
                return await this.GetAll();
            }
            if (active.Value == false)
            {
                return await this.GetInActive();
            }

            return await this.GetActive();

        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<Countries>> GetAll()
        {
            return await _ctx.Countries.ToListAsync();
        }

        [HttpGet("GetActive")]
        public async Task<IEnumerable<Countries>> GetActive()
        {
            return await _ctx.Countries.Where(r => r.Active).ToListAsync();
        }

        [HttpGet("GetInActive")]
        public async Task<IEnumerable<Countries>> GetInActive()
        {
            return await _ctx.Countries.Where(r => !r.Active).ToListAsync();
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rec = await _ctx.Countries.FirstOrDefaultAsync(r => r.CountryId == id);
            if (rec == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(rec);
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var rec = await _ctx.Countries.FirstOrDefaultAsync(r => r.CountryCode == code);
            if (rec == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(rec);
        }

    }
}