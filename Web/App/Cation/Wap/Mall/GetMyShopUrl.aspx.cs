using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class GetMyShopUrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (BLLStatic.bll.IsLogin)
            {
                //获取我的分店url
                var currUser = BLLStatic.bll.GetCurrentUserInfo();

                string url = "/customize/comeoncloud/Index.aspx?key=MallHome&sale_id=" + currUser.UserID;

                Response.Redirect(url);
            }
            else
            {
                Response.Redirect("/customize/comeoncloud/Index.aspx?key=PersonalCenter");
            }
            
        }
    }
}