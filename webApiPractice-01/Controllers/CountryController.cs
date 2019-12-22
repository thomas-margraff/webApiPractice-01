using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace webApiPractice_01.Controllers
{
    // comment

    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ntpContext _ctx;
        private readonly ICountriesRepository _repository;

        public CountryController(ntpContext ctx, ICountriesRepository repository)
        {
            this._ctx = ctx;
            this._repository = repository;
        }

        //[HttpGet("GetByStatus/{active:bool?}")]
        //public async Task<IEnumerable<Countries>> GetByStatus(bool? active)
        //{
        //    //return await _ctx.Countries.ToListAsync();
        //    if (active == null)
        //    {
        //        return await this.GetAll();
        //    }
        //    if (active.Value == false)
        //    {
        //        return await this.GetInActive();
        //    }

        //    return await this.GetActive();

        //}

        [HttpGet("GetAll")]
        public async Task<IEnumerable<Countries>> GetAll()
        {
            return await this._repository.GetAll();
        }

        [HttpGet("GetActive/{includeIndicators:bool?}")]
        public async Task<IEnumerable<Countries>> GetActive(bool? includeIndicators=false)
        {
            var countries = await this._repository.GetActive(includeIndicators);

            if (includeIndicators.Value)
            {
                return countries.ClearForJson();
            }
            return countries;
        }

        //[HttpGet("GetInActive")]
        //public async Task<IEnumerable<Countries>> GetInActive()
        //{
        //    return await _ctx.Countries.Where(r => !r.Active).ToListAsync();
        //}

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rec = await this._repository.GetById(id); 
            if (rec == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(rec);
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var rec = await this._repository.GetByCode(code);
            if (rec == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(rec.ClearForJson());
        }

    }
}