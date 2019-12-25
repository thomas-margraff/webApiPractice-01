using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL_SqlServer.Models;

namespace DAL_SqlServer.Repository
{
    public interface IIndicatorDataRepository
    {
        IndicatorData Create(IndicatorData data);
        void Create(List<IndicatorData> data);
        Task<List<IndicatorData>> GetByCurrency(string currency);
        List<IndicatorData> BulkUpdate(List<IndicatorData> recs);
        Task<List<vwCountryIndicator>> GetCurrencyIndicators();
        Task<List<vwCountryIndicator>> GetCurrencyIndicatorsByCcy(string currency);

    }
}
