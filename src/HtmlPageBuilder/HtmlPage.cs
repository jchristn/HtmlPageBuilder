using System;
using System.Text;

namespace HtmlPageBuilder
{
    /// <summary>
    /// HTML page.
    /// </summary>
    public class HtmlPage
    {
        #region Public-Members

        /// <summary>
        /// HTML language attribute. If null or empty, no lang attribute is emitted.
        /// </summary>
        public string Language { get; set; } = null;

        /// <summary>
        /// HTML document head.
        /// </summary>
        public HtmlHead Head { get; private set; } = new HtmlHead();

        /// <summary>
        /// HTML document body.
        /// </summary>
        public HtmlBody Body { get; private set; } = new HtmlBody();

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate the object.
        /// </summary>
        public HtmlPage()
        {

        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Set the HTML language attribute.
        /// </summary>
        /// <param name="language">Language value.</param>
        /// <returns>This page.</returns>
        public HtmlPage WithLanguage(string language)
        {
            Language = language;
            return this;
        }

        /// <summary>
        /// Set the page title.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <returns>This page.</returns>
        public HtmlPage WithTitle(string title)
        {
            Head.Title = title;
            return this;
        }

        /// <summary>
        /// Configure the head using a fluent callback.
        /// </summary>
        /// <param name="configure">Head configuration callback.</param>
        /// <returns>This page.</returns>
        public HtmlPage WithHead(Action<HtmlHead> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            configure(Head);
            return this;
        }

        /// <summary>
        /// Configure the body using a fluent callback.
        /// </summary>
        /// <param name="configure">Body configuration callback.</param>
        /// <returns>This page.</returns>
        public HtmlPage WithBody(Action<HtmlBody> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            configure(Body);
            return this;
        }

        /// <summary>
        /// Append an H1 element containing encoded text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>This page.</returns>
        public HtmlPage AddH1Text(string text)
        {
            Body.AddH1Text(text);
            return this;
        }

        /// <summary>
        /// Append a paragraph containing encoded text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>This page.</returns>
        public HtmlPage AddParagraphText(string text)
        {
            Body.AddParagraphText(text);
            return this;
        }

        /// <summary>
        /// Append raw HTML to the body.
        /// </summary>
        /// <param name="html">HTML.</param>
        /// <returns>This page.</returns>
        public HtmlPage AddRaw(string html)
        {
            Body.AddRaw(html);
            return this;
        }

        /// <summary>
        /// Append an HTML fragment to the body.
        /// </summary>
        /// <param name="fragment">HTML fragment.</param>
        /// <returns>This page.</returns>
        public HtmlPage Add(HtmlFragment fragment)
        {
            Body.Add(fragment);
            return this;
        }

        /// <summary>
        /// Create an HTML string from the document.
        /// </summary>
        /// <returns>HTML string.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<!DOCTYPE html>");
            builder.Append("<html");
            HtmlRendering.AppendAttribute(builder, "lang", Language);
            builder.Append('>');
            builder.Append(Head.Content);
            builder.Append(Body.ToString());
            builder.Append("</html>");
            return builder.ToString();
        }

        #endregion
    }
}
