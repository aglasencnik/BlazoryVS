using BlazoryVS.Enums;
using BlazoryVS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazoryVS.Services
{
    /// <summary>
    /// Represents a service for managing snippets.
    /// </summary>
    internal class SnippetService
    {
        /// <summary>
        /// Gets the snippets from the specified JSON url.
        /// </summary>
        /// <param name="jsonUrl">Snippet JSON url.</param>
        /// <returns>
        /// An array of snippet objects.
        /// A task that represents the asynchronous operation.
        /// </returns>
        public static async Task<Snippet[]> GetSnippetsAsync(string jsonUrl)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                return Array.Empty<Snippet>();
            }
        }

        /// <summary>
        /// Applies the snippets.
        /// </summary>
        /// <param name="snippetType">Snippet type.</param>
        /// <param name="snippets">Array of snippet objects.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ApplySnippetsAsync(SnippetType snippetType, Snippet[] snippets)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Removes the placeholder snippets.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task RemovePlaceholderSnippetsAsync()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Generates the snippet comparison report.
        /// </summary>
        /// <param name="snippetType">Snippet type.</param>
        /// <param name="snippets">Array of snippet objects.</param>
        /// <returns>
        /// Snippet comparison report object.
        /// A task that represents the asynchronous operation.
        /// </returns>
        public static async Task<SnippetComparisonReport> GenerateSnippetComparisonReportAsync(SnippetType snippetType, Snippet[] snippets)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                return new SnippetComparisonReport();
            }
        }
    }
}
