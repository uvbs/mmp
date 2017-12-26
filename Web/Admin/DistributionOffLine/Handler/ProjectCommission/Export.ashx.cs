using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.ProjectCommission
{
    /// <summary>
    /// 导出分佣记录
    /// </summary>
    public class Export : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
          string fromDate=context.Request["from_date"];
          string toDate = context.Request["to_date"];
          string keyWord=context.Request["keyword"];
          string type=context.Request["type"];
            string userId=context.Request["user_id"];
          System.Text.StringBuilder sbSql = new System.Text.StringBuilder();

          sbSql.AppendFormat("Select");

          sbSql.AppendFormat(" ProjectId as[订单号],");
          sbSql.AppendFormat(" ProjectAmount as[订单金额],");
          sbSql.AppendFormat(" Rate as[佣金比例],");
          sbSql.AppendFormat(" Amount as[佣金金额],");
          sbSql.AppendFormat(" Remark as[备注],");
          sbSql.AppendFormat(" InsertDate as[时间]");
          sbSql.AppendFormat(" From ZCJ_ProjectCommission");

          sbSql.AppendFormat(" Where ZCJ_ProjectCommission.WebsiteOwner='{0}'",bllUser.WebsiteOwner);

          if (!string.IsNullOrEmpty(type))
          {
              sbSql.AppendFormat(" And ProjectType='{0}'",type);
          }
          if (!string.IsNullOrEmpty(fromDate))
          {
              sbSql.AppendFormat(" And InsertDate>='{0}'", fromDate);
          }
          if (!string.IsNullOrEmpty(toDate))
          {
              toDate = (DateTime.Parse(toDate).AddHours(23).AddMinutes(59).AddSeconds(59)).ToString();
              sbSql.AppendFormat(" And InsertDate<='{0}'", toDate);
          }
          if (!string.IsNullOrEmpty(keyWord))
          {
              sbSql.AppendFormat(" And ProjectName like '%{0}%'", keyWord);
          }
          if (!string.IsNullOrEmpty(userId))
          {
              sbSql.AppendFormat(" And UserID ='{0}'", userId);
          }
          sbSql.AppendFormat(" Order By ProjectId ASC,InsertDate ASC");
          var dataTable = ZentCloud.ZCBLLEngine.BLLBase.Query(sbSql.ToString()).Tables[0];

          
          DataLoadTool.ExportDataTable(dataTable, string.Format("{0}_{1}_data.xls", "分佣记录", DateTime.Now.ToString()));

        }

        
    }
}