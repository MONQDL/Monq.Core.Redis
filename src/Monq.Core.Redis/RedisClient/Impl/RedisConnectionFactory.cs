using Microsoft.Extensions.Options;
using Monq.Core.Redis.Configuration;
using StackExchange.Redis;
using System;

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
            Options = optionsAccessor.Value;

            var configuration = new ConfigurationOptions { 
                Password = Options.Password,
            };

            if (Options.Proxy is not null)
            {
                configuration.Proxy = Options.Proxy.Value;
            }

            if (Options.AbortOnConnectFail is not null)
            {
                configuration.AbortOnConnectFail = Options.AbortOnConnectFail.Value;
            }

            if (Options.AllowAdmin is not null)
            {
                configuration.AllowAdmin = Options.AllowAdmin.Value;
            }

            if (Options.AsyncTimeout is not null)
            {
                configuration.AsyncTimeout = Options.AsyncTimeout.Value;
            }

            if (Options.SyncTimeout is not null)
            {
                configuration.SyncTimeout = Options.SyncTimeout.Value;
            }

            if (Options.Ssl is not null)
            {
                configuration.Ssl = Options.Ssl.Value;
            }

            if (Options.SslHost is not null)
            {
                configuration.SslHost = Options.SslHost;
            }

            if (Options.SslProtocols is not null)
            {
                configuration.SslProtocols = Options.SslProtocols;
            }

            if (Options.ResolveDns is not null)
            {
                configuration.ResolveDns = Options.ResolveDns.Value;
            }

            if (Options.DefaultDatabase is not null)
            {
                configuration.DefaultDatabase = Options.DefaultDatabase.Value;
            }

            if (Options.DefaultVersion is not null)
            {
                configuration.DefaultVersion = Options.DefaultVersion;
            }

            if (Options.ConnectRetry is not null)
            {
                configuration.ConnectRetry = Options.ConnectRetry.Value;
            }

            if (Options.ConnectTimeout is not null)
            {
                configuration.ConnectTimeout = Options.ConnectTimeout.Value;
            }

            if (Options.ChannelPrefix is not null)
            {
                configuration.ChannelPrefix = Options.ChannelPrefix.Value;
            }

            if (Options.ClientName is not null)
            {
                configuration.ClientName = Options.ClientName;
            }

            if (Options.ConfigCheckSeconds is not null)
            {
                configuration.ConfigCheckSeconds = Options.ConfigCheckSeconds.Value;
            }

            if (Options.KeepAlive is not null)
            {
                configuration.KeepAlive = Options.KeepAlive.Value;
            }

            if (Options.User is not null)
            {
                configuration.User = Options.User;
            }

            if (Options.TieBreaker is not null)
            {
                configuration.TieBreaker = Options.TieBreaker;
            }

            if (Options.CheckCertificateRevocation is not null)
            {
                configuration.CheckCertificateRevocation = Options.CheckCertificateRevocation.Value;
            }

            foreach (var endPoint in Options.EndPoints)
                configuration.EndPoints.Add(endPoint.Host, endPoint.Port);

            _connection = ConnectionMultiplexer.Connect(configuration);
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