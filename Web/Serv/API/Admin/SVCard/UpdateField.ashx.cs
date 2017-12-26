using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SVCard
{
    /// <summary>
    /// UpdateField 的摘要说明
    /// </summary>
    public class UpdateField : BaseHandlerNeedLoginAdminNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string field = context.Request["field"];
            string value = context.Request["value"];

            Dictionary<string, string> fieldDic = new Dictionary<string, string>();
            fieldDic.Add("max_count", "MaxCount");
            fieldDic.Add("valid_to", "ValidTo");
            if (!fieldDic.Keys.Contains(field))
            {
                apiResp.msg = "修改对象不支持";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }

            if (field == "valid_to")
            {
                if (value == "")
                {
                    value = null;
                }
                else
                {
                    DateTime tryTime = DateTime.Now;
                    if (!DateTime.TryParse(value, out tryTime))
                    {
                        apiResp.msg = "有效期时间格式错误";
                        apiResp.code = (int)APIErrCode.OperateFail;
                        bll.ContextResponse(context, apiResp);
                        return;
                    }
                }
            }

            if (bll.UpdateMultByKey<StoredValueCard>("AutoId", ids, fieldDic[field], value, null, true) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "批量修改储值卡成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "批量修改储值卡失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}