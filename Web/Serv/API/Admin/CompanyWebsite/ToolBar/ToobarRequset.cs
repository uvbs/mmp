using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CompanyWebsite.ToolBar
{
    public class ToobarRequset
    {
        public int id { get; set; }
        public int pre_id { get; set; }
        public string name { get; set; }
        public string describe { get; set; }
        public int sort { get; set; }
        public string ico { get; set; }
        public string is_show { get; set; }
        public string key_type { get; set; }
        public string use_type { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string websiteowner { get; set; }
        public string active_bg_color { get; set; }
        public string bg_color { get; set; }
        public string active_color { get; set; }
        public string color { get; set; }
    }
}