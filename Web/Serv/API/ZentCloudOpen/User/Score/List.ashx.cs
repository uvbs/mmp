using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.User.Score
{
    /// <summary>
    /// 积分记录
    /// </summary>
    public class List : BaseHanderOpen
    {

        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            if (pageIndex==0)
            {
                pageIndex = 1;
            }
            string openId = context.Request["openid"];
            string userId=context.Request["userid"];
            string scoreType = context.Request["score_type"];
            string serialNumber = context.Request["serial_number"];//流水号
            int totalCount=0; 
            var sourceData = bllScore.GetScoreRecord(out totalCount,bllScore.WebsiteOwner, userId,openId, scoreType,serialNumber, pageIndex, pageSize);
            var list = from p in sourceData
                       select new
                       {
                           title = p.AddNote,
                           score = p.Score,
                           time =p.AddTime.ToString(),
                           openid=p.OpenId,
                           serial_number=p.SerialNumber
                       };
            resp.msg = "ok";
            resp.status = true;
            resp.result = new
            {
                totalcount = totalCount,
                list = list

            };
           bllScore.ContextResponse(context,resp);


        }


    }
}