using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 更新剩余刮奖次数
    /// </summary>
    public class UpdateLotteryCount : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 抽奖BLL
        /// </summary>
        BLLJIMP.BllLottery bllLottery = new BLLJIMP.BllLottery();
        public void ProcessRequest(HttpContext context)
        {

           
            string count = context.Request["count"];//购买抽奖次数
            string lotteryId = context.Request["lottery_id"];//抽奖活动id
            int countInt = 0;
            if (string.IsNullOrEmpty(lotteryId))
            {
                apiResp.code = 1;
                apiResp.msg = "lottery_id 参数必传";
                bllLottery.ContextResponse(context, apiResp);
                return;
                
            }
            if (string.IsNullOrEmpty(count))
            {
                apiResp.code = 1;
                apiResp.msg = "count 参数必传";
                bllLottery.ContextResponse(context, apiResp);
                return;

            }
            if (!int.TryParse(count,out countInt))
            {
                apiResp.code = 1;
                apiResp.msg = "count 参数 错误";
                bllLottery.ContextResponse(context, apiResp);
                return;
            }
            if (countInt<=0)
            {
                apiResp.code = 1;
                apiResp.msg = "count 参数必须大于0";
                bllLottery.ContextResponse(context, apiResp);
                return;
            }

            WXLotteryV1 lotteryInfo = bllLottery.GetLottery(int.Parse(lotteryId));
            if (lotteryInfo == null)
            {
                apiResp.code = 1;
                apiResp.msg = "lottery_id 不存在";
                bllLottery.ContextResponse(context, apiResp);
                return;

            }
            if (lotteryInfo.WebsiteOwner != bllLottery.WebsiteOwner)
            {
                apiResp.code = 1;
                apiResp.msg = "lottery_id 不存在";
                bllLottery.ContextResponse(context, apiResp);
                return;
            }
            if (lotteryInfo.UsePoints > 0)
            {

                if (CurrentUserInfo.TotalScore >= (lotteryInfo.UsePoints * countInt))
                {

                    CurrentUserInfo.TotalScore -= (lotteryInfo.UsePoints * countInt);
                    CurrentUserInfo.LotteryCount += countInt;
                    if (bllLottery.Update(CurrentUserInfo))
                    {
                        apiResp.status = true;
                        apiResp.msg = "ok";
                    }

                }
                else
                {
                    apiResp.code = 1;
                    apiResp.msg = "您的积分不足";
                }

            }
            else
            {
                apiResp.code = 1;
                apiResp.msg = "该活动未设置积分兑换刮奖次数";
            }

            bllLottery.ContextResponse(context, apiResp);


        }


    }
}