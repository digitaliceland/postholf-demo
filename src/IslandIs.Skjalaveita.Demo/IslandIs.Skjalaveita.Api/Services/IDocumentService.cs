using IslandIs.Skjalaveita.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IslandIs.Skjalaveita.Api.Services
{
    public interface IDocumentService
    {
        bool ValidKennitala(string kennitala);
        bool ValidDocumentId(string documentId);
        Task<Document> GetDocument(string kennitala, string documentId);

    }
}
