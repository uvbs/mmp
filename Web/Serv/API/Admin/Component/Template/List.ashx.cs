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
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        BLLArticleCategory bllCate = new BLLArticleCategory();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string keyword = context.Request["keyword"];
            string cate = context.Request["cate"];
            int total = 0;
            List<ComponentTemplate> componentTemplate = bll.GetTemplateList(rows, page, keyword, out total, cate);

            int cateTotal = 0;
            List<ArticleCategory> cateList = bllCate.GetCateList(out cateTotal, "CompTempType", 0, "Common", int.MaxValue, 1, null);

            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in componentTemplate)
            {
                ArticleCategory nCate = cateList.FirstOrDefault(p=>p.AutoID == item.CateId);
                resultList.Add(new
                {
                    id = item.AutoId,
                    name = item.Name,
                    img = item.ThumbnailsPath,
                    sort = item.Sort,
                    web = item.FromWebsite,
                    cate = item.CateId,
                    cate_name = nCate == null ? "" : nCate.CategoryName
                });
            }

            apiResp.status = true;
            apiResp.result = new
            {
                totalcount = total,
                list = resultList
            };
            apiResp.msg = "获取模板列表成功";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}