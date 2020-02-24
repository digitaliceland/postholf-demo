
namespace IslandIs.Skjalaveita.Api.Models
{
    public class Document
    {
        /// <summary>
        /// Type of the document.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Base64 encoded content.  
        /// Except if docment is type html or url it should be plain text.
        /// Notice html can not contain any script or references.
        /// All content needs to be inlined.
        /// </summary>
        public string Content { get; set; }
    }
}
