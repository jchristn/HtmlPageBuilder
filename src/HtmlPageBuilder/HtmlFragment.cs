namespace HtmlPageBuilder
{
    /// <summary>
    /// Represents HTML that is ready to append to a document or another fragment.
    /// </summary>
    public sealed class HtmlFragment
    {
        /// <summary>
        /// Empty HTML fragment.
        /// </summary>
        public static readonly HtmlFragment Empty = new HtmlFragment("");

        /// <summary>
        /// Instantiate from raw HTML.
        /// </summary>
        /// <param name="html">Raw HTML.</param>
        public HtmlFragment(string html)
        {
            Content = html ?? "";
        }

        /// <summary>
        /// Fragment content.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Create a fragment from encoded text.
        /// </summary>
        /// <param name="text">Plain text.</param>
        /// <returns>HTML fragment containing encoded text.</returns>
        public static HtmlFragment FromText(string text)
        {
            return new HtmlFragment(HtmlRendering.EncodeText(text));
        }

        /// <summary>
        /// Create a fragment from raw HTML.
        /// </summary>
        /// <param name="html">Raw HTML.</param>
        /// <returns>HTML fragment containing raw HTML.</returns>
        public static HtmlFragment FromHtml(string html)
        {
            return new HtmlFragment(html);
        }

        /// <summary>
        /// Combine two fragments.
        /// </summary>
        /// <param name="left">Left fragment.</param>
        /// <param name="right">Right fragment.</param>
        /// <returns>Combined fragment.</returns>
        public static HtmlFragment operator +(HtmlFragment left, HtmlFragment right)
        {
            return new HtmlFragment((left == null ? "" : left.Content) + (right == null ? "" : right.Content));
        }

        /// <summary>
        /// Convert a fragment to its HTML string.
        /// </summary>
        /// <param name="fragment">Fragment.</param>
        public static implicit operator string(HtmlFragment fragment)
        {
            return fragment == null ? "" : fragment.Content;
        }

        /// <summary>
        /// Return fragment HTML.
        /// </summary>
        /// <returns>HTML.</returns>
        public override string ToString()
        {
            return Content;
        }
    }
}
