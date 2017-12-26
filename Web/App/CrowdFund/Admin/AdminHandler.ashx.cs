using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using ZentCloud.BLLJIMP.Model;
using System.Text;

namespace ZentCloud.JubitIMP.Web.App.CrowdFund.Admin
{
    /// <summary>
    /// 筹资后台管理 处理文件
    /// </summary>
    public class AdminHandler : IHttpHandler, IRequiresSessionState
    {
        BLLJIMP.BLL bllBase = new BLLJIMP.BLL();
        protected AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "Action为空！";
                    result = Common.JSONHelper.ObjectToJson(resp);
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

        #region 众筹项目管理
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCrowdFundInfo(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string title=context.Request["Title"];
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bllBase.WebsiteOwner));
            if (!string.IsNullOrEmpty(title))
            {
                sbWhere.AppendFormat(" And Title like '%{0}%'", title);
            }

            int totalCount = this.bllBase.GetCount<CrowdFundInfo>(sbWhere.ToString());
            List<CrowdFundInfo> dataList = this.bllBase.GetLit<CrowdFundInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = dataList
                });



        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCrowdFundInfo(HttpContext context)
        {
            CrowdFundInfo model = bllBase.ConvertRequestToModel<CrowdFundInfo>(new CrowdFundInfo());
            model.WebSiteOwner = bllBase.WebsiteOwner;
            if (bllBase.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditCrowdFundInfo(HttpContext context)
        {
            CrowdFundInfo requestModel = bllBase.ConvertRequestToModel<CrowdFundInfo>(new CrowdFundInfo());
            if (bllBase.Update(requestModel))
            {
                resp.Status = 1;
                resp.Msg = "修改成功";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteCrowdFundInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            CrowdFundInfo model;
            foreach (var item in ids.Split(','))
            {
                model = bllBase.Get<CrowdFundInfo>(string.Format("AutoId={0}", item));
                if (model == null || (!model.WebSiteOwner.Equals(bllBase.WebsiteOwner)))
                {
                    return "无权删除";

                }

            }
            int count = bllBase.Delete(new CrowdFundInfo(), string.Format("AutoId in ({0})", ids));
            return string.Format("成功删除了 {0} 条数据", count);

        }

        #endregion

        #region 众筹项目付款记录
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCrowdFundRecord(HttpContext context)
        {
            int crowdFundId = int.Parse(context.Request["CrowdFundID"]);
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" CrowdFundID={0}", crowdFundId));
            sbWhere.AppendFormat("And Status=1");
            int totalCount = this.bllBase.GetCount<CrowdFundRecord>(sbWhere.ToString());
            List<CrowdFundRecord> dataList = this.bllBase.GetLit<CrowdFundRecord>(pageSize, pageIndex, sbWhere.ToString(), "RecordID DESC");
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = dataList
                });

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