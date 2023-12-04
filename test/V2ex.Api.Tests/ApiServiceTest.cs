using Volo.Abp.Testing;

namespace V2ex.Api.Tests;

public class ApiServiceTest : AbpIntegratedTest<ApiTestModule>
{
    public ApiServiceTest()
    {
        this.ApiService = this.GetRequiredService<ApiService>();
    }

    private ApiService ApiService { get; }

    [Fact]
    public async Task GetDailyHotTest()
    {
        var dailyHotInfo = await this.ApiService.GetDailyHot();
        Assert.NotNull(dailyHotInfo);
    }

    [Fact]
    public async Task GetNodeInfoTest()
    {
        var nodeName = "suzhou";
        var nodeInfo = await this.ApiService.GetNodeInfo(nodeName);
        Assert.NotNull(nodeInfo);
    }

    [Fact]
    public async Task GetNodesInfoTest()
    {
        var nodesInfo = await this.ApiService.GetNodesInfo();

        Assert.NotNull(nodesInfo);
    }

    [Fact]
    public async Task GetNodesNavInfoTest()
    {
        var nodesNavInfo = await this.ApiService.GetNodesNavInfo();
        Assert.NotNull(nodesNavInfo);
    }

    [Fact]
    public async Task GetMemberInfoTest()
    {
        var nodeName = "rwecho";
        var memberInfo = await this.ApiService.GetMemberInfo(nodeName);
        Assert.NotNull(memberInfo);
    }

    [Fact]
    public async Task SearchTest()
    {
        var result = await this.ApiService.Search("v2ex");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTabTopicsTest()
    {
        var tab = "all";
        var result = await this.ApiService.GetTabTopics(tab);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetRecentTopicsTest()
    {
        var result = await this.ApiService.GetRecentTopics();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetLoginParametersTest()
    {
        var result = await this.ApiService.GetLoginParameters();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task LoginTest()
    {
        var loginParameters = await this.ApiService.GetLoginParameters();

        var image = await this.ApiService.GetCaptchaImage(loginParameters);

        using (var fileStream = File.OpenWrite($"{loginParameters.Once}.png"))
        {
            await fileStream.WriteAsync(image);
        }
        var captcha = "";
        await this.ApiService.Login(loginParameters, "rwecho", "", captcha);
    }

    [Fact]
    public async Task GetTopicDetailTest()
    {
        var result = await this.ApiService.GetTopicDetail("961440");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetNotificationsTest()
    {
        var result = await this.ApiService.GetNotifications();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetFollowingInfoTest()
    {
        var result = await this.ApiService.GetFollowingInfo();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetFavoriteTopicsTest()
    {
        var result = await this.ApiService.GetFavoriteTopics();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetFavoriteNodesTest()
    {
        var result = await this.ApiService.GetFavoriteNodes();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserPageInfoTest()
    {
        var username = "rwecho";
        var result = await this.ApiService.GetUserPageInfo(username);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCreateTopicParameterTest()
    {
        var result = await this.ApiService.GetCreateTopicParameter();
    }
}