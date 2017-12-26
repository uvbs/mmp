using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.User
{
    [Serializable]
    public class Field
    {
        public bool show { get; set; }
        public bool edit { get; set; }

        public string showname{ get; set; }
    }
}
