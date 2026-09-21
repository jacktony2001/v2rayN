namespace ServiceLib.Tests.CoreConfig.Context;

public class TunOptionWarningsTests
{
    private static TunModeItem Defaults()
    {
        return new TunModeItem
        {
            EnableTun = true,
            AutoRoute = true,
            StrictRoute = true,
            IcmpRouting = Global.TunIcmpRoutingPolicies.First(),
        };
    }

    [Test]
    public async Task TunOptionWarnings_SingboxWithDefaults_ShouldNotWarn()
    {
        await CoreConfigContextBuilder.TunOptionWarnings(Defaults(), ECoreType.sing_box).Should().BeEmpty();
    }

    [Test]
    public async Task TunOptionWarnings_SingboxWithoutAutoRoute_ShouldWarnAboutTotalBypass()
    {
        var tunMode = Defaults();
        tunMode.AutoRoute = false;

        var warnings = CoreConfigContextBuilder.TunOptionWarnings(tunMode, ECoreType.sing_box);

        await warnings.Should().HaveCount(1);
        await warnings[0].Should().BeEqualTo(ResUI.MsgTunAutoRouteDisabled);
    }

    [Test]
    public async Task TunOptionWarnings_XrayOwningTunnel_ShouldNameIgnoredOptions()
    {
        var tunMode = Defaults();
        tunMode.AutoRoute = false;
        tunMode.Stack = Global.TunStacks.Last();
        tunMode.IcmpRouting = "direct";

        var warnings = CoreConfigContextBuilder.TunOptionWarnings(tunMode, ECoreType.Xray);

        await warnings.Should().HaveCount(1);
        await warnings[0].Contains("auto_route").Should().BeTrue();
        await warnings[0].Contains("stack").Should().BeTrue();
        await warnings[0].Contains("icmp_routing").Should().BeTrue();
    }

    [Test]
    public async Task TunOptionWarnings_XrayWithDefaults_ShouldNotWarn()
    {
        await CoreConfigContextBuilder.TunOptionWarnings(Defaults(), ECoreType.Xray).Should().BeEmpty();
    }
}
