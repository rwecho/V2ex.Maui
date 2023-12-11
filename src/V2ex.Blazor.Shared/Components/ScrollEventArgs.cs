namespace V2ex.Blazor.Components;

public class ScrollEventArgs(double scrollTop, double scrollHeight, double clientHeight) : EventArgs
{
    public double ScrollTop => scrollTop;
    public double ScrollHeight => scrollHeight;
    public double ClientHeight => clientHeight;
}