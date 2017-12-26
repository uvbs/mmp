using CommonPlatform.Helper.Aspose;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// ImportWord 的摘要说明
    /// </summary>
    public class ImportWord : BaseHandlerNoAction
    {
        BLL bll = new BLL();
        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile postFile = context.Request.Files[0];
            string result = WordHelper.WordToHtml(postFile.InputStream);
            apiResp.result = result;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "导入成功";
            bll.ContextResponse(context, apiResp);
        }
    }
}