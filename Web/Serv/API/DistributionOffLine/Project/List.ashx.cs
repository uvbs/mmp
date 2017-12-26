using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.Project
{
    /// <summary>
    ///项目列表
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
            string keyWord = context.Request["keyword"];//关键字
            string status = context.Request["status"];
            string from = context.Request["from"];//开始时间
            string to = context.Request["to"];//结束时间
            string projectType = context.Request["projectType"];//项目类型
            int totalCount = 0;
            if (!string.IsNullOrEmpty(from))
            {
                from = bll.GetTime(long.Parse(from)).ToString();
            }
            if (!string.IsNullOrEmpty(to))
            {
                to = bll.GetTime(long.Parse(to)).ToString();
            }
            var sourceList = bll.QueryProjectList(pageIndex, pageSize, out totalCount, keyWord, status, CurrentUserInfo.UserID, from, to, projectType);
            var list = from p in sourceList
                       select new
                       {
                           project_id = p.ProjectId,//项目ID
                           project_name = p.ProjectName,//项目名称
                           project_status = p.Status,//项目状态
                           project_amount = p.Amount,//项目金额
                           project_time = bll.GetTimeStamp(p.InsertDate),//项目时间
                           project_remark=p.Remark//项目备注 审核不通过原因
                       };
            var data = new
            {
                totalcount = totalCount,
                list = list,
                sourceList = sourceList
            };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = data;
            context.Response.Write(JsonConvert.SerializeObject(apiResp));


        }

    }
}