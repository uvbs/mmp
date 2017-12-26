using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Category
{
    /// <summary>
    /// 删除分类接口
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string categoryIds = context.Request["category_ids"];
            if (string.IsNullOrEmpty(categoryIds))
            {
                resp.errmsg = "category_ids 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllMall.GetCount<WXMallCategory>(string.Format("  WebsiteOwner='{0}' AND AutoID in ({1}) And IsSys=1",bllMall.WebsiteOwner,categoryIds))>0)
            {
                resp.errmsg = "无法删除系统分类";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllMall.Delete(new WXMallCategory(), string.Format(" WebsiteOwner='{0}' AND AutoID in ({1})", bllMall.WebsiteOwner, categoryIds)) == categoryIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除分类出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

    }
}