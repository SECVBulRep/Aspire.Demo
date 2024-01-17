namespace Web.Clients;

public class WeatherApiClient(HttpClient client)
{
    public async Task<WeatherForecast[]> GetForecast() =>
        await client.GetFromJsonAsync<WeatherForecast[]>("weatherforecast") ?? [];
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}