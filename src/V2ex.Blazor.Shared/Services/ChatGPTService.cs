using Microsoft.Extensions.Options;

namespace V2ex.Blazor.Services;

public class ChatGPTService
{
    private readonly HttpClient HttpClient;

    public ChatGPTService(IHttpClientFactory httpClientFactory, IAppInfoService appInfoService,
        IOptions<ChatGPTOptions> options)
    {
        this.HttpClient = httpClientFactory.CreateClient("ai");

        if (this.HttpClient.BaseAddress == null)
        {
            this.HttpClient.BaseAddress = new Uri(options.Value.Endpoint);
        }

        this.AppInfoService = appInfoService;
    }

    private IAppInfoService AppInfoService { get; }

    public async Task<Stream> FormatTopicAsync(string topicId)
    {
        var version = this.AppInfoService.GetVersionNumber();
        var response = await HttpClient.GetAsync($"/api/topic/{topicId}?version={version}", HttpCompletionOption.ResponseHeadersRead);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStreamAsync();
        }
        throw new InvalidOperationException(response.ReasonPhrase);
    }
}
