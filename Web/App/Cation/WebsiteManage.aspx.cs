using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class WebsiteManage : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        public System.Text.StringBuilder sbTemplateList = new System.Text.StringBuilder();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected UserInfo CurrentUserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUserInfo = bll.GetCurrentUserInfo();
            sbTemplateList.Append("<option value=\"0\">无</option>");
            foreach (var item in bll.GetList<ZentCloud.BLLJIMP.Model.IndustryTemplate>(""))
            {
                sbTemplateList.AppendFormat("<option value=\"{1}\">{0}</option>", item.IndustryTemplateName, item.AutoID);
            }

        }
    }
}