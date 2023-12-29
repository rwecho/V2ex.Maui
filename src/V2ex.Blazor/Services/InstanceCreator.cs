
namespace V2ex.Blazor.Services;

public class InstanceCreator
{
    private static IServiceProvider ServiceProvider { get; set; } = null!;
    internal static void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public static T Create<T>()
        where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}
