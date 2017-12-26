using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.Project
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string userId=context.Request["userId"];
            string projectName=context.Request["projectName"];
            string projectIntro=context.Request["projectIntro"];
            string amount=context.Request["amount"];
            string status =context.Request["status"];
            string remark=context.Request["remark"];

            string type=context.Request["type"];
            string contack = context.Request["contack"];
            string phone=context.Request["phone"];
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

            if (string.IsNullOrEmpty(userId)) userId = bllUser.GetCurrUserID();

            string msg="";
            apiResp.status = bll.AddProject(userId, projectName, projectIntro,amount, out msg, status, contack, phone, "", "", remark, ex1, ex2, ex3, ex4, ex5, ex6, ex7, ex8, ex9, ex10, ex11, ex12, ex13,ex14, type);
            if (apiResp.status)
            {
                apiResp.msg = "添加成功";
            }
            else
            {
                apiResp.msg = msg;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }


    }
}