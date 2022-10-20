using StackExchange.Redis;

namespace Monq.Core.Redis.RedisClient
{
    /// <summary>
    /// Интерфейс представляет собой методы для взаимодействия с Redis.
    /// </summary>
    public interface IRedisClient
    {
        IConnectionMultiplexer Connection { get; }
    }
}