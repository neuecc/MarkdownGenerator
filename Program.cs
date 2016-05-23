using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarkdownWikiGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // put dll & xml on same diretory.
            var target = "UniRx.dll"; // :)
            if (args.Length == 1)
            {
                target = args[0];
            }

            var types = MarkdownGenerator.Load(target);

            // Home Markdown Builder
            var homeBuilder = new MarkdownBuilder();
            homeBuilder.Header(1, "References");
            homeBuilder.AppendLine();

            foreach (var g in types.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                if (!Directory.Exists("md")) Directory.CreateDirectory("md");

                homeBuilder.HeaderWithLink(2, g.Key, g.Key);
                homeBuilder.AppendLine();

                var sb = new StringBuilder();
                foreach (var item in g.OrderBy(x => x.Name))
                {
                    homeBuilder.ListLink(MarkdownBuilder.MarkdownCodeQuote(item.BeautifyName), g.Key + "#" + item.BeautifyName.Replace("<", "").Replace(">", "").Replace(",", "").Replace(" ", "-").ToLower());

                    sb.Append(item.ToString());
                }

                File.WriteAllText(@"md\" + g.Key + ".md", sb.ToString());
                homeBuilder.AppendLine();
            }

            // Gen Home
            File.WriteAllText(@"md\Home.md", homeBuilder.ToString());
        }
    }
}
