using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery
{
    /// <summary>
    ///删除刮奖
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BllLottery bllLottery = new BllLottery();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string forceDelete = context.Request["force_delete"];
            //force_delete等于1时，则不管是否存在中奖记录，进行强制删除
            if (forceDelete != "1")
            {
                if (bllLottery.GetCountByKey<WXLotteryRecordV1>("LotteryId", id)>0)
                {
                    resp.errcode = (int)APIErrCode.LotteryHaveRecord;
                    resp.errmsg = string.Format("已经有人中奖,不能删除");
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
            }
            BLLTransaction tran = new BLLTransaction();
            try
            {
                if (bllLottery.DeleteByKey<WXLotteryV1>("LotteryID", id, tran)==-1){
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "删除活动主表失败";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
                
                if (bllLottery.DeleteByKey<WXAwardsV1>("LotteryId", id, tran)==-1){
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "删除奖项失败";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
                if (bllLottery.DeleteByKey<WXLotteryRecordV1>("LotteryId", id, tran)==-1){
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "删除中奖记录失败";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
                if (bllLottery.DeleteByKey<WXLotteryLogV1>("LotteryId", id, tran)==-1){
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "删除抽奖记录失败";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
                if (bllLottery.DeleteByKey<WXLotteryWinningDataV1>("LotteryId", id, tran) == -1)
                {
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "删除默认中奖设置失败";
                    bllLottery.ContextResponse(context, resp);
                    return;
                }
                tran.Commit();
                resp.isSuccess = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            bllLottery.ContextResponse(context, resp);
        }
    }
}