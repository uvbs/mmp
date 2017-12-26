using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Open.EfastSDK.Entity
{
    public class RespDataBase
    {
        public int code { get; set; }
        
        public string msg { get; set; }
        
        public int total_results { get; set; }
        
        public int page_no { get; set; }

        public int page_size { get; set; }
        
    }
}
