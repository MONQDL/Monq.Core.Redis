using Monq.Core.Redis.Configuration;
using StackExchange.Redis;

namespace Monq.Core.Redis.RedisClient
{
    /// <summary>
    /// Connect to Redis.
    /// </summary>
    public interface IRedisConnectionFactory
    {
        /// <summary>
        /// Connect to Redis.
        /// </summary>
        /// <returns></returns>
        IConnectionMultiplexer Connection();

        /// <summary>
        /// Get RedisOptions.
        /// </summary>
        RedisOptions Options { get; }
    }
}