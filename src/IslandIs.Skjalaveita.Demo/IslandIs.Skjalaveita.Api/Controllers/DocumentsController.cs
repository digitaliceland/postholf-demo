using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IslandIs.Skjalaveita.Api.Models;
using IslandIs.Skjalaveita.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IslandIs.Skjalaveita.Api.Controllers
{
    [Route("api/v1/customers")]
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;
        private ILogger<DocumentsController> _logger;

        public DocumentsController(ILogger<DocumentsController> logger, IDocumentService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        /// <summary>
        /// Callback service for Pósthólfið at Ísland.is.  Is called when user opens document.
        /// </summary>
        /// <param name="kennitala">Kennitala for the owner (end user).</param>
        /// <param name="documentId">Id of the document on the provider side.</param>
        /// <param name="authenticationType">Query parameter indicating how end user was authenticated</param>
        /// <returns></returns>
        [HttpGet("{kennitala}/[controller]/{documentId}")]
        [Authorize]
        [ProducesResponseType(typeof(Document), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Document>> Get(string kennitala, string documentId, string authenticationType)
        {
            string[] allowedAuthTypes = { "LOW", "SUBSTANTIAL", "HIGH" };
            if (!allowedAuthTypes.Contains(authenticationType))
            {
                _logger.LogWarning("Authentication type is not allowed", new { kennitala, documentId, authenticationType});
                return BadRequest(new Error { ErrorMessage = "Authentication type is not allowed." });
            }

            _logger.LogInformation("Starting Get Document", new { kennitala, documentId });
            // Validate kennitala and DocumentId
            // NOTE: You should also check if kennitala and skjalId is a pair.
            if (_documentService.ValidKennitala(kennitala)
                && _documentService.ValidDocumentId(documentId))
            {
                _logger.LogInformation("Kannitala and SkjalId is valid");
                return await _documentService.GetDocument(kennitala, documentId);
            }
            else
            {
                _logger.LogWarning("Kennitala and/or skjalId is not valid", new { kennitala, documentId });
                // Kennitala and/or DocumentId not valid.
                return BadRequest(new Error { ErrorMessage = "Kennitala and/or skjalId is not valid." });
            }

        }

    }
}
