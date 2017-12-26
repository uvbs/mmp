using System;
using NetDimension.Json;
using System.Collections.Generic;

namespace ZentCloud.BLLJIMP.Model.Weibo.Statuses
{
    public class PublicTimeline
    {
        [JsonProperty(PropertyName = "statuses")]
        public List<Statuses.Show> Statuses { get; set; }
        [JsonProperty(PropertyName = "previous_cursor")]
        public int PreviousCursor { get; set; }
        [JsonProperty(PropertyName = "next_cursor")]
        public int NextCursor { get; set; }
        [JsonProperty(PropertyName = "total_number")]
        public int TotalNumber { get; set; }

    }
}
