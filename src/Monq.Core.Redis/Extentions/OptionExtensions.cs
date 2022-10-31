using Monq.Core.Redis.Configuration;
using StackExchange.Redis;

namespace Monq.Core.Redis.Extentions
{
    public static class OptionExtensions
    {
        public static ConfigurationOptions ToRedisConfig(this RedisOptions options)
        {
            var configuration = new ConfigurationOptions
            {
                Password = options.Password ?? options.Auth,
            };

            if (options.Proxy is not null)
                configuration.Proxy = options.Proxy.Value;

            if (options.AbortOnConnectFail is not null)
                configuration.AbortOnConnectFail = options.AbortOnConnectFail.Value;

            if (options.AllowAdmin is not null)
                configuration.AllowAdmin = options.AllowAdmin.Value;

            if (options.AsyncTimeout is not null)
                configuration.AsyncTimeout = options.AsyncTimeout.Value;

            if (options.SyncTimeout is not null)
                configuration.SyncTimeout = options.SyncTimeout.Value;

            if (options.Ssl is not null)
                configuration.Ssl = options.Ssl.Value;

            if (options.SslHost is not null)
                configuration.SslHost = options.SslHost;

            if (options.SslProtocols is not null)
                configuration.SslProtocols = options.SslProtocols;

            if (options.ResolveDns is not null)
                configuration.ResolveDns = options.ResolveDns.Value;

            if (options.DefaultDatabase is not null)
                configuration.DefaultDatabase = options.DefaultDatabase.Value;

            if (options.DefaultVersion is not null)
                configuration.DefaultVersion = options.DefaultVersion;

            if (options.ConnectRetry is not null)
                configuration.ConnectRetry = options.ConnectRetry.Value;

            if (options.ConnectTimeout is not null)
                configuration.ConnectTimeout = options.ConnectTimeout.Value;

            if (options.ChannelPrefix is not null)
                configuration.ChannelPrefix = options.ChannelPrefix.Value;

            if (options.ClientName is not null)
                configuration.ClientName = options.ClientName;

            if (options.ConfigCheckSeconds is not null)
                configuration.ConfigCheckSeconds = options.ConfigCheckSeconds.Value;

            if (options.KeepAlive is not null)
                configuration.KeepAlive = options.KeepAlive.Value;

            if (options.User is not null)
                configuration.User = options.User;

            if (options.TieBreaker is not null)
                configuration.TieBreaker = options.TieBreaker;

            if (options.ServiceName is not null)
                configuration.ServiceName = options.ServiceName;

            if (options.CheckCertificateRevocation is not null)
                configuration.CheckCertificateRevocation = options.CheckCertificateRevocation.Value;

            foreach (var endPoint in options.EndPoints)
                configuration.EndPoints.Add(endPoint.Host, endPoint.Port);

            return configuration;
        }
    }
}
