using PrisApi.Data;
using PrisApi.Helper;
using PrisApi.Helper.IHelper;
using PrisApi.Models;
using PrisApi.Models.Responses;
using PrisApi.Models.Scraping;
using PrisApi.Repository.IRepository;
using PrisApi.Services.IService;

namespace PrisApi.Services
{
    public class ScrapeOrchestrator : IScrapeOrchestrator
    {
        private readonly IScrapeConfigHelper _configHelper;
        private readonly IRepository<Store> _locationRepository;
        private readonly ScraperService _scraperService;
        private readonly AppDbContext _dbContext;
        private readonly IDiscordService _discordService;
        private readonly ILogger<ScrapeOrchestrator> _logger;

        public ScrapeOrchestrator(
            IScrapeConfigHelper configHelper,
            IRepository<Store> locationRepository,
            ScraperService scraperService,
            AppDbContext dbContext,
            IDiscordService discordService,
            ILogger<ScrapeOrchestrator> logger)
        {
            _configHelper = configHelper;
            _locationRepository = locationRepository;
            _scraperService = scraperService;
            _dbContext = dbContext;
            _discordService = discordService;
            _logger = logger;
        }

        public async Task<APIResponse> OrchestrateAsync(ScrapeRequest request)
        {
            _logger.LogInformation($"Scrape of {request.ActionName} initiated");

            var config = await _configHelper.GetConfig(request.ConfigId);

            var allLocations = await _locationRepository.GetListOnFilterAsync(l => l.Name == config.StoreName);

            var filteredLocations = allLocations
                .Where(loc => request.AllowedCities.Contains(loc.StoreLocation.City))
                // .Where(loc => request.AllowedDistricts.Length == 0
                //     || request.AllowedDistricts.Contains(loc.StoreLocation.District))
                .ToList();

            var jobList = new List<ScrapingJob>();
            foreach (var loc in filteredLocations)
            {
                var job = await _scraperService.ScrapeAsync(request.NavigationPath, loc, request.CategoryId);
                job.StoreLocation = $"{loc.StoreLocation.City}, {loc.StoreLocation.District}";
                jobList.Add(job);

                await _dbContext.ScrapingJobs.AddAsync(job);
                await _dbContext.SaveChangesAsync();
            }

            var response = ResponseHelper.CreateApiResponse(jobList);

            if (!response.IsSuccess)
            {
                await _discordService.SendErrorToDiscordAsync(jobList);
            }

            return response;
        }
    }
}