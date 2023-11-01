using BlazoryVS.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BlazoryVS.Helpers
{
    /// <summary>
    /// Represents a helper class for serializing snippets.
    /// </summary>
    internal static class SnippetSerializationHelper
    {
        /// <summary>
        /// Serializes the snippet to xml.
        /// </summary>
        /// <param name="snippet">Snippet object.</param>
        /// <returns>String containing snippet in xml form.</returns>
        public static string SerializeSnippet(Snippet snippet)
        {
            try
            {
                XNamespace ns = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";

                var declarations = new List<XElement>();

                if (snippet.Literals != null && snippet.Literals.Length > 0)
                {
                    declarations.Add(new XElement(ns + "Declarations",
                        snippet.Literals.Select(literal =>
                            new XElement(ns + "Literal",
                                new XElement(ns + "ID", literal.Id ?? string.Empty),
                                new XElement(ns + "Default", literal.Default ?? string.Empty)
                            )
                        )
                    ));
                }

                var doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", null),
                    new XElement(ns + "CodeSnippets",
                        new XElement(ns + "CodeSnippet",
                            new XAttribute("Format", "1.0.0"),
                            new XElement(ns + "Header",
                                new XElement(ns + "Title", snippet.Name ?? string.Empty),
                                new XElement(ns + "Author", snippet.Author ?? string.Empty),
                                new XElement(ns + "Description", snippet.Description ?? string.Empty),
                                new XElement(ns + "Shortcut", snippet.Prefix ?? string.Empty)
                            ),
                            new XElement(ns + "Snippet",
                                new XElement(ns + "Code",
                                    new XAttribute("Language", snippet.Language ?? string.Empty),
                                    new XCData(snippet.Code ?? string.Empty)
                                ),
                                declarations
                            )
                        )
                    )
                );

                using (var memoryStream = new MemoryStream())
                {
                    using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                    {
                        using (var xmlWriter = XmlWriter.Create(streamWriter, new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true }))
                        {
                            doc.WriteTo(xmlWriter);
                        }
                    }

                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
