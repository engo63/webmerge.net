using System;
using System.Net;
using System.Net.Http;

namespace WebMerge.Client.Core
{
    public class WebMergeException : HttpRequestException
    {
        public WebMergeException()
            : base("[WebMerge Error]: Unspecified Error")
        {
        }

        public WebMergeException(string message)
            : base($"[WebMerge Error]: {message}")
        {
        }

        public WebMergeException(string message, Exception innerException)
            : base($"[WebMerge Error]: {message}", innerException)
        {
        }
    }
}