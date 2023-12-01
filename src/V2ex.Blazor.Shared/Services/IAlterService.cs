namespace V2ex.Blazor.Services;

public interface IAlterService
{
    Task<bool> Confirm(string title, string message, string accept = "确定", string cancel = "取消");
}