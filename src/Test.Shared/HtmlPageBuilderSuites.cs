namespace Test.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using HtmlPageBuilder;
    using Touchstone.Core;

    public static class HtmlPageBuilderSuites
    {
        public static IReadOnlyList<TestSuiteDescriptor> All
        {
            get
            {
                return new List<TestSuiteDescriptor>
                {
                    PageSuite(),
                    HeadSuite(),
                    BodyEncodingSuite(),
                    BodyHelperSuite(),
                    ListAndTableSuite(),
                    AttributeSuite(),
                    DeveloperExperienceSuite(),
                    CompatibilitySuite(),
                };
            }
        }

        private static TestSuiteDescriptor PageSuite()
        {
            return Suite("Page", "Page rendering", new List<TestCaseDescriptor>
            {
                Case("Page", "DefaultDocumentHasBodyWrapper", "Default document includes head and body wrappers", () =>
                {
                    string html = new HtmlPage().ToString();
                    AssertContains(html, "<head>");
                    AssertContains(html, "</head><body>");
                    AssertContains(html, "</body></html>");
                    AssertNotContains(html, "</head></html>");
                }),

                Case("Page", "LanguageAttributeIsEncoded", "Page language attribute is encoded", () =>
                {
                    string html = new HtmlPage().WithLanguage("en\"us").ToString();
                    AssertContains(html, "<html lang=\"en&quot;us\">");
                }),

                Case("Page", "FluentPageChaining", "Page supports fluent title and body construction", () =>
                {
                    string html = new HtmlPage()
                        .WithLanguage("en")
                        .WithTitle("Title & More")
                        .AddH1Text("Heading <safe>")
                        .AddParagraphText("Body & text")
                        .ToString();

                    AssertContains(html, "<html lang=\"en\">");
                    AssertContains(html, "<title>Title &amp; More</title>");
                    AssertContains(html, "<h1>Heading &lt;safe&gt;</h1>");
                    AssertContains(html, "<p>Body &amp; text</p>");
                }),

                Case("Page", "WithHeadAndWithBodyCallbacks", "Page supports fluent callback configuration", () =>
                {
                    string html = new HtmlPage()
                        .WithHead(h => h.WithTitle("Callback"))
                        .WithBody(b => b.AddParagraphText("Configured"))
                        .ToString();

                    AssertContains(html, "<title>Callback</title>");
                    AssertContains(html, "<p>Configured</p>");
                }),

                Case("Page", "AddRawAppendsRawHtml", "Page AddRaw appends raw body HTML", () =>
                {
                    string html = new HtmlPage()
                        .AddRaw("<section><strong>raw</strong></section>")
                        .ToString();

                    AssertContains(html, "<body><section><strong>raw</strong></section></body>");
                }),

                Case("Page", "EmptyLanguageIsOmitted", "Empty page language is omitted", () =>
                {
                    string html = new HtmlPage().WithLanguage("").ToString();
                    AssertContains(html, "<html>");
                    AssertNotContains(html, "lang=");
                }),

                Case("Page", "NullConfigurationCallbacksThrow", "Null page configuration callbacks throw", () =>
                {
                    HtmlPage page = new HtmlPage();
                    AssertThrows<ArgumentNullException>(() => page.WithHead(null));
                    AssertThrows<ArgumentNullException>(() => page.WithBody(null));
                }),
            });
        }

        private static TestSuiteDescriptor HeadSuite()
        {
            return Suite("Head", "Head rendering", new List<TestCaseDescriptor>
            {
                Case("Head", "DefaultMetaTags", "Head emits charset and viewport defaults", () =>
                {
                    string html = new HtmlHead().ToString();
                    AssertContains(html, "<meta charset=\"UTF-8\">");
                    AssertContains(html, "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                }),

                Case("Head", "EncodesHeadValues", "Head text and attributes are encoded", () =>
                {
                    HtmlHead head = new HtmlHead()
                        .WithTitle("<Title & \"quoted\">")
                        .WithFavicon("/icon\".ico")
                        .WithBaseUrl("https://example.com/?q=\"x\"")
                        .WithMetaDescription("A <description> & more")
                        .WithMetaKeywords("one,two")
                        .WithMetaAuthor("A & B");

                    string html = head.ToString();
                    AssertContains(html, "<title>&lt;Title &amp; &quot;quoted&quot;&gt;</title>");
                    AssertContains(html, "href=\"/icon&quot;.ico\"");
                    AssertContains(html, "<base href=\"https://example.com/?q=&quot;x&quot;\">");
                    AssertContains(html, "content=\"A &lt;description&gt; &amp; more\"");
                    AssertContains(html, "content=\"one,two\"");
                    AssertContains(html, "content=\"A &amp; B\"");
                }),

                Case("Head", "StyleRemainsRawCss", "Style content is preserved as raw CSS", () =>
                {
                    string html = new HtmlHead().WithStyle("body > main { color: #111; }").ToString();
                    AssertContains(html, "<style>");
                    AssertContains(html, "body > main { color: #111; }");
                }),

                Case("Head", "AdditionalRawElementsRemainRaw", "Raw head extension point remains available", () =>
                {
                    string html = new HtmlHead()
                        .AddRawElement("<meta property=\"og:title\" content=\"Raw <value>\">")
                        .ToString();

                    AssertContains(html, "<meta property=\"og:title\" content=\"Raw <value>\">");
                }),

                Case("Head", "StylesheetAndScriptHelpersEncodeAttributes", "Stylesheet and script helpers encode attributes", () =>
                {
                    string html = new HtmlHead()
                        .AddStylesheet("/site.css?theme=\"dark\"", "screen")
                        .AddScriptSource("/app.js?x=\"1\"", true)
                        .ToString();

                    AssertContains(html, "<link rel=\"stylesheet\" href=\"/site.css?theme=&quot;dark&quot;\" media=\"screen\">");
                    AssertContains(html, "<script src=\"/app.js?x=&quot;1&quot;\" defer></script>");
                }),

                Case("Head", "ContentPropertyMatchesToString", "Head Content returns the rendered head", () =>
                {
                    HtmlHead head = new HtmlHead().WithTitle("Same");
                    AssertEqual(head.ToString(), head.Content);
                }),

                Case("Head", "EmptyOptionalValuesAreOmitted", "Empty optional head values are omitted", () =>
                {
                    HtmlHead head = new HtmlHead
                    {
                        Title = "",
                        FaviconUrl = "",
                        Style = "",
                        BaseUrl = "",
                        MetaCharset = "",
                        MetaViewport = "",
                        MetaKeywords = "",
                        MetaDescription = "",
                        MetaAuthor = "",
                    };

                    string html = head.ToString();
                    AssertEqual("<head></head>", html);
                }),

                Case("Head", "ScriptWithoutDeferOmitsDefer", "Script helper omits defer when not requested", () =>
                {
                    string html = new HtmlHead().AddScriptSource("/app.js").ToString();
                    AssertContains(html, "<script src=\"/app.js\"></script>");
                    AssertNotContains(html, " defer");
                }),

                Case("Head", "NullRawHeadElementIsIgnored", "Null raw head elements are ignored", () =>
                {
                    HtmlHead head = new HtmlHead().AddRawElement(null);
                    AssertEqual(0, head.AdditionalElements.Count);
                }),

                Case("Head", "NullAdditionalElementEntriesAreSkipped", "Null entries in AdditionalElements are skipped during rendering", () =>
                {
                    HtmlHead head = new HtmlHead();
                    head.AdditionalElements.Add(null);
                    head.AdditionalElements.Add("<meta name=\"x\" content=\"y\">");

                    AssertEqual("<head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><meta name=\"x\" content=\"y\"></head>", head.ToString());
                }),

                Case("Head", "FluentHeadMethodsReturnSameInstance", "Head fluent methods return the same instance", () =>
                {
                    HtmlHead head = new HtmlHead();
                    AssertSame(head, head.WithTitle("t"));
                    AssertSame(head, head.WithFavicon("/favicon.ico"));
                    AssertSame(head, head.WithStyle("body{}"));
                    AssertSame(head, head.WithBaseUrl("https://example.com/"));
                    AssertSame(head, head.WithMetaDescription("d"));
                    AssertSame(head, head.WithMetaKeywords("k"));
                    AssertSame(head, head.WithMetaAuthor("a"));
                    AssertSame(head, head.AddStylesheet("/site.css"));
                    AssertSame(head, head.AddScriptSource("/app.js"));
                    AssertSame(head, head.AddRawElement("<meta name=\"x\" content=\"y\">"));
                }),
            });
        }

        private static TestSuiteDescriptor BodyEncodingSuite()
        {
            return Suite("BodyEncoding", "Body encoding", new List<TestCaseDescriptor>
            {
                Case("BodyEncoding", "ContentPropertyIsInnerHtml", "Content property stores body inner HTML", () =>
                {
                    HtmlBody body = new HtmlBody();
                    body.Content = "<p>raw</p>";
                    AssertEqual("<p>raw</p>", body.Content);
                    AssertEqual("<body><p>raw</p></body>", body.ToString());
                }),

                Case("BodyEncoding", "AddTextEncodes", "Fluent text append encodes text", () =>
                {
                    string html = new HtmlBody().AddText("<raw & text>").ToString();
                    AssertContains(html, "&lt;raw &amp; text&gt;");
                    AssertNotContains(html, "<raw & text>");
                }),

                Case("BodyEncoding", "ParagraphTextEncodes", "ParagraphText encodes plain text", () =>
                {
                    AssertEqual("<p>Hello &lt;b&gt;world&lt;/b&gt; &amp; all</p>", new HtmlBody().ParagraphText("Hello <b>world</b> & all"));
                }),

                Case("BodyEncoding", "ParagraphHtmlKeepsRawHtml", "ParagraphHtml keeps nested HTML raw", () =>
                {
                    AssertEqual("<p>Hello <strong>world</strong></p>", new HtmlBody().ParagraphHtml("Hello <strong>world</strong>"));
                }),

                Case("BodyEncoding", "LegacyParagraphIsRawHtml", "Paragraph remains a raw HTML compatibility helper", () =>
                {
                    AssertEqual("<p><a href=\"/\">Home</a></p>", new HtmlBody().Paragraph("<a href=\"/\">Home</a>"));
                }),

                Case("BodyEncoding", "HeadingAndPreformattedEncodeText", "Heading and preformatted helpers encode text", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<h1>&lt;h1&gt;</h1>", body.H1Text("<h1>"));
                    AssertEqual("<h2>A &amp; B</h2>", body.H2Text("A & B"));
                    AssertEqual("<h3>&quot;Q&quot;</h3>", body.H3Text("\"Q\""));
                    AssertEqual("<pre>&lt;xml&gt;</pre>", body.Preformatted("<xml>"));
                }),

                Case("BodyEncoding", "LinkTextEncodesTextAndAttributes", "Link helper encodes text and URL attributes", () =>
                {
                    string html = new HtmlBody().LinkText("<home>", "/?q=\"x\"", true, "id\"1", "nav", "color:\"red\"");
                    AssertContains(html, "href=\"/?q=&quot;x&quot;\"");
                    AssertContains(html, "target=\"_blank\"");
                    AssertContains(html, "rel=\"noopener noreferrer\"");
                    AssertContains(html, "id=\"id&quot;1\"");
                    AssertContains(html, "style=\"color:&quot;red&quot;\"");
                    AssertContains(html, ">&lt;home&gt;</a>");
                }),

                Case("BodyEncoding", "LinkHtmlAllowsRawContent", "LinkHtml keeps display HTML raw", () =>
                {
                    string html = new HtmlBody().LinkHtml("<span>Home</span>", "/");
                    AssertEqual("<a href=\"/\"><span>Home</span></a>", html);
                }),

                Case("BodyEncoding", "ImageEncodesAttributes", "Image helper encodes src and alt attributes", () =>
                {
                    string html = new HtmlBody().Image("An \"image\"", "/img.png?x=\"1\"");
                    AssertEqual("<img src=\"/img.png?x=&quot;1&quot;\" alt=\"An &quot;image&quot;\">", html);
                }),

                Case("BodyEncoding", "ButtonTextEncodesAndUsesButtonType", "Button helper encodes text and sets a safe type default", () =>
                {
                    string html = new HtmlBody().ButtonText("<Save>");
                    AssertEqual("<button type=\"button\">&lt;Save&gt;</button>", html);
                }),

                Case("BodyEncoding", "ButtonHtmlAllowsRawContent", "ButtonHtml keeps display HTML raw", () =>
                {
                    string html = new HtmlBody().ButtonHtml("<span>Save</span>");
                    AssertEqual("<button type=\"button\"><span>Save</span></button>", html);
                }),

                Case("BodyEncoding", "RawHeadingHelpersKeepHtml", "Raw heading helpers keep nested HTML", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<h1><em>one</em></h1>", body.H1Html("<em>one</em>"));
                    AssertEqual("<h2><em>two</em></h2>", body.H2Html("<em>two</em>"));
                    AssertEqual("<h3><em>three</em></h3>", body.H3Html("<em>three</em>"));
                }),

                Case("BodyEncoding", "NullContentAndRawValuesAreHandled", "Null content clears or no-ops predictably", () =>
                {
                    HtmlBody body = new HtmlBody();
                    body.Content = "<p>before</p>";
                    body.Content = null;
                    AssertEqual("", body.Content);
                    AssertEqual("<body></body>", body.AddRaw(null).ToString());
                    AssertEqual("<p></p>", body.ParagraphText(null));
                    AssertEqual("<p></p>", body.ParagraphHtml(null));
                    AssertEqual("<span></span>", body.SpanHtml(null));
                }),

                Case("BodyEncoding", "LinkWithoutNewWindowOmitsTargetAndRel", "Normal links omit target and rel", () =>
                {
                    string html = new HtmlBody().Link("Home", "/");
                    AssertEqual("<a href=\"/\">Home</a>", html);
                    AssertNotContains(html, "target=");
                    AssertNotContains(html, "rel=");
                }),

                Case("BodyEncoding", "RawLinkAndButtonNullContentRenderEmpty", "Raw link and button helpers handle null content", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<a href=\"/\"></a>", body.LinkHtml(null, "/"));
                    AssertEqual("<button type=\"button\"></button>", body.ButtonHtml(null));
                }),

                Case("BodyEncoding", "ImageOmitsEmptyOptionalAttributes", "Image omits empty optional attributes", () =>
                {
                    string html = new HtmlBody().Image(null, "/image.png", "", "", "");
                    AssertEqual("<img src=\"/image.png\">", html);
                }),

                Case("BodyEncoding", "ButtonLegacyAliasAndAttributes", "Legacy Button alias encodes text and attributes", () =>
                {
                    string html = new HtmlBody().Button("<Save>", "save\"id", "primary", "font-weight:bold");
                    AssertEqual("<button type=\"button\" id=\"save&quot;id\" class=\"primary\" style=\"font-weight:bold\">&lt;Save&gt;</button>", html);
                }),
            });
        }

        private static TestSuiteDescriptor BodyHelperSuite()
        {
            return Suite("BodyHelpers", "Body helper coverage", new List<TestCaseDescriptor>
            {
                Case("BodyHelpers", "AdditionalHeadings", "H4 through H6 helpers are available", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<h4>Four</h4>", body.H4Text("Four"));
                    AssertEqual("<h5>Five</h5>", body.H5Text("Five"));
                    AssertEqual("<h6>Six</h6>", body.H6Text("Six"));
                }),

                Case("BodyHelpers", "DivSpanAndSemanticContainers", "Div, span, and semantic containers render expected tags", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<div>&lt;x&gt;</div>", body.DivText("<x>"));
                    AssertEqual("<div><p>x</p></div>", body.DivHtml("<p>x</p>"));
                    AssertEqual("<span>small</span>", body.SpanText("small"));
                    AssertEqual("<section><p>s</p></section>", body.SectionHtml("<p>s</p>"));
                    AssertEqual("<article><p>a</p></article>", body.ArticleHtml("<p>a</p>"));
                    AssertEqual("<main><p>m</p></main>", body.MainHtml("<p>m</p>"));
                    AssertEqual("<header><p>h</p></header>", body.HeaderHtml("<p>h</p>"));
                    AssertEqual("<footer><p>f</p></footer>", body.FooterHtml("<p>f</p>"));
                    AssertEqual("<nav><a>n</a></nav>", body.NavHtml("<a>n</a>"));
                }),

                Case("BodyHelpers", "HorizontalRuleAndLineBreak", "Void helpers render stable HTML", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<hr>", body.HorizontalRule());
                    AssertEqual("<br>", body.LineBreak());
                    AssertEqual("<hr id=\"rule\">", body.HorizontalRule("rule"));
                    AssertEqual("<br class=\"gap\">", body.LineBreak(null, "gap"));
                }),

                Case("BodyHelpers", "GenericElementTextAndHtml", "Generic element helpers support encoded text and raw HTML", () =>
                {
                    HtmlBody body = new HtmlBody();
                    HtmlAttributes attributes = new HtmlAttributes().Data("role", "summary").Aria("label", "A \"label\"");
                    string text = body.ElementText("aside", "<summary>", "id", "box", "display:block", attributes);
                    string html = body.ElementHtml("aside", "<strong>summary</strong>");

                    AssertContains(text, "<aside id=\"id\" class=\"box\" style=\"display:block\" data-role=\"summary\" aria-label=\"A &quot;label&quot;\">");
                    AssertContains(text, "&lt;summary&gt;</aside>");
                    AssertEqual("<aside><strong>summary</strong></aside>", html);
                }),

                Case("BodyHelpers", "FluentBodyAppendMethodsChain", "Body fluent append methods chain and preserve order", () =>
                {
                    string html = new HtmlBody()
                        .WithAttributes("main", "page", "margin:0")
                        .AddH1Text("Title")
                        .AddHorizontalRule()
                        .AddParagraphHtml("<strong>raw</strong>")
                        .AddLineBreak()
                        .ToString();

                    AssertEqual("<body id=\"main\" class=\"page\" style=\"margin:0\"><h1>Title</h1><hr><p><strong>raw</strong></p><br></body>", html);
                }),

                Case("BodyHelpers", "ClearRemovesContent", "Clear removes accumulated body content", () =>
                {
                    HtmlBody body = new HtmlBody().AddParagraphText("one").Clear().AddParagraphText("two");
                    AssertEqual("<body><p>two</p></body>", body.ToString());
                }),

                Case("BodyHelpers", "LegacyDivAliasIsRawHtml", "Legacy Div alias remains raw HTML", () =>
                {
                    AssertEqual("<div><strong>x</strong></div>", new HtmlBody().Div("<strong>x</strong>"));
                }),

                Case("BodyHelpers", "SpanHtmlKeepsRawContent", "SpanHtml keeps nested HTML raw", () =>
                {
                    AssertEqual("<span><em>x</em></span>", new HtmlBody().SpanHtml("<em>x</em>"));
                }),

                Case("BodyHelpers", "TextHelpersApplyCommonAttributes", "Text helpers apply common attributes consistently", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<h4 id=\"h\" class=\"c\" style=\"s\">Four</h4>", body.H4Text("Four", "h", "c", "s"));
                    AssertEqual("<pre id=\"p\" class=\"c\" style=\"white-space:pre\">Value</pre>", body.Preformatted("Value", "p", "c", "white-space:pre"));
                    AssertEqual("<p id=\"p\" class=\"c\" style=\"s\">Text</p>", body.ParagraphText("Text", "p", "c", "s"));
                    AssertEqual("<div id=\"d\" class=\"c\" style=\"s\">Text</div>", body.DivText("Text", "d", "c", "s"));
                    AssertEqual("<span id=\"s\" class=\"c\" style=\"color:red\">Text</span>", body.SpanText("Text", "s", "c", "color:red"));
                }),

                Case("BodyHelpers", "SemanticContainersApplyCommonAttributes", "Semantic container helpers apply common attributes", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<section id=\"s\" class=\"c\" style=\"display:block\"><p>x</p></section>", body.SectionHtml("<p>x</p>", "s", "c", "display:block"));
                    AssertEqual("<article id=\"a\" class=\"c\" style=\"display:block\"><p>x</p></article>", body.ArticleHtml("<p>x</p>", "a", "c", "display:block"));
                    AssertEqual("<main id=\"m\" class=\"c\" style=\"display:block\"><p>x</p></main>", body.MainHtml("<p>x</p>", "m", "c", "display:block"));
                    AssertEqual("<header id=\"h\" class=\"c\" style=\"display:block\"><p>x</p></header>", body.HeaderHtml("<p>x</p>", "h", "c", "display:block"));
                    AssertEqual("<footer id=\"f\" class=\"c\" style=\"display:block\"><p>x</p></footer>", body.FooterHtml("<p>x</p>", "f", "c", "display:block"));
                    AssertEqual("<nav id=\"n\" class=\"c\" style=\"display:block\"><p>x</p></nav>", body.NavHtml("<p>x</p>", "n", "c", "display:block"));
                }),

                Case("BodyHelpers", "FluentAppendMethodsReturnSameInstance", "Body fluent append methods return the same instance", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertSame(body, body.WithAttributes("id", "class", "style"));
                    AssertSame(body, body.AddRaw("<p>x</p>"));
                    AssertSame(body, body.AddText("text"));
                    AssertSame(body, body.Clear());
                    AssertSame(body, body.AddH1Text("h"));
                    AssertSame(body, body.AddParagraphText("p"));
                    AssertSame(body, body.AddParagraphHtml("<strong>p</strong>"));
                    AssertSame(body, body.AddLineBreak());
                    AssertSame(body, body.AddHorizontalRule());
                }),
            });
        }

        private static TestSuiteDescriptor ListAndTableSuite()
        {
            return Suite("ListsTables", "List and table helpers", new List<TestCaseDescriptor>
            {
                Case("ListsTables", "UnorderedListEncodesItems", "UnorderedList encodes item text", () =>
                {
                    string html = new HtmlBody().UnorderedList(new List<string> { "<one>", "two & three" });
                    AssertEqual("<ul type=\"disc\"><li>&lt;one&gt;</li><li>two &amp; three</li></ul>", html);
                }),

                Case("ListsTables", "OrderedListEncodesItemsAndAttributes", "OrderedList encodes values and attributes", () =>
                {
                    string html = new HtmlBody().OrderedList(new List<string> { "\"one\"" }, "A", null, "list\"1");
                    AssertEqual("<ol type=\"A\" id=\"list&quot;1\"><li>&quot;one&quot;</li></ol>", html);
                }),

                Case("ListsTables", "HtmlListVariantsKeepRawItems", "HTML list variants keep item content raw", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<ul type=\"disc\"><li><strong>one</strong></li></ul>", body.UnorderedListHtml(new List<string> { "<strong>one</strong>" }));
                    AssertEqual("<ol type=\"1\"><li><em>one</em></li></ol>", body.OrderedListHtml(new List<string> { "<em>one</em>" }));
                }),

                Case("ListsTables", "EmptyAndNullLists", "Empty and null lists render predictably", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<ul type=\"disc\"><li>(empty)</li></ul>", body.UnorderedList(new List<string>()));
                    AssertEqual("<ol type=\"1\"></ol>", body.OrderedList(null));
                    AssertEqual("<ul type=\"disc\"></ul>", body.UnorderedList(new List<string>(), "disc", null));
                }),

                Case("ListsTables", "TableEncodesColumnsAndValues", "Table encodes column names and cell values", () =>
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Name <Full>");
                    table.Columns.Add("Notes");
                    table.Rows.Add("A & B", DBNull.Value);
                    table.Rows.Add("<script>", "\"quote\"");

                    string html = new HtmlBody().Table(table, "t");
                    AssertContains(html, "<table id=\"t\">");
                    AssertContains(html, "<th>Name &lt;Full&gt;</th>");
                    AssertContains(html, "<td>A &amp; B</td>");
                    AssertContains(html, "<td></td>");
                    AssertContains(html, "<td>&lt;script&gt;</td>");
                    AssertContains(html, "<td>&quot;quote&quot;</td>");
                }),

                Case("ListsTables", "NullTableRendersEmptyTable", "Null table renders an empty table", () =>
                {
                    AssertEqual("<table></table>", new HtmlBody().Table(null));
                }),

                Case("ListsTables", "ListsOmitEmptyTypeAndEncodeEmptyValue", "Lists omit empty type and encode empty placeholder", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<ol><li>&lt;empty&gt;</li></ol>", body.OrderedList(new List<string>(), "", "<empty>"));
                    AssertEqual("<ul><li><strong>empty</strong></li></ul>", body.UnorderedListHtml(new List<string>(), "", "<strong>empty</strong>"));
                }),

                Case("ListsTables", "NullListItemsRenderEmptyListItems", "Null list items render empty list item content", () =>
                {
                    HtmlBody body = new HtmlBody();
                    AssertEqual("<ul type=\"disc\"><li></li></ul>", body.UnorderedList(new List<string> { null }));
                    AssertEqual("<ol type=\"1\"><li></li></ol>", body.OrderedListHtml(new List<string> { null }));
                }),

                Case("ListsTables", "TableWithColumnsAndNoRowsRendersHeaderOnly", "Table with columns and no rows renders only headers", () =>
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("Only");

                    AssertEqual("<table class=\"grid\" style=\"width:100%\"><tr><th>Only</th></tr></table>", new HtmlBody().Table(table, null, "grid", "width:100%"));
                }),
            });
        }

        private static TestSuiteDescriptor AttributeSuite()
        {
            return Suite("Attributes", "Attribute safety", new List<TestCaseDescriptor>
            {
                Case("Attributes", "HtmlAttributesSetReplaceRemove", "HtmlAttributes supports set, replace, and remove", () =>
                {
                    HtmlAttributes attributes = new HtmlAttributes()
                        .Set("data-id", "1")
                        .Set("data-id", "2")
                        .Set("data-id", null)
                        .Class("primary");

                    AssertEqual(1, attributes.Count);
                    string html = new HtmlBody().ElementText("div", "x", attributes: attributes);
                    AssertEqual("<div class=\"primary\">x</div>", html);
                }),

                Case("Attributes", "InvalidAttributeNameThrows", "Invalid attribute names are rejected", () =>
                {
                    AssertThrows<ArgumentException>(() => new HtmlAttributes().Set("bad name", "x"));
                    AssertThrows<ArgumentException>(() => new HtmlBody().ElementText("div script", "x"));
                }),

                Case("Attributes", "BodyAttributesEncode", "Body wrapper attributes are encoded", () =>
                {
                    HtmlBody body = new HtmlBody().WithAttributes("id\"1", "class&1", "color:\"red\"");
                    body.Attributes.Data("state", "a\"b");
                    string html = body.ToString();
                    AssertEqual("<body id=\"id&quot;1\" class=\"class&amp;1\" style=\"color:&quot;red&quot;\" data-state=\"a&quot;b\"></body>", html);
                }),

                Case("Attributes", "ConvenienceAttributeMethodsEncodeAndChain", "Attribute convenience methods encode and chain", () =>
                {
                    HtmlAttributes attributes = new HtmlAttributes()
                        .Id("id\"1")
                        .Class("class&1")
                        .Style("color:\"red\"");

                    AssertEqual(3, attributes.Count);
                    AssertEqual("<div id=\"id&quot;1\" class=\"class&amp;1\" style=\"color:&quot;red&quot;\">x</div>", new HtmlBody().ElementText("div", "x", attributes: attributes));
                }),

                Case("Attributes", "AttributeReplacementIsCaseInsensitive", "Attribute replacement is case-insensitive", () =>
                {
                    HtmlAttributes attributes = new HtmlAttributes()
                        .Set("DATA-ID", "1")
                        .Set("data-id", "2");

                    AssertEqual(1, attributes.Count);
                    AssertEqual("<div data-id=\"2\">x</div>", new HtmlBody().ElementText("div", "x", attributes: attributes));
                }),

                Case("Attributes", "NewNullAttributeIsIgnored", "Setting a new attribute to null does not add it", () =>
                {
                    HtmlAttributes attributes = new HtmlAttributes().Set("data-empty", null);
                    AssertEqual(0, attributes.Count);
                    AssertEqual("<div>x</div>", new HtmlBody().ElementText("div", "x", attributes: attributes));
                }),

                Case("Attributes", "InvalidDataAndAriaNamesThrow", "Invalid data and aria names are rejected", () =>
                {
                    AssertThrows<ArgumentException>(() => new HtmlAttributes().Data("", "x"));
                    AssertThrows<ArgumentException>(() => new HtmlAttributes().Aria("", "x"));
                }),

                Case("Attributes", "InvalidNamesRejectWhitespaceAndSymbols", "Invalid element and attribute names reject whitespace and symbols", () =>
                {
                    AssertThrows<ArgumentException>(() => new HtmlBody().ElementText(null, "x"));
                    AssertThrows<ArgumentException>(() => new HtmlBody().ElementText("", "x"));
                    AssertThrows<ArgumentException>(() => new HtmlBody().ElementText("bad<tag", "x"));
                    AssertThrows<ArgumentException>(() => new HtmlAttributes().Set(null, "x"));
                    AssertThrows<ArgumentException>(() => new HtmlAttributes().Set("bad<tag", "x"));
                    AssertThrows<ArgumentException>(() => new HtmlAttributes().Set(" ", "x"));
                }),

                Case("Attributes", "ValidNamesAllowSupportedCharacters", "Element and attribute names allow supported name characters", () =>
                {
                    HtmlAttributes attributes = new HtmlAttributes()
                        .Set("data_ID:part.name-9", "value");

                    string html = new HtmlBody().ElementText("x-tag_name:part.1", "value", attributes: attributes);
                    AssertEqual("<x-tag_name:part.1 data_ID:part.name-9=\"value\">value</x-tag_name:part.1>", html);
                }),
            });
        }

        private static TestSuiteDescriptor DeveloperExperienceSuite()
        {
            return Suite("DeveloperExperience", "Developer experience helpers", new List<TestCaseDescriptor>
            {
                Case("DeveloperExperience", "HtmlContextCreatesPrimaryTypes", "Html context creates the primary builder types", () =>
                {
                    Html builder = new Html();

                    AssertIsType<HtmlPage>(builder.Page());
                    AssertIsType<HtmlHead>(builder.Head());
                    AssertIsType<HtmlBody>(builder.Body());
                    AssertIsType<HtmlAttributes>(builder.Attributes());
                }),

                Case("DeveloperExperience", "HtmlFragmentsEncodeOrPreserveContent", "HtmlFragment supports encoded text and raw HTML", () =>
                {
                    Html builder = new Html();
                    HtmlFragment text = builder.Text("<safe>");
                    HtmlFragment raw = builder.Raw("<strong>raw</strong>");
                    HtmlFragment combined = text + raw;
                    HtmlFragment combinedWithNull = text + null;
                    string nullFragmentString = (HtmlFragment)null;

                    AssertEqual("&lt;safe&gt;", text.ToString());
                    AssertEqual("<strong>raw</strong>", raw.Content);
                    AssertEqual("&lt;safe&gt;<strong>raw</strong>", combined.ToString());
                    AssertEqual("&lt;safe&gt;", combinedWithNull.ToString());
                    AssertEqual("", nullFragmentString);
                    AssertEqual("", HtmlFragment.Empty.ToString());
                    AssertEqual("", HtmlFragment.FromHtml(null).ToString());
                }),

                Case("DeveloperExperience", "FragmentsAppendToPageAndBody", "Fragments append through page and body fluent APIs", () =>
                {
                    Html builder = new Html();
                    HtmlFragment fragment = builder.Text("A < B") + builder.Raw("<br>");
                    string page = builder.Page().Add(fragment).ToString();
                    string body = builder.Body().Add(fragment).ToString();

                    AssertContains(page, "<body>A &lt; B<br></body>");
                    AssertEqual("<body>A &lt; B<br></body>", body);
                }),

                Case("DeveloperExperience", "ClassAndStyleBuildersComposeAttributes", "Class and style builders compose conditional attributes", () =>
                {
                    Html builder = new Html();
                    HtmlAttributes attributes = builder.Attributes()
                        .AddClass("card")
                        .AddClassIf(false, "hidden")
                        .AddClassIf(true, "active")
                        .AddStyle("display", "block")
                        .AddStyleIf(false, "color", "red")
                        .AddStyleIf(true, "margin", "0");

                    string html = builder.Body().ElementText("section", "Ready", attributes: attributes);
                    AssertEqual("<section class=\"card active\" style=\"display:block;margin:0;\">Ready</section>", html);
                }),

                Case("DeveloperExperience", "ClassAndStyleBuildersIgnoreEmptyValues", "Class and style builders ignore empty values", () =>
                {
                    Html builder = new Html();
                    HtmlAttributes attributes = builder.Attributes()
                        .AddClass("")
                        .AddClass("   ")
                        .AddStyle("", "value")
                        .AddStyle("color", null);

                    AssertEqual(0, attributes.Count);
                }),

                Case("DeveloperExperience", "EnumerableListsAcceptArraysAndYieldedSequences", "List helpers accept arrays and yielded sequences", () =>
                {
                    Html builder = new Html();
                    HtmlBody body = builder.Body();
                    string ordered = body.OrderedList(new[] { "one", "two" });
                    string unordered = body.UnorderedList(YieldItems());

                    AssertEqual("<ol type=\"1\"><li>one</li><li>two</li></ol>", ordered);
                    AssertEqual("<ul type=\"disc\"><li>alpha</li><li>beta</li></ul>", unordered);
                }),

                Case("DeveloperExperience", "FluentAppendMethodsCoverCommonHelpers", "Fluent append methods cover common helpers", () =>
                {
                    Html builder = new Html();
                    string html = builder.Body()
                        .AddH1Html("<em>H1</em>")
                        .AddH2Text("H2")
                        .AddH2Html("<em>H2 raw</em>")
                        .AddH3Text("H3")
                        .AddH3Html("<em>H3</em>")
                        .AddH4Text("H4")
                        .AddH5Text("H5")
                        .AddH6Text("H6")
                        .AddPreformatted("pre")
                        .AddDivText("div")
                        .AddDivHtml("<strong>div</strong>")
                        .AddSpanText("span")
                        .AddSpanHtml("<strong>span</strong>")
                        .AddSectionHtml("<p>section</p>")
                        .AddArticleHtml("<p>article</p>")
                        .AddMainHtml("<p>main</p>")
                        .AddHeaderHtml("<p>header</p>")
                        .AddFooterHtml("<p>footer</p>")
                        .AddNavHtml("<a>nav</a>")
                        .AddLinkText("link", "/")
                        .AddLinkHtml("<strong>link</strong>", "/raw")
                        .AddImage("alt", "/image.png")
                        .AddOrderedList(new[] { "one" })
                        .AddUnorderedList(new[] { "two" })
                        .AddButtonText("button")
                        .AddButtonHtml("<span>button</span>")
                        .ToString();

                    AssertContains(html, "<h1><em>H1</em></h1>");
                    AssertContains(html, "<h2>H2</h2>");
                    AssertContains(html, "<h2><em>H2 raw</em></h2>");
                    AssertContains(html, "<h3>H3</h3>");
                    AssertContains(html, "<h3><em>H3</em></h3>");
                    AssertContains(html, "<pre>pre</pre>");
                    AssertContains(html, "<div>div</div>");
                    AssertContains(html, "<div><strong>div</strong></div>");
                    AssertContains(html, "<span>span</span>");
                    AssertContains(html, "<span><strong>span</strong></span>");
                    AssertContains(html, "<section><p>section</p></section>");
                    AssertContains(html, "<a href=\"/\">link</a>");
                    AssertContains(html, "<a href=\"/raw\"><strong>link</strong></a>");
                    AssertContains(html, "<img src=\"/image.png\" alt=\"alt\">");
                    AssertContains(html, "<button type=\"button\">button</button>");
                    AssertContains(html, "<button type=\"button\"><span>button</span></button>");
                }),

                Case("DeveloperExperience", "TableFromHeadersAndRows", "Table helper accepts headers and rows", () =>
                {
                    Html builder = new Html();
                    string html = builder.Body().Table(
                        new[] { "Name", "Value <Raw>" },
                        new[]
                        {
                            new[] { "A", "1" },
                            new[] { "B", "<2>" },
                        },
                        classId: "grid");

                    AssertEqual("<table class=\"grid\"><tr><th>Name</th><th>Value &lt;Raw&gt;</th></tr><tr><td>A</td><td>1</td></tr><tr><td>B</td><td>&lt;2&gt;</td></tr></table>", html);
                }),

                Case("DeveloperExperience", "TableFromDictionariesUsesFirstSeenHeaders", "Dictionary table helper uses first-seen headers", () =>
                {
                    Html builder = new Html();
                    List<IDictionary<string, object>> rows = new List<IDictionary<string, object>>
                    {
                        new Dictionary<string, object> { { "Name", "One" }, { "Status", "Ready" } },
                        new Dictionary<string, object> { { "Status", "<Done>" }, { "Extra", "Yes" } },
                    };

                    string html = builder.Body().TableFromDictionaries(rows);
                    AssertEqual("<table><tr><th>Name</th><th>Status</th><th>Extra</th></tr><tr><td>One</td><td>Ready</td><td></td></tr><tr><td></td><td>&lt;Done&gt;</td><td>Yes</td></tr></table>", html);
                }),

                Case("DeveloperExperience", "TableFromObjectsUsesPublicProperties", "Object table helper uses public properties", () =>
                {
                    Html builder = new Html();
                    BuildRow[] rows =
                    {
                        new BuildRow { Name = "Restore", Status = "Passed" },
                        new BuildRow { Name = "Tests", Status = "<Passed>" },
                    };

                    string html = builder.Body().TableFromObjects(rows, id: "builds");
                    AssertEqual("<table id=\"builds\"><tr><th>Name</th><th>Status</th></tr><tr><td>Restore</td><td>Passed</td></tr><tr><td>Tests</td><td>&lt;Passed&gt;</td></tr></table>", html);
                }),

                Case("DeveloperExperience", "FluentTableAppendMethods", "Fluent table append methods add generated tables", () =>
                {
                    Html builder = new Html();
                    DataTable table = new DataTable();
                    table.Columns.Add("Name");
                    table.Rows.Add("DataTable");

                    string html = builder.Body()
                        .AddTable(table)
                        .AddTable(new[] { "Name" }, new[] { new[] { "Value" } })
                        .AddTableFromObjects(new[] { new BuildRow { Name = "Build", Status = "Green" } })
                        .ToString();

                    AssertContains(html, "<table><tr><th>Name</th></tr><tr><td>DataTable</td></tr></table>");
                    AssertContains(html, "<table><tr><th>Name</th></tr><tr><td>Value</td></tr></table>");
                    AssertContains(html, "<table><tr><th>Name</th><th>Status</th></tr><tr><td>Build</td><td>Green</td></tr></table>");
                }),

                Case("DeveloperExperience", "HtmlOptionsApplyDefaults", "Html options apply defaults to created pages, heads, and bodies", () =>
                {
                    HtmlOptions options = new HtmlOptions
                    {
                        DefaultLanguage = "en",
                        DefaultTitle = "Default title",
                        DefaultMetaCharset = "utf-8",
                        DefaultMetaViewport = "",
                        DefaultBodyId = "app",
                        DefaultBodyClass = "shell",
                        DefaultBodyStyle = "margin:0",
                    };

                    Html builder = new Html(options);
                    HtmlPage page = builder.Page();
                    HtmlHead head = builder.Head();
                    HtmlBody body = builder.Body();

                    AssertContains(page.ToString(), "<html lang=\"en\">");
                    AssertContains(page.ToString(), "<title>Default title</title>");
                    AssertContains(page.ToString(), "<body id=\"app\" class=\"shell\" style=\"margin:0\">");
                    AssertContains(head.ToString(), "<title>Default title</title>");
                    AssertNotContains(head.ToString(), "viewport");
                    AssertEqual("<body id=\"app\" class=\"shell\" style=\"margin:0\"></body>", body.ToString());
                }),

                Case("DeveloperExperience", "HtmlContextTracksCreationCounts", "Html context tracks created object counts", () =>
                {
                    Html builder = new Html();
                    DateTime createdUtc = builder.CreatedUtc;

                    builder.Page();
                    builder.Head();
                    builder.Body();
                    builder.Attributes();
                    builder.Text("text");
                    builder.Raw("<br>");

                    AssertTrue(createdUtc <= DateTime.UtcNow);
                    AssertEqual(1, builder.PagesCreated);
                    AssertEqual(1, builder.HeadsCreated);
                    AssertEqual(1, builder.BodiesCreated);
                    AssertEqual(1, builder.AttributeCollectionsCreated);
                    AssertEqual(2, builder.FragmentsCreated);
                    AssertFalse(builder.IsDisposed);
                }),

                Case("DeveloperExperience", "DisposedHtmlContextRejectsFactoryMethods", "Disposed Html context rejects factory methods", () =>
                {
                    Html builder = new Html();
                    builder.Dispose();

                    AssertTrue(builder.IsDisposed);
                    AssertThrows<ObjectDisposedException>(() => builder.Page());
                    AssertThrows<ObjectDisposedException>(() => builder.Head());
                    AssertThrows<ObjectDisposedException>(() => builder.Body());
                    AssertThrows<ObjectDisposedException>(() => builder.Attributes());
                    AssertThrows<ObjectDisposedException>(() => builder.Text("x"));
                    AssertThrows<ObjectDisposedException>(() => builder.Raw("<br>"));
                }),
            });
        }

        private sealed class BuildRow
        {
            public string Name { get; set; }

            public string Status { get; set; }
        }

        private static IEnumerable<string> YieldItems()
        {
            yield return "alpha";
            yield return "beta";
        }

        private static TestSuiteDescriptor CompatibilitySuite()
        {
            return Suite("Compatibility", "Compatibility and sample coverage", new List<TestCaseDescriptor>
            {
                Case("Compatibility", "OriginalSampleScenarioRenders", "Original Test project sample scenario is covered", () =>
                {
                    HtmlPage page = new HtmlPage();

                    page.Head.Title = "My Page";
                    page.Head.FaviconUrl = "./assets/icon.ico";
                    page.Head.MetaKeywords = "my,page";
                    page.Head.Style = "h1 { font-family: 'arial' } p { font-family: 'arial' } ul { font-family: 'arial' }";

                    page.Body.Content += page.Body.H1Text("My Page");
                    page.Body.Content += page.Body.HorizontalRule();
                    page.Body.Content += page.Body.ParagraphText("This is some sample text.");
                    page.Body.Content += page.Body.ParagraphText("This is a button within a paragraph that links to Github!");
                    page.Body.Content += page.Body.Paragraph(page.Body.LinkHtml(page.Body.ButtonText("Github"), "https://github.com/jchristn/htmlpagebuilder", true));
                    page.Body.Content += page.Body.UnorderedList(new List<string> { "foo", "bar", "baz" });

                    string html = page.ToString();
                    AssertContains(html, "<body>");
                    AssertContains(html, "<h1>My Page</h1>");
                    AssertContains(html, "<hr>");
                    AssertContains(html, "<button type=\"button\">Github</button>");
                    AssertContains(html, "<ul type=\"disc\"><li>foo</li><li>bar</li><li>baz</li></ul>");
                    AssertContains(html, "</body></html>");
                }),

                Case("Compatibility", "LegacyContentConcatenationStillWorks", "Content += remains available", () =>
                {
                    HtmlBody body = new HtmlBody();
                    body.Content += body.H1Text("One");
                    body.Content += body.ParagraphHtml("<strong>Two</strong>");
                    AssertEqual("<body><h1>One</h1><p><strong>Two</strong></p></body>", body.ToString());
                }),
            });
        }

        private static TestSuiteDescriptor Suite(string id, string name, IReadOnlyList<TestCaseDescriptor> cases)
        {
            return new TestSuiteDescriptor(id, name, cases);
        }

        private static TestCaseDescriptor Case(string suiteId, string caseId, string displayName, Action execute)
        {
            return new TestCaseDescriptor(
                suiteId: suiteId,
                caseId: caseId,
                displayName: displayName,
                executeAsync: _ =>
                {
                    execute();
                    return Task.CompletedTask;
                });
        }

        private static void AssertEqual(string expected, string actual)
        {
            if (!String.Equals(expected, actual, StringComparison.Ordinal))
                throw new InvalidOperationException("Expected '" + expected + "' but got '" + actual + "'.");
        }

        private static void AssertEqual(int expected, int actual)
        {
            if (expected != actual)
                throw new InvalidOperationException("Expected '" + expected + "' but got '" + actual + "'.");
        }

        private static void AssertContains(string value, string expected)
        {
            if (value == null || !value.Contains(expected))
                throw new InvalidOperationException("Expected value to contain '" + expected + "' but got '" + value + "'.");
        }

        private static void AssertNotContains(string value, string unexpected)
        {
            if (value != null && value.Contains(unexpected))
                throw new InvalidOperationException("Expected value not to contain '" + unexpected + "' but got '" + value + "'.");
        }

        private static void AssertSame(object expected, object actual)
        {
            if (!Object.ReferenceEquals(expected, actual))
                throw new InvalidOperationException("Expected both values to reference the same instance.");
        }

        private static void AssertTrue(bool value)
        {
            if (!value)
                throw new InvalidOperationException("Expected value to be true.");
        }

        private static void AssertFalse(bool value)
        {
            if (value)
                throw new InvalidOperationException("Expected value to be false.");
        }

        private static void AssertIsType<T>(object value)
        {
            if (!(value is T))
                throw new InvalidOperationException("Expected value to be of type '" + typeof(T).Name + "'.");
        }

        private static void AssertThrows<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T)
            {
                return;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Expected exception '" + typeof(T).Name + "' but got '" + ex.GetType().Name + "'.");
            }

            throw new InvalidOperationException("Expected exception '" + typeof(T).Name + "' but no exception was thrown.");
        }
    }
}
