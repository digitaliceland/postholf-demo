using System;
using System.ComponentModel.DataAnnotations;

namespace IslandIs.Skjalaveita.DocumentindexCLI.Models
{
    public class Result
    {
        public string Kennitala { get; set; }
        public string DocumentId { get; set; }
        public bool Success { get; set; }
        public string[] Errors { get; set; }

    }
}
