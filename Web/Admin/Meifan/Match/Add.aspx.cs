using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Meifan.Match
{
    public partial class Add : System.Web.UI.Page
    {
        /// <summary>
        /// 组别
        /// </summary>
        public List<BLLJIMP.Model.MemberTag> GroupList = new List<BLLJIMP.Model.MemberTag>();
        /// <summary>
        /// 组别
        /// </summary>
        public string GroupListJson = "[]";
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        BLLJIMP.BLLMeifan bllMeifan = new BLLJIMP.BLLMeifan();
        protected void Page_Load(object sender, EventArgs e)
        {
            int totalCount = 0;
            GroupList = bllTag.GetTags(bllMeifan.WebsiteOwner, "", 1, int.MaxValue, out totalCount,

"match");

            if (GroupList.Count > 0)
            {
                GroupListJson = ZentCloud.Common.JSONHelper.ObjectToJson(GroupList.Select(p =>

p.TagName));

            }
        }
    }
}