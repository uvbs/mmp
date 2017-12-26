﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Meifan.Train
{
    public partial class Update : System.Web.UI.Page
    {
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public JuActivityInfo activityInfo = new JuActivityInfo();
        List<MeifanActivityItem> activityItems = new List<MeifanActivityItem>();
        /// <summary>
        /// 
        /// </summary>
        public string activityItemsJson = "";
        /// <summary>
        /// 组别
        /// </summary>
        public List<BLLJIMP.Model.MemberTag> GroupList = new List<BLLJIMP.Model.MemberTag>();
        /// <summary>
        /// 组别
        /// </summary>
        public string GroupListJson = "[]";
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        protected void Page_Load(object sender, EventArgs e)
        {
            activityInfo = bll.GetActivity(Request["activity_id"]);
            activityItems = bll.ActivityItemList(activityInfo.JuActivityID.ToString());
            foreach (var item in activityItems)
            {
                if (!string.IsNullOrEmpty(item.FromDate))
                {
                    item.FromDate = Convert.ToDateTime(item.FromDate).ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(item.ToDate))
                {
                    item.ToDate = Convert.ToDateTime(item.ToDate).ToString("yyyy-MM-dd");
                }

            }
            activityItemsJson = ZentCloud.Common.JSONHelper.ObjectToJson(activityItems);

            int totalCount = 0;
            GroupList = bllTag.GetTags(bllTag.WebsiteOwner, "", 1, int.MaxValue, out totalCount,

"train");

            if (GroupList.Count > 0)
            {
                GroupListJson = ZentCloud.Common.JSONHelper.ObjectToJson(GroupList.Select(p =>

p.TagName));

            }

        }
    }
}