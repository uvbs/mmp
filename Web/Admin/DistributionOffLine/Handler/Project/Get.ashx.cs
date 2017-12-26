using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.JubitIMP.Web.Serv;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.Project
{
    /// <summary>
    /// Get 的摘要说明 详情
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();

        public void ProcessRequest(HttpContext context)
        {
            string projectId=context.Request["project_id"];
            BLLJIMP.Model.Project project = bll.GetProject(int.Parse(projectId));
            dynamic  data = new {
                project_id = project.ProjectId,
                project_name=project.ProjectName,
                project_type=project.ProjectType,
                project_status = project.Status,//项目状态
                project_amount = project.Amount,//项目金额
                project_time = project.InsertDate,
                project_remark = project.Remark,//项目备注 审核不通过原因
                project_introduction=project.Introduction,
                contact=project.Contact,
                phone=project.Phone,
                company=project.Company,
                ex1 = project.Ex1,
                ex2 = project.Ex2,
                ex3 = project.Ex3,
                ex4 = project.Ex4,
                ex5 = project.Ex5,
                ex6 = project.Ex6,
                ex7 = project.Ex7,
                ex8 = project.Ex8,
                ex9 = project.Ex9,
                ex10 = project.Ex10,
                ex11 = project.Ex11,
                ex12 = project.Ex12,
                ex13 = project.Ex13,
                ex14 = project.Ex14
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
        }
    }
}