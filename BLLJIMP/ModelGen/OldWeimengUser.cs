using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class OldWeimengUser : ZCBLLEngine.ModelTable
    {
        public int AutoId { get; set; }

        public string CarId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Sex { get; set; }

        public string GetCardTime { get; set; }

        public string Amount { get; set; }

        public string Score { get; set; }

        public string UserLevel { get; set; }

        public string Bday { get; set; }

        public string Address { get; set; }

        public string ToUserId { get; set; }

        public string ToUserAutoId { get; set; }

        public string ToTime { get; set; }

    }
}
