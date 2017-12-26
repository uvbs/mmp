using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// GetSystemTime 的摘要说明
    /// </summary>
    public class GetSystemTime : IHttpHandler
    {
        /// <summary>
        /// Api响应模型
        /// </summary>
        protected BaseResponse apiResp = new BaseResponse();
        public void ProcessRequest(HttpContext context)
        {
            BLL bll = new BLL();
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "获取成功";
            apiResp.result = ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now);
            bll.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}