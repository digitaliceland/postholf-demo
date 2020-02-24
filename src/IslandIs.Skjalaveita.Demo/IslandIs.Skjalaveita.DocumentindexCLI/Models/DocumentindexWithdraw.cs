using System.ComponentModel.DataAnnotations;

namespace IslandIs.Skjalaveita.DocumentindexCLI.Models
{
    public class DocumentindexWithdraw
    {
        [Required]
        public string Kennitala { get; set; }

        [Required]
        public string DocumentId { get; set; }

        [Required]
        public string Reason { get; set; }
    }
}
