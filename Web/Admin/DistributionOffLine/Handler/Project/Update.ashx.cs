using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.Project
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string projectId=context.Request["projectId"];
            string userId = context.Request["userId"];
            string projectName = context.Request["projectName"];
            string projectIntro = context.Request["projectIntro"];
            string amount = context.Request["amount"];
            string status = context.Request["status"];
            string remark = context.Request["remark"];

            string type = context.Request["type"];
            string contack = context.Request["contack"];
            string phone = context.Request["phone"];
            string ex1 = context.Request["ex1"];
            string ex2 = context.Request["ex2"];
            string ex3 = context.Request["ex3"];
            string ex4 = context.Request["ex4"];
            string ex5 = context.Request["ex5"];
            string ex6 = context.Request["ex6"];
            string ex7 = context.Request["ex7"];
            string ex8 = context.Request["ex8"];
            string ex9 = context.Request["ex9"];
            string ex10 = context.Request["ex10"];
            string ex11 = context.Request["ex11"];
            string ex12 = context.Request["ex12"];
            string ex13 = context.Request["ex13"];
            string ex14 = context.Request["ex14"];
            string msg = "";
            if (string.IsNullOrEmpty(userId)) userId = bllUser.GetCurrUserID();
            apiResp.status = bll.UpdateProject(int.Parse(projectId), userId, projectName, projectIntro, amount, status, out msg, remark, ex1, ex2, ex3, ex4, ex5, ex6, ex7, ex8, ex9, ex10, ex11, ex12, ex13, ex14, type,contack,phone);
            if (apiResp.status)
            {
                apiResp.msg = "修改成功";

            }
            else
            {
                apiResp.msg = msg;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

    }
}