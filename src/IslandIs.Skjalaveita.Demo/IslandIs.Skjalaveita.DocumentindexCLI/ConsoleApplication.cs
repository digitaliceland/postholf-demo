using IslandIs.Skjalaveita.Api.Services;
using IslandIs.Skjalaveita.DocumentindexCLI.Models;
using IslandIs.Skjalaveita.DocumentindexCLI.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace IslandIs.Skjalaveita.DocumentindexCLI
{
    public class ConsoleApplication
    {
        private ILogger _logger;
        private ConsoleApplicationSettings _config;
        private DocumentindexService _documentindexService;

        public ConsoleApplication(ILogger<ConsoleApplication> logger, IOptions<ConsoleApplicationSettings> config, DocumentindexService documentindexService)
        {
            _documentindexService = documentindexService;
            _logger = logger;
            _config = config.Value;
        }

        public void Run(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return;
            }

            switch (args[0])
            {
                case "/c":
                    GetCategories(args).Wait();
                    break;
                case "/t":
                    GetTypes(args).Wait();
                    break;
                case "/n":
                    CreateDocumentindex(args).Wait();
                    break;
                case "/r":
                    Read(args).Wait();
                    break;
                case "/w":
                    Withdraw(args).Wait();
                    break;
                default:
                    PrintUsage();
                    break;
            }
            return;
        }

        public async Task GetCategories(string[] args)
        {
            try
            {
                var categories = await _documentindexService.GetCategories();

                Console.WriteLine("");
                Console.WriteLine("Catergories:");
                foreach (string category in categories)
                {
                    Console.WriteLine("  " + category);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception getting categories.");
                Console.WriteLine(ex.Message);
            }
        }

        public async Task GetTypes(string[] args)
        {
            try
            {
                var types = await _documentindexService.GetTypes();

                Console.WriteLine("");
                Console.WriteLine("Types:");
                foreach (string type in types)
                {
                    Console.WriteLine("  " + type);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception getting types.");
            }
        }

        public async Task CreateDocumentindex(string[] args)
        {
            if (args.Length != 5)
            {
                PrintUsage();
                return;
            }
            
            try
            {
                var documentId = Guid.NewGuid();

                var docindex = new Documentindex()
                {
                    Kennitala = args[1],
                    DocumentId = documentId.ToString(),
                    SenderKennitala = _config.SenderKennitala,
                    SenderName = _config.SenderName,
                    Category = args[3],
                    Type = args[4],
                    Subject = args[2],
                    DocumentDate = DateTime.Now
                };

                var result = await _documentindexService.CreateDocumentindex(new Documentindex[] { docindex });

                Console.WriteLine("");
                Console.WriteLine("Success:{0}", result[0].Success);
                Console.WriteLine("DocumentId:{0}", result[0].DocumentId);
                if (result[0].Errors != null && result[0].Errors.Length > 0)
                {
                    Console.WriteLine("Error:{0}", result[0].Errors[0]);
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception creation new document index.");
            }
        }

        public async Task Read(string[] args)
        {
            if (args.Length != 3)
            {
                PrintUsage();
                return;
            }

            try
            {
                var docindexAction = new DocumentindexRead()
                {
                    Kennitala = args[1],
                    DocumentId = args[2]
                };

                var result = await _documentindexService.MarkRead(new DocumentindexRead[] { docindexAction });

                Console.WriteLine("");
                Console.WriteLine("Success:{0}", result[0].Success);
                Console.WriteLine("DocumentId:{0}", result[0].DocumentId);
                if (result[0].Errors != null && result[0].Errors.Length > 0)
                {
                    Console.WriteLine("Error:{0}", result[0].Errors[0]);
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception marking document read.");
            }
        }

        public async Task Withdraw(string[] args)
        {
            if (args.Length != 3)
            {
                PrintUsage();
                return;
            }

            try
            {
                var docindexAction = new DocumentindexWithdraw()
                {
                    Kennitala = args[1],
                    DocumentId = args[2],
                    Reason = "Withrawn in demo console."
                };

                var result = await _documentindexService.MarkWitdrawn(new DocumentindexWithdraw[] { docindexAction });

                Console.WriteLine("");
                Console.WriteLine("Success:{0}", result[0].Success);
                Console.WriteLine("DocumentId:{0}", result[0].DocumentId);
                if (result[0].Errors != null && result[0].Errors.Length > 0)
                {
                    Console.WriteLine("Error:{0}", result[0].Errors[0]);
                }

                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception marking document withdrawn.");
            }
        }

        private void PrintUsage()
        {
            Console.WriteLine("");
            Console.WriteLine("Usage: DocumentindexCLI [/command] [attributes..]       ");
            Console.WriteLine("  /c           Get available categories");
            Console.WriteLine("  /t           Get available types");
            Console.WriteLine("  /n           Creates new documentindex.  Returns id of the document (documentId).");
            Console.WriteLine("  attributes    kennitala");
            Console.WriteLine("                subject");
            Console.WriteLine("                category");
            Console.WriteLine("                type");
            Console.WriteLine("  /w           Withdraws documentindex.");
            Console.WriteLine("  attributes    kennitala");
            Console.WriteLine("                documentId");
            Console.WriteLine("  /r           Marks document as read.");
            Console.WriteLine("  attributes    kennitala");
            Console.WriteLine("                documentId");

        }
    }
}
