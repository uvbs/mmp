using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Admin.Statistic
{
    public partial class ScoreStatistic : System.Web.UI.Page
    {
        protected string moduleName = "积分";
        protected string data = "{total:0,rows:[]}";
        BLLUserScoreDetailsInfo bll = new BLLUserScoreDetailsInfo();
        BLLUser bllUser = new BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request["moduleName"])) moduleName = this.Request["moduleName"];
            double userScoreTotal = bllUser.GetSumScore(bllUser.WebsiteOwner);
            double rechargeScoreTotal = bll.GetSumScore(bllUser.WebsiteOwner, "Recharge");
            List<dynamic> rows = new List<dynamic>();
            rows.Add(new { title = string.Format("平台{0}总额", moduleName), num = userScoreTotal });
            rows.Add(new { title = string.Format("充值{0}总额", moduleName), num = rechargeScoreTotal });

            dynamic result = new
            {
                total = rows.Count,
                rows = rows
            };
            data = JsonConvert.SerializeObject(result);
        }
    }
}