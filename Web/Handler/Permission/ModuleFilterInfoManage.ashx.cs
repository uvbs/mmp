using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler.Permission
{
    /// <summary>
    /// ModuleFilterInfoManage 的摘要说明
    /// </summary>
    public class ModuleFilterInfoManage : IHttpHandler, IRequiresSessionState
    {

        static BLLJIMP.BLL bll;
        public void ProcessRequest(HttpContext context)
        {
            bll = new BLLJIMP.BLL("");
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            string result = "false";
            if (!bll.GetCurrentUserInfo().UserType.Equals(1))
            {
                return;
            }
            switch (action)
            {

                case "Add":
                    result = Add(context);
                    break;
                case "Edit":
                    result = Edit(context);
                    break;
                case "Delete":
                    result = Delete(context);
                    break;
                case "Query":
                    result = GetAllByAny(context);
                    break;
            }
            context.Response.Write(result);


        }
        private static string GetAllByAny(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string searchtitle = context.Request["SearchTitle"];
            string filterType = context.Request["FilterType"];



            var sbWhere = new System.Text.StringBuilder(" 1=1");

            if (!string.IsNullOrEmpty(searchtitle))
            {
                sbWhere.AppendFormat(" And PagePath like '%{0}%'", searchtitle);
            }
            if (!string.IsNullOrEmpty(filterType))
            {
                sbWhere.AppendFormat(" And FilterType ='{0}'", filterType);
            }
            else
            {
                sbWhere.AppendFormat(" And FilterType !='WXOAuth'");
            }

            List<ModuleFilterInfo> list = bll.GetLit<ModuleFilterInfo>(pageSize, pageIndex, sbWhere.ToString(), "PagePath ASC");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.ModuleFilterInfo>(sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = list
     });
        }
        /// <summary>
        /// 添加
        /// </summary>
        private static string Add(HttpContext context)
        {
            try
            {
                var pagePath = context.Request["PagePath"];
                var filterDescription = context.Request["FilterDescription"];
                var filterType = context.Request["FilterType"];
                var matchType = context.Request["MatchType"];
                var ex1 = context.Request["Ex1"];
                var model = new ModuleFilterInfo();
                model.PagePath = pagePath;
                model.FilterDescription = filterDescription;
                model.FilterType = filterType;
                model.MatchType = matchType;
                model.Ex1 = ex1;

                if (bll.Add(model))
                {
                    UpdateRedis();
                    return "true";
                }
                return "false";

                //return bll.Add(model).ToString().ToLower();
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            //return "false";

        }


        /// <summary>
        /// 修改
        /// </summary>
        public static string Edit(HttpContext context)
        {
            try
            {

                var pagePath = context.Request["PagePath"];
                var filterDescription = context.Request["FilterDescription"];
                var filterType = context.Request["FilterType"];
                var oldPagePath = context.Request["OldPagePath"];
                var oldFilterType = context.Request["OldFilterType"];
                var matchType = context.Request["MatchType"];
                var ex1 = context.Request["Ex1"];
                var model = new ModuleFilterInfo();
                model.PagePath = pagePath;
                model.FilterDescription = filterDescription;
                model.FilterType = filterType;
                if (bll.Update(model, string.Format("PagePath='{0}',FilterType='{1}',FilterDescription='{2}',MatchType='{3}',Ex1='{4}'", pagePath, filterType, filterDescription, matchType, ex1), string.Format(" PagePath='{0}' and FilterType='{1}'", oldPagePath, oldFilterType)) > 0)
                {
                    UpdateRedis();
                    return "true";

                }
                else
                {
                    return "false";

                }

            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }


        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {

            try
            {
                var ids = context.Request["id"].Split(',');
                int count = 0;
                foreach (var item in ids)
                {

                    if (bll.Delete(new ModuleFilterInfo(), string.Format("PagePath ='{0}' ", item)) > 0)
                    {
                        count++;
                    }

                }
                UpdateRedis();
                if (count == ids.Length)
                {
                    return "true";
                }
                else
                {
                    return "false";

                }




            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }

        /// <summary>
        /// 更新Redis
        /// </summary>
        private static void UpdateRedis()
        {
            List<ZentCloud.BLLPermission.Model.ModuleFilterInfo> pathList = bll.GetList<ZentCloud.BLLPermission.Model.ModuleFilterInfo>("");
            try
            {
                RedisHelper.RedisHelper.StringSetSerialize(RedisHelper.Enums.RedisKeyEnum.WXModuleFilterInfo, pathList);
            }
            catch (Exception ex)
            {

            }
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