![HtmlPageBuilder icon](https://raw.githubusercontent.com/jchristn/HtmlPageBuilder/main/assets/icon.ico)

# HtmlPageBuilder

[![NuGet Version](https://img.shields.io/nuget/v/HtmlPageBuilder.svg?style=flat)](https://www.nuget.org/packages/HtmlPageBuilder/) [![NuGet Downloads](https://img.shields.io/nuget/dt/HtmlPageBuilder.svg)](https://www.nuget.org/packages/HtmlPageBuilder)

HtmlPageBuilder is a small C# library for building HTML strings without dragging in a web framework, template engine, or view runtime.

Use it when you need to generate a modest HTML page or fragment from code: a build report, diagnostics endpoint, status page, admin utility, email-like snippet, test artifact, or simple server response. The API is intentionally direct: create a page, configure the head, append body elements, and render the result as a string.

## Why It Exists

Hand-concatenating HTML strings is fast at first and painful soon after. Full templating stacks are excellent, but often too heavy for small generated outputs.

HtmlPageBuilder sits in that gap:

- Structured enough to avoid unreadable string soup.
- Small enough to stay close to the code producing the data.
- Safe by default for text and attributes.
- Explicit when you intentionally embed raw HTML.
- Fluent when you are building a page, string-returning when you need reusable fragments.
- Instance-based, so options, state, and lifetime can belong to a specific `Html` context.

## Install

```bash
dotnet add package HtmlPageBuilder
```

## Quick Start

Prefer the fluent append API for ordinary page construction:

```csharp
using HtmlPageBuilder;

Html builder = new Html();

string html = builder.Page()
    .WithLanguage("en")
    .WithTitle("Build Report")
    .WithHead(head => head
        .WithMetaDescription("Nightly build report")
        .WithStyle("body { font-family: Arial, sans-serif; }"))
    .WithBody(body => body
        .AddH1Text("Build Report")
        .AddParagraphText("Generated successfully.")
        .AddUnorderedList(new[] { "Restore completed", "Tests passed", "Package created" }))
    .ToString();
```

The result is a complete document with `<!DOCTYPE html>`, `<html>`, `<head>`, and `<body>`.

## Html Is An Instance

`Html` is intentionally a class, not a static utility. Create an instance and use it as the factory/context for the pages, bodies, attributes, and fragments you build:

```csharp
Html builder = new Html();

HtmlPage page = builder.Page();
HtmlBody body = builder.Body();
HtmlAttributes attributes = builder.Attributes();
```

That gives the library a place to put configuration, state, and lifetime without introducing global behavior. Today the instance tracks created object counts, supports disposal, and applies options to newly created pages, heads, and bodies:

```csharp
HtmlOptions options = new HtmlOptions
{
    DefaultLanguage = "en",
    DefaultTitle = "Operations",
    DefaultBodyClass = "report"
};

Html builder = new Html(options);

string html = builder.Page()
    .WithBody(body => body.AddParagraphText("Ready"))
    .ToString();
```

If your application needs different defaults in different places, create separate `Html` instances.

## The Mental Model

HtmlPageBuilder has two main usage styles.

Use fluent `Add...` methods when building a document:

```csharp
Html builder = new Html();

HtmlBody body = builder.Body()
    .AddH1Text("Status")
    .AddParagraphText("All systems nominal.")
    .AddHorizontalRule();
```

Use string-returning helpers when composing reusable fragments:

```csharp
Html builder = new Html();
HtmlBody html = builder.Body();

string link = html.LinkText("Repository", "https://github.com/jchristn/HtmlPageBuilder", newWindow: true);
string card = html.DivHtml(
    html.H2Text("Resources") +
    html.ParagraphHtml(link),
    classId: "card");
```

The fluent path is the recommended default. The string-returning path is there when you need to compose, reuse, or nest fragments manually.

## Text vs Raw HTML

This is the most important convention in the library.

Use `*Text` methods for plain text. Text is HTML encoded:

```csharp
Html builder = new Html();

string safe = builder.Body().ParagraphText("5 < 10 & 10 > 5");
```

Output:

```html
<p>5 &lt; 10 &amp; 10 &gt; 5</p>
```

Use `*Html` methods only when the value is already trusted HTML:

```csharp
Html builder = new Html();

string rich = builder.Body().ParagraphHtml("Click <strong>Deploy</strong> when ready.");
```

HtmlPageBuilder encodes text and attributes. It does not sanitize untrusted HTML. If the input is user-authored markup, sanitize it before passing it to a `*Html` method.

For compatibility, `Paragraph(...)` and `Div(...)` remain raw HTML helpers. In new code, prefer `ParagraphText(...)`, `ParagraphHtml(...)`, `DivText(...)`, and `DivHtml(...)` because the intent is visible at the call site.

## Fragments

Use `HtmlFragment` when you want to carry "this is already HTML" through your own code without reducing everything to an anonymous string.

```csharp
Html builder = new Html();

HtmlFragment fragment =
    builder.Text("A < B") +
    builder.Raw("<br>") +
    builder.Text("B > A");

string html = builder.Page()
    .WithTitle("Fragment Example")
    .Add(fragment)
    .ToString();
```

`builder.Text(...)` encodes. `builder.Raw(...)` preserves the input.

## Attributes, Classes, And Styles

Simple helpers accept `id`, `classId`, and `style` parameters:

```csharp
Html builder = new Html();

string heading = builder.Body().H1Text("Dashboard", id: "title", classId: "page-title");
```

Use `HtmlAttributes` when you need custom attributes, conditional classes, or composed styles:

```csharp
Html builder = new Html();

HtmlAttributes attributes = builder.Attributes()
    .Id("status")
    .AddClass("card")
    .AddClassIf(isWarning, "warning")
    .AddStyle("display", "block")
    .AddStyleIf(isWarning, "border-color", "red")
    .Data("state", "ready")
    .Aria("label", "Build status");

string panel = builder.Body().ElementText("section", "Ready", attributes: attributes);
```

Attribute names are validated. Attribute values are encoded.

## Lists

List helpers accept `IEnumerable<string>`, so arrays, lists, and LINQ projections all work:

```csharp
Html builder = new Html();

string list = builder.Body().UnorderedList(new[] { "one", "two", "three" });
```

Use the `Html` variants when list item content is trusted HTML:

```csharp
Html builder = new Html();

string list = builder.Body().UnorderedListHtml(new[] { "<strong>Ready</strong>" });
```

## Tables

`DataTable` is supported:

```csharp
using System.Data;
using HtmlPageBuilder;

Html builder = new Html();

DataTable table = new DataTable();
table.Columns.Add("Name");
table.Columns.Add("Status");
table.Rows.Add("Restore", "Passed");
table.Rows.Add("Tests", "Passed");

string html = builder.Body().Table(table, classId: "results");
```

Headers and rows can be supplied directly:

```csharp
Html builder = new Html();

string html = builder.Body().Table(
    new[] { "Name", "Status" },
    new[]
    {
        new[] { "Restore", "Passed" },
        new[] { "Tests", "Passed" },
    });
```

Dictionaries and objects are supported through named helpers:

```csharp
Html builder = new Html();

string fromObjects = builder.Body().TableFromObjects(buildRows);
string fromDictionaries = builder.Body().TableFromDictionaries(dictionaryRows);
```

These are named instead of one-argument `Table(...)` overloads so existing `Table(null)` and `DataTable` call sites stay unambiguous.

## Common Body Helpers

The body API includes fluent append methods and string-returning helpers for:

- Headings: `H1` through `H6`
- Paragraphs and preformatted text
- `div` and `span`
- Semantic containers: `section`, `article`, `main`, `header`, `footer`, `nav`
- Links and images
- Ordered and unordered lists
- Tables
- Buttons
- Line breaks and horizontal rules
- Generic elements through `ElementText(...)` and `ElementHtml(...)`

For links opened in a new window, HtmlPageBuilder adds `rel="noopener noreferrer"`. Buttons default to `type="button"`.

## When To Use It

Use HtmlPageBuilder when:

- The HTML is generated from runtime data.
- The output is small or medium-sized.
- You want the generation logic near the data-producing code.
- A full view engine would be unnecessary overhead.
- You need deterministic HTML strings in tests or tools.

## When Not To Use It

Use something else when you need:

- Large public websites.
- Componentized frontends.
- Designer-authored templates.
- Layout inheritance.
- Client-side interactivity.
- Sanitization of untrusted rich HTML.

HtmlPageBuilder is a builder, not a sanitizer or frontend framework.

## Testing

The test suite uses Touchstone as the shared source of truth:

- `Test.Shared` defines the reusable test descriptors.
- `Test.Automated` runs the descriptors through the Touchstone CLI runner.
- `Test.Xunit` adapts the same descriptors to xUnit.
- `Test.Nunit` adapts the same descriptors to NUnit (NUnit 5).

The shared suite exercises document rendering, fluent construction, encoded text paths, raw HTML paths, fragments, context helpers, custom attributes, invalid input, null/empty behavior, lists, tables, legacy compatibility helpers, and target-framework coverage for both `net8.0` and `net10.0`.

Run all tests:

```bash
dotnet test src/HtmlPageBuilder.sln
```

Run the Touchstone CLI runner:

```bash
dotnet run --project src/Test.Automated/Test.Automated.csproj
```

Collect coverage with the xUnit adapter:

```bash
dotnet test src/Test.Xunit/Test.Xunit.csproj -f net8.0 --collect:"XPlat Code Coverage"
```

## Design Goals

HtmlPageBuilder aims to stay:

- Small: easy to understand and easy to drop into utility code.
- Explicit: text and raw HTML paths should be obvious.
- Safe by default: plain text and attributes should encode correctly.
- Fluent where it helps: common page construction should read top-to-bottom.
- Composable: fragments should be easy to reuse without inventing a framework.
- Escape-hatch friendly: unsupported tags and raw HTML should still be possible.

## Issues And Discussions

Bug reports, enhancement requests, and API design feedback are welcome.

- File issues at https://github.com/jchristn/HtmlPageBuilder/issues.
- Start or join discussions at https://github.com/jchristn/HtmlPageBuilder/discussions.
- For contribution guidance, see [CONTRIBUTING.md](CONTRIBUTING.md).

If Discussions are not enabled, open an issue with a question or design label. When filing an issue, include the target framework, package version, expected HTML, actual HTML, and a minimal code sample that reproduces the behavior.

## License

HtmlPageBuilder is released under the MIT license. See [LICENSE.md](LICENSE.md).

## Version History

See [CHANGELOG.md](CHANGELOG.md).
