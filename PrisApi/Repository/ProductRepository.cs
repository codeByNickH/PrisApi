using PrisApi.Repository.IRepository;
using PrisApi.Models.Scraping;
using PrisApi.Models;
using PrisApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PrisApi.Mapper.IMapper;

namespace PrisApi.Repository
{
    public class ProductRepository : IRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapping<Product> _mapping;
        public ProductRepository(AppDbContext dbContext, IMapping<Product> mapping)
        {
            _dbContext = dbContext;
            _mapping = mapping;
        }
        public Task<Product> GetOnFilterAsync(Expression<Func<Product, bool>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetListOnFilterAsync(Expression<Func<Product, bool>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public async Task<List<int>> SaveAsync(List<Product> scrapedProducts, int categoryId)
        {
            // var mappedProducts = await _mapping.ToProduct(scrapedProducts); Change so mapping happens here instead?
            // 

            var existingProducts = await _dbContext.Products
                .Where(p => p.StoreId == scrapedProducts.First().StoreId)
                .Select(p => new { p.Id, p.Name, p.ProdCode, p.CreatedAt })
                .ToListAsync();

            var productDict = existingProducts
                .ToDictionary(p => $"{p.Name}_{p.ProdCode}", p => p.Id);

            var toAdd = new List<Product>();
            var toUpdate = new List<Product>();

            foreach (var scrapedProduct in scrapedProducts)
            {
                var key = $"{scrapedProduct.Name}_{scrapedProduct.ProdCode}";

                if (productDict.ContainsKey(key))
                {
                    scrapedProduct.Id = productDict[key];
                    scrapedProduct.UpdatedAt = DateTime.Now;
                    scrapedProduct.CreatedAt = existingProducts.Where(i => i.Id == productDict[key]).Select(t => t.CreatedAt).FirstOrDefault();
                    toUpdate.Add(scrapedProduct);
                }
                else
                {
                    scrapedProduct.CreatedAt = DateTime.Now;
                    toAdd.Add(scrapedProduct);
                }
            }

            if (toAdd.Any())
            {
                await _dbContext.Products.AddRangeAsync(toAdd);
            }
            if (toUpdate.Any())
            {
                _dbContext.Products.UpdateRange(toUpdate);
            }

            await _dbContext.SaveChangesAsync();

            var priceHistories = new List<PriceHistory>();

            foreach (var product in scrapedProducts)
            {
                priceHistories.Add(new PriceHistory
                {
                    ProductId = product.Id,
                    Price = product.CurrentPrice,
                    ComparePrice = product.CurrentComparePrice,
                    WasDiscount = product.WasDiscount,
                    CompareUnit = product.Unit,
                    RecordedAt = DateTime.Now,
                });
            }

            var existingCategoty = await _dbContext.CategoryLists
                .Where(c => c.StoreId == scrapedProducts.First().StoreId)
                .ToListAsync();

            var categoryDict = existingCategoty
                .ToDictionary(c => $"{c.ProductId}_{c.CategoryId}", c => c.Id);
            var categories = new List<CategoryList>();

            foreach (var product in scrapedProducts)
            {
                var key = $"{product.Id}_{categoryId}";
                if (!categoryDict.ContainsKey(key))
                {
                    categories.Add(new CategoryList
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId,
                        StoreId = product.StoreId
                    });
                }
            }
            if (categories.Any())
            {
                await _dbContext.CategoryLists.AddRangeAsync(categories);
            }
            
            await _dbContext.PriceHistories.AddRangeAsync(priceHistories);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine($"{toAdd.Count} ----- {toUpdate.Count} ----- {priceHistories.Count} ----- {categories.Count}");
            var job = new List<int>
            {
                toAdd.Count,
                toUpdate.Count
            };
            return await Task.FromResult(job);
        }
        public Task<Product> SaveAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }

}