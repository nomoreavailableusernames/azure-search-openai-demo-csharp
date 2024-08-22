// Copyright (c) Microsoft. All rights reserved.

namespace SharedWebComponents.Components;

public sealed partial class SupportingContent
{
    internal static ParsedSupportingContentItem ParseSupportingContent(SupportingContentRecord item)
    {
        // Assumes the item starts with the file name followed by : and the content.
        // Example: "sdp_corporate.pdf: this is the content that follows".
        var title = item.Title;
        var content = item.Content;
        var baseUrl = item.BaseUrl;

        return new ParsedSupportingContentItem(title, baseUrl, content.Trim());
    }
}

internal readonly record struct ParsedSupportingContentItem(
    string Title,
    string? BaseUrl = null,
    string? Content = null)
{
    internal bool IsEmpty =>
        string.IsNullOrWhiteSpace(Title) ||
        string.IsNullOrWhiteSpace(BaseUrl) ||
        string.IsNullOrWhiteSpace(Content);
}