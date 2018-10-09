using System;

namespace WebMerge.Client.Core
{
    public class WebMergeConfiguration : IApiConfigurator
    {
        public string ApiKey { get; }
        public string ApiSecret { get; }
        public Uri BaseUri { get; }
    }
}