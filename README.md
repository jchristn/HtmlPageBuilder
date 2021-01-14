![alt tag](https://github.com/jchristn/htmlpagebuilder/blob/master/assets/icon.ico)

# HtmlPageBuilder

## Description

HtmlPageBuilder is a simple library that allows you to build HTML strings including head and body components.

For a sample app exercising this library, refer to the ```Test``` project contained within the solution.

## New in v1.0.0

- Initial release

## Simple Example

```csharp
HtmlPage p = new HtmlPage();
p.Head.Title = "My Page";
p.Head.MetaKeywords = "my,page";
p.Head.Style = "h1 { font-family: 'arial' } p { font-family: 'arial' } ul { font-family: 'arial' }";
p.Body.Content += _Page.Body.H1Text("My Page");
p.Body.Content += _Page.Body.Paragraph("This is some sample text.");
p.Body.Content += _Page.Body.UnorderedList(new List<string> { "foo", "bar", "baz" });
Console.WriteLine(p.ToString());
```

## Version history

Refer to CHANGELOG.md.
