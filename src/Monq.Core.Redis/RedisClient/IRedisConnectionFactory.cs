using Monq.Core.Redis.Configuration;
using StackExchange.Redis;

namespace Monq.Core.Redis.RedisClient
{
    /// <summary>
    /// Создание подключения к Redis.
    /// </summary>
    public interface IRedisConnectionFactory
    {
        /// <summary>
        /// Подключиться к Redis.
        /// </summary>
        /// <returns></returns>
        IConnectionMultiplexer Connection();

        /// <summary>
        /// Get RedisOptions.
        /// </summary>
        RedisOptions Options { get; }
    }
}