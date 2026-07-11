namespace HtmlPageBuilder
{
    /// <summary>
    /// Options used by an Html factory instance when creating pages, heads, and bodies.
    /// </summary>
    public class HtmlOptions
    {
        /// <summary>
        /// Default language applied to pages created by the factory. Null leaves the page default unchanged.
        /// </summary>
        public string DefaultLanguage { get; set; } = null;

        /// <summary>
        /// Default title applied to pages and heads created by the factory. Null leaves the target default unchanged.
        /// </summary>
        public string DefaultTitle { get; set; } = null;

        /// <summary>
        /// Default meta charset applied to heads created by the factory. Null leaves the target default unchanged.
        /// </summary>
        public string DefaultMetaCharset { get; set; } = null;

        /// <summary>
        /// Default viewport applied to heads created by the factory. Null leaves the target default unchanged.
        /// </summary>
        public string DefaultMetaViewport { get; set; } = null;

        /// <summary>
        /// Default body id applied to bodies created by the factory. Null leaves the target default unchanged.
        /// </summary>
        public string DefaultBodyId { get; set; } = null;

        /// <summary>
        /// Default body class applied to bodies created by the factory. Null leaves the target default unchanged.
        /// </summary>
        public string DefaultBodyClass { get; set; } = null;

        /// <summary>
        /// Default body style applied to bodies created by the factory. Null leaves the target default unchanged.
        /// </summary>
        public string DefaultBodyStyle { get; set; } = null;

        /// <summary>
        /// Create a copy of these options.
        /// </summary>
        /// <returns>Options copy.</returns>
        public HtmlOptions Clone()
        {
            return new HtmlOptions
            {
                DefaultLanguage = DefaultLanguage,
                DefaultTitle = DefaultTitle,
                DefaultMetaCharset = DefaultMetaCharset,
                DefaultMetaViewport = DefaultMetaViewport,
                DefaultBodyId = DefaultBodyId,
                DefaultBodyClass = DefaultBodyClass,
                DefaultBodyStyle = DefaultBodyStyle,
            };
        }
    }
}
