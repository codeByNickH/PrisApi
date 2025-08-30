using System.Linq.Expressions;
using PrisApi.Models;

namespace PrisApi.Repository.IRepository
{
    public class PriceHistoryRepository : IRepository<PriceHistory>, IPriceHistoryRepository
    {
        public Task<List<PriceHistory>> GetListOnFilterAsync(Expression<Func<PriceHistory, bool>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<PriceHistory> GetOnFilterAsync(Expression<Func<PriceHistory, bool>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<PriceHistory> SaveAsync(PriceHistory data)
        {
            throw new NotImplementedException();
        }

        public Task<PriceHistory> SaveAsync(List<PriceHistory> data)
        {
            throw new NotImplementedException();
        }
    }
}