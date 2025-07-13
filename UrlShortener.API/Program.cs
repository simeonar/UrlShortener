var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<UrlShortener.Core.Repositories.IClickStatisticRepository, UrlShortener.Infrastructure.Repositories.InMemoryClickStatisticRepository>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UrlShortener.Core.Services.IQRCodeCache, UrlShortener.Infrastructure.Services.InMemoryQRCodeCache>();
builder.Services.AddScoped<UrlShortener.Core.Services.IShortCodeGenerator, UrlShortener.Core.Services.ShortCodeGenerator>();
builder.Services.AddScoped<UrlShortener.Core.Services.IShortCodeUniquenessChecker, UrlShortener.Infrastructure.Repositories.EfShortCodeUniquenessChecker>();


// Ensure the data directory and file exist
var dataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "data");
var filePath = Path.Combine(dataDir, "shortened_urls.json");
var usersFilePath = Path.Combine(dataDir, "users.json");
if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}
if (!File.Exists(filePath))
{
    File.WriteAllText(filePath, "[]");
}

if (!File.Exists(usersFilePath))
{
    File.WriteAllText(usersFilePath, "[]");
}

builder.Services.AddSingleton<UrlShortener.Core.Repositories.IShortenedUrlRepository>(
    provider => new UrlShortener.Infrastructure.Repositories.FileShortenedUrlRepository(filePath)
);

builder.Services.AddSingleton<UrlShortener.Core.Repositories.IUserRepository>(
    provider => new UrlShortener.Infrastructure.Repositories.FileUserRepository(usersFilePath)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


// Rate limiting middleware (must be before Authorization)
app.UseMiddleware<UrlShortener.API.Middleware.RateLimitingMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
