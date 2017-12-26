using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.ProjectCommission
{
    /// <summary>
    /// 项目分佣记录接口
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
            int totalCount = 0;
            var sourceList = bll.QueryProjectCommissionList(pageIndex, pageSize, out totalCount, keyWord, CurrentUserInfo.UserID);
            var list = from p in sourceList
                       select new
                       {
                           project_id = p.ProjectId,//项目ID
                           project_name = p.ProjectName,//项目名称
                           amount = p.Amount,//提成金额
                           rate=p.Rate,//提成比例
                           time = bll.GetTimeStamp(p.InsertDate),//时间
                           remark = p.Remark//备注
                       };
            var data = new
            {
                totalcount = totalCount,
                list = list

            };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = data;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));


        }

    }
}