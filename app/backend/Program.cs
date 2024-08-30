// Copyright (c) Microsoft. All rights reserved.

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MinimalApi.Hubs;
// Replace 'YourNamespace' with the actual namespace of the 'GridEventsHub' class


var webAppOptions = new WebApplicationOptions()
{
    Args = args,
    EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
    ApplicationName = "MinimalApi",
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "app/frontend/wwwroot")
};

var builder = WebApplication.CreateBuilder(webAppOptions);

//builder.Configuration.ConfigureAzureKeyVault();

// See: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.AddCrossOriginResourceSharing();
builder.Services.AddAzureServices();
builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN-HEADER"; options.FormFieldName = "X-CSRF-TOKEN-FORM"; });
builder.Services.AddHttpClient();

//builder.WebHost.UseWebRoot(String.Format("{0}/{1}/wwwroot", builder.Configuration["SERVICE_WEBROOT_PATH"], "frontend"));
//builder.WebHost.UseWebRoot("/app/frontend/wwwroot");

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    static string? GetEnvVar(string key) => Environment.GetEnvironmentVariable(key);

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        var name = builder.Configuration["AzureRedisCacheName"] +
            ".redis.cache.windows.net";
        var key = builder.Configuration["AzureRedisCachePrimaryKey"];
        var ssl = "true";


        if (GetEnvVar("REDIS_HOST") is string redisHost)
        {
            name = $"{redisHost}:{GetEnvVar("REDIS_PORT")}";
            key = GetEnvVar("REDIS_PASSWORD");
            ssl = "false";
        }

        if (GetEnvVar("AZURE_REDIS_HOST") is string azureRedisHost)
        {
            name = $"{azureRedisHost}:{GetEnvVar("AZURE_REDIS_PORT")}";
            key = GetEnvVar("AZURE_REDIS_PASSWORD");
            ssl = "false";
        }

        options.Configuration = $"""
            {name},abortConnect=false,ssl={ssl},allowAdmin=true,password={key}
            """;
        options.InstanceName = "content";


    });

    // set application telemetry
    if (GetEnvVar("APPLICATIONINSIGHTS_CONNECTION_STRING") is string appInsightsConnectionString && !string.IsNullOrEmpty(appInsightsConnectionString))
    {
        builder.Services.AddApplicationInsightsTelemetry((option) =>
        {
            option.ConnectionString = appInsightsConnectionString;
        });
    }
}

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:5002")
    });

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseOutputCache();
app.UseRouting();
app.UseStaticFiles();
app.UseCors();
app.UseBlazorFrameworkFiles();
app.UseAntiforgery();
app.MapRazorPages();
app.MapControllers();

app.Use(next => context =>
{
    var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens?.RequestToken ?? string.Empty, new CookieOptions() { HttpOnly = false });
    return next(context);
});

app.MapHub<GridEventsHub>("/hubs/gridevents");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapApi();

app.Run();
