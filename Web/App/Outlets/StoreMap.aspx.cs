using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Outlets
{
    public partial class StoreMap : System.Web.UI.Page
    {
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public ZentCloud.BLLJIMP.Model.WebsiteInfo currWebSiteInfo;
        /// <summary>
        /// 高德地图key
        /// </summary>
        public string AMapKey;
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            currWebSiteInfo = bll.Get<ZentCloud.BLLJIMP.Model.WebsiteInfo>(string.Format(" WebsiteOwner='{0}'",bll.WebsiteOwner));
            AMapKey = ZentCloud.Common.ConfigHelper.GetConfigString("AMapKey");

            if (bll.IsLogin)
            {
                var currentUserInfo = bllUser.GetUserInfo(bllUser.GetCurrUserID(), bllUser.WebsiteOwner);
                if (!string.IsNullOrEmpty(currentUserInfo.BindId) && (string.IsNullOrEmpty(Request["redict"])))
                {
                    if (bll.WebsiteOwner=="hailandev")
                    {
                        Response.Redirect("/customize/comeoncloud/Index.aspx?cgid=1618");
                    }
                    else
                    {
                        Response.Redirect("/customize/comeoncloud/Index.aspx?cgid=1595");
                    }
                   


                }
            }


        }
    }
}