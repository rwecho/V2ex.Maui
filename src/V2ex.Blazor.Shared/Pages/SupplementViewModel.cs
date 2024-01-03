using Microsoft.AspNetCore.Components;

namespace V2ex.Blazor.Pages;

public record SupplementViewModel(
    string? CreatedText,
    MarkupString? Content
    )
{ }

