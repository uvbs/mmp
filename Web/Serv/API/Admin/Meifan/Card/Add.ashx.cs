using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Card
{
    /// <summary>
    /// 添加会员卡
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            //模型
            CardModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<CardModel>(data);
            }
            catch (Exception ex)
            {

                apiResp.msg = "格式错误,请检查。错误信息:" + ex.Message;
                bll.ContextResponse(context, apiResp);
                return;

            }
            MeifanCard model = new MeifanCard();
            model.InsertDate = DateTime.Now;
            model.Websiteowner = bll.WebsiteOwner;
            model.CardId = bll.GetGUID(BLLJIMP.TransacType.CommAdd);
            model.Amount = requestModel.amount;
            model.CardImg = requestModel.card_img;
            model.CardName = requestModel.card_name;
            model.CardNameEn = requestModel.card_name_en;
            model.CardType = requestModel.card_type;
            model.Description = requestModel.description;
            model.IsDelete = 0;
            model.IsDisable =1;
            model.ServerAmount = requestModel.server_amount;
            model.ValidMonth = requestModel.valid_month;
            if (bll.Add(model))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.msg = "添加失败";
            }
            bll.ContextResponse(context, apiResp);



        }

        /// <summary>
        /// 模型
        /// </summary>
        public class CardModel
        {
            /// <summary>
            /// 卡id
            /// </summary>
            public string card_id { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string card_name { get; set; }
            /// <summary>
            /// 名称 英文
            /// </summary>
            public string card_name_en { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string card_img { get; set; }
            /// <summary>
            ///类型
            ///个人
            ///personal
            ///家庭
            ///family
            ///船东卡
            ///chuandong
            /// </summary>
            public string card_type { get; set; }
            /// <summary>
            /// 金额
            /// </summary>
            public decimal amount { get; set; }
            /// <summary>
            /// 手续费
            /// </summary>
            public decimal server_amount { get; set; }
            /// <summary>
            /// 有效期
            /// </summary>
            public int valid_month { get; set; }
            /// <summary>
            /// 说明
            /// </summary>
            public string description { get; set; }


        }


    }
}