using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL_SqlServer.Dto;
using DAL_SqlServer.Models;
using DAL_SqlServer.SearchModels;

namespace DAL_SqlServer.Repository
{
    public interface IIndicatorDataRepository
    {
        IndicatorData Create(IndicatorData data);
        void Create(List<IndicatorData> data);
        Task<List<IndicatorData>> GetByCurrency(string currency);
        Task<List<IndicatorData>> GetIndicatorsForDate(DateTime dt);
        Task<List<IndicatorData>> GetIndicatorsForCcyAndName(string ccy, string indicatorName);
        List<IndicatorData> BulkUpdate(List<IndicatorData> recs);
        Task<List<vwCountryIndicator>> GetCurrencyIndicators();
        Task<List<vwCountryIndicator>> GetCurrencyIndicatorsByCcy(string currency);
        Task<List<IndicatorData>> GetIndicatorHistory(IndicatorDataSearchModel search);
        Task<List<IndicatorData>> ThisWeek();
        Task<List<IndicatorData>> NextWeek();
        Task<List<IndicatorData>> LastWeek();
        Task<List<string>> CountriesGetAll();

        List<ReleaseDto> IndicatorsGroupByCcyIndicator(string currency);
        List<ReleaseDto> GetIndicatorsGroupByCcyIndicatorName(string currency, string indicatorName);

    }
}
