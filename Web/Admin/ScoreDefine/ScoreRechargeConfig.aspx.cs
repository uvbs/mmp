using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.ScoreDefine
{
    public partial class ScoreRechargeConfig : System.Web.UI.Page
    {
        protected string UserId;
        protected string Recharge;
        protected string SendNoticePrice;
        protected string MinScore;
        protected string MinWithdrawCashScore;
        protected string VIPPrice;
        protected string VIPPrice0;
        protected string VIPDatelong;
        protected string VIPInterestID;
        protected string VIPInterestDescription;
        protected string moduleName = "积分";
        protected int isHideVIPPrice = 0;
        protected int isHideVIPPrice0 = 0;
        protected int isHideVIPDatelong = 0;
        protected int isHideVIPInterestID = 0;
        protected int isShowSendNotice = 0;
        protected int isShowMinScore = 0;
        protected int isShowMinWithdrawCashScore = 0;
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        BLLJuActivity bllJuAct = new BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
            var moduleNameReq = Request["moduleName"];
            if (!string.IsNullOrWhiteSpace(moduleNameReq)) moduleName = moduleNameReq;
            isHideVIPPrice = Convert.ToInt32(Request["isHideVIPPrice"]);
            isHideVIPPrice0 = Convert.ToInt32(Request["isHideVIPPrice0"]);
            isHideVIPDatelong = Convert.ToInt32(Request["isHideVIPDatelong"]);
            isHideVIPInterestID = Convert.ToInt32(Request["isHideVIPInterestID"]);
            isShowSendNotice = Convert.ToInt32(Request["isShowSendNotice"]);
            isShowMinScore = Convert.ToInt32(Request["isShowMinScore"]);
            isShowMinWithdrawCashScore = Convert.ToInt32(Request["isShowMinWithdrawCashScore"]);

            UserInfo currentUserInfo = bllKeyValueData.GetCurrentUserInfo();
            if (currentUserInfo != null)
            {
                UserId = currentUserInfo.UserID;
                Recharge = bllKeyValueData.GetDataVaule("Recharge", "100", bllKeyValueData.WebsiteOwner);
                SendNoticePrice = bllKeyValueData.GetDataVaule("SendNoticePrice", "1", bllKeyValueData.WebsiteOwner);
                VIPPrice = bllKeyValueData.GetDataVaule("VIPPrice", "1", bllKeyValueData.WebsiteOwner);
                VIPPrice0 = bllKeyValueData.GetDataVaule("VIPPrice", "0", bllKeyValueData.WebsiteOwner);
                VIPDatelong = bllKeyValueData.GetDataVaule("VIPDatelong", "1", bllKeyValueData.WebsiteOwner);
                VIPInterestID = bllKeyValueData.GetDataVaule("VIPInterestID", "1", bllKeyValueData.WebsiteOwner);
                MinScore = bllKeyValueData.GetDataVaule("MinScore", "1", bllKeyValueData.WebsiteOwner);
                MinWithdrawCashScore = bllKeyValueData.GetDataVaule("MinWithdrawCashScore", "1", bllKeyValueData.WebsiteOwner);
                if (!string.IsNullOrWhiteSpace(VIPInterestID))
                {
                    JuActivityInfo juAct = bllJuAct.GetJuActivity(Convert.ToInt32(VIPInterestID));
                    if (juAct != null) VIPInterestDescription = juAct.ActivityDescription;
                }
                else
                {
                    VIPInterestID = "0";
                }
            }

        }
    }
}