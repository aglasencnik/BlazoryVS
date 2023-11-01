using BlazoryVS.Enums;
using BlazoryVS.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace BlazoryVS
{
    /// <summary>
    /// Represents the entry extension class.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(BlazoryVSPackage.PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class BlazoryVSPackage : AsyncPackage
    {
        /// <summary>
        /// Gets the BlazoryVSPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "a0775924-0734-4872-b19d-3a1a56325a58";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            try
            {
                // Gets the snippets from the specified JSON url.
                var csharpSnippets = await SnippetService.GetSnippetsAsync(BlazoryVSDefaults.CSharpSnippetsJsonUrl, BlazoryVSDefaults.CSharpSnippetLanguage, BlazoryVSDefaults.SnippetAuthor);
                var razorSnippets = await SnippetService.GetSnippetsAsync(BlazoryVSDefaults.RazorSnippetsJsonUrl, BlazoryVSDefaults.RazorSnippetLanguage, BlazoryVSDefaults.SnippetAuthor);

                // Switch to main thread.
                await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

                // Initialize settings manager.
                var settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);

                // Removes the placeholder snippets.
                SnippetService.RemovePlaceholderSnippets(settingsManager);

                // Gets the old snippets from the settings.
                var oldCsharpSnippets = SettingsService.GetLastSnippets(settingsManager, SnippetType.CSharp);
                var oldRazorSnippets = SettingsService.GetLastSnippets(settingsManager, SnippetType.Razor);

                // Generates the snippet comparison reports.
                var csharpSnippetsComparison = SnippetService.GenerateSnippetComparisonReport(csharpSnippets, oldCsharpSnippets);
                var razorSnippetsComparison = SnippetService.GenerateSnippetComparisonReport(razorSnippets, oldRazorSnippets);

                // Removes the snippets that are not in the JSON.
                SnippetService.RemoveSnippets(settingsManager, SnippetType.CSharp, csharpSnippetsComparison.SnippetsToBeDeleted);
                SnippetService.RemoveSnippets(settingsManager, SnippetType.Razor, razorSnippetsComparison.SnippetsToBeDeleted);

                // Adds the snippets that are different than in the JSON.
                SnippetService.ApplySnippets(settingsManager, SnippetType.CSharp, csharpSnippetsComparison.SnippetsToBeEdited);
                SnippetService.ApplySnippets(settingsManager, SnippetType.Razor, razorSnippetsComparison.SnippetsToBeEdited);

                // Update old snippets to the new ones.
                SettingsService.SaveToLastSnippets(settingsManager, SnippetType.CSharp, csharpSnippets);
                SettingsService.SaveToLastSnippets(settingsManager, SnippetType.Razor, razorSnippets);
            }
            catch
            {
            }
        }

        #endregion
    }
}
