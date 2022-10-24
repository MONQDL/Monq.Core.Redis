using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Monq.Core.Redis.Configuration;
using StackExchange.Redis;

namespace Monq.Core.Redis.RedisClient
{
    /// <summary>
    /// Realization <see cref="IRedisClient"/>, which contains are methods for interacting with Redis.
    /// </summary>
    public abstract class RedisClientBase : IRedisClient
    {
        readonly IRedisConnectionFactory _connectionFactory;

        protected string KeyPrefix { get; private set; }
        protected RedisOptions Options => _connectionFactory.Options;
        protected IDatabase Db { get; private set; }

        public IConnectionMultiplexer Connection { get; private set; }

        public RedisClientBase(IRedisConnectionFactory connectionFactory, IHostEnvironment env, IConfiguration configuration)
        {
            _connectionFactory = connectionFactory;
            Connection = connectionFactory.Connection();

            var applicationName = configuration[AppConstants.ApplicationName];
            if (string.IsNullOrEmpty(applicationName))
                applicationName = env.ApplicationName;

            KeyPrefix = string.IsNullOrEmpty(connectionFactory.Options.KeyPrefix)
                ? $"{env.EnvironmentName}:{applicationName}"
                : $"{connectionFactory.Options.KeyPrefix}:{applicationName}";

            Db = Connection.GetDatabase(connectionFactory.Options.DbNum ?? -1);
        }
    }
}
