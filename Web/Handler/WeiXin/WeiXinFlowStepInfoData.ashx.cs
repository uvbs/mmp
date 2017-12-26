using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Web.UI.WebControls;
using System.Data;

namespace ZentCloud.JubitIMP.Web.Handler.WeiXin
{
    /// <summary>
    /// WeiXinFlowStepInfoData 的摘要说明
    /// </summary>
    public class WeiXinFlowStepInfoData : IHttpHandler, IRequiresSessionState
    {
        static BLLJIMP.BLLWeixin bll;
        public void ProcessRequest(HttpContext context)
        {

            //  BLLMenuPermission perbll = new BLLMenuPermission("");
            //_isedit = perbll.CheckUserAndPms(Comm.DataLoadTool.GetCurrUserID(), 255);
            //_isview = perbll.CheckUserAndPms(Comm.DataLoadTool.GetCurrUserID(), 250);
            bll = new BLLWeixin("");
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string Action = context.Request["Action"];
            string result = "false";
            switch (Action)
            {
                //case "Add":
                //    result = Add(context);
                //    break;
                //case "Edit":
                //    result = Edit(context);
                //    break;
                //case "Delete":
                //    result = Delete(context);
                //    break;
                case "Query":
                    result = GetAllByAny(context);
                    break;
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private static string GetAllByAny(HttpContext context)
        {
            //if (!_isview)
            //{
            //    return null;
            //}
            var userid = Comm.DataLoadTool.GetCurrUserID();
            if (string.IsNullOrEmpty(userid))
            {
                return "请重新登录";
            }
            string flowid = context.Request["FlowID"];
            //if (bll.Get<WXFlowInfo>(string.Format("FlowID={0} and UserID='{1}'", flowid, userid)) == null)
            //{
            //    return "无权查看";

            //}
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string searchtitle = context.Request["SearchTitle"];

            var searchCondition = string.Format("FlowID={0}", flowid);
            //if (!string.IsNullOrEmpty(searchtitle))
            //{
            //    searchCondition += "And FlowField like '%" + searchtitle + "%'";
            //}

            List<WXFlowDataInfo> list = bll.GetList<WXFlowDataInfo>(searchCondition);
            var datatable = bll.GetMemberFlowDataView(list);
            var newdatatable = GetPagedTable(datatable,page,rows);//分页
            int totalCount = datatable.Rows.Count;
            string jsonResult =ZentCloud.Common.JSONHelper.DataTableToEasyUIJson(totalCount, newdatatable);
            return jsonResult;
        }

        #region GetPagedTable DataTable分页
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns></returns>
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;
            if (rowbegin >= dt.Rows.Count)
                return newdt;
            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
        #endregion


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}