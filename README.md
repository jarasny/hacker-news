# Hacker News
Simple API to retrieve Best Stories from Hacker News

## Usage

To use dockerized application, first build docker image with command

```
docker build . -t hacker-news
```

Then you can run web api on specific port, 8000 as an example
```
docker run -p 8000:80 hacker-news
```

## Endpoints

`Get first n best stories`
```
http://localhost:8000/stories/best?limit={limit}
```

`Swagger UI`
```
http://localhost:8000/swagger
```

## Cache

Application is using simple in-memory caching to avoid too many requests to Hacker News API. Instead of built-in `IMemoryCache`, which has common multi threading issue, `LazyCache` external package was used in applicaton.<br />
Current solution assumes that there is single instance of application running. Otherwise, if we would like to achieve consistency between many instances, we should apply some Distributed Cache solution like Redis.

There are two main resources that are being cached in application:
* <b>Stories ids</b> - can change over the time, we want to keep as new as posible version but not call Hacker News API every time. Expiration time is controlled by `StoriesListCacheExpirationInSeconds`. Default expiration is 60s absolute time, so the list will be refreshed every 60s.

* <b>Stories objects</b> - only Score and CommentCount are changing over time, so if we want to keep this propeties actual, we should shorten its cache expiration time controlled by `StoryCacheExpirationInSeconds`. Default expiration is 300s absolute time which is 5 minutes, every 5 minutes story is removed from cache and new version is taken from API if needed.

## Configuration
```json
"HackerNewsSettings": {
  "BaseAddress": "https://hacker-news.firebaseio.com/v0/",
  "StoriesListCacheExpirationInSeconds": 60,
  "StoryCacheExpirationInSeconds": 300
}
```

## Enhancements
* Be aware of hacker News API limitations, running API calls in parallel may result in exceeding bandwidth
* Use Scrutor to register cache as decorator
* Add unit tests
* Add error handling (e.g. no connection to Hacker News API)
* Add logging
