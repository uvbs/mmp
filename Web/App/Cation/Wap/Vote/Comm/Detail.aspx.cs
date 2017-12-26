using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm
{
    public partial class Detail : VoteCommBase
    {
       /// <summary>
       /// 短信数量
       /// </summary>
        public int SMSCount;
        protected void Page_Load(object sender, EventArgs e)
        {

            SMSCount = bllVote.GetCount<SMSDetails>(string.Format("PlanID={0} And Receiver='{1}'", Request["vid"], currentUserInfo.UserID));

        }
    }
}