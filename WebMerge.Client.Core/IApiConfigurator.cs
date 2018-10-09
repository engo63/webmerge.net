using System;

namespace WebMerge.Client.Core
{
    public interface IApiConfigurator
    {
        string ApiKey { get; }
        string ApiSecret { get; }
        Uri BaseUri { get; }
    }
}