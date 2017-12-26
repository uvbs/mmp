using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class WeixinMsgDetailsManage : System.Web.UI.Page
    {
        public bool IsManager = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DataLoadTool.GetCurrUserModel().UserType == 1)
            {
                IsManager = true;
            }



        }
    }
}