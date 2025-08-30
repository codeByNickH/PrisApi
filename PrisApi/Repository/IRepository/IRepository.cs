using System.Linq.Expressions;
using PrisApi.Models;
using PrisApi.Models.Scraping;

namespace PrisApi.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetOnFilterAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task<List<T>> GetListOnFilterAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task<T> SaveAsync(T data);

    }
}