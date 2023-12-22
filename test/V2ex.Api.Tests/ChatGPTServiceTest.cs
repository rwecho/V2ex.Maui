using V2ex.Blazor.Services;
using Volo.Abp.Testing;
using Xunit.Abstractions;

namespace V2ex.Api.Tests;

public class ChatGPTServiceTest: AbpIntegratedTest<ApiTestModule>
{
    public ChatGPTServiceTest(ITestOutputHelper testOutput)
    {
        this.ChatGPTService = this.GetRequiredService<ChatGPTService>();
        this.TestOutput = testOutput;
    }

    public ITestOutputHelper TestOutput { get; }
    private ChatGPTService ChatGPTService { get; }


    [Fact]
    public async Task FormatTopicAsync()
    {
        using var stream = await this.ChatGPTService.FormatTopicAsync("");

        using var textReader = new StreamReader(stream);

        string? line = null;
        while ((line = await textReader.ReadLineAsync()) != null)
        {
            TestOutput.WriteLine(line);
        }
    }
}
