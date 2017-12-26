using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Card
{
    /// <summary>
    /// 发放会员卡
    /// </summary>
    public class Send : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        public void ProcessRequest(HttpContext context)
        {
            string userId = context.Request["user_id"];
            string cardId = context.Request["card_id"];
            string validDate = context.Request["valid_date"];
            string orderId=context.Request["order_id"];
            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.msg = "order_id 必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(cardId))
            {
                apiResp.msg = "card_id 必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(userId))
            {
                apiResp.msg = "user_id 必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            var userInfo = bllUser.GetUserInfo(userId, bll.WebsiteOwner);
            if (userInfo == null)
            {
                apiResp.msg = "user_id 错误";
                bll.ContextResponse(context, apiResp);
                return;
            }
            var card = bll.GetCard(cardId);
            if (cardId == null)
            {
                apiResp.msg = "card_id 错误";
                bll.ContextResponse(context, apiResp);
                return;
            }

            if (bll.GetCount<MeifanMyCard>(string.Format(" CardId='{0}' And UserId='{1}'", cardId, userId)) > 0)
            {
                apiResp.msg = "已经开过卡了";
                bll.ContextResponse(context, apiResp);
                return;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            MeifanMyCard model = new MeifanMyCard();
            model.CardId = card.CardId;
            model.CardNum = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), userInfo.AutoID);
            model.InsertDate = DateTime.Now;
            model.UserId = userInfo.UserID;
            model.ValidMonth = card.ValidMonth;
            model.ValidDate = Convert.ToDateTime(validDate);
            if (bll.Add(model,tran))
            {
                //Ex1 开卡状态
                //Ex2 过期时间
                //Ex3 会员卡号
                //Ex4 到期日期
                if (bll.Update(new OrderPay(), string.Format("Ex1='1',Ex2='{0}',RelationId='{1}',Ex3='{2}',Ex4='{3}'", validDate, model.CardId, model.CardNum, (model.ValidDate.AddMonths(model.ValidMonth).ToString())), string.Format(" OrderId='{0}'", orderId), tran) > 0)
                {
                    apiResp.status = true;
                    apiResp.msg = "ok";
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                    apiResp.msg = "发卡失败";

                }

            }
            else
            {
                apiResp.msg = "发卡失败";
            }
            bll.ContextResponse(context, apiResp);


        }


    }
}