﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Article.Category
{
    /// <summary>
    /// Delete 的摘要说明   文章分类目录
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string categoryIds = context.Request["category_ids"];
            if (string.IsNullOrEmpty(categoryIds))
            {
                resp.errmsg = "category_ids 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bll.Delete(new ArticleCategory(), string.Format(" WebsiteOwner='{0}' AND CategoryType='article' AND AutoID in ({1})", bll.WebsiteOwner, categoryIds)) == categoryIds.Split(',').Length)
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除文章分类出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}