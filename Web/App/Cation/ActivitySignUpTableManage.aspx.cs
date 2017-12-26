using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class ActivitySignUpTableManage : System.Web.UI.Page
    {
        /// <summary>
        /// 高级管理员查看权限
        /// </summary>
        public bool Pms_JuActivity_Advanced=true;
        public ActivityInfo activityInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            string activityID = Request["ActivityID"];

            this.ViewState["ActivityID"] = activityID;
            activityInfo = new BLLActivity("").Get<ActivityInfo>(string.Format(" ActivityID = '{0}'", activityID));
            if (activityInfo == null)
            {
                Response.Write("<script>alert('活动不存在!')</script>");
                Response.End();
                return;
            }

            var userInfo = DataLoadTool.GetCurrUserModel();
            if (userInfo.UserType != 1)//普通用户
            {
                if (activityInfo.WebsiteOwner != userInfo.WebsiteOwner)
                {
                    //Response.Redirect("/FShare/ActivityManage.aspx");
                    Response.Write("<script>alert('无权访问该数据，请确认当前用户是否拥有该活动权限!')</script>");
                    Response.End();
                    return;
                }
            }
            //this.lbActivityName.Text = activityInfo.ActivityName;
            //Pms_JuActivity_Advanced = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_JuActivity_Advanced);

        }
    }
}
