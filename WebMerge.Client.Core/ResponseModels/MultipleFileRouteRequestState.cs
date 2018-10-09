﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebMerge.Client.Core.ResponseModels
{
    public class MultipleFileRouteRequestState : ActionResponse
    {
        [JsonProperty("files")]
        public List<DataRouteFile> Files { get; set; }
    }
}