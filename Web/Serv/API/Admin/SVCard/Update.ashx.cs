using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SVCard
{
    /// <summary>
    /// 修改储值卡
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            Add.RequestModel requestModel = bll.ConvertRequestToModel<Add.RequestModel>(new Add.RequestModel());

            if (string.IsNullOrWhiteSpace(requestModel.id) || requestModel.id == "0")
            {
                apiResp.msg = "储值卡编号错误";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (bll.GetCountByKey<StoredValueCardRecord>("CardId", requestModel.id, true) > 0)
            {
                apiResp.msg = "储值卡存在发放记录，禁止修改";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.name))
            {
                apiResp.msg = "请输入卡名";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.amount <= 0)
            {
                apiResp.msg = "请输入金额";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.max_count <= 0)
            {
                apiResp.msg = "请输入最大发放数";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.bg_img))
            {
                apiResp.msg = "请选择背景图";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bll.ContextResponse(context, apiResp);
                return;
            }
            string msg = "";
            if (bll.UpdateCard(requestModel.id, requestModel.name, requestModel.amount, requestModel.max_count,requestModel.valid_to,
            requestModel.bg_img, currentUserInfo.UserID, bll.WebsiteOwner, out msg))
            {
                apiResp.status = true;
                apiResp.msg = "修改储值卡完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = msg;
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}