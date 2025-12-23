using Microsoft.Extensions.Configuration;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.RedisClient;
using Monq.Core.Redis.RedisClient.Impl;
using System.Diagnostics.CodeAnalysis;

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
    [RequiresUnreferencedCode("Configuration binding requires unreferenced code")]
    [RequiresDynamicCode("Configuration binding requires dynamic code")]
    public static IServiceCollection AddRedisClient(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Use source generator after drop dotnet 7.
        services.Configure<RedisOptions>(configuration);

        services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();

        return services;
    }
}
