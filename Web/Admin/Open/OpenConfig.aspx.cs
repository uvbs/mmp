using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Open
{
    public partial class OpenConfig : System.Web.UI.Page
    {
        protected string UserId;
        protected string Creater;
        protected string OpenClassNotice;
        BLLUserExpand bllUserExpand = new BLLUserExpand();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo currentUserInfo = bllUserExpand.GetCurrentUserInfo();
            if (currentUserInfo!=null)
            {
                UserId = currentUserInfo.UserID;
                Creater = bllUserExpand.GetUserExpandValue(BLLJIMP.Enums.UserExpandType.UserOpenCreate, currentUserInfo.UserID);
                OpenClassNotice = bllKeyValueData.GetDataDefVaule("OpenClassNotice", "0");
            }
        }
    }
}