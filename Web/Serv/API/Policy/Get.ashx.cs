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
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            JuActivityInfo nInfo = bllJuActivity.GetByKey<JuActivityInfo>("JuActivityID", id, true);
            if (nInfo == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "政策未找到";
                bllJuActivity.ContextResponse(context, apiResp);
                return;
            }
            List<JuActivityFiles> nFiles = bllJuActivity.GetColMultListByKey<JuActivityFiles>(int.MaxValue, 1, "JuActivityID", id, "AutoId,FileName,FilePath,FileClass");
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                id = nInfo.JuActivityID,
                policy_name = nInfo.ActivityName,
                subsidy_standard = nInfo.K3,
                subsidy_period = nInfo.K4,
                policy_number = nInfo.K5,
                policy_level = nInfo.K6,
                domicile_place = nInfo.K7,
                summary = nInfo.Summary,
                policy_files = from p in nFiles.Where(p => p.FileClass == 1)
                               select new
                               {
                                   file_name=p.FileName,
                                   file_path=p.FilePath
                               },
                guide_files = from p in nFiles.Where(p => p.FileClass == 2)
                               select new
                               {
                                   file_name=p.FileName,
                                   file_path=p.FilePath
                               }
            };
            bllJuActivity.ContextResponse(context, apiResp);
        }
    }
}