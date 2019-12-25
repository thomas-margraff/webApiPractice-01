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

    }
}