using PrisApi.Models;
using PrisApi.Models.Scraping;

namespace PrisApi.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<List<int>> SaveAsync(List<Product> data, int categoryId);
    }
}