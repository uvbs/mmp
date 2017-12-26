using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Member
{
    public partial class Recommend :System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        public BLLJIMP.Model.UserInfo userInfo;
        /// <summary>
        ///推荐好友数
        /// </summary>
        public int RecommCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            userInfo = bll.GetCurrentUserInfo();
            RecommCount = bll.GetCount<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' and TypeName = '分享'", userInfo.UserID));
        }


    }
}