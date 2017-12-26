using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.customize.xinchenglu
{
    /// <summary>
    /// Wap 的摘要说明
    /// </summary>
    public class Wap : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLJIMP.BLL bll;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                bll = new BLLJIMP.BLL();
                string action = context.Request["Action"];
                switch (action)
                {

                    case "GetNeedList"://获取需求列表
                        result = GetNeedList(context);
                        break;

                    case "GetVolunteerList"://获取志愿者列表
                        result = GetVolunteerList(context);
                        break;


                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);

            }

            context.Response.Write(result);
        }


        /// <summary>
        /// 获取需求列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetNeedList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string needName = context.Request["NeedName"];
            string activityId = context.Request["ActivityID"];
            string kName = context.Request["KName"];
            string kStatus=context.Request["KStatus"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" IsDelete=0 And ActivityID='{0}' And  {1} IS NOT NULL And {1} !='' ", activityId, kStatus));
            if (!string.IsNullOrEmpty(needName))
            {
                sbWhere.AppendFormat(" And {0} like '%{1}%'", kName, needName);
            }
            totalCount = bll.GetCount<ActivityDataInfo>(sbWhere.ToString());
            List<ActivityDataInfo> data = bll.GetLit<ActivityDataInfo>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate DESC");
            resp.ExObj = data;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 获取志愿者列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetVolunteerList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string name = context.Request["Name"];
            string rootCategory = context.Request["RootCategory"];
            string secCategory = context.Request["SecCategory"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format(" IsDelete=0 And ActivityID='216214' And  K8 IS NOT NULL And K8 !='' "));
            if (!string.IsNullOrEmpty(name))
            {
                sbWhere.AppendFormat(" And (Name like '%{0}%' Or K4 like '%{0}%') ", name);
            }
            if (!string.IsNullOrEmpty(rootCategory))
            {
                sbWhere.AppendFormat(" And K9 like '{0}%'", rootCategory);
            }
            if (!string.IsNullOrEmpty(secCategory))
            {
                sbWhere.AppendFormat(" And K10 like '{0}%'", secCategory);
            }
            totalCount = bll.GetCount<ActivityDataInfo>(sbWhere.ToString());
            List<ActivityDataInfo> data = bll.GetLit<ActivityDataInfo>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate ASC");
            resp.ExObj = data;
            resp.ExStr = "";
            int totalpage = bll.GetTotalPage(totalCount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);


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