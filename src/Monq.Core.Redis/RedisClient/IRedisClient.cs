using StackExchange.Redis;

namespace Monq.Core.Redis.RedisClient
{
    /// <summary>
    /// The interface presents methods for interacting with Redis.
    /// </summary>
    public interface IRedisClient
    {
        /// <summary>
        /// Connect to Redis.
        /// </summary>
        /// <returns></returns>
        IConnectionMultiplexer Connection { get; }
    }
}