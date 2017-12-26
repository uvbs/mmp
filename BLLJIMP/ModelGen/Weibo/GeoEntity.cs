using System;
using NetDimension.Json;
using System.Collections.Generic;

namespace ZentCloud.BLLJIMP.Model.Weibo
{
    public class GeoEntity
    {
        [JsonProperty("type")]
        public string Type { get; internal set; }
        [JsonProperty("coordinates")]
        public IEnumerable<float> Coordinates { get; internal set; }
    }
}
