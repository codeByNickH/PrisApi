using PrisApi.Models;
using PrisApi.Models.Scraping;

namespace PrisApi.Mapper.IMapper
{
    public interface IMapping<T> where T : class
    {
        Task<T> ToProduct(ScrapedProduct scraped);
        Task<List<T>> ToProduct(List<ScrapedProduct> scraped);
    }
}