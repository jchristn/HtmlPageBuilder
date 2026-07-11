using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlPageBuilder
{
    /// <summary>
    /// HTML attribute collection with validated names and encoded values.
    /// </summary>
    public sealed class HtmlAttributes
    {
        private readonly List<KeyValuePair<string, string>> _Attributes = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Number of attributes in the collection.
        /// </summary>
        public int Count
        {
            get { return _Attributes.Count; }
        }

        /// <summary>
        /// Set or replace an attribute. A null value removes the attribute.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes Set(string name, string value)
        {
            HtmlRendering.ValidateAttributeName(name);

            for (int i = 0; i < _Attributes.Count; i++)
            {
                if (String.Equals(_Attributes[i].Key, name, StringComparison.OrdinalIgnoreCase))
                {
                    if (value == null)
                        _Attributes.RemoveAt(i);
                    else
                        _Attributes[i] = new KeyValuePair<string, string>(name, value);

                    return this;
                }
            }

            if (value != null)
                _Attributes.Add(new KeyValuePair<string, string>(name, value));

            return this;
        }

        /// <summary>
        /// Set the id attribute.
        /// </summary>
        /// <param name="value">ID value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes Id(string value)
        {
            return Set("id", value);
        }

        /// <summary>
        /// Set the class attribute.
        /// </summary>
        /// <param name="value">Class value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes Class(string value)
        {
            return Set("class", value);
        }

        /// <summary>
        /// Add one or more CSS classes.
        /// </summary>
        /// <param name="classNames">Class name string.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes AddClass(string classNames)
        {
            if (String.IsNullOrWhiteSpace(classNames)) return this;

            string current = GetValue("class");
            if (String.IsNullOrWhiteSpace(current))
                return Set("class", classNames.Trim());

            return Set("class", current.Trim() + " " + classNames.Trim());
        }

        /// <summary>
        /// Add one or more CSS classes when a condition is true.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="classNames">Class name string.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes AddClassIf(bool condition, string classNames)
        {
            return condition ? AddClass(classNames) : this;
        }

        /// <summary>
        /// Set the style attribute.
        /// </summary>
        /// <param name="value">Style value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes Style(string value)
        {
            return Set("style", value);
        }

        /// <summary>
        /// Add a CSS declaration to the style attribute.
        /// </summary>
        /// <param name="name">CSS property name.</param>
        /// <param name="value">CSS property value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes AddStyle(string name, string value)
        {
            if (String.IsNullOrWhiteSpace(name) || value == null) return this;

            string declaration = name.Trim() + ":" + value.Trim();
            if (!declaration.EndsWith(";")) declaration += ";";

            string current = GetValue("style");
            if (String.IsNullOrWhiteSpace(current))
                return Set("style", declaration);

            current = current.Trim();
            if (!current.EndsWith(";")) current += ";";
            return Set("style", current + declaration);
        }

        /// <summary>
        /// Add a CSS declaration to the style attribute when a condition is true.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="name">CSS property name.</param>
        /// <param name="value">CSS property value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes AddStyleIf(bool condition, string name, string value)
        {
            return condition ? AddStyle(name, value) : this;
        }

        /// <summary>
        /// Set a data-* attribute.
        /// </summary>
        /// <param name="name">Data attribute suffix.</param>
        /// <param name="value">Attribute value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes Data(string name, string value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Data attribute name cannot be empty.", nameof(name));

            return Set("data-" + name, value);
        }

        /// <summary>
        /// Set an aria-* attribute.
        /// </summary>
        /// <param name="name">ARIA attribute suffix.</param>
        /// <param name="value">Attribute value.</param>
        /// <returns>This attribute collection.</returns>
        public HtmlAttributes Aria(string name, string value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("ARIA attribute name cannot be empty.", nameof(name));

            return Set("aria-" + name, value);
        }

        internal void AppendTo(StringBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            foreach (KeyValuePair<string, string> attribute in _Attributes)
            {
                HtmlRendering.AppendAttribute(builder, attribute.Key, attribute.Value);
            }
        }

        private string GetValue(string name)
        {
            for (int i = 0; i < _Attributes.Count; i++)
            {
                if (String.Equals(_Attributes[i].Key, name, StringComparison.OrdinalIgnoreCase))
                    return _Attributes[i].Value;
            }

            return null;
        }
    }
}
