using Newtonsoft.Json;
using WebMerge.Client.Core.Converters;

namespace WebMerge.Client.Core.ResponseModels
{
    public class DataRouteFile
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("file_contents")]
        [JsonConverter(typeof (Base64ByteConverter))]
        public byte[] FileContents { get; set; }
    }
}