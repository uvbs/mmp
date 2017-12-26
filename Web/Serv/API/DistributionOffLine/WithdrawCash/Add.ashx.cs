using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.WithdrawCash
{
    /// <summary>
    /// 申请提现
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        //BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        /// <summary>
        /// 线上分销BLL
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            string amount = context.Request["amount"];//提现金额
            string bankCardId=context.Request["bank_card_id"];//银行卡ID
            string type=context.Request["type"];//到账类型 0银行卡1微信
            if (string.IsNullOrEmpty(type))
            {
                type = "0";

            }
            if (string.IsNullOrEmpty(amount))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "amount 参数必填";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            if (string.IsNullOrEmpty(bankCardId))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "bank_card_id 参数必填";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            string msg="";
            apiResp.status = bllDis.ApplyWithrawCash(CurrentUserInfo, bankCardId, amount, bllDis.WebsiteOwner, int.Parse(type), out msg);
            if (apiResp.status)
            {
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.code=(int)APIErrCode.OperateFail ;
                apiResp.msg = msg;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}