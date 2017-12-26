using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDimension.Json;

namespace ZentCloud.BLLJIMP.Model.Weibo.Place
{
    public class NearbyUsers
    {
        private List<Dictionary<string, int>> _states = new List<Dictionary<string, int>>();

        [JsonProperty("users")]
        public List<Users.PlaceShow> Users { get; set; }
        [JsonProperty("total_number")]
        public int TotalNumber { get; set; }
        [JsonProperty("states")]
        public List<Dictionary<string, int>> States
        {
            get
            {
                return _states;
            }
            set
            {
                _states = value;
            }
        }

    }
}
