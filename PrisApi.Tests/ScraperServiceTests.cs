using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using PrisApi.Mapper.IMapper;
using PrisApi.Models;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services;
using PrisApi.Helper.IHelper;
using PrisApi.Services.Scrapers;
using Xunit;

namespace PrisApi.Tests
{
    public class ScraperServiceTests
    {
        private class NoDelayScraperService : ScraperService
        {
            public NoDelayScraperService(NullLogger<ScraperService> logger, IScrapeConfigHelper configHelper, IProductRepository repository, IMapping<Product> mapping,
                WillysScrapeService willysScrapeService, IcaScrapeService icaScrapeService, CoopScrapeService coopScrapeService, CitygrossScrapeService citygrossScrapeService)
                : base(logger, configHelper, repository, mapping, willysScrapeService, icaScrapeService, coopScrapeService, citygrossScrapeService)
            {
            }

            protected override Task DelayBetweenRequests(int milliseconds = 10000)
            {
                return Task.CompletedTask;
            }
        }

        private class FakeMapping : IMapping<Product>
        {
            public Task<Product> ToProduct(ScrapedProduct scraped)
            {
                var p = new Product { Id = 1, Name = scraped.RawName };
                return Task.FromResult(p);
            }

            public Task<List<Product>> ToProduct(List<ScrapedProduct> scraped)
            {
                var list = new List<Product>();
                foreach (var s in scraped)
                {
                    list.Add(new Product { Id = 1, Name = s.RawName });
                }
                return Task.FromResult(list);
            }
        }

        private class FakeRepository : IProductRepository
        {
            private readonly List<int> _return;
            public FakeRepository(List<int> ret)
            {
                _return = ret;
            }
            public Task<List<int>> SaveAsync(List<Product> data, int categoryId)
            {
                return Task.FromResult(_return);
            }
        }

        private class ReturningWillysScraperService : NoDelayScraperService
        {
            private readonly List<ScrapedProduct> _products;
            public ReturningWillysScraperService(List<ScrapedProduct> products, IProductRepository repo, IMapping<Product> mapping)
                : base(new NullLogger<ScraperService>(), null!, repo, mapping, null!, null!, null!, null!)
            {
                _products = products;
            }

            protected override Task<List<ScrapedProduct>> ScrapeWillys(string navigation, Store storeConfig)
            {
                return Task.FromResult(_products);
            }
        }

        private class ThrowingWillysScraperService : NoDelayScraperService
        {
            public ThrowingWillysScraperService(IProductRepository repo, IMapping<Product> mapping)
                : base(new NullLogger<ScraperService>(), null!, repo, mapping, null!, null!, null!, null!)
            {
            }

            protected override Task<List<ScrapedProduct>> ScrapeWillys(string navigation, Store storeConfig)
            {
                throw new Exception("scrape failed");
            }
        }

        [Fact]
        public async Task ScrapeWillys_Throws_OnEmptyNavigation()
        {
            var svc = new NoDelayScraperService(new NullLogger<ScraperService>(), null!, null!, null!, null!, null!, null!, null!);

            await Assert.ThrowsAsync<ArgumentException>(() => svc.ScrapeAsync("", new Store { Name = "x" }, 1));
            await Assert.ThrowsAsync<ArgumentException>(() => svc.ScrapeAsync("   ", new Store { Name = "x" }, 1));
        }

        [Fact]
        public async Task ScrapeWillys_Throws_OnNullStore()
        {
            var svc = new NoDelayScraperService(new NullLogger<ScraperService>(), null!, null!, null!, null!, null!, null!, null!);

            await Assert.ThrowsAsync<ArgumentNullException>(() => svc.ScrapeAsync("/nav", null!, 1));
        }

        [Fact]
        public async Task ScrapeWillys_ReturnsFailedJob_WhenScrapeThrows()
        {
            var mapping = new FakeMapping();
            var repo = new FakeRepository(new List<int> { 0, 0 });

            var svc = new ThrowingWillysScraperService(repo, mapping);

            var job = await svc.ScrapeAsync("/nav", new Store { Name = "Willys" }, 1);

            Assert.False(job.Success);
            Assert.Contains("scrape failed", job.ErrorMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ScrapeWillys_Succeeds_WhenServicesWork()
        {
            var scraped = new List<ScrapedProduct>
            {
                new ScrapedProduct { RawName = "p1" },
                new ScrapedProduct { RawName = "p2" }
            };

            var mapping = new FakeMapping();
            var repo = new FakeRepository(new List<int> { 2, 0 });


            var svc = new ReturningWillysScraperService(scraped, repo, mapping);

            var job = await svc.ScrapeAsync("/nav", new Store { Name = "Willys" }, 5);

            Assert.True(job.Success);
            Assert.Equal(2, job.ProductsScraped);
            Assert.Equal(2, job.NewProducts);
            Assert.Equal(0, job.UpdatedProducts);
        }
    }
}
