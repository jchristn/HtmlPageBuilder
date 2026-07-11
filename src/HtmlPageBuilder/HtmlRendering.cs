using System;
using System.Net;
using System.Text;

namespace HtmlPageBuilder
{
    internal static class HtmlRendering
    {
        public static string EncodeText(string value)
        {
            return WebUtility.HtmlEncode(value ?? "");
        }

        public static string EncodeAttribute(string value)
        {
            return WebUtility.HtmlEncode(value ?? "");
        }

        public static void AppendAttribute(StringBuilder builder, string name, string value)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (String.IsNullOrEmpty(value)) return;

            ValidateAttributeName(name);
            builder.Append(' ');
            builder.Append(name);
            builder.Append("=\"");
            builder.Append(EncodeAttribute(value));
            builder.Append("\"");
        }

        public static void AppendCommonAttributes(
            StringBuilder builder,
            string id,
            string classId,
            string style,
            HtmlAttributes attributes)
        {
            AppendAttribute(builder, "id", id);
            AppendAttribute(builder, "class", classId);
            AppendAttribute(builder, "style", style);

            if (attributes != null)
                attributes.AppendTo(builder);
        }

        public static string Element(
            string tagName,
            string content,
            bool encodeContent,
            string id,
            string classId,
            string style,
            HtmlAttributes attributes)
        {
            ValidateTagName(tagName);

            StringBuilder builder = new StringBuilder();
            builder.Append('<');
            builder.Append(tagName);
            AppendCommonAttributes(builder, id, classId, style, attributes);
            builder.Append('>');
            builder.Append(encodeContent ? EncodeText(content) : (content ?? ""));
            builder.Append("</");
            builder.Append(tagName);
            builder.Append('>');
            return builder.ToString();
        }

        public static string VoidElement(
            string tagName,
            string id,
            string classId,
            string style,
            HtmlAttributes attributes)
        {
            ValidateTagName(tagName);

            StringBuilder builder = new StringBuilder();
            builder.Append('<');
            builder.Append(tagName);
            AppendCommonAttributes(builder, id, classId, style, attributes);
            builder.Append('>');
            return builder.ToString();
        }

        public static void ValidateTagName(string name)
        {
            if (!IsValidName(name))
                throw new ArgumentException("HTML tag name must be non-empty and contain only letters, digits, hyphen, underscore, colon, or period.", nameof(name));
        }

        public static void ValidateAttributeName(string name)
        {
            if (!IsValidName(name))
                throw new ArgumentException("HTML attribute name must be non-empty and contain only letters, digits, hyphen, underscore, colon, or period.", nameof(name));
        }

        private static bool IsValidName(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) return false;

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if ((c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= '0' && c <= '9') ||
                    c == '-' ||
                    c == '_' ||
                    c == ':' ||
                    c == '.')
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
