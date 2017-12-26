using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string jid=context.Request["jid"];
            string acticleName = context.Request["title"];
            string content=context.Request["content"], 
                summary = context.Request["summary"];
            string cateId=context.Request["cateId"];
            string k3=context.Request["k3"];
            string thumbnails = context.Request["thumbnails"];

            JuActivityInfo model = bllJuActivity.GetJuActivity(int.Parse(jid),true,bllJuActivity.WebsiteOwner);
            if (model == null)
            {
                apiResp.msg = "不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllJuActivity.ContextResponse(context,apiResp);
                return;
            }
            if (model.UserID != bllJuActivity.GetCurrUserID())
            {
                apiResp.msg = "没有权限";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.NoPms;
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }

            if (string.IsNullOrWhiteSpace(summary))
            {
                summary = MySpider.MyRegex.RemoveHTMLTags(content);
                if (summary.Length > 200) summary = summary.Substring(0, 200) + "...";
            }
            model.ActivityName = acticleName;
            model.Summary = summary;
            model.ActivityDescription = content;
            model.CategoryId = cateId;
            model.K3 = k3;
            model.ThumbnailsPath = thumbnails;

            if (bllJuActivity.Update(model))
            {
                apiResp.status = true;
                apiResp.msg = "操作完成";
            }
            else
            {
                apiResp.msg = "操作出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllJuActivity.ContextResponse(context,apiResp);
        }
    }
}