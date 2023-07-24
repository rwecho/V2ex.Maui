using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using V2ex.Api;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class DailyHotPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private List<Components.TopicViewModel> _items = new();

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        this.Items = (await this.ApiService.GetDailyHot()
            ?? throw new InvalidOperationException("Can not get daily hot info"))
            .Select(o => new Components.TopicViewModel
            {
                Title = o.Title,
                Avatar = o.Member?.AvatarLarge,
                UserName = o.Member?.UserName,
                NodeName = o.Node?.Name,
                CreatedText = DateTimeOffset.FromUnixTimeSeconds(o.Created)
                    .ToLocalTime()
                    .ToHumanReadable(this.Localizer),
                Replies = o.Replies,
                LastReplyBy = o.LastReplyBy
            })
            .ToList();
    }
}
