using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlPageBuilder
{
    /// <summary>
    /// HTML head.
    /// </summary>
    public class HtmlHead
    {
        #region Public-Members

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; } = null;

        /// <summary>
        /// Favicon URL.
        /// </summary>
        public string FaviconUrl { get; set; } = null;

        /// <summary>
        /// Style.
        /// </summary>
        public string Style { get; set; } = null;

        /// <summary>
        /// Base URL.
        /// </summary>
        public string BaseUrl { get; set; } = null;

        /// <summary>
        /// Character set.
        /// </summary>
        public string MetaCharset { get; set; } = "UTF-8";

        /// <summary>
        /// Viewport.
        /// </summary>
        public string MetaViewport { get; set; } = "width=device-width, initial-scale=1.0";

        /// <summary>
        /// Keywords.
        /// </summary>
        public string MetaKeywords { get; set; } = null;

        /// <summary>
        /// Description.
        /// </summary>
        public string MetaDescription { get; set; } = null;

        /// <summary>
        /// Author.
        /// </summary>
        public string MetaAuthor { get; set; } = null;

        /// <summary>
        /// Additional elements, which will be added exactly as included in the list.
        /// Elements added to this list must already be valid HTML.
        /// </summary>
        public List<string> AdditionalElements { get; private set; } = new List<string>();

        /// <summary>
        /// HEAD content.
        /// </summary>
        public string Content
        {
            get { return ToString(); }
        }

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate the object.
        /// </summary>
        public HtmlHead()
        {

        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Set the title.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <returns>This head.</returns>
        public HtmlHead WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Set the favicon URL.
        /// </summary>
        /// <param name="url">Favicon URL.</param>
        /// <returns>This head.</returns>
        public HtmlHead WithFavicon(string url)
        {
            FaviconUrl = url;
            return this;
        }

        /// <summary>
        /// Set inline style content.
        /// </summary>
        /// <param name="style">CSS style content.</param>
        /// <returns>This head.</returns>
        public HtmlHead WithStyle(string style)
        {
            Style = style;
            return this;
        }

        /// <summary>
        /// Set the base URL.
        /// </summary>
        /// <param name="url">Base URL.</param>
        /// <returns>This head.</returns>
        public HtmlHead WithBaseUrl(string url)
        {
            BaseUrl = url;
            return this;
        }

        /// <summary>
        /// Set the meta description.
        /// </summary>
        /// <param name="description">Description.</param>
        /// <returns>This head.</returns>
        public HtmlHead WithMetaDescription(string description)
        {
            MetaDescription = description;
            return this;
        }

        /// <summary>
        /// Set the meta keywords.
        /// </summary>
        /// <param name="keywords">Keywords.</param>
        /// <returns>This head.</returns>
        public HtmlHead WithMetaKeywords(string keywords)
        {
            MetaKeywords = keywords;
            return this;
        }

        /// <summary>
        /// Set the meta author.
        /// </summary>
        /// <param name="author">Author.</param>
        /// <returns>This head.</returns>
        public HtmlHead WithMetaAuthor(string author)
        {
            MetaAuthor = author;
            return this;
        }

        /// <summary>
        /// Add a stylesheet link.
        /// </summary>
        /// <param name="href">Stylesheet URL.</param>
        /// <param name="media">Optional media attribute.</param>
        /// <returns>This head.</returns>
        public HtmlHead AddStylesheet(string href, string media = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<link rel=\"stylesheet\"");
            HtmlRendering.AppendAttribute(builder, "href", href);
            HtmlRendering.AppendAttribute(builder, "media", media);
            builder.Append('>');
            AdditionalElements.Add(builder.ToString());
            return this;
        }

        /// <summary>
        /// Add a script source.
        /// </summary>
        /// <param name="src">Script URL.</param>
        /// <param name="defer">True to include defer.</param>
        /// <returns>This head.</returns>
        public HtmlHead AddScriptSource(string src, bool defer = false)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script");
            HtmlRendering.AppendAttribute(builder, "src", src);
            if (defer) builder.Append(" defer");
            builder.Append("></script>");
            AdditionalElements.Add(builder.ToString());
            return this;
        }

        /// <summary>
        /// Add a raw head element. The value is not encoded.
        /// </summary>
        /// <param name="html">Raw HTML element.</param>
        /// <returns>This head.</returns>
        public HtmlHead AddRawElement(string html)
        {
            if (html != null) AdditionalElements.Add(html);
            return this;
        }

        /// <summary>
        /// Create an HTML string from the head.
        /// </summary>
        /// <returns>HTML string.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<head>");

            if (!String.IsNullOrEmpty(Title))
                builder.Append(HtmlRendering.Element("title", Title, true, null, null, null, null));

            if (!String.IsNullOrEmpty(FaviconUrl))
            {
                builder.Append("<link rel=\"icon\" type=\"image/x-icon\"");
                HtmlRendering.AppendAttribute(builder, "href", FaviconUrl);
                builder.Append('>');
            }

            if (!String.IsNullOrEmpty(Style))
            {
                builder.Append("<style>");
                builder.Append(Environment.NewLine);
                builder.Append(Style);
                builder.Append(Environment.NewLine);
                builder.Append("</style>");
            }

            if (!String.IsNullOrEmpty(BaseUrl))
            {
                builder.Append("<base");
                HtmlRendering.AppendAttribute(builder, "href", BaseUrl);
                builder.Append('>');
            }

            AppendMeta(builder, "charset", null, MetaCharset);
            AppendMeta(builder, "name", "viewport", MetaViewport);
            AppendMeta(builder, "name", "keywords", MetaKeywords);
            AppendMeta(builder, "name", "description", MetaDescription);
            AppendMeta(builder, "name", "author", MetaAuthor);

            if (AdditionalElements != null && AdditionalElements.Count > 0)
            {
                foreach (string element in AdditionalElements)
                {
                    if (element != null) builder.Append(element);
                }
            }

            builder.Append("</head>");
            return builder.ToString();
        }

        #endregion

        #region Private-Methods

        private static void AppendMeta(StringBuilder builder, string keyAttribute, string keyValue, string contentValue)
        {
            if (String.IsNullOrEmpty(contentValue)) return;

            builder.Append("<meta");
            if (!String.IsNullOrEmpty(keyValue))
                HtmlRendering.AppendAttribute(builder, keyAttribute, keyValue);

            if (String.Equals(keyAttribute, "charset", StringComparison.OrdinalIgnoreCase))
                HtmlRendering.AppendAttribute(builder, "charset", contentValue);
            else
                HtmlRendering.AppendAttribute(builder, "content", contentValue);

            builder.Append('>');
        }

        #endregion
    }
}
