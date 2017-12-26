using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class WXMemberMgr : System.Web.UI.Page
    {
        /// <summary>
        /// 查看所有微信注册会员权限
        /// </summary>
        public bool Pms_WX_ViewAllMember=true;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Pms_WX_ViewAllMember = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_WX_ViewAllMember);
        }
    }
}