using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.Project
{
    /// <summary>
    /// 获取项目详细信息
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string projectId = context.Request["project_id"];
            if (string.IsNullOrEmpty(projectId))
            {
                apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "project_id 参数必填";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }

            var projectInfo = bll.GetProject(int.Parse(projectId));
            if (projectInfo == null)
            {
                apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "项目不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (projectInfo.UserId!=CurrentUserInfo.UserID)
            {
                apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "无权查看";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            var data = new
            {
                project_id = projectInfo.ProjectId,//项目ID
                project_name = projectInfo.ProjectName,//项目名称
                project_status = projectInfo.Status,//项目状态
                project_amount = projectInfo.Amount,//项目金额
                project_time = bll.GetTimeStamp(projectInfo.InsertDate),//项目时间
                prop_list = bll.GetProjectPropF(projectInfo),//项目自定义字段列表
                commission_amount =(decimal)(decimal.Parse(bll.GetUserLevel(CurrentUserInfo).DistributionRateLevel0)/100) *projectInfo.Amount//预计佣金
            };
            apiResp.result = data;
            apiResp.status = true;
            apiResp.msg = "ok";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }




    }
}