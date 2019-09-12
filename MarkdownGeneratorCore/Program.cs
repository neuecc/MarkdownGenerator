using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MarkdownWikiGeneratorCore
{
    class Program
    {
        // 0 = DLL source path, 1 = destination root
        private static void Main(string[] args)
        {
            string target = string.Empty;
            string dest = "docs";
            string namespaceMatch = string.Empty;

            if (args.Length == 0)
            {
                Console.WriteLine("DLL file location is a required argument");
                Environment.Exit(1);
            }
            if (args.Length == 1)
            {
                target = args[0];
            }
            else if (args.Length == 2)
            {
                target = args[0];
                dest = args[1];
            }
            else if (args.Length == 3)
            {
                target = args[0];
                dest = args[1];
                namespaceMatch = args[2];
            }

            MarkdownableType[] types = MarkdownGenerator.Load(target, namespaceMatch);

            // Home Markdown Builder
            MarkdownBuilder homeBuilder = new MarkdownBuilder();
            homeBuilder.Header(1, "References");
            homeBuilder.AppendLine();

            foreach (IGrouping<string, MarkdownableType> g in types.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }

                homeBuilder.HeaderWithLink(2, g.Key, g.Key);
                homeBuilder.AppendLine();

                StringBuilder sb = new StringBuilder();
                foreach (MarkdownableType item in g.OrderBy(x => x.Name))
                {
                    homeBuilder.ListLink(MarkdownBuilder.MarkdownCodeQuote(item.BeautifyName), g.Key + "#" + item.BeautifyName.Replace("<", "").Replace(">", "").Replace(",", "").Replace(" ", "-").ToLower());

                    sb.Append(item.ToString());
                }

                File.WriteAllText(Path.Combine(dest, g.Key + ".md"), sb.ToString());
                homeBuilder.AppendLine();
            }

            // Gen Home
            File.WriteAllText(Path.Combine(dest, "Home.md"), homeBuilder.ToString());
        }
    }
}
