using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Customize.kuanqiao
{
    public partial class SubmitInfoStatus : System.Web.UI.Page
    {
        public string WxOpenId = "";
        BLLJIMP.BLLActivity bll = new BLLJIMP.BLLActivity("");
        public List<ActivityDataInfo> list;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["oid"]))
            {
                WxOpenId = Request["oid"];

            }
            WxOpenId =bll.GetCurrentUserInfo().WXOpenId;
           list= bll.GetList<ActivityDataInfo>(string.Format(" ActivityID='130725' And WeixinOpenID='{0}'",WxOpenId));

        }
    }
}