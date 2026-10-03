# Change Log

## Unreleased - Test Infrastructure Maintenance

No changes to the `HtmlPageBuilder` library or its public API; the library has no external package dependencies, so the published package remains at v1.2.0.

### Test Dependency Updates

- `Touchstone.Core`, `Touchstone.Cli`, `Touchstone.XunitAdapter`, `Touchstone.NunitAdapter` 0.1.12 -> 0.2.0
- `NUnit` 4.6.1 -> 5.0.0
- `NUnit.Analyzers` 4.14.0 -> 4.15.0
- `NUnit3TestAdapter` 6.2.0 -> 6.3.0
- `Microsoft.NET.Test.Sdk` 18.9.0 -> 18.10.1
- `coverlet.collector` 10.0.1 -> 10.1.0
- Validated the full shared suite through Touchstone CLI, xUnit, and NUnit on both `net8.0` and `net10.0`.

## v1.2.0 - Current

This release focuses on developer experience: fewer awkward `AddRaw(body.Helper(...))` call sites, better data-shape support, clearer fragment handling, and more ergonomic attributes.

### Fluent Body Construction

- Added fluent `Add...` methods for the common body helper surface.
- Added append helpers for headings, preformatted text, divs, spans, semantic containers, links, images, lists, tables, buttons, line breaks, and horizontal rules.
- Added `HtmlBody.Add(HtmlFragment)` and `HtmlPage.Add(HtmlFragment)` for appending explicit fragments.

### Html Context And Fragments

- Added the instance-based `Html` context with `Page()`, `Head()`, `Body()`, `Attributes()`, `Text(...)`, and `Raw(...)` entry points.
- Added `HtmlOptions` for applying defaults when an `Html` context creates pages, heads, and bodies.
- Added creation counters and disposal behavior to the `Html` context so state and lifetime have a first-class home.
- Added `HtmlFragment` for carrying generated or trusted HTML as an intentional value instead of an anonymous string.
- Added fragment concatenation and string conversion for simple composition.

### Attribute Ergonomics

- Added class composition helpers: `AddClass(...)` and `AddClassIf(...)`.
- Added style composition helpers: `AddStyle(...)` and `AddStyleIf(...)`.
- Kept attribute name validation and value encoding in place for custom attributes.

### Lists And Tables

- Updated ordered and unordered list helpers to accept `IEnumerable<string>`, not only `List<string>`.
- Added table generation from explicit headers and rows.
- Added `TableFromObjects(...)` for public-property object collections.
- Added `TableFromDictionaries(...)` for dictionary row data.
- Kept the `DataTable` helper intact for existing consumers.

### Documentation

- Rewrote the README around the recommended consumption path: fluent page building first, reusable fragments second, raw HTML escape hatches clearly marked.
- Added examples for fragments, attributes, class/style composition, enumerable lists, and table generation from multiple data shapes.

### Tests

- Expanded the shared Touchstone descriptor suite to cover the new developer-experience APIs.
- Validated the expanded suite through Touchstone CLI, xUnit, and NUnit on both `net8.0` and `net10.0`.

## v1.1.0

This release turned HtmlPageBuilder from a small snippet helper into a safer, more expressive HTML string builder while keeping the library lightweight and direct.

### Complete Document Output

- Added proper `<body>` wrapper rendering.
- `HtmlPage.ToString()` now emits a complete document structure with `<!DOCTYPE html>`, `<html>`, `<head>`, and `<body>`.
- Added optional `lang` support through `HtmlPage.WithLanguage(...)`.

### Fluent Construction

- Added fluent configuration for pages, heads, and bodies.
- Added page-level helpers such as `WithTitle(...)`, `WithHead(...)`, `WithBody(...)`, `AddH1Text(...)`, `AddParagraphText(...)`, and `AddRaw(...)`.
- Added body-level append helpers that return the body instance for chaining.

### Safer Text And Attribute Handling

- Added HTML encoding for text-oriented helpers.
- Added attribute value encoding across generated elements.
- Added `HtmlAttributes` for validated custom attributes.
- Added explicit `*Text` and `*Html` helper families so call sites show whether a value is plain text or intentional raw HTML.

### Expanded HTML Helper Surface

- Added H4, H5, and H6 helpers.
- Added span helpers.
- Added semantic container helpers for `section`, `article`, `main`, `header`, `footer`, and `nav`.
- Added generic element helpers for encoded text and raw HTML.
- Added line break support.
- Improved link generation for new-window links with `rel="noopener noreferrer"`.
- Changed button generation to use `type="button"` by default instead of injecting opinionated visual styling.

### Larger Output Support

- Reworked body content storage to use `StringBuilder`.
- Kept `Body.Content` available for compatibility while making fluent append operations the preferred path for larger generated documents.

### Target Frameworks

- Updated every project to multi-target `net8.0` and `net10.0`.
- The library package now produces builds for both supported modern .NET target frameworks.
- The Touchstone test projects run against the same target framework set as the library.

### Test Infrastructure

- Replaced the legacy manual `Test` project with a Touchstone-based test architecture.
- Added `Test.Shared` as the single source of test truth.
- Added `Test.Automated` for the Touchstone CLI runner.
- Added `Test.Xunit` and `Test.Nunit` adapters that run the same shared descriptors.
- Expanded test coverage across document rendering, head rendering, encoding, raw HTML escape hatches, attributes, invalid input, null/empty behavior, lists, tables, fluent construction, compatibility behavior, and the original sample scenario.
- Increased the shared descriptor suite to drive 100% library line coverage and materially stronger branch coverage under the xUnit coverage run.

### Documentation

- Rewrote the README around the developer experience: what the library is, when to use it, when not to use it, safe usage patterns, examples, tests, support, and license.

## v1.0.0

Initial release.

- Created the core `HtmlPage`, `HtmlHead`, and `HtmlBody` types.
- Added simple helpers for titles, metadata, headings, paragraphs, divs, links, images, ordered and unordered lists, tables, buttons, and horizontal rules.
- Published the initial package as a lightweight way to create HTML strings from C#.
