using BlazoryVS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BlazoryVS.Helpers
{
    /// <summary>
    /// Represents the snippet deserialization helper class.
    /// </summary>
    internal static class SnippetDeserializationHelper
    {
        /// <summary>
        /// Deserializes the JSON string to a snippet array.
        /// </summary>
        /// <param name="json">JSON containing snippet objects.</param>
        /// <param name="language">Snippet language.</param>
        /// <param name="author">Snippet author.</param>
        /// <returns>Array of Snippet objects.</returns>
        public static Snippet[] DeserializeJsonToSnippets(string json, string language, string author)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(author))
                    return Array.Empty<Snippet>();

                var jObjects = JsonConvert.DeserializeObject<Dictionary<string, JsonSnippet>>(json);
                var snippets = new List<Snippet>();

                foreach (var kvp in jObjects)
                {
                    var jsonSnippet = kvp.Value;

                    var code = string.Empty;

                    if (jsonSnippet.Body.Type == JTokenType.String)
                        code = jsonSnippet.Body.ToString();
                    else if (jsonSnippet.Body.Type == JTokenType.Array)
                        code = string.Join("\n", jsonSnippet.Body.ToObject<string[]>());

                    var parsedCodeAndLiterals = CodeParserHelper.ParseCode(code);

                    snippets.Add(new Snippet
                    {
                        Name = kvp.Key,
                        Author = author,
                        Language = language,
                        Prefix = jsonSnippet.Prefix,
                        Description = jsonSnippet.Description,
                        Code = parsedCodeAndLiterals.Item1,
                        Literals = parsedCodeAndLiterals.Item2
                    });
                }

                return snippets.ToArray();
            }
            catch
            {
                return Array.Empty<Snippet>();
            }
        }
    }
}
