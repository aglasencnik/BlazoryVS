using BlazoryVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlazoryVS.Helpers
{
    /// <summary>
    /// Represents a helper class for parsing snippet code.
    /// </summary>
    internal static class CodeParserHelper
    {
        /// <summary>
        /// Parses the snippet code.
        /// </summary>
        /// <param name="code">Snippet code string.</param>
        /// <returns>A tuple containing parsed code string and an array of Literal objects.</returns>
        public static (string, Literal[]) ParseCode(string code)
        {
            try
            {
                var regex = new Regex(@"\$\{(\d+)(?:\:([^}|]+))?(\|([^}]+)\|)?\}|(\$0)");

                var literalsDict = new Dictionary<string, Literal>();

                var vsFormatted = regex.Replace(code, match =>
                {
                    // Handle the special case of $0 (final cursor position in VS Code)
                    if (match.Value == "$0")
                        return "$end$";

                    var id = match.Groups[1].Value;
                    var defaultValue = match.Groups[2].Value;

                    // Handle choices: We'll default to the first choice for VS 
                    // as VS doesn't support choices directly in the snippet
                    if (string.IsNullOrWhiteSpace(defaultValue))
                    {
                        var choices = match.Groups[4].Value?.Split(',');
                        if (choices != null && choices.Length > 0)
                        {
                            defaultValue = choices[0];
                        }
                    }

                    if (!literalsDict.ContainsKey(id))
                    {
                        literalsDict[id] = new Literal { Id = id, Default = defaultValue };
                    }

                    return $"${id}$";
                });

                return (vsFormatted, literalsDict.Values.ToArray());
            }
            catch
            {
                return (string.Empty, Array.Empty<Literal>());
            }
        }
    }
}
