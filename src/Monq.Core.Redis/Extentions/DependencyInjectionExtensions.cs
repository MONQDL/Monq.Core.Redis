using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.RedisClient;
using Monq.Core.Redis.RedisClient.Impl;

namespace Monq.Core.Redis.Extentions
{
    /// <summary>
    /// Extensions for easy implementation with DI tools.
    /// </summary>
    public static class DependencyInjectionExtensions
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
