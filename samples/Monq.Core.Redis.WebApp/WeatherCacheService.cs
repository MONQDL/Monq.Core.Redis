using Monq.Core.Redis.RedisClient;

namespace Monq.Core.Redis.WebApp
{
    public class WeatherCacheService : RedisClientBase
    {
        public WeatherCacheService(IRedisConnectionFactory connectionFactory, IHostEnvironment env, IConfiguration configuration)
            : base(connectionFactory, env, configuration)
        {

        }

        public string GetWeatherDescription()
        {
            return Db.HashGet(KeyPrefix + "description", "test").ToString();
        }
    }
}
