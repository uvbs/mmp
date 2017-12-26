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
    /// 新增储值卡
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = bll.ConvertRequestToModel<RequestModel>(new RequestModel());
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

            if (bll.AddCard(requestModel.name, requestModel.amount, requestModel.max_count, requestModel.valid_to, 
                requestModel.bg_img, bll.WebsiteOwner, currentUserInfo.UserID))
            {
                apiResp.status = true;
                apiResp.msg = "新增储值卡完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "新增储值卡失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
        public class RequestModel
        {
            /// <summary>
            /// id
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 卡名
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// 金额
            /// </summary>
            public decimal amount { get; set; }

            /// <summary>
            /// 最大发放数
            /// </summary>
            public int max_count { get; set; }

            /// <summary>
            /// 有效期
            /// </summary>
            public DateTime? valid_to { get; set; }

            /// <summary>
            /// 背景图
            /// </summary>
            public string bg_img { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            public string status { get; set; }
        }
    }
}