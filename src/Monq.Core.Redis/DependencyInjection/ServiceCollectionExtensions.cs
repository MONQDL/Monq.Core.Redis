using Microsoft.Extensions.Configuration;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.RedisClient;
using Monq.Core.Redis.RedisClient.Impl;

namespace Microsoft.Extensions.DependencyInjection
{
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

            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();

            return services;
        }
    }
}
