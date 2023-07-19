using HackerNews.Models;

namespace HackerNews.Services;

public interface IHackerNewsService
{
    Task<IEnumerable<HackerNewsStoryDto>> GetBestStoriesAsync(int limit);
}

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsApiClient _hackerNewsApiClient;

    public HackerNewsService(IHackerNewsApiClient hackerNewsApiClient)
    {
        _hackerNewsApiClient = hackerNewsApiClient;
    }
    
    public async Task<IEnumerable<HackerNewsStoryDto>> GetBestStoriesAsync(int limit)
    {
        var storiesIds = await _hackerNewsApiClient.GetBestStoriesIdsAsync();
        if (storiesIds == null)
        {
            return Enumerable.Empty<HackerNewsStoryDto>();
        }

        var tasks = storiesIds.Take(limit).Select(storyId => _hackerNewsApiClient.GetStoryAsync(storyId));
        var result = await Task.WhenAll(tasks);

        return result.Where(x => x != null).OrderByDescending(x => x.Score);
    }
}