using BlazoryVS.Enums;
using BlazoryVS.Helpers;
using BlazoryVS.Models;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
        /// <param name="language">Snippet language.</param>
        /// <param name="author">Snippet author.</param>
        /// <returns>
        /// An array of snippet objects.
        /// A task that represents the asynchronous operation.
        /// </returns>
        public static async Task<Snippet[]> GetSnippetsAsync(string jsonUrl, string language, string author)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonUrl) || string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(author))
                    return Array.Empty<Snippet>();

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync(jsonUrl);

                    if (string.IsNullOrWhiteSpace(response))
                        return Array.Empty<Snippet>();

                    return SnippetDeserializationHelper.DeserializeJsonToSnippets(response, language, author);
                }
            }
            catch
            {
                return Array.Empty<Snippet>();
            }
        }

        /// <summary>
        /// Generates the snippet comparison report.
        /// </summary>
        /// <param name="currentSnippets">Array of snippet objects.</param>
        /// <param name="oldSnippets">Array of snippet objects.</param>
        /// <returns>Snippet comparison report object.</returns>
        public static SnippetComparisonReport GenerateSnippetComparisonReport(Snippet[] currentSnippets, Snippet[] oldSnippets)
        {
            try
            {
                if (currentSnippets == null || !currentSnippets.Any())
                    return new SnippetComparisonReport();

                var snippetsToBeEdited = new List<Snippet>();
                var snippetsToBeDeleted = new List<string>();

                var oldSnippetDict = oldSnippets.ToDictionary(s => s.Name);

                foreach (var currentSnippet in currentSnippets)
                {
                    if (!oldSnippetDict.ContainsKey(currentSnippet.Name))
                        snippetsToBeEdited.Add(currentSnippet);
                    else
                    {
                        var oldSnippet = oldSnippetDict[currentSnippet.Name];
                        if (currentSnippet.Author != oldSnippet.Author ||
                            currentSnippet.Language != oldSnippet.Language ||
                            currentSnippet.Prefix != oldSnippet.Prefix ||
                            currentSnippet.Description != oldSnippet.Description ||
                            currentSnippet.Code != oldSnippet.Code ||
                            (currentSnippet.Literals.Count() == oldSnippet.Literals.Length &&
                             !currentSnippet.Literals.Zip(oldSnippet.Literals, (c, o) => new { Current = c, Old = o })
                             .All(pair => pair.Current.Id == pair.Old.Id && pair.Current.Default == pair.Old.Default)))
                        {
                            snippetsToBeEdited.Add(currentSnippet);
                        }

                        oldSnippetDict.Remove(currentSnippet.Name);
                    }
                }

                snippetsToBeDeleted.AddRange(oldSnippetDict.Keys);

                return new SnippetComparisonReport
                {
                    SnippetsToBeEdited = snippetsToBeEdited.ToArray(),
                    SnippetsToBeDeleted = snippetsToBeDeleted.ToArray()
                };
            }
            catch
            {
                return new SnippetComparisonReport();
            }
        }

        /// <summary>
        /// Applies snippets.
        /// </summary>
        /// <param name="settingsManager">Shell settings manager object.</param>
        /// <param name="snippetType">Snippet type.</param>
        /// <param name="snippets">Array of Snippet objects.</param>
        public static void ApplySnippets(ShellSettingsManager settingsManager, SnippetType snippetType, Snippet[] snippets)
        {
            try
            {
                if (settingsManager == null || snippets == null || !snippets.Any())
                    return;

                var extensionsPath = settingsManager.GetApplicationDataFolder(ApplicationDataFolder.UserExtensions);

                if (string.IsNullOrWhiteSpace(extensionsPath))
                    return;

                var snippetFolderName = (snippetType == SnippetType.CSharp) ? BlazoryVSDefaults.CSharpSnippetsFolderName : BlazoryVSDefaults.RazorSnippetsFolderName;
                var snippetsFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), BlazoryVSDefaults.SnippetsFolderName, snippetFolderName);

                if (!Directory.Exists(snippetsFolderPath))
                    return;

                foreach (var snippet in snippets)
                {
                    var serializedSnippet = SnippetSerializationHelper.SerializeSnippet(snippet);
                    var fileName = $"{PathHelper.SanitizeFileName(snippet.Name)}.snippet";

                    File.WriteAllText(Path.Combine(snippetsFolderPath, fileName), serializedSnippet);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Removes the placeholder snippets.
        /// </summary>
        /// <param name="settingsManager">Shell settings manager object.</param>
        public static void RemovePlaceholderSnippets(ShellSettingsManager settingsManager)
        {
            try
            {
                if (settingsManager == null)
                    return;

                var extensionsPath = settingsManager.GetApplicationDataFolder(ApplicationDataFolder.UserExtensions);

                if (string.IsNullOrWhiteSpace(extensionsPath))
                    return;

                var snippetsFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), BlazoryVSDefaults.SnippetsFolderName);

                if (!Directory.Exists(snippetsFolderPath))
                    return;

                var csharpSnippetsFolderPath = Path.Combine(snippetsFolderPath, BlazoryVSDefaults.CSharpSnippetsFolderName);
                var razorSnippetsFolderPath = Path.Combine(snippetsFolderPath, BlazoryVSDefaults.RazorSnippetsFolderName);

                if (Directory.Exists(csharpSnippetsFolderPath) && File.Exists(Path.Combine(csharpSnippetsFolderPath, BlazoryVSDefaults.PlaceholderSnippetName)))
                    File.Delete(Path.Combine(csharpSnippetsFolderPath, BlazoryVSDefaults.PlaceholderSnippetName));

                if (Directory.Exists(razorSnippetsFolderPath) && File.Exists(Path.Combine(razorSnippetsFolderPath, BlazoryVSDefaults.PlaceholderSnippetName)))
                    File.Delete(Path.Combine(razorSnippetsFolderPath, BlazoryVSDefaults.PlaceholderSnippetName));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Removes the snippets.
        /// </summary>
        /// <param name="settingsManager">Shell settings manager object.</param>
        /// <param name="snippetType">Snippet type.</param>
        /// <param name="snippets">Array of snippet names.</param>
        public static void RemoveSnippets(ShellSettingsManager settingsManager, SnippetType snippetType, string[] snippets)
        {
            try
            {
                if (settingsManager == null || snippets == null || !snippets.Any())
                    return;

                var extensionsPath = settingsManager.GetApplicationDataFolder(ApplicationDataFolder.UserExtensions);

                if (string.IsNullOrWhiteSpace(extensionsPath))
                    return;

                var snippetFolderName = (snippetType == SnippetType.CSharp) ? BlazoryVSDefaults.CSharpSnippetsFolderName : BlazoryVSDefaults.RazorSnippetsFolderName;
                var snippetsFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), BlazoryVSDefaults.SnippetsFolderName, snippetFolderName);

                if (!Directory.Exists(snippetsFolderPath))
                    return;

                foreach (var snippet in snippets)
                {
                    var snippetPath = Path.Combine(snippetsFolderPath, $"{snippet}.snippet");

                    if (File.Exists(snippetPath))
                        File.Delete(snippetPath);
                }
            }
            catch
            {
            }
        }
    }
}
