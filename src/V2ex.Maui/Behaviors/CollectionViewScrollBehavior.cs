namespace V2ex.Maui.Behaviors;

public class CollectionViewScrollBehavior : Behavior<CollectionView>
{
    protected override void OnAttachedTo(CollectionView bindable)
    {
        bindable.Scrolled += CollectionView_Scrolled;
        _parentPage = FindAncestorPage(bindable);
        base.OnAttachedTo(bindable);
    }

    private bool _isAnimating = false;
    private IDispatcherTimer? _debounceTimer;
    private ContentPage _parentPage = null!;

    private void CollectionView_Scrolled(object? sender, ItemsViewScrolledEventArgs e)
    {
        // ref: https://stackoverflow.com/questions/76767407/how-to-implement-title-bar-hide-show-when-scrollbar-up-down-in-android/76937302#76937302

        var collectionView = (CollectionView)sender!;
        var delta = e.VerticalDelta;
        var offset = e.VerticalOffset;
        if (_isAnimating || delta == 0)
        {
            return;
        }

        _debounceTimer?.Stop();
        _debounceTimer = Application.Current!.Dispatcher.CreateTimer();
        _debounceTimer.Interval = TimeSpan.FromMilliseconds(40);
        _debounceTimer.Tick += (s, eventArgs) =>
        {
            _isAnimating = true;
            if (delta > 0)
            {
                HideNavigationBar();
            }
            else if (delta < 0)
            {
                ShowNavigationBar();
            }

            _isAnimating = false;
            _debounceTimer.Stop();
        };
        _debounceTimer.Start();
    }

    private void HideNavigationBar()
    {
        Shell.SetNavBarIsVisible(_parentPage, false);
    }

    private void ShowNavigationBar()
    {

        Shell.SetNavBarIsVisible(_parentPage, true);
    }

    private static ContentPage FindAncestorPage(CollectionView collectionView)
    {
        var parent = collectionView.Parent;
        while (parent is not Page)
        {
            parent = parent?.Parent;
        }

        if (parent == null)
        {
            throw new InvalidOperationException("ContentPage not found.");
        }

        return (ContentPage)parent;
    }

    protected override void OnDetachingFrom(CollectionView bindable)
    {
        bindable.Scrolled -= CollectionView_Scrolled;
        base.OnDetachingFrom(bindable);
    }
}
