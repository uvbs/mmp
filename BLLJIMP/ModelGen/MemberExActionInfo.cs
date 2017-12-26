using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class MemberExActionInfo:ZCBLLEngine.ModelTable
    {

        public int AutoId { get; set; }
        public string ModelId { get; set; }
        public string MemberId { get; set; }
        public DateTime ActionTime { get; set; }
        public DateTime InsertTime { get; set; }
        public int IsAction { get; set; }
        public DateTime? ProcessTime { get; set; }

    }
}
