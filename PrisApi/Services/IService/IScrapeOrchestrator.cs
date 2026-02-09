using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;

namespace PrisApi.Services.IService
{
    public interface IScrapeOrchestrator
    {
        Task<APIResponse> OrchestrateAsync(ScrapeRequest request);
    }
}