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
http://localhost:8000/stories/best/{limit}
```

`Swagger UI`
```
http://localhost:8000/swagger
```

## Cache

Application is using simple in-memory caching to avoid too many requests to Hacker News API. Instead of built-in `IMemoryCache`, which has common multi threading issue, `LazyCache` external package was used in applicaton.<br />
Current solution assumes that there is single instance of application running. Otherwise, if we would like to achieve consistency between many instances, we should apply some Distributed Cache solution like Redis.

There are two main resources that are being cached in application:
* <b>Best stories ids list</b> - can change over the time, trying to keep newest version but don't want to call Hacker News API every time.
Expiration time is controlled by `StoriesListCacheExpirationInSeconds`. Default expiration is 60s absolute time, so the list will be refreshed every 60s.

* <b>Stories objects</b> - not chagning over the time itself, but are not needed when not on Best Stories list anymore. Its expiration time is counted since last usage and controlled by `StoryCacheExpirationInSeconds`. Default expiration is 3600s which is 1 hour, so if given Story was not used for 1 hour, its being removed from the cache.

## Configuration
```json
"HackerNewsSettings": {
  "BaseAddress": "https://hacker-news.firebaseio.com/v0/",
  "StoriesListCacheExpirationInSeconds": 60,
  "StoryCacheExpirationInSeconds": 3600
}
```

## Enhancements
* Use more sophisticated method of caching Stories objects, e.g. keeping in cache only Stories that are in current best stories ids list
* Be aware of hacker News API limitations, running API calls in parallel may result in exceeding bandwidth
* Use Scrutor to register cache as decorator
* Add unit tests
* Add error handling (e.g. no connection to Hacker News API)
* Add logging
