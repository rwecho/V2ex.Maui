using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Maui.Pages.Components;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.ViewModels;

public partial class DailyHotPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private List<TopicRowViewModel> _items = new();

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        this.Items = (await this.ApiService.GetDailyHot()
            ?? throw new InvalidOperationException("Can not get daily hot info"))
            .Select(o => TopicRowViewModel.Create(
                o.Title,
                o.Member?.AvatarLarge,
                o.Member?.UserName,
                DateTimeOffset.FromUnixTimeSeconds(o.Created)
                    .ToLocalTime()
                    .ToHumanReadable(this.Localizer),
                o.Node?.Title,
                o.LastReplyBy,
                o.Id.ToString(),
                o.Node?.Name.ToString(),
                o.Replies))
            .ToList();
    }
}