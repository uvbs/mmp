using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Score
{
    /// <summary>
    /// 商城积分配置更新接口
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL 积分配置
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();

        public  void ProcessRequest(HttpContext context)
        {
            string orderAmount = context.Request["order_amount"];//订单金额
            string orderScore = context.Request["order_score"];//所得积分
            string exchangeAmount = context.Request["exchange_amount"];//积分兑换 金额
            string exchangeScore = context.Request["exchange_score"];//积分兑换 积分

            ScoreConfig model = bllScore.GetScoreConfig();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(orderAmount))
                {
                    model.OrderAmount = int.Parse(orderAmount);
                }

                if (!string.IsNullOrEmpty(orderScore))
                {
                    model.OrderScore = int.Parse(orderScore);
                }

                if (!string.IsNullOrEmpty(exchangeAmount))
                {
                    model.ExchangeAmount = decimal.Parse(exchangeAmount);
                }
                if (!string.IsNullOrEmpty(exchangeScore))
                {
                    model.ExchangeScore = int.Parse(exchangeScore);
                }
                if (bllScore.Update(model))
                {
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                    resp.isSuccess = true;

                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "修改积分配置出错";
                }
            }
            else
            {
                model = new ScoreConfig();

                model.WebsiteOwner = bllScore.WebsiteOwner;

                if (!string.IsNullOrEmpty(orderAmount))
                {
                    model.OrderAmount = int.Parse(orderAmount);
                }

                if (!string.IsNullOrEmpty(orderScore))
                {
                    model.OrderScore = int.Parse(orderScore);
                }

                if (!string.IsNullOrEmpty(exchangeAmount))
                {
                    model.ExchangeAmount = decimal.Parse(exchangeAmount);
                }
                if (!string.IsNullOrEmpty(exchangeScore))
                {
                    model.ExchangeScore = int.Parse(exchangeScore);
                }
                if (bllScore.Add(model))
                {
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                    resp.isSuccess = true;

                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "添加积分出错";
                }

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}