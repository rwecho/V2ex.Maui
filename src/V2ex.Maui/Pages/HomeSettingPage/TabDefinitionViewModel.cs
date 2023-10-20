
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using V2ex.Api;

namespace V2ex.Maui.Pages;

public partial class TabDefinitionViewModel : ObservableObject
{
    private bool _isVisible;
    [ObservableProperty]
    private string _name, _description;

    [ObservableProperty]
    private int _order;
    [ObservableProperty]
    private bool _isBeingDragged, _isBeingDraggedOver;

    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        set
        {
            if (!value && this.Owner.Tabs.Count(o => o.IsVisible) <= 3)
            {
                OnPropertyChanged(nameof(IsVisible));
                return;
            }
            this.SetProperty(ref _isVisible, value);
        }
    }

    protected override void OnPropertyChanging(System.ComponentModel.PropertyChangingEventArgs e)
    {
        base.OnPropertyChanging(e);
    }

    public HomeSettingPageViewModel Owner { get; }

    public TabDefinitionViewModel(HomeSettingPageViewModel owner,TabDefinition tabDefinition)
    {
        this.Owner = owner;
        this.Name = tabDefinition.Name;
        this.Description = tabDefinition.Description;
        this.Order = tabDefinition.Order;
        this.IsVisible = tabDefinition.IsVisible;
    }

    [RelayCommand]
    public Task DragStarting(CancellationToken cancellationToken)
    {
        this.IsBeingDragged = true;

        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task Drop(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task DropCompleted(CancellationToken cancellationToken)
    {
        this.IsBeingDragged = false;
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task DragLeave(CancellationToken cancellationToken)
    {
        this.IsBeingDraggedOver = false;
        return Task.CompletedTask;
    }

    [RelayCommand]
    public Task DragOver(CancellationToken cancellationToken)
    {
        this.IsBeingDraggedOver = true;
        return Task.CompletedTask;
    }
}
