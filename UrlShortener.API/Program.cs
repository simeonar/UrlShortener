var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UrlShortener.Core.Services.IQRCodeCache, UrlShortener.Infrastructure.Services.InMemoryQRCodeCache>();
builder.Services.AddScoped<UrlShortener.Core.Services.IShortCodeGenerator, UrlShortener.Core.Services.ShortCodeGenerator>();
builder.Services.AddScoped<UrlShortener.Core.Services.IShortCodeUniquenessChecker, UrlShortener.Infrastructure.Repositories.EfShortCodeUniquenessChecker>();
builder.Services.AddScoped<UrlShortener.Core.Repositories.IShortenedUrlRepository, UrlShortener.Infrastructure.Repositories.InMemoryShortenedUrlRepository>();


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

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
