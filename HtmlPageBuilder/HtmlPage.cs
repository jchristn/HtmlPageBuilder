using System;

namespace HtmlPageBuilder
{
    /// <summary>
    /// HTML page.
    /// </summary>
    public class HtmlPage
    {
        #region Public-Members

        /// <summary>
        /// HTML document head.
        /// </summary>
        public HtmlHead Head { get; private set; } = new HtmlHead();

        /// <summary>
        /// HTML document body.
        /// </summary>
        public HtmlBody Body { get; private set; } = new HtmlBody();

        #endregion

        #region Private-Members

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
        /// Create an HTML string from the document.
        /// </summary>
        /// <returns>HTML string.</returns>
        public override string ToString()
        {
            string ret =
                "<!DOCTYPE html>" +
                "<html>" +
                Head.Content +
                Body.Content +
                "</html>";
            return ret;
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
