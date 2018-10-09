using Newtonsoft.Json;
using WebMerge.Client.Core.Converters;

namespace WebMerge.Client.Core.ResponseModels
{
    public class ActionResponse
    {
        [JsonProperty("success")]
        [JsonConverter(typeof (BitBooleanConverter))]
        public bool Success { get; set; }
    }
}