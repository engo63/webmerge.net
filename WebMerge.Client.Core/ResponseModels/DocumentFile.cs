using System;
using Newtonsoft.Json;
using WebMerge.Client.Core.Converters;
using WebMerge.Client.Core.Enums;

namespace WebMerge.Client.Core.ResponseModels
{
    public class DocumentFile
    {
        [JsonProperty("type")]
        [JsonConverter(typeof (EnumConverter))]
        public DocumentType DocumentType { get; set; }

        [JsonProperty("last_update")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("contents")]
        [JsonConverter(typeof (Base64ByteConverter))]
        public byte[] FileContents { get; set; }
    }
}