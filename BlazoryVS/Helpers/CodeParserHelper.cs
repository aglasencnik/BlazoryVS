using BlazoryVS.Models;
using System;

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
                throw new NotImplementedException();
            }
            catch
            {
                return (string.Empty, Array.Empty<Literal>());
            }
        }
    }
}
