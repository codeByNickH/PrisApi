using PrisApi.Repository.IRepository;
using PrisApi.Models.Scraping;
using PrisApi.Models;
using PrisApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PrisApi.Mapper.IMapper;
using System.Text.RegularExpressions;
using PrisApi.Services;
using PrisApi.Services.IService;

namespace PrisApi.Repository
{
    public class ProductRepository : IRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IDiscordService _discordService;
        public ProductRepository(AppDbContext dbContext, IDiscordService discordService)
        {
            _dbContext = dbContext;
            _discordService = discordService;
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
            var existingProducts = await _dbContext.Products
                .Where(p => p.StoreId == scrapedProducts.First().StoreId)
                .Select(p => new 
                { 
                    p.Id,
                    p.Name,
                    p.ProdCode,
                    p.CreatedAt,
                    p.CurrentPrice,
                    p.CurrentComparePrice,
                })
                .ToListAsync();

            var store = await _dbContext.Stores
                .Where(s => s.Id == scrapedProducts.First().StoreId)
                .Select(s => new { s.Name, s.StoreLocation.City }).FirstOrDefaultAsync();

            var storeName = store?.Name ?? "Unknown Store";

            var productDict = existingProducts
                .ToDictionary(p => $"{p.Name}_{p.ProdCode}", p => p);

            var toAdd = new List<Product>();
            var toUpdate = new List<Product>();
            var priceChangeForDiscord = new List<ProductPriceChange>();
            var targetProducts = new List<string>()
            {
                $@"\bnötfärs\b",
                $@"\blövbiff\b",
                $@"\bkycklingfilé\b",
                $@"\bkycklingfärs\b",
                $@"\bsmör\b",
                $@"\bcocoa dark\b",
            };

            foreach (var scrapedProduct in scrapedProducts)
            {
                var key = $"{scrapedProduct.Name}_{scrapedProduct.ProdCode}";

                if (productDict.TryGetValue(key, out var existingProduct))
                {
                    if (scrapedProduct.CurrentPrice != existingProduct.CurrentPrice &&
                        targetProducts.Any(keyword =>
                            Regex.IsMatch(scrapedProduct.Name, keyword, RegexOptions.IgnoreCase)))
                    {
                        priceChangeForDiscord.Add(new ProductPriceChange
                        {
                            StoreName = storeName,
                            ProductName = scrapedProduct.Name,
                            Size = scrapedProduct?.Size,
                            Unit = scrapedProduct?.Unit,
                            NewPrice = scrapedProduct.CurrentPrice,
                            NewComparePrice = scrapedProduct.CurrentComparePrice,
                            OldPrice = existingProduct.CurrentPrice,
                            OldComparePrice = existingProduct.CurrentComparePrice,

                        });
                    }

                    scrapedProduct.Id = existingProduct.Id;
                    scrapedProduct.UpdatedAt = DateTime.Now;
                    scrapedProduct.CreatedAt = existingProduct.CreatedAt;
                    toUpdate.Add(scrapedProduct);
                }
                else
                {
                    // Product matches keyword but does not exist in Db
                    if (targetProducts.Any(keyword => Regex.IsMatch(scrapedProduct.Name, keyword, RegexOptions.IgnoreCase)))
                    {
                        priceChangeForDiscord.Add(new ProductPriceChange
                        {
                            StoreName = storeName,
                            ProductName = scrapedProduct.Name,
                            Size = scrapedProduct?.Size,
                            Unit = scrapedProduct?.Unit,
                            NewPrice = scrapedProduct.CurrentPrice,
                            NewComparePrice = scrapedProduct.CurrentComparePrice,
                            OldPrice = null,
                            OldComparePrice = null,

                        });
                    }
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
            await _discordService.SendToDiscordAsync(priceChangeForDiscord);
            return await Task.FromResult(job);
        }
        public Task<Product> SaveAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }

}