using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.User;

namespace ZentCloud.JubitIMP.Web.Serv.API.Config
{
    /// <summary>
    /// GetUserInfoFields 的摘要说明
    /// </summary>
    public class GetUserInfoFields : BaseHandlerNoAction
    {
        BLL bll = new BLL();
        public void ProcessRequest(HttpContext context)
        {
            WebsiteInfo webInfo = bll.GetWebsiteInfoModelFromDataBase();
            if (!string.IsNullOrWhiteSpace(webInfo.UserCenterFieldJson))
            {
                apiResp.result = JToken.Parse(webInfo.UserCenterFieldJson);
            }
            else
            {
                apiResp.result =  new JObject();
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
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