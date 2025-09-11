using PrisApi.Mapper.IMapper;
using PrisApi.Models;
using PrisApi.Models.Scraping;

namespace PrisApi.Mapper
{
    public class ProductMapping : IMapping<Product>
    {
        public async Task<List<Product>> ToProduct(List<ScrapedProduct> scraped)
        {
            var mappedProducts = new List<Product>();

            foreach (var item in scraped)
            {
                if (item.HasDiscount)
                {
                    mappedProducts.Add(new Product
                    {
                        Name = item.RawName,
                        Brand = item.RawBrand,
                        CountryOfOrigin = item.CountryOfOrigin,
                        DiscountPercentage = Math.Round(((item.RawOrdPrice - item.RawDiscountPrice) / item.RawOrdPrice) * 100, 2),
                        ProdCode = item.ProdCode,
                        CurrentPrice = item.RawDiscountPrice,
                        CurrentComparePrice = item.DiscountJmfPrice,
                        OriginalPrice = item.RawOrdPrice,
                        ComparePrice = item.OrdJmfPrice,
                        // CategoryId = item.CategoryId,
                        Size = item.Size,
                        StoreId = item.StoreId,
                        MaxQuantity = item.MaxQuantity,
                        MinQuantity = item.MinQuantity,
                        MemberDiscount = item.MemberDiscount,
                        WasDiscount = item.HasDiscount,
                        Unit = item.RawUnit,
                    });
                }
                else
                {
                    mappedProducts.Add(new Product
                    {
                        Name = item.RawName,
                        Brand = item.RawBrand,
                        CountryOfOrigin = item.CountryOfOrigin,
                        ProdCode = item.ProdCode,
                        CurrentPrice = item.RawOrdPrice,
                        CurrentComparePrice = item.OrdJmfPrice,
                        // CategoryId = item.CategoryId,
                        Size = item.Size,
                        StoreId = item.StoreId,
                        MaxQuantity = item.MaxQuantity,
                        MinQuantity = item.MinQuantity,
                        MemberDiscount = item.MemberDiscount,
                        WasDiscount = item.HasDiscount,
                        Unit = item.RawUnit,
                    });
                }
            }
            return await Task.FromResult(mappedProducts);
        }
        public Task<Product> ToProduct(ScrapedProduct scraped)
        {
            if (scraped.RawDiscountPrice > 0 && scraped.RawDiscountPrice != scraped.RawOrdPrice)
            {
                return Task.FromResult(new Product
                {
                    Name = scraped.RawName,
                    Brand = scraped.RawBrand,
                    CountryOfOrigin = scraped.CountryOfOrigin,
                    ProdCode = scraped.ProdCode,
                    CurrentPrice = scraped.RawDiscountPrice,
                    OriginalPrice = scraped.RawOrdPrice,
                    ComparePrice = scraped.OrdJmfPrice,
                    CurrentComparePrice = scraped.DiscountJmfPrice,
                    DiscountPercentage = Math.Round(((scraped.RawOrdPrice - scraped.RawDiscountPrice) / scraped.RawOrdPrice) * 100, 2),
                    // CategoryId = scraped.CategoryId,
                    Size = scraped.Size,
                    StoreId = scraped.StoreId,
                    MaxQuantity = scraped.MaxQuantity,
                    MinQuantity = scraped.MinQuantity,
                    MemberDiscount = scraped.MemberDiscount,
                    WasDiscount = true,
                    Unit = scraped.RawUnit,
                });
            }
            else
            {
                return Task.FromResult(new Product
                {
                    Name = scraped.RawName,
                    Brand = scraped.RawBrand,
                    ProdCode = scraped.ProdCode,
                    CountryOfOrigin = scraped.CountryOfOrigin,
                    CurrentPrice = scraped.RawOrdPrice,
                    CurrentComparePrice = scraped.OrdJmfPrice,
                    // CategoryId = scraped.CategoryId,
                    Size = scraped.Size,
                    StoreId = scraped.StoreId,
                    MaxQuantity = scraped.MaxQuantity,
                    MinQuantity = scraped.MinQuantity,
                    Unit = scraped.RawUnit,
                    MemberDiscount = scraped.MemberDiscount,
                    WasDiscount = scraped.HasDiscount,
                });
            }
        }
    }
}