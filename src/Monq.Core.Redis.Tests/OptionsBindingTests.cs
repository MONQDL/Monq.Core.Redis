using Xunit;

namespace Monq.Core.Redis.Tests;

public class OptionsBindingTests
{
    [Fact]
    public void OrdinaryRegistrationPreservesLiteralChannelPrefixes() => OptionsQualification.ChannelPrefixes();

    [Fact]
    public void DatabaseSelectionPreservesExplicitValuesAndClientDefault() => OptionsQualification.DatabaseSelection();
}
