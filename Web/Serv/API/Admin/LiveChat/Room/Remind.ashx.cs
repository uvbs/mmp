using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.LiveChat.Room
{
    /// <summary>
    /// Remind 的摘要说明
    /// </summary>
    public class Remind : BaseHandlerNeedLoginAdminNoAction
    {

        BLLLiveChat bll = new BLLLiveChat();
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            DateTime dtNow = DateTime.Now;
            if (bll.GetCount<LiveChatRoom>(string.Format("Websiteowner='{0}' And UnReadCount>0 ",bll.WebsiteOwner)) > 0)
            {
                apiResp.status = true;

            }
            bll.ContextResponse(context, apiResp);

        }
    }
}