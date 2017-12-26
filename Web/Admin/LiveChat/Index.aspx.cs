using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.LiveChat
{
    public partial class Index : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public UserInfo currentUserInfo = new UserInfo();
        /// <summary>
        /// WebSocket主机
        /// </summary>
        public string WebSocketHost = "";
        protected void Page_Load(object sender, EventArgs e)
        {
              currentUserInfo = bllUser.GetCurrentUserInfo();

              //if (currentUserInfo.UserID=="jubit")
              //{
              //    currentUserInfo.AutoID = 7984;
              //}

              if (string.IsNullOrEmpty(currentUserInfo.Ex15))
              {
                  currentUserInfo.Ex15 = "1";
              }
              else
              {

              }
              WebSocketHost = ZentCloud.Common.ConfigHelper.GetConfigString("WebSocketHost");
                
            
        }
    }
}