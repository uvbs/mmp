using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Card
{
    /// <summary>
    /// 会员卡列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = 1;
            int pageSize = 1000;
            int totalCount;
            var data = bll.CardList(pageIndex, pageSize, "", "", out totalCount);
            var list = from p in data
                       select new
                       {
                           card_id = p.CardId,
                           card_name = p.CardName,
                           card_name_en = p.CardNameEn,
                           card_img = p.CardImg,
                           card_type = p.CardType,
                           amount = p.Amount,
                           server_amount = p.ServerAmount,
                           valid_month = p.ValidMonth,
                           is_enable=p.IsDisable,
                           description=p.Description,



                       };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list
            };

            bll.ContextResponse(context, apiResp);

        }

       
    }
}