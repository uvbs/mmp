using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model.Weixin;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 微信卡券 Edit By FJW
    /// </summary>
    public class BLLWeixinCard : BLL
    {
        /// <summary>
        /// 请求
        /// </summary>
        ZentCloud.Common.HttpInterFace request = new Common.HttpInterFace();
        /// <summary>
        /// 微信
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        ///创建卡券
        /// </summary>
        /// <param name="model">卡券实体</param>
        /// <param name="cardId">卡券Id</param>
        /// <returns></returns>
        public bool Create(WeixinCard model, out string cardId)
        {
            //接口文档地址 https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1451025056&token=&lang=zh_CN
            string postUrl = string.Format(" https://api.weixin.qq.com/card/create?access_token={0}", bllWeixin.GetAccessToken());
            cardId = "";
            var postData = new
            {

                card = new
                {
                    card_type = model.CardType,
                    general_coupon = new//卡券信息
                    {

                        base_info = new//基本信息
                        {
                            logo_url = model.LogoUrl,
                            code_type = model.CodeType,
                            brand_name = model.BrandName,
                            title = model.Title,
                            color = model.Color,
                            notice = model.Notice,
                            service_phone = model.ServicePhone,
                            description = model.Description,
                            date_info = new
                            {
                                type = model.Type,
                                begin_timestamp = model.BeginTimeStamp,
                                end_timestamp = model.EndTimeStamp,
                                fixed_term = model.FixedTerm,
                                fixed_begin_term = model.FixedBeginTerm,
                            },
                            sku = new
                            {
                                quantity = model.Quantity
                            },
                            use_limit = model.UseLimit,
                            get_limit = model.GetLimit,
                            use_custom_code = model.UseCustomCode,
                            bind_openid = model.BindOpenId,
                            can_share = model.CanShare,
                            can_give_friend = model.CanGiveFriend,
                            center_title = model.CenterTitle,
                            center_sub_title = model.CenterSubTitle,
                            center_url = model.CenterUrl,
                            custom_url_name = model.CustomUrlName,
                            custom_url = model.CustomUrl,
                            custom_url_sub_title = model.CustomUrlSubTitle,
                            promotion_url_name = model.PromotionUrlName,
                            promotion_url = model.PromotionUrl,
                            source = model.Source,
                            location_id_list = model.LocationIdList

                        },
                        advanced_info = new {
                            temp_abstract=new {
                                temp_abstract = "",
                                icon_url_list = new string[] { model.Image }
                            }
                        
                        },//高级信息
                        default_detail = model.DefaultDetail

                    }

                }

            };
            string postDataJson = ZentCloud.Common.JSONHelper.ObjectToJson(postData);
            postDataJson = postDataJson.Replace("temp_abstract", "abstract");
            string result = request.PostWebRequest(postDataJson, postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString() == "0")
            {
                cardId = jToken["card_id"].ToString();
                return true;
            }
            return false;

        }



        /// <summary>
        /// 发放卡券(群发接口)
        /// </summary>
        /// <param name="cardId">卡券Id</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public bool SendByMass(string cardId, string openId)
        {
            //接口文档地址 https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1451025056&token=&lang=zh_CN
            string postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}", bllWeixin.GetAccessToken());
            var postData = new
            {
                touser = new string[] { openId, openId },
                wxcard = new { card_id = cardId },
                msgtype = "wxcard"

            };
            string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString() == "0")
            {

                return true;
            }
            return false;


        }

        /// <summary>
        /// 发放卡券(客服接口)
        /// </summary>
        /// <param name="cardId">卡券Id</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public bool SendByKefu(string cardId, string openId)
        {
            //接口文档地址 https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1451025056&token=&lang=zh_CN
            string postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", bllWeixin.GetAccessToken());
            var postData = new
            {
                touser = openId,
                wxcard = new { card_id = cardId },
                msgtype = "wxcard"

            };
            string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString() == "0")
            {

                return true;
            }
            return false;


        }

        /// <summary>
        /// 核销卡券
        /// </summary>
        /// <param name="code">卡券code</param>
        /// <returns></returns>
        public bool Consume(string code)
        {
            //接口文档地址 https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1451025056&token=&lang=zh_CN
            string postUrl = string.Format("https://api.weixin.qq.com/card/code/consume?access_token={0}", bllWeixin.GetAccessToken());
            var postData = new
            {
                code = code


            };
            string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString() == "0")
            {

                return true;
            }
            return false;


        }

        /// <summary>
        /// 生成卡券二维码
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public string CreateQrCode(string cardId)
        {

            string qrCodeUrl = "";
            //接口文档地址 https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1451025062&token=&lang=zh_CN
            string postUrl = string.Format("https://api.weixin.qq.com/card/qrcode/create?access_token={0}", bllWeixin.GetAccessToken());
            var postData = new
            {
                action_name = "QR_CARD",
                action_info = new
                {
                    card = new
                    {
                        card_id = cardId

                    }


                }


            };
            string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString() == "0")
            {
                return jToken["show_qrcode_url"].ToString();
            }
            return qrCodeUrl;

        }

        /// <summary>
        ///创建门店
        /// </summary>
        /// <param name="model">门店实体</param>
        /// <param name="poiId">门店Id</param>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public bool CreateStore(WeixinStore model, out string poiId,out string msg)
        {
            //接口文档地址 https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141217&token=&lang=zh_CN
            string postUrl = string.Format(" http://api.weixin.qq.com/cgi-bin/poi/addpoi?access_token={0}", bllWeixin.GetAccessToken());
            poiId = "";
            msg = "";
            var postData = new
            {

                business = new
                {

                    base_info = new
                    {
                        //sid=model.sid,
                        business_name = model.business_name,
                        branch_name = model.branch_name,
                        province = model.province,
                        city = model.city,
                        district = model.district,
                        address = model.address,
                        telephone = model.telephone,
                        categories = model.categories,
                        offset_type = model.offset_type,
                        longitude = model.longitude,
                        latitude = model.latitude
                        //photo_list = model.photo_list,
                        //recommend = model.recommend,
                        //special = model.special,
                        //introduction = model.introduction,
                        //open_time = model.open_time,
                        //avg_price = model.avg_price


                    }
                }

            };

            var postD=ZentCloud.Common.JSONHelper.ObjectToJson(postData);
            string result = request.PostWebRequest(postD, postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString() == "0")
            {
                poiId = jToken["poi_id"].ToString();
                return true;
            }
            else
            {
                msg = jToken["errmsg"].ToString();
                return false;
            }
           

        }

        /// <summary>
        ///更新门店
        /// </summary>
        /// <param name="model">门店实体</param>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public bool UpdateStore(WeixinStore model, out string msg)
        {
            //接口文档地址 https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141217&token=&lang=zh_CN
            string postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/poi/updatepoi?access_token={0}", bllWeixin.GetAccessToken());
            msg = "";
            var postData = new
            {

                business = new
                {

                    base_info = new
                    {
                        sid=model.sid,
                        poi_id =model.poi_id,
                        business_name = model.business_name,
                        branch_name = model.branch_name,
                        province = model.province,
                        city = model.city,
                        district = model.district,
                        address = model.address,
                        telephone = model.telephone,
                        categories = model.categories,
                        offset_type = model.offset_type,
                        longitude = model.longitude,
                        latitude = model.latitude,
                        photo_list = model.photo_list,
                        recommend = model.recommend,
                        special = model.special,
                        introduction = model.introduction,
                        open_time = model.open_time,
                        avg_price = model.avg_price


                    }
                }

            };

            var postD = ZentCloud.Common.JSONHelper.ObjectToJson(postData);
            string result = request.PostWebRequest(postD, postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString() == "0")
            {
               
                return true;
            }
            else
            {
                msg = jToken["errmsg"].ToString();
                return false;
            }


        }

        /// <summary>
        /// 查询门店列表
        /// </summary>
        /// <returns></returns>
        public string GetWXCategory()
        {

            string postUrl = string.Format("http://api.weixin.qq.com/cgi-bin/poi/getwxcategory?access_token={0}", bllWeixin.GetAccessToken());
            string result = request.PostWebRequest("", postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            return jToken["category_list"].ToString();


        }

        /// <summary>
        /// 查询微信门店
        /// </summary>
        /// <param name="poiId"></param>
        /// <returns></returns>
        public JToken GetStoreById(string poiId)
        {
            string postUrl = string.Format("http://api.weixin.qq.com/cgi-bin/poi/getpoi?access_token={0}", bllWeixin.GetAccessToken());
            var postData = new 
            {
                poi_id=poiId

            };
            string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            return jToken;
        
        
        }

        /// <summary>
        /// 查询微信门店
        /// </summary>
        /// <param name="begin">开始位置，0 即为从第一条开始查询</param>
        /// <param name="limit">返回数据条数，最大允许50，默认为20</param>
        /// <returns></returns>
        public JToken GetStoreList(int begin=0, int limit=20)
        {
            string postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token={0}", bllWeixin.GetAccessToken());
            var postData = new
            {
                begin = begin,
                limit = limit

            };
            string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            return jToken;


        }

        /// <summary>
        /// 删除门店
        /// </summary>
        /// <param name="poiId">微信门店id</param>
        /// <returns></returns>
        public bool DeleteStore(string poiId)
        {
            string postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/poi/delpoi?access_token={0}", bllWeixin.GetAccessToken());
            var postData = new
            {
                poi_id = poiId

            };
            string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), postUrl, Encoding.UTF8);
            JToken jToken = JToken.Parse(result);
            if (jToken["errcode"].ToString()=="0")
            {
                return true;
            }
            return false;


        }


    }
}
