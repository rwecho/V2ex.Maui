using V2ex.Api;

namespace V2ex.Blazor.Services;

public class EmojiService(IPreferences preferences)
{
    private IPreferences Preferences { get; } = preferences;
    private const string RecentEmojiKey = nameof(RecentEmojiKey);

    public async Task<List<IEmoji>> GetRecentListAsync()
    {
        var emojiNames = Preferences.Get<List<string>>(RecentEmojiKey, []);

        var all = (await this.GetListAsync()).SelectMany(x => x.Value).ToArray();
        var list = emojiNames.Select(name => all.FirstOrDefault(x => x.Name == name)).Where(x => x != null).ToList();
        return list as List<IEmoji>;
    }

    public async Task SetRecentAsync(IEmoji emoji)
    {
        var emojiNames = Preferences.Get<List<string>>(RecentEmojiKey, []);
        emojiNames.Remove(emoji.Name);
        emojiNames.Insert(0, emoji.Name);
        emojiNames = emojiNames.Distinct().ToList();
        if (emojiNames.Count > 10)
        {
            emojiNames = emojiNames.Take(10).ToList();
        }
        Preferences.Set(RecentEmojiKey, emojiNames);
        await Task.CompletedTask;
    }

    public Task<Dictionary<string, List<IEmoji>>> GetListAsync()
    {
        var result = new Dictionary<string, List<IEmoji>>
        {
            {
                "经典表情",
                [
                    new ImageEmoji("[狗头]","https://i.imgur.com/io2SM1h.png","https://i.imgur.com/0icl60r.png"),
                    new ImageEmoji("[马]","https://i.imgur.com/8EKZv7I.png","https://i.imgur.com/ANFUX52.png"),
                    new ImageEmoji("[不高兴]","https://i.imgur.com/huX6coX.png","https://i.imgur.com/N7JEuvc.png"),
                    new ImageEmoji("[呵呵]","https://i.imgur.com/RvoLAbX.png","https://i.imgur.com/xSzIqrK.png"),
                    new ImageEmoji("[真棒]","https://i.imgur.com/xr1UOz1.png","https://i.imgur.com/w8YEw9Q.png"),
                    new ImageEmoji("[鄙视]","https://i.imgur.com/u6jlqVq.png","https://i.imgur.com/8JFNANq.png"),
                    new ImageEmoji("[疑问]","https://i.imgur.com/F29pmQ6.png","https://i.imgur.com/EbbTQAR.png"),
                    new ImageEmoji("[嘲笑]","https://i.imgur.com/BaWcsMR.png","https://i.imgur.com/0OGfJw4.png"),
                    new ImageEmoji("[滑稽]","https://i.imgur.com/lmbN0yI.png","https://i.imgur.com/Pc0wH85.png"),
                    new ImageEmoji("[笑眼]","https://i.imgur.com/ZveiiGy.png","https://i.imgur.com/PI1CfEr.png"),
                    new ImageEmoji("[狂汗]","https://i.imgur.com/veWihk6.png","https://i.imgur.com/3LtHdQv.png"),
                    new ImageEmoji("[大哭]","https://i.imgur.com/hu4oR6C.png","https://i.imgur.com/b4X9XLE.png"),
                    new ImageEmoji("[喷]","https://i.imgur.com/bkw3VRr.png","https://i.imgur.com/wnZL13L.png"),
                    new ImageEmoji("[苦笑]","https://i.imgur.com/VUWFktU.png","https://i.imgur.com/NAfspZ1.png"),
                    new ImageEmoji("[喝酒]","https://i.imgur.com/2ZZSapE.png","https://i.imgur.com/rVbSVak.png"),
                    new ImageEmoji("[吃瓜]","https://i.imgur.com/ee8Lq7H.png","https://i.imgur.com/0L26og9.png"),
                    new ImageEmoji("[捂脸]","https://i.imgur.com/krir4IG.png","https://i.imgur.com/qqBqgVm.png"),
                    new ImageEmoji("[呕]","https://i.imgur.com/6CUiUxv.png","https://i.imgur.com/kgdxRsG.png"),
                    new ImageEmoji("[阴险]","https://i.imgur.com/MA8YqTP.png","https://i.imgur.com/e94jbaT.png"),
                    new ImageEmoji("[怒]","https://i.imgur.com/n4kWfGB.png","https://i.imgur.com/iMXxNxh.png"),
                    new ImageEmoji("[衰]","https://i.imgur.com/voHFDyQ.png","https://i.imgur.com/XffE6gu.png"),
                    new ImageEmoji("[合十]","https://i.imgur.com/I8x3ang.png","https://i.imgur.com/T4rJVee.png"),
                    new ImageEmoji("[赞]","https://i.imgur.com/lG44yUl.png","https://i.imgur.com/AoF5PLp.png"),
                    new ImageEmoji("[踩]","https://i.imgur.com/cJp0uKZ.png","https://i.imgur.com/1XYGfXj.png"),
                    new ImageEmoji("[爱心]","https://i.imgur.com/sLENaF5.png","https://i.imgur.com/dND56oX.png"),
                    new ImageEmoji("[心碎]","https://i.imgur.com/AZxJzve.png","https://i.imgur.com/RiUsPci.png"),
                ]
            },
            {
                "小黄脸",
                [
                    new TextEmoji("😀"),
                    new TextEmoji("😁"),
                    new TextEmoji("😂"),
                    new TextEmoji("🤣"),
                    new TextEmoji("😅"),
                    new TextEmoji("😊"),
                    new TextEmoji("😋"),
                    new TextEmoji("😘"),
                    new TextEmoji("🥰"),
                    new TextEmoji("😗"),
                    new TextEmoji("🤩"),
                    new TextEmoji("🤔"),
                    new TextEmoji("🤨"),
                    new TextEmoji("😐"),
                    new TextEmoji("😑"),
                    new TextEmoji("🙄"),
                    new TextEmoji("😏"),
                    new TextEmoji("😪"),
                    new TextEmoji("😫"),
                    new TextEmoji("🥱"),
                    new TextEmoji("😜"),
                    new TextEmoji("😒"),
                    new TextEmoji("😔"),
                    new TextEmoji("😨"),
                    new TextEmoji("😰"),
                    new TextEmoji("😱"),
                    new TextEmoji("🥵"),
                    new TextEmoji("😡"),
                    new TextEmoji("🥳"),
                    new TextEmoji("🥺"),
                    new TextEmoji("🤭"),
                    new TextEmoji("🧐"),
                    new TextEmoji("😎"),
                    new TextEmoji("🤓"),
                    new TextEmoji("😭"),
                    new TextEmoji("🤑"),
                    new TextEmoji("🤮")
                ]
            },
            {
                "手势",
                [
                    new TextEmoji("🙋"),
                    new TextEmoji("🙎"),
                    new TextEmoji("🙅"),
                    new TextEmoji("🙇"),
                    new TextEmoji("🤷"),
                    new TextEmoji("🤏"),
                    new TextEmoji("👉"),
                    new TextEmoji("✌️"),
                    new TextEmoji("🤘"),
                    new TextEmoji("🤙"),
                    new TextEmoji("👌"),
                    new TextEmoji("🤌"),
                    new TextEmoji("👍"),
                    new TextEmoji("👎"),
                    new TextEmoji("👋"),
                    new TextEmoji("🤝"),
                    new TextEmoji("🙏"),
                    new TextEmoji("👏")
                ]
            },
            {
                "庆祝",
                [
                    new TextEmoji("✨"), new TextEmoji("🎉"), new TextEmoji("🎊")
                ]
            },
            {
                "其他",
                [
                    new TextEmoji("👻"),
                    new TextEmoji("🤡"),
                    new TextEmoji("🐔"),
                    new TextEmoji("👀"),
                    new TextEmoji("💩"),
                    new TextEmoji("🦄"),
                    new TextEmoji("🐧"),
                    new TextEmoji("🐶"),
                    new TextEmoji("🐒"),
                    new TextEmoji("🙈"),
                    new TextEmoji("🙉"),
                    new TextEmoji("🙊"),
                    new TextEmoji("🐵")
                ]
            }
        };


        return Task.FromResult(result);
    }
}


public interface IEmoji
{
    string Name { get; }
}

public class TextEmoji(string name) : IEmoji
{
    public string Name { get; } = name;
}

public class ImageEmoji(string name, string? low, string? high) : IEmoji
{
    public string Name { get; } = name;

    public string? Low { get; } = low;

    public string? High { get; } = high;
}