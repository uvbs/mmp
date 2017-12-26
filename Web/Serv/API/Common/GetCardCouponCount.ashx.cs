using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// GetCardCouponCount 的摘要说明
    /// </summary>
    public class GetCardCouponCount : BaseHandlerNeedLoginNoAction
    {
        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            string websiteOwner = bllCardCoupon.WebsiteOwner;
            string curUserId = CurrentUserInfo.UserID;

            int couponCount = bllCardCoupon.GetCardCouponCount(websiteOwner, curUserId, null, "1", null);
            int cardCount = bll.GetCanUseStoredValueCardList(curUserId).Count;
          
            apiResp.msg = "前台查询储卡券数量";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = couponCount + cardCount;
            bll.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}