using System;
using System.Net;

namespace WebMerge.Client.Core
{
    public class WebMergeException : WebException
    {
        public WebMergeException()
            : base("[WebMerge Error]: Unspecified Error")
        {
        }

        public WebMergeException(string message)
            : base($"[WebMerge Error]: {message}")
        {
        }
    }
}