using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Monq.Core.Redis.Configuration;
using StackExchange.Redis;
using System;

namespace Monq.Core.Redis.RedisClient;

/// <summary>
/// Realization <see cref="IRedisClient"/>, which contains are methods for interacting with Redis.
/// </summary>
public abstract class RedisClientBase : IRedisClient
{
    readonly IRedisConnectionFactory _connectionFactory;

    /// <summary>
    /// Key prefix. Default - {env.EnvironmentName}:{applicationName}, f.e. Production:pl-userspaces-api
    /// </summary>
    protected string KeyPrefix { get; private set; }

    /// <summary>
    /// Connection options.
    /// </summary>
    protected RedisOptions Options => _connectionFactory.Options;

    /// <summary>
    /// Currently connected DB.
    /// </summary>
    protected IDatabase Db { get; private set; }

    /// <summary>
    /// Connection to Redis server.
    /// </summary>
    public IConnectionMultiplexer Connection { get; private set; }

    /// <summary>
    /// Create new object of <see cref="RedisClientBase"/>.
    /// </summary>
    /// <param name="connectionFactory">Redis connection factory.</param>
    /// <param name="env">HostEnvironment.</param>
    /// <param name="configuration">Configuration.</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected RedisClientBase(IRedisConnectionFactory connectionFactory, 
        IHostEnvironment env, 
        IConfiguration configuration)
    {
        if (env == null)
            throw new ArgumentNullException(nameof(env), $"{nameof(env)} is null.");

        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory), $"{nameof(connectionFactory)} is null.");
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
