# Monq.Core.Redis

[![NuGet](https://img.shields.io/nuget/v/Monq.Core.Redis.svg)](https://www.nuget.org/packages/Monq.Core.Redis)
[![Downloads](https://img.shields.io/nuget/dt/Monq.Core.Redis.svg)](https://www.nuget.org/packages/Monq.Core.Redis)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A lightweight wrapper around **StackExchange.Redis** that provides seamless dependency injection integration for .NET applications with automatic key prefixing and centralized configuration.

## Features

- 🚀 **Quick DI Setup** — Register Redis client in one line of code
- 🔑 **Automatic Key Prefixing** — Isolate data by environment and application name
- ⚙️ **Type-Safe Configuration** — Strongly-typed `RedisOptions` class
- 🔗 **Singleton Connection** — Single `IConnectionMultiplexer` instance per application
- 🛡️ **Redis Sentinel Support** — Built-in support for high-availability setups
- 📦 **Full StackExchange.Redis Access** — No limitations on Redis commands
- 🎯 **Native AOT Compatible** — Fully compatible with Native AOT compilation

## Installation

```powershell
Install-Package Monq.Core.Redis
```

## Quick Start

### 1. Configure Redis Connection

Add Redis configuration to your `appsettings.json`:

```json
{
  "Redis": {
    "EndPoints": [
      {
        "Host": "localhost",
        "Port": 6379
      }
    ],
    "Password": "your-password",
    "DbNum": 0
  }
}
```

### 2. Register in Dependency Injection

In `Program.cs` or `Startup.cs`:

```csharp
using Microsoft.Extensions.DependencyInjection;

// ...

builder.Services.AddRedisClient(builder.Configuration.GetSection("Redis"));
```

### 3. Create a Redis Service

Inherit from `RedisClientBase` to get automatic connection and key prefixing:

```csharp
using Monq.Core.Redis.RedisClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

public class WeatherCacheService : RedisClientBase
{
    public WeatherCacheService(
        IRedisConnectionFactory connectionFactory,
        IHostEnvironment env,
        IConfiguration configuration)
        : base(connectionFactory, env, configuration)
    {
    }

    public string GetWeatherDescription(string id)
    {
        return Db.HashGet(KeyPrefix + ":weather", id).ToString();
    }

    public void SetWeatherDescription(string id, string description)
    {
        Db.HashSet(KeyPrefix + ":weather", id, description);
    }

    public bool DeleteWeatherDescription(string id)
    {
        return Db.HashDelete(KeyPrefix + ":weather", id);
    }
}
```

### 4. Use in Your Controllers

```csharp
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly WeatherCacheService _weatherCacheService;

    public WeatherForecastController(WeatherCacheService weatherCacheService)
    {
        _weatherCacheService = weatherCacheService;
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var description = _weatherCacheService.GetWeatherDescription("test");
        // ...
    }
}
```

## Configuration Options

### Minimal Configuration

```json
{
  "Redis": {
    "EndPoints": [
      {
        "Host": "redis-server.com",
        "Port": 6379
      }
    ]
  }
}
```

### Full Configuration Example

```json
{
  "Redis": {
    "EndPoints": [
      {
        "Host": "redis1.com",
        "Port": 6379
      },
      {
        "Host": "redis2.com",
        "Port": 6379
      }
    ],
    "Password": "password",
    "DbNum": 1,
    "AbortOnConnectFail": false,
    "AllowAdmin": false,
    "ChannelPrefix": null,
    "ClientName": "my-app",
    "ConfigCheckSeconds": 60,
    "ConnectRetry": 3,
    "ConnectTimeout": 5000,
    "DefaultDatabase": null,
    "KeepAlive": 60,
    "User": "myuser",
    "Proxy": "None",
    "ResolveDns": false,
    "ServiceName": "mymaster",
    "Ssl": false,
    "SslHost": null,
    "SslProtocols": "Tls12,Tls13",
    "AsyncTimeout": 0,
    "SyncTimeout": 0,
    "TieBreaker": "",
    "DefaultVersion": "2.0",
    "CheckCertificateRevocation": true,
    "KeyPrefix": "myapp"
  }
}
```

### Configuration Properties

| Property | Type | Description |
|----------|------|-------------|
| `EndPoints` | Array | Redis server endpoints (host and port) |
| `Password` | string | Authentication password |
| `DbNum` | int | Database number (0-15) |
| `AbortOnConnectFail` | bool | If true, fails immediately when no servers are available |
| `AllowAdmin` | bool | Enables administrative commands |
| `ClientName` | string | Connection identifier in Redis |
| `ConnectTimeout` | int | Connection timeout in milliseconds |
| `KeepAlive` | int | Keep-alive interval in seconds |
| `User` | string | Username for Redis ACL (Redis 6+) |
| `ServiceName` | string | Sentinel master service name |
| `Ssl` | bool | Enable SSL encryption |
| `SslProtocols` | string | SSL/TLS versions (e.g., "Tls12,Tls13") |
| `KeyPrefix` | string | Custom prefix for all keys |
| `SyncTimeout` | int | Timeout for synchronous operations |
| `AsyncTimeout` | int | Timeout for asynchronous operations |

## Key Prefixing

The library automatically generates key prefixes to isolate data between environments:

- **Default behavior**: `{EnvironmentName}:{ApplicationName}`
  - Example: `Production:my-api`
- **With custom `KeyPrefix`**: `{KeyPrefix}:{ApplicationName}`
  - Example: `myapp:my-api`

This ensures that development, staging, and production environments don't share cached data.

### Override Application Name

Set a custom application name via environment variable or configuration:

```json
{
  "APPLICATION_NAME": "custom-app-name"
}
```

## Advanced Usage

### Direct Connection Access

If you don't want to inherit from `RedisClientBase`, you can inject `IRedisConnectionFactory` directly:

```csharp
public class MyService
{
    private readonly IDatabase _db;

    public MyService(IRedisConnectionFactory factory)
    {
        _db = factory.Connection().GetDatabase();
    }

    public string GetValue(string key)
    {
        return _db.StringGet(key).ToString();
    }

    public void SetValue(string key, string value)
    {
        _db.StringSet(key, value);
    }
}
```

### Redis Sentinel Configuration

```json
{
  "Redis": {
    "EndPoints": [
      { "Host": "sentinel1.com", "Port": 26379 },
      { "Host": "sentinel2.com", "Port": 26379 },
      { "Host": "sentinel3.com", "Port": 26379 }
    ],
    "ServiceName": "mymaster",
    "Password": "password"
  }
}
```

### Using Pub/Sub

```csharp
public class MessageService : RedisClientBase
{
    public MessageService(
        IRedisConnectionFactory connectionFactory,
        IHostEnvironment env,
        IConfiguration configuration)
        : base(connectionFactory, env, configuration)
    {
    }

    public void PublishMessage(string channel, string message)
    {
        var subscriber = Connection.GetSubscriber();
        subscriber.Publish(channel, message);
    }

    public void Subscribe(string channel, Action<string> handler)
    {
        var subscriber = Connection.GetSubscriber();
        subscriber.Subscribe(channel, (ch, msg) => handler(msg));
    }
}
```

## Requirements

- .NET 8.0 or higher
- StackExchange.Redis 2.x
- Redis server 3.0 or higher

## Troubleshooting

### Connection Issues

**Problem**: Application starts but Redis connection fails silently

**Solution**: Set `AbortOnConnectFail: true` to fail fast during development:

```json
{
  "Redis": {
    "AbortOnConnectFail": true,
    "ConnectTimeout": 5000
  }
}
```

**Problem**: Timeout errors on operations

**Solution**: Increase timeout values:

```json
{
  "Redis": {
    "SyncTimeout": 5000,
    "AsyncTimeout": 5000
  }
}
```

### Key Prefix Not Working

**Problem**: Keys don't have expected prefix

**Solution**: Ensure you're using `KeyPrefix` property from base class:

```csharp
// ✅ Correct
Db.StringSet(KeyPrefix + ":mykey", value);

// ❌ Incorrect
Db.StringSet(":mykey", value);
```

### SSL Connection Issues

**Problem**: SSL handshake fails

**Solution**: Specify SSL protocols explicitly:

```json
{
  "Redis": {
    "Ssl": true,
    "SslProtocols": "Tls12,Tls13",
    "SslHost": "redis.example.com"
  }
}
```

## Migration Guide

### From Direct StackExchange.Redis Usage

**Before:**
```csharp
var connection = ConnectionMultiplexer.Connect("localhost");
var db = connection.GetDatabase();
```

**After:**
```csharp
// In Program.cs
services.AddRedisClient(Configuration.GetSection("Redis"));

// In your service
public class MyService : RedisClientBase
{
    public MyService(IRedisConnectionFactory factory, IHostEnvironment env, IConfiguration config)
        : base(factory, env, config) { }
    
    // Use Db property directly
}
```

### From Version 1.x to 2.x

The `Auth` property is deprecated. Use `Password` instead:

```json
// ❌ Old (deprecated)
{ "Auth": "password" }

// ✅ New
{ "Password": "password" }
```

## Samples

See the [WebApp sample](samples/Monq.Core.Redis.WebApp) for a complete working example.

## License

This project is licensed under the [MIT License](LICENSE).
