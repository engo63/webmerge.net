﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebMerge.Client.Core.Enums;
using WebMerge.Client.Core.RequestModels;
using WebMerge.Client.Core.ResponseModels;

namespace WebMerge.Client.Core
{
    public class WebMergeClient : IWebMergeClient
    {
        private readonly IApiConfigurator _configurator;
        private readonly HttpClient _httpClient;

        public WebMergeClient(HttpClient httpClient, IApiConfigurator configurator)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            AddAuthentication();
            _httpClient.BaseAddress = configurator.BaseUri;
        }

        public async Task<Stream> MergeDocumentAndDownloadAsync(int documentId, string documentKey, object mergeObject, bool testMode = false)
        {
            var endpoint = $"merge/{documentId}/{documentKey}?download=1";

            if (testMode)
            {
                endpoint += "&test=1";
            }

            var response = await _httpClient.PostAsJsonAsync(endpoint, mergeObject);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<ActionResponse> MergeDocumentAsync(int documentId, string documentKey, object mergeObject, bool testMode = false)
        {
            var endpoint = $"merge/{documentId}/{documentKey}";

            if (testMode)
            {
                endpoint += "?test=1";
            }

            var response = await _httpClient.PostAsJsonAsync(endpoint, mergeObject);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ActionResponse>();
        }

        public async Task<Document> CreateDocumentAsync(DocumentRequest request)
        {
            CheckRequest(request);

            var response = await _httpClient.PostAsJsonAsync("api/documents", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Document>();
        }

        public async Task<Document> UpdateDocumentAsync(int documentId, DocumentUpdateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.DocumentType.HasValue)
            {
                throw new WebMergeException("You cannot change the type of the document via the API");
            }

            var response = await _httpClient.PutAsJsonAsync($"api/documents/{documentId}", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Document>();
        }

        public async Task<List<Document>> GetDocumentListAsync(string search = null, string folder = null)
        {
            var endpoint = "api/documents";
            var args = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                args.Add($"search={search.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(folder))
            {
                args.Add($"folder={folder.Trim()}");
            }

            if (args.Any())
            {
                endpoint += $"?{string.Join("&", args)}";
            }


            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var reasonPhrase = string.Empty;

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    reasonPhrase = !string.IsNullOrWhiteSpace(folder)
                        ? "The folder does not exist"
                        : "Not Found";
                }

                throw new WebMergeException($"{(int)response.StatusCode} - {response.StatusCode} : {reasonPhrase}", new Exception(response.ReasonPhrase));
            }

            return await response.Content.ReadAsAsync<List<Document>>();

        }

        public async Task<Document> GetDocumentAsync(int documentId)
        {
            var response = await _httpClient.GetAsync($"api/documents/{documentId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Document>();
        }

        public async Task<List<Field>> GetDocumentFieldsAsync(int documentId)
        {
            var response = await _httpClient.GetAsync($"api/documents/{documentId}/fields");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Field>>();
        }

        public async Task<DocumentFile> GetFileForDocumentAsync(int documentId)
        {
            var response = await _httpClient.GetAsync($"api/documents/{documentId}/file");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<DocumentFile>();
        }

        public async Task<Document> CopyDocumentAsync(int documentId, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var content = new StringContent(JsonConvert.SerializeObject(new {name}), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/documents/{documentId}/copy", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Document>();
        }

        public async Task<ActionResponse> DeleteDocumentAsync(int documentId)
        {
            var response = await _httpClient.DeleteAsync($"api/documents/{documentId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ActionResponse>();
        }

        public async Task<Stream> MergeDataRouteWithSingleDownloadAsync(int dataRouteId, string dataRouteKey, object mergeObject, bool testMode = false)
        {
            var endpoint = $"route/{dataRouteId}/{dataRouteKey}?download=1";

            if (testMode)
            {
                endpoint += "&test=1";
            }

            var response = await _httpClient.PostAsJsonAsync(endpoint, mergeObject);
            response.EnsureSuccessStatusCode();

            try
            {
                await response.Content.ReadAsAsync<MultipleFileRouteRequestState>();
            }
            catch (UnsupportedMediaTypeException)
            {
                // download of single file was successful. It's a bit hacky - but only way to ensure the correct response
                return await response.Content.ReadAsStreamAsync();
            }

            throw new WebMergeException($"Response indicated multiple files available for download. Try using {nameof(MergeDataRouteWithMultipleDownloadAsync)} instead");
        }

        public async Task<ActionResponse> MergeDataRouteAsync(int dataRouteId, string dataRouteKey, object mergeObject, bool testMode = false)
        {
            var endpoint = $"route/{dataRouteId}/{dataRouteKey}";

            if (testMode)
            {
                endpoint += "?test=1";
            }

            var response = await _httpClient.PostAsJsonAsync(endpoint, mergeObject);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ActionResponse>();
        }

        public async Task<MultipleFileRouteRequestState> MergeDataRouteWithMultipleDownloadAsync(int dataRouteId, string dataRouteKey, object mergeObject, bool testMode = false)
        {
            var endpoint = $"route/{dataRouteId}/{dataRouteKey}?download=1";

            if (testMode)
            {
                endpoint += "&test=1";
            }

            var response = await _httpClient.PostAsJsonAsync(endpoint, mergeObject);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<MultipleFileRouteRequestState>();
        }

        public async Task<List<DataRoute>> GetDataRouteListAsync()
        {
            var response = await _httpClient.GetAsync("api/routes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<DataRoute>>();
        }

        public async Task<DataRoute> GetDataRouteAsync(int dataRouteId)
        {
            var response = await _httpClient.GetAsync($"api/routes/{dataRouteId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<DataRoute>();
        }

        public async Task<List<Field>> GetDataRouteFieldsAsync(int dataRouteId)
        {
            var response = await _httpClient.GetAsync($"api/routes/{dataRouteId}/fields");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Field>>();
        }

        public async Task<ActionResponse> DeleteDataRouteAsync(int dataRouteId)
        {
            var response = await _httpClient.DeleteAsync($"api/routes/{dataRouteId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ActionResponse>();
        }

        public void Dispose() => _httpClient?.Dispose();

        private void AddAuthentication()
        {
            if (string.IsNullOrWhiteSpace(_configurator.ApiKey))
            {
                throw new ArgumentException("Missing Api Key value. Make sure the 'WebMerge.ApiKey' app setting contains your WebMerge API Key");
            }

            if (string.IsNullOrWhiteSpace(_configurator.ApiSecret))
            {
                throw new ArgumentException("Missing Api Secret value. Make sure there is an environment variable with the key 'WebMerge.ApiSecret' and your WebMerge API Secret value");
            }

            var authBytes = Encoding.UTF8.GetBytes($"{_configurator.ApiKey}:{_configurator.ApiSecret}");
            var authToken = Convert.ToBase64String(authBytes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        private void CheckRequest(DocumentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!string.IsNullOrWhiteSpace(request.Html) && request.DocumentType != DocumentType.Html)
            {
                throw new WebMergeException("Html content can only be used for document type of HTML");
            }

            if (request.DocumentType != DocumentType.Html && string.IsNullOrWhiteSpace(request.FileContents))
            {
                throw new WebMergeException($"Could not create a '{request.DocumentType?.ToString("G")}' because there were no file contents.");
            }
        }
    }
}