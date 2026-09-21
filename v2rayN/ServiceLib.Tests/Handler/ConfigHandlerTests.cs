namespace ServiceLib.Tests.Handler;

public class ConfigHandlerTests
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public void GetPreSocksItem_V2flyWithTun_ShouldHandTunnelToSingboxHelper(bool legacyProtect)
    {
        var config = CoreConfigTestFactory.CreateConfigWithTun(ECoreType.v2fly, false);
        config.TunModeItem.EnableLegacyProtect = legacyProtect;
        CoreConfigTestFactory.BindAppManagerConfig(config);

        var node = CoreConfigTestFactory.CreateVmessNode(ECoreType.v2fly);
        var item = ConfigHandler.GetPreSocksItem(config, node, ECoreType.v2fly);

        await item.Should().NotBeNull();
        await item!.CoreType.Should().BeEqualTo(ECoreType.sing_box);
    }

    [Test]
    public void GetPreSocksItem_XrayWithoutLegacyProtect_ShouldNotBuildHelper()
    {
        var config = CoreConfigTestFactory.CreateConfigWithTun(ECoreType.Xray, false);
        config.TunModeItem.EnableLegacyProtect = false;

        var node = CoreConfigTestFactory.CreateVmessNode(ECoreType.Xray);
        var item = ConfigHandler.GetPreSocksItem(config, node, ECoreType.Xray);

        await item.Should().BeNull();
    }
}
