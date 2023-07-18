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
        
        var result = new List<HackerNewsStoryDto>();

        foreach (var storyId in storiesIds.Take(limit))
        {
            var story = await _hackerNewsApiClient.GetStoryAsync(storyId);
            if (story != null)
            {
                result.Add(story);
            }
        }
        
        return result.OrderByDescending(x => x.Score).ToList();
    }
}