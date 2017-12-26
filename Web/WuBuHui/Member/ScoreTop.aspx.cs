using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Member
{
    public partial class ScoreTop : UserPage
    {
        public List<UserInfo> UserList = new List<UserInfo>();
        BLLJIMP.BLLUser bll = new BLLJIMP.BLLUser("");
        public string IsHaveUnReadMessage = "false";
        BLLJIMP.BLLSystemNotice bllNotice = new BLLJIMP.BLLSystemNotice();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
            //int count= bll.GetCount<TutorInfo>(string.Format("WebSiteOwner='{0}' And UserId !='' And UserId IS NOT NULL", bll.WebsiteOwner));
            //if (count > 0)
            //{
            //}
            //else
            //{
            //    sbWhere.AppendFormat("select top 50 * from ZCJ_UserInfo where WebSiteOwner='{0}' And TrueName IS NOT NULL And WXHeadimgurl IS NOT NULL Order By TotalScore DESC,TrueName DESC", bll.WebsiteOwner);
            //    UserList = ZentCloud.ZCBLLEngine.BLLBase.Query<UserInfo>(sbWhere.ToString());

            //}
            sbWhere.AppendFormat("select top 21 * from ZCJ_UserInfo where WebSiteOwner='{0}' And UserId not in(Select UserId from ZCJ_TutorInfo where WebSiteOwner='{0}' And UserId!='' And UserId IS NOT NULL ) And TrueName IS NOT NULL And TrueName!='' And WXHeadimgurl IS NOT NULL Order By TotalScore DESC,TrueName DESC", bll.WebsiteOwner);
            UserList = ZentCloud.ZCBLLEngine.BLLBase.Query<UserInfo>(sbWhere.ToString());


            IsHaveUnReadMessage = bllNotice.IsHaveUnReadMessage(bllNotice.GetCurrentUserInfo().UserID).ToString();

        }
    }
}