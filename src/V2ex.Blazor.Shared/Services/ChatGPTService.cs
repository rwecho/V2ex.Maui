namespace V2ex.Blazor.Services;

public class ChatGPTService
{
    private readonly HttpClient HttpClient;

    public ChatGPTService(IHttpClientFactory httpClientFactory, IAppInfoService appInfoService)
    {
        this.HttpClient = httpClientFactory.CreateClient("ai");

        if(this.HttpClient.BaseAddress == null)
        {
            this.HttpClient.BaseAddress = new Uri("http://localhost:3000");
        }

        this.AppInfoService = appInfoService;
    }

    private IAppInfoService AppInfoService { get; }

    public async Task<Stream> FormatTopicAsync(string topicId)
    {
        var version = this.AppInfoService.GetVersionNumber();
        var response = await HttpClient.GetAsync($"/api/topic/{topicId}?version={version}", HttpCompletionOption.ResponseHeadersRead);
        return await response.Content.ReadAsStreamAsync();
    }
}
