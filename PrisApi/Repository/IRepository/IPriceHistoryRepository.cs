using PrisApi.Models;

namespace PrisApi.Repository.IRepository
{
    public interface IPriceHistoryRepository
    {
        Task<PriceHistory> SaveAsync(List<PriceHistory> data);
    }
}