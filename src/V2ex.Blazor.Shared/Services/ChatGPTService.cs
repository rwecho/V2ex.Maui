namespace V2ex.Blazor.Services;

public class ChatGPTService
{
    public ChatGPTService(IHttpClientFactory httpClientFactory)
    {
        this.HttpClientFactory = httpClientFactory;
    }

    private IHttpClientFactory HttpClientFactory { get; }

    public async Task<Stream> FormatTopicAsync(string topicId)
    {
        var client = this.HttpClientFactory.CreateClient("api");
        var response = await client.GetAsync($"https://v2ex-maui.vercel.app/api/topic/{topicId}");
        return await response.Content.ReadAsStreamAsync();
    }
}