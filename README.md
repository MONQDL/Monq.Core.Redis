# Monq.Core.Redis

## Installation

```powershell
Install-Package Monq.Core.Redis
```

## Using the library

The library allows you create and configure Redis connection.

##### An example of using a configuration file

`appsettings.json`.
```json
  "Redis": {
    "EndPoints": [ // The endpoints defined for this configuration.
      {
        "Host": "redis1.com",
        "Port": 6379
      },
      {
        "Host": "redis2.com",
        "Port": 6379
      }
    ], 
    "Password": "password", // The password for the redis server.
    "DbNum": 1 // Database number.
    "AbortOnConnectFail": false, // If true, Connect will not create a connection while no servers are available.
    "AllowAdmin": false, // Enables a range of commands that are considered risky.
    "ChannelPrefix": null, // Optional channel prefix for all pub/sub operations.
    "ClientName": null, // Identification for the connection within redis.
    "ConfigCheckSeconds": 60, // Time (seconds) to check configuration. This serves as a keep-alive for interactive sockets, if it is supported.
    "ConnectRetry": 3, // The number of times to repeat connect attempts during initial Connect.
    "ConnectTimeout": 5000, // Timeout (ms) for connect operations.
    "DefaultDatabase": null, // Default database index, from 0 to databases - 1.
    "KeepAlive": -1, // Time (seconds) at which to send a message to help keep sockets alive (60 sec default).
    "User": null, // User for the redis server (for use with ACLs on redis 6 and above).
    "Proxy": Proxy.None, // Type of proxy in use (if any); for example “twemproxy/envoyproxy”.
    "ResolveDns": false, // Specifies that DNS resolution should be explicit and eager, rather than implicit.
    "ServiceName": null, // Used for connecting to a sentinel primary service.
    "Ssl": false, // Specifies that SSL encryption should be used.
    "SslHost": null, // Enforces a particular SSL host identity on the server’s certificate.
    "SslProtocols": null, // Ssl/Tls versions supported when using an encrypted connection. Use ‘|’ to provide multiple values..
    "AsyncTimeout": 0, // Time (ms) to allow for asynchronous operations.
    "SyncTimeout": 0, // Time (ms) to allow for synchronous operations.
    "TieBreaker": "", // Key to use for selecting a server in an ambiguous primary scenario.
    "DefaultVersion": "2.0", // Redis version level (useful when the server does not make this available).
    "CheckCertificateRevocation": true, //A Boolean value that specifies whether the certificate revocation list is checked during authentication.
    "KeyPrefix": "" // If not empty this field can be used. Full value: $"{KeyPrefix}:{applicationName}".
  },
```

Declare function in CacheService.

```csharp
public class WeatherCacheService : RedisClientBase
{
    public WeatherCacheService(IRedisConnectionFactory connectionFactory, IHostEnvironment env, IConfiguration configuration) 
        : base(connectionFactory, env, configuration)
    {
        ...
    }

    public string GetWeatherDescription()
    {
        return Db.HashGet(KeyPrefix + "description", "test").ToString();
    }
}
```

Set connection with `Redis` from `appsettings.json`. Allow inject `IRedisConnectionFactory`.

`startup.cs`.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddRedisClient(Configuration.GetSection(Redis));
    services.AddTransient<CacheService>();
    ...
}
```

Use in controller.

```csharp
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    readonly WeatherCacheService _weatherCacheService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherCacheService weatherCacheService)
    {
        _weatherCacheService = weatherCacheService;
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var description = _weatherCacheService.GetWeatherDescription();
        ...
    }
}
```