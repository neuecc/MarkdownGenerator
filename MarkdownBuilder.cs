using System.Collections.Generic;
using System.Text;

namespace MarkdownWikiGenerator
{
    public class MarkdownBuilder
    {
        public static string MarkdownCodeQuote(string code)
        {
            return "`" + code + "`";
        }


        StringBuilder sb = new StringBuilder();

        public void Append(string text)
        {
            sb.Append(text);
        }

        public void AppendLine()
        {
            sb.AppendLine();
        }

        public void AppendLine(string text)
        {
            sb.AppendLine(text);
        }

        public void Header(int level, string text)
        {
            for (int i = 0; i < level; i++)
            {
                sb.Append("#");
            }
            sb.Append(" ");
            sb.AppendLine(text);
        }

        public void HeaderWithCode(int level, string code)
        {
            for (int i = 0; i < level; i++)
            {
                sb.Append("#");
            }
            sb.Append(" ");
            CodeQuote(code);
            sb.AppendLine();
        }

        public void Link(string text, string url)
        {
            sb.Append("[");
            sb.Append(text);
            sb.Append("]");
            sb.Append("(");
            sb.Append(url);
            sb.Append(")");
        }

        public void Image(string altText, string imageUrl)
        {
            sb.Append("!");
            Link(altText, imageUrl);
        }

        public void Code(string language, string code)
        {
            sb.Append("```");
            sb.AppendLine(language);
            sb.AppendLine(code);
            sb.AppendLine("```");
        }

        public void CodeQuote(string code)
        {
            sb.Append("`");
            sb.Append(code);
            sb.Append("`");
        }

        public void Table(string[] headers, IEnumerable<string[]> items)
        {
            sb.Append("| ");
            foreach (var item in headers)
            {
                sb.Append(item);
                sb.Append(" | ");
            }
            sb.AppendLine();

            sb.Append("| ");
            foreach (var item in headers)
            {
                sb.Append("---");
                sb.Append(" | ");
            }
            sb.AppendLine();


            foreach (var item in items)
            {
                sb.Append("| ");
                foreach (var item2 in item)
                {
                    sb.Append(item2);
                    sb.Append(" | ");
                }
                sb.AppendLine();
            }
            sb.AppendLine();
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
