using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ApplyPass 的摘要说明
    /// </summary>
    public class ApplyPass : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLWeixin bllWeixin = new BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<UserInfo> list = bllUser.GetList<UserInfo>(string.Format(" AutoID In ({0}) ", ids));
            list = list.Where(p => p.MemberApplyStatus < 9 || p.AccessLevel < 1).ToList();
            if (list.Count > 0)
            {
                BLLTransaction tran = new BLLTransaction();
                foreach (var item in list)
                {
                    if (item.AccessLevel < 1) {
                        item.AccessLevel = 1;
                    }
                    item.MemberStartTime = DateTime.Now;
                    item.MemberApplyStatus = 9;
                    if (!bllUser.Update(item, tran))
                    {
                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "提交失败";
                        bllUser.ContextResponse(context, apiResp);
                        return;
                    }

                }
                tran.Commit();
                foreach (var user in list)
                {
                     bllWeixin.SendTemplateMessageNotifyComm(user, "审核通过", "您已经通过会员审核");
                }
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "提交完成";
            bllUser.ContextResponse(context, apiResp);
        }
    }
}