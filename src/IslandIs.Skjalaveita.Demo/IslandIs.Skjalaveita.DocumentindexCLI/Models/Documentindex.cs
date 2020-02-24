using System;
using System.ComponentModel.DataAnnotations;

namespace IslandIs.Skjalaveita.DocumentindexCLI.Models
{
    public class Documentindex
    {
        [Required]
        public string Kennitala { get; set; }

        [Required]
        public string DocumentId { get; set; }

        [Required]
        public string SenderKennitala { get; set; }

        [Required]
        public string SenderName { get; set; }

        public string AuthorKennitala { get; set; }

        public string CaseId { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Type { get; set; }

        public string SubType { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public DateTime DocumentDate { get; set; }

        public DateTime? PublicationDate { get; set; }

        public bool NotifyOwner { get; set; }

        public string MinimumAuthenticationType { get; set; }
    }
}
