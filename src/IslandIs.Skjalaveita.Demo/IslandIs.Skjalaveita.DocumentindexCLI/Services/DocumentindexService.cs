using IslandIs.Skjalaveita.DocumentindexCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IslandIs.Skjalaveita.DocumentindexCLI.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityModel.Client;
using System.Text;
using System.Text.Json;

namespace IslandIs.Skjalaveita.Api.Services
{
    public class DocumentindexService
    {
        private ILogger<DocumentindexService> _logger;
        private DocumentindexServiceSettings _config;
        private HttpClient _httpTokenClient = new HttpClient();
        private HttpClient _httpClient = new HttpClient();

        public DocumentindexService(IOptions<DocumentindexServiceSettings> config, ILogger<DocumentindexService> logger)
        {
            _logger = logger;
            _config = config.Value;
            _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        }

        public async Task<string[]> GetCategories()
        {
            await updateAccessToken();
            // Make async call and return result.
            HttpResponseMessage response = await _httpClient.GetAsync("categories");
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<string[]>(contentStream);
            }
        }

        public async Task<string[]> GetTypes()
        {
            await updateAccessToken();
            // Make async call and return result.
            HttpResponseMessage response = await _httpClient.GetAsync("types");
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<string[]>(contentStream);
            }
        }

        public async Task<Result[]> CreateDocumentindex(Documentindex[] documents)
        {
            await updateAccessToken();

            var json = JsonSerializer.Serialize(documents);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Make async call.
            HttpResponseMessage response = await _httpClient.PostAsync("", stringContent);
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<Result[]>(contentStream);
            }
        }
        public async Task<Result[]> MarkRead(DocumentindexRead[] documentindexRead)
        {
            await updateAccessToken();

            var json = JsonSerializer.Serialize(documentindexRead);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Make async call.
            HttpResponseMessage response = await _httpClient.PostAsync("read", stringContent);
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<Result[]>(contentStream);
            }
        }

        public async Task<Result[]> MarkWitdrawn(DocumentindexWithdraw[] documentindexWithdraw)
        {
            await updateAccessToken();

            var json = JsonSerializer.Serialize(documentindexWithdraw);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // Make async call.
            HttpResponseMessage response = await _httpClient.PostAsync("withdraw", stringContent);
            using (var contentStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<Result[]>(contentStream);
            }
        }

        private async Task updateAccessToken()
        {
            // Get Accecc token
            var tokenResponse = await _httpTokenClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _config.Authority + "/connect/token",

                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret,
                Scope = _config.Scope
            });


            if (tokenResponse.IsError)
            {
                _logger.LogError("Error getting new access token.", tokenResponse.Error);
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            // Set authentication bearer
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResponse.AccessToken);
        }
    }
}
