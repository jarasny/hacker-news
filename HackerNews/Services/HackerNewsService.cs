using HackerNews.Models;

namespace HackerNews.Services;

public interface IHackerNewsService
{
    Task<IEnumerable<HackerNewsStoryDto>> GetBestStories(int limit);
}

public class HackerNewsService : IHackerNewsService
{
    private readonly IHackerNewsApiClient _hackerNewsApiClient;

    public HackerNewsService(IHackerNewsApiClient hackerNewsApiClient)
    {
        _hackerNewsApiClient = hackerNewsApiClient;
    }
    
    public async Task<IEnumerable<HackerNewsStoryDto>> GetBestStories(int limit)
    {
        var storiesIds = await _hackerNewsApiClient.GetBestStoriesIds();
        if (storiesIds == null)
        {
            return Enumerable.Empty<HackerNewsStoryDto>();
        }
        
        var result = new List<HackerNewsStoryDto>();

        foreach (var storyId in storiesIds.Take(limit))
        {
            var story = await _hackerNewsApiClient.GetStory(storyId);
            if (story != null)
            {
                result.Add(story);
            }
        }
        
        return result.OrderByDescending(x => x.Score).ToList();
    }
}