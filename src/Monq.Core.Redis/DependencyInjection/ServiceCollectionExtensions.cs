using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.RedisClient;
using Monq.Core.Redis.RedisClient.Impl;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for easy implementation with DI tools.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add service implementations for communication and caching in Redis.
    /// </summary>
    /// <param name="services">Dependencies injection container.</param>
    /// <param name="configuration">Configuration section <see cref="RedisOptions"/>.</param>
    /// <returns></returns>
    public static IServiceCollection AddRedisClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisOptions>(configuration);

        services.TryAddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
        services.TryAddSingleton<IConnectionMultiplexer>(sp => sp.GetRequiredService<IRedisConnectionFactory>().Connection());

        return services;
    }
}
