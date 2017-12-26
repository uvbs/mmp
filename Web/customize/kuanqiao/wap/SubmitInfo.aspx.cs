using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Customize.kuanqiao
{
    public partial class SubmitInfo : System.Web.UI.Page
    {
        BLLJIMP.BLLActivity bll = new BLLJIMP.BLLActivity("");
        /// <summary>
        /// 微信OPenID
        /// </summary>
        public string WxOpenId = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(Request["oid"]))
            {
                //Response.End();
            }
            else
            {
                WxOpenId= Request["oid"];
            }
            WxOpenId = bll.GetCurrentUserInfo().WXOpenId;



           // var data = bll.Get<ActivityDataInfo>(string.Format("WeixinOpenID='{0}'", WxOpenId));
           //if (data==null)
           //{
           //    //Response.End();

           //}
         



        }
    }
}