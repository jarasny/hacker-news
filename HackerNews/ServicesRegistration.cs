using HackerNews.Models;
using HackerNews.Services;
using LazyCache;
using Microsoft.Extensions.Options;

namespace HackerNews;

public static class ServicesRegistration
{
    public static void AddPreBuildDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var hackerNewsSettings = builder.Configuration.GetSection(nameof(HackerNewsSettings));

        builder.Services.AddLazyCache();
        
        builder.Services.Configure<HackerNewsSettings>(hackerNewsSettings);
        builder.Services.AddHttpClient(nameof(HackerNewsApiClient), client =>
        {
            var baseAddress = hackerNewsSettings.GetValue<string>("BaseAddress");
            if (baseAddress != null)
            {
                client.BaseAddress = new Uri(baseAddress);
            }
        });

        builder.Services.AddScoped<HackerNewsApiClient>();
        builder.Services.AddScoped<IHackerNewsApiClient>(x => new HackerNewsApiClientCached(
            x.GetRequiredService<HackerNewsApiClient>(),
            x.GetRequiredService<IAppCache>(),
            x.GetRequiredService<IOptions<HackerNewsSettings>>()
        ));
        builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
    }

    public static void AddPostBuildDependencies(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}