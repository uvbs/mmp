using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Article
{
    /// <summary> 
    /// Delete 的摘要说明   删除文章接口
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string articleIds = context.Request["article_ids"];
            if (string.IsNullOrEmpty(articleIds))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                resp.errmsg = "article_ids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllJuActivity.Update(new BLLJIMP.Model.JuActivityInfo(), " IsDelete=1 ", string.Format("  WebsiteOwner='{0}' AND JuActivityID in ({1}) ", bllJuActivity.WebsiteOwner, articleIds)) == articleIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "删除文章出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


    }
}