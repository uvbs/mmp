using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Coupon
{
    /// <summary>
    /// GetCount 的摘要说明
    /// </summary>
    public class GetCount : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        public void ProcessRequest(HttpContext context)
        {
            string cardcouponStatus = context.Request["cardcoupon_status"];
            string isCanUse = context.Request["is_can_use"];//可以正常使用的标识
            string cardcouponType = context.Request["cardcoupon_type"];//可以正常使用的标识
            int count = bllCardCoupon.GetCardCouponCount(bllCardCoupon.WebsiteOwner, CurrentUserInfo.UserID,
                cardcouponStatus, isCanUse, cardcouponType);

            apiResp.status = true;
            apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.result = count;
            bllCardCoupon.ContextResponse(context, apiResp);
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