using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SVCard.Record
{
    /// <summary>
    /// 储值卡发放列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string status = context.Request["status"];
            string card_id = context.Request["card_id"];
            string websiteOwner = bll.WebsiteOwner;
            int total = bll.GetRecordCount(card_id,websiteOwner, status, null);
            List<StoredValueCardRecord> list = new List<StoredValueCardRecord>();
            if (total > 0) list = bll.GetRecordList(rows, page, card_id, websiteOwner, status, null);

            List<dynamic> rList = new List<dynamic>();
            if (list.Count > 0)
            {
                StoredValueCard card = bll.GetColByKey<StoredValueCard>("AutoId", card_id, "AutoId,Name,BgImage", websiteOwner: websiteOwner);
                foreach (StoredValueCardRecord item in list)
                {
                    UserInfo user = bllUser.GetUserInfo(item.UserId,websiteOwner);
                    UserInfo toUser = null;
                    if (!string.IsNullOrWhiteSpace(item.ToUserId)) toUser = bllUser.GetUserInfo(item.ToUserId, websiteOwner);
                    int useStatus = bll.GetUseStatusByAdmin(item);
                    rList.Add(new
                    {
                        id = item.AutoId,
                        card_id = item.CardId,
                        card_number = item.CardNumber,
                        name = card.Name,
                        bg_img = card.BgImage,
                        create_date = item.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                        valid_to = !item.ValidTo.HasValue ? "" : item.ValidTo.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                        use_date = !item.UseDate.HasValue ? "" : item.UseDate.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                        user_id = user == null ? 0 : user.AutoID,
                        user_userId = item.UserId,
                        user_nickname = user == null ? "" : bllUser.GetUserDispalyName(user),
                        user_avatar = user == null ? "" : bllUser.GetUserDispalyAvatar(user),
                        user_phone = user == null ? "" : user.Phone,
                        touser_id = toUser == null ? 0 : toUser.AutoID,
                        touser_userId = item.ToUserId,
                        touser_nickname = toUser == null ? "" : bllUser.GetUserDispalyName(toUser),
                        touser_avatar = toUser == null ? "" : bllUser.GetUserDispalyAvatar(toUser),
                        touser_phone = toUser == null ? "" : toUser.Phone,
                        status = item.Status,
                        use_status = useStatus,
                        amount=item.Amount,
                        canuse_amount = string.IsNullOrEmpty(item.ToUserId) ? bllMall.GetStoreValueCardCanUseAmount(item.AutoId.ToString(), item.UserId) : bllMall.GetStoreValueCardCanUseAmount(item.AutoId.ToString(), item.UserId)
                    });
                }
            }
            apiResp.msg = "查询储值卡发放列表";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = new {
                totalcount = total,
                list = rList
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}