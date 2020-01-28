using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_SqlServer;
using DAL_SqlServer.Dto;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace webApiPractice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorDataController : ControllerBase
    {
        private readonly ntpContext _ctx;
        private readonly IIndicatorDataRepository _repository;

        public IndicatorDataController(ntpContext ctx, IIndicatorDataRepository repository)
        {
            this._ctx = ctx;
            this._repository = repository;
        }
        /// <summary>
        /// get all
        /// </summary>
        /// <returns></returns>
        [HttpGet("CountriesGetAll")]
        // Task<ActionResult<IEnumerable<AppProfile>>>
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
        public async Task<IEnumerable<string>> CountriesGetAll()
        {
            try
            {
                return await this._repository.CountriesGetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// by ccy
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        [HttpGet("GetByCurrency/{currency}")]
        public async Task<IEnumerable<IndicatorData>> GetByCurrency(string currency)
        {
            return await this._repository.GetByCurrency(currency);
        }

        [HttpGet("GetCurrencyIndicators/")]
        public async Task<IEnumerable<vwCountryIndicator>> GetCurrencyIndicators()
        {
            return await this._repository.GetCurrencyIndicators();
        }

        [HttpGet("GetCurrencyIndicatorsByCcy/{currency}")]
        public async Task<IEnumerable<vwCountryIndicator>> GetCurrencyIndicatorsByCcy(string currency)
        {
            return await this._repository.GetCurrencyIndicatorsByCcy(currency);
        }

        [HttpGet("GetIndicatorsForToday/")]
        public async Task<IEnumerable<IndicatorData>> GetIndicatorsForToday()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return await this._repository.GetIndicatorsForDate(dt);
        }


        [HttpGet("GetIndicatorsForDate/{dt}")]
        public async Task<IEnumerable<IndicatorData>> GetIndicatorsForDate(DateTime dt)
        {
            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            return await this._repository.GetIndicatorsForDate(dt);
        }

        [HttpGet("thisweek/")]
        public async Task<IEnumerable<IndicatorData>> ThisWeek()
        {
            return await this._repository.ThisWeek();
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