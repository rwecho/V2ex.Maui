namespace V2ex.Maui.Styles;

public static class StyleExtensionMethods
{
    public static void Compound(this Style style1, Style style2)
    {
        ArgumentNullException.ThrowIfNull(style1);
        ArgumentNullException.ThrowIfNull(style2);

        if (style2.BasedOn != null)
        {
            Compound(style1, style2.BasedOn);
        }

        foreach (Setter setter in style2.Setters)
        {
            var existSetter = style1.Setters.FirstOrDefault(o => o.Property.PropertyName == setter.Property.PropertyName);

            if (existSetter != null)
            {
                style1.Setters.Remove(existSetter);
            }
            style1.Setters.Add(setter);
        }

        foreach (TriggerBase trigger in style2.Triggers)
        {
            style1.Triggers.Add(trigger);
        }

        foreach (Behavior behavior in style2.Behaviors)
        {
            style1.Behaviors.Add(behavior);
        }
    }
}