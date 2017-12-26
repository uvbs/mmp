using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.Communal
{
    public partial class BindWXOpenId : System.Web.UI.Page
    {
        protected UserInfo curUser = null;
        protected bool status = false;
        protected string msg;
        ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {

            int id = Convert.ToInt32(this.Request["id"]);
            if (id == 0)
            {
                msg = "参数错误";
                return;
            }
            if (this.Session["currWXOpenId"] == null)
            {
                msg = "请用微信打开";
                return;
            }
            string opendId = this.Session["currWXOpenId"].ToString();
            curUser = bllUser.GetUserInfoByOpenId(opendId, bllUser.WebsiteOwner);
            if (curUser != null)
            {
                if (curUser.AutoID != id)
                {
                    msg = "该微信已绑定其他账号";
                }
                else
                {
                    msg = "您已绑定过该微信";
                }
                return;
            }
            curUser = bllUser.GetUserInfoByAutoID(id, bllUser.WebsiteOwner);
            if (curUser == null)
            {
                msg = "用户未找到";
                return;
            }
            if (!string.IsNullOrWhiteSpace(curUser.WXOpenId))
            {
                if (curUser.WXOpenId != opendId)
                {
                    msg = "您已绑定过其他微信";
                }
                else
                {
                    msg = "您已绑定过该微信";
                }
                return;
            }
            if (bllUser.Update(curUser, string.Format("WXOpenId='{0}'", opendId), 
                string.Format("AutoID={0} And WebsiteOwner='{1}'", curUser.AutoID, bllUser.WebsiteOwner)) <= 0)
            {
                msg = "绑定失败";
                return;
            }
            msg = "绑定成功";
            status = true;
        }
    }
}