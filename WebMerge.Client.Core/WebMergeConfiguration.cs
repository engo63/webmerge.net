using System;

namespace WebMerge.Client.Core
{
    public class WebMergeConfiguration : IApiConfigurator
    {
        public WebMergeConfiguration(string apiKey, string apiSecret, Uri baseUri)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            BaseUri = baseUri;
        }

        public string ApiKey { get; }
        public string ApiSecret { get;}
        public Uri BaseUri { get;}
    }
}