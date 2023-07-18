using HackerNews;
using HackerNews.Models;
using HackerNews.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddPreBuildDependencies();

var app = builder.Build();

app.AddPostBuildDependencies();

app.MapGet("stories/best/{limit:int}",
        async (int limit, IHackerNewsService hackerNewsService) =>
        {
            var stories = await hackerNewsService.GetBestStoriesAsync(limit);

            return Results.Json(stories, Helpers.DefaultJsonSerializerOptions);
        })
    .Produces<IList<HackerNewsStoryDto>>();

app.Run();