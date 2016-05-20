using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            foreach (var g in types.GroupBy(x => x.Namespace))
            {
                if (!Directory.Exists("md")) Directory.CreateDirectory("md");

                var sb = new StringBuilder();
                foreach (var item in g.OrderBy(x => x.Name))
                {
                    sb.Append(item.ToString());
                }

                File.WriteAllText(@"md\" + g.Key + ".md", sb.ToString());
            }
        }
    }
}
