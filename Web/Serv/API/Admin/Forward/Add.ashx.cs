using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Forward
{
    /// <summary>
    /// Add 的摘要说明 加入微问卷
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLQuestion bllQuestion = new BLLJIMP.BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {  
            string ids=context.Request["ids"];
            string type=context.Request["forward_type"];
            if (string.IsNullOrEmpty(ids))
            {
                apiResp.msg = "请至少选择一条记录进行操作！";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            string[] qIds = ids.Split(',');

            foreach (var item in qIds)
            {
                Questionnaire qModel = bllQuestion.Get<Questionnaire>(string.Format(" WebsiteOwner='{0}' AND QuestionnaireID={1} AND QuestionnaireType=1 ",bllQuestion.WebsiteOwner,item));

                if (qModel == null) continue;

                ActivityForwardInfo forwardModel = bllQuestion.Get<ActivityForwardInfo>(string.Format(" WebsiteOwner='{0}'  AND  ActivityId='{1}'",bllQuestion.WebsiteOwner,item));

                if (forwardModel != null)
                {
                    apiResp.msg = forwardModel.ActivityName + "已经转发，请您重新选择！";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                ActivityForwardInfo model = new ActivityForwardInfo();
                model.ActivityId = item;
                model.ActivityName = qModel.QuestionnaireName;
                model.InsertDate = DateTime.Now;
                model.ReadNum = qModel.PV;
                model.UserId = bllQuestion.GetCurrUserID();
                model.ThumbnailsPath = qModel.QuestionnaireImage;
                model.WebsiteOwner = bllQuestion.WebsiteOwner;
                model.ForwardType = type;
                model.PV = 0;
                bllQuestion.Add(model);
            }
            apiResp.msg = "操作完成";
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }
    }
}