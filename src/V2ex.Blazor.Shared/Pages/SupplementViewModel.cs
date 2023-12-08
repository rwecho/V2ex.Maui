using Microsoft.AspNetCore.Components;

namespace V2ex.Blazor.Pages;

public record SupplementViewModel(
    DateTime Created,
    string CreatedText,
    MarkupString? Content
    )
{ }

