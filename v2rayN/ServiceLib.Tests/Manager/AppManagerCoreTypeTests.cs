using ServiceLib.Tests.CoreConfig;

namespace ServiceLib.Tests.Manager;

public class AppManagerCoreTypeTests
{
    [Test]
    [Arguments(ECoreType.v2fly)]
    [Arguments(ECoreType.v2fly_v5)]
    public async Task GetCoreType_GroupNodeOnCoreWithoutTunnel_ShouldFallBackToXray(ECoreType coreType)
    {
        CoreConfigTestFactory.BindAppManagerConfig(CoreConfigTestFactory.CreateConfig());
        var node = new ProfileItem { ConfigType = EConfigType.ProxyChain, CoreType = coreType };

        await AppManager.Instance.GetCoreType(node, node.ConfigType).Should().BeEqualTo(ECoreType.Xray);
    }

    [Test]
    public async Task GetCoreType_PolicyGroupOnSingbox_ShouldKeepSingbox()
    {
        CoreConfigTestFactory.BindAppManagerConfig(CoreConfigTestFactory.CreateConfig());
        var node = new ProfileItem { ConfigType = EConfigType.PolicyGroup, CoreType = ECoreType.sing_box };

        await AppManager.Instance.GetCoreType(node, node.ConfigType).Should().BeEqualTo(ECoreType.sing_box);
    }

    [Test]
    public async Task GetCoreType_SingleNodeOnV2fly_ShouldKeepV2fly()
    {
        CoreConfigTestFactory.BindAppManagerConfig(CoreConfigTestFactory.CreateConfig());
        var node = CoreConfigTestFactory.CreateVmessNode(ECoreType.v2fly);

        await AppManager.Instance.GetCoreType(node, node.ConfigType).Should().BeEqualTo(ECoreType.v2fly);
    }
}
