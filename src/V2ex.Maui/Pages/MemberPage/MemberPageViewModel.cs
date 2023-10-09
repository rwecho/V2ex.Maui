using CommunityToolkit.Mvvm.ComponentModel;
using V2ex.Api;

namespace V2ex.Maui.Pages;

public partial class MemberPageViewModel : BaseViewModel, IQueryAttributable
{
    public const string QueryUserNameKey = "username";

    [ObservableProperty]
    private string? _userName;

    [ObservableProperty]
    private MemberViewModel? _member;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(QueryUserNameKey, out var username))
        {
            this.UserName = username.ToString();
        }
    }

    protected override async Task OnLoad(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.UserName))
        {
            throw new InvalidOperationException("User name can not not be empty.");
        }

        var memberInfo = await this.ApiService.GetUserPageInfo(this.UserName);
        this.Member = memberInfo == null ? null : new MemberViewModel(memberInfo);
    }
}
