using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IslandIs.Skjalaveita.Api.Models;
using Microsoft.Extensions.Logging;

namespace IslandIs.Skjalaveita.Api.Services
{
    public class DemoDocumentService : IDocumentService
    {
        private ILogger<DemoDocumentService> _logger;

        public DemoDocumentService(ILogger<DemoDocumentService> logger)
        {
            _logger = logger;
            this.DemoType = "pdf";
        }

        public string DemoType { get; set; }

        public async Task<Document> GetDocument(string kennitala, string documentId)
        {
            var document = new Document();
            await Task.Run(() =>
            {
                if (DemoType == "html")
                {
                    document.Type = "html";
                    document.Content = "<html>" +
                                  "<body>" +
                                  "<p>hello world</p>" +
                                  "</body>" +
                                  "</html>";
                }
                else
                {
                    var pdfFile = System.IO.File.ReadAllBytes(@".\data\demo.pdf");
                    document.Type = "pdf";
                    document.Content = Convert.ToBase64String(pdfFile);
                }
            });
            return document;
        }

        public bool ValidDocumentId(string documentId)
        {
            return documentId.Length > 3;
        }

        public bool ValidKennitala(string kennitala)
        {
            // Check if string is not empty and is 10 numeric chars.
            if (string.IsNullOrEmpty(kennitala) || kennitala.Length != 10 || !long.TryParse(kennitala, out _))
            {
                return false;
            }
            int kennitalaSum =
                (int.Parse(kennitala.Substring(0, 1)) * 3) +
                (int.Parse(kennitala.Substring(1, 1)) * 2) +
                (int.Parse(kennitala.Substring(2, 1)) * 7) +
                (int.Parse(kennitala.Substring(3, 1)) * 6) +
                (int.Parse(kennitala.Substring(4, 1)) * 5) +
                (int.Parse(kennitala.Substring(5, 1)) * 4) +
                (int.Parse(kennitala.Substring(6, 1)) * 3) +
                (int.Parse(kennitala.Substring(7, 1)) * 2);
            int modulus = kennitalaSum % 11;
            int checkSum = modulus == 0 ? 0 : 11 - modulus;
            return int.Parse(kennitala.Substring(8, 1)) == checkSum;
        }
    }
}
