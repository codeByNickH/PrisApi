using PrisApi.Models.Scraping;

namespace PrisApi.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> SaveProductAsync();
    }
}