using HackerNews.Models;

namespace HackerNews.Services;

public static class HackerNewsStoryMapper
{
    public static HackerNewsStoryDto? ToHackerNewsStoryDto(HackerNewsStory? story)
    {
        if (story == null)
        {
            return null;
        }
        
        var storyDto = new HackerNewsStoryDto();
        storyDto.Title = story.Title;
        storyDto.Uri = story.Url;
        storyDto.PostedBy = story.By;
        storyDto.Time = DateTimeOffset.FromUnixTimeSeconds(story.Time);
        storyDto.Score = story.Score;
        storyDto.CommentCount = story.Descendants;
        
        return storyDto;
    }
}