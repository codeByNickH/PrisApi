using PrisApi.Services.Scrapers;
using PrisApi.Services;
using PrisApi.Helper.IHelper;
using PrisApi.Helper;
using PrisApi.Data;
using Microsoft.EntityFrameworkCore;
using PrisApi.Repository.IRepository;
using PrisApi.Models.Scraping;
using PrisApi.Repository;
using PrisApi.Models;
using PrisApi.Mapper.IMapper;
using PrisApi.Mapper;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton(_ =>
{
    try
    {
        // Install Playwright browser
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
        if (exitCode != 0)
        {
            throw new Exception($"Playwright installation failed with exit code {exitCode}");
        }
        return Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        throw new Exception("Failed to initialize Playwright", ex);
    }
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found")
));

builder.Services.AddControllers();
builder.Services.AddTransient<WillysScrapeService>();
builder.Services.AddTransient<IcaScrapeService>();
builder.Services.AddScoped<ScraperService>();
builder.Services.AddScoped<IScrapeHelper, ScrapeHelper>();
builder.Services.AddScoped<IRepository<ScraperConfig>, ConfigRepository>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IScrapeConfigHelper, ScrapeConfigHelper>();
builder.Services.AddScoped<IRepository<StoreLocation>, LocationRepository>();
builder.Services.AddScoped<IRepository<Store>, StoreRepository>();
builder.Services.AddScoped<IMapping<Product>, ProductMapping>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder =>
//         {
//             builder.AllowAnyOrigin()
//                    .AllowAnyMethod()
//                    .AllowAnyHeader();
//         });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS if needed
app.UseCors("AllowAll");

// Add this line to enable controller routing
app.MapControllers();

app.Run();