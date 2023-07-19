namespace V2ex.Maui.Services;

public static class InstanceActivator
{
    public static IServiceProvider? ServiceProvider { get; set; }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public static T Create<T>(params object[] parameters)
        where T : notnull
    {
        if (ServiceProvider == null)
        {
            throw new InvalidOperationException("Activator is not initialized");
        }
        return ActivatorUtilities.CreateInstance<T>(ServiceProvider, parameters);
    }

    public static T Create<T>()
        where T : notnull
    {
        if (ServiceProvider == null)
        {
            throw new InvalidOperationException("Activator is not initialized");
        }
        return ServiceProvider.GetRequiredService<T>();
    }
}