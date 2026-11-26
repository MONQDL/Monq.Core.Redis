using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monq.Core.Redis.Configuration;
using Monq.Core.Redis.Extensions;

internal static class OptionsQualification
{
    internal static void ChannelPrefixes()
    {
        foreach (var prefix in new string?[] { null, "", "events:", " \t ", "события:猫:", "events:*" })
        {
            // JSON null and omitted are distinct configuration inputs.
            var value = prefix is null ? "null" : "\"" + System.Text.Json.JsonEncodedText.Encode(prefix).ToString() + "\"";
            CheckPrefix("{\"Redis\":{\"ChannelPrefix\":" + value + "}}", prefix);
        }
        CheckPrefix("{\"Redis\":{}}", null);
    }

    static void CheckPrefix(string json, string? expected)
    {
        var options = Bind(json);
        Require(options.ChannelPrefix?.ToString() == expected, "ChannelPrefix changed during ordinary registration");
        var channel = options.ToRedisConfig().ChannelPrefix;
        Require(!channel.IsPattern, "ChannelPrefix must be literal even when containing *");
        var actualBytes = (byte[]?)channel;
        var expectedBytes = expected is null ? null : Encoding.UTF8.GetBytes(expected);
        // RedisChannel represents an empty literal as an empty channel, like its default value.
        Require((actualBytes ?? Array.Empty<byte>()).AsSpan().SequenceEqual(expectedBytes ?? Array.Empty<byte>()),
            "ChannelPrefix UTF-8 bytes changed");
    }

    internal static void DatabaseSelection()
    {
        foreach (var defaultDatabase in new int?[] { null, 5 })
        {
            foreach (var selection in new (string? Json, int Expected)[] { (null, -1), ("null", -1), ("-1", -1), ("0", 0), ("3", 3), ("31", 31) })
            {
                var members = defaultDatabase is null ? "" : "\"DefaultDatabase\":5,";
                var json = "{\"Redis\":{" + members + "\"ClientName\":\"offline\"" +
                    (selection.Json is null ? "" : ",\"DbNum\":" + selection.Json) + "}}";
                var options = Bind(json);
                Require(selection.Json == "null" ? options.DbNum is null or -1 : options.DbNum == selection.Expected, "DbNum selection changed during registration");
                Require(options.ToRedisConfig().DefaultDatabase == defaultDatabase, "DefaultDatabase is independent");
                Require((options.DbNum ?? -1) == selection.Expected, "Wrapper GetDatabase argument changed");
            }
        }
        var explicitNull = new RedisOptions { DbNum = null, DefaultDatabase = 5 };
        Require((explicitNull.DbNum ?? -1) == -1, "Actual null must delegate to the client");
        Require(explicitNull.ToRedisConfig().DefaultDatabase == 5, "Actual null must not erase DefaultDatabase");
    }

    static RedisOptions Bind(string json)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
        var services = new ServiceCollection();
        services.AddRedisClient(configuration.GetSection("Redis"));
        using var provider = services.BuildServiceProvider();
        // Never resolve the connection factory/multiplexer; these assertions are offline.
        return provider.GetRequiredService<IOptions<RedisOptions>>().Value;
    }

    static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
