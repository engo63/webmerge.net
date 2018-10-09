using System;

namespace WebMerge.Client.Core
{
    public class WebMergeConfiguration : IApiConfigurator
    {
        //public WebMergeConfiguration(string apiKey, string apiSecret, Uri baseUri)
        //{
        //    if (string.IsNullOrWhiteSpace(apiKey))
        //    {
        //        throw new ArgumentException("must supply an api key", nameof(apiKey));
        //    }

        //    if (string.IsNullOrWhiteSpace(apiSecret))
        //    {
        //        throw new ArgumentException("must supply an api secret", nameof(apiSecret));
        //    }

        //    if (baseUri == null)
        //    {
        //        throw new ArgumentNullException("must supply a base uri", nameof(baseUri));
        //    }

        //    ApiKey = apiKey;
        //    ApiSecret = apiSecret;
        //    BaseUri = baseUri;
        //}

        public string ApiKey { get; }
        public string ApiSecret { get; }
        public Uri BaseUri { get; }
    }
}