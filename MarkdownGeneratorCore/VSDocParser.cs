using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MarkdownWikiGeneratorCore
{
    public enum MemberType
    {
        Field = 'F',
        Property = 'P',
        Type = 'T',
        Event = 'E',
        Method = 'M',
        None = 0
    }

    public class XmlDocumentComment
    {
        public MemberType MemberType { get; set; }
        public string ClassName { get; set; }
        public string MemberName { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public string Returns { get; set; }

        public override string ToString() => MemberType + ":" + ClassName + "." + MemberName;
    }

    public static class VSDocParser
    {
        public static XmlDocumentComment[] ParseXmlComment(XDocument xDocument) => ParseXmlComment(xDocument, null);

        // cheap, quick hack parser:)
        internal static XmlDocumentComment[] ParseXmlComment(XDocument xDocument, string namespaceMatch)
        {

            string assemblyName = xDocument.Descendants("assembly").First().Elements("name").First().Value;

            return xDocument.Descendants("member")
                .Select(x =>
                {
                    Match match = Regex.Match(x.Attribute("name").Value, @"(.):(.+)\.([^.()]+)?(\(.+\)|$)");
                    if (!match.Groups[1].Success)
                    {
                        return null;
                    }

                    MemberType memberType = (MemberType)match.Groups[1].Value[0];
                    if (memberType == MemberType.None)
                    {
                        return null;
                    }

                    string summaryXml = x.Elements("summary").FirstOrDefault()?.ToString()
                        ?? x.Element("summary")?.ToString()
                        ?? "";
                    summaryXml = Regex.Replace(summaryXml, @"<\/?summary>", string.Empty);
                    summaryXml = Regex.Replace(summaryXml, @"<para\s*/>", Environment.NewLine);
                    summaryXml = Regex.Replace(summaryXml, @"<see cref=""\w:([^\""]*)""\s*\/>", m => ResolveSeeElement(m, assemblyName));

                    string parsed = Regex.Replace(summaryXml, @"<(type)*paramref name=""([^\""]*)""\s*\/>", e => $"`{e.Groups[1].Value}`");

                    string summary = parsed;

                    if (summary != "")
                    {
                        summary = string.Join("  ", summary.Split(new[] { "\r", "\n", "\t" }, StringSplitOptions.RemoveEmptyEntries).Select(y => y.Trim()));
                    }

                    string returns = ((string)x.Element("returns")) ?? "";
                    string remarks = ((string)x.Element("remarks")) ?? "";
                    Dictionary<string, string> parameters = x.Elements("param")
                        .Select(e => Tuple.Create(e.Attribute("name").Value, e))
                        .Distinct(new Item1EqualityCompaerer<string, XElement>())
                        .ToDictionary(e => e.Item1, e => e.Item2.Value);

                    string className = (memberType == MemberType.Type)
                        ? match.Groups[2].Value + "." + match.Groups[3].Value
                        : match.Groups[2].Value;

                    return new XmlDocumentComment
                    {
                        MemberType = memberType,
                        ClassName = className,
                        MemberName = match.Groups[3].Value,
                        Summary = summary.Trim(),
                        Remarks = remarks.Trim(),
                        Parameters = parameters,
                        Returns = returns.Trim()
                    };
                })
                .Where(x => x != null)
                .ToArray();
        }

        private static string ResolveSeeElement(Match m, string ns)
        {
            string typeName = m.Groups[1].Value;
            if (!string.IsNullOrWhiteSpace(ns))
            {
                if (typeName.StartsWith(ns))
                {
                    return $"[{typeName}]({Regex.Replace(typeName, $"\\.(?:.(?!\\.))+$", me => me.Groups[0].Value.Replace(".", "#").ToLower())})";
                }
            }
            return $"`{typeName}`";
        }

        private class Item1EqualityCompaerer<T1, T2> : EqualityComparer<Tuple<T1, T2>>
        {
            public override bool Equals(Tuple<T1, T2> x, Tuple<T1, T2> y) => x.Item1.Equals(y.Item1);

            public override int GetHashCode(Tuple<T1, T2> obj) => obj.Item1.GetHashCode();
        }
    }
}
