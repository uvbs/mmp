using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Question
{
    /// <summary>
    /// 调查问卷列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string questionnaireName = context.Request["questionnaire_name"];
            string type = context.Request["type"];
            int totalCount = 0;
            List<Questionnaire> data = bllQuestion.GetQuestionnaireList(pageSize, pageIndex, type, questionnaireName, bllQuestion.WebsiteOwner, out totalCount);
            var result = from p in data
                         select new ResponseModel
                         {
                             id = p.QuestionnaireID,
                             questionnaire_name = p.QuestionnaireName,
                             questionnaire_image = p.QuestionnaireImage,
                             questionnaire_stopdate = DateTimeHelper.DateTimeToStr(p.QuestionnaireStopDate),
                             questionnaire_visible = p.QuestionnaireVisible,
                             add_score = p.AddScore
                         };
            resp.isSuccess = true;
            resp.returnObj = new
            {
                totalcount = totalCount,
                list = result
            };
            bllQuestion.ContextResponse(context, resp);
        }
        public class ResponseModel
        {
            /// <summary>
            /// 问卷id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 问卷名称
            /// </summary>
            public string questionnaire_name { get; set; }
            /// <summary>
            /// 问卷停止日期
            /// </summary>
            public string questionnaire_stopdate { get; set; }
            /// <summary>
            /// 问卷是否可见 0不可见 1可见
            /// </summary>
            public int questionnaire_visible { get; set; }
            /// <summary>
            /// 问卷图片
            /// </summary>
            public string questionnaire_image { get; set; }
            /// <summary>
            /// 赠送积分
            /// </summary>
            public int add_score { get; set; }
        }

    }
}