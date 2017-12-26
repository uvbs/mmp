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
    /// 储值卡列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        BLLUser bllUser = new BLLUser();
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            try
            {

            
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string status = context.Request["status"]; //0未使用 1已使用 2已转赠 3已过期
            string websiteOwner = bll.WebsiteOwner;
            string curUserId = CurrentUserInfo.UserID;
            int total = bll.GetRecordCount(null, websiteOwner, status, curUserId);

            List<StoredValueCardRecord> list = new List<StoredValueCardRecord>();
            if (total > 0) list = bll.GetRecordList(rows, page, null, websiteOwner, status, curUserId);
            List<dynamic> rList = new List<dynamic>();
            List<UserInfo> uList = new List<UserInfo>();
            if (list.Count > 0)
            {
                foreach (StoredValueCardRecord item in list)
                {
                    StoredValueCard card = bll.GetColByKey<StoredValueCard>("AutoId", item.CardId.ToString(), "AutoId,Name,BgImage", websiteOwner: websiteOwner);
                    UserInfo user = uList.FirstOrDefault(p=>p.UserID == item.UserId);
                    if (user == null) {
                        user = bll.GetColByKey<UserInfo>("UserID", item.UserId, "AutoID", websiteOwner: websiteOwner);
                        uList.Add(user);
                    }
                    if (user==null)
                    {
                        continue;
                    }
                    UserInfo toUser = null;
                    if (!string.IsNullOrWhiteSpace(item.ToUserId)) {
                        toUser = uList.FirstOrDefault(p => p.UserID == item.ToUserId);
                        if (toUser == null) {
                            toUser = bll.GetColByKey<UserInfo>("UserID", item.ToUserId, "AutoID", websiteOwner: websiteOwner);
                            uList.Add(toUser);
                        }
                    }
                    if (toUser==null)
                    {
                        continue;
                    }
                    int useStatus = bll.GetUseStatus(card, item, CurrentUserInfo, user, toUser, false);
                    rList.Add(new
                    {
                        id = item.AutoId,
                        card_id = item.CardId,
                        card_number = item.CardNumber,
                        amount = item.Amount,
                        name = card.Name,
                        bg_img = card.BgImage,
                        valid_to = !item.ValidTo.HasValue ? "" : item.ValidTo.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                        status = item.Status,
                        use_status = useStatus,
                        canuse_amount = string.IsNullOrEmpty(item.ToUserId) ? bllMall.GetStoreValueCardCanUseAmount(item.AutoId.ToString(), CurrentUserInfo.UserID) : bllMall.GetStoreValueCardCanUseAmount(item.AutoId.ToString(), item.UserId)
                    });
                }
            }
            apiResp.msg = "前台查询储值卡发放列表";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = new
            {
                totalcount = total,
                list = rList
            };
            bll.ContextResponse(context, apiResp);
            }
            catch (Exception ex)
            {

                apiResp.msg = ex.ToString();
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
        }
    }
}