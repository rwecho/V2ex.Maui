using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages.ViewModels;

public partial class MemberPageViewModel : ObservableObject, IQueryAttributable, ITransientDependency
{
    [ObservableProperty]
    private string? _currentState, _userName;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoadCommand))]
    private bool _canCurrentStateChange = true;

    [ObservableProperty]
    private Exception? _exception;
    [ObservableProperty]
    private MemberViewModel? _member;

    public MemberPageViewModel(ApiService apiService)
    {
        this.ApiService = apiService;
    }

    private ApiService ApiService { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("username", out var username))
        {
            this.UserName = username.ToString();
        }
    }

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;

            if (string.IsNullOrEmpty(this.UserName))
            {
                throw new InvalidOperationException("User name can not not be empty.");
            }

            var memberInfo = await this.ApiService.GetMemberInfo(this.UserName);
            this.Member = memberInfo == null ? null : new MemberViewModel(memberInfo);
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }
}

public class MemberViewModel
{
    public MemberViewModel(MemberInfo member)
    {
            
    }
}