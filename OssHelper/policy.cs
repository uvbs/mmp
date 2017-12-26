using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliOss
{
    public class policy
    {
        public string expiration { get; set; }
        public List<object> conditions { get; set; }
    }
}
