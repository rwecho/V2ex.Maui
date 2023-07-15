using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace V2ex.Api;

public class ApiService
{
    private const string WAP_Android_USER_AGENT = "Mozilla/5.0 (Linux; Android 9.0; V2er Build/OPD3.170816.012) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Mobile Safari/537.36";

    private const string WAP_IOS_USER_AGENT = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1";

    private const string WEB_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79";

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        this.HttpClient = httpClientFactory.CreateClient("api");

        if (this.HttpClient.BaseAddress == null)
        {
            this.HttpClient.BaseAddress = new Uri(UrlUtils.BASE_URL);
            this.HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(WEB_USER_AGENT);
        }
    }

    private HttpClient HttpClient { get; }

    public async Task<DailyHotInfo?> GetDailyHot()
    {
        var url = "/api/topics/hot.json";
        var response = await this.HttpClient.GetAsync(url);
        return await response.ReadFromJson<DailyHotInfo>();
    }

    public async Task<NodeInfo?> GetNodeInfo(string nodeName)
    {
        var url = $"/api/nodes/show.json?name={nodeName}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.ReadFromJson<NodeInfo>();
    }

    public async Task<NodesInfo?> GetNodesInfo()
    {
        var url = "/api/nodes/s2.json";
        var response = await this.HttpClient.GetAsync(url);
        return await response.ReadFromJson<NodesInfo>();
    }

    public async Task<MemberInfo?> GetMemberInfo(string username)
    {
        var url = $"/api/members/show.json?username={username}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.ReadFromJson<MemberInfo>();
    }

    public async Task<SoV2EXSearchResultInfo?> Search(string keyword, int from = 0, string sort = "created")
    {
        var queryString = new Dictionary<string, string>
        {
            { "q", keyword },
            { "from", from.ToString() },
            { "sort", sort }
        };

        var url = $"https://www.sov2ex.com/api/search?{EncodeQuerystring(queryString)}";
        var response = await this.HttpClient.GetAsync(url);

        return await response.ReadFromJson<SoV2EXSearchResultInfo>();
    }

    private static string EncodeQuerystring(Dictionary<string, string> queryString)
    {
        return string.Join("&", queryString.Select(x => $"{x.Key}={x.Value}"));
    }

    public async Task<NewsInfo> GetTabTopics(string tab)
    {
        var url = "/?tab=" + tab;
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<NewsInfo>();
    }

    public async Task<NewsInfo> GetRecentTopics()
    {
        var url = "/recent";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<NewsInfo>();
    }

    public async Task<LoginParameters> GetLoginParameters()
    {
        var url = "/signin?next=/mission/daily";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<LoginParameters, RestrictedProblem>((error) =>
        {
            if (error.IsRestricted())
            {
                throw new InvalidOperationException(error.RestrictedContent);
            }
        });
    }

    public async Task<byte[]> GetCaptchaImage(LoginParameters loginParameters)
    {
        var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var url = $"{loginParameters.Captcha}?once={loginParameters.Once}&now={now}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<NewsInfo> Login(
        LoginParameters loginParameters,
        string username,
        string password,
        string captcha)
    {
        var url = "/signin";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { loginParameters.NameParameter, username },
                { loginParameters.PasswordParameter, password },
                { loginParameters.CaptchaParameter, captcha},
                { "once", loginParameters.Once },
                { "next", "/" },
            })
        };

        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/signin");
        var response = await this.HttpClient.SendAsync(request);

        return await response.GetEncapsulatedData<NewsInfo, LoginProblem>((error) =>
        {
            if (error.HasProblem())
            {
                throw new InvalidOperationException(string.Join("\r\n", error.Errors));
            }
        });
    }

    public async Task<TopicInfo> GetTopicDetail(string topicId, int page = 1)
    {
        var url = $"/t/{topicId}?p={page}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<TopicInfo>();
    }

    public async Task<NotificationInfo?> GetNotifications(int page = 1)
    {
        var url = $"/notifications?p={page}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<NotificationInfo>();
    }

    public async Task<FollowingInfo?> GetFollowingInfo(int page = 1)
    {
        var url = $"/my/following?page={page}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<FollowingInfo>();
    }

    public async Task<FavoriteTopicsInfo?> GetFavoriteTopics(int page = 1)
    {
        var url = $"/my/topics?page={page}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<FavoriteTopicsInfo>();
    }

    public async Task<FavoriteNodeInfo> GetFavoriteNodes()
    {
        var url = "/my/nodes";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<FavoriteNodeInfo>();
    }

    public async Task<NodesNavInfo> GetNodesNavInfo()
    {
        var url = "/";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<NodesNavInfo>();
    }

    public async Task<NodeTopicsInfo> GetNodeTopics(string node, int page = 1)
    {
        var url = $"/go/{node}?page={page}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<NodeTopicsInfo>();
    }

    public async Task<MemberPageInfo> GetUserPageInfo(string username)
    {
        var url = $"/member/{username}";

        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<MemberPageInfo>();
    }

    public Task<BingSearchResultInfo> BingSearch(string url)
    {
        throw new NotImplementedException();
    }

    public async Task<CreateTopicParameter> GetCreateTopicParameter()
    {
        var url = "/new";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<CreateTopicParameter>();
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
        return await response.GetEncapsulatedData<TopicInfo>();
    }

    public async Task<AppendTopicParameter> GetAppendTopicParameter(string topicId)
    {
        var url = $"/t/{topicId}/append";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/t/{topicId}");
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<AppendTopicParameter>();
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
        return await response.GetEncapsulatedData<TopicInfo>();
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
        return await response.GetEncapsulatedData<UnitInfo>();
    }

    public async Task<ThanksInfo> ThanksMoney()
    {
        var url = "/ajax/money";
        var response = await this.HttpClient.PostAsync(url, null);
        return await response.GetEncapsulatedData<ThanksInfo>();
    }

    public async Task<TopicInfo> FavoriteTopic(string topicId, string once)
    {
        var url = $"/favorite/topic/{topicId}?once={once}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/t/{topicId}");
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<TopicInfo>();
    }

    public async Task<NewsInfo> IgnoreTopic(string topicId, string once)
    {
        var url = $"/ignore/topic/{topicId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<NewsInfo>();
    }

    public async Task<UnitInfo> IgnoreReply(string replyId, string once)
    {
        var url = $"/ignore/reply/{replyId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<UnitInfo>();
    }

    public async Task<NodeTopicInfo> IgnoreNode(string nodeId, string once)
    {
        var url = $"/settings/ignore/node/{nodeId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<NodeTopicInfo>();
    }

    public async Task<NodeTopicInfo> UnignoreNode(string nodeId, string once)
    {
        var url = $"/settings/ignore/node/{nodeId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<NodeTopicInfo>();
    }

    public async Task<TopicInfo> UnfavoriteTopic(string topicId, string once)
    {
        var url = $"/unfavorite/topic/{topicId}?once={once}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/t/{topicId}");
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<TopicInfo>();
    }

    public async Task<UnitInfo> UpTopic(string topicId, string once)
    {
        var url = $"/up/topic/{topicId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<UnitInfo>();
    }

    public async Task<UnitInfo> DownTopic(string topicId, string once)
    {
        var url = $"/down/topic/{topicId}?once={once}";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<UnitInfo>();
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
        return await response.GetEncapsulatedData<TopicInfo>();
    }

    public async Task<UnitInfo> BlockUser(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<UnitInfo>();
    }

    public async Task<MemberPageInfo> FollowUser(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<MemberPageInfo>();
    }

    public async Task<UnitInfo> FavoriteNode(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<UnitInfo>();
    }

    public async Task<DailyInfo> GetDailyInfo()
    {
        var url = "/mission/daily";
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<DailyInfo>();
    }

    public async Task<DailyInfo> CheckIn(string once)
    {
        var url = $"/mission/daily/redeem?once={once}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<DailyInfo>();
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
        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<NewsInfo>();
    }

    public async Task<DailyInfo> RequestByUrl(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Referer", $"{UrlUtils.BASE_URL}/mission/daily");
        var response = await this.HttpClient.SendAsync(request);
        return await response.GetEncapsulatedData<DailyInfo>();
    }

    public async Task<TopicInfo> FadeTopic(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<TopicInfo>();
    }

    public async Task<TopicInfo> StickyTopic(string url)
    {
        var response = await this.HttpClient.GetAsync(url);
        return await response.GetEncapsulatedData<TopicInfo>();
    }
}