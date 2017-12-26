using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Template
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        BLLArticleCategory bllCate = new BLLArticleCategory();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            ComponentTemplate componentTemplate = bll.GetByKey<ComponentTemplate>("AutoId", id);
            if (componentTemplate == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "模板未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }

            apiResp.status = true;
            string cate_name = "";
            ArticleCategory cate = bllCate.GetArticleCategory(componentTemplate.CateId);
            if (cate != null) cate_name = cate.CategoryName;
            apiResp.result = new
            {
                id = componentTemplate.AutoId,
                name = componentTemplate.Name,
                img = componentTemplate.ThumbnailsPath,
                config = componentTemplate.Config,
                data = componentTemplate.Data,
                cate = componentTemplate.CateId,
                cate_name = cate_name,
            };
            apiResp.msg = "获取模板设置成功";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}