using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Card.My
{
    /// <summary>
    /// 我的会员卡列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            var data = bll.MyCardList(CurrentUserInfo.UserID);
            List<MyCardModel> list = new List<MyCardModel>();
            List<MeifanCard> cardList = new List<MeifanCard>();
            foreach (var item in data)
            {
                MyCardModel model = new MyCardModel();
                MeifanCard card = bll.GetCard(item.CardId);
                cardList.Add(card);
                model.id = item.AutoId;
                model.card_id = item.CardId;
                model.bind_name = CurrentUserInfo.TrueName;
                model.card_img = card.CardImg;
                model.card_name = card.CardName;
                model.card_name_en = card.CardNameEn;
                model.card_number = item.CardNum;
                model.card_type = card.CardType;
                model.create_date = item.ValidDate.ToString("yyyy-MM-dd");
                model.description = card.Description;
                model.expire_date = bll.GetMyCardExpireDate(item);
                model.over_days = bll.GetMyCardOverDays(item).ToString();
                model.is_defualt = 0;
                list.Add(model);

            }
            if (cardList.Count > 0)
            {
                var def = cardList.OrderByDescending(p => p.Amount).First();

               var defu= list.Where(p => p.card_id == def.CardId).ToList()[0];
               defu.is_defualt = 1;

            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = data.Count,
                list = list
            };

            bll.ContextResponse(context, apiResp);

        }

        /// <summary>
        /// 会员卡模型
        /// </summary>
        public class MyCardModel
        {
            /// <summary>
            /// id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 主卡id
            /// </summary>
            public string card_id { get; set; }
            /// <summary>
            /// 卡号
            /// </summary>
            public string card_number { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string card_name { get; set; }
            /// <summary>
            /// 英文名称
            /// </summary>
            public string card_name_en { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string card_img { get; set; }
            /// <summary>
            /// 类型
            /// 
            /// </summary>
            public string card_type { get; set; }
            /// <summary>
            /// 绑定人
            /// </summary>
            public string bind_name { get; set; }
            /// <summary>
            /// 开卡时间
            /// </summary>
            public string create_date { get; set; }
            /// <summary>
            /// 到期时间
            /// </summary>
            public string expire_date { get; set; }
            /// <summary>
            /// 过期天数
            /// </summary>
            public string over_days { get; set; }
            /// <summary>
            /// 会员权益说明
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 是否默认
            /// </summary>
            public int is_defualt { get; set; }


        }




    }
}