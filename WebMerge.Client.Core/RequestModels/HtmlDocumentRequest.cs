﻿using WebMerge.Client.Core.Enums;

namespace WebMerge.Client.Core.RequestModels
{
    public class HtmlDocumentRequest : DocumentRequest
    {
        public HtmlDocumentRequest(string name, string html)
            : this(name, html, DocumentOutputType.Pdf)
        {
        }

        public HtmlDocumentRequest(string name, string html, DocumentOutputType output)
            : base(name)
        {
            DocumentType = Enums.DocumentType.Html;
            OutputType = output;
            Html = html;
            FileContents = null;
        }
    }
}