using BlazoryVS.Enums;
using BlazoryVS.Models;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using Newtonsoft.Json;
using System;

namespace BlazoryVS.Services
{
    /// <summary>
    /// Represents the settings service.
    /// </summary>
    internal class SettingsService
    {
        /// <summary>
        /// Gets the last snippets.
        /// </summary>
        /// <param name="settingsManager">Shell settings manager object.</param>
        /// <param name="snippetType">Snippet type.</param>
        /// <returns>Array of Snippet objects.</returns>
        public static Snippet[] GetLastSnippets(ShellSettingsManager settingsManager, SnippetType snippetType)
        {
            try
            {
                var settingsStore = settingsManager.GetReadOnlySettingsStore(SettingsScope.UserSettings);
                var settingsProperty = string.Empty;

                if (snippetType == SnippetType.CSharp)
                    settingsProperty = BlazoryVSDefaults.CSharpSnippetsSettingName;
                else if (snippetType == SnippetType.Razor)
                    settingsProperty = BlazoryVSDefaults.RazorSnippetsSettingName;

                if (!settingsStore.TryGetString(Vsix.Name, settingsProperty, out string storedSnippets) || string.IsNullOrWhiteSpace(storedSnippets))
                    return Array.Empty<Snippet>();

                return JsonConvert.DeserializeObject<Snippet[]>(storedSnippets) ?? Array.Empty<Snippet>();
            }
            catch
            {
                return Array.Empty<Snippet>();
            }
        }

        /// <summary>
        /// Saves the snippets to the settings.
        /// </summary>
        /// <param name="settingsManager">Shell settings manager object.</param>
        /// <param name="snippetType">Snippet type.</param>
        /// <param name="snippets">Array of Snippet objects.</param>
        public static void SaveToLastSnippets(ShellSettingsManager settingsManager, SnippetType snippetType, Snippet[] snippets)
        {
            try
            {
                var settingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
                var settingsProperty = string.Empty;

                if (snippetType == SnippetType.CSharp)
                    settingsProperty = BlazoryVSDefaults.CSharpSnippetsSettingName;
                else if (snippetType == SnippetType.Razor)
                    settingsProperty = BlazoryVSDefaults.RazorSnippetsSettingName;

                if (!settingsStore.CollectionExists(Vsix.Name))
                    settingsStore.CreateCollection(Vsix.Name);

                var serializedSnippets = JsonConvert.SerializeObject(snippets);
                if (!string.IsNullOrWhiteSpace(serializedSnippets))
                    settingsStore.SetString(Vsix.Name, settingsProperty, serializedSnippets);
            }
            catch
            {
            }
        }
    }
}
