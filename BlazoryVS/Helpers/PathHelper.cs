using System.IO;
using System.Linq;

namespace BlazoryVS.Helpers
{
    /// <summary>
    /// Represents a helper class for path operations.
    /// </summary>
    internal class PathHelper
    {
        /// <summary>
        /// Sanitizes the file name.
        /// </summary>
        /// <param name="fileName">Proposed filename.</param>
        /// <returns>Sanitized file name string.</returns>
        public static string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return new string(fileName.Where(ch => !invalidChars.Contains(ch)).ToArray());
        }
    }
}
