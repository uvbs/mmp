using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Card
{
    /// <summary>
    /// UpdateEnable 的摘要说明
    /// </summary>
    public class UpdateEnable : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string cardIds = context.Request["card_ids"];
            string isEnable=context.Request["is_enable"];
            if (bll.Update(new MeifanCard(), string.Format(" IsDisable={0}", isEnable), string.Format(" CardId in({0})", cardIds)) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.msg = "操作失败";
            }

            bll.ContextResponse(context, apiResp);


        }

    }
}