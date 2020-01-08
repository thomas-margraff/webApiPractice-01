using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_SqlServer;
using DAL_SqlServer.Dto;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
using DAL_SqlServer.SearchModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ntpWebApi.Controllers
{
    [Route("api/[controller]")]
    public class IndicatorDataController : Controller
    {
        private readonly ntpContext _ctx;
        private readonly IIndicatorDataRepository _repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx = ntpContext"></param>
        /// <param name="repository = data repository"></param>
        public IndicatorDataController(ntpContext ctx, IIndicatorDataRepository repository)
        {
            this._ctx = ctx;
            this._repository = repository;
        }

        /// <summary>
        /// Get a list of all Countries
        /// </summary>
        /// <returns>list of countries</returns>
        [HttpGet("CountriesGetAll")]
        public async Task<IEnumerable<string>> CountriesGetAll()
        {
            return await this._repository.CountriesGetAll();
        }

        /// <summary>
        /// Top 300 IndicatorData records (sorted by ReleaseDateTime descending)
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        [HttpGet("GetByCountry/{country}")]
        public async Task<IEnumerable<IndicatorData>> GetByCountry(string country)
        {
            return await this._repository.GetByCurrency(country);
        }

        /// <summary>
        /// get all indicators for all countries
        /// </summary>
        /// <returns></returns>
        [HttpGet("CountryIndicatorsGetAll/")]
        public async Task<IEnumerable<vwCountryIndicator>> CountryIndicatorsGetAll()
        {
            return await this._repository.GetCurrencyIndicators();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        [HttpGet("CountryIndicatorsByCountry/{country}")]
        public async Task<IEnumerable<vwCountryIndicator>> CountryIndicatorsByCountry(string country)
        {
            return await this._repository.GetCurrencyIndicatorsByCcy(country);
        }

        [HttpGet("GetIndicatorsForDate/{dt}")]
        public async Task<IActionResult> GetIndicatorsForDate(DateTime dt)
        {
            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            var recs = await this._repository.GetIndicatorsForDate(dt);
            if (recs.Count() == 0)
            {
                return this.NotFound("no indicators for the requested date.");
            }
            return Ok(recs);
        }

        [HttpGet("GetIndicatorsForToday/")]
        public async Task<IActionResult> GetIndicatorsForToday()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var recs = await this._repository.GetIndicatorsForDate(dt);
            if (recs.Count() == 0)
            {
                return this.NotFound("no indicators for the requested date.");
            }
            return Ok(recs);
        }

        [HttpGet("thisweek/")]
        public async Task<IEnumerable<IndicatorData>> ThisWeek()
        {
            return await this._repository.ThisWeek();
        }

        [HttpGet("nextweek/")]
        public async Task<IEnumerable<IndicatorData>> NextWeek()
        {
            return await this._repository.NextWeek();
        }

        [HttpPost("GetIndicatorHistory/search")]
        public async Task<IEnumerable<IndicatorData>> GetIndicatorHistory(IndicatorDataSearchModel search)
        {
            var recs = await this._repository.GetIndicatorHistory(search);
            return recs;
        }

        

        [HttpGet("GroupByCcyIndicators/{currency}")]
        public List<ReleaseDto> GroupByCcyIndicators(string currency)
        {
            var recs = this._repository.IndicatorsGroupByCcyIndicator(currency);
            // var json = JsonConvert.SerializeObject(recs, Formatting.Indented);

            return recs;
        }

        [HttpGet("GetIndicatorsGroupByCcyIndicatorName/{currency}/{indicatorName}")]
        public List<ReleaseDto> GetIndicatorsGroupByCcyIndicatorName(string currency, string indicatorName)
        {
            var recs = this._repository.GetIndicatorsGroupByCcyIndicatorName(currency, indicatorName);
            // var json = JsonConvert.SerializeObject(recs, Formatting.Indented);

            return recs;
        }

    }
}
