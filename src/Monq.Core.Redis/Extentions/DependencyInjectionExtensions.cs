using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.RedisClient;
using Monq.Core.Redis.RedisClient.Impl;

namespace Monq.Core.Redis.Extentions
{
    /// <summary>
    /// Расширения для удобства внедрения средствами DI.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Добавить реализации сервисов для взаимодействия и кэширования в Redis.
        /// </summary>
        /// <param name="services">Контейнер внедрения зависимостей.</param>
        /// <param name="configuration">Секция конфигурации <see cref="RedisOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisOptions>(configuration);

            services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();

            return services;
        }
    }

}
