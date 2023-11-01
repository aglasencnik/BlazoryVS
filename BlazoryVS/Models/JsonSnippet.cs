using Newtonsoft.Json.Linq;

namespace BlazoryVS.Models
{
    /// <summary>
    /// Represents the JSON snippet model.
    /// </summary>
    internal class JsonSnippet
    {
        /// <summary>
        /// Gets or sets the snippet body.
        /// </summary>
        public JToken Body { get; set; }

        /// <summary>
        /// Gets or sets the snippet description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the snippet prefix.
        /// </summary>
        public string Prefix { get; set; }
    }

}
