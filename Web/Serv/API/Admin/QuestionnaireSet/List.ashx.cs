using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.QuestionnaireSet
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string title = context.Request["title"];

            List<BLLJIMP.Model.QuestionnaireSet> data = bllQuestion.GetSetList(pageSize, pageIndex, bllQuestion.WebsiteOwner, title, null);
            int totalCount = bllQuestion.GetSetCount(bllQuestion.WebsiteOwner, title, null);
            var result = from p in data
                         select new
                         {
                             id = p.AutoID,
                             title = p.Title,
                             img = p.Img,
                             score = p.Score,
                             score_num = p.ScoreNum,
                             ip=p.IP,
                             pv=p.PV,
                             uv=p.UV,
                             start_date = DateTimeHelper.DateTimeToUnixTimestamp(p.StartDate),
                             end_date = DateTimeHelper.DateTimeToUnixTimestamp(p.EndDate),
                             isrand_question = p.IsOptionRandom.HasValue? p.IsQuestionRandom.Value:0,
                             isrand_option = p.IsOptionRandom.HasValue? p.IsOptionRandom.Value:0,
                             answer_count = bllQuestion.GetRecordCount(null, p.AutoID, null)
                         };
            apiResp.status = true;
            apiResp.result = new
            {
                totalcount = totalCount,
                list = result
            };
            bllQuestion.ContextResponse(context, apiResp);
        }
    }
}