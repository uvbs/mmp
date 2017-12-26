using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class MsgDetails : System.Web.UI.Page
    {
        UserInfo userInfo;
        BLLWeixin weixinBll;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userInfo = Comm.DataLoadTool.GetCurrUserModel();
            this.weixinBll = new BLLWeixin(this.userInfo.UserID);
            if (!IsPostBack)
            {
                this.LoadData();
            }
        }

        private void LoadData()
        {
            this.grvData.DataSource = this.weixinBll.GetList<WeixinMsgDetails>(string.Format(" UserID = '{0}' ", this.userInfo.UserID));
            this.grvData.DataBind();
        }

    }
}