using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class ArticeCategoryTypeResponse
    {
        public int time_set_method { get; set; }
        public int time_set_style { get; set; }
        public int spend_method { get; set; }
        public string title { get; set; }
        public string home_title { get; set; }
        public string order_list_title { get; set; }
        public string order_detail_title { get; set; }
        public string category_type {get;set;}
        public string category_name { get; set; }
        public string stock_name { get; set; }
        public int slide_width {get;set;}
        public int slide_height {get;set;}
        public bool is_login {get;set;}
        public bool is_member {get;set;}
        public int access_level {get;set;}
        public string truename {get;set;}
        public string phone {get;set;}
        public string apply_url {get;set;}
        public string nopms_url {get;set;}
        public string share_title {get;set;}
        public string share_img {get;set;}
        public string share_desc {get;set;}
        public string share_link {get;set;}
    }
}
