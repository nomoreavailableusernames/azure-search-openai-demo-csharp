// Copyright (c) Microsoft. All rights reserved.

using Shared.Models;

namespace ClientApp.Tests;

#pragma warning disable CA1416 // Validate platform compatibility

public class SupportingContentParserTests
{
    public static IEnumerable<object[]> ParserInput
    {
        get
        {
            yield return new object[]
            {
                new SupportingContentRecord("test.pdf", "url", "blah blah"),
                "test.pdf",
                "url",
                "blah blah",
            };

            yield return new object[]
            {
                new SupportingContentRecord("sdp_corporate.pdf", "url", "this is the content that follows"),
                "sdp_corporate.pdf",
                "url",
                "this is the content that follows",
            };
        }
    }

    [Theory, MemberData(nameof(ParserInput))]
    public void SupportingContentCorrectlyParsesText(
        SupportingContentRecord supportingContent,
        string expectedTitle,
        string expectedBaseUrl,
        string? expectedContent)
    {
        var actual = SupportingContent.ParseSupportingContent(supportingContent);
        var expected = new ParsedSupportingContentItem(expectedTitle, expectedBaseUrl, expectedContent);
        Assert.Equal(actual, expected);
    }
}


#pragma warning restore CA1416 // Validate platform compatibility