using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SignIn.Address
{
    /// <summary>
    /// 签到地址列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLSignIn bllSignIn = new BLLSignIn();
        public void ProcessRequest(HttpContext context)
        {
            int page = Convert.ToInt32( context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            string type=context.Request["type"];
            int total = 0;
            List<SignInAddress> list = bllSignIn.GetSignInAddressList(rows, page, keyword, bllSignIn.WebsiteOwner, out total,true,type);
            var result = from p in list
                         select new
                         {
                             id = p.AutoID,
                             address = p.Address,
                             longitude = p.Longitude,
                             latitude = p.Latitude,
                             range = p.Range,
                             isdelete = p.IsDelete,
                             type=p.Type,
                             desc=p.Description,
                             lotteryid=p.LotteryId

                         };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                totalcount = total,
                list = result
            };
            bllSignIn.ContextResponse(context, apiResp);
        }

    }
}