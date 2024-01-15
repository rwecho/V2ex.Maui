namespace V2ex.Blazor.Services;

public record Topic(string Id, string Title, string? UserName,  string?Avatar, string? NodeName, int Replies);

public class TopicHistoryService
{
    private const string TopicHistoryFileName = "topics.json";

    private readonly List<Topic> _history = [];
    public TopicHistoryService(IPreferences preferences)
    {
        this.Preferences = preferences;
        this.Load();
    }

    private IPreferences Preferences { get; }

    private void Load()
    {
        _history.AddRange(Preferences.Get(TopicHistoryFileName, Array.Empty<Topic>()));
    }

    public void Push(Topic topic)
    {
        var existTopic = _history.FirstOrDefault(x => x.Id == topic.Id);

        if (existTopic != null)
        {
            _history.Remove(existTopic);
        }

        _history.Insert(0, topic);

        if (_history.Count > 100)
        {
            _history.Remove(_history.Last());
        }
        Preferences.Set(TopicHistoryFileName, _history);
    }

    public IReadOnlyList<Topic> GetList()
    {
        return _history;
    }
}
