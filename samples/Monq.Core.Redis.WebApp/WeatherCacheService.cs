using Microsoft.Extensions.Options;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.RedisClient;
using StackExchange.Redis;

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
            return Db.HashGet(KeyPrefix + "desc", "fdg").ToString();
        }
    }
}
