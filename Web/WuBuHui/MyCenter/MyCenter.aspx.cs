using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.MyCenter
{
    public partial class MyCenter : UserPage
    {
        public BLLJIMP.Model.UserInfo uinfo;
        public bool IsShowRed = false;
        public int UserLevel =1;
        BLLJIMP.BLLUserScore bllUserScore ;
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FlowerCount = 0;
        /// <summary>
        /// 关注数
        /// </summary>
        public int AttentionCount = 0;
        public string IsHaveUnReadMessage = "false";
        BLLJIMP.BLLSystemNotice bllNotice = new BLLJIMP.BLLSystemNotice();
        public bool IsTutor;
        ZentCloud.BLLJIMP.BLLWeixin bllweixin = new ZentCloud.BLLJIMP.BLLWeixin("");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                uinfo = DataLoadTool.GetCurrUserModel();
                bllUserScore = new BLLJIMP.BLLUserScore(uinfo.UserID);
                BLLJIMP.BLL bll = new BLLJIMP.BLL();
                txtCount.Text = bll.GetCount<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' and TypeName = '分享'", uinfo.UserID)).ToString();
                IsShowRed = bll.GetCount<BLLJIMP.Model.ReviewInfo>(string.Format("ForeignkeyId='{0}' and IsRead <> 1", uinfo.UserID)) > 0 ? true : false;
                UserLevel = bllUserScore.GetUserLevelByTotalScore(uinfo.HistoryTotalScore);
                FlowerCount = bllUser.GetUserFlowerCount(uinfo.UserID);
                AttentionCount = bllUser.GetUserAttentionCount(uinfo.UserID);
                IsHaveUnReadMessage = bllNotice.IsHaveUnReadMessage(bll.GetCurrentUserInfo().UserID).ToString();
                IsTutor = bllUser.IsTutor(uinfo);
            }
        }
    }
}