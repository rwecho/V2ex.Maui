using System.Net.Http.Json;
using System.Web;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class ApiService : ITransientDependency
{
    private const string WAP_Android_USER_AGENT = "Mozilla/5.0 (Linux; Android 9.0; V2er Build/OPD3.170816.012) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Mobile Safari/537.36";

    private const string WEB_USER_AGENT = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_4; V2er) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36";

    private const string WAP_IOS_USER_AGENT = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1";

    private const string BASE_URL = "https://www.v2ex.com";

    public ApiService(HttpClient httpClient)
    {
        this.HttpClient = httpClient;
    }

    private HttpClient HttpClient { get; }

    public async Task<IReadOnlyList<DailyHotInfo>> GetDailyHot()
    {
        var url = "/api/topics/hot.json";
        var response = await this.HttpClient.GetAsync(url);
        return await response.Content.ReadFromJsonAsync<List<DailyHotInfo>>() ?? new List<DailyHotInfo>();
    }

    public async Task<IReadOnlyList<NodeInfo>> GetNodeInfo()
    {
        var url = "/api/nodes/show.json";
        var response = await this.HttpClient.GetAsync(url);
        return await response.Content.ReadFromJsonAsync<List<NodeInfo>>() ?? new List<NodeInfo>();
    }

    public async Task<NodesInfo> GetNodesInfo()
    {
        var url = "/api/nodes/s2.json";
        var response = await this.HttpClient.GetAsync(url);
        return await response.Content.ReadFromJsonAsync<NodesInfo>() ?? new NodesInfo();
    }

    public async Task<MemberInfo> GetMemberInfo(string username)
    {
        var url = $"/api/members/show.json?username={username}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.Content.ReadFromJsonAsync<MemberInfo>() ?? new MemberInfo();
    }

    public async Task<SoV2EXSearchResultInfo> Search(string keyword, int from, string sort)
    {
        var queryString = new Dictionary<string, string>
        {
            { "q", keyword },
            { "from", from.ToString() },
            { "sort", sort }
        };

        var url = $"https://www.sov2ex.com/api/search?{EncodeQuerystring(queryString)}";
        var response = await this.HttpClient.GetAsync(url);

        return await response.Content.ReadFromJsonAsync<SoV2EXSearchResultInfo>() ??
            new SoV2EXSearchResultInfo();
    }

    private static string EncodeQuerystring(Dictionary<string, string> queryString)
    {
        return HttpUtility.UrlEncode(string.Join("&", queryString.Select(x => $"{x.Key}={x.Value}")));
    }

    public async Task<NewsInfo> GetTabTopics(string tab)
    {
        var url = "/?tab=" + tab;
        var response = await this.HttpClient.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();

        return NewsInfo.Parse(content);
    }

    public async Task<NewsInfo> GetRecentTopics()
    {
        var url = "/recent";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return NewsInfo.Parse(content);
    }

    public async Task<LoginParameters> GetLoginParameters()
    {
        var url = "/signin?next=/mission/daily";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return LoginParameters.Parse(content);
    }

    public async Task Login(string username, string password, string captcha)
    {
        var url = "/signin";

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "u", "username" },
                { "p", "password" },
                { "once", "once" },
                { "next", "/mission/daily" },
            })
        };

        request.Headers.Add("Referer", $"{BASE_URL}/signin");
        var response = await this.HttpClient.SendAsync(request);
    }

    public async Task<TopicInfo> GetTopicDetail(string topicId, int page)
    {
        var url = $"/t/{topicId}?p={page}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(content);
    }

    public async Task<NotificationInfo?> GetNotifications(int page)
    {
        var url = $"/notifications?p={page}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("user-agent", WEB_USER_AGENT);
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return NotificationInfo.Parse(content);
    }

    public async Task<FavoriteUsersTopicsInfo?> GetFavoriteUsersTopics(int page)
    {
        var url = "/my/following?page" + page;
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("user-agent", WAP_IOS_USER_AGENT);
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return FavoriteUsersTopicsInfo.Parse(content);
    }

    public async Task<FavoriteTopicsInfo?> GetFavoriteTopics(int page)
    {
        var url = "/my/topics?page" + page;
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("user-agent", WAP_IOS_USER_AGENT);
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return FavoriteTopicsInfo.Parse(content);
    }

    public async Task<FavoriteNodeInfo> GetFavoriteNodes()
    {
        var url = "/my/nodes";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return FavoriteNodeInfo.Parse(content);
    }

    public async Task<NodesNavInfo> GetNodesNavInfo()
    {
        var url = "/";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return NodesNavInfo.Parse(content);
    }

    public async Task<NodeTopicsInfo> GetNodeTopics(string node, int page)
    {
        var url = $"/go/{node}?page={page}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return NodeTopicsInfo.Parse(content);
    }

    public async Task<MemberPageInfo> GetUserPageInfo(string username)
    {
        var url = $"/member/{username}";

        var response = await this.HttpClient.GetAsync(url);
        var conent = await response.Content.ReadAsStringAsync();
        return MemberPageInfo.Parse(conent);
    }

    public Task<BingSearchResultInfo> BingSearch(string url)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateTopicParameter> GetCreateTopicParameter()
    {
        var url = "/new";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return CreateTopicParameter.Parse(content);
    }

    public async Task<TopicInfo> PostTopic(string title, string content,
        string nodeId, string once)
    {
        var url = "/new";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "title", title},
                { "content", content },
                { "node_name", nodeId },
                { "once", once },
            })
        };
        var response = await this.HttpClient.SendAsync(request);
        var contentHtml = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(contentHtml);
    }

    public async Task<AppendTopicParameter> GetAppendTopicParameter(string topicId)
    {
        var url = $"/t/{topicId}/append";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{BASE_URL}/t/{topicId}");
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return AppendTopicParameter.Parse(content);
    }

    public async Task<TopicInfo> AppendTopic(string topicId,
        string once,
        string content)
    {
        var url = $"/t/{topicId}/append";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "content", content },
                { "once", once },
            })
        };
        var response = await this.HttpClient.SendAsync(request);
        var htmlContent = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(htmlContent);
    }

    public async Task<UnitInfo> ThanksReplier(string replyId, string once)
    {
        var url = $"/thank/reply/{replyId}?once={once}";
        var response = await this.HttpClient.PostAsync(url, null);
        var content = await response.Content.ReadAsStringAsync();
        return UnitInfo.Parse(content);
    }

    public async Task<UnitInfo> ThanksCreator(string replyId, string once)
    {
        var url = $"/thank/topic/{replyId}?once={once}";
        var response = await this.HttpClient.PostAsync(url, null);
        var content = await response.Content.ReadAsStringAsync();
        return UnitInfo.Parse(content);
    }

    public async Task<ThanksInfo> ThanksMoney()
    {
        var url = "/ajax/money";
        var response = await this.HttpClient.PostAsync(url, null);
        var content = await response.Content.ReadAsStringAsync();
        return ThanksInfo.Parse(content);
    }

    public async Task<TopicInfo> FavoriteTopic(string topicId, string once)
    {
        var url = $"/favorite/topic/{topicId}?once={once}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{BASE_URL}/t/{topicId}");
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(content);
    }

    public async Task<NewsInfo> IgnoreTopic(string topicId, string once)
    {
        var url = $"/ignore/topic/{topicId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return NewsInfo.Parse(content);
    }

    public async Task<UnitInfo> IgnoreReply(string replyId, string once)
    {
        var url = $"/ignore/reply/{replyId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return UnitInfo.Parse(content);
    }

    public async Task<NodeTopicInfo> IgnoreNode(string nodeId, string once)
    {
        var url = $"/settings/ignore/node/{nodeId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return NodeTopicInfo.Parse(content);
    }

    public async Task<NodeTopicInfo> UnignoreNode(string nodeId, string once)
    {
        var url = $"/settings/ignore/node/{nodeId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return NodeTopicInfo.Parse(content);
    }

    public async Task<TopicInfo> UnfavoriteTopic(string topicId, string once)
    {
        var url = $"/unfavorite/topic/{topicId}?once={once}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{BASE_URL}/t/{topicId}");
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(content);
    }

    public async Task<UnitInfo> UpTopc(string topicId, string once)
    {
        var url = $"/up/topic/{topicId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return UnitInfo.Parse(content);
    }

    public async Task<UnitInfo> DownTopc(string topicId, string once)
    {
        var url = $"/down/topic/{topicId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return UnitInfo.Parse(content);
    }

    public async Task<TopicInfo> ReplyTopic(string topicId, string content, string once)
    {
        var url = $"/t/{topicId}";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "content", content },
                { "once", once },
            })
        };
        var response = await this.HttpClient.SendAsync(request);
        var contentHtml = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(contentHtml);
    }

    public async Task<UnitInfo> BlockUser(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return UnitInfo.Parse(content);
    }

    public async Task<MemberPageInfo> FollowUser(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return MemberPageInfo.Parse(content);
    }

    public async Task<UnitInfo> FavoriteNode(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return UnitInfo.Parse(content);
    }

    public async Task<DailyInfo> GetDailyInfo()
    {
        var url = "/mission/daily";
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return DailyInfo.Parse(content);
    }

    public async Task<DailyInfo> CheckIn(string once)
    {
        var url = $"/mission/daily/redeem?once={once}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return DailyInfo.Parse(content);
    }

    public async Task<NewsInfo> SignInTwoStep(string code, string once)
    {
        var url = "/2fa?next=/mission/daily";

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "code", code },
                { "once", once },
            })
        };
        request.Headers.Add("Referer", $"{BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return NewsInfo.Parse(content);
    }

    public async Task<DailyInfo> RequestByUrl(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return DailyInfo.Parse(content);
    }

    public async Task<TopicInfo> FadeTopic(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(content);
    }

    public async Task<TopicInfo> StickyTopic(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return TopicInfo.Parse(content);
    }
}