using DAL_SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL_SqlServer.Repository
{
    public interface IRepository
    {
        Task<List<T>> GetActive<T>() where T : class;
    }
}
