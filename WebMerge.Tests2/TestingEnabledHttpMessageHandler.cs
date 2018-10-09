﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebMerge.Tests
{
    public class TestingEnabledHttpMessageHandler : HttpMessageHandler
    {
        private Dictionary<Uri, HttpResponseMessage> ResponseMap { get; } = new Dictionary<Uri, HttpResponseMessage>();
        public event Action<HttpRequestMessage> RequestSent;

        public void AddResponse(Uri route, HttpResponseMessage response) => ResponseMap.Add(route, response);

        public void AddResponse(Uri route, string rawJsonContent)
        {
            var content = new StringContent(rawJsonContent, Encoding.UTF8, "application/json");
            ResponseMap.Add(route, new HttpResponseMessage(HttpStatusCode.OK) {Content = content});
        }

        public void AddResponse(Uri route, byte[] data)
        {
            var content = new ByteArrayContent(data);
            ResponseMap.Add(route, new HttpResponseMessage(HttpStatusCode.OK) {Content = content});
        }

        public void AddResponse(Uri route, Stream stream)
        {
            var content = new StreamContent(stream);
            ResponseMap.Add(route, new HttpResponseMessage(HttpStatusCode.OK) {Content = content});
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestSent?.Invoke(request);

            if (ResponseMap.ContainsKey(request.RequestUri))
            {
                return Task.FromResult(ResponseMap[request.RequestUri]);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) {RequestMessage = request});
        }
    }
}