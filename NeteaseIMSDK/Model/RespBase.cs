using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NeteaseIMSDK.Model
{
    [Serializable]
    public class RespBase
    {
        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
