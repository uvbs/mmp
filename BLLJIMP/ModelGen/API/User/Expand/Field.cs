using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.User.Expand
{
    [Serializable]
    public class Field
    {
        public string field { get; set; }
        public string mfield { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int width { get; set; }
    }
}
