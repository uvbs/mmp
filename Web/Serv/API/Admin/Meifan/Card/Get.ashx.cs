using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Card
{
    /// <summary>
    /// 获取会员卡
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string cardId = context.Request["card_id"];

            if (string.IsNullOrEmpty(cardId))
            {
                apiResp.status = false;
                apiResp.msg = "card_id 参数必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            var data = bll.GetCard(cardId);
            if (data == null)
            {
                apiResp.status = false;
                apiResp.msg = "card_id 参数错误";
                bll.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                card_id = data.CardId,
                card_name = data.CardName,
                card_name_en = data.CardNameEn,
                card_img = data.CardImg,
                card_type = data.CardType,
                amount = data.Amount,
                server_amount = data.ServerAmount,
                valid_month = data.ValidMonth,
                description = data.Description,
            };
            bll.ContextResponse(context, apiResp);

        }

        
    }
}