using StackExchange.Redis;
using System;
using System.Security.Authentication;

namespace Monq.Core.Redis.Configuration
{
    public class RedisOptions
    {
        public EndPoint[] EndPoints { get; set; } = Array.Empty<EndPoint>();

        /// <summary>
        /// If true, Connect will not create a connection while no servers are available.
        /// </summary>
        public bool? AbortOnConnectFail { get; set; }

        /// <summary>
        /// Enables a range of commands that are considered risky.
        /// </summary>
        public bool? AllowAdmin { get; set; }

        /// <summary>
        /// Time (ms) to allow for asynchronous operations.
        /// </summary>
        public int? AsyncTimeout { get; set; }

        /// <summary>
        /// Optional channel prefix for all pub/sub operations.
        /// </summary>
        public RedisChannel? ChannelPrefix { get; set; }

        /// <summary>
        /// Identification for the connection within redis.
        /// </summary>
        public string? ClientName { get; set; }

        /// <summary>
        /// Time (seconds) to check configuration. This serves as a keep-alive for interactive sockets, if it is supported.
        /// </summary>
        public int? ConfigCheckSeconds { get; set; }

        /// <summary>
        /// The number of times to repeat connect attempts during initial Connect.
        /// </summary>
        public int? ConnectRetry { get; set; }

        /// <summary>
        /// Timeout (ms) for connect operations.
        /// </summary>
        public int? ConnectTimeout { get; set; }

        /// <summary>
        /// Default database index, from 0 to databases - 1.
        /// </summary>
        public int? DefaultDatabase { get; set; }

        /// <summary>
        /// Time (seconds) at which to send a message to help keep sockets alive (60 sec default).
        /// </summary>
        public int? KeepAlive { get; set; }

        /// <summary>
        /// User for the redis server (for use with ACLs on redis 6 and above).
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// The password for the redis server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Type of proxy in use (if any); for example “twemproxy/envoyproxy”.
        /// </summary>
        public Proxy? Proxy { get; set; }

        /// <summary>
        /// Specifies that DNS resolution should be explicit and eager, rather than implicit.
        /// </summary>
        public bool? ResolveDns { get; set; }

        /// <summary>
        /// Used for connecting to a sentinel primary service.
        /// </summary>
        public string? ServiceName { get; set; }

        /// <summary>
        /// Specifies that SSL encryption should be used.
        /// </summary>
        public bool? Ssl { get; set; }

        /// <summary>
        /// Enforces a particular SSL host identity on the server’s certificate.
        /// </summary>
        public string? SslHost { get; set; }

        /// <summary>
        /// Ssl/Tls versions supported when using an encrypted connection. Use ‘|’ to provide multiple values..
        /// </summary>
        public SslProtocols? SslProtocols { get; set; }

        /// <summary>
        /// Time (ms) to allow for synchronous operations.
        /// </summary>
        public int? SyncTimeout { get; set; }

        /// <summary>
        /// Key to use for selecting a server in an ambiguous primary scenario.
        /// </summary>
        public string? TieBreaker { get; set; }

        /// <summary>
        /// Redis version level (useful when the server does not make this available).
        /// </summary>
        public Version? DefaultVersion { get; set; }

        /// <summary>
        /// A Boolean value that specifies whether the certificate revocation list is checked during authentication.
        /// </summary>
        public bool? CheckCertificateRevocation { get; set; }

        /// <summary>
        /// If not empty this field can be used. Full value: $"{KeyPrefix}:{applicationName}".
        /// </summary>
        public string? KeyPrefix { get; set; }

        /// <summary>
        /// Database number.
        /// </summary>
        public int? DbNum { get; set; } = 0;
    }

    public class EndPoint
    {
        /// <summary>
        /// Host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port.
        /// </summary>
        public int Port { get; set; } = 6379;
    }
}
