using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HtmlPageBuilder
{
    /// <summary>
    /// HTML page body.
    /// </summary>
    public class HtmlBody
    {
        #region Public-Members

        /// <summary>
        /// HTML body.
        /// </summary>
        public string Content { get; set; } = "";

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        #endregion

        #region Public-Methods

        /// <summary>
        /// H1 text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string H1Text(string text, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            string ret = "<h1";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</h1>";
            return ret;
        }

        /// <summary>
        /// H2 text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string H2Text(string text, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            string ret = "<h2";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</h2>";
            return ret;
        }

        /// <summary>
        /// H3 text.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string H3Text(string text, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            string ret = "<h3";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</h3>";
            return ret;
        }

        /// <summary>
        /// Paragraph.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Preformatted(string text, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            string ret = "<pre";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</pre>";
            return ret;
        }

        /// <summary>
        /// Paragraph.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Paragraph(string text, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            string ret = "<p";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</p>";
            return ret;
        }

        /// <summary>
        /// Div.  
        /// Do not use within an encapsulating Paragraph.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Div(string text, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));
            string ret = "<div";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + text + "</div>";
            return ret;
        }

        /// <summary>
        /// Link.
        /// </summary>
        /// <param name="textToDisplay">Text to display.</param>
        /// <param name="url">URL.</param>
        /// <param name="newWindow">Flag to indicate if the link should be opened in a new window.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Link(string textToDisplay, string url, bool newWindow = false, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(textToDisplay)) throw new ArgumentNullException(nameof(textToDisplay));
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            string ret = "<a href='" + url + "'";
            if (newWindow) ret += " target='_blank'";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">" + textToDisplay + "</a>";
            return ret;
        }

        /// <summary>
        /// Image.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="url">URL.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Image(string text, string url, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            string ret = "<img src='" + url + "'";
            if (!String.IsNullOrEmpty(text)) ret += " alt='" + text + "'";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += "/>";
            return ret;
        }

        /// <summary>
        /// Ordered list.
        /// </summary>
        /// <param name="items">Items.</param>
        /// <param name="listType">HTML list type, i.e. 1, A, a, I, or i.</param>
        /// <param name="emptyVal">Value to display if the list itself is empty.  If null, no list items will be added.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string OrderedList(List<string> items, string listType = "1", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            string ret = "<ol";
            if (!String.IsNullOrEmpty(listType)) ret += " type='" + listType + "'";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">";
            if (items.Count < 1 && !String.IsNullOrEmpty(emptyVal))
            {
                ret += "<li>" + emptyVal + "</li>";
            }
            else
            {
                foreach (string item in items)
                {
                    ret += "<li>" + item + "</li>";
                }
            }
            ret += "</ol>";
            return ret;
        }

        /// <summary>
        /// Unordered list.
        /// </summary>
        /// <param name="items">Items.</param>
        /// <param name="listType">HTML list type, i.e. disc, circle, square, or none.</param>
        /// <param name="emptyVal">Value to display if the list itself is empty.  If null, no list items will be added.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string UnorderedList(List<string> items, string listType = "disc", string emptyVal = "(empty)", string id = null, string classId = null, string style = null)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            string ret = "<ul";
            if (!String.IsNullOrEmpty(listType)) ret += " type='" + listType + "'";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">";
            if (items.Count < 1 && !String.IsNullOrEmpty(emptyVal))
            {
                ret += "<li>" + emptyVal + "</li>";
            }
            else
            {
                foreach (string item in items)
                {
                    ret += "<li>" + item + "</li>";
                }
            }
            ret += "</ul>";
            return ret;
        }

        /// <summary>
        /// Table.
        /// </summary>
        /// <param name="table">Data table.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Table(DataTable table, string id = null, string classId = null, string style = null)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (table.Columns == null || table.Columns.Count < 1) throw new ArgumentException("DataTable contains no columns.");
            if (table.Rows == null || table.Rows.Count < 1) throw new ArgumentException("DataTable contains no rows.");

            string ret = "<table";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">";

            ret += "<tr>";
            foreach (DataColumn column in table.Columns)
            {
                ret += "<th>" + column.ColumnName + "</th>";
            }
            ret += "</tr>";

            foreach (DataRow row in table.Rows)
            {
                ret += "<tr>";

                foreach (DataColumn column in table.Columns)
                {
                    string val = "";
                    if (row[column.ColumnName] != null && row[column.ColumnName] != DBNull.Value)
                    {
                        val = row[column.ColumnName].ToString();
                    }
                    ret += "<td>" + val + "</td>";
                }

                ret += "</tr>";
            }

            ret += "</table>";
            return ret;
        }

        /// <summary>
        /// Button.
        /// </summary>
        /// <param name="textToDisplay">Text to display.</param>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string Button(string textToDisplay, string id = null, string classId = null, string style = null)
        {
            if (String.IsNullOrEmpty(textToDisplay)) throw new ArgumentNullException(nameof(textToDisplay));
            string ret = "<button";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            else ret += " style='background-color:#33bd55; color:white; padding:8px; border:none; outline:none'";
            ret += ">" + textToDisplay + "</button>";
            return ret;
        }

        /// <summary>
        /// Horizontal rule.
        /// </summary>
        /// <param name="id">ID.</param>
        /// <param name="classId">Class ID.</param>
        /// <param name="style">Style tag contents, i.e. background-color:powderblue;</param>
        /// <returns>String.</returns>
        public string HorizontalRule(string id = null, string classId = null, string style = null)
        {
            string ret = "<hr";
            if (!String.IsNullOrEmpty(id)) ret += " id='" + id + "'";
            if (!String.IsNullOrEmpty(classId)) ret += " class='" + classId + "'";
            if (!String.IsNullOrEmpty(style)) ret += " style='" + style + "'";
            ret += ">";
            return ret;
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
