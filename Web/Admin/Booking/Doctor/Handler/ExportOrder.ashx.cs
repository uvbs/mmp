using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.Handler
{
    /// <summary>
    /// 导出订单
    /// </summary>
    public class ExportOrder : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string keyWord = context.Request["KeyWord"];
            string fromDate = context.Request["FromDate"];
            string toDate = context.Request["ToDate"];
            string type = context.Request["type"];
            string status = context.Request["status"];
            string orderType = context.Request["orderType"];
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("Select (Case When OrderType =5 Then '预约'  When OrderType=6 Then '推荐' When OrderType=7 Then '预约' When OrderType=8 Then '推荐' Else '' End) as 类型,");
            sbSql.AppendFormat(" Status as 确认状态,");
            sbSql.AppendFormat(" Consignee as 姓名,");
            sbSql.AppendFormat(" Phone as 手机号,");
            sbSql.AppendFormat(" Ex1 as 年龄,");
            sbSql.AppendFormat(" Ex2 as 性别,");
            sbSql.AppendFormat(" Ex3 as 症状描述,");
            sbSql.AppendFormat(" Ex5 as 专家,");
            sbSql.AppendFormat(" '' as 身份,");

            sbSql.AppendFormat(" Ex6 as 科室,");
            sbSql.AppendFormat(" InsertDate as 提交时间 ");


            var fieldList = bllMall.GetList<TableFieldMapping>(string.Format("WebsiteOwner='{0}' And TableName ='ZCJ_WXMallOrderInfo' Order by Sort DESC", bllMall.WebsiteOwner));
            if (fieldList.Count > 0)
            {
                for (int i = 0; i < fieldList.Count; i++)
                {
                    sbSql.AppendFormat(" ,{0} as {1}", fieldList[i].Field, fieldList[i].MappingName);

                }
            }

            sbSql.AppendFormat(" from ZCJ_WXMallOrderInfo  Where WebsiteOwner='{0}' ", bllMall.WebsiteOwner);
            if (string.IsNullOrEmpty(orderType))
            {
                sbSql.AppendFormat(" And OrderType in(5,6)", type);
            }
            else
            {
                sbSql.AppendFormat(" And OrderType in(7,8)", type);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbSql.AppendFormat("And OrderType={0}", type);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbSql.AppendFormat("And Status='{0}'", status);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbSql.AppendFormat("And ( Consignee like'%{0}%' Or Ex5 like'%{0}%' Or Ex6  like'%{0}%')", keyWord);
            }
            if ((!string.IsNullOrEmpty(fromDate)))//大于开始时间
            {
                sbSql.AppendFormat("And InsertDate>='{0}'", Convert.ToDateTime(fromDate));
            }
            if ((!string.IsNullOrEmpty(toDate)))//小于结束时间
            {
                sbSql.AppendFormat("And InsertDate<'{0}'", Convert.ToDateTime(toDate).AddDays(1));
            }
            sbSql.AppendFormat(" Order By InsertDate DESC");
            var dataTable = ZentCloud.ZCBLLEngine.BLLBase.Query(sbSql.ToString()).Tables[0];
            foreach (System.Data.DataRow item in dataTable.Rows)
            {
                try
                {

                    if (!string.IsNullOrEmpty(item["专家"].ToString()))
                    {
                        string result = "";
                        var doctorNameList = item["专家"].ToString();
                        foreach (var doctorName in doctorNameList.Split(','))
                        {
                            WXMallProductInfo doctorInfo = bllMall.Get<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' And PName='{1}'", bllMall.WebsiteOwner, doctorName));
                            if (doctorInfo != null)
                            {
                                result += doctorInfo.ExArticleTitle_2 + ",";
                            }

                        }
                        result = result.TrimEnd(',');
                        item["身份"] = result;

                    }
                }
                catch (Exception)
                {

                    continue;
                }


            }
            DataLoadTool.ExportDataTable(dataTable, string.Format("{0}_{1}_data.xls", "预约", DateTime.Now.ToString()));


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}