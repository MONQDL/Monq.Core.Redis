# Offline package and Native AOT consumer qualification

This executable consumes a locally packed artifact using the ordinary
`AddRedisClient(IConfiguration)` registration and its source-generated binding.
It resolves only `IOptions<RedisOptions>`: no Redis connection or credentials are
needed. It checks exact literal channel-prefix bytes and the database-selection
matrix, including absent/null, explicit 0/-1/nonzero and independent DefaultDatabase.
The linked unit tests exercise the same matrix. Native publication applies to the
consumer executable, not to the library as a standalone application.

Keep official package version/publication separate from this fixture. In PowerShell,
from the repository root, use an isolated feed/cache and a source-specific suffix:

```powershell
$source = git rev-parse --short=12 HEAD
$suffix = "local-$source"
dotnet test src/Monq.Core.Redis.Tests -c Release
dotnet pack src/Monq.Core.Redis -c Release -p:VersionSuffix=$suffix -o artifacts/feed
dotnet publish samples/OptionsSmoke -c Release -r win-x64 -p:RedisQualificationVersion=2.2.0-$suffix -p:RestoreAdditionalProjectSources="$PWD/artifacts/feed" -p:RestorePackagesPath="$PWD/artifacts/packages-win" -o artifacts/smoke-win
./artifacts/smoke-win/OptionsSmoke.exe
```

On a Linux x64 host with the .NET 10 SDK and Native AOT prerequisites, use the
same checkout and local feed (absolute paths), with a separate cache/output:

```sh
source=$(git rev-parse --short=12 HEAD)
dotnet publish samples/OptionsSmoke -c Release -r linux-x64 \
  -p:RedisQualificationVersion=2.2.0-local-$source \
  -p:RestoreAdditionalProjectSources="$PWD/artifacts/feed" \
  -p:RestorePackagesPath="$PWD/artifacts/packages-linux" \
  -p:BaseIntermediateOutputPath="$PWD/artifacts/obj-linux/" \
  -o artifacts/smoke-linux
./artifacts/smoke-linux/OptionsSmoke
```

Do not publish the fixture package or install it as released 2.2.0. An accepted
official version/channel and integration approval are required before merging a
branch that triggers package publication. The existing workflow runs on pushes to
develop/master and publishes; topic PR validation is absent unless explicitly added.
