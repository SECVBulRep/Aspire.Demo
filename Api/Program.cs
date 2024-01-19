using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddServiceDefaults();


builder.AddRedisDistributedCache("redis", option =>
{
    option.HealthChecks = true;
    option.Tracing = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async (ILogger<Program> logger, IDistributedCache cache) =>
    {
        var cachedForecast = await cache.GetAsync("forecast");

        if (cachedForecast is null)
        {

            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                .ToArray();

            await cache.SetAsync("forecast", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(forecast)), new ()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(10)
            }); ;

            return forecast;
            
        }

        logger.LogInformation("finish");
        
        return JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(cachedForecast);
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}