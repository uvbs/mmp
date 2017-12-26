using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// 积分排行
    /// </summary>
    public class Rank : BaseHandlerNoAction
    {
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        public void ProcessRequest(HttpContext context)
        {

            var data = bllScore.ScoreRank(context.Request["year"],context.Request["month"]);
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = from p in data
                             select new
                             {
                                 show_name=p.ShowName,
                                 head_img_url=p.HeadImg,
                                 score=p.Score
                             };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }


    }
}