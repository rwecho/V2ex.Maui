using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using V2ex.Maui.Services;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Pages;

public partial class SearchPageViewModel : BaseViewModel, ITransientDependency
{
    public SearchPageViewModel()
    {
    }

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private int _took, _pageSize = 50, _currentPage, _maximumPage, _total;

    [ObservableProperty]
    private ObservableCollection<SearchItemViewModel> _searchItems = new();


    protected override Task OnLoad(CancellationToken cancellationToken)
    {

        return Task.CompletedTask;
    }

    [RelayCommand]
    public async Task Search(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.SearchText))
        {
            return;
        }

        this.CurrentPage = 0;
        var result = await this.ApiService.Search(this.SearchText, this.CurrentPage * this.PageSize);

        this.MaximumPage = 0;
        this.Took = 0;
        this.SearchItems.Clear();
        if (result == null)
        {
            return;
        }
        this.MaximumPage = (int)Math.Ceiling(result.Total / (this.PageSize * 1.0));
        this.Took = result.Took;
        this.Total = result.Total;

        if (result.Hits != null)
        {
            foreach (var hit in result.Hits)
            {
                var source = hit.Source;
                // notes: remove the newline to compact the string.
                var content = source.Content.Replace("\r\n", "");

                var item = InstanceActivator.Create<SearchItemViewModel>();
                item.Content = content;
                item.Created = source.Created;
                item.Creator = source.Creator;
                item.Id = source.Id;
                item.Title = source.Title;
                item.Replies = source.Replies;

                this.SearchItems.Add(item);
            }
        }
    }

    [RelayCommand]
    public async Task RemainingReached(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(this.SearchText))
        {
            return;
        }

        if (this.CurrentPage >= this.MaximumPage) return;

        this.CurrentPage += 1;
        var result = await this.ApiService.Search(this.SearchText, this.CurrentPage * this.PageSize);

        if (result == null)
        {
            return;
        }
        this.MaximumPage = (int)Math.Ceiling(result.Total / (this.PageSize * 1.0));
        this.Took = result.Took;
        this.Total = result.Total;

        if (result.Hits != null)
        {
            foreach (var hit in result.Hits)
            {
                var source = hit.Source;
                // notes: remove the newline to compact the string.
                var content = source.Content.Replace("\r\n", "");

                var item = InstanceActivator.Create<SearchItemViewModel>();
                item.Content = content;
                item.Created = source.Created;
                item.Creator = source.Creator;
                item.Id = source.Id;
                item.Title = source.Title;
                item.Replies = source.Replies;

                this.SearchItems.Add(item);
            }
        }
    }
}