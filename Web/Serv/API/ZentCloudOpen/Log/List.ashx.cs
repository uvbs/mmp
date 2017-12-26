using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Log
{
    /// <summary>
    /// Api日志
    /// </summary>
    public class List:BaseHanderOpen
    {
        BLLJIMP.BLLApiLog bllApiLog = new BLLJIMP.BLLApiLog();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string fromTime=context.Request["from_time"];
            string toTime=context.Request["to_time"];
            string openId = context.Request["openid"];
            string type = context.Request["type"];
            string serialNumber = context.Request["serial_number"];//流水号
            int totalCount = 0;
            var sourceData = bllApiLog.List(bllApiLog.WebsiteOwner, pageIndex, pageSize, type, out totalCount, openId, "", fromTime, toTime, serialNumber);
            var list = from p in sourceData
                       select new
                       {
                           time=p.InsertDate.ToString(),
                           ip=p.IP,
                           type = p.Module,
                           user_agent=p.UserAgent,
                           http_method = p.HttpMethod,
                           url = p.Url,
                           parameters=p.Parameters,
                           remark=p.Remark,
                           openid=p.OpenId,
                           serial_number = p.SerialNumber
                       };
            resp.msg = "ok";
            resp.status = true;
            resp.result = new
            {
                totalcount = totalCount,
                list = list

            };
            bllApiLog.ContextResponse(context, resp);


        }


    }
}