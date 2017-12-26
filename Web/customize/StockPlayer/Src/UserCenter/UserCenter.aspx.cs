using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter
{
    public partial class UserCenter : System.Web.UI.Page
    {
        protected BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLCommRelation bLLCommRelation = new BLLJIMP.BLLCommRelation();
        protected BLLJIMP.Model.UserInfo userInfo = new BLLJIMP.Model.UserInfo();
        protected BLLJIMP.Model.UserInfo curUser = new BLLJIMP.Model.UserInfo();
        protected bool isMe = false;
        protected bool isFriend = false;
        protected string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                curUser = bllUser.GetCurrentUserInfo();
                userInfo = curUser;
                isMe = true;
            }
            else
            {
                int auId = Convert.ToInt32(id);
                userInfo = bllUser.GetUserInfoByAutoID(auId);
                curUser = bllUser.GetCurrentUserInfo();

                if (userInfo == null)
                {
                    userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                    userInfo.TrueName = "已禁用";
                    return;
                }
                if (curUser != null && userInfo.AutoID == curUser.AutoID) isMe = true;
                if (curUser != null) { 
                    isFriend = bLLCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.Friend, userInfo.AutoID.ToString(), curUser.AutoID.ToString());
                }
            }
        }
    }
}