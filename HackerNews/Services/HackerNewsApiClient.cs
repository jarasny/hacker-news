using HackerNews.Models;

namespace HackerNews.Services;

public interface IHackerNewsApiClient
{
    Task<HackerNewsStoryDto?> GetStoryAsync(int id);
    
    Task<IEnumerable<int>?> GetBestStoriesIdsAsync();
}

public class HackerNewsApiClient : IHackerNewsApiClient
{
    private readonly HttpClient _httpClient;

    public HackerNewsApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(HackerNewsApiClient));
    }

    public async Task<IEnumerable<int>?> GetBestStoriesIdsAsync()
    {
        var ids = await _httpClient.GetFromJsonAsync<List<int>>("beststories.json");

        return ids;
    }

    public async Task<HackerNewsStoryDto?> GetStoryAsync(int id)
    {
        var story = await _httpClient.GetFromJsonAsync<HackerNewsStory?>($"item/{id}.json");
        var storyDto = HackerNewsStoryMapper.ToHackerNewsStoryDto(story);
        
        return storyDto;
    }
}