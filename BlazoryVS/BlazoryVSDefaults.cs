namespace BlazoryVS
{
    /// <summary>
    /// Represents the default constant values.
    /// </summary>
    internal class BlazoryVSDefaults
    {
        #region Path constants

        /// <summary>
        /// Gets the name of the folder that contains the snippets.
        /// </summary>
        public const string SnippetsFolderName = "Snippets";

        /// <summary>
        /// Gets the name of the folder that contains the C# snippets.
        /// </summary>
        public const string CSharpSnippetsFolderName = "Blazory CSharp Snippets";

        /// <summary>
        /// Gets the name of the folder that contains the Razor snippets.
        /// </summary>
        public const string RazorSnippetsFolderName = "Blazory Razor Snippets";

        #endregion

        #region Snippet constants

        /// <summary>
        /// Gets the name of the snippet author.
        /// </summary>
        public const string SnippetAuthor = "Blazory";

        /// <summary>
        /// Gets the name of the C# snippet language.
        /// </summary>
        public const string CSharpSnippetLanguage = "CSharp";

        /// <summary>
        /// Gets the name of the Razor snippet language.
        /// </summary>
        public const string RazorSnippetLanguage = "Razor";

        /// <summary>
        /// Gets the name of the placeholder snippet.
        /// </summary>
        public const string PlaceholderSnippetName = "placeholder.snippet";

        #endregion

        #region Blazory repository constants

        /// <summary>
        /// Gets the URL of the Blazory C# snippets JSON file.
        /// </summary>
        public const string CSharpSnippetsJsonUrl = "https://raw.githubusercontent.com/bartvanhoey/Blazory/master/snippets/csharp.json";

        /// <summary>
        /// Gets the URL of the Blazory Razor snippets JSON file.
        /// </summary>
        public const string RazorSnippetsJsonUrl = "https://raw.githubusercontent.com/bartvanhoey/Blazory/master/snippets/razor.json";

        #endregion

        #region Settings constants

        /// <summary>
        /// Gets the name of the C# snippets setting.
        /// </summary>
        public const string CSharpSnippetsSettingName = "CSharpSnippets";

        /// <summary>
        /// Gets the name of the Razor snippets setting.
        /// </summary>
        public const string RazorSnippetsSettingName = "RazorSnippets";

        #endregion
    }
}
