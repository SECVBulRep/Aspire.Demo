using Microsoft.EntityFrameworkCore;
using Web.Clients;
using Web.Components;
using Web.Db;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationContext>("postgresdb", null,
    optionsBuilder => optionsBuilder.UseNpgsql());

builder.AddRedisOutputCache("redis", option =>
{
    option.HealthChecks = true;
    option.Tracing = true;
});

builder.Services.AddHttpClient<WeatherApiClient>(opt => { opt.BaseAddress = new Uri("http://apiservice/"); });


var app = builder.Build();

app.UseOutputCache();
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();