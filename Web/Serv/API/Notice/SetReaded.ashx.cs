using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Notice
{
    /// <summary>
    /// SetReaded 的摘要说明
    /// </summary>
    public class SetReaded : NoticeBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }

                string id = context.Request["id"];

                if (bll.Update(new BLLJIMP.Model.SystemNotice(), " Readtime=GETDATE() ", string.Format("AutoID={0}", id)) > 0)
                {
                    resp.isSuccess = true;
                    resp.errcode = 0;
                    resp.errmsg = "操作成功";
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "操作失败";
                }

            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }
    }
}