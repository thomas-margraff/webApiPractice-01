using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


    }
}