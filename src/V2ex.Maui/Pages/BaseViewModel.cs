using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using V2ex.Api;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public abstract partial class BaseViewModel : ObservableObject, ITransientDependency
{
    [ObservableProperty]
    private string? _currentState;

    [ObservableProperty]
    private Exception? _exception;

    [ObservableProperty]
    private bool _canCurrentStateChange = true;

    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;

    protected ApiService ApiService => LazyServiceProvider.LazyGetRequiredService<ApiService>();

    protected IStringLocalizer<MauiResource> Localizer => LazyServiceProvider.LazyGetRequiredService<IStringLocalizer<MauiResource>>();

    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);

    [RelayCommand(CanExecute = nameof(CanCurrentStateChange))]
    public async Task Load(CancellationToken cancellationToken = default)
    {
        try
        {
            this.CurrentState = StateKeys.Loading;
            await this.OnLoad(cancellationToken);
            this.CurrentState = StateKeys.Success;
        }
        catch (Exception exception)
        {
            this.Exception = exception;
            this.CurrentState = StateKeys.Error;
        }
    }

    protected abstract Task OnLoad(CancellationToken cancellationToken);
}
