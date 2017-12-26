using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CrowdFund.Category
{
    /// <summary>
    /// 更新众筹分类
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        public void ProcessRequest(HttpContext context)
        { 
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.category_id <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "分类名称 必填,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.category_name))
            {
                resp.errmsg = "分类名称 必填";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            var category = bll.Get<ArticleCategory>(string.Format(" AutoID={0} ",requestModel.category_id));
            if (category == null)
            {
                resp.errcode = 1;
                resp.errmsg = "分类不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            category.CategoryName = requestModel.category_name;
            if (bll.Update(category))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "修改出错";
                resp.errcode = -1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }




        public class RequestModel
        {
            /// <summary>
            /// 分类编号
            /// </summary>
            public int category_id { get; set; }
            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name { get; set; }
        }
    }
}