using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Score
{
    /// <summary>
    /// Export 的摘要说明  积分导出
    /// </summary>
    public class Export : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 积分详情 BLL
        /// </summary>
        BLLUserScoreDetailsInfo bllUserScoreDetail = new BLLUserScoreDetailsInfo();
        public void ProcessRequest(HttpContext context)
        {
            string startTime = context.Request["start_time"];
            string stopTime = context.Request["stop_time"];
            string userId = context.Request["user_id"];
            string type = context.Request["type"];

            StringBuilder sbSQL = new StringBuilder();

            sbSQL.AppendFormat(" WebSiteOwner='{0}' ", bllUserScoreDetail.WebsiteOwner);

            if (!string.IsNullOrWhiteSpace(userId)) sbSQL.AppendFormat(" And UserID='{0}' ", userId);

            if (!string.IsNullOrWhiteSpace(type)) { sbSQL.AppendFormat(" And ScoreType='{0}' ", type); }
            else { sbSQL.AppendFormat(" And ScoreType!='AccountAmount' And ScoreType!='TotalAmount' "); }

            if (!string.IsNullOrEmpty(startTime)) sbSQL.AppendFormat(" AND AddTime>='{0}' ", DateTime.Parse(startTime));

            if (!string.IsNullOrEmpty(stopTime)) sbSQL.AppendFormat(" AND AddTime<='{0}' ", DateTime.Parse(stopTime));

            List<UserScoreDetailsInfo> userScores = bllUserScoreDetail.GetList<UserScoreDetailsInfo>(sbSQL.ToString());

            DataTable dt = new DataTable();

            dt.Columns.Add("日期");
            dt.Columns.Add("用户");
            dt.Columns.Add("手机");
            dt.Columns.Add("积分");
            dt.Columns.Add("说明");

            for (int i = 0; i < userScores.Count; i++)
            {
                UserInfo user=bllUser.GetUserInfo(userScores[i].UserID);
                if(user==null) continue;
                DataRow newRow = dt.NewRow();
                newRow["日期"] = userScores[i].AddTime;
                newRow["用户"] = user.TrueName;
                newRow["手机"] = user.Phone;
                newRow["积分"] = userScores[i].Score;
                newRow["说明"] = userScores[i].AddNote;
                dt.Rows.Add(newRow);
            }
            dt.TableName = "积分详情";
            DataTable[] dt2 = { dt };
            DataLoadTool.ExportDataTable(dt2, string.Format("{0}_data.xls", DateTime.Now.ToString()));
        }
    }
}