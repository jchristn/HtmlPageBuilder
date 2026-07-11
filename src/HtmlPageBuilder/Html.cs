using System;

namespace HtmlPageBuilder
{
    /// <summary>
    /// Instance-based factory and context for creating pages, bodies, attributes, and fragments.
    /// </summary>
    public class Html : IDisposable
    {
        private bool _Disposed = false;

        /// <summary>
        /// Instantiate with default options.
        /// </summary>
        public Html() : this(null)
        {

        }

        /// <summary>
        /// Instantiate with options.
        /// </summary>
        /// <param name="options">Options. If null, default options are used.</param>
        public Html(HtmlOptions options)
        {
            Options = options == null ? new HtmlOptions() : options.Clone();
            CreatedUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Options used by this factory instance.
        /// </summary>
        public HtmlOptions Options { get; private set; }

        /// <summary>
        /// Time this factory instance was created.
        /// </summary>
        public DateTime CreatedUtc { get; private set; }

        /// <summary>
        /// True after this factory instance has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _Disposed; }
        }

        /// <summary>
        /// Number of pages created by this factory instance.
        /// </summary>
        public int PagesCreated { get; private set; } = 0;

        /// <summary>
        /// Number of heads created by this factory instance.
        /// </summary>
        public int HeadsCreated { get; private set; } = 0;

        /// <summary>
        /// Number of bodies created by this factory instance.
        /// </summary>
        public int BodiesCreated { get; private set; } = 0;

        /// <summary>
        /// Number of attribute collections created by this factory instance.
        /// </summary>
        public int AttributeCollectionsCreated { get; private set; } = 0;

        /// <summary>
        /// Number of fragments created by this factory instance.
        /// </summary>
        public int FragmentsCreated { get; private set; } = 0;

        /// <summary>
        /// Create an HTML page.
        /// </summary>
        /// <returns>HTML page.</returns>
        public HtmlPage Page()
        {
            EnsureNotDisposed();
            PagesCreated++;

            HtmlPage page = new HtmlPage();
            ApplyOptions(page);
            return page;
        }

        /// <summary>
        /// Create an HTML head.
        /// </summary>
        /// <returns>HTML head.</returns>
        public HtmlHead Head()
        {
            EnsureNotDisposed();
            HeadsCreated++;

            HtmlHead head = new HtmlHead();
            ApplyOptions(head);
            return head;
        }

        /// <summary>
        /// Create an HTML body.
        /// </summary>
        /// <returns>HTML body.</returns>
        public HtmlBody Body()
        {
            EnsureNotDisposed();
            BodiesCreated++;

            HtmlBody body = new HtmlBody();
            ApplyOptions(body);
            return body;
        }

        /// <summary>
        /// Create an attribute collection.
        /// </summary>
        /// <returns>HTML attributes.</returns>
        public HtmlAttributes Attributes()
        {
            EnsureNotDisposed();
            AttributeCollectionsCreated++;
            return new HtmlAttributes();
        }

        /// <summary>
        /// Create an encoded text fragment.
        /// </summary>
        /// <param name="text">Plain text.</param>
        /// <returns>HTML fragment.</returns>
        public HtmlFragment Text(string text)
        {
            EnsureNotDisposed();
            FragmentsCreated++;
            return HtmlFragment.FromText(text);
        }

        /// <summary>
        /// Create a raw HTML fragment.
        /// </summary>
        /// <param name="html">Raw HTML.</param>
        /// <returns>HTML fragment.</returns>
        public HtmlFragment Raw(string html)
        {
            EnsureNotDisposed();
            FragmentsCreated++;
            return HtmlFragment.FromHtml(html);
        }

        /// <summary>
        /// Dispose this factory instance.
        /// </summary>
        public void Dispose()
        {
            _Disposed = true;
        }

        private void ApplyOptions(HtmlPage page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            if (Options.DefaultLanguage != null) page.Language = Options.DefaultLanguage;
            ApplyOptions(page.Head);
            ApplyOptions(page.Body);
        }

        private void ApplyOptions(HtmlHead head)
        {
            if (head == null) throw new ArgumentNullException(nameof(head));

            if (Options.DefaultTitle != null) head.Title = Options.DefaultTitle;
            if (Options.DefaultMetaCharset != null) head.MetaCharset = Options.DefaultMetaCharset;
            if (Options.DefaultMetaViewport != null) head.MetaViewport = Options.DefaultMetaViewport;
        }

        private void ApplyOptions(HtmlBody body)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));

            if (Options.DefaultBodyId != null) body.Id = Options.DefaultBodyId;
            if (Options.DefaultBodyClass != null) body.Class = Options.DefaultBodyClass;
            if (Options.DefaultBodyStyle != null) body.Style = Options.DefaultBodyStyle;
        }

        private void EnsureNotDisposed()
        {
            if (_Disposed) throw new ObjectDisposedException(nameof(Html));
        }
    }
}
