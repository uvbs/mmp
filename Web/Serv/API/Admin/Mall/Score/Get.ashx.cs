using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Score
{
    /// <summary>
    /// 积分配置获取接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL 积分配置
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        public void ProcessRequest(HttpContext context)
        {
            ScoreConfig scoerConfig = bllScore.GetScoreConfig();
            if (scoerConfig == null)
            {
                resp.errcode = 1;
                resp.errmsg = "没有设置积分配置";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requestModel = new RequestModel();
            requestModel.order_amount = scoerConfig.OrderAmount;
            requestModel.order_score = scoerConfig.OrderScore;
            requestModel.exchange_amount = scoerConfig.ExchangeAmount;
            requestModel.exchange_score = scoerConfig.ExchangeScore;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
        }

        /// <summary>
        /// 返回实体
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 订单金额
            /// </summary>
            public int order_amount { get; set; }

            /// <summary>
            /// 所得积分
            /// </summary>
            public int? order_score { get; set; }

            /// <summary>
            /// 积分兑换 金额
            /// </summary>
            public decimal exchange_amount { get; set; }

            /// <summary>
            /// 积分兑换 积分
            /// </summary>
            public int exchange_score { get; set; }
        }



    }
}