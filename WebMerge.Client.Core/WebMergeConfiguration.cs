using System;
using System.Configuration;

namespace WebMerge.Client.Core
{
    public class WebMergeConfiguration : IApiConfigurator
    {
        public WebMergeConfiguration(string apiKey, string apiSecret, Uri baseUri)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("Must supply an API key", nameof(apiKey));
            }

            if (string.IsNullOrWhiteSpace(apiSecret))
            {
                throw new ArgumentException("Must supply an API secret", nameof(apiSecret));
            }

            if (baseUri == null)
            {
                throw new ArgumentNullException("Must supply a Base URI",nameof(baseUri));
            }

            ApiKey = apiKey;
            ApiSecret = apiSecret;
            BaseUri = baseUri;
        }

        public string ApiKey { get; }
        public string ApiSecret { get; }
        public Uri BaseUri { get; }
    }
}