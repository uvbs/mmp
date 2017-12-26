using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.SVCard
{
    /// <summary>
    /// 获取储值卡详情
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        BLLUser bllUser = new BLLUser();
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            try
            {

            
            string id = context.Request["id"];
            string give = context.Request["give"]; //是否是领卡页
            string websiteOwner = bll.WebsiteOwner;
            StoredValueCardRecord record = bll.GetByKey<StoredValueCardRecord>("AutoId", id, websiteOwner: websiteOwner);
            StoredValueCard card = bll.GetByKey<StoredValueCard>("AutoId", record.CardId.ToString(), websiteOwner: websiteOwner);

            UserInfo fromUser = bllUser.GetUserInfo(record.UserId, websiteOwner);
            UserInfo toUser = null;
            if (!string.IsNullOrWhiteSpace(record.ToUserId)) toUser = bllUser.GetUserInfo(record.ToUserId, websiteOwner);

            int useStatus = bll.GetUseStatus(card, record, CurrentUserInfo, fromUser, toUser, give == "1");
            
            apiResp.result = new{
                id = record.AutoId,
                card_id = record.CardId,
                card_number = record.CardNumber,
                name = card.Name,
                amount = card.Amount,
                bg_img = card.BgImage,
                create_date = record.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                valid_to = !record.ValidTo.HasValue ? "" : record.ValidTo.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                use_date = !record.UseDate.HasValue ? "" : record.UseDate.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                cur_user_id = CurrentUserInfo.AutoID,
                user_id = fromUser == null ? 0 : fromUser.AutoID,
                user_nickname = fromUser == null ? "" : bllUser.GetUserDispalyName(fromUser),
                user_avatar = fromUser == null ? "" : bllUser.GetUserDispalyAvatar(fromUser),
                touser_id = toUser == null ? 0 : toUser.AutoID,
                touser_nickname = toUser == null ? "" : bllUser.GetUserDispalyName(toUser),
                touser_avatar = toUser == null ? "" : bllUser.GetUserDispalyAvatar(toUser),
                touser_phone = toUser == null ? "" : toUser.Phone,
                status = record.Status,
                use_status = useStatus,
                canuse_amount = bllMall.GetStoreValueCardCanUseAmount(record.AutoId.ToString(), fromUser.UserID)
            };
            apiResp.msg = "前台查询储值卡详情";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
            }
            catch (Exception ex)
            {
                apiResp.result = ex.ToString();
                bll.ContextResponse(context, apiResp);


            }
        }

        //public bool IsReusable
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
    }
}