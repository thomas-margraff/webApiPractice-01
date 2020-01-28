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
/*
 * ActionResult<T> type:
    ASP.NET Core 2.1 adds new programming conventions that make it easier to build clean and descriptive web APIs. 
    ActionResult<T> is a new type added to allow an app to return either a response type or any other action result 
    (similar to IActionResult), while still indicating the response type.

    ActionResult<T> is more specific to Web APIs in ASP.NET Core >= 2.1 and ActionResult<T> offers the following 
    benefits over the IActionResult type:

    The [ProducesResponseType] attribute's Type property can be excluded. 
    For example, [ProducesResponseType(200, Type = typeof(Product))] is simplified to [ProducesResponseType(200)]. 
    The action's expected return type is instead inferred from the T in ActionResult<T>.
    Implicit cast operators support the conversion of both T and ActionResult to ActionResult<T>. 
    T converts to ObjectResult, which means return new ObjectResult(T); is simplified to return T;.
    For more details: Controller action return types in ASP.NET Core Web API
 * 
 * 
 * 
 * return types
 * https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1
*/
namespace ntpWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorDataController : ControllerBase
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

        [HttpGet("CountriesGetAllAsync")]
        public async Task<ActionResult<IEnumerable<string>>> CountriesGetAllAsync()
        {
            try
            {
                return Ok(await this._repository.CountriesGetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get a list of all Countries
        /// </summary>
        /// <returns>list of countries</returns>
        [HttpGet("CountriesGetAll")]
        public async Task<IEnumerable<string>> CountriesGetAll()
        {
            try
            {
                return await this._repository.CountriesGetAll();
            }
            catch (Exception ex)
            {
                throw ex;
                //return new List<string>()
                //{
                //    "broken!",
                //    ex.Message
                //};
            }
            
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

        [HttpGet("lastweek/")]
        public async Task<IEnumerable<IndicatorData>> LastWeek()
        {
            return await this._repository.LastWeek();
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

        [HttpGet("GetIndicatorsForCcyAndName/{currency}/{indicatorName}")]
        public async Task<List<IndicatorData>> GetIndicatorsForCcyAndName(string currency, string indicatorName)
        {
            string ind = Uri.UnescapeDataString(indicatorName);
            return await this._repository.GetIndicatorsForCcyAndName(currency, ind);
        }

        [HttpGet("GetConfig/{name}")]
        public async Task<Configuration> GetConfig(string name)
        {
            return await this._repository.GetConfig(name);
        }


        [HttpGet("test")]
        public string Test()
        {
            return "ok!";
        }

    }
}
