using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web
{
    public partial class WXTimingTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //BLLJIMP.BLLJuActivity bllActivity = new BLLJIMP.BLLJuActivity();
            //BLLJIMP.BLLDistributionOffLine bllDis = new BLLJIMP.BLLDistributionOffLine();
            //BLLJIMP.BLLUser bllUser=new BLLJIMP.BLLUser();
            //string trueActivityId = "724636";

            //string applyActivityId = "714274";

            //List<ActivityDataInfo> dataList = bllActivity.GetList<ActivityDataInfo>(string.Format(" ActivityID =724636 and SpreadUserID is not null and SpreadUserID <>''  and weixinopenid <> '' and weixinopenid is not null "));

            //ActivityInfo trueActivityInfo = bllActivity.Get<ActivityInfo>(string.Format("ActivityID={0}", trueActivityId));

            //ActivityInfo applyActivityInfo = bllActivity.Get<ActivityInfo>(string.Format("ActivityID={0}", applyActivityId));
            //int successCount = 0;
            //foreach (var item in dataList)
            //{
            //    UserInfo userInfo=bllUser.GetUserInfoByOpenId(item.WeixinOpenID);
            //    UserInfo recommUserInfo=bllUser.GetUserInfo(item.SpreadUserID);

            //    if (bllDis.GetCount<ActivityDataInfo>(string.Format("Activityid=714274 And UserId='{0}'",userInfo.UserID))==0)
            //    {
            //        if (bllDis.AutoApply(userInfo, trueActivityInfo, applyActivityInfo, recommUserInfo, item))
            //        {
            //            successCount++;
            //        } ;

            //    }



            //}

            //Response.Write(successCount);


        }
    }
    
}