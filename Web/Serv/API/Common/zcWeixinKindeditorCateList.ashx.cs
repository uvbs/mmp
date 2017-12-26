using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// zcWeixinKindeditorCateList 的摘要说明
    /// </summary>
    public class zcWeixinKindeditorCateList : BaseHandlerNoAction
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        public void ProcessRequest(HttpContext context)
        {
            apiResp.result = Comm.StaticData.zcWxEditorData;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllKeyValueData.ContextResponse(context, apiResp);
        }
    }
}