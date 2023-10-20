
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using V2ex.Api;
using IPreferences = V2ex.Api.IPreferences;

namespace V2ex.Maui.Pages;

public partial class HomeSettingPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<TabDefinitionViewModel> _tabs = new();

    public HomeSettingPageViewModel(IPreferences preferences)
    {
        this.Preferences = preferences;
    }

    public IPreferences Preferences { get; }

    const string TabDefinitionsKey = "TabDefinitionsKey";
    protected override Task OnLoad(CancellationToken cancellationToken)
    {
        var tabDefinitions = this.Preferences.Get(TabDefinitionsKey, TabDefinition.GetTabDefinitions());

        this.Tabs.Clear();
        foreach (var definition in tabDefinitions)
        {
            var tab = new TabDefinitionViewModel(this, definition);
            tab.PropertyChanged += Tab_PropertyChanged;
            this.Tabs.Add(tab);
        }
        return Task.CompletedTask;
    }

    private void Tab_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TabDefinitionViewModel.IsVisible))
        {
            var tabDefinitions = this.Tabs
                .Select(o => new TabDefinition(o.Name, o.Description, o.Order, o.IsVisible))
                .ToList();
            this.Preferences.Set(TabDefinitionsKey, tabDefinitions);
        }
    }

    public void Move(TabDefinitionViewModel source, TabDefinitionViewModel destination)
    {
        this.Tabs.Remove(source);
        var index = this.Tabs.IndexOf(destination);
        this.Tabs.Insert(index, source);

        for (var i = 0; i < this.Tabs.Count; i++)
        {
            this.Tabs[i].Order = i;
        }

        var tabDefinitions = this.Tabs.Select(o => new TabDefinition(o.Name, o.Description, o.Order, o.IsVisible))
            .ToList();
        this.Preferences.Set(TabDefinitionsKey, tabDefinitions);
    }
}
