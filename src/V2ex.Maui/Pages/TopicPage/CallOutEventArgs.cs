namespace V2ex.Maui.Pages;

public class CallOutEventArgs : EventArgs
{
    public CallOutEventArgs(string owner, string target, int floor)
    {
        this.Owner = owner;
        this.Target = target;
        this.Floor = floor;
    }

    public string Owner { get; }
    public string Target { get; }
    public int Floor { get; }
}
