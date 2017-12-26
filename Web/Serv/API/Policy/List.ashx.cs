using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Policy
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            string policy_object = context.Request["policy_object"];
            string policy_level = context.Request["policy_level"];
            string domicile_place = context.Request["domicile_place"];
            string sex = context.Request["sex"];
            string age = context.Request["age"];
            string education = context.Request["education"];
            string graduation_year = context.Request["graduation_year"];
            string employment_status = context.Request["employment_status"];
            string current_job_life = context.Request["current_job_life"];
            string unemployment_period = context.Request["unemployment_period"];
            string company_type = context.Request["company_type"];
            string registered_capital = context.Request["registered_capital"];
            string personnel_size = context.Request["personnel_size"];
            string company_size = context.Request["company_size"];
            string industry = context.Request["industry"];

            int total = 0;
            List<JuActivityInfo> dataList = bllJuActivity.GetPolicyList(rows, page, null, null, keyword, false,
                "JuActivityID,ActivityName,Summary,Sort,K2,K3,K4,K6", out total,
                null, bllJuActivity.WebsiteOwner, null, policy_object, policy_level, domicile_place, sex, age, education, graduation_year, employment_status,
                current_job_life, unemployment_period, company_type, registered_capital, personnel_size, company_size, industry);

            var rData = from p in dataList
                        select new
                        {
                            id = p.JuActivityID,
                            policy_name = p.ActivityName,
                            subsidy_standard = p.K3,
                            subsidy_period = p.K4,
                            policy_level = p.K6,
                            summary = p.Summary
                        };
            apiResp.result = new
            {
                totalcount = total,
                list = rData
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllJuActivity.ContextResponse(context, apiResp);

        }

    }
}