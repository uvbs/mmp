using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// SlidelList 的摘要说明
    /// </summary>
    public class SlideList : BaseHandlerNoAction
    {
        BLLSlide bll = new BLLSlide();

        public void ProcessRequest(HttpContext context)
        {
            List<Slide> dataList = bll.ListByType(context.Request["type"], bll.WebsiteOwner);
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = from p in dataList
                             select new
                             {
                                 id = p.AutoID,
                                 websiteOwner = p.WebsiteOwner,
                                 title = p.LinkText,
                                 img = p.ImageUrl,
                                 link = p.Link,
                                 type = p.Type,
                                 width = p.Width,
                                 height = p.Height,
                                 s_type = p.Stype,
                                 s_text = p.Stext,
                                 s_value = p.Svalue
                             };
            bll.ContextResponse(context, apiResp);
        }
    }
}