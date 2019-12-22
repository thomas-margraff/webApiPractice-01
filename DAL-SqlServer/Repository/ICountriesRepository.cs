using DAL_SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL_SqlServer.Repository
{
    public interface ICountriesRepository
    {
        Task<List<Countries>> GetActive(bool? includeIndicators = false);
        Task<List<Countries>> GetAll();
        Task<Countries> GetById(int id);
        Task<Countries> GetByCode(string code);

    }
}
