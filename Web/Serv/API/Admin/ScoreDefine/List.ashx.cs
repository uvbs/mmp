using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ScoreDefine
{
    /// <summary>
    /// 积分规则列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLScoreDefine bllScoreDefine = new BLLScoreDefine();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);

            int totalCount = 0;
            List<ScoreDefineInfo> data = bllScoreDefine.GetScoreDefineList(pageSize, pageIndex, bllScoreDefine.WebsiteOwner, out totalCount);
            var result = from p in data
                         select new
                         {
                             id = p.ScoreId,
                             score_type = p.ScoreType,
                             name = p.Name,
                             score = p.Score,
                             ishide = p.IsHide,
                             day_limit = p.DayLimit,
                             order_num = p.OrderNum,
                             describe = p.Description
                         };
            resp.isSuccess = true;
            resp.returnObj = new
            {
                totalcount = totalCount,
                list = result
            };
            bllScoreDefine.ContextResponse(context, resp);
        }
    }
}