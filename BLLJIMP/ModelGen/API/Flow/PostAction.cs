using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Flow
{
    public class PostAction
    {
        public string flow_key { get; set; }
        public int flow_id { get; set; }
        public int step_id { get; set; }
        public int next_step_id { get; set; }
        public int action_id { get; set; }
        public string member_userid { get; set; }
        public decimal amount { get; set; }
        public string content { get; set; }
        public string ex1 { get; set; }
        public string ex2 { get; set; }
        public string ex3 { get; set; }
        public DateTime? select_date { get; set; }
        public string files { get; set; }
        public int audit { get; set; }
        public string rel_id { get; set; }
    }
}
