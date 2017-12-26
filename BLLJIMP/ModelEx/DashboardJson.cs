using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class DashboardJson
    {
        public DashboardJson()
        {
            visit_num_list = new List<int>();
            order_num_list = new List<int>();
            member_num_list = new List<int>();
            uv_num_list = new List<int>();
            fans_num_list = new List<int>();
            day_list = new List<string>();
        }

        public int visit_num_lastday { get; set; }
        public int visit_num_lastweek { get; set; }
        public int visit_num_lastmonth { get; set; }
        public int order_num_lastday { get; set; }
        public int order_num_lastweek { get; set; }
        public int order_num_lastmonth { get; set; }
        public int member_num_lastday { get; set; }
        public int member_num_lastweek { get; set; }
        public int member_num_lastmonth { get; set; }
        public int uv_num_lastday { get; set; }
        public int uv_num_lastweek { get; set; }
        public int uv_num_lastmonth { get; set; }
        public int fans_num_lastday { get; set; }
        public int fans_num_lastweek { get; set; }
        public int fans_num_lastmonth { get; set; }
        public List<int> visit_num_list { get; set; }
        public List<int> order_num_list { get; set; }
        public List<int> member_num_list { get; set; }
        public List<int> uv_num_list { get; set; }
        public List<int> fans_num_list { get; set; }
        public List<string> day_list { get; set; }
        public int uv_total { get; set; }
        public int order_total { get; set; }
        public int member_total { get; set; }
        public int fans_total { get; set; }
        public int visit_total { get; set; }
        public long timestamp { get; set; }
    }
}
