using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    
    public partial class NewGreetingCard : System.Web.UI.Page
    {
        public string WxNickName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
          UserInfo userModel= DataLoadTool.GetCurrUserModel();
          if (userModel!=null)
          {
             if (!string.IsNullOrEmpty(userModel.TrueName))
              {
                   WxNickName = userModel.TrueName;
              }
             else if (!string.IsNullOrEmpty(userModel.WXNickname))
             {
                 WxNickName = userModel.WXNickname;
             }

          }
        }
    }
}