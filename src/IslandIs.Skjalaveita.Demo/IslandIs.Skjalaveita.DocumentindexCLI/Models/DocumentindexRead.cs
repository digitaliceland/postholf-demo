using System.ComponentModel.DataAnnotations;

namespace IslandIs.Skjalaveita.DocumentindexCLI.Models
{
    public class DocumentindexRead
    {
        [Required]
        public string Kennitala { get; set; }

        [Required]
        public string DocumentId { get; set; }
    }
}
