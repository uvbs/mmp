using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.ProjectLog
{
    /// <summary>
    /// 项目日志列表接口
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;//页码
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;//页数
            string projectId = context.Request["project_id"];
            if (string.IsNullOrEmpty(projectId))
            {
                apiResp.msg = "project_id为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            int totalCount = 0;
            var logList = bll.GetProJectLogList(pageSize,pageIndex,bll.GetCurrUserID(),projectId,bll.WebsiteOwner,out totalCount);
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in logList)
            {
                returnList.Add(new 
                {
                    status=item.Status,//状态
                    remark=item.Remark,//备注
                    projectlog_time=bll.GetTimeStamp(item.InsertDate),//时间
                    projectlog_time_str = item.InsertDate.ToString("yyyy-MM-dd HH:mm")
                });
            }
            apiResp.result = new 
            {
                totalcount=totalCount,
                list=returnList
            };
            apiResp.status = true;
            apiResp.msg = "ok";
            bll.ContextResponse(context, apiResp);
        }

    }
}