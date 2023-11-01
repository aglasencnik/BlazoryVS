namespace BlazoryVS.Models
{
    /// <summary>
    /// Represents the snippet comparison report model.
    /// </summary>
    internal class SnippetComparisonReport
    {
        /// <summary>
        /// Gets or sets the snippets to be added or changed.
        /// </summary>
        public Snippet[] SnippetsToBeEdited { get; set; }

        /// <summary>
        /// Gets or sets the snippets to be deleted.
        /// </summary>
        public string[] SnippetsToBeDeleted { get; set; }
    }
}
