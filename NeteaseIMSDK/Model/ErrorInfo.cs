using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeteaseIMSDK.Model
{
    [Serializable]
    public class ErrorInfo
    {
        public int code { get; set; }

        public string name { get; set; }

    }
}
