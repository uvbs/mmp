using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NeteaseIMSDK.Model
{
    [Serializable]
    public class NIMUser
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// 对应用户的autoid
        /// </summary>
        [JsonProperty("accid")]
        public string Accid { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
