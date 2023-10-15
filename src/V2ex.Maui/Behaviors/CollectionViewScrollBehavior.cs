﻿namespace V2ex.Maui.Behaviors;

public class CollectionViewScrollBehavior : Behavior<CollectionView>
{
    public Page ParentPage
    {
        get { return (Page)GetValue(ParentPageProperty); }
        set { SetValue(ParentPageProperty, value); }
    }

    public static readonly BindableProperty ParentPageProperty =
        BindableProperty.Create(nameof(ParentPage), typeof(Page), typeof(CollectionViewScrollBehavior));

    protected override void OnAttachedTo(CollectionView bindable)
    {
        base.OnAttachedTo(bindable);
        // todo: disable the scroll behavior when it became more effectively.
        //bindable.Scrolled += CollectionView_Scrolled;
    }

    private bool _isAnimating = false;
    private IDispatcherTimer? _debounceTimer;

    private void CollectionView_Scrolled(object? sender, ItemsViewScrolledEventArgs e)
    {
        // ref: https://stackoverflow.com/questions/76767407/how-to-implement-title-bar-hide-show-when-scrollbar-up-down-in-android/76937302#76937302
        var collectionView = (CollectionView)sender!;
        var delta = e.VerticalDelta;
        var offset = e.VerticalOffset;
        if (_isAnimating || delta == 0 || Math.Abs(delta) < 50)
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
        if(ParentPage == null)
        {
            throw new InvalidOperationException("Parent page can not binding.");
        }
        Shell.SetNavBarIsVisible(ParentPage, false);
    }

    private void ShowNavigationBar()
    {
        if (ParentPage == null)
        {
            throw new InvalidOperationException("Parent page can not binding.");
        }
        Shell.SetNavBarIsVisible(ParentPage, true);
    }

    protected override void OnDetachingFrom(CollectionView bindable)
    {
        // todo: disable the scroll behavior when it became more effectively.
        //bindable.Scrolled -= CollectionView_Scrolled;
        base.OnDetachingFrom(bindable);
    }
}
