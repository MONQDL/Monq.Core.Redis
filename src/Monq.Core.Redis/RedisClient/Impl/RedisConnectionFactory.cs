using Microsoft.Extensions.Options;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.Exceptions;
using Monq.Core.Redis.Extentions;
using StackExchange.Redis;

namespace Monq.Core.Redis.RedisClient.Impl
{
    /// <summary>
    /// Create IConnectionMultiplexer with Redis Server.
    /// </summary>
    /// <seealso cref="IRedisConnectionFactory" />
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        readonly IConnectionMultiplexer _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionFactory"/> class.
        /// </summary>
        /// <param name="optionsAccessor">The options accessors.</param>
        public RedisConnectionFactory(IOptions<RedisOptions> optionsAccessor)
        {
            if (optionsAccessor?.Value == null)
                throw new ConfigurationException("Can't read configuration from JSON");

            Options = optionsAccessor.Value;

            _connection = ConnectionMultiplexer.Connect(Options.ToRedisConfig());
        }

        /// <inheritdoc />
        public IConnectionMultiplexer Connection()
        {
            return _connection;
        }

        /// <inheritdoc />
        public RedisOptions Options { get; private set; }
    }
}