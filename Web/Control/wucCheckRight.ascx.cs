using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucCheckRight : System.Web.UI.UserControl
    {
        private string pms = "";

        public string Pms
        {
            get { return pms; }
            set { pms = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            //检查当前用户是否有权限，没有则跳转至错误页
            if (pms != "" && this.Session[Comm.SessionKey.UserID] != null)
            {
                BLLUser bllUser = new BLLUser(this.Session[Comm.SessionKey.UserID].ToString());
                bool tmp = false;

                foreach (string item in pms.Split(' '))
                {
                    if (bllUser.Get<UserInfo>(string.Format("UserID = '{0}'", bllUser.UserID)).UserType.ToString() == item)
                    {
                        tmp = true;
                        break;
                    }
                }

                if (!tmp)
                {
                    this.Response.Redirect(Common.ConfigHelper.GetConfigString("noPmsUrl"));
                }
            }


        }
    }
}