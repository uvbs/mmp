using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Data
{
    public partial class ActivityManage : System.Web.UI.Page
    {
        BLLActivity bll = new BLLActivity("");
        /// <summary>
        /// 客服列表
        /// </summary>
        public System.Text.StringBuilder sbActivityNoticeKeFuList = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (var item in bll.GetList<WXKeFu>(string.Format("WebsiteOwner='{0}'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner)))
            {
                sbActivityNoticeKeFuList.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.TrueName);


            }


        }
    }
}