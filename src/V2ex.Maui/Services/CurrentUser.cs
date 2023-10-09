using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class CurrentUser : ICurrentUser, ITransientDependency
{
    private UserManager UserManager { get; } = InstanceActivator.Create<UserManager>();
    public string? Name => this.UserManager.CurrentUser?.Name;

    public bool IsAuthorized()
    {
        return this.Name != null;
    }
}
