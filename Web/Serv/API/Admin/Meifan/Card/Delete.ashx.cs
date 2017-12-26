using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Card
{
    /// <summary>
    /// 删除
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string cardIds = context.Request["card_ids"];
            if (bll.Update(new MeifanCard(),string.Format(" IsDelete=1"),string.Format(" CardId in({0})",cardIds))>0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.msg = "删除失败";
            }

            bll.ContextResponse(context, apiResp);


        }

       
    }
}