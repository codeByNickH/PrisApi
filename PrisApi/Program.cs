using PrisApi.Services.Scrapers;
using PrisApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// Initialize Playwright with proper error handling
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

builder.Services.AddControllers();
builder.Services.AddTransient<WillysScraper>();
builder.Services.AddScoped<ScraperService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

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