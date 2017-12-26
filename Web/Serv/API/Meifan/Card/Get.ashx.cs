using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Card
{
    /// <summary>
    ///会员卡详情
    /// </summary>
    public class Get : BaseHandlerNoAction
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
                amount=data.Amount,
                //amount = bll.GetTrueAmount(data),
                //server_amount = bll.GetTrueServerAmount(data),
                server_amount =data.ServerAmount,
                pre_amount = data.Amount,
                valid_month = data.ValidMonth,
                description = data.Description,
            };
            bll.ContextResponse(context, apiResp);

        }



    }
}