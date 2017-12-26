using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Admin.Meifan.Activity.SignUp
{
    public partial class ListAll : System.Web.UI.Page
    {
        public List<JuActivityInfo> ActivityList = new List<JuActivityInfo>();
        BLLJIMP.BLLMeifan bllMeifan = new BLLJIMP.BLLMeifan();
        BLLJIMP.BLLTag bllTag = new BLLJIMP.BLLTag();
        /// <summary>
        /// 组别
        /// </summary>
        public List<BLLJIMP.Model.MemberTag> GroupList = new List<BLLJIMP.Model.MemberTag>();
        /// <summary>
        /// 组别
        /// </summary>
        public string GroupListJson = "[]";
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 添加订单权限
        /// </summary>
        public bool PmsAdd = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int totalCount = 0;
            ActivityList = bllMeifan.ActivityList(1, int.MaxValue, "activity", "", "", out totalCount);

            GroupList = bllTag.GetTags(bllMeifan.WebsiteOwner, "", 1, int.MaxValue, out totalCount, "activity");

            if (GroupList.Count > 0)
            {
                GroupListJson = ZentCloud.Common.JSONHelper.ObjectToJson(GroupList.Select(p => p.TagName));

            }

            PmsAdd = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.PMS_MFACTIVITY_ADDSIGNUP);



        }
    }
}