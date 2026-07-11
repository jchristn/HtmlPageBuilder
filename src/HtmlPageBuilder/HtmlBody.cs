using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace HtmlPageBuilder
{
    /// <summary>
    /// HTML page body.
    /// </summary>
    public class HtmlBody
    {
        private readonly StringBuilder _Content = new StringBuilder();

        #region Public-Members

        /// <summary>
        /// Body id attribute.
        /// </summary>
        public string Id { get; set; } = null;

        /// <summary>
        /// Body class attribute.
        /// </summary>
        public string Class { get; set; } = null;

        /// <summary>
        /// Body style attribute.
        /// </summary>
        public string Style { get; set; } = null;

        /// <summary>
        /// Additional body attributes.
        /// </summary>
        public HtmlAttributes Attributes { get; private set; } = new HtmlAttributes();

        /// <summary>
        /// HTML body inner content. Setting this property replaces current body content.
        /// </summary>
        public string Content
        {
            get { return _Content.ToString(); }
            set
            {
                _Content.Length = 0;
                if (value != null) _Content.Append(value);
            }
        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Set body attributes.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>This body.</returns>
        public HtmlBody WithAttributes(string id = null, string classId = null, string style = null)
        {
            Id = id;
            Class = classId;
            Style = style;
            return this;
        }

        /// <summary>
        /// Append raw HTML to the body. The value is not encoded.
        /// </summary>
        /// <param name="html">Raw HTML.</param>
        /// <returns>This body.</returns>
        public HtmlBody AddRaw(string html)
        {
            if (html != null) _Content.Append(html);
            return this;
        }

        /// <summary>
        /// Append an HTML fragment to the body.
        /// </summary>
        /// <param name="fragment">HTML fragment.</param>
        /// <returns>This body.</returns>
        public HtmlBody Add(HtmlFragment fragment)
        {
            if (fragment != null) _Content.Append(fragment.Content);
            return this;
        }

        /// <summary>
        /// Append encoded text to the body.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>This body.</returns>
        public HtmlBody AddText(string text)
        {
            _Content.Append(HtmlRendering.EncodeText(text));
            return this;
        }

        /// <summary>
        /// Clear body content.
        /// </summary>
        /// <returns>This body.</returns>
        public HtmlBody Clear()
        {
            _Content.Length = 0;
            return this;
        }

        /// <summary>
        /// Append an H1 element containing encoded text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents.</param>
        /// <returns>This body.</returns>
        public HtmlBody AddH1Text(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H1Text(text, id, classId, style));
        }

        /// <summary>
        /// Append a paragraph containing encoded text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents.</param>
        /// <returns>This body.</returns>
        public HtmlBody AddParagraphText(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(ParagraphText(text, id, classId, style));
        }

        /// <summary>
        /// Append a paragraph containing raw HTML.
        /// </summary>
        /// <param name="html">Raw HTML.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents.</param>
        /// <returns>This body.</returns>
        public HtmlBody AddParagraphHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(ParagraphHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a line break.
        /// </summary>
        /// <returns>This body.</returns>
        public HtmlBody AddLineBreak()
        {
            return AddRaw(LineBreak());
        }

        /// <summary>
        /// Append a horizontal rule.
        /// </summary>
        /// <returns>This body.</returns>
        public HtmlBody AddHorizontalRule()
        {
            return AddRaw(HorizontalRule());
        }

        /// <summary>
        /// Append an H1 element containing raw HTML.
        /// </summary>
        public HtmlBody AddH1Html(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H1Html(html, id, classId, style));
        }

        /// <summary>
        /// Append an H2 element containing encoded text.
        /// </summary>
        public HtmlBody AddH2Text(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H2Text(text, id, classId, style));
        }

        /// <summary>
        /// Append an H2 element containing raw HTML.
        /// </summary>
        public HtmlBody AddH2Html(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H2Html(html, id, classId, style));
        }

        /// <summary>
        /// Append an H3 element containing encoded text.
        /// </summary>
        public HtmlBody AddH3Text(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H3Text(text, id, classId, style));
        }

        /// <summary>
        /// Append an H3 element containing raw HTML.
        /// </summary>
        public HtmlBody AddH3Html(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H3Html(html, id, classId, style));
        }

        /// <summary>
        /// Append an H4 element containing encoded text.
        /// </summary>
        public HtmlBody AddH4Text(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H4Text(text, id, classId, style));
        }

        /// <summary>
        /// Append an H5 element containing encoded text.
        /// </summary>
        public HtmlBody AddH5Text(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H5Text(text, id, classId, style));
        }

        /// <summary>
        /// Append an H6 element containing encoded text.
        /// </summary>
        public HtmlBody AddH6Text(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(H6Text(text, id, classId, style));
        }

        /// <summary>
        /// Append preformatted encoded text.
        /// </summary>
        public HtmlBody AddPreformatted(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(Preformatted(text, id, classId, style));
        }

        /// <summary>
        /// Append a div containing encoded text.
        /// </summary>
        public HtmlBody AddDivText(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(DivText(text, id, classId, style));
        }

        /// <summary>
        /// Append a div containing raw HTML.
        /// </summary>
        public HtmlBody AddDivHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(DivHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a span containing encoded text.
        /// </summary>
        public HtmlBody AddSpanText(string text, string id = null, string classId = null, string style = null)
        {
            return AddRaw(SpanText(text, id, classId, style));
        }

        /// <summary>
        /// Append a span containing raw HTML.
        /// </summary>
        public HtmlBody AddSpanHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(SpanHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a section containing raw HTML.
        /// </summary>
        public HtmlBody AddSectionHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(SectionHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append an article containing raw HTML.
        /// </summary>
        public HtmlBody AddArticleHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(ArticleHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a main element containing raw HTML.
        /// </summary>
        public HtmlBody AddMainHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(MainHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a header containing raw HTML.
        /// </summary>
        public HtmlBody AddHeaderHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(HeaderHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a footer containing raw HTML.
        /// </summary>
        public HtmlBody AddFooterHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(FooterHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a nav containing raw HTML.
        /// </summary>
        public HtmlBody AddNavHtml(string html, string id = null, string classId = null, string style = null)
        {
            return AddRaw(NavHtml(html, id, classId, style));
        }

        /// <summary>
        /// Append a link containing encoded display text.
        /// </summary>
        public HtmlBody AddLinkText(string textToDisplay, string url, bool newWindow = false, string id = null, string classId = null, string style = null)
        {
            return AddRaw(LinkText(textToDisplay, url, newWindow, id, classId, style));
        }

        /// <summary>
        /// Append a link containing raw HTML display content.
        /// </summary>
        public HtmlBody AddLinkHtml(string htmlToDisplay, string url, bool newWindow = false, string id = null, string classId = null, string style = null)
        {
            return AddRaw(LinkHtml(htmlToDisplay, url, newWindow, id, classId, style));
        }

        /// <summary>
        /// Append an image.
        /// </summary>
        public HtmlBody AddImage(string text, string url, string id = null, string classId = null, string style = null)
        {
            return AddRaw(Image(text, url, id, classId, style));
        }

        /// <summary>
        /// Append an ordered list containing encoded item text.
        /// </summary>
        public HtmlBody AddOrderedList(IEnumerable<string> items, string listType = "1", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            return AddRaw(OrderedList(items, listType, emptyVal, id, classId, style));
        }

        /// <summary>
        /// Append an unordered list containing encoded item text.
        /// </summary>
        public HtmlBody AddUnorderedList(IEnumerable<string> items, string listType = "disc", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            return AddRaw(UnorderedList(items, listType, emptyVal, id, classId, style));
        }

        /// <summary>
        /// Append a table from a DataTable.
        /// </summary>
        public HtmlBody AddTable(DataTable table, string id = null, string classId = null, string style = null)
        {
            return AddRaw(Table(table, id, classId, style));
        }

        /// <summary>
        /// Append a table from headers and rows.
        /// </summary>
        public HtmlBody AddTable(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows, string id = null, string classId = null, string style = null)
        {
            return AddRaw(Table(headers, rows, id, classId, style));
        }

        /// <summary>
        /// Append a table from object properties.
        /// </summary>
        public HtmlBody AddTableFromObjects<T>(IEnumerable<T> items, string id = null, string classId = null, string style = null)
        {
            return AddRaw(TableFromObjects(items, id, classId, style));
        }

        /// <summary>
        /// Append a button containing encoded display text.
        /// </summary>
        public HtmlBody AddButtonText(string textToDisplay, string id = null, string classId = null, string style = null)
        {
            return AddRaw(ButtonText(textToDisplay, id, classId, style));
        }

        /// <summary>
        /// Append a button containing raw HTML display content.
        /// </summary>
        public HtmlBody AddButtonHtml(string htmlToDisplay, string id = null, string classId = null, string style = null)
        {
            return AddRaw(ButtonHtml(htmlToDisplay, id, classId, style));
        }

        /// <summary>
        /// H1 text. Text is HTML encoded.
        /// </summary>
        public string H1Text(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("h1", text, id, classId, style);
        }

        /// <summary>
        /// H1 HTML. Content is added without encoding.
        /// </summary>
        public string H1Html(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("h1", html, id, classId, style);
        }

        /// <summary>
        /// H2 text. Text is HTML encoded.
        /// </summary>
        public string H2Text(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("h2", text, id, classId, style);
        }

        /// <summary>
        /// H2 HTML. Content is added without encoding.
        /// </summary>
        public string H2Html(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("h2", html, id, classId, style);
        }

        /// <summary>
        /// H3 text. Text is HTML encoded.
        /// </summary>
        public string H3Text(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("h3", text, id, classId, style);
        }

        /// <summary>
        /// H3 HTML. Content is added without encoding.
        /// </summary>
        public string H3Html(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("h3", html, id, classId, style);
        }

        /// <summary>
        /// H4 text. Text is HTML encoded.
        /// </summary>
        public string H4Text(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("h4", text, id, classId, style);
        }

        /// <summary>
        /// H5 text. Text is HTML encoded.
        /// </summary>
        public string H5Text(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("h5", text, id, classId, style);
        }

        /// <summary>
        /// H6 text. Text is HTML encoded.
        /// </summary>
        public string H6Text(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("h6", text, id, classId, style);
        }

        /// <summary>
        /// Preformatted text. Text is HTML encoded.
        /// </summary>
        public string Preformatted(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("pre", text, id, classId, style);
        }

        /// <summary>
        /// Paragraph containing raw HTML. Prefer ParagraphText for plain text.
        /// </summary>
        public string Paragraph(string html, string id = null, string classId = null, string style = null)
        {
            return ParagraphHtml(html, id, classId, style);
        }

        /// <summary>
        /// Paragraph containing encoded text.
        /// </summary>
        public string ParagraphText(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("p", text, id, classId, style);
        }

        /// <summary>
        /// Paragraph containing raw HTML.
        /// </summary>
        public string ParagraphHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("p", html, id, classId, style);
        }

        /// <summary>
        /// Div containing raw HTML. Prefer DivText for plain text.
        /// </summary>
        public string Div(string html, string id = null, string classId = null, string style = null)
        {
            return DivHtml(html, id, classId, style);
        }

        /// <summary>
        /// Div containing encoded text.
        /// </summary>
        public string DivText(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("div", text, id, classId, style);
        }

        /// <summary>
        /// Div containing raw HTML.
        /// </summary>
        public string DivHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("div", html, id, classId, style);
        }

        /// <summary>
        /// Span containing encoded text.
        /// </summary>
        public string SpanText(string text, string id = null, string classId = null, string style = null)
        {
            return ElementText("span", text, id, classId, style);
        }

        /// <summary>
        /// Span containing raw HTML.
        /// </summary>
        public string SpanHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("span", html, id, classId, style);
        }

        /// <summary>
        /// Section containing raw HTML.
        /// </summary>
        public string SectionHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("section", html, id, classId, style);
        }

        /// <summary>
        /// Article containing raw HTML.
        /// </summary>
        public string ArticleHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("article", html, id, classId, style);
        }

        /// <summary>
        /// Main element containing raw HTML.
        /// </summary>
        public string MainHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("main", html, id, classId, style);
        }

        /// <summary>
        /// Header element containing raw HTML.
        /// </summary>
        public string HeaderHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("header", html, id, classId, style);
        }

        /// <summary>
        /// Footer element containing raw HTML.
        /// </summary>
        public string FooterHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("footer", html, id, classId, style);
        }

        /// <summary>
        /// Nav element containing raw HTML.
        /// </summary>
        public string NavHtml(string html, string id = null, string classId = null, string style = null)
        {
            return ElementHtml("nav", html, id, classId, style);
        }

        /// <summary>
        /// Link with encoded display text.
        /// </summary>
        public string Link(string textToDisplay, string url, bool newWindow = false, string id = null, string classId = null, string style = null)
        {
            return LinkText(textToDisplay, url, newWindow, id, classId, style);
        }

        /// <summary>
        /// Link with encoded display text.
        /// </summary>
        public string LinkText(string textToDisplay, string url, bool newWindow = false, string id = null, string classId = null, string style = null)
        {
            return LinkCore(textToDisplay, true, url, newWindow, id, classId, style);
        }

        /// <summary>
        /// Link with raw HTML display content.
        /// </summary>
        public string LinkHtml(string htmlToDisplay, string url, bool newWindow = false, string id = null, string classId = null, string style = null)
        {
            return LinkCore(htmlToDisplay, false, url, newWindow, id, classId, style);
        }

        /// <summary>
        /// Image.
        /// </summary>
        public string Image(string text, string url, string id = null, string classId = null, string style = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<img");
            HtmlRendering.AppendAttribute(builder, "src", url);
            HtmlRendering.AppendAttribute(builder, "alt", text);
            HtmlRendering.AppendCommonAttributes(builder, id, classId, style, null);
            builder.Append('>');
            return builder.ToString();
        }

        /// <summary>
        /// Ordered list containing encoded item text.
        /// </summary>
        public string OrderedList(IEnumerable<string> items, string listType = "1", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            return ListCore("ol", items, true, listType, emptyVal, id, classId, style);
        }

        /// <summary>
        /// Ordered list containing raw HTML list item content.
        /// </summary>
        public string OrderedListHtml(IEnumerable<string> items, string listType = "1", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            return ListCore("ol", items, false, listType, emptyVal, id, classId, style);
        }

        /// <summary>
        /// Unordered list containing encoded item text.
        /// </summary>
        public string UnorderedList(IEnumerable<string> items, string listType = "disc", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            return ListCore("ul", items, true, listType, emptyVal, id, classId, style);
        }

        /// <summary>
        /// Unordered list containing raw HTML list item content.
        /// </summary>
        public string UnorderedListHtml(IEnumerable<string> items, string listType = "disc", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            return ListCore("ul", items, false, listType, emptyVal, id, classId, style);
        }

        /// <summary>
        /// Table. Column names and values are HTML encoded.
        /// </summary>
        public string Table(DataTable table, string id = null, string classId = null, string style = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<table");
            HtmlRendering.AppendCommonAttributes(builder, id, classId, style, null);
            builder.Append('>');

            if (table != null && table.Columns != null && table.Columns.Count > 0)
            {
                builder.Append("<tr>");

                foreach (DataColumn column in table.Columns)
                {
                    builder.Append("<th>");
                    builder.Append(HtmlRendering.EncodeText(column.ColumnName));
                    builder.Append("</th>");
                }

                builder.Append("</tr>");
            }

            if (table != null && table.Rows != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    builder.Append("<tr>");

                    foreach (DataColumn column in table.Columns)
                    {
                        string val = "";
                        if (row[column.ColumnName] != null && row[column.ColumnName] != DBNull.Value)
                            val = row[column.ColumnName].ToString();

                        builder.Append("<td>");
                        builder.Append(HtmlRendering.EncodeText(val));
                        builder.Append("</td>");
                    }

                    builder.Append("</tr>");
                }
            }

            builder.Append("</table>");
            return builder.ToString();
        }

        /// <summary>
        /// Table from headers and rows. Header and cell values are HTML encoded.
        /// </summary>
        public string Table(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows, string id = null, string classId = null, string style = null)
        {
            List<string> headerList = new List<string>();
            if (headers != null)
            {
                foreach (string header in headers)
                    headerList.Add(header);
            }

            List<List<string>> rowList = new List<List<string>>();
            if (rows != null)
            {
                foreach (IEnumerable<string> row in rows)
                {
                    List<string> values = new List<string>();
                    if (row != null)
                    {
                        foreach (string value in row)
                            values.Add(value);
                    }
                    rowList.Add(values);
                }
            }

            return TableCore(headerList, rowList, id, classId, style);
        }

        /// <summary>
        /// Table from dictionary rows. Keys become headers in first-seen order.
        /// </summary>
        public string TableFromDictionaries(IEnumerable<IDictionary<string, object>> rows, string id = null, string classId = null, string style = null)
        {
            List<IDictionary<string, object>> rowList = new List<IDictionary<string, object>>();
            List<string> headers = new List<string>();

            if (rows != null)
            {
                foreach (IDictionary<string, object> row in rows)
                {
                    rowList.Add(row);
                    if (row == null) continue;

                    foreach (string key in row.Keys)
                    {
                        if (!ContainsOrdinal(headers, key))
                            headers.Add(key);
                    }
                }
            }

            List<List<string>> values = new List<List<string>>();
            foreach (IDictionary<string, object> row in rowList)
            {
                List<string> rowValues = new List<string>();
                foreach (string header in headers)
                {
                    object value = null;
                    if (row != null && row.ContainsKey(header))
                        value = row[header];

                    rowValues.Add(value == null ? "" : value.ToString());
                }
                values.Add(rowValues);
            }

            return TableCore(headers, values, id, classId, style);
        }

        /// <summary>
        /// Table from public instance properties on objects.
        /// </summary>
        public string TableFromObjects<T>(IEnumerable<T> items, string id = null, string classId = null, string style = null)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            List<string> headers = new List<string>();
            foreach (PropertyInfo property in properties)
                headers.Add(property.Name);

            List<List<string>> rows = new List<List<string>>();
            if (items != null)
            {
                foreach (T item in items)
                {
                    List<string> row = new List<string>();
                    foreach (PropertyInfo property in properties)
                    {
                        object value = item == null ? null : property.GetValue(item, null);
                        row.Add(value == null ? "" : value.ToString());
                    }
                    rows.Add(row);
                }
            }

            return TableCore(headers, rows, id, classId, style);
        }

        /// <summary>
        /// Button with encoded display text.
        /// </summary>
        public string Button(string textToDisplay, string id = null, string classId = null, string style = null)
        {
            return ButtonText(textToDisplay, id, classId, style);
        }

        /// <summary>
        /// Button with encoded display text.
        /// </summary>
        public string ButtonText(string textToDisplay, string id = null, string classId = null, string style = null)
        {
            return ButtonCore(textToDisplay, true, id, classId, style);
        }

        /// <summary>
        /// Button with raw HTML display content.
        /// </summary>
        public string ButtonHtml(string htmlToDisplay, string id = null, string classId = null, string style = null)
        {
            return ButtonCore(htmlToDisplay, false, id, classId, style);
        }

        /// <summary>
        /// Horizontal rule.
        /// </summary>
        public string HorizontalRule(string id = null, string classId = null, string style = null)
        {
            return HtmlRendering.VoidElement("hr", id, classId, style, null);
        }

        /// <summary>
        /// Line break.
        /// </summary>
        public string LineBreak(string id = null, string classId = null, string style = null)
        {
            return HtmlRendering.VoidElement("br", id, classId, style, null);
        }

        /// <summary>
        /// Create an element containing encoded text.
        /// </summary>
        public string ElementText(string tagName, string text, string id = null, string classId = null, string style = null, HtmlAttributes attributes = null)
        {
            return HtmlRendering.Element(tagName, text, true, id, classId, style, attributes);
        }

        /// <summary>
        /// Create an element containing raw HTML.
        /// </summary>
        public string ElementHtml(string tagName, string html, string id = null, string classId = null, string style = null, HtmlAttributes attributes = null)
        {
            return HtmlRendering.Element(tagName, html, false, id, classId, style, attributes);
        }

        /// <summary>
        /// Create HTML from the body, including the body wrapper.
        /// </summary>
        /// <returns>HTML string.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<body");
            HtmlRendering.AppendCommonAttributes(builder, Id, Class, Style, Attributes);
            builder.Append('>');
            builder.Append(_Content.ToString());
            builder.Append("</body>");
            return builder.ToString();
        }

        #endregion

        #region Private-Methods

        private string LinkCore(string content, bool encodeContent, string url, bool newWindow, string id, string classId, string style)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<a");
            HtmlRendering.AppendAttribute(builder, "href", url);
            if (newWindow)
            {
                HtmlRendering.AppendAttribute(builder, "target", "_blank");
                HtmlRendering.AppendAttribute(builder, "rel", "noopener noreferrer");
            }
            HtmlRendering.AppendCommonAttributes(builder, id, classId, style, null);
            builder.Append('>');
            builder.Append(encodeContent ? HtmlRendering.EncodeText(content) : (content ?? ""));
            builder.Append("</a>");
            return builder.ToString();
        }

        private string ListCore(string tagName, IEnumerable<string> items, bool encodeItems, string listType, string emptyVal, string id, string classId, string style)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('<');
            builder.Append(tagName);
            HtmlRendering.AppendAttribute(builder, "type", listType);
            HtmlRendering.AppendCommonAttributes(builder, id, classId, style, null);
            builder.Append('>');

            if (items != null)
            {
                bool hasItems = false;
                foreach (string item in items)
                {
                    hasItems = true;
                    builder.Append("<li>");
                    builder.Append(encodeItems ? HtmlRendering.EncodeText(item) : (item ?? ""));
                    builder.Append("</li>");
                }

                if (!hasItems && !String.IsNullOrEmpty(emptyVal))
                {
                    builder.Append("<li>");
                    builder.Append(encodeItems ? HtmlRendering.EncodeText(emptyVal) : emptyVal);
                    builder.Append("</li>");
                }
            }

            builder.Append("</");
            builder.Append(tagName);
            builder.Append('>');
            return builder.ToString();
        }

        private string TableCore(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows, string id, string classId, string style)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<table");
            HtmlRendering.AppendCommonAttributes(builder, id, classId, style, null);
            builder.Append('>');

            if (headers != null)
            {
                builder.Append("<tr>");
                foreach (string header in headers)
                {
                    builder.Append("<th>");
                    builder.Append(HtmlRendering.EncodeText(header));
                    builder.Append("</th>");
                }
                builder.Append("</tr>");
            }

            if (rows != null)
            {
                foreach (IEnumerable<string> row in rows)
                {
                    builder.Append("<tr>");
                    if (row != null)
                    {
                        foreach (string value in row)
                        {
                            builder.Append("<td>");
                            builder.Append(HtmlRendering.EncodeText(value));
                            builder.Append("</td>");
                        }
                    }
                    builder.Append("</tr>");
                }
            }

            builder.Append("</table>");
            return builder.ToString();
        }

        private static bool ContainsOrdinal(List<string> values, string value)
        {
            foreach (string existing in values)
            {
                if (String.Equals(existing, value, StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        private string ButtonCore(string content, bool encodeContent, string id, string classId, string style)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<button type=\"button\"");
            HtmlRendering.AppendCommonAttributes(builder, id, classId, style, null);
            builder.Append('>');
            builder.Append(encodeContent ? HtmlRendering.EncodeText(content) : (content ?? ""));
            builder.Append("</button>");
            return builder.ToString();
        }

        #endregion
    }
}
