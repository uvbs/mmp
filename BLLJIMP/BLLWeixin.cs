using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ZentCloud.BLLJIMP.Model.Weixin;
using System.IO;
using System.Web;
using System.Data;
using ZentCloud.BLLJIMP.Model;
using System.Net;
using ZentCloud.Common;
using System.Web.UI;
using ZentCloud.BLLJIMP.ModelGen.Weixin;
using ZCJson.Linq;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;

namespace ZentCloud.BLLJIMP
{

    /// <summary>
    /// 接收到的消息类型
    /// </summary>
    public enum WeixinRequestMsgType
    {
        /// <summary>
        /// 文字
        /// </summary>
        Text,
        /// <summary>
        /// 位置
        /// </summary>
        Location,
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 事件
        /// </summary>
        Event,
        /// <summary>
        /// 语音
        /// </summary>
        Voice,
        /// <summary>
        /// 视频
        /// </summary>
        Video

    }
    /// <summary>
    /// 回复消息类型
    /// </summary>
    public enum WeixinResponseMsgType
    {
        /// <summary>
        /// 文字
        /// </summary>
        Text,
        /// <summary>
        /// 图文
        /// </summary>
        News,
        /// <summary>
        /// 事件
        /// </summary>
        Event,
        /// <summary>
        /// 语音
        /// </summary>
        Voice,
        /// <summary>
        /// 图片
        /// </summary>
        Image
    }

    ///// <summary>
    ///// 微信会员流程步骤信息
    ///// </summary>
    //public class WeixinMemberFlowStepIDInfo
    //{
    //    /// <summary>
    //    /// 流程ID
    //    /// </summary>
    //    public int FlowID { get; set; }
    //    /// <summary>
    //    /// 步骤ID
    //    /// </summary>
    //    public int StepID { get; set; }

    //    /// <summary>
    //    /// 是否在验证码阶段:1是0否
    //    /// </summary>
    //    public int IsInVerifyCode { get; set; }
    //}


    /// <summary>
    /// 响应创建
    /// </summary>
    public abstract class CreateActionResultStrategy
    {
        abstract public ResponseMessageBase CreateActionResult(IRequestMessageBase requestMessage, string userID, string sendMsgUserId = "", UserInfo tmpUser = null);
    }

    /// <summary>
    /// 构建关键字回复结果
    /// </summary>
    public class CreateActionResultJubitForKeyword : CreateActionResultStrategy
    {


        /// <summary>
        /// 被动回复
        /// </summary>
        /// <param name="requestMessage">接收到的信息</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="sendMsgUserId">发送用户UserId</param>
        /// <returns></returns>
        public override ResponseMessageBase CreateActionResult(IRequestMessageBase requestMessage, string websiteOwner, string sendMsgUserId = "", UserInfo tmpUser = null)
        {
            BLLWebsiteDomainInfo bllWebsiteDomain = new BLLWebsiteDomainInfo();
            BLLWeixin bllWeixin = new BLLWeixin(websiteOwner);
            BLLJuActivity bllActivity = new BLLJuActivity();
            BLLUser bllUser = new BLLUser();
            BLLDistribution bllDis = new BLLDistribution();
            BLLMall bllMall = new BLLMall();
            BLLWeixinCard bllWeixinCard = new BLLWeixinCard();
            BLLCardCoupon bllCardCoupon = new BLLCardCoupon();

            //Model.UserInfo userInfo = userBll.GetUserInfo(websiteOwner);//站点所有者信息
            //Model.WeixinMemberInfo memberInfo = bllWeixin.GetMemberInfo(websiteOwner, requestMessage.FromUserName);
            //Model.WebsiteInfo currWebsiteInfoModel = weixinBll.GetWebsiteInfoModelFromDataBase();
            Model.WebsiteInfo websiteInfo = bllWeixin.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", websiteOwner));//当前站点信息
            Model.UserInfo currWebsiteOwnerUserInfo = bllUser.GetUserInfo(websiteOwner, websiteOwner);
            UserInfo currSendMsgUser = null;
            var strongRequestMessage = requestMessage as RequestMessageText;
            Model.WeixinReplyRuleInfo replyModel = new Model.WeixinReplyRuleInfo();
            string mediaId = "";//接收到的媒体Id
            // bool isDistributionQrCodeSucess = false;//是否为分销推广推送并设置推荐人成功

            if (tmpUser != null)
            {
                currSendMsgUser = tmpUser;
            }
            else
            {
                currSendMsgUser = bllUser.GetUserInfo(sendMsgUserId, websiteOwner);
                //处理消息前处理下微信个人信息
                try
                {
                    bllWeixin.ToBLLWeixinLog("处理消息前处理下微信个人信息，currSendMsgUser.WXNickname：" + currSendMsgUser.WXNickname);
                    if (string.IsNullOrWhiteSpace(currSendMsgUser.WXNickname))
                    {
                        var accessToken = bllWeixin.GetAccessToken(websiteOwner);
                        bllWeixin.ToBLLWeixinLog("处理消息前处理下微信个人信息，accessToken：" + accessToken);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            var wxUserInfo = bllWeixin.GetWeixinUserInfo(accessToken, currSendMsgUser.WXOpenId);
                            bllWeixin.ToBLLWeixinLog("处理消息前处理下微信个人信息，wxUserInfo：" + JsonConvert.SerializeObject(wxUserInfo));
                            if (wxUserInfo != null)
                            {
                                currSendMsgUser.WXHeadimgurl = wxUserInfo.headimgurl;
                                currSendMsgUser.WXNickname = string.IsNullOrWhiteSpace(wxUserInfo.nickname) ? "" : wxUserInfo.nickname.Replace("'", "");
                                currSendMsgUser.WXProvince = wxUserInfo.province;
                                currSendMsgUser.WXCity = wxUserInfo.city;

                                bllUser.Update(new UserInfo(), string.Format(" WXHeadimgurl='{0}',WXNickname='{1}',WXProvince='{2}',WXCity='{3}' ",
                                        currSendMsgUser.WXHeadimgurl,
                                        currSendMsgUser.WXNickname,
                                        currSendMsgUser.WXProvince,
                                        currSendMsgUser.WXCity
                                    ), string.Format(" UserId='{0}' ", currSendMsgUser.UserID));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    bllWeixin.ToBLLWeixinLog("处理消息前处理下微信个人信息异常，ex：" + ex.Message);
                }
            }

            switch (requestMessage.MsgType)
            {
                case WeixinRequestMsgType.Text:
                    break;
                case WeixinRequestMsgType.Location:
                    break;
                case WeixinRequestMsgType.Image:
                    var imageMsg = requestMessage as RequestMessageImage;
                    strongRequestMessage = new RequestMessageText();
                    strongRequestMessage.CreateTime = imageMsg.CreateTime;
                    strongRequestMessage.FromUserName = imageMsg.FromUserName;
                    strongRequestMessage.MsgId = imageMsg.MsgId;
                    strongRequestMessage.MsgType = WeixinRequestMsgType.Text;
                    strongRequestMessage.ToUserName = imageMsg.ToUserName;
                    strongRequestMessage.Content = imageMsg.PicUrl;
                    mediaId = imageMsg.MediaId;
                    WXFileReceive fileReceive = new WXFileReceive();
                    fileReceive.FileType = "image";
                    fileReceive.WeixinOpenID = currSendMsgUser.WXOpenId;
                    fileReceive.WebsiteOwner = websiteOwner;
                    fileReceive.MediaID = mediaId;
                    string accessToken = bllWeixin.GetAccessToken(websiteOwner);
                    string fileUrl = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accessToken, mediaId);
                    fileReceive.FilePath = bllWeixin.DownLoadRemoteImage(fileUrl);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        bllWeixin.Add(fileReceive);
                    }
                    else
                    {

                    }
                    break;
                case WeixinRequestMsgType.Event:
                    var eventMsg = requestMessage as RequestMessageEvent;
                    strongRequestMessage = new RequestMessageText();
                    strongRequestMessage.CreateTime = eventMsg.CreateTime;
                    strongRequestMessage.FromUserName = eventMsg.FromUserName;
                    strongRequestMessage.MsgId = eventMsg.MsgId;
                    strongRequestMessage.MsgType = WeixinRequestMsgType.Event;
                    strongRequestMessage.ToUserName = eventMsg.ToUserName;
                    strongRequestMessage.Content = eventMsg.EventKey;

                    #region 第一次领取卡券
                    if (!string.IsNullOrEmpty(eventMsg.UserCardCode) && (eventMsg.Event == "user_get_card") && (eventMsg.IsGiveByFriend == "0"))//领取卡券
                    {

                        CardCoupons cardCoupon = bllWeixin.Get<CardCoupons>(string.Format("WeixinCardId='{0}'", eventMsg.CardId));
                        if (cardCoupon != null)
                        {
                            
                            if (bllWeixin.GetCount<MyCardCoupons>(string.Format("CardId='{0}' And UserId='{1}'", cardCoupon.CardId, currSendMsgUser.UserID)) == 0)//系统未接收卡券
                            {
                                cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon);

                                //
                                if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
                                {
                                    replyModel.ReplyType = "text";
                                    replyModel.ReplyContent = "卡券已经过期";
                                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                                }
                                if (cardCoupon.MaxCount > 0)
                                {
                                    if (bllCardCoupon.GetCardCouponSendCount(cardCoupon.CardId) >= cardCoupon.MaxCount)
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "卡券已经领完";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                    }
                                }

                                if (cardCoupon.GetLimitType != null)
                                {
                                    if (cardCoupon.GetLimitType == "1" && !bllUser.IsDistributionMember(currSendMsgUser, true))
                                    {


                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "只有分销员才能领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                    }
                                    if (cardCoupon.GetLimitType == "2" && bllUser.IsDistributionMember(currSendMsgUser))
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "该券仅新用户（无购买历史）可以领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                    }
                                }
                                if (!string.IsNullOrEmpty(cardCoupon.BindChannelUserId))
                                {

                                    if (string.IsNullOrEmpty(currSendMsgUser.DistributionOwner))
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "只有指定渠道才能领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                    }

                                    string channelUserId = bllDis.GetUserChannel(currSendMsgUser);

                                    if (cardCoupon.BindChannelUserId != channelUserId)
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "只有指定渠道才能领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);


                                    }



                                }
                                MyCardCoupons model = new MyCardCoupons();
                                model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), bllMall.GetGUID(BLLJIMP.TransacType.CommAdd));
                                model.CardCouponType = cardCoupon.CardCouponType;
                                model.CardId = cardCoupon.CardId;
                                model.InsertDate = DateTime.Now;
                                model.UserId = currSendMsgUser.UserID;
                                model.WebSiteOwner = websiteOwner;
                                model.WeixinHexiaoCode = eventMsg.UserCardCode;

                                #region 翼码优惠券接收
                                Open.HongWareSDK.Client client = new Open.HongWareSDK.Client(websiteOwner);
                                Open.HongWareSDK.Entity.YimaGetCard yimaCard = new Open.HongWareSDK.Entity.YimaGetCard();
                                string yiMaCardCode = "";
                                string msg = "";
                                if (client.GetCard(yimaCard, out msg, out yiMaCardCode))
                                {
                                    model.YimaCardCode = yiMaCardCode;

                                }
                                else
                                {
                                    replyModel.ReplyType = "text";
                                    replyModel.ReplyContent = "领取翼码优惠券失败" + msg;
                                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                } 
                                #endregion
 

                                if (!bllCardCoupon.Add(model))
                                {
                                    replyModel.ReplyType = "text";
                                    replyModel.ReplyContent = "领取优惠券失败";
                                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                }


                            }
                            else
                            {
                                bllWeixin.Update(new MyCardCoupons(), string.Format("WeixinHexiaoCode='{0}'", eventMsg.UserCardCode), string.Format("CardId='{0}' And UserId='{1}'", cardCoupon.CardId, currSendMsgUser.UserID));

                            }

                        }
                    }
                    #endregion

                    #region 赠送卡券给朋友后
                    if (eventMsg.Event == "user_gifting_card")
                    {

                        UserInfo friendUser = bllUser.GetUserInfoByOpenId(eventMsg.FriendUserName, websiteOwner);
                        if (friendUser == null)
                        {
                            //注册用户
                            friendUser = new ZentCloud.BLLJIMP.Model.UserInfo();
                            friendUser.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());
                            friendUser.Password = ZentCloud.Common.Rand.Str_char(12);
                            friendUser.UserType = 2;
                            friendUser.WebsiteOwner = websiteOwner; 
                            friendUser.Regtime = DateTime.Now;
                            friendUser.WXOpenId = eventMsg.FriendUserName;
                            friendUser.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                            friendUser.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                            friendUser.LastLoginDate = DateTime.Now;
                            friendUser.LoginTotalCount = 1;
                            bllWeixin.Add(friendUser);

                        }

                        //原有的卡券失效
                        CardCoupons cardCoupon = bllWeixin.Get<CardCoupons>(string.Format("WeixinCardId='{0}'", eventMsg.CardId));
                        if (cardCoupon != null)
                        {
                          
                            var myCardCoupon = bllWeixin.Get<MyCardCoupons>(string.Format(string.Format("CardId='{0}' And UserId='{1}'", cardCoupon.CardId, currSendMsgUser.UserID)));
                            myCardCoupon.Status = 2;
                            myCardCoupon.FromUserId = currSendMsgUser.UserID;
                            myCardCoupon.ToUserId = friendUser.UserID;
                            myCardCoupon.ToOpenId = friendUser.WXOpenId;
                            bllCardCoupon.Update(myCardCoupon);


                        }



                    }
                    #endregion

                    #region 转赠的卡券被朋友接收后
                    if (!string.IsNullOrEmpty(eventMsg.UserCardCode) && (eventMsg.Event == "user_get_card") && (eventMsg.IsGiveByFriend == "1"))
                    {

                        CardCoupons cardCoupon = bllWeixin.Get<CardCoupons>(string.Format("WeixinCardId='{0}'", eventMsg.CardId));
                        if (cardCoupon != null)
                        {
                            if (bllWeixin.GetCount<MyCardCoupons>(string.Format("CardId='{0}' And UserId='{1}'", cardCoupon.CardId, currSendMsgUser.UserID)) == 0)//系统未接收卡券
                            {
                                cardCoupon = bllCardCoupon.ConvertExpireTime(cardCoupon);

                                //
                                if (DateTime.Now > (DateTime)(cardCoupon.ValidTo))
                                {
                                    replyModel.ReplyType = "text";
                                    replyModel.ReplyContent = "卡券已经过期";
                                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                                }
                                //if (cardCoupon.MaxCount > 0)
                                //{
                                //    if (bllCardCoupon.GetCardCouponSendCount(cardCoupon.CardId) >= cardCoupon.MaxCount)
                                //    {

                                //        replyModel.ReplyType = "text";
                                //        replyModel.ReplyContent = "卡券已经领完";
                                //        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                //    }
                                //}

                                if (cardCoupon.GetLimitType != null)
                                {
                                    if (cardCoupon.GetLimitType == "1" && !bllUser.IsDistributionMember(currSendMsgUser, true))
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "只有分销员才能领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                    }
                                    if (cardCoupon.GetLimitType == "2" && bllUser.IsDistributionMember(currSendMsgUser))
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "该券仅新用户（无购买历史）可以领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                    }
                                }
                                if (!string.IsNullOrEmpty(cardCoupon.BindChannelUserId))
                                {

                                    if (string.IsNullOrEmpty(currSendMsgUser.DistributionOwner))
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "只有指定渠道才能领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                    }

                                    string channelUserId = bllDis.GetUserChannel(currSendMsgUser);

                                    if (cardCoupon.BindChannelUserId != channelUserId)
                                    {

                                        replyModel.ReplyType = "text";
                                        replyModel.ReplyContent = "只有指定渠道才能领取";
                                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);


                                    }



                                }
                                MyCardCoupons model = new MyCardCoupons();
                                model.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), bllMall.GetGUID(BLLJIMP.TransacType.CommAdd));
                                model.CardCouponType = cardCoupon.CardCouponType;
                                model.CardId = cardCoupon.CardId;
                                model.InsertDate = DateTime.Now;
                                model.UserId = currSendMsgUser.UserID;
                                model.WebSiteOwner = websiteOwner;
                                model.WeixinHexiaoCode = eventMsg.UserCardCode;
                                if (!bllCardCoupon.Add(model))
                                {
                                    replyModel.ReplyType = "text";
                                    replyModel.ReplyContent = "领取优惠券失败";
                                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                                }


                            }
                            else
                            {
                                bllWeixin.Update(new MyCardCoupons(), string.Format("WeixinHexiaoCode='{0}'", eventMsg.UserCardCode), string.Format("CardId='{0}' And UserId='{1}'", cardCoupon.CardId, currSendMsgUser.UserID));

                            }

                        }




                    }
                    #endregion

                    #region 核销卡券
                    if (eventMsg.Event == "user_consume_card")
                    {

                        bllWeixin.Update(new MyCardCoupons(), string.Format("Status=1,UseDate=GetDate()"), string.Format("WeixinHexiaoCode='{0}'",eventMsg.UserCardCode));

                    }
                    #endregion
                    break;
                case WeixinRequestMsgType.Voice:
                    var voiceMsg = requestMessage as RequestMessageVoice;
                    strongRequestMessage = new RequestMessageText();
                    strongRequestMessage.CreateTime = voiceMsg.CreateTime;
                    strongRequestMessage.FromUserName = voiceMsg.FromUserName;
                    strongRequestMessage.MsgId = voiceMsg.MsgId;
                    strongRequestMessage.MsgType = WeixinRequestMsgType.Voice;
                    strongRequestMessage.ToUserName = voiceMsg.ToUserName;
                    strongRequestMessage.Content = voiceMsg.Recognition;
                    mediaId = voiceMsg.MediaId;
                    break;
                case WeixinRequestMsgType.Video:
                    break;
                default:
                    break;
            }

            //if (requestMessage.MsgType == WeixinRequestMsgType.Event)
            //{
            //    var tmpMsg = requestMessage as RequestMessageEvent;
            //    strongRequestMessage = new RequestMessageText();
            //    strongRequestMessage.CreateTime = tmpMsg.CreateTime;
            //    strongRequestMessage.FromUserName = tmpMsg.FromUserName;
            //    strongRequestMessage.MsgId = tmpMsg.MsgId;
            //    strongRequestMessage.MsgType = WeixinRequestMsgType.Event;
            //    strongRequestMessage.ToUserName = tmpMsg.ToUserName;
            //    strongRequestMessage.Content = tmpMsg.EventKey;



            //}
            //if (requestMessage.MsgType == WeixinRequestMsgType.Voice)
            //{
            //    var tmpMsg = requestMessage as RequestMessageVoice;
            //    strongRequestMessage = new RequestMessageText();
            //    strongRequestMessage.CreateTime = tmpMsg.CreateTime;
            //    strongRequestMessage.FromUserName = tmpMsg.FromUserName;
            //    strongRequestMessage.MsgId = tmpMsg.MsgId;
            //    strongRequestMessage.MsgType = WeixinRequestMsgType.Voice;
            //    strongRequestMessage.ToUserName = tmpMsg.ToUserName;
            //    strongRequestMessage.Content = tmpMsg.Recognition;
            //    mediaId = tmpMsg.MediaId;
            //}
            //if (requestMessage.MsgType == WeixinRequestMsgType.Image)
            //{

            //    var tmpMsg = requestMessage as RequestMessageImage;
            //    strongRequestMessage = new RequestMessageText();
            //    strongRequestMessage.CreateTime = tmpMsg.CreateTime;
            //    strongRequestMessage.FromUserName = tmpMsg.FromUserName;
            //    strongRequestMessage.MsgId = tmpMsg.MsgId;
            //    strongRequestMessage.MsgType = WeixinRequestMsgType.Text;
            //    strongRequestMessage.ToUserName = tmpMsg.ToUserName;
            //    strongRequestMessage.Content = tmpMsg.PicUrl;
            //    mediaId = tmpMsg.MediaId;

            //    WXFileReceive fileReceive = new WXFileReceive();
            //    fileReceive.FileType = "image";
            //    fileReceive.WeixinOpenID = memberInfo.WeixinOpenID;
            //    fileReceive.WebsiteOwner = currWebsiteOwner.UserID;
            //    fileReceive.MediaID = mediaId;

            //    string accessToken = weixinBll.GetAccessToken();
            //    //fileReceive.FilePath=MediaOperate.DownloadImage(accesstoken, mediaId);
            //    string fileUrl = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accessToken, mediaId);
            //    fileReceive.FilePath = weixinBll.DownLoadRemoteImage(fileUrl);
            //    if (!string.IsNullOrEmpty(accessToken))
            //    {
            //        weixinBll.Add(fileReceive);
            //    }
            //    else
            //    {

            //    }



            //}

            //if ()
            //{

            //}

            //var strongRequestMessage = requestMessage as RequestMessageText;
            //ResponseMessageBase responseMessage = null;


            string pushContent = strongRequestMessage == null ? "" : strongRequestMessage.Content.Trim();

            #region 事件处理
            if (requestMessage.MsgType == WeixinRequestMsgType.Event)
            {

                var tmpReq = requestMessage as RequestMessageEvent;
                pushContent = tmpReq.EventKey;
                pushContent = pushContent.Replace("qrscene_", "");//未关注的用户会多了这个字符串 qrscene_

                //线上分销处理
                //if (currSendMsgUser != null)
                // {

                bllWeixin.ToBLLWeixinLog("websiteInfo.DistributionRelationBuildQrCode:" + websiteInfo.DistributionRelationBuildQrCode + "  ,websiteOwner :" + websiteOwner);
                string distributionOwner = "";//分销员用户名

                #region 分销
                if (websiteInfo.DistributionRelationBuildQrCode == 1)//通过二维码建立关系
                {

                    bllWeixin.ToBLLWeixinLog("通过二维码建立关系 start ,websiteOwner :" + websiteOwner);


                    #region 移动分销
                    string disbCodeKey = "DistributionCode_";

                    if (pushContent.IndexOf(disbCodeKey) > -1)
                    {
                        bllWeixin.ToLog("进入通过二维码建立关系 disbCodeKey" + pushContent + " 进入者：" + currSendMsgUser.UserID);

                        //获取推荐人id 并设置用户的线上分销上级id
                        distributionOwner = pushContent.Substring(pushContent.IndexOf(disbCodeKey) + disbCodeKey.Length);

                        if (tmpUser != null)
                        {
                            //判断：如果是临时用户，则保存openid临时关系，并返回绑定手机页面提醒注册
                            bllWeixin.ToLog("是临时用户，保存openid临时关系，并返回绑定手机页面提醒注册");
                            var userDistributionOwnerTempInfo = bllDis.GetUserDistributionOwnerTempInfo(tmpUser.WXOpenId, websiteOwner);

                            bllWeixin.ToLog("查询到的 userDistributionOwnerTempInfo： " + JsonConvert.SerializeObject(userDistributionOwnerTempInfo));

                            if (userDistributionOwnerTempInfo == null)
                            {
                                userDistributionOwnerTempInfo = new UserDistributionOwnerTempInfo()
                                {
                                    DistributionOwner = distributionOwner,
                                    OpenId = tmpUser.WXOpenId,
                                    InsertTime = DateTime.Now,
                                    Status = 0,
                                    WebsiteOwner = websiteOwner,
                                    FromSource = "qrcode"
                                };

                                bllWeixin.Add(userDistributionOwnerTempInfo);
                                bllWeixin.ToLog("保存临时关系，返回绑定手机页面提醒注册");
                               // var domain = new BLLWebsiteDomainInfo().GetWebsiteDoMain(tmpUser.WebsiteOwner);

                            }

                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = string.Format("<a href=\"http://{0}/customize/shop/?v=1.0&ngroute=/bindPhone/1#/bindPhone/1\">点击注册</a>", bllWebsiteDomain.GetWebsiteDoMain(websiteOwner));
                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                        }
                        else
                        {

                            #region 更新上级处理

                            var preUser = bllUser.GetUserInfo(distributionOwner,bllUser.WebsiteOwner);

                            //仅扫码有用
                            if (string.IsNullOrEmpty(currSendMsgUser.TagName))
                            {
                                currSendMsgUser.TagName = preUser.AutoID.ToString();
                            }
                            else
                            {
                                if (!currSendMsgUser.TagName.Contains(preUser.AutoID.ToString()))
                                {
                                    currSendMsgUser.TagName = currSendMsgUser.TagName.TrimEnd(',');
                                    currSendMsgUser.TagName += "," + preUser.AutoID.ToString();
                                }

                            }

                            bllUser.Update(currSendMsgUser);

                            bllDis.SetUserDistributionOwner(currSendMsgUser.UserID, distributionOwner, currSendMsgUser.WebsiteOwner);
                            
                            #endregion

                        }

                    }
                    #endregion

                    #region 微吸粉
                    if (pushContent.IndexOf("ArticleId_") > -1 && (tmpReq.Event.Equals("subscribe")))
                    {
                        if (string.IsNullOrWhiteSpace(currSendMsgUser.DistributionOwner))
                        {
                            string articleId = pushContent.Split('_')[1];
                            distributionOwner = pushContent.Split('_')[2];

                            if (distributionOwner != currSendMsgUser.UserID)
                            {
                                UserInfo preUser = bllUser.GetUserInfo(distributionOwner, currSendMsgUser.WebsiteOwner);
                                if (string.IsNullOrEmpty(currSendMsgUser.TagName))
                                {
                                    currSendMsgUser.TagName = preUser.AutoID.ToString();
                                }
                                else
                                {
                                    currSendMsgUser.TagName = currSendMsgUser.TagName.TrimEnd(',');
                                    currSendMsgUser.TagName += "," + preUser.AutoID.ToString();
                                }
                                bllUser.Update(currSendMsgUser);
                                if (bllUser.IsDistributionMember(preUser))
                                {

                                    var setUserDistributionOwner = 
                                        bllDis.SetUserDistributionOwner(currSendMsgUser.UserID, preUser.UserID, currSendMsgUser.WebsiteOwner);

                                    if (setUserDistributionOwner)
                                    {
                                        currSendMsgUser.DistributionOwner = preUser.UserID;
                                        currSendMsgUser.Channel = preUser.Channel;
                                    }
                                    
                                    currSendMsgUser.ArticleID = articleId;
                                    if (string.IsNullOrEmpty(currSendMsgUser.TagName))
                                    {
                                        currSendMsgUser.TagName = preUser.AutoID.ToString();
                                    }
                                    else
                                    {
                                        if (!currSendMsgUser.TagName.Contains(preUser.AutoID.ToString()))
                                        {
                                            currSendMsgUser.TagName = currSendMsgUser.TagName.TrimEnd(',');
                                            currSendMsgUser.TagName += "," + preUser.AutoID.ToString();
                                        }
                                    }
                                    bllUser.Update(currSendMsgUser);
                                  
                                    bllUser.AddUserScoreDetail(preUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.RecommendWeixinSubscribeAddScore), websiteOwner, null, null);

                                    #region 更新粉丝人数
                                    JuActivityInfo juActivityModel = bllActivity.GetJuActivity(int.Parse(articleId));
                                    if (juActivityModel != null)
                                    {
                                        //int powdercCount = bllActivity.GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND  DistributionOwner='{2}'", websiteOwner, articleId, preUser.UserID));
                                        bllActivity.Update(new MonitorLinkInfo(), string.Format(" PowderCount+={0} ", 1), string.Format(" MonitorPlanID={0} AND LinkName='{1}'", juActivityModel.MonitorPlanID, preUser.UserID));//更新微吸粉人数


                                    }
                                    #endregion

                                  

                                }

                            }
                        }
                        //replyModel.ReplyType = "text";
                        //replyModel.ReplyContent = "欢迎关注!";
                        //return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                    }

                    #endregion

                    //#region 渠道分销
                    //string channelCodeKey = "ChannelCode_";//渠道关键字
                    //if (pushContent.IndexOf(channelCodeKey) > -1 && (string.IsNullOrEmpty(currSendMsgUser.Channel)))
                    //{

                    //    bllWeixin.ToLog("准备更新上级及渠道");
                    //    distributionOwner = pushContent.Replace(channelCodeKey, null);
                    //    var preUser=bllUser.GetUserInfo(distributionOwner,websiteOwner);
                    //    bllWeixin.ToLog("渠道:" + distributionOwner);
                    //    if (string.IsNullOrEmpty(currSendMsgUser.DistributionOwner))
                    //    {
                    //        currSendMsgUser.DistributionOwner = distributionOwner;
                    //    }
                    //    currSendMsgUser.Channel = distributionOwner;
                    //    if (bllUser.Update(currSendMsgUser))
                    //    {

                    //        bllWeixin.ToLog("设置渠道成功");
                    //        bllDis.UpdateUpUserCount(currSendMsgUser);
                    //        bllWeixin.ToLog("更新上级后其他处理完成");
                    //        //replyModel.ReplyType = "text";
                    //        //replyModel.ReplyContent = string.Format("您已经成为渠道{0}的会员", preUser.TrueName);
                    //        //return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                    //    }
                    //    else
                    //    {
                    //        bllWeixin.ToLog("更新上级失败");
                    //    }




                    //}
                    //#endregion

                }
                #endregion

                #region 抽奖相关
                if (pushContent.StartsWith("LotteryCode"))
                {
                    // LotteryCode_抽奖id_分销员用户名
                    string[] arry = pushContent.Split('_');
                    string lotteryId = pushContent.Split('_')[1];//抽奖Id


                    #region 分销
                    if (arry.Length >= 3)//有分销用户
                    {
                        var autoId = arry[2];
                        distributionOwner = bllUser.GetUserInfoByAutoID(int.Parse(autoId)).UserID;

                        if (websiteInfo.DistributionRelationBuildQrCode == 1)//通过二维码建立关系
                        {

                            //判断，如果上线是空、系统的当前用户，准备上级不是他自己也不是他下线的，则可以成为他的上线
                            
                            var preUser = bllUser.GetUserInfo(distributionOwner, websiteOwner);//推荐人
                            
                            if (string.IsNullOrEmpty(currSendMsgUser.TagName))
                            {
                                currSendMsgUser.TagName = preUser.AutoID.ToString();
                            }
                            else
                            {
                                if (!currSendMsgUser.TagName.Contains(preUser.AutoID.ToString()))
                                {
                                    currSendMsgUser.TagName = currSendMsgUser.TagName.TrimEnd(',');
                                    currSendMsgUser.TagName += "," + preUser.AutoID.ToString();
                                }
                                
                            }
                            bllUser.Update(currSendMsgUser);

                            var setUserDistributionOwnerResult = bllDis.SetUserDistributionOwner(currSendMsgUser.UserID, distributionOwner, websiteOwner);
                            
                        }


                    }
                    #endregion


                    #region 加入抽奖

                    LotteryUserInfo model = bllUser.Get<LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryId={1} AND UserId='{2}' ", websiteOwner, lotteryId, currSendMsgUser.UserID));
                    if (model == null)
                    {

                        model = new LotteryUserInfo();
                        model.WebsiteOwner = websiteOwner;
                        model.CreateDate = DateTime.Now;
                        model.WinnerDate = DateTime.Now;
                        model.IsWinning = 0;
                        model.LotteryId = Convert.ToInt32(lotteryId);
                        model.UserId = currSendMsgUser.UserID;
                        model.WXHeadimgurl = currSendMsgUser.WXHeadimgurl;
                        model.WXNickname = bllUser.GetUserDispalyName(currSendMsgUser);

                        if (!string.IsNullOrEmpty(model.WXNickname))
                        {
                            if (bllUser.Add(model))
                            {
                                int count = bllUser.GetCount<LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}", websiteOwner, lotteryId));
                                bllUser.UpdateByKey<WXLotteryV1>("LotteryID", lotteryId, "WinnerCount", count.ToString());


                                replyModel.ReplyType = "text";
                                replyModel.ReplyContent = string.Format("加入抽奖成功 ，点击<a href=\"http://{0}/App/LuckDraw/Wap/Join.aspx?lotteryId={1}\">查看中奖情况</a>", bllWebsiteDomain.GetWebsiteDoMain(websiteOwner), lotteryId);
                                return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                            }
                            else
                            {
                                replyModel.ReplyType = "text";
                                replyModel.ReplyContent = "加入抽奖失败";
                                return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                            }

                        }
                        else
                        {
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "获取不到昵称头像,不能加入抽奖";
                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                        }


                    }
                    else
                    {
                        replyModel.ReplyType = "text";
                        replyModel.ReplyContent = string.Format("加入抽奖成功 ，点击<a href=\"http://{0}/App/LuckDraw/Wap/Join.aspx?lotteryId={1}\">查看中奖情况</a>", bllWebsiteDomain.GetWebsiteDoMain(websiteOwner), lotteryId);
                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                    }

                    #endregion




                }
                #endregion

                #region 活动签到
                if (pushContent.Contains("activitysignin_"))//活动签到逻辑
                {

                    BLLJuActivity bllJuactivity = new BLLJuActivity();
                    int juActivityID = int.Parse(pushContent.Split('_')[1]);// JuActivityID 
                    //查询活动是否存在
                    var juActivityInfo = bllJuactivity.GetJuActivity(juActivityID);
                    ActivityDataInfo dataInfo = bllJuactivity.Get<ActivityDataInfo>(string.Format("ActivityID={0} And IsDelete=0 And (WeixinOpenID='{1}' Or UserId='{2}')", juActivityInfo.SignUpActivityID, currSendMsgUser.WXOpenId, currSendMsgUser.UserID));//报名数据
                    if (dataInfo == null)
                    {
                        WebsiteDomainInfo doMain = bllActivity.Get<WebsiteDomainInfo>(string.Format(" WebsiteOwner='{0}'", websiteOwner));
                        replyModel.ReplyType = "text";
                        replyModel.ReplyContent = string.Format("您还未报名参加本次活动。<a href=\"http://{0}/{1}/detail.chtml?autosignin=1\">点此报名并签到</a>", doMain.WebsiteDomain, juActivityInfo.JuActivityIDHex);
                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                    }
                    WXSignInInfo signInInfo = new WXSignInInfo();
                    signInInfo.SignInUserID = currSendMsgUser.UserID;
                    signInInfo.Name = dataInfo.Name;
                    signInInfo.Phone = dataInfo.Phone;
                    signInInfo.JuActivityID = juActivityID;
                    signInInfo.SignInOpenID = currSendMsgUser.WXOpenId;
                    signInInfo.SignInTime = DateTime.Now;

                    //判断是否已经签到过
                    if (bllJuactivity.Exists(signInInfo, new List<string>() { "SignInUserID", "JuActivityID" }))
                    {

                        replyModel.ReplyType = "text";
                        replyModel.ReplyContent = "您已经签过到了";
                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);



                    }
                    if (bllJuactivity.Add(signInInfo))
                    {
                        bllUser.AddUserScoreDetail(currSendMsgUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.SignIn), currSendMsgUser.WebsiteOwner, null, null);
                        replyModel.ReplyType = "text";
                        replyModel.ReplyContent = string.Format("签到成功!欢迎您参加{0}", juActivityInfo.ActivityName);
                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                    }




                }
                #endregion

                #region 关注
                //关注处理
                if (tmpReq.Event.Equals("subscribe"))//关注事件
                {
                    string source = "普通关注";
                    if (pushContent.IndexOf("DistributionCode_") > -1)
                    {

                        source = "分销二维码关注";
                    }
                    if (pushContent.IndexOf("LotteryCode_") > -1)
                    {

                        source = "抽奖二维码关注";
                    }
                    //if (pushContent.IndexOf("ChannelCode_") > -1)
                    //{

                    //    source = "渠道二维码关注";
                    //}
                    //pushContent = currWebsiteOwnerUserInfo.SubscribeKeyWord;
                    pushContent = "关注自动回复";
                    //if (currSendMsgUser.IsWeixinFollower != 1)
                    //{
                    currSendMsgUser.IsWeixinFollower = 1;
                    currSendMsgUser.SubscribeTime = DateTime.Now.ToString();

                    bllUser.Update(currSendMsgUser, string.Format(" IsWeixinFollower=1,SubscribeTime='{0}'", currSendMsgUser.SubscribeTime), string.Format(" UserID='{0}'", currSendMsgUser.UserID));

                    string parentUserId = "";
                    string parentShowName = "";
                    if (!string.IsNullOrEmpty(distributionOwner))
                    {
                        var preUser = bllUser.GetUserInfo(distributionOwner, websiteOwner);
                        if (preUser != null)
                        {
                            parentUserId = preUser.UserID;
                            parentShowName = bllUser.GetUserDispalyName(preUser);
                        }


                    }
                    if (bllUser.GetCount<WeixinFollowersInfo>(string.Format(" WebsiteOwner='{0}' And OpenId='{1}'", currSendMsgUser.WebsiteOwner, currSendMsgUser.WXOpenId)) == 0)
                    {

                        #region 新增粉丝
                        var flowerInfo = bllWeixin.GetWeixinUserInfo(bllWeixin.GetAccessToken(websiteOwner), currSendMsgUser.WXOpenId.ToString());//粉丝信息    
                        if (flowerInfo == null)
                        {
                            flowerInfo = new BLLWeixin.WeixinFollowerInfo();
                        }
                        BLLJIMP.Model.WeixinFollowersInfo newFollower = new Model.WeixinFollowersInfo();
                        newFollower.WebsiteOwner = websiteOwner;
                        newFollower.City = flowerInfo.city;
                        newFollower.Country = flowerInfo.country;
                        newFollower.HeadImgUrl = flowerInfo.headimgurl;
                        newFollower.Language = flowerInfo.language;
                        newFollower.NickName = flowerInfo.nickname;
                        newFollower.OpenId = currSendMsgUser.WXOpenId;
                        newFollower.Province = flowerInfo.province;
                        newFollower.Sex = flowerInfo.sex;
                        newFollower.IsWeixinFollower = 1;
                        newFollower.Source = source;
                        newFollower.Subscribe_time = currSendMsgUser.SubscribeTime;
                        newFollower.ParentUserId = parentUserId;
                        newFollower.ParentShowName = parentShowName;
                        bllWeixin.Add(newFollower);



                        #endregion
                    }
                    else
                    {

                        bllUser.Update(new WeixinFollowersInfo(), string.Format(" IsWeixinFollower=1,Subscribe_time='{0}', Source='{1}',ParentUserId='{2}',ParentShowName='{3}'", currSendMsgUser.SubscribeTime, source, parentUserId, parentShowName), string.Format(" WebsiteOwner='{0}' And OpenId='{1}'", currSendMsgUser.WebsiteOwner, currSendMsgUser.WXOpenId));

                    }
                    //}

                    bllUser.AddUserScoreDetail(currSendMsgUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.WeixinSubscribeAddScore), currSendMsgUser.WebsiteOwner, null, null);
                    Log log = new Log();
                    log.Module = "Weixin";
                    log.WebsiteOwner = websiteOwner;
                    log.InsertDate = DateTime.Now;
                    log.Action = "Subscribe";
                    log.UserID = currSendMsgUser.UserID;
                    log.Remark = "关注公众号";
                    log.WxOpenId = currSendMsgUser.WXOpenId;
                    bllUser.Add(log);
                    BLLCardCoupon bllCard = new BLLCardCoupon();
                    bllCard.SubscribeGive(currSendMsgUser);

                }
                #endregion

                #region 取消关注
                else if (tmpReq.Event.Equals("unsubscribe"))//取消关注事件
                {
                    currSendMsgUser.IsWeixinFollower = 0;
                    currSendMsgUser.UnSubscribeTime = DateTime.Now.ToString();
                    if (tmpUser != null)
                    {
                        bllUser.Update(currSendMsgUser, string.Format(" IsWeixinFollower=0,UnSubscribeTime='{0}'", currSendMsgUser.UnSubscribeTime), string.Format(" UserID='{0}'", currSendMsgUser.UserID));
                    }

                    //bllUser.Update(currSendMsgUser, " IsWeixinFollower=0", string.Format(" AutoId={0}", currSendMsgUser.AutoID));
                    bllUser.Update(new WeixinFollowersInfo(), string.Format(" IsWeixinFollower=0,UnSubscribeTime='{0}'", currSendMsgUser.SubscribeTime), string.Format(" WebsiteOwner='{0}' And OpenId='{1}'", currSendMsgUser.WebsiteOwner, currSendMsgUser.WXOpenId));

                    Log log = new Log();
                    log.Module = "Weixin";
                    log.WebsiteOwner = websiteOwner;
                    log.InsertDate = DateTime.Now;
                    log.Action = "UnSubscribe";
                    log.UserID = currSendMsgUser.UserID;
                    log.Remark = "取消关注公众号";
                    log.WxOpenId = currSendMsgUser.WXOpenId;
                    bllUser.Add(log);
                }
                #endregion



            }
            #endregion

            //if (!BLL.GetCheck()) pushContent = "";

            //if (requestMessage.MsgType == WeixinRequestMsgType.Voice)
            //{

            //    var tmpReq = requestMessage as RequestMessageVoice;
            //    pushContent = tmpReq.Recognition;

            //}

            //SystemSet systemSet = bllWeixin.Get<SystemSet>("");
            //var currentUser = bllUser.GetUserInfoByOpenId(memberInfo.WeixinOpenID);
            //if (pushContent.ToLower().StartsWith("wxmenu_") || pushContent.ToLower().StartsWith(systemSet.WXArticleKey))
            //{
            //    memberInfo.CurrAction = "访问";
            //    memberInfo.FlowStep = "";
            //    weixinBll.Update(memberInfo);
            //}

            #region 客服回复处理

            var kefuList = bllWeixin.GetList<WXKeFu>(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            if (kefuList.Count > 0 && (currWebsiteOwnerUserInfo.WeixinIsAdvancedAuthenticate.Equals(1)) && (kefuList.Where(p => p.WeiXinOpenID == currSendMsgUser.WXOpenId).Count() > 0))//当前用户是客服
            {

                //if (memberInfo.WeixinOpenID.Equals(currWebsiteOwner.WeiXinKeFuOpenId))//当前用户是客户微信号
                //{

                WXKeFuLog kefuLog = new WXKeFuLog();
                kefuLog.KeFuWeixinOpenId = currSendMsgUser.WXOpenId;
                kefuLog.SendMessage = pushContent;
                //if (currentUser != null)
                //{
                kefuLog.UserID = currSendMsgUser.UserID;
                //}
                kefuLog.InsertDate = DateTime.Now;
                bllWeixin.Add(kefuLog);
                if (pushContent.ToLower().StartsWith("k"))//处理客户回复
                {
                    //回复格式  K:编号:回复内容
                    if (pushContent.Split(':').Length >= 3)//回复格式正确
                    {
                        string[] arry = pushContent.Split(':');
                        if (!string.IsNullOrEmpty(arry[1]))//检查编号
                        {
                            int autoId = 0;
                            if (int.TryParse(arry[1], out autoId))
                            {
                                var question = bllWeixin.Get<WXKeFuQuestionReceive>(string.Format("QuestionID={0}", autoId));
                                if (question != null)
                                {
                                    string replyContent = "";
                                    if (arry.Length > 3)
                                    {
                                        replyContent = pushContent.Replace(arry[0] + ":", null).Replace(arry[1] + ":", null);
                                    }
                                    else
                                    {
                                        replyContent = arry[2];
                                    }
                                    if (!string.IsNullOrEmpty(replyContent))
                                    {
                                        var replymodel = new WXKeFuQuestionReply();
                                        replymodel.QuestionId = question.QuestionID;
                                        replymodel.ReplyContent = replyContent;
                                        replymodel.Status = "0";
                                        replymodel.InsertDate = DateTime.Now;
                                        replymodel.ReplyWeixinOpenId = currSendMsgUser.WXOpenId;
                                        replymodel.WebsiteOwner = currSendMsgUser.WebsiteOwner;
                                        if (bllWeixin.Add(replymodel))
                                        {
                                            replyModel.ReplyType = "text";
                                            replyModel.ReplyContent = "回复已提交，系统稍后会将消息回复给用户。";
                                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                                        }
                                        else
                                        {
                                            replyModel.ReplyType = "text";
                                            replyModel.ReplyContent = "提交回复失败，请重新回复。";
                                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                                        }
                                    }
                                    else
                                    {

                                        var replymodelmedia = new WXKeFuQuestionReplyTemp();
                                        replymodelmedia.QuestionId = question.QuestionID;
                                        replymodelmedia.Status = "0";//多媒体回复
                                        replymodelmedia.InsertDate = DateTime.Now;
                                        replymodelmedia.ReplyWeixinOpenId = currSendMsgUser.WXOpenId;
                                        if (bllWeixin.Add(replymodelmedia))
                                        {
                                            replyModel.ReplyType = "text";
                                            replyModel.ReplyContent = "请录入语音或图片，语音或图片将发送给用户。";
                                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                                        }
                                        else
                                        {
                                            replyModel.ReplyType = "text";
                                            replyModel.ReplyContent = "提交失败,请重新回复。";
                                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                                        }
                                        //记录要回复的ID到表中
                                        //replyModel.ReplyType = "text";
                                        //replyModel.ReplyContent = "回复内容不能为空，请按照以下格式回复 K:编号:回复内容";
                                        //return weixinBll.GetResponseMessage(replyModel, strongRequestMessage);


                                    }

                                }
                                else//编号不存在
                                {
                                    replyModel.ReplyType = "text";
                                    replyModel.ReplyContent = "编号不存在，请按照以下格式回复 K:编号:回复内容";
                                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                                }


                            }
                            else
                            {
                                replyModel.ReplyType = "text";
                                replyModel.ReplyContent = "编号不存在，请按照以下格式回复 K:编号:回复内容";
                                return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                            }


                        }
                        else
                        {
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "编号不能为空，请按照以下格式回复 K:编号:回复内容";
                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                        }

                    }
                    else
                    {
                        replyModel.ReplyType = "text";
                        replyModel.ReplyContent = "回复格式不正确，请按照以下格式回复 K:编号:回复内容";
                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                    }
                }
                if (requestMessage.MsgType.ToString().ToLower().Equals("voice") || requestMessage.MsgType.ToString().ToLower().Equals("image"))//客服回复语音或图片
                {
                    WXKeFuQuestionReplyTemp replyMedia = bllWeixin.Get<WXKeFuQuestionReplyTemp>(string.Format("Status='0' Order By AutoID ASC"));
                    if (replyMedia != null)
                    {
                        var replymodel = new WXKeFuQuestionReply();
                        replymodel.QuestionId = replyMedia.QuestionId;
                        replymodel.ReplyContent = "语音";
                        replymodel.Status = "0";
                        replymodel.InsertDate = DateTime.Now;
                        replymodel.ReplyWeixinOpenId = replyMedia.ReplyWeixinOpenId;
                        replymodel.MediaId = mediaId;
                        replymodel.MsgType = requestMessage.MsgType.ToString().ToLower();
                        replymodel.WebsiteOwner = currSendMsgUser.WebsiteOwner;
                        if (requestMessage.MsgType.ToString().ToLower().Equals("voice"))
                        {
                            replymodel.ReplyContent = "语音";
                        }
                        if (requestMessage.MsgType.ToString().ToLower().Equals("image"))
                        {
                            replymodel.ReplyContent = "图片";
                        }
                        if (bllWeixin.Add(replymodel))
                        {
                            replyMedia.Status = "1";
                            replyMedia.MsgType = requestMessage.MsgType.ToString().ToLower();
                            replyMedia.MediaId = mediaId;
                            if (bllWeixin.Update(replyMedia))
                            {
                                replyModel.ReplyType = "text";
                                if (requestMessage.MsgType.ToString().ToLower().Equals("voice"))
                                {
                                    replyModel.ReplyContent = "回复已提交，系统稍后会将该语音发送给用户。";
                                }
                                if (requestMessage.MsgType.ToString().ToLower().Equals("image"))
                                {
                                    replyModel.ReplyContent = "回复已提交，系统稍后会将该图片发送给用户。";
                                }
                                // replyModel.ReplyContent = "回复已提交，系统稍后会回复给用户。";
                                return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);


                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "提交回复失败，请重新回复。";
                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                        }
                    }
                    else
                    {

                    }

                }

                //}

            }
            #endregion

            //#region 菜单模块

            //Model.WeixinReplyRuleInfo menuModel = bllWeixin.Get<Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' and RuleType = 4 ", websiteOwner));

            //if (menuModel != null)
            //{

            //    if (pushContent.Equals(menuModel.MsgKeyword, StringComparison.OrdinalIgnoreCase))
            //    {
            //        //先清除状态
            //        //memberInfo.CurrAction = "访问";
            //        //memberInfo.FlowStep = "";
            //        //bllWeixin.Update(memberInfo);

            //        if (menuModel != null)
            //            replyModel = menuModel;
            //        else
            //        {
            //            replyModel.ReplyType = "text";
            //            replyModel.ReplyContent = "菜单未设置!";
            //        }

            //        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

            //    }
            //}

            //#endregion



            //#region 流程模块

            ///* TODO:20130331 流程处理
            // * 1.检查是否在流程内。
            // * 1a.是则进入相应流程处理。
            // * 1b.否则进入流程关键字判断。
            // * 
            // */

            //if (memberInfo.CurrAction.Equals("流程"))
            //{
            //    return weixinBll.FlowProcess(memberInfo, requestMessage);
            //}

            ///*
            // * TODO:20130331 流程关键字处理
            // * 1.获取当前公众号所有流程关键字。
            // * 2.全文判断用户回复信息。
            // * 3.[判断用户是否已操作过该流程?]。
            // * 4.匹配关键字进入流程处理。
            // * 5.查看历史记录是否是当前流程，是的话则进入历史步骤，否则从第一步开始。
            // * 
            // */

            //List<Model.WXFlowInfo> flowList = weixinBll.GetList<Model.WXFlowInfo>(string.Format(" UserID = '{0}' and FlowKeyword = '{1}' and IsEnable = 1 ", userID, pushContent));

            //if (flowList.Count > 0)
            //{

            //    //判断是否重复执行流程，是则返回提示信息
            //    if (flowList[0].MemberLimitState == 1 && memberInfo.isExitFlowIDHistory(flowList[0].FlowID))
            //    {
            //        replyModel.ReplyType = "text";
            //        replyModel.ReplyContent = flowList[0].FlowLimitMsg;

            //        return weixinBll.GetResponseMessage(replyModel, strongRequestMessage);
            //    }

            //    //更改用户流程信息，并调出流程

            //    memberInfo.CurrAction = "流程";
            //    WeixinMemberFlowStepIDInfo flowStepIDModel = new WeixinMemberFlowStepIDInfo() { FlowID = -1, StepID = -1 };

            //    try
            //    {
            //        if (!string.IsNullOrWhiteSpace(memberInfo.FlowStep))
            //            flowStepIDModel = Common.JSONHelper.JsonToModel<WeixinMemberFlowStepIDInfo>(memberInfo.FlowStep);
            //    }
            //    catch { }

            //    //进入历史记录
            //    if (flowStepIDModel.FlowID != flowList[0].FlowID)
            //    {
            //        flowStepIDModel.FlowID = flowList[0].FlowID;
            //        flowStepIDModel.StepID = 1;

            //        //进入历史记录(数据库)
            //        Model.WXMemberFlowHistory memberFlowHistory = new WXMemberFlowHistory();
            //        memberFlowHistory.FlowID = flowStepIDModel.FlowID;
            //        memberFlowHistory.WeixinMemberID = memberInfo.WeixinMemberID;
            //        memberFlowHistory.BeginDate = DateTime.Now;
            //        weixinBll.Add(memberFlowHistory);
            //    }

            //    //flowStepIDModel.FlowID = flowList[0].FlowID;
            //    //flowStepIDModel.StepID = 1;

            //    Model.WXFlowStepInfo flowStepModel = weixinBll.Get<Model.WXFlowStepInfo>(string.Format(" FlowID = {0} and StepID = {1} ", flowStepIDModel.FlowID, flowStepIDModel.StepID));

            //    if (flowStepModel == null)
            //    {
            //        flowStepIDModel.StepID = 1;
            //        flowStepModel = weixinBll.Get<Model.WXFlowStepInfo>(string.Format(" FlowID = {0} and StepID = {1} ", flowStepIDModel.FlowID, flowStepIDModel.StepID));
            //    }

            //    memberInfo.FlowStep = Common.JSONHelper.ObjectToJson(flowStepIDModel);
            //    if (weixinBll.Update(memberInfo))
            //    {

            //    }

            //    replyModel.ReplyType = "text";
            //    replyModel.ReplyContent = flowStepModel.SendMsg;

            //    return weixinBll.GetResponseMessage(replyModel, strongRequestMessage);

            //    //return weixinBll.FlowProcess(memberInfo, requestMessage);
            //}

            //#endregion

            //var regInfo = weixinBll.Get<WXMemberInfo>(string.Format("UserID='{0}' and WeixinOpenID='{1}'", userInfo.UserID, memberInfo.WeixinOpenID));//注册信息

            //#region 注册模块
            //if (pushContent.Equals(systemSet.WeiXinRegKey))
            //{



            //    if (regInfo == null)
            //    {


            //        string url = string.Format("{0}/weixin/{1}/wx_reg.chtml", weixinBll.Get<SystemSet>("").weiXinAdDomain, Convert.ToString(int.Parse(memberInfo.WeixinMemberID), 16));

            //        //
            //        ResponseMessageNews responseMessage = (ResponseMessageNews)ResponseMessageBase.CreateFromRequestMessage(requestMessage, "news");

            //        responseMessage.Articles.Add(new Article
            //        {
            //            Description = "请点击“查看全文“ 进行注册",
            //            Title = "注册",
            //            PicUrl = string.IsNullOrEmpty(userInfo.WXLogoImg) ? string.Format("{0}/img/jubit_logo.jpg", systemSet.weiXinAdDomain) : string.Format("{0}{1}", systemSet.weiXinAdDomain, userInfo.WXLogoImg),
            //            Url = url
            //        });
            //        return responseMessage;
            //        //
            //        //replyModel.ReplyType = "text";
            //        //replyModel.ReplyContent =url;
            //        //return weixinBll.GetResponseMessage(replyModel, strongRequestMessage);


            //    }
            //    else
            //    {
            //        replyModel.ReplyType = "text";
            //        replyModel.ReplyContent = "您已经注册过了!";
            //        return weixinBll.GetResponseMessage(replyModel, strongRequestMessage);

            //    }


            //}

            //#endregion

            //#region 进入个人中心
            ////进入个人中心回复
            //if (pushContent.Equals(systemSet.WeiXinMemberCenterMenuKey))
            //{
            //    //返回链接
            //    ResponseMessageNews responseMessage = (ResponseMessageNews)ResponseMessageBase.CreateFromRequestMessage(requestMessage, "news");
            //    string quickurl = string.Format("{0}/JuActivity/Wap/WXMemberUserCenter.aspx?uspam={1}&WXCurrOpenerOpenID={2}",
            //            systemSet.weiXinAdDomain,
            //            userInfo.GetUserAutoIDHex(),
            //            memberInfo.WeixinOpenID
            //        );

            //    responseMessage.Articles.Add(new Article
            //    {
            //        Description = "请点击“查看全文“ 进入个人中心",
            //        Title = "个人中心",
            //        PicUrl = string.IsNullOrEmpty(userInfo.WXLogoImg) ? string.Format("{0}/img/jubit_logo.jpg", systemSet.weiXinAdDomain) : string.Format("{0}{1}", systemSet.weiXinAdDomain, userInfo.WXLogoImg),
            //        Url = quickurl
            //    });
            //    return responseMessage;
            //}
            //#endregion

            //#region 进入影响力转发

            //if (pushContent.Equals(systemSet.WeiXinSpreadKey))
            //{
            //    if (regInfo == null)
            //    {
            //        replyModel.ReplyType = "text";
            //        replyModel.ReplyContent = "您还没有注册，请先注册。";
            //        return weixinBll.GetResponseMessage(replyModel, strongRequestMessage);


            //    }


            //    string url = string.Format("{0}/JuActivity/Wap/ActivityList.aspx?aid={1}&m={2}", systemSet.weiXinAdDomain, Convert.ToString(userInfo.AutoID, 16), Convert.ToString(regInfo.MemberID, 16));

            //    ResponseMessageNews responseMessage = (ResponseMessageNews)ResponseMessageBase.CreateFromRequestMessage(requestMessage, "news");

            //    responseMessage.Articles.Add(new Article
            //    {
            //        Description = "请点击“查看全文”，选择文章进行分享",
            //        Title = "影响力转发",
            //        PicUrl = string.IsNullOrEmpty(userInfo.WXLogoImg) ? string.Format("{0}/img/jubit_logo.jpg", systemSet.weiXinAdDomain) : string.Format("{0}{1}", systemSet.weiXinAdDomain, userInfo.WXLogoImg),
            //        Url = url
            //    });
            //    return responseMessage;




            //}
            //#endregion

            //#region 根据ArticleKey_ID直接返回指定文章
            //if (pushContent.StartsWith(systemSet.WXArticleKey))
            //{
            //    int articleID = int.Parse(pushContent.Split('_')[1]);
            //    JuActivityInfo articleInfo = weixinBll.Get<JuActivityInfo>(string.Format("JuActivityID={0}", articleID));

            //    //返回链接
            //    ResponseMessageNews responseMessage = (ResponseMessageNews)ResponseMessageBase.CreateFromRequestMessage(requestMessage, "news");
            //    string quickurl = string.Format("{0}/wxad/{1}/{2}/{3}/detail.chtml", systemSet.weiXinAdDomain, Convert.ToString(articleID, 16), "XXX", memberInfo.WeixinOpenID);
            //    responseMessage.Articles.Add(new Article
            //    {
            //        Description = articleInfo.ActivityName,
            //        Title = articleInfo.ActivityName,
            //        PicUrl = string.IsNullOrEmpty(userInfo.WXLogoImg) ? string.Format("{0}/img/jubit_logo.jpg", systemSet.weiXinAdDomain) : string.Format("{0}{1}", systemSet.weiXinAdDomain, userInfo.WXLogoImg),
            //        Url = quickurl
            //    });
            //    return responseMessage;

            //}
            //#endregion

            //#region 宽桥企业帮 企业核名及状态查询
            //if (pushContent.Equals("kqsubinfo"))//提交宽桥提交信息
            //{
            //    //返回链接
            //    ResponseMessageNews responseMessage = (ResponseMessageNews)ResponseMessageBase.CreateFromRequestMessage(requestMessage, "news");
            //    string quickurl = string.Format("http://{0}/customize/kuanqiao/wap/submitInfo.aspx?oid={1}", HttpContext.Current.Request.Url.Host, memberInfo.WeixinOpenID);
            //    responseMessage.Articles.Add(new Article
            //    {
            //        Description = "点击进入提交申请页面",
            //        Title = "申请企业核名",
            //        PicUrl = string.Format("http://{0}/customize/kuanqiao/image/logo.jpg", HttpContext.Current.Request.Url.Host),
            //        Url = quickurl
            //    });
            //    return responseMessage;
            //}
            //if (pushContent.Equals("kqsubinfostatus"))//查询宽桥提交信息状态
            //{
            //    //返回链接
            //    ResponseMessageNews responseMessage = (ResponseMessageNews)ResponseMessageBase.CreateFromRequestMessage(requestMessage, "news");
            //    string quickurl = string.Format("http://{0}/customize/kuanqiao/wap/submitInfostatus.aspx?oid={1}", HttpContext.Current.Request.Url.Host, memberInfo.WeixinOpenID);
            //    responseMessage.Articles.Add(new Article
            //    {
            //        Description = "点击进入查看进度页面",
            //        Title = "查看进度",
            //        PicUrl = string.Format("http://{0}/customize/kuanqiao/image/logo.jpg", HttpContext.Current.Request.Url.Host),
            //        Url = quickurl
            //    });
            //    return responseMessage;

            //} 
            //#endregion


            #region 微信加V

            if (!string.IsNullOrEmpty(websiteInfo.AddVMenuRName))
            {
                if (pushContent.ToLower().StartsWith("v"))//微信加V
                {
                    try
                    {
                        var addvMap = bllWeixin.Get<WXAddVMaping>(string.Format("AddVKey='{0}' And WebsiteOwner='{1}'", pushContent.ToUpper(), websiteInfo.WebsiteOwner));
                        if (addvMap == null)
                        {
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "加V编号不正确";
                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                        }
                        else
                        {
                            pushContent = addvMap.AddVValue;
                        }
                        //微信加V关键字格式为V_0_1  v为微信加V关键字,0_1 代表 目录_文件名(不含后缀名)
                        string[] arry = pushContent.Split('_');

                        // //加V
                        if (string.IsNullOrEmpty(currSendMsgUser.WXHeadimgurlLocal))
                        {
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "你还没有头像，请先设置头像";
                            //return weixinBll.GetResponseMessage(replyModel, strongRequestMessage);

                        }
                        if (!File.Exists(HttpContext.Current.Server.MapPath(string.Format("/img/WXADDV/{0}/{1}/{2}.png", websiteInfo.WebsiteOwner, arry[1], arry[2]))))
                        {
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "加V编号不正确";
                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                        }

                        //添加到加V任务
                        WXAddVTask task = new WXAddVTask();
                        task.UserId = currSendMsgUser.UserID;
                        task.WeixinOpenID = currSendMsgUser.WXOpenId;
                        task.FilePath = HttpContext.Current.Server.MapPath(string.Format("/img/WXADDV/{0}/{1}/{2}.png", websiteInfo.WebsiteOwner, arry[1], arry[2]));
                        task.FilePath = task.FilePath.Replace("\\", "\\\\");

                        task.Statu = "0";
                        task.InsertDate = DateTime.Now;
                        task.WebsiteOwner = websiteInfo.WebsiteOwner;
                        if (bllWeixin.Add(task))
                        {
                            //添加到加V任务
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "系统正在处理中，稍后您会接收到加V图片。";

                        }
                        else
                        {
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "提交任务失败,请重新输入。";

                        }

                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);




                    }
                    catch (Exception ex)
                    {

                        replyModel.ReplyType = "text";
                        replyModel.ReplyContent = "提交任务失败,请重新输入。";
                        return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);


                    }


                }
            }
            #endregion

            #region 获取自己的OpenID
            if (pushContent.ToLower().Equals("openid"))
            {
                replyModel.ReplyType = "text";
                replyModel.ReplyContent = currSendMsgUser.WXOpenId;
                return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

            }
            #endregion

            #region 我的分销图片

            //TODO: 一键授权的账号，获取我的二维码关键字取到的图片，都是错误的#暂时隐藏该功能#

            if (pushContent.ToLower().Equals("我的二维码"))
            {

                if (!bllUser.IsDistributionMember(currSendMsgUser))
                {
                    replyModel.ReplyType = "text";
                    replyModel.ReplyContent = "您还不是代言人，暂时不能生成专属二维码。";
                    if (!string.IsNullOrEmpty(websiteInfo.NotDistributionMsg))
                    {
                        replyModel.ReplyContent = websiteInfo.NotDistributionMsg;
                    }
                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                }
                if (bllWeixin.GetCount<TimingTask>(string.Format(" Receivers='{0}' And TaskType=9 And DateDiff(ss,InsertDate,GETDATE())<5", currSendMsgUser.WXOpenId)) > 0)
                {
                    replyModel.ReplyType = "text";//处理重复发图片
                    replyModel.ReplyContent = "";
                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                }
                TimingTask task = new TimingTask();
                task.WebsiteOwner = websiteOwner;
                task.InsertDate = DateTime.Now;
                task.Status = 1;
                WebsiteDomainInfo doMain = bllWeixin.Get<WebsiteDomainInfo>(string.Format(" WebsiteOwner='{0}' AND WebsiteDomain <> 'localhost' ", websiteOwner));
                // task.TaskInfo = HttpContext.Current.Request.Url.Authority;
                task.TaskInfo = doMain.WebsiteDomain;
                if (websiteOwner == "comeoncloud")
                {
                    task.TaskInfo = "comeoncloud.comeoncloud.net";
                }
                task.TaskType = 9;
                task.ScheduleDate = DateTime.Now;
                task.Receivers = currSendMsgUser.WXOpenId;
                if (bllUser.Add(task))
                {
                    replyModel.ReplyType = "text";
                    replyModel.ReplyContent = "请稍候，系统正在生成您的专属二维码...";
                    return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);

                }

            }
            #endregion

            #region 关键字自动回复模块

            //全文匹配、开头匹配、结尾匹配、包含匹配
            //获取当前用户所有关键字
            List<Model.WeixinReplyRuleInfo> keywordList = bllWeixin.GetList<Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' and RuleType = 1 ", websiteOwner));

            if (keywordList.Count > 0)
            {
                foreach (var item in keywordList)
                {
                    if (!string.IsNullOrEmpty(item.ReplyContent))
                    {
                        if (item.ReplyContent.Contains("{{openid}}"))
                        {
                            item.ReplyContent = item.ReplyContent.Replace("{{openid}}", currSendMsgUser.WXOpenId);
                        }

                    }
                }
                //先进行全文匹配
                var searchResult = keywordList.Where(p => p.MsgKeyword.Equals(pushContent, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("全文匹配")).ToList();

                if (searchResult.Count > 0)
                {
                    bllWeixin.ToBLLWeixinLog("全文匹配 关键字内容  pushContent:" + pushContent);
                    return bllWeixin.GetResponseMessage(searchResult[0], strongRequestMessage);
                }

                //开头匹配
                searchResult = keywordList.Where(p => pushContent.StartsWith(p.MsgKeyword, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("开头匹配")).ToList();
                if (searchResult.Count > 0)
                {
                    bllWeixin.ToBLLWeixinLog("开头匹配 关键字内容  pushContent:" + pushContent);
                    return bllWeixin.GetResponseMessage(searchResult[0], strongRequestMessage);
                }

                //结尾匹配
                searchResult = keywordList.Where(p => pushContent.EndsWith(p.MsgKeyword, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("结尾匹配")).ToList();
                if (searchResult.Count > 0)
                {
                    bllWeixin.ToBLLWeixinLog("结尾匹配 关键字内容  pushContent:" + pushContent);
                    return bllWeixin.GetResponseMessage(searchResult[0], strongRequestMessage);
                }

                //包含匹配
                searchResult = keywordList.Where(p => pushContent.ToLower().Contains(p.MsgKeyword.ToLower()) && p.MatchType.Equals("包含匹配")).ToList();
                if (searchResult.Count > 0)
                {
                    bllWeixin.ToBLLWeixinLog("包含匹配 关键字内容  pushContent:" + pushContent);
                    return bllWeixin.GetResponseMessage(searchResult[0], strongRequestMessage);
                }

            }


            #region 把信息提交到客户问题接收表

            //当前用户是客服

            if (
                kefuList.Count > 0 && (currWebsiteOwnerUserInfo.WeixinIsAdvancedAuthenticate.Equals(1)) && kefuList.Count(p => p.WeiXinOpenID == currSendMsgUser.WXOpenId) == 0
                //&&
                //(!pushContent.StartsWith("ChannelCode_"))
                && (!pushContent.StartsWith("DistributionCode_"))
                &&
                (!pushContent.StartsWith("ArticleId_"))
                &&
                (!pushContent.Contains("LotteryCode"))
                )
            {
                try
                {
                    if ((!string.IsNullOrEmpty(pushContent) && (!pushContent.StartsWith("http://"))) || (requestMessage.MsgType == WeixinRequestMsgType.Voice) || (requestMessage.MsgType == WeixinRequestMsgType.Image))
                    {
                        //提交问题到客服问题表
                        WXKeFuQuestionReceive keFuQuestionModel = new WXKeFuQuestionReceive();
                        keFuQuestionModel.WebsiteOwner = currWebsiteOwnerUserInfo.UserID;
                        keFuQuestionModel.FromWeixinOpenId = currSendMsgUser.WXOpenId;
                        keFuQuestionModel.QuestionContent = pushContent;
                        keFuQuestionModel.Status = "0";
                        keFuQuestionModel.InsertDate = DateTime.Now;
                        if (requestMessage.MsgType == WeixinRequestMsgType.Voice)//语音信息
                        {
                            keFuQuestionModel.QuestionContent = "语音";
                            keFuQuestionModel.MsgType = "voice";
                            keFuQuestionModel.MediaId = mediaId;
                        }
                        if (requestMessage.MsgType == WeixinRequestMsgType.Image)//图片信息
                        {
                            keFuQuestionModel.QuestionContent = "图片";
                            keFuQuestionModel.MsgType = "image";
                            keFuQuestionModel.MediaId = mediaId;
                        }

                        keFuQuestionModel.WXNickName = currSendMsgUser == null ? "无昵称" : currSendMsgUser.WXNickname;

                        if (bllWeixin.Add(keFuQuestionModel))
                        {
                            //提交问题到客服问题表
                            replyModel.ReplyType = "text";
                            replyModel.ReplyContent = "您的消息已提交给客服。";
                            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
                    //{
                    //    sw.WriteLine(string.Format("{0} kefu异常 :{1}", DateTime.Now.ToString(), ex.Message));
                    //}

                }
            }


            #endregion

            //什么都没匹配上，显示菜单
            //replyModel = menuModel;

            #region 自动回复
            Model.WeixinReplyRuleInfo autoReply = bllWeixin.Get<Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' And MsgKeyword ='消息自动回复'", websiteOwner));
            if (autoReply != null)
            {
                replyModel = autoReply;
                if (string.IsNullOrEmpty(replyModel.ReplyContent))
                {
                    replyModel.ReplyContent = "";
                }
            }
            else
            {
                replyModel.ReplyType = "text";
                replyModel.ReplyContent = " ";
            }

            #endregion

            return bllWeixin.GetResponseMessage(replyModel, strongRequestMessage);


            #endregion

        }

    }

    /// <summary>
    /// 微信 BLL
    /// </summary>
    public class BLLWeixin : BLL
    {

        BLLUser userBll = new BLLUser();
        /// <summary>
        /// 微信开放平台
        /// </summary>
        BLLWeixinOpen bllWeixinOpen = new BLLWeixinOpen();

        /// <summary>
        /// 图文消息结构
        /// </summary>
        public class WeiXinArticle
        {
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// 跳转链接
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 图片链接
            /// </summary>
            public string PicUrl { get; set; }
        }

        public BLLWeixin(string userID)
            : base(userID)
        {

        }
        public BLLWeixin()
        {

        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="log"></param>
        public void ToBLLWeixinLog(string log)
        {
            try
            {
                //return;
                using (StreamWriter sw = new StreamWriter(@"D:\BLLWeixinLog.txt", true, Encoding.GetEncoding("UTF-8")))
                {
                    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                }
            }
            catch { }
        }

        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="signature">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机字符串</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static bool Check(string signature, string timestamp, string nonce, string token)
        {
            return signature == GetSignature(timestamp, nonce, token);
        }
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机字符串</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce, string token)
        {
            //token = token ?? Token;
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }

        #region MsgTypeHelper
        /// <summary>
        /// 获取消息类型
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string GetMsgTypeStr(XDocument doc)
        {
            return doc.Root.Element("MsgType").Value;
        }
        public static WeixinRequestMsgType GetMsgType(XDocument doc)
        {
            return GetMsgType(doc.Root.Element("MsgType").Value);
        }
        public static WeixinRequestMsgType GetMsgType(string str)
        {
            return (WeixinRequestMsgType)Enum.Parse(typeof(WeixinRequestMsgType), str, true);
        }
        #endregion

        #region EntityHelper
        public static void FillEntityWithXml<T>(T entity, XDocument doc) where T : RequestMessageBase, new()
        {
            entity = entity ?? new T();
            var root = doc.Root;

            var props = entity.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (root.Element(propName) != null)
                {
                    switch (prop.PropertyType.Name)
                    {
                        //case "String":
                        //    goto default;
                        case "DateTime":
                            prop.SetValue(entity, new DateTime(long.Parse(root.Element(propName).Value)), null);
                            break;
                        case "Boolean":
                            if (propName == "FuncFlag")
                            {
                                prop.SetValue(entity, root.Element(propName).Value == "1", null);
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "Int64":
                            prop.SetValue(entity, long.Parse(root.Element(propName).Value), null);
                            break;
                        case "Int32":
                            prop.SetValue(entity, int.Parse(root.Element(propName).Value), null);
                            break;
                        case "Double":
                            prop.SetValue(entity, double.Parse(root.Element(propName).Value), null);
                            break;
                        case "WeixinRequestMsgType":
                            prop.SetValue(entity, GetMsgType(root.Element(propName).Value), null);
                            break;
                        default:
                            prop.SetValue(entity, root.Element(propName).Value, null);
                            break;
                    }
                }
            }
        }

        public static XDocument ConvertEntityToXml<T>(T entity) where T : class , new()
        {
            entity = entity ?? new T();
            var doc = new XDocument();
            doc.Add(new XElement("xml"));
            var root = doc.Root;

            //经过测试，微信对字段排序有严格要求，这里对排序进行强制约束
            var propNameOrder = new[] { "ToUserName", "FromUserName", "CreateTime", "MsgType", "Content", "ArticleCount", "Articles", "FuncFlag",/*以下是Atricle属性*/"Title ", "Description ", "PicUrl", "Url" }.ToList();
            Func<string, int> orderByPropName = propNameOrder.IndexOf;

            var props = entity.GetType().GetProperties().OrderBy(p => orderByPropName(p.Name)).ToList();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (propName == "Articles")
                {
                    var atriclesElement = new XElement("Articles");
                    var articales = prop.GetValue(entity, null) as List<Model.Weixin.Article>;
                    foreach (var articale in articales)
                    {
                        var subNodes = ConvertEntityToXml(articale).Root.Elements();
                        atriclesElement.Add(new XElement("item", subNodes));
                    }
                    root.Add(atriclesElement);
                }
                else
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "String":
                            root.Add(new XElement(propName,
                                                  new XCData(prop.GetValue(entity, null) as string ?? "")));
                            break;
                        case "DateTime":
                            root.Add(new XElement(propName, ((DateTime)prop.GetValue(entity, null)).Ticks));
                            break;
                        case "Boolean":
                            if (propName == "FuncFlag")
                            {
                                root.Add(new XElement(propName, (bool)prop.GetValue(entity, null) ? "1" : "0"));
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "ResponseMsgType":
                            root.Add(new XElement(propName, prop.GetValue(entity, null).ToString().ToLower()));
                            break;
                        case "Article":
                            root.Add(new XElement(propName, prop.GetValue(entity, null).ToString().ToLower()));
                            break;
                        default:
                            root.Add(new XElement(propName, prop.GetValue(entity, null)));
                            break;
                    }
                }
            }
            return doc;
        }
        #endregion

        #region RequestMessageFactory
        public static IRequestMessageBase GetRequestEntity(XDocument doc)
        {
            var msgType = GetMsgType(doc);
            RequestMessageBase requestMessage = null;
            switch (msgType)
            {
                case WeixinRequestMsgType.Text:
                    requestMessage = new RequestMessageText();
                    break;
                case WeixinRequestMsgType.Location:
                    requestMessage = new RequestMessageLocation();
                    break;
                case WeixinRequestMsgType.Image:
                    requestMessage = new RequestMessageImage();
                    break;
                case WeixinRequestMsgType.Event:
                    requestMessage = new RequestMessageEvent();
                    break;
                case WeixinRequestMsgType.Voice:
                    requestMessage = new RequestMessageVoice();
                    break;


                default:
                    throw new ArgumentOutOfRangeException();
            }
            FillEntityWithXml(requestMessage, doc);
            return requestMessage;
        }
        #endregion

        /// <summary>
        /// 获取系统微信用户信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="openID">微信用户OpenID</param>
        /// <returns></returns>
        public Model.WeixinMemberInfo GetMemberInfo(string userID, string openID)
        {
            return Get<Model.WeixinMemberInfo>(string.Format(" WeixinOpenID = '{0}' and UserID = '{1}' ", openID, userID));
        }

        /// <summary>
        /// 获取微信会员信息信息(因为修改代码已导致新的WXMemberInfo出现，影响力转发的时候产生，在流程里面做的特殊会员注册导致，所以后面所有会员操作以WXMemberInfo为准)
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="openID">微信用户OpenID</param>
        /// <returns></returns>
        public Model.WXMemberInfo GetWXMemberInfo(string userID, string openID)
        {
            return Get<Model.WXMemberInfo>(string.Format(" WeixinOpenID = '{0}' and UserID = '{1}' ", openID, userID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns></returns>
        public string ActionResult(Stream inputStream, string websiteOwner)
        {
            var doc = XDocument.Load(inputStream);
            doc.Save(HttpContext.Current.Server.MapPath("~/FileUpload/Weixin/msg/" + DateTime.Now.Ticks + "_Request.txt"));

            var requestMessage = GetRequestEntity(doc);
            return ActionResultProcess(requestMessage, websiteOwner);
        }

        /// <summary>
        /// 微信被动回复 开放平台
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns></returns>
        public string ActionResultOpen(string xml, string websiteOwner)
        {
            var doc = XDocument.Parse(xml);
            doc.Save(HttpContext.Current.Server.MapPath("~/FileUpload/Weixin/msg/" + DateTime.Now.Ticks + "_Request.txt"));
            var requestMessage = GetRequestEntity(doc);

            return ActionResultProcess(requestMessage, websiteOwner);
        }

        /// <summary>
        /// 微信被动回复
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public string ActionResultProcess(IRequestMessageBase requestMessage, string websiteOwner)
        {
            ResponseMessageBase responseMessage = null;
            BLLUser bllUser = new BLLUser(websiteOwner);
            Model.UserInfo currWebsiteOwnerUserInfo = bllUser.GetUserInfo(websiteOwner, websiteOwner);

            //存入消息明细信息
            Model.WeixinMsgDetails weixinMsgDetailsModel = CreateMsgDetailsByReq(requestMessage as RequestMessageBase, websiteOwner);

            //记录站点所属信息
            weixinMsgDetailsModel.WebsiteOwner = currWebsiteOwnerUserInfo.UserID;

            var websiteWXUserRegType = new BLLWebSite().GetWebsiteWXUserRegType(websiteOwner);

            UserInfo tmpUser = null;

            #region 记录会员信息
            string openID = requestMessage.FromUserName;
            //Model.WeixinMemberInfo memberInfo = GetMemberInfo(websiteOwner, openID);
            //if (memberInfo != null)
            //{
            //    memberInfo.LastVisitDate = DateTime.Now;
            //    Update(memberInfo);
            //}
            //else
            //{
            //    memberInfo = new Model.WeixinMemberInfo();
            //    memberInfo.WeixinMemberID = new BLLUser(websiteOwner).GetGUID(TransacType.WeixinMemberAdd);
            //    memberInfo.UserID = websiteOwner;
            //    memberInfo.WeixinOpenID = openID;
            //    memberInfo.UserWeixinPubOrgID = currWebsiteOwner.WeixinPubOrgID;
            //    memberInfo.FirstVisitDate = DateTime.Now;
            //    memberInfo.LastVisitDate = DateTime.Now;
            //    memberInfo.CurrAction = "访问";
            //    memberInfo.IsRegMember = 0;
            //    Add(memberInfo);


            //}

            UserInfo currAccessUser = bllUser.GetUserInfoByOpenId(openID, websiteOwner);//当前访问的用户
            if (currAccessUser == null)
            {
                //var isNotRegNew = new BLLCommRelation().GetRelationInfo(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, websiteOwner, "");
                if (websiteWXUserRegType == Enums.WebsiteWXUserRegType.ManualRegBeforePage)
                {
                    var websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
                    var websiteConfig = new BLLWebSite().GetCompanyWebsiteConfig(websiteOwner);
                    var strongresponseMessage = ResponseMessageBase.CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.News) as ResponseMessageNews;
                    string href = string.Format("http://{0}/App/Member/Wap/RegisterBinding.aspx", HttpContext.Current.Request.Url.Host);

                    strongresponseMessage.Articles.Add(new Article
                    {
                        Description = "点击此链接，注册并成为会员！",
                        Title = string.Format("注册成为{0}会员！", websiteInfo.WebsiteName),
                        PicUrl = websiteConfig.WebsiteImage,
                        Url = href
                    });

                    responseMessage = strongresponseMessage;
                    var responseDocNotReg = ConvertEntityToXml(responseMessage);
                    responseDocNotReg.Save(HttpContext.Current.Server.MapPath("~/FileUpload/Weixin/msg/" + DateTime.Now.Ticks + "_Response.txt"));
                    return responseDocNotReg.ToString();

                }


                //注册用户
                currAccessUser = new ZentCloud.BLLJIMP.Model.UserInfo();
                currAccessUser.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());
                currAccessUser.Password = ZentCloud.Common.Rand.Str_char(12);

                currAccessUser.UserType = 2;

                currAccessUser.WebsiteOwner = websiteOwner; //ZentCloud.Common.ConfigHelper.GetConfigString("WebsiteOwner");

                currAccessUser.Regtime = DateTime.Now;
                currAccessUser.WXOpenId = openID;

                currAccessUser.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                currAccessUser.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                currAccessUser.LastLoginDate = DateTime.Now;
                currAccessUser.LoginTotalCount = 1;

                ToBLLWeixinLog("通过消息注册新用户，开始获取微信用户信息，websiteOwner:" + websiteOwner);

                var accessToken = GetAccessToken(websiteOwner);

                ToBLLWeixinLog("accessToken:" + accessToken);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    ToBLLWeixinLog("开始获取微信个人信息，openid：" + openID);




                    var wxUserInfo = GetWeixinUserInfo(accessToken, openID);
                    ToBLLWeixinLog("wxUserInfo:" + JSONHelper.ObjectToJson(wxUserInfo));
                    if (wxUserInfo != null)
                    {
                        currAccessUser.WXHeadimgurl = wxUserInfo.headimgurl;
                        currAccessUser.WXNickname = wxUserInfo.nickname;
                        currAccessUser.WXProvince = wxUserInfo.province;
                        currAccessUser.WXCity = wxUserInfo.city;

                    }

                }


                if (websiteWXUserRegType == Enums.WebsiteWXUserRegType.ManualRegAfterOperate)
                {
                    tmpUser = currAccessUser;
                }
                else
                {
                    if (Add(currAccessUser) && string.IsNullOrEmpty(currAccessUser.WXNickname))
                    {
                        currAccessUser = bllUser.GetUserInfo(currAccessUser.UserID, currAccessUser.WebsiteOwner);
                        var th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(UpdateWexinUserInfo));
                        th.Start(currAccessUser);
                    }
                }



            }
            else
            {
                //ToLog("老用户登录");
                //更新用户信息
                //currAccessUser.WXOpenId = openID;
                //currAccessUser.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                currAccessUser.LastLoginDate = DateTime.Now;
                // currAccessUser.LoginTotalCount++;
                Update(currAccessUser, string.Format("LastLoginDate='{0}'", DateTime.Now), string.Format("AutoID={0}", currAccessUser.AutoID));
            }

            #endregion

            if (requestMessage.MsgType == WeixinRequestMsgType.Text || requestMessage.MsgType == WeixinRequestMsgType.Event || requestMessage.MsgType == WeixinRequestMsgType.Voice || requestMessage.MsgType == WeixinRequestMsgType.Image)
            {
                var reqStrongMsg = requestMessage as RequestMessageText;

                responseMessage = new CreateActionResultJubitForKeyword().CreateActionResult(requestMessage, websiteOwner, currAccessUser.UserID, tmpUser);
                ToBLLWeixinLog("ActionResultProcess responseMessage:" + JsonConvert.SerializeObject(responseMessage));
                if (string.IsNullOrEmpty(responseMessage.Content) && responseMessage.MsgType == WeixinResponseMsgType.Text)
                {
                    return " ";
                }
            }
            else
            {
                var strongRequestMessage = requestMessage as RequestMessageText;
                var strongresponseMessage = ResponseMessageBase.CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.Text) as ResponseMessageText;
                strongresponseMessage.Content = "亲，目前只支持文本查询，请重新输入~";
                responseMessage = strongresponseMessage;
            }

            var responseDoc = ConvertEntityToXml(responseMessage);
            responseDoc.Save(HttpContext.Current.Server.MapPath("~/FileUpload/Weixin/msg/" + DateTime.Now.Ticks + "_Response.txt"));

            //更新消息明细信息
            UpdateMsgDetailsReplyByRep(responseMessage, weixinMsgDetailsModel);

            return responseDoc.ToString();
        }

        /// <summary>
        /// 根据请求信息创建信息明细实体
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Model.WeixinMsgDetails CreateMsgDetailsByReq(RequestMessageBase req, string userID)
        {
            Model.WeixinMsgDetails model = new Model.WeixinMsgDetails();

            model.UID = GetGUID(TransacType.WeinxinDetailsAdd);
            model.ReceiveDate = DateTime.Now;
            model.ReceiveDateOrg = req.CreateTime.ToString();
            model.UserID = userID;
            model.ReceiveOpenID = req.FromUserName;
            model.UserWeixinPubOrgID = req.ToUserName;
            model.ReceiveMsgID = req.MsgId;

            switch (req.MsgType)
            {
                case WeixinRequestMsgType.Text:
                    {
                        var strongReq = req as RequestMessageText;
                        model.ReceiveType = "text";
                        model.ReceiveContent = strongReq.Content;

                        break;
                    }
                case WeixinRequestMsgType.Location:
                    {
                        var strongReq = req as RequestMessageLocation;
                        model.ReceiveType = "location";
                        model.ReceiveLocationX = strongReq.Location_X.ToString();
                        model.ReceiveLocationY = strongReq.Location_Y.ToString();
                        model.ReceiveLocationScale = strongReq.Scale;
                        model.ReceiveLocationLabel = strongReq.Label;

                        break;
                    }
                case WeixinRequestMsgType.Image:
                    {
                        var strongReq = req as RequestMessageImage;
                        model.ReceiveType = "image";
                        model.ReceivePicUrl = strongReq.PicUrl;

                        break;
                    }
                case WeixinRequestMsgType.Event:
                    {
                        var strongReq = req as RequestMessageEvent;
                        model.ReceiveType = "event";
                        model.ReceiveContent = strongReq.EventKey;
                        break;
                    }
                default:
                    break;
            }

            try
            {
                Add(model);
            }
            catch (Exception ex)
            {

                ToLog(ex.ToString());
            }


            return model;
        }

        /// <summary>
        /// 更新消息明细回复信息
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMsgDetailsReplyByRep(ResponseMessageBase rep, Model.WeixinMsgDetails model)
        {
            bool result = false;
            model.ReplyDate = DateTime.Now;

            try
            {
                if (rep == null)
                {
                    model.Description = "没有对应的规则，不返回信息";
                }
                else
                {
                    switch (rep.MsgType)
                    {
                        case WeixinResponseMsgType.Text:
                            {
                                model.ReplyType = "text";
                                var repStrong = rep as ResponseMessageText;
                                model.ReplyContent = repStrong.Content;

                                break;
                            }
                        case WeixinResponseMsgType.News:
                            {
                                model.ReplyType = "news";
                                var repStrong = rep as ResponseMessageNews;
                                model.ReplyReplyArticleCount = repStrong.ArticleCount;

                                //存入图片
                                int index = 0;
                                foreach (var item in repStrong.Articles)
                                {
                                    Add(new Model.WeixinMsgDetailsImgsInfo()
                                    {
                                        UID = GetGUID(TransacType.WeixinMsgDetailsImgsAdd),
                                        MsgID = model.UID,
                                        OrderIndex = index++,
                                        Title = item.Title,
                                        PicUrl = item.PicUrl,
                                        Url = item.Url,
                                        Description = item.Description
                                    });
                                }

                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                model.Description = ex.Message;
            }


            result = Update(model);

            return result;
        }

        /// <summary>
        /// 根据回复规则实体和请求信息获取回复信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="reqmsg"></param>
        /// <returns></returns>
        public ResponseMessageBase GetResponseMessage(Model.WeixinReplyRuleInfo model, RequestMessageBase reqMsg)
        {
            ToBLLWeixinLog("进入 responseMessage reqMsg：" + JsonConvert.SerializeObject(reqMsg));
            ResponseMessageBase responseMessage = null;
            try
            {

                responseMessage = ResponseMessageBase.CreateFromRequestMessage(reqMsg, model.ReplyType);

                switch (responseMessage.MsgType)
                {
                    case WeixinResponseMsgType.Text:
                        {
                            var tmpStrongResponseMessage = responseMessage as ResponseMessageText;
                            tmpStrongResponseMessage.Content = model.ReplyContent;
                            responseMessage = tmpStrongResponseMessage;
                        }
                        break;
                    case WeixinResponseMsgType.Event:
                        {
                            var tmpStrongResponseMessage = responseMessage as ResponseMessageText;
                            tmpStrongResponseMessage.Content = model.ReplyContent;
                            responseMessage = tmpStrongResponseMessage;
                        }
                        break;
                    case WeixinResponseMsgType.News:
                        {
                            var tmpStrongResponseMessage = responseMessage as ResponseMessageNews;
                            tmpStrongResponseMessage.Content = model.ReplyContent;

                            //获取图片集合
                            List<Model.WeixinReplyRuleImgsInfo> imgList = GetList<Model.WeixinReplyRuleImgsInfo>(string.Format(" RuleID = '{0}'", model.UID)).OrderBy(p => p.OrderIndex.Value).ToList();
                            foreach (var item in imgList)
                            {
                                tmpStrongResponseMessage.Articles.Add(new Article()
                                {
                                    Title = item.Title,
                                    Description = item.Description,
                                    PicUrl = item.PicUrl,
                                    Url = item.Url
                                });
                            }
                            responseMessage = tmpStrongResponseMessage;
                        }

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ToBLLWeixinLog("构造 responseMessage 异常：" + ex.Message);
            }

            ToBLLWeixinLog("构造 的 responseMessage ：" + JsonConvert.SerializeObject(responseMessage));
            return responseMessage;
        }

        ///// <summary>
        ///// 流程处理(处理处在流程内的事务)
        ///// </summary>
        ///// <param name="memberInfo"></param>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //public ResponseMessageBase FlowProcess(Model.WeixinMemberInfo memberInfo, IRequestMessageBase requestMessage)
        //{
        //    /*
        //     * TODO:20130331 流程处理
        //     * 1.获取用户所在的流程步骤。
        //     * 2.验证用户发送信息格式。
        //     * 3.保存用户发送信息。
        //     * 4.查询用户是否有下一步。
        //     * 4a.是则更改用户状态为下一步状态，并返回步骤下发信息。
        //     * 4b.否则更改用户状态为访问状态，并返回流程结束信息。
        //     * 
        //     */
        //    Model.WeixinReplyRuleInfo replyModel = new Model.WeixinReplyRuleInfo();
        //    RequestMessageText strongRequestMessage = requestMessage as RequestMessageText;
        //    ResponseMessageBase responseMessage = null;
        //    string reqMsg = strongRequestMessage.Content;
        //    //responseMessage = ResponseMessageBase.CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.Text) as ResponseMessageText;

        //    WeixinMemberFlowStepIDInfo flowStepIDModel = Common.JSONHelper.JsonToModel<WeixinMemberFlowStepIDInfo>(memberInfo.FlowStep);
        //    Model.WXFlowStepInfo flowStepModel = Get<Model.WXFlowStepInfo>(string.Format(" FlowID = {0} and StepID = {1} ", flowStepIDModel.FlowID, flowStepIDModel.StepID));
        //    Model.WXFlowInfo flowModel = Get<Model.WXFlowInfo>(" FlowID = " + flowStepIDModel.FlowID.ToString());
        //    Model.UserInfo userInfo = new BLLUser(memberInfo.UserID).GetUserInfo(memberInfo.UserID);

        //    if (flowStepModel == null)
        //    {
        //        memberInfo.CurrAction = "访问";

        //        replyModel.ReplyContent = flowModel.FlowEndMsg;
        //        if (Update(memberInfo))
        //        {

        //        }
        //        replyModel.ReplyType = "text";
        //        responseMessage = GetResponseMessage(replyModel, strongRequestMessage);
        //        return responseMessage;
        //    }

        //    if (flowStepIDModel.IsInVerifyCode != 1)//不处于验证码阶段才验证格式保存数据
        //    {
        //        #region 验证格式



        //        bool logicAuth = true;

        //        switch (flowStepModel.AuthFunc)
        //        {
        //            case "email":
        //                {
        //                    logicAuth = Common.ValidatorHelper.EmailLogicJudge(reqMsg);
        //                    break;
        //                }
        //            case "phone":
        //                {
        //                    flowStepIDModel.IsInVerifyCode = 0;//防止初始化没有赋值
        //                    logicAuth = Common.ValidatorHelper.PhoneNumLogicJudge(reqMsg);
        //                    break;
        //                }
        //            default:
        //                break;
        //        }


        //        if (!logicAuth)//验证失败
        //        {
        //            replyModel.ReplyType = "text";
        //            replyModel.ReplyContent = flowStepModel.ErrorMsg;
        //            return GetResponseMessage(replyModel, strongRequestMessage);
        //        }


        //        #endregion

        //        #region 验证通过保存信息
        //        Model.WXFlowDataInfo dataInfo = new Model.WXFlowDataInfo();

        //        dataInfo.FlowID = flowStepIDModel.FlowID;
        //        dataInfo.StepID = flowStepIDModel.StepID;
        //        dataInfo.OpenID = strongRequestMessage.FromUserName;
        //        dataInfo.FlowField = flowStepModel.FlowField;
        //        dataInfo.Data = reqMsg;
        //        dataInfo.InsertDate = DateTime.Now;


        //        if (!Exists(dataInfo, new List<string>() { "FlowID", "StepID", "OpenID" }))
        //        {
        //            if (Add(dataInfo))
        //            {

        //            }
        //        }
        //        else
        //        {
        //            if (Update(dataInfo))
        //            {

        //            }
        //        }
        //        #endregion
        //    }

        //    if (flowStepModel.IsVerifyCode == 1 && flowStepIDModel.IsInVerifyCode == 1)
        //    {
        //        //处在验证阶段，则验证验证码是否正确

        //        if (memberInfo.RegVerifyCode != reqMsg)
        //        {
        //            replyModel.ReplyType = "text";
        //            replyModel.ReplyContent = "验证码错误，请重新输入";
        //            return GetResponseMessage(replyModel, strongRequestMessage);
        //        }


        //    }
        //    else if (flowStepModel.IsVerifyCode == 1 && flowStepIDModel.IsInVerifyCode == 0)
        //    {
        //        //不处在验证码阶段，发送验证码

        //        //我们已经给您号码为【{0}】的手机发送了验证码，请回复验证码完成验证！
        //        if (flowStepModel.AuthFunc == "phone")
        //        {
        //            flowStepIDModel.IsInVerifyCode = 1;
        //            memberInfo.RegVerifyCode = Common.Rand.Number(6);//构造验证码
        //            memberInfo.FlowStep = Common.JSONHelper.ObjectToJson(flowStepIDModel);//更新验证步骤

        //            if (Update(memberInfo))
        //            {

        //            }

        //            //发送短信
        //            new BLLSMS(memberInfo.UserID).SubmitSMS("membertrigger", reqMsg, string.Format("您的验证码为{0} [{1}]", memberInfo.RegVerifyCode, (userInfo.WeixinPublicName == "") || (userInfo.WeixinPublicName == null) ? "臻云" : userInfo.WeixinPublicName));

        //            replyModel.ReplyType = "text";
        //            replyModel.ReplyContent = string.Format("我们已经给您号码为【{0}】的手机发送了验证码，请回复验证码完成验证！", reqMsg);
        //            return GetResponseMessage(replyModel, strongRequestMessage);//返回提示输入验证码信息


        //        }


        //    }



        //    #region 查询用户是否有下一步并更新会员状态
        //    //查询用户是否有下一步
        //    Model.WXFlowStepInfo nextFlowStepModel = Get<Model.WXFlowStepInfo>(string.Format(" FlowID = {0} and StepID = {1} ", flowStepIDModel.FlowID, flowStepIDModel.StepID + 1));
        //    if (nextFlowStepModel != null)
        //    {
        //        memberInfo.CurrAction = "流程";
        //        flowStepIDModel.StepID = nextFlowStepModel.StepID;
        //        memberInfo.FlowStep = Common.JSONHelper.ObjectToJson(flowStepIDModel);
        //        replyModel.ReplyContent = nextFlowStepModel.SendMsg;
        //    }
        //    else
        //    {
        //        memberInfo.CurrAction = "访问";
        //        memberInfo.FlowStep = "";

        //        //将流程ID记录到用户流程历史
        //        memberInfo.AddFlowIDHistory(flowStepIDModel.FlowID);

        //        replyModel.ReplyContent = flowModel.FlowEndMsg;

        //        //流程结束记录
        //        foreach (var item in GetList<Model.WXMemberFlowHistory>(1, string.Format(" FlowID = {0} and WeixinMemberID = '{1}' ", flowModel.FlowID, memberInfo.WeixinMemberID), "BeginDate Desc"))
        //        {
        //            Model.WXMemberFlowHistory memberFlowHistory = item;

        //            memberFlowHistory.EndDate = DateTime.Now;
        //            Update(memberFlowHistory);
        //        }

        //    }

        //    if (Update(memberInfo))
        //    {

        //    }

        //    //构造下发回复信息
        //    replyModel.ReplyType = "text";
        //    responseMessage = GetResponseMessage(replyModel, strongRequestMessage);
        //    #endregion


        //    return responseMessage;
        //}

        //public string ActionResultDemo(Stream inputStream)
        //{
        //    try
        //    {
        //        var doc = XDocument.Load(inputStream);
        //        doc.Save(HttpContext.Current.Server.MapPath("~/FileUpload/Weixin/msg/" + DateTime.Now.Ticks + "_Request.txt"));

        //        var requestMessage = GetRequestEntity(doc);
        //        ResponseMessageBase responseMessage = null;
        //        switch (requestMessage.MsgType)
        //        {
        //            case WeixinRequestMsgType.Text:
        //                {
        //                    //TODO:交给Service处理具体信息
        //                    var strongRequestMessage = requestMessage as RequestMessageText;
        //                    var strongresponseMessage = ResponseMessageBase.CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.Text) as ResponseMessageText;

        //                    strongresponseMessage.Content = string.Format("您刚才发送了文字信息：{0}", strongRequestMessage.Content);
        //                    responseMessage = strongresponseMessage;

        //                    break;
        //                }
        //            case WeixinRequestMsgType.Location:
        //                {
        //                    var strongRequestMessage = requestMessage as RequestMessageLocation;
        //                    var strongresponseMessage =
        //                        ResponseMessageBase.CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.Text) as
        //                        ResponseMessageText;
        //                    strongresponseMessage.Content = string.Format("您刚才发送了地理位置信息。Location_X：{0}，Location_Y：{1}，Scale：{2}",
        //                        strongRequestMessage.Location_X, strongRequestMessage.Location_Y, strongRequestMessage.Scale);
        //                    responseMessage = strongresponseMessage;
        //                    break;
        //                }
        //            case WeixinRequestMsgType.Image:
        //                {
        //                    var strongRequestMessage = requestMessage as RequestMessageImage;
        //                    var strongresponseMessage =
        //                        ResponseMessageBase.CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.News) as
        //                        ResponseMessageNews;
        //                    strongresponseMessage.Content = "这里是正文内容，一共将发2条Article。";
        //                    strongresponseMessage.Articles.Add(new Article()
        //                    {
        //                        Title = "您刚才发送了图片信息",
        //                        Description = "您发送的图片将会显示在边上",
        //                        PicUrl = strongRequestMessage.PicUrl,
        //                        Url = "http://www.jubit.org/weixin/wellcome2.jpg"
        //                    });
        //                    strongresponseMessage.Articles.Add(new Article()
        //                    {
        //                        Title = "第二条",
        //                        Description = "第二条带连接的内容",
        //                        PicUrl = strongRequestMessage.PicUrl,
        //                        Url = "http://www.jubit.org/weixin/wellcome2.jpg"
        //                    });
        //                    responseMessage = strongresponseMessage;
        //                    break;
        //                }
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //        var responseDoc = ConvertEntityToXml(responseMessage);
        //        responseDoc.Save(HttpContext.Current.Server.MapPath("~/FileUpload/Weixin/msg/" + DateTime.Now.Ticks + "_Response.txt"));

        //        return responseDoc.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        ///// <summary>
        ///// 获取会员流程数据
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //public DataTable GetMemberFlowDataView(List<Model.WXFlowDataInfo> dataList)
        //{
        //    DataTable dt = new DataTable();

        //    if (dataList.Count <= 0)
        //        return dt;

        //    dt.Columns.Add("OpenID", typeof(String));

        //    //设置列：只根据具体数据字段定义
        //    //foreach (var item in dataList.DistinctBy(p => p.FlowField).ToList())
        //    //{
        //    //    dt.Columns.Add(item.FlowField, typeof(String));
        //    //}

        //    //设置列：根据流程步骤字段定义

        //    //获取流所有步骤
        //    List<Model.WXFlowStepInfo> flowStepAll = GetList<Model.WXFlowStepInfo>(string.Format(" FlowID = {0} ", dataList[0].FlowID.ToString()));

        //    foreach (var item in flowStepAll)
        //    {
        //        dt.Columns.Add(item.FlowField, typeof(String));
        //    }

        //    foreach (var item in dataList.GroupBy(p => p.OpenID))
        //    {
        //        DataRow dr = dt.NewRow();
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            string columnName = dt.Columns[i].ColumnName;
        //            var tmpList = dataList.Where(p => p.OpenID.Equals(item.Key) && p.FlowField.Equals(columnName)).ToList();
        //            if (tmpList.Count > 0)
        //            {
        //                dr[columnName] = tmpList[0].Data;
        //            }
        //        }
        //        dr["OpenID"] = item.Key;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}
        /// <summary>
        /// 检查关键字重复
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public bool CheckUserKeyword(string userId, string keyWord)
        {

            var count = GetCount<WeixinReplyRuleInfo>(string.Format("UserID='{0}' and MsgKeyword='{1}' ", userId, keyWord));
            if (count > 0)
            {
                return false;

            }
            var flowCount = GetCount<WXFlowInfo>(string.Format("UserID='{0}' and FlowKeyword='{1}' ", userId, keyWord));
            if (flowCount > 0)
            {
                return false;

            }
            return true;



        }

        ///// <summary>
        ///// 添加用户默认流程
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public bool AddUserDefaultFlow(string userID)
        //{
        //    //添加默认注册流程
        //    try
        //    {

        //        WXFlowInfo flowModel = new WXFlowInfo();

        //        flowModel.FlowID = int.Parse(GetGUID(TransacType.WeixinFlowAdd));
        //        flowModel.FlowKeyword = "R";
        //        flowModel.FlowName = "用户注册";
        //        flowModel.FlowEndMsg = "恭喜您已注册成功！";
        //        flowModel.MemberLimitState = 1;
        //        flowModel.FlowLimitMsg = "您已注册为我们会员！";
        //        flowModel.FlowSysType = 3;
        //        flowModel.IsEnable = 0;
        //        flowModel.UserID = userID;

        //        List<WXFlowStepInfo> flowStepList = new List<WXFlowStepInfo>()
        //    {
        //        new WXFlowStepInfo(){ FlowID = flowModel.FlowID, StepID = 1, FlowField = "姓名", FieldDescription ="用户的姓名", SendMsg ="请输入您的姓名：", ErrorMsg ="姓名输入有误，请重新输入!"},
        //        new WXFlowStepInfo(){ FlowID = flowModel.FlowID, StepID = 2, FlowField = "手机", FieldDescription ="用户的手机号码", SendMsg ="请输入您的手机号码：", ErrorMsg ="手机号输入有误，请重新输入!", AuthFunc = "phone", IsVerifyCode = 0},
        //        new WXFlowStepInfo(){ FlowID = flowModel.FlowID, StepID = 3, FlowField = "邮箱", FieldDescription ="用户的邮箱地址", SendMsg ="请输入您的邮箱地址：", ErrorMsg ="邮箱地址输入有误，请重新输入!", AuthFunc="email"}
        //    };

        //        Add(flowModel);
        //        AddList<WXFlowStepInfo>(flowStepList);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 创建AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        //public WeixinAccessToken GetAccessToken(string appId, string appSecret)
        //{

        //    Common.HttpInterFace WebRequest = new Common.HttpInterFace();//
        //    string Parm = string.Format("grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
        //    string result = WebRequest.GetWebRequest(Parm, "https://api.weixin.qq.com/cgi-bin/token", System.Text.Encoding.GetEncoding("utf-8"));

        //    return Common.JSONHelper.JsonToModel<WeixinAccessToken>(result);



        //}

        /// <summary>
        /// 获取AccessToken 指定站点
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns></returns>
        public string GetAccessToken(string websiteOwner)
        {

            BLLWeixin bllWeixin = new BLLWeixin();
            bllWeixin.ToBLLWeixinLog("GetAccessToken Start,websiteOwner: " + websiteOwner);
            WebsiteInfo websiteInfo = userBll.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            bllWeixin.ToBLLWeixinLog("GetAccessToken websiteInfo:" + JsonConvert.SerializeObject(websiteInfo));

            //if (IsAuthToOpen(websiteOwner) && (websiteInfo.AuthorizerServiceType == "2") && (websiteInfo.AuthorizerVerifyType == "0"))//服务号且经过微信认证

            if (IsAuthToOpen(websiteOwner))//
            {
                bllWeixin.ToBLLWeixinLog("GetAccessToken IsAuthToOpen");
                return bllWeixinOpen.GetAuthorizerToken(websiteOwner);
            }

            UserInfo userInfo = userBll.GetUserInfo(websiteOwner, websiteOwner);
            if (string.IsNullOrEmpty(userInfo.WXAccessToken))
            {
                userInfo.WXAccessTokenExpireTicks = 0;
            }
            //到期前30分钟刷新accesstoken
            if (DateTime.Now.Ticks < userInfo.WXAccessTokenExpireTicks - (long)1800 * 10000000)
            {
                return userInfo.WXAccessToken;
            }

            Common.HttpInterFace webRequest = new Common.HttpInterFace();//
            string parm = string.Format("grant_type=client_credential&appid={0}&secret={1}", userInfo.WeixinAppId, userInfo.WeixinAppSecret);
            string requestResult = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/token", System.Text.Encoding.GetEncoding("utf-8"));
            WeixinAccessToken wxAccToken = Common.JSONHelper.JsonToModel<WeixinAccessToken>(requestResult);
            if (wxAccToken.errcode != 0)
            {
                return string.Empty;
            }
            userInfo.WXAccessToken = wxAccToken.access_token;
            userInfo.WXAccessTokenExpireTicks = DateTime.Now.Ticks + (long)wxAccToken.expires_in * 10000000;

            int updateResultCount = Update(userInfo, string.Format("WXAccessToken='{0}',WXAccessTokenExpireTicks={1}", userInfo.WXAccessToken, userInfo.WXAccessTokenExpireTicks), string.Format("AutoID={0}", userInfo.AutoID));
            if (updateResultCount == 0)
            {
                return string.Empty;
            }
            return userInfo.WXAccessToken;
        }

        /// <summary>
        /// 获取AccessToken 当前站点
        /// </summary>
        /// <returns>AccessToken</returns>
        public string GetAccessToken()
        {
            if (!BLL.GetCheck()) return "";

            #region 使用开放平台Token
            if (IsAuthToOpen())//如果已经授权给开放平台,则使用开放平台授权的Token
            {
                return bllWeixinOpen.GetAuthorizerToken();
            }
            #endregion

            #region 默认Token
            else
            {


                UserInfo userInfo = userBll.GetCurrWebSiteUserInfo();

                if (userInfo == null)
                {
                    return "";
                }

                ToLog("userInfo.WXAccessToken:" + userInfo.WXAccessToken, "D:\\getjsapiconfig.txt");
                if (string.IsNullOrWhiteSpace(userInfo.WXAccessToken))
                {
                    userInfo.WXAccessTokenExpireTicks = 0;
                }
                ToLog("userInfo.WXAccessTokenExpireTicks:" + userInfo.WXAccessTokenExpireTicks, "D:\\getjsapiconfig.txt");
                if (DateTime.Now.Ticks < userInfo.WXAccessTokenExpireTicks - (long)1800 * 10000000)
                {
                    ToLog("userInfo.WXAccessToken2:" + userInfo.WXAccessToken, "D:\\getjsapiconfig.txt");
                    return userInfo.WXAccessToken;
                }
                ToLog("到期前30分钟刷新accesstoken");
                //到期前30分钟刷新accesstoken
                //
                //
                Common.HttpInterFace webRequest = new Common.HttpInterFace();//
                string parm = string.Format("grant_type=client_credential&appid={0}&secret={1}", userInfo.WeixinAppId, userInfo.WeixinAppSecret);
                string requestResult = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/token", System.Text.Encoding.GetEncoding("utf-8"));
                WeixinAccessToken wxAccToken = Common.JSONHelper.JsonToModel<WeixinAccessToken>(requestResult);

                if (wxAccToken.errcode != 0)
                {
                    return string.Empty;
                }
                userInfo.WXAccessToken = wxAccToken.access_token;
                userInfo.WXAccessTokenExpireTicks = DateTime.Now.Ticks + (long)wxAccToken.expires_in * 10000000;

                int updateResultCount = Update(userInfo, string.Format("WXAccessToken='{0}',WXAccessTokenExpireTicks={1}", userInfo.WXAccessToken, userInfo.WXAccessTokenExpireTicks), string.Format("AutoID={0}", userInfo.AutoID));
                if (updateResultCount == 0)
                {
                    return string.Empty;
                }
                return userInfo.WXAccessToken;
            }
            #endregion

        }
        ///// <summary>
        ///// 获取AccessToken
        ///// </summary>
        ///// <param name="requestResult"></param>
        ///// <returns></returns>
        //public string GetAccessToken(out string requestResult)
        //{
        //    UserInfo userInfo = this.Get<UserInfo>(string.Format("UserId = '{0}'", WebsiteOwner));
        //    requestResult = "";
        //    if (string.IsNullOrEmpty(userInfo.WXAccessToken))
        //    {
        //        userInfo.WXAccessTokenExpireTicks = 0;
        //    }
        //    if (!BLL.GetCheck()) return "";

        //    //到期前30分钟刷新accesstoken
        //    if (DateTime.Now.Ticks < userInfo.WXAccessTokenExpireTicks - (long)1800 * 10000000)
        //    {
        //        return userInfo.WXAccessToken;
        //    }

        //    Common.HttpInterFace webRequest = new Common.HttpInterFace();//
        //    string parm = string.Format("grant_type=client_credential&appid={0}&secret={1}", userInfo.WeixinAppId, userInfo.WeixinAppSecret);
        //    requestResult = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/token", System.Text.Encoding.GetEncoding("utf-8"));
        //    WeixinAccessToken wxAccToken = Common.JSONHelper.JsonToModel<WeixinAccessToken>(requestResult);
        //    if (wxAccToken.errcode != 0)
        //    {
        //        return string.Empty;
        //    }
        //    userInfo.WXAccessToken = wxAccToken.access_token;
        //    userInfo.WXAccessTokenExpireTicks = DateTime.Now.Ticks + (long)wxAccToken.expires_in * 10000000;

        //    int updateResultCount = Update(userInfo, string.Format("WXAccessToken='{0}',WXAccessTokenExpireTicks={1}", userInfo.WXAccessToken, userInfo.WXAccessTokenExpireTicks), string.Format("AutoID={0}", userInfo.AutoID));
        //    if (updateResultCount == 0)
        //    {
        //        return string.Empty;
        //    }
        //    return userInfo.WXAccessToken;

        //}

        /// <summary>
        /// 获取企业微信号 AccessToken
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="corpSecret"></param>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public WeixinAccessToken GetWXQiyeAccessToken(string corpId, string corpSecret, out bool isSuccess)
        {
            isSuccess = false;
            Common.HttpInterFace webRequest = new Common.HttpInterFace();//
            string parm = string.Format("corpid={0}&corpsecret={1}", corpId, corpSecret);
            string requestResult = webRequest.GetWebRequest(parm, "https://qyapi.weixin.qq.com/cgi-bin/gettoken", System.Text.Encoding.GetEncoding("utf-8"));
            var returnResult = Common.JSONHelper.JsonToModel<WeixinAccessToken>(requestResult);
            if (!string.IsNullOrEmpty(returnResult.access_token))
            {
                isSuccess = true;
            }
            return returnResult;

        }

        /// <summary>
        /// 获取推广二维码
        /// </summary>
        /// <param name="url">二维码地址</param>
        /// <param name="userId">账户</param>
        /// <param name="type">类型 空表示分销 channel表示渠道</param>
        public void GetDistributionWxQrcodeLimit(out string url, string userId = "", string type = "", string websiteOwner = "")
        {

            ToLog("GetDistributionWxQrcodeLimit 开始获取微信分销推广二维码：userid" + userId);

            url = string.Empty;
            UserInfo userInfo = null;

            if (string.IsNullOrWhiteSpace(userId))
            {
                ToLog("GetDistributionWxQrcodeLimit 无userId");
                userInfo = GetCurrentUserInfo();
            }
            else
            {
                ToLog("GetDistributionWxQrcodeLimit 有userid");
                userInfo = userBll.GetUserInfo(userId, websiteOwner);

            }

            ToLog("GetDistributionWxQrcodeLimit 获取 currUser 完毕,WebsiteOwner:" + userInfo.WebsiteOwner);

            if (!string.IsNullOrWhiteSpace(userInfo.DistributionWxQrcodeLimitUrl) && userInfo.DistributionWxQrcodeLimitUrl != "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" && type == "")
            {
                url = userInfo.DistributionWxQrcodeLimitUrl;
                ToLog("直接返回用户对象里的链接地址：" + url);
                return;
            }
            if (!string.IsNullOrWhiteSpace(userInfo.DistributionWxQrcodeLimitUrlChannel) && userInfo.DistributionWxQrcodeLimitUrlChannel != "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" && type == "channel")
            {
                url = userInfo.DistributionWxQrcodeLimitUrlChannel;
                ToLog("直接返回用户对象里的链接地址：" + url);
                return;
            }
            ToLog("GetDistributionWxQrcodeLimit 开始获取accessToken");
            var accessToken = GetAccessToken(userInfo.WebsiteOwner);
            ToLog("GetDistributionWxQrcodeLimit 获取accessToken完毕");

            var reqUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken;

            string preStr = "DistributionCode_";

            switch (type)
            {
                case "channel"://渠道
                    preStr = "ChannelCode_";
                    break;
                default:
                    break;
            }
            dynamic data = new
            {
                action_name = "QR_LIMIT_STR_SCENE",
                action_info = new
                {
                    scene = new
                    {
                        scene_str = preStr + userInfo.UserID
                    }
                }
            };

            Common.HttpInterFace http = new HttpInterFace();

            var resp = http.PostWebRequest(JsonConvert.SerializeObject(data), reqUrl, Encoding.UTF8);

            ToLog("微信请求返回：" + resp);

            if (!string.IsNullOrWhiteSpace(resp))
            {
                WeixinQrcodeTicketResp weixinQrcodeTiket = JsonConvert.DeserializeObject<WeixinQrcodeTicketResp>(resp);

                if (weixinQrcodeTiket != null)
                {
                    switch (type)
                    {
                        case "channel":
                            userInfo.DistributionWxQrcodeLimitUrlChannel = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(weixinQrcodeTiket.ticket);
                            url = userInfo.DistributionWxQrcodeLimitUrlChannel;
                            break;
                        default:
                            userInfo.DistributionWXQrcodeLimitTicket = weixinQrcodeTiket.ticket;
                            userInfo.DistributionWxQrcodeLimitUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(userInfo.DistributionWXQrcodeLimitTicket);
                            url = userInfo.DistributionWxQrcodeLimitUrl;
                            break;
                    }

                    Update(userInfo);
                }

            }

        }


        /// <summary>
        /// 获取推广二维码
        /// </summary>
        /// <param name="url">二维码地址</param>
        /// <param name="userId">账户</param>
        /// <param name="type">类型 空表示分销 channel表示渠道</param>
        public void GetDistributionWxQrcodeLimit(out string url, UserInfo userInfo = null, string type = "")
        {

            ToLog("GetDistributionWxQrcodeLimit 开始获取微信分销推广二维码：userid" + userInfo.UserID);

            url = string.Empty;




            ToLog("GetDistributionWxQrcodeLimit 获取 currUser 完毕,WebsiteOwner:" + userInfo.WebsiteOwner);

            if (!string.IsNullOrWhiteSpace(userInfo.DistributionWxQrcodeLimitUrl) && userInfo.DistributionWxQrcodeLimitUrl != "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" && type == "")
            {
                url = userInfo.DistributionWxQrcodeLimitUrl;
                ToLog("直接返回用户对象里的链接地址：" + url);
                return;
            }
            if (!string.IsNullOrWhiteSpace(userInfo.DistributionWxQrcodeLimitUrlChannel) && userInfo.DistributionWxQrcodeLimitUrlChannel != "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" && type == "channel")
            {
                url = userInfo.DistributionWxQrcodeLimitUrlChannel;
                ToLog("直接返回用户对象里的链接地址：" + url);
                return;
            }
            ToLog("GetDistributionWxQrcodeLimit 开始获取accessToken");
            var accessToken = GetAccessToken(userInfo.WebsiteOwner);
            ToLog("GetDistributionWxQrcodeLimit 获取accessToken完毕");

            var reqUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken;

            string preStr = "DistributionCode_";

            switch (type)
            {
                case "channel"://渠道
                    preStr = "ChannelCode_";
                    break;
                default:
                    break;
            }
            dynamic data = new
            {
                action_name = "QR_LIMIT_STR_SCENE",
                action_info = new
                {
                    scene = new
                    {
                        scene_str = preStr + userInfo.UserID
                    }
                }
            };

            Common.HttpInterFace http = new HttpInterFace();

            var resp = http.PostWebRequest(JsonConvert.SerializeObject(data), reqUrl, Encoding.UTF8);

            ToLog("微信请求返回：" + resp);

            if (!string.IsNullOrWhiteSpace(resp))
            {
                WeixinQrcodeTicketResp weixinQrcodeTiket = JsonConvert.DeserializeObject<WeixinQrcodeTicketResp>(resp);

                if (weixinQrcodeTiket != null)
                {
                    switch (type)
                    {
                        case "channel":
                            userInfo.DistributionWxQrcodeLimitUrlChannel = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(weixinQrcodeTiket.ticket);
                            url = userInfo.DistributionWxQrcodeLimitUrlChannel;
                            break;
                        default:
                            userInfo.DistributionWXQrcodeLimitTicket = weixinQrcodeTiket.ticket;
                            userInfo.DistributionWxQrcodeLimitUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(userInfo.DistributionWXQrcodeLimitTicket);
                            url = userInfo.DistributionWxQrcodeLimitUrl;
                            break;
                    }

                    Update(userInfo);
                }

            }

        }




        /// <summary>
        /// 生成微信永久二维码
        /// </summary>
        /// <param name="sceneStr"></param>
        /// <returns></returns>
        public string GetWxQrcodeLimit(string sceneStr)
        {

            WXQrCode record = Get<WXQrCode>(string.Format("Scene='{0}'", sceneStr));
            if (record != null)
            {
                return record.QrCodeUrl;
            }

            var accessToken = GetAccessToken();
            var reqUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken;
            dynamic data = new
            {
                action_name = "QR_LIMIT_STR_SCENE",
                action_info = new
                {
                    scene = new
                    {
                        scene_str = sceneStr
                    }
                }
            };

            Common.HttpInterFace http = new HttpInterFace();
            var resp = http.PostWebRequest(JsonConvert.SerializeObject(data), reqUrl, Encoding.UTF8);
            if (!string.IsNullOrWhiteSpace(resp))
            {
                WeixinQrcodeTicketResp weixinQrcodeTiket = JsonConvert.DeserializeObject<WeixinQrcodeTicketResp>(resp);

                if (weixinQrcodeTiket != null)
                {
                    record = new WXQrCode();
                    record.WebsiteOwner = WebsiteOwner;
                    record.Scene = sceneStr;
                    record.QrCodeType = "WeiXinLimitScene";
                    record.QrCodeUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(weixinQrcodeTiket.ticket);
                    Add(record);
                    return record.QrCodeUrl;

                }

            }
            return "";


        }


        /// <summary>
        /// 生成微信临时二维码
        /// </summary>
        /// <param name="sceneStr">二维码字符串</param>
        /// <returns></returns>
        public string GetWxQrcodeTemp(int sceneId)
        {


            var accessToken = GetAccessToken();
            var reqUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken;
            dynamic data = new
            {
                action_name = "QR_SCENE",
                expire_seconds = 2592000,
                action_info = new
                {
                    scene = new
                    {
                        scene_id = sceneId
                    }
                }
            };

            Common.HttpInterFace http = new HttpInterFace();
            var resp = http.PostWebRequest(JsonConvert.SerializeObject(data), reqUrl, Encoding.UTF8);
            if (!string.IsNullOrWhiteSpace(resp))
            {
                WeixinQrcodeTicketResp weixinQrcodeTiket = JsonConvert.DeserializeObject<WeixinQrcodeTicketResp>(resp);

                if (weixinQrcodeTiket != null)
                {
                    return "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(weixinQrcodeTiket.ticket);

                }

            }
            return "";


        }
        ///// <summary>
        ///// 获取AccessToken (缓存方式)
        ///// </summary>
        ///// <param name="appId">appId</param>
        ///// <param name="appSecret">appSecret</param>
        ///// <param name="isSuccess">是否成功获取access_token标识</param>
        ///// <returns></returns>
        //public string GetAccessTokenCache(string appId, string appSecret, out bool isSuccess)
        //{

        //    if (DataCache.GetCache("access_token") != null)//有缓存,直接把缓存中的access_token 返回
        //    {
        //        string access_token = DataCache.GetCache("access_token").ToString();
        //        if (!string.IsNullOrEmpty(access_token))
        //        {
        //            isSuccess = true;
        //            return access_token;
        //        }
        //        else
        //        {
        //            isSuccess = false;
        //            return "缓存 access_token为空";

        //        }
        //    }
        //    else//创建access_token 并缓存
        //    {
        //        var WeixinAccessToken = GetAccessToken(appId, appSecret);
        //        if (!string.IsNullOrEmpty(WeixinAccessToken.access_token))
        //        {

        //            DataCache.SetCache("access_token", WeixinAccessToken.access_token, DateTime.MaxValue, new TimeSpan(0, 0, 7200));
        //            isSuccess = true;
        //            return WeixinAccessToken.access_token;
        //        }
        //        else
        //        {
        //            isSuccess = false;
        //            return GetCodeMessage(WeixinAccessToken.errcode);

        //        }

        //    }
        //}

        ///// <summary>
        ///// 创建AccessToken (缓存方式)
        ///// </summary>
        ///// <param name="appId">appId</param>
        ///// <param name="appSecret">appSecret</param>
        ///// <param name="isSuccess">是否成功获取access_token标识</param>
        ///// <returns></returns>
        //public string CreateAccessTokenCache(string appId, string appSecret, out bool isSuccess)
        //{


        //    var WeixinAccessToken = GetAccessToken(appId, appSecret);
        //    if (!string.IsNullOrEmpty(WeixinAccessToken.access_token))
        //    {

        //        DataCache.SetCache("access_token", WeixinAccessToken.access_token, DateTime.MaxValue, new TimeSpan(0, 0, 7200));
        //        isSuccess = true;
        //        return WeixinAccessToken.access_token;
        //    }
        //    else
        //    {
        //        isSuccess = false;
        //        return GetCodeMessage(WeixinAccessToken.errcode);

        //    }




        //}


        /// <summary>
        /// 获取AccessToken (缓存方式)
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="appSecret">appSecret</param>
        /// <param name="isSuccess">是否成功获取access_token标识</param>
        /// <returns></returns>
        //public string GetAccessTokenCacheV1(string userId, string appId, string appSecret, out bool isSuccess)
        //{

        //    if (DataCache.GetCache(string.Format("access_token{0}", userId)) != null)//有缓存,直接把缓存中的access_token 返回
        //    {
        //        string access_token = DataCache.GetCache(string.Format("access_token{0}", userId)).ToString();
        //        if (access_token != null)
        //        {

        //            isSuccess = true;
        //            return access_token;
        //        }
        //        else
        //        {
        //            isSuccess = false;
        //            return "缓存 access_token为空";

        //        }
        //    }
        //    else//创建access_token 并缓存
        //    {
        //        var WeixinAccessToken = GetAccessToken(appId, appSecret);
        //        if (!string.IsNullOrEmpty(WeixinAccessToken.access_token))
        //        {


        //            DataCache.SetCache(string.Format("access_token{0}", userId), WeixinAccessToken.access_token, DateTime.MaxValue, new TimeSpan(0, 0, 7200));
        //            isSuccess = true;
        //            return WeixinAccessToken.access_token;
        //        }
        //        else
        //        {
        //            isSuccess = false;
        //            return GetCodeMessage(WeixinAccessToken.errcode);

        //        }

        //    }

        //}

        /// <summary>
        /// 创建AccessToken (缓存方式)
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="appSecret">appSecret</param>
        /// <param name="isSuccess">是否成功获取access_token标识</param>
        /// <returns></returns>
        //public string CreateAccessTokenCacheV1(string userId, string appId, string appSecret, out bool isSuccess)
        //{
        //    var WeixinAccessToken = GetAccessToken(appId, appSecret);
        //    if (!string.IsNullOrEmpty(WeixinAccessToken.access_token))
        //    {

        //        DataCache.SetCache(string.Format("access_token{0}", userId), WeixinAccessToken.access_token, DateTime.MaxValue, new TimeSpan(0, 0, 7200));
        //        isSuccess = true;
        //        return WeixinAccessToken.access_token;
        //    }
        //    else
        //    {
        //        isSuccess = false;
        //        return GetCodeMessage(WeixinAccessToken.errcode);

        //    }


        //}



        /// <summary>
        /// 拉取微信用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WeixinUserInfo GetWeixinUserInfo(string userId, string appId, string appSecret, string openId)
        {

            WeixinUserInfo model = new WeixinUserInfo();
            string accessToken = GetAccessToken();
            if (accessToken != string.Empty)
            {
                Common.HttpInterFace webRequest = new Common.HttpInterFace();//
                string parm = string.Format("access_token={0}&openid={1}&lang=zh_CN", accessToken, openId);
                string result = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/user/info", System.Text.Encoding.GetEncoding("utf-8"));

                if (!result.Contains("errcode"))
                {
                    return Common.JSONHelper.JsonToModel<WeixinUserInfo>(result);
                }

            }
            return model;

        }

        /// <summary>
        /// 拉取微信信息到用户信息里面
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public UserInfo GetWeixinInfoToUserInfo(string userId, string appId, string appSecret, string openId)
        {
            UserInfo userModel = new BLLUser("").GetUserInfoByOpenId(openId);

            WeixinUserInfo wxInfo = GetWeixinUserInfo(userId, appId, appSecret, openId);

            userModel.WXNickname = wxInfo.NickName;
            userModel.WXHeadimgurl = wxInfo.HeadImgUrl;
            userModel.WXCity = wxInfo.City;
            userModel.WXCountry = wxInfo.Country;
            userModel.WXProvince = wxInfo.Province;
            userModel.WXSex = wxInfo.Sex;

            Update(userModel, string.Format(" WXNickname='{0}',WXHeadimgurl='{1}',WXCity='{2}',WXCountry='{3}',WXProvince='{4}',WXSex='{5}'", userModel.WXNickname, userModel.WXHeadimgurl, userModel.WXCity, userModel.WXCountry, userModel.WXProvince, userModel.WXSex), string.Format(" AutoID={0}", userModel.AutoID));

            return userModel;
        }

        /// <summary>
        /// 生成微信客户端菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuStr"></param>
        /// <returns></returns>
        public WeixinAccessToken CreateWeixinClientMenu(string accessToken, string menuStr)
        {

            string postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", accessToken);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(postUrl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postData = Encoding.GetEncoding("UTF-8").GetBytes(menuStr);
            request.ContentLength = postData.Length;

            Stream newStream = request.GetRequestStream();
            newStream.Write(postData, 0, postData.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            return Common.JSONHelper.JsonToModel<WeixinAccessToken>(content);


        }

        /// <summary>
        /// 客服接口 向用户发送消息，返回服务器结果
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuStr"></param>
        /// <returns></returns>
        private string SendKeFuMessage(string accessToken, string msg)
        {
            return PostMPRequest(accessToken, msg, string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", accessToken));
        }

        /// <summary>
        /// 客服接口 Post 公众号请求
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="menuStr"></param>
        /// <returns></returns>
        private string PostMPRequest(string accessToken, string msg, string postUrl)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(postUrl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postData = Encoding.GetEncoding("UTF-8").GetBytes(msg);
            request.ContentLength = postData.Length;

            Stream newStream = request.GetRequestStream();
            newStream.Write(postData, 0, postData.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();//得到结果
            return result;
        }

        /// <summary>
        /// 客服接口 向用户发送文本消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuStr"></param>
        /// <returns></returns>
        public string SendKeFuMessageText(string accessToken, string openId, string text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.Append("\"msgtype\":\"text\",");
            sb.Append("\"text\":");
            sb.Append("{");
            sb.AppendFormat("\"content\":\"{0}\"", text);
            sb.Append("}");
            sb.Append("}");

            return SendKeFuMessage(accessToken, sb.ToString());
        }

        /// <summary>
        /// 客服接口 向用户发送图文消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuStr"></param>
        /// <returns></returns>
        public string SendKeFuMessageImageText(string accessToken, string openId, List<WeiXinArticle> articleList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.Append("\"msgtype\":\"news\",");
            sb.Append("\"news\":");
            sb.Append("{");
            sb.Append("\"articles\":[");

            for (int i = 0; i < articleList.Count; ++i)
            {
                sb.Append("{");
                sb.AppendFormat("\"title\":\"{0}\",", articleList[i].Title);
                sb.AppendFormat("\"description\":\"{0}\",", articleList[i].Description);
                sb.AppendFormat("\"url\":\"{0}\",", articleList[i].Url);
                sb.AppendFormat("\"picurl\":\"{0}\",", articleList[i].PicUrl);
                sb.Append("}");
                if (i != articleList.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append("]");
            sb.Append("}");
            sb.Append("}");

            return SendKeFuMessage(accessToken, sb.ToString());
        }

        /// <summary>
        /// 客服接口 向用户列表发送图文消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuStr"></param>
        /// <returns></returns>
        public int SendKeFuMessageImageText(string accessToken, List<string> openIdList, List<WeiXinArticle> articleList)
        {
            foreach (string openId in openIdList)
            {
                SendKeFuMessageImageText(accessToken, openId, articleList);
            }
            return openIdList.Count;
        }

        /// <summary>
        /// 客服接口 向用户发送图片消息
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public string SendKeFuMessageImage(string accessToken, string openId, string mediaId)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.Append("\"msgtype\":\"image\",");
            sb.Append("\"image\":");
            sb.Append("{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("}");
            sb.Append("}");

            return SendKeFuMessage(accessToken, sb.ToString());
        }

        /// <summary>
        /// 客服接口 向用户发送语音消息
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public string SendKeFuMessageVoice(string accessToken, string openId, string mediaId)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.Append("\"msgtype\":\"voice\",");
            sb.Append("\"voice\":");
            sb.Append("{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("}");
            sb.Append("}");

            return SendKeFuMessage(accessToken, sb.ToString());
        }

        ///// <summary>
        ///// 客服接口 向userId的所有用户群发图文消息  OLD
        ///// 返回 openid，和发送结果状态
        ///// </summary>
        ///// <param name="access_token"></param>
        ///// <param name="menuStr"></param>
        ///// <returns></returns>
        //public ReturnValue BroadcastKeFuMessageImageText(string websiteOwner, List<WeiXinArticle> articleList)
        //{
        //    BLLUser bllUser = new BLLUser();
        //    ReturnValue rv = new ReturnValue();
        //    int totalcount = 0;
        //    int successCount = 0;
        //    try
        //    {
        //        Dictionary<string, string> openidDic = new Dictionary<string, string>();
        //        WeixinFollowers model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(GetAccessToken(websiteOwner), string.Empty));
        //        while (model.count > 0)
        //        {
        //            Dictionary<string, object> DicOpenId = (Dictionary<string, object>)model.data;
        //            object[] openidArry = (object[])DicOpenId.First().Value;
        //            foreach (var openid in openidArry)
        //            {

        //                UserInfo UserInfo = bllUser.GetUserInfoByOpenId(openid.ToString());
        //                if ((UserInfo != null) && (UserInfo.LastLoginDate != null))
        //                {
        //                    if ((DateTime.Now - (UserInfo.LastLoginDate)).TotalHours <= 48)//超过48小时不与公众号互动的不发送
        //                    {

        //                        string rtnStr = SendKeFuMessageImageText(GetAccessToken(websiteOwner), openid.ToString(), articleList);
        //                        if (rtnStr.Contains("{\"errcode\":0,"))
        //                        {
        //                            ++successCount;
        //                        }
        //                        openidDic.Add(openid.ToString(), rtnStr);
        //                        System.Threading.Thread.Sleep(50);

        //                    }
        //                }

        //            }
        //            model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(GetAccessToken(websiteOwner), model.next_openid));

        //        }
        //        totalcount = model.total;
        //        DateTime dt = DateTime.Now;
        //        string serialNum = this.GetGUID(TransacType.WXBroadcast);
        //        foreach (KeyValuePair<string, string> status in openidDic)
        //        {
        //            WXBroadcastHistory broadHis = new WXBroadcastHistory();
        //            broadHis.OpenId = status.Key;
        //            broadHis.StatusMsg = status.Value;
        //            broadHis.SerialNum = serialNum;
        //            broadHis.WebsiteOwner = websiteOwner;
        //            broadHis.InsertDate = dt;
        //            this.Add(broadHis);

        //        }
        //    }
        //    catch (Exception)
        //    {


        //    }
        //    rv.Code = totalcount;
        //    rv.Msg = string.Format("共向 {0} 人群发，成功到达 {1} 人", totalcount, successCount);
        //    return rv;
        //}


        ///// <summary>
        ///// 客服接口 群发图文信息 OLD
        ///// </summary>
        ///// <param name="websiteOwner"></param>
        ///// <param name="articleList"></param>
        ///// <param name="sourceIds"></param>
        ///// <returns></returns>
        //public ReturnValue BroadcastKeFuMessageImageText(string websiteOwner, List<WeiXinArticle> articleList, string sourceIds)
        //{
        //    BLLUser bllUser = new BLLUser();
        //    ReturnValue rv;
        //    Dictionary<string, string> openidDic = new Dictionary<string, string>();
        //    int successCount = 0;
        //    WeixinFollowers model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(GetAccessToken(websiteOwner), string.Empty));
        //    try
        //    {
        //        while (model.count > 0)
        //        {
        //            Dictionary<string, object> DicOpenId = (Dictionary<string, object>)model.data;
        //            object[] openidArry = (object[])DicOpenId.First().Value;
        //            foreach (var openid in openidArry)
        //            {
        //                UserInfo UserInfo = bllUser.GetUserInfoByOpenId(openid.ToString());
        //                if ((UserInfo != null) &&(UserInfo.LastLoginDate != null))
        //                {
        //                    if ((DateTime.Now - (UserInfo.LastLoginDate)).TotalHours <= 48)//超过48小时不与公众号互动的不发送
        //                    {
        //                        string rtnStr = SendKeFuMessageImageText(GetAccessToken(websiteOwner), openid.ToString(), articleList);
        //                        if (rtnStr.Contains("{\"errcode\":0,"))
        //                        {
        //                            successCount++;
        //                        }
        //                        openidDic.Add(openid.ToString(), rtnStr);
        //                        System.Threading.Thread.Sleep(50);
        //                    }
        //                }
        //            }
        //            model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(GetAccessToken(websiteOwner), model.next_openid));

        //        }
        //        DateTime dt = DateTime.Now;
        //        string serialNum = this.GetGUID(TransacType.WXBroadcast);
        //        foreach (KeyValuePair<string, string> status in openidDic)
        //        {
        //            WXBroadcastHistory broadHis = new WXBroadcastHistory();
        //            broadHis.OpenId = status.Key;
        //            broadHis.StatusMsg = status.Value;
        //            broadHis.SerialNum = serialNum;
        //            broadHis.WebsiteOwner = websiteOwner;
        //            broadHis.InsertDate = dt;
        //            this.Add(broadHis);

        //        }
        //    }
        //    catch (Exception)
        //    {


        //    }
        //    rv.Code = model.count;
        //    rv.Msg = string.Format("共向 {0} 人群发，成功到达 {1} 人", model.count, successCount);
        //    //更新发送成功数
        //    Update(new WeixinMsgSourceInfo(), string.Format("SendSuccessCount+={0}", successCount), string.Format("SourceID in ({0})", sourceIds));

        //    //更新发送成功数
        //    return rv;
        //}

        /// <summary>
        /// 客服接口 群发图文信息
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="articleList"></param>
        /// <param name="sourceIds"></param>
        /// <returns></returns>
        public void BroadcastKeFuMessageImageText(string websiteOwner, List<WeiXinArticle> articleList, string sourceIds)
        {
            BLLUser bllUser = new BLLUser();
            SMSPlanInfo plan = new SMSPlanInfo();
            plan.PlanID = sourceIds;
            plan.SubmitDate = DateTime.Now;
            plan.PlanType = (int)BLLJIMP.Enums.SMSPlanType.WxNews;
            //plan.SendContent = "";
            //plan.SendFrom = "自动回复图文发送任务";
            plan.SubmitDate = DateTime.Now;
            plan.Title = "群发图文消息";
            foreach (var item in articleList)
            {
                plan.SendContent += articleList[0].Title;
                plan.SendContent += "<br/>";
            }

            plan.Url = "";
            plan.UsePipe = "none";
            plan.ProcStatus = 2;
            plan.WebsiteOwner = websiteOwner;
            if (Add(plan))
            {
                plan = Get<SMSPlanInfo>(string.Format(" PlanId='{0}' Order By AutoID DESC", sourceIds));
                //
                int successCount = 0;
                int pageIndex = 1;
                int pageSize = 100;
                int batchCount = 0;
                DateTime dtNow = DateTime.Now;
                //只发送48小时内与公众号互动的粉丝
                StringBuilder sbWhere = new StringBuilder();
                sbWhere.AppendFormat(" WebSiteOwner='{0}' and LastLoginDate IS NOT NULL AND datediff( hour, LastLoginDate, '{1}' ) <=48 And WXOpenId IS NOT NULL", websiteOwner, dtNow.ToString("yyyy-MM-dd HH:mm"));
                int totalCount = bllUser.GetCount<UserInfo>(sbWhere.ToString());
                try
                {
                    do
                    {

                        var userList = bllUser.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), "AutoID ASC");
                        batchCount = userList.Count;
                        foreach (var user in userList)
                        {

                            string rtnStr = SendKeFuMessageImageText(GetAccessToken(websiteOwner), user.WXOpenId, articleList);
                            WXBroadcastHistory broadHis = new WXBroadcastHistory();
                            broadHis.OpenId = user.WXOpenId;
                            broadHis.StatusMsg = rtnStr;
                            broadHis.SerialNum = sourceIds;
                            broadHis.WebsiteOwner = websiteOwner;
                            broadHis.InsertDate = DateTime.Now;

                            if (rtnStr.Contains("{\"errcode\":0,"))
                            {
                                successCount++;
                                broadHis.Status = 1;
                                plan.SuccessCount++;
                            }
                            else
                            {
                                plan.FailCount++;
                            }


                            Add(broadHis);
                            System.Threading.Thread.Sleep(100);

                        }
                        pageIndex++;

                    } while (batchCount > 0);

                }
                catch (Exception ex)
                {
                    //try
                    //{
                    //    using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("gb2312")))
                    //    {
                    //        sw.WriteLine(string.Format("群发客服消息错误:{0}\t{1}", DateTime.Now.ToString(), ex.Message));
                    //    }
                    //}
                    //catch { }

                }
                //更新发送成功数
                //Update(new WeixinMsgSourceInfo(), string.Format("SendSuccessCount+={0}", successCount), string.Format("SourceID in ({0})", sourceIds));
                plan.ProcStatus = 3;
                Update(plan);
                //更新发送成功数



                //

            }



        }

        /// <summary>
        /// 企业微信号向用户发送消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuStr"></param>
        /// <returns></returns>
        public string PostMessageQiye(string accessToken, string msg)
        {

            string postUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", accessToken);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(postUrl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            //ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postData = Encoding.GetEncoding("UTF-8").GetBytes(msg);
            request.ContentLength = postData.Length;

            Stream newStream = request.GetRequestStream();
            newStream.Write(postData, 0, postData.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            return content;


        }



        /// <summary>
        /// 发送文字信息-企业微信号
        /// </summary>
        /// <param name="corpId"></param>
        /// <param name="corpSecret"></param>
        /// <param name="agentId"></param>
        /// <param name="userIds"></param>
        /// <param name="content"></param>
        public void SendMessageTextQiye(string corpId, string corpSecret, string agentId, string userIds, string content)
        {
            bool isSuccess;
            WeixinAccessToken model = GetWXQiyeAccessToken(corpId, corpSecret, out isSuccess);
            if (isSuccess)
            {
                string msg = CreateTextStrMessageQiye(userIds, content, agentId);
                PostMessageQiye(model.access_token, msg);


            }



        }



        /// <summary>
        /// 创建企业微信号文本信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="content"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public string CreateTextStrMessageQiye(string userIds, string content, string agentId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", userIds);
            sb.Append("\"msgtype\":\"text\",");
            sb.AppendFormat("\"agentid\": \"{0}\",", agentId);
            sb.Append("\"text\":");
            sb.Append("{");
            sb.AppendFormat("\"content\":\"{0}\"", content);
            sb.Append("}");
            sb.Append(" \"safe\":\"0\" ");
            sb.Append("}");
            return sb.ToString();

        }




        /// <summary>
        /// 删除微信自定义菜单
        /// </summary>
        /// <param name="accesToken"></param>
        /// <returns></returns>
        public string DeleteWeixinClientMenu(string accessToken)
        {

            Common.HttpInterFace webRequest = new Common.HttpInterFace();//
            string parm = string.Format("access_token={0}", accessToken);
            string result = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/menu/delete", System.Text.Encoding.GetEncoding("utf-8"));
            var model = Common.JSONHelper.JsonToModel<WeixinAccessToken>(result);
            return GetCodeMessage(model.errcode);

        }

        /// <summary>
        /// 获取返回消息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetCodeMessage(int code)
        {
            var value = CodeList.SingleOrDefault(p => p.Key == code).Value;
            if (value == null)
            {
                value = "错误码" + code.ToString();
            }
            return value;

        }

        /// <summary>
        /// 上传多媒体文件到微信服务器
        /// 注:上传的多媒体文件有格式和大小限制，如下：
        /// 图片（image）: 128K，只支持JPG格式
        /// 语音（voice）：256K，播放长度不超过60s，支持AMR\MP3格式
        /// 视频（video）：1MB，支持MP4格式
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// 注:媒体文件在后台保存时间为3天，即3天后media_id失效。
        /// </summary>
        /// <param name="accessToken">access_token 调用接口凭证</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="filePath">文件的本地物理路径:如:E:\file.jpg</param>
        /// <returns>返回处理结果字符串</returns>
        public string UploadFileToWeixin(string accessToken, string type, string filePath)
        {

            List<FormItem> list = new List<FormItem>();
            list.Add(new FormItem() { Name = "access_token", ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.Text, Value = accessToken });
            list.Add(new FormItem() { Name = "type", Value = type, ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.Text });
            list.Add(new FormItem() { Name = "media", Value = filePath, ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.File });
            return MediaOperate.PostFormData(list, "http://file.api.weixin.qq.com/cgi-bin/media/upload");


        }
        /// <summary>
        /// 上传多媒体文件到微信服务器
        /// 注:上传的多媒体文件有格式和大小限制，如下：
        /// 图片（image）: 128K，只支持JPG格式
        /// 语音（voice）：256K，播放长度不超过60s，支持AMR\MP3格式
        /// 视频（video）：1MB，支持MP4格式
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// 注:媒体文件在后台保存时间为3天，即3天后media_id失效。
        /// </summary>
        /// <param name="accessToken">access_token 调用接口凭证</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="filePath">文件的本地物理路径:如:E:\file.jpg</param>
        /// <returns>返回处理结果字符串</returns>
        public UpLoadResult UploadFileToWeixinModel(string accessToken, string type, string filePath)
        {

            List<FormItem> list = new List<FormItem>();
            list.Add(new FormItem() { Name = "access_token", ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.Text, Value = accessToken });
            list.Add(new FormItem() { Name = "type", Value = type, ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.Text });
            list.Add(new FormItem() { Name = "media", Value = filePath, ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.File });
            return Common.JSONHelper.JsonToModel<UpLoadResult>(MediaOperate.PostFormData(list, "http://file.api.weixin.qq.com/cgi-bin/media/upload"));


        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string UploadImageToWeixin(string accessToken, string type, string filePath)
        {

            List<FormItem> list = new List<FormItem>();
            list.Add(new FormItem() { Name = "access_token", ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.Text, Value = accessToken });
            list.Add(new FormItem() { Name = "type", Value = type, ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.Text });
            list.Add(new FormItem() { Name = "media", Value = filePath, ParamType = ZentCloud.BLLJIMP.Model.Weixin.MediaOperate.ParamType.File });
            return MediaOperate.PostFormData(list, "https://api.weixin.qq.com/cgi-bin/media/uploadimg");


        }



        /// <summary>
        /// 微信多媒体文件操作返回实体
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public MediaOperateReturn GetMediaOperateReturnModel(string result)
        {

            return Common.JSONHelper.JsonToModel<MediaOperateReturn>(result);

        }

        /// <summary>
        /// 微信操作返回错误消息实体
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public ErrorMessage GetErrorMessageModel(string result)
        {
            return Common.JSONHelper.JsonToModel<ErrorMessage>(result);

        }

        ///// <summary>
        ///// 加V任务
        ///// </summary>
        //public void AddVTask()
        //{

        //    while (true)
        //    {
        //        try
        //        {
        //            BLLJIMP.BLLWeixin weixinBll = new BLLJIMP.BLLWeixin("");
        //            var task = weixinBll.Get<WXAddVTask>(string.Format("Statu='0' order by AutoID ASC"));
        //            if (task != null)//有任务
        //            {
        //                UserInfo WebsiteOwnerUserInfo = weixinBll.Get<UserInfo>(string.Format("UserID='{0}'", task.WebsiteOwner));
        //                task.Statu = "1";//处理中
        //                weixinBll.Update(task);
        //                var userInfo = weixinBll.Get<UserInfo>(string.Format("UserID='{0}'", task.UserId));
        //                string imgOrgPath = MapPath(userInfo.WXHeadimgurlLocal);
        //                string imgBorderPath = MapPath("/FileUpload/WXADDV/border/" + Guid.NewGuid().ToString() + ".jpg");
        //                ZentCloud.Common.ImgWatermarkHelper im = new ZentCloud.Common.ImgWatermarkHelper();
        //                im.ImgAddBord(imgOrgPath, imgBorderPath);//加边框
        //                string imgVstr = "/FileUpload/WXADDV/" + Guid.NewGuid().ToString() + ".jpg";
        //                string imgVstrLocal = MapPath(imgVstr);
        //                im.SaveWatermark(imgBorderPath, task.FilePath, 1f, ZentCloud.Common.ImgWatermarkHelper.WatermarkPosition.RigthBottom, 0, imgVstrLocal, 0.3f, 90L);//, 0.25f

        //                string accessToken = weixinBll.GetAccessToken(WebsiteOwnerUserInfo.UserID);

        //                if (accessToken != string.Empty)
        //                {
        //                    var result = weixinBll.UploadFileToWeixin(accessToken, "image", imgVstrLocal);
        //                    if (result.Contains("errcode"))//上传出错
        //                    {
        //                        if (result.Contains("invalid meida size"))//尺寸太大重新上传
        //                        {
        //                            string imgOrgPath1 = MapPath(userInfo.WXHeadimgurlLocal);
        //                            string imgBorderPath1 = MapPath("/FileUpload/WXADDV/border/" + Guid.NewGuid().ToString() + ".jpg");
        //                            ZentCloud.Common.ImgWatermarkHelper im1 = new ZentCloud.Common.ImgWatermarkHelper();
        //                            im1.ImgAddBord(imgOrgPath1, imgBorderPath1);//加边框
        //                            string imgVstr1 = "/FileUpload/WXADDV/" + Guid.NewGuid().ToString() + ".jpg";
        //                            string imgVstrLocal1 = MapPath(imgVstr1);
        //                            im1.SaveWatermark(imgBorderPath1, task.FilePath, 1f, ZentCloud.Common.ImgWatermarkHelper.WatermarkPosition.RigthBottom, 0, imgVstrLocal1, 0.3f, 50L);//, 0.25f

        //                            string re = weixinBll.UploadFileToWeixin(accessToken, "image", imgVstrLocal1);
        //                            MediaOperateReturn mediaOperateReturn = weixinBll.GetMediaOperateReturnModel(re);
        //                            var sendResult = weixinBll.SendKeFuMessageImage(accessToken, task.WeixinOpenID, mediaOperateReturn.media_id); //调用客服接口发送图片消息                              
        //                            if (sendResult.Contains("ok"))
        //                            {
        //                                task.Statu = "2";//处理成功
        //                                weixinBll.Update(task);
        //                                ToLog(string.Format("{0}ReUploadFileToWeixinSuccess:{1}\t TaskID:{2}", DateTime.Now.ToString(), sendResult, task.AutoID));
        //                            }
        //                            else
        //                            {
        //                                if (sendResult.Contains("require subscribe"))
        //                                {
        //                                    task.Statu = "-1";//
        //                                    weixinBll.Update(task);

        //                                }
        //                                ToLog(string.Format("{0}ReUploadFileToWeixinError:{1}\t TaskID:{2}", DateTime.Now.ToString(), sendResult, task.AutoID));
        //                            }


        //                        }
        //                        ToLog(string.Format("{0}UploadFileToWeixinError:{1}\t TaskID:{2}", DateTime.Now.ToString(), result, task.AutoID));

        //                    }
        //                    else//上传成功
        //                    {

        //                        MediaOperateReturn mediaOperateReturn = weixinBll.GetMediaOperateReturnModel(result);
        //                        var sendResult = weixinBll.SendKeFuMessageImage(accessToken, task.WeixinOpenID, mediaOperateReturn.media_id); //调用客服接口发送图片消息
        //                        if (sendResult.Contains("ok"))
        //                        {
        //                            task.Statu = "2";//处理成功
        //                            weixinBll.Update(task);

        //                        }
        //                        else
        //                        {
        //                            if (sendResult.Contains("require subscribe"))
        //                            {
        //                                task.Statu = "-1";//
        //                                weixinBll.Update(task);

        //                            }
        //                            else
        //                            {
        //                                task.Statu = "0";//
        //                                weixinBll.Update(task);

        //                            }
        //                        }
        //                        ToLog(string.Format("{0}WeixinAddVResult:{1}\t TaskID:{2}", DateTime.Now.ToString(), sendResult, task.AutoID));

        //                    }



        //                }
        //                else
        //                {
        //                    task.Statu = "0";//
        //                    weixinBll.Update(task);
        //                    ToLog(string.Format("{0}\t acctoken noval", DateTime.Now.ToString()));
        //                }







        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ToLog(string.Format("{0}WeixinAddVException:{1}", DateTime.Now.ToString(), ex.ToString()));
        //        }

        //        System.Threading.Thread.Sleep(10000);

        //    }
        //}


        /// <summary>
        /// 向客服微信号发送问题 任务
        /// </summary>
        public void SendMessageToKeFuTask()
        {

            try
            {

                BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin("");
                BLLUser bllUser = new BLLUser("");
                var task = bllWeixin.Get<WXKeFuQuestionReceive>(string.Format("Status='0' order by QuestionID ASC"));
                if (task != null)//有任务
                {

                    //Model.UserInfo websiteOwnerInfo =
                    //    bllUser.GetUserInfo(
                    //        (bllUser.GetUserInfoByOpenIdClient(task.FromWeixinOpenId)
                    //    ).WebsiteOwner);



                    task.Status = "1";
                    bllWeixin.Update(task);
                    string message = ""; ;
                    string prompt = "回复请按以下格式\nK:编号:文字/语音/图片";//客服回复提示
                    ToLog("task.MsgType " + task.MsgType);
                    if (!string.IsNullOrEmpty(task.MsgType))
                    {
                        task.MsgType = task.MsgType.TrimEnd();
                        switch (task.MsgType)
                        {
                            case "voice"://声音
                                message = string.Format("[编号{0}]\n{1} 发来语音:\n{2}", task.QuestionID, string.IsNullOrEmpty(task.WXNickName) ? "无昵称" : task.WXNickName, prompt);
                                break;
                            case "image"://图片
                                message = string.Format("[编号{0}]\n{1} 发来图片:\n{2}", task.QuestionID, string.IsNullOrEmpty(task.WXNickName) ? "无昵称" : task.WXNickName, prompt);
                                break;
                            default:
                                break;
                        }


                    }
                    else
                    {
                        //文本消息
                        message = string.Format("[编号{0}]\n{1} 发来消息:\n{2}\n{3}", task.QuestionID, string.IsNullOrEmpty(task.WXNickName) ? "无昵称" : task.WXNickName, task.QuestionContent, prompt);
                    }

                    string accessToken = bllWeixin.GetAccessToken(task.WebsiteOwner);
                    if (!string.IsNullOrEmpty(accessToken))
                    {

                        foreach (var kefu in GetList<WXKeFu>(string.Format(" WebsiteOwner='{0}'", task.WebsiteOwner)))//给所有客服发送消息
                        {
                            string sendTextResult = SendKeFuMessageText(accessToken, kefu.WeiXinOpenID, message);//文本消息结果
                            //task.QuestionContent += sendTextResult;
                            //bllUser.Update(task);
                            if (sendTextResult.Contains("ok"))
                            {
                                if (task.MsgType != null)
                                {
                                    switch (task.MsgType)
                                    {

                                        #region 给客服发送语音信息
                                        case "voice"://声音
                                            string sendVoiceResult = SendKeFuMessageVoice(accessToken, kefu.WeiXinOpenID, task.MediaId);
                                            if (sendVoiceResult.Contains("ok"))
                                            {
                                                task.Status = "2";//已经向客服微信号发送消息
                                                bllWeixin.Update(task);
                                                ToLog(string.Format("{0}\tSendVoiceToKeFuSuccess:{1}", DateTime.Now.ToString(), sendVoiceResult));

                                            }
                                            else
                                            {
                                                ToLog(string.Format("{0}\tSendVoiceToKeFuErro:{1}", DateTime.Now.ToString(), sendVoiceResult));
                                            }
                                            break;
                                        #endregion

                                        #region 给客服发送图片信息
                                        case "image"://图片
                                            string sendImageResult = SendKeFuMessageImage(accessToken, kefu.WeiXinOpenID, task.MediaId);
                                            if (sendImageResult.Contains("ok"))
                                            {
                                                task.Status = "2";//已经向客服微信号发送消息
                                                bllWeixin.Update(task);
                                                ToLog(string.Format("{0}\tSendImageToKeFuSuccess:{1}", DateTime.Now.ToString(), sendImageResult));

                                            }
                                            else
                                            {
                                                ToLog(string.Format("{0}\tSendImageToKeFuErro:{1}", DateTime.Now.ToString(), sendImageResult));
                                            }
                                            break;
                                        #endregion


                                        default:
                                            break;
                                    }


                                }
                                else
                                {
                                    task.Status = "2";//已经向客服微信号发送消息
                                    bllWeixin.Update(task);

                                }


                            }
                            else
                            {
                                //task.Status = "0";//
                                //task.QuestionContent += sendTextResult;
                                //bllWeixin.Update(task);
                                ToLog(string.Format("{0}SendMessageToKeFuResultErro:{1}", DateTime.Now.ToString(), sendTextResult));
                            }
                        }

                    }
                    else
                    {
                        task.QuestionContent += "accesstoken为空";
                        bllWeixin.Update(task);

                    }
                }
            }
            catch (Exception ex)
            {
                ToLog(string.Format("{0}SendMessageToKeFuTaskException:{1}", DateTime.Now.ToString(), ex.ToString()));
            }

            ////System.Threading.Thread.Sleep(60000);

            //}



        }
        /// <summary>
        /// 向用户微信号发送问题回复
        /// </summary>
        public void SendMessageToUserTask()
        {

            try
            {

                BLLWeixin bllWeixin = new BLLWeixin("");
                BLLUser bllUser = new BLLUser("");
                var task = bllWeixin.Get<WXKeFuQuestionReply>(string.Format("Status='0' order by ReplyID ASC"));
                if (task != null)//有任务
                {
                    //Model.UserInfo websiteOwnerUserInfo =
                    //    bllUser.GetUserInfo(
                    //        (bllUser.GetUserInfoByOpenIdClient(task.ReplyWeixinOpenId)
                    //    ).WebsiteOwner);


                    task.Status = "1";
                    bllWeixin.Update(task);

                    var question = bllWeixin.Get<WXKeFuQuestionReceive>(string.Format("QuestionID={0}", task.QuestionId));//问题
                    string message = ""; ;
                    if (!string.IsNullOrEmpty(task.MsgType))
                    {
                        task.MsgType = task.MsgType.TrimEnd();
                        switch (task.MsgType)
                        {
                            case "voice":
                                message = string.Format("客服发给您语音");
                                break;
                            case "image":
                                message = string.Format("客服发给您图片");
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        message = string.Format("客服已经回复了您:\n[消息]:{0}\n[回复]:{1}", question.QuestionContent, task.ReplyContent);
                    }

                    string accessToken = bllWeixin.GetAccessToken(task.WebsiteOwner);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        string sendTextResult = SendKeFuMessageText(accessToken, question.FromWeixinOpenId, message);
                        if (sendTextResult.Contains("ok"))
                        {

                            if (task.MsgType != null)
                            {
                                switch (task.MsgType)
                                {
                                    #region 语音
                                    case "voice":
                                        string sendVoiceResult = SendKeFuMessageVoice(accessToken, question.FromWeixinOpenId, task.MediaId);
                                        if (sendVoiceResult.Contains("ok"))
                                        {
                                            task.Status = "2";//已经向客服微信号发送消息
                                            bllWeixin.Update(task);
                                            ToLog(string.Format("{0}\tSendVoiceToUserSuccess:{1}", DateTime.Now.ToString(), sendVoiceResult));

                                        }
                                        else
                                        {
                                            ToLog(string.Format("{0}\tSendVoiceToUserErro:{1}", DateTime.Now.ToString(), sendVoiceResult));
                                        }
                                        break;
                                    #endregion

                                    #region 图片
                                    case "image":
                                        string sendImageResult = SendKeFuMessageImage(accessToken, question.FromWeixinOpenId, task.MediaId);
                                        if (sendImageResult.Contains("ok"))
                                        {
                                            task.Status = "2";//已经向客服微信号发送消息
                                            bllWeixin.Update(task);
                                            ToLog(string.Format("{0}\tSendImageToUserSuccess:{1}", DateTime.Now.ToString(), sendImageResult));

                                        }
                                        else
                                        {
                                            ToLog(string.Format("{0}\tSendImageToUserErro:{1}", DateTime.Now.ToString(), sendImageResult));
                                        }
                                        break;
                                    #endregion

                                    default:
                                        break;
                                }

                            }
                            else
                            {
                                task.Status = "2";//已经向用户微信号发送消息
                                bllWeixin.Update(task);

                            }


                        }
                        else
                        {
                            //task.Status = "0";//
                            //task.ReplyContent += sendTextResult;
                            // bllUser.Update(task);
                            ToLog(string.Format("{0}SendMessageToUserResultErro:{1}", DateTime.Now.ToString(), sendTextResult));

                        }

                        #region 给其它客服发送回复消息
                        //给其它客服发送消息
                        List<WXKeFu> kefuList = bllUser.GetList<WXKeFu>(string.Format(" WebsiteOwner='{0}'", task.WebsiteOwner)).Where(p => p.WeiXinOpenID != task.ReplyWeixinOpenId).ToList();
                        foreach (var kefu in kefuList)
                        {

                            UserInfo replyKefuInfo = bllUser.GetUserInfo(task.ReplyWeixinOpenId, task.WebsiteOwner);//回复客服的个人信息
                            string replyKefuShowName = bllUser.GetUserDispalyName(replyKefuInfo);//回复客服的显示名称
                            message = string.Format("{0}已经回复[编号{1}]:\n{2}\n[回复内容]:{3}", replyKefuShowName, question.QuestionID, question.QuestionContent, task.ReplyContent);
                            SendKeFuMessageText(accessToken, kefu.WeiXinOpenID, message);

                        }
                        #endregion



                    }
                    else
                    {
                        //task.Status = "0";
                        // weixinBll.Update(task);
                    }




                }
            }
            catch (Exception ex)
            {
                ToLog(string.Format("{0}SendMessageToUserTaskException:{1}", DateTime.Now.ToString(), ex.ToString()));
            }


            //System.Threading.Thread.Sleep(60000);
        }

        /// <summary>
        /// 下载微信公众号文章
        /// </summary>
        /// <param name="url">公众号文章链接</param>
        /// <param name="idContent">不为空时仅取对应id块内html</param>
        /// <returns></returns>
        public string DownLoadMpArticle(string url)
        {

            BLLJuActivity bllJuActivity = new BLLJuActivity();
            HttpWebResponse res = null;
            string html = "";
            try
            {
                if (!url.StartsWith("http"))
                {
                    url = "http://" + url;
                }
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/Html,application/xhtml+XML,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_3_5 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Mobile/13G36 MicroMessenger/6.3.30 NetType/WIFI Language/zh_CN";
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                html = reader.ReadToEnd();
                html = System.Text.RegularExpressions.Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                reader.Close();
            }
            catch
            {

            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            #region 替换微信远程图片为本地路径
            System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.MatchCollection matches = regImg.Matches(html);
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                String name = match.ToString();
                string reomoteurl = match.Groups["imgUrl"].Value;
                //微信特殊处理"<img src=\"http://res.wx.qq.com/mmbizwap/zh_CN/htmledition/images/big_loading19d82d.gif\" onerror=\"this.parentNode.removeChild(this)\" data-src=\"http://mmbiz.qpic.cn/mmbiz/lybKlMLvO2WzibKRgibMx82Zsg2Lhts4dibN9JNicHeTrzUJfiarSia2GaIjtlPyictzVxPmEUH4iaeTuXszSbO7CbTibdQ/0\" />"
                if (name.Contains("data-src="))
                {
                    string tmp = Common.StringHelper.CutByStarTag(name, "data-src=", true);// name.Substring(name.IndexOf("data-src"));

                    tmp = tmp.Substring(1, tmp.IndexOf(" ") - 2);
                    reomoteurl = tmp;
                }
                //微信的三个域名
                //mmsns.qpic.cn
                //mmbiz.qpic.cn
                //res.wx.qq.com
                if (reomoteurl.StartsWith("http://mmsns.qpic.cn/") || reomoteurl.StartsWith("http://mmbiz.qpic.cn/") || reomoteurl.StartsWith("http://res.wx.qq.com/"))//替换微信官方图片地址为本地 
                {
                    html = html.Replace(name, string.Format("<img src=\"{0}?x-oss-process=image/resize,w_800,limit_1\">", bllJuActivity.DownLoadImageToOss(reomoteurl)));
                    //html = html.Replace(name, string.Format("<img src=\"http://{0}:{1}{2}\">", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, bllJuActivity.DownLoadRemoteImage(reomoteurl)));
                }
            }

            #endregion
            return html;

        }
        //}


        ///// <summary>
        ///// 清理加V临时图片任务
        ///// </summary>
        //public void ClearAddVTempImageTask()
        //{
        //    while (true)
        //    {


        //    try
        //    {
        //        DateTime dtNow = DateTime.Now;
        //        if (dtNow.Hour >= 3 && dtNow.Hour <= 6)
        //        {
        //            //string boderForlder = "D:\\wwwroot\\JubitFriendShare\\pub\\FileUpload\\WXADDV\\border\\";
        //            string boderForlder = MapPath("/FileUpload/WXADDV/border/");
        //            string[] boderFileList = System.IO.Directory.GetFiles(boderForlder);

        //            //string addvForlder="D:\\wwwroot\\JubitFriendShare\\pub\\FileUpload\\WXADDV\\";
        //            string addvForlder = MapPath("/FileUpload/WXADDV/");
        //            string[] addvFileList = System.IO.Directory.GetFiles(addvForlder);

        //            foreach (var item in boderFileList)
        //            {
        //                System.IO.File.Delete(item);
        //            }
        //            foreach (var item in addvFileList)
        //            {
        //                System.IO.File.Delete(item);
        //            }
        //            ToLog(string.Format("{0}删除加V临时图片完成", DateTime.Now.ToString()));
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        ToLog(string.Format("{0}删除加V临时图片异常:{1}", DateTime.Now.ToString(), ex.Message));
        //    }
        //    System.Threading.Thread.Sleep(7200000);
        //    }

        //}

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    //strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                    strPath = strPath.TrimStart('\\');
                }
                var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);

                return path.Replace("\\", "\\\\");
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        private void ToLog(string message)
        {

            using (StreamWriter sw = new StreamWriter("D:\\kefuLog.txt", true, Encoding.UTF8))
            {
                sw.WriteLine(message);
            }

        }


        /// <summary>
        /// 获取 Jsapi_Ticket
        /// </summary>
        /// <param name="isSuccess">是否获取成功</param>
        /// <param name="repeatCount">重置的次数，如果发现站点accesstoken无效的时候，需要重置，最多只能重置5次，再多再看</param>
        /// <returns></returns>
        public string GetJsapiTicket(out bool isSuccess, int repeatCount = 0)
        {

            isSuccess = false;
            string ticket = "";
            var currWebSiteInfo = GetWebsiteInfoModelFromDataBase();
            //ToLog("currWebSiteInfo:" + JsonConvert.SerializeObject(currWebSiteInfo), "D:\\getjsapiconfig.txt");
            #region 获取缓存的Ticke
            ToLog("currWebSiteInfo.WeiXinJsapi_TicketGetTime:" + currWebSiteInfo.WeiXinJsapi_TicketGetTime, "D:\\getjsapiconfig.txt");
            if (currWebSiteInfo.WeiXinJsapi_TicketGetTime != null)
            {
                if ((DateTime.Now - (DateTime)currWebSiteInfo.WeiXinJsapi_TicketGetTime).TotalSeconds <= 7000)
                {
                    //默认7200秒过期 获取时改为7000秒过期
                    if (!string.IsNullOrEmpty(currWebSiteInfo.WeiXinJsapi_Ticket))
                    {
                        isSuccess = true;
                        return currWebSiteInfo.WeiXinJsapi_Ticket;
                    }
                }
            }
            #endregion

            #region 获取新的Ticket
            ToLog("获取新的Ticket ", "D:\\getjsapiconfig.txt");
            string accessToken = GetAccessToken();
            ToLog(" accessToken:" + accessToken, "D:\\getjsapiconfig.txt");
            if (!string.IsNullOrEmpty(accessToken))
            {
                string source = ZentCloud.Common.MySpider.GetPageSourceForUTF8(string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accessToken));
                ToLog("source:" + source, "D:\\getjsapiconfig.txt");
                JsapiTicketModel jsApiTicketModel = Common.JSONHelper.JsonToModel<JsapiTicketModel>(source);
                ToLog("jsApiTicketModel.errcode:" + jsApiTicketModel.errcode, "D:\\getjsapiconfig.txt");
                if (jsApiTicketModel.errcode.Equals(0))
                {
                    //获取Ticket成功 存储到数据库
                    currWebSiteInfo.WeiXinJsapi_Ticket = jsApiTicketModel.ticket;
                    currWebSiteInfo.WeiXinJsapi_TicketGetTime = DateTime.Now;
                    if (Update(currWebSiteInfo))
                    {
                        isSuccess = true;
                    }
                    return jsApiTicketModel.ticket;

                }
                else if (jsApiTicketModel.errcode.Equals(42001))
                {
                    if (repeatCount >= 5)//最多重复5次重置
                    {
                        ToLog("最多重复5次重置", "D:\\getjsapiconfig.txt");
                        return ticket;
                    }
                    //需要重置当前站点的 accesstoken 和时间
                    UserInfo currWebSiteOwnerInfo = userBll.GetCurrWebSiteUserInfo();

                    currWebSiteOwnerInfo.WXAccessToken = "";
                    currWebSiteOwnerInfo.WXAccessTokenExpireTicks = 0;

                    if (Update(currWebSiteOwnerInfo, " WXAccessToken = '',WXAccessTokenExpireTicks=0 ",
                        string.Format(" AutoID={0} ", currWebSiteOwnerInfo.AutoID)) > 0)
                    {
                        repeatCount++;
                        ToLog("进行第" + repeatCount.ToString() + "次重置", "D:\\getjsapiconfig.txt");
                        ticket = GetJsapiTicket(out isSuccess, repeatCount);
                        ToLog("重置后取得ticket:" + ticket, "D:\\getjsapiconfig.txt");
                        return ticket;
                    }

                }


            }
            #endregion

            return ticket;
        }
        /// <summary>
        /// 获取卡券Ticket
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public string GetCardTicket(out bool isSuccess)
        {
            isSuccess = false;
            string ticket = "";
            var currWebSiteInfo = GetWebsiteInfoModelFromDataBase();

            #region 获取缓存的Ticke
            if (currWebSiteInfo.WeiXinCard_TicketGetTime != null)
            {
                if ((DateTime.Now - (DateTime)currWebSiteInfo.WeiXinCard_TicketGetTime).TotalSeconds <= 7000)
                {
                    //默认7200秒过期 获取时改为7000秒过期
                    if (!string.IsNullOrEmpty(currWebSiteInfo.WeiXinCard_Ticket))
                    {
                        isSuccess = true;
                        return currWebSiteInfo.WeiXinCard_Ticket;
                    }
                }
            }
            #endregion
            #region 获取新的Ticket
            string accessToken = GetAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                string source = ZentCloud.Common.MySpider.GetPageSourceForUTF8(string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=wx_card", accessToken));

                JsapiTicketModel jsApiTicketModel = Common.JSONHelper.JsonToModel<JsapiTicketModel>(source);
                if (jsApiTicketModel.errcode.Equals(0))
                {
                    //获取Ticket成功 存储到数据库
                    currWebSiteInfo.WeiXinCard_Ticket = jsApiTicketModel.ticket;
                    currWebSiteInfo.WeiXinCard_TicketGetTime = DateTime.Now;
                    Update(currWebSiteInfo);
                    isSuccess = true;
                    return jsApiTicketModel.ticket;

                }

            }
            #endregion
            return ticket;
        }


        /// <summary>
        /// 获取 微信 JSAPI 配置
        /// </summary>
        /// <param name="url">用户访问的url</param>
        /// <returns></returns>
        public string GetJSAPIConfig(string url, string cardId = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            }
            bool isGetTicKetSuccess;
            string ticket = GetJsapiTicket(out isGetTicKetSuccess);

            ToLog("ticket:" + ticket, "D:\\getjsapiconfig.txt");

            JsapiConfigModel config = new JsapiConfigModel();
            if (isGetTicKetSuccess)
            {
                BLLUser bllUser = new BLLUser(WebsiteOwner);



                config.appId = bllUser.GetCurrWebSiteUserInfo().WeixinAppId;
                ToLog("config.appId:" + config.appId, "D:\\getjsapiconfig.txt");
                if (bllWeixinOpen.IsAuthToOpen())
                {
                    WebsiteInfo currentWebsiteInfo = bllWeixinOpen.GetWebsiteInfoModelFromDataBase();
                    config.appId = currentWebsiteInfo.AuthorizerAppId;
                }
                config.timestamp = GetTimeStamp();
                ToLog("config.timestamp:" + config.timestamp, "D:\\getjsapiconfig.txt");
                config.nonceStr = CreateNoncestr();
                ToLog("config.nonceStr:" + config.nonceStr, "D:\\getjsapiconfig.txt");

                config.signature = SHA1(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, config.nonceStr, config.timestamp, url)).ToLower();

                if (!string.IsNullOrWhiteSpace(cardId))
                {
                    bool getCardTicketResult = false;
                    string cardTicket = GetCardTicket(out getCardTicketResult);

                    if (getCardTicketResult)
                    {
                        config.cardCode = Guid.NewGuid().ToString();
                        config.cardTicket = cardTicket;
                        config.cardSN = string.Format("{0}{1}{2}{3}", config.timestamp, cardTicket, cardId, config.nonceStr);
                        config.cardSign = SHA1(config.cardSN).ToLower();
                    }

                }
            }

            return Common.JSONHelper.ObjectToJson(config);
        }


        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public Int64 GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        /// <summary>
        /// 生成16位随机字符串
        /// </summary>
        /// <returns></returns>
        public static String CreateNoncestr()
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }


        /// <summary>
        /// 错误代码信息
        /// </summary>
        Dictionary<int, string> CodeList = new Dictionary<int, string>()
        {
            {-1,"系统繁忙"},
            {0,"请求成功"},
            {40001,"验证失败"},
            {40002,"不合法的凭证类型"},
            {40003,"不合法的OpenID"},
            {40004,"不合法的媒体文件类型"},
            {40005,"不合法的文件类型"},
            {40006,"不合法的文件大小"},
            {40007,"不合法的媒体文件id"},
            {40008,"不合法的消息类型"},
            {40009,"不合法的图片文件大小"},
            {40010,"不合法的语音文件大小"},
            {40011,"不合法的视频文件大小"},
            {40012,"不合法的缩略图文件大小"},
            {40013,"不合法的APPID"},
            {40014,"不合法的access_token"},
            {40015,"不合法的菜单类型"},
            {40016,"不合法的按钮个数"},
            {40017,"不合法的按钮个数"},
            {40018,"不合法的按钮名字长度"},
            {40019,"不合法的按钮KEY长度"},
            {40020,"不合法的按钮URL长度"},
            {40021,"不合法的菜单版本号"},
            {40022,"不合法的子菜单级数"},
            {40023,"不合法的子菜单按钮个数"},
            {40024,"不合法的子菜单按钮类型"},
            {40025,"不合法的子菜单按钮名字长度"},
            {40026,"不合法的子菜单按钮KEY长度"},
            {40027,"不合法的子菜单按钮URL长度"},
            {40028,"不合法的自定义菜单使用用户"},
            {41001,"缺少access_token参数"},
            {41002,"缺少appid参数"},
            {41003,"缺少refresh_token参数"},
            {41004,"缺少secret参数"},
            {41005,"缺少多媒体文件数据"},
            {41006,"缺少media_id参数"},
            {41007,"缺少子菜单数据"},
            {42001,"access_token超时"},
            {43001,"需要GET请求"},
            {43002,"需要POST请求"},
            {43003,"需要HTTPS请求"},
            {43004,"用户未关注"},
            {44001,"多媒体文件为空"},
            {44002,"POST的数据包为空"},
            {44003,"图文消息内容为空"},
            {45001,"多媒体文件大小超过限制"},
            {45002,"消息内容超过限制"},
            {45003,"标题字段超过限制"},
            {45004,"描述字段超过限制"},
            {45005,"链接字段超过限制"},
            {45006,"图片链接字段超过限制"},
            {45007,"语音播放时间超过限制"},
            {45008,"图文消息超过限制"},
            {45009,"接口调用超过限制"},
            {45010,"创建菜单个数超过限制"},
            {45015,"超过时间限制"},
            {46001,"不存在媒体数据"},
            {46002,"不存在的菜单版本"},
            {46003,"不存在的菜单数据"},
            {47001,"解析JSON/XML内容错误"}
        };

        /// <summary>
        /// 下载远程地址图片存到本地
        /// </summary>
        /// <param name="reomoteUrl">远程图片地址</param>
        /// <returns></returns>
        public string DownLoadRemoteImage(string reomoteUrl)
        {
            JuActivityImageMapping model = Get<JuActivityImageMapping>(string.Format("RemoteUrl='{0}'", reomoteUrl));
            if (model != null)
            {
                return model.LocalPath;
            }
            else
            {
                JuActivityImageMapping newModel = new JuActivityImageMapping();
                try
                {

                    string absoluteDirectory = System.Web.HttpContext.Current.Server.MapPath("/FileUpload/ImageMapping/");
                    if (!Directory.Exists(absoluteDirectory))
                    {
                        Directory.CreateDirectory(absoluteDirectory);
                    }
                    System.Net.WebRequest wreq = System.Net.WebRequest.Create(reomoteUrl);
                    System.Net.HttpWebResponse wresp = (System.Net.HttpWebResponse)wreq.GetResponse();
                    Stream stream = wresp.GetResponseStream();
                    System.Drawing.Image img;
                    img = System.Drawing.Image.FromStream(stream);
                    string guid = Guid.NewGuid().ToString();
                    string localRelativelyPath = string.Format("/FileUpload/ImageMapping/{0}.jpg", guid);
                    img.Save(System.Web.HttpContext.Current.Server.MapPath(localRelativelyPath), System.Drawing.Imaging.ImageFormat.Jpeg);   //保存 
                    newModel.RemoteUrl = reomoteUrl;
                    newModel.LocalPath = localRelativelyPath;
                    if (Add(newModel))
                    {
                        return localRelativelyPath;
                    }
                    else
                    {
                        return "";
                    }


                }
                catch (Exception)
                {

                    return "";
                }


            }


        }

        ///// <summary>
        ///// 根据文章ID返回文章Html
        ///// </summary>
        ///// <param name="juActivityID">文章ID</param>
        ///// <returns>Html</returns>
        //public string GetArticleHtmlByArticleID(int juActivityID)
        //{
        //    try
        //    {
        //        ///最终输出的Html
        //        string Html = string.Empty; ;
        //        JuActivityInfo articleInfo = Get<JuActivityInfo>(string.Format("JuActivityID={0}", juActivityID));
        //        if (articleInfo != null)//检查文章是否存在
        //        {
        //            if (!articleInfo.IsDelete.Equals(1))//检查文章是否删除
        //            {
        //                if (articleInfo.IsHide.Equals(0))
        //                {

        //                    UserInfo userInfo = Get<UserInfo>(string.Format("UserID='{0}'", articleInfo.UserID));//文章发布者信息
        //                    SystemSet systemset = Get<SystemSet>("");//系统配置信息

        //                    #region 更新打开人次
        //                    if (articleInfo.OpenCount == null)
        //                    {
        //                        articleInfo.OpenCount = 0;
        //                    }
        //                    articleInfo.OpenCount++;
        //                    Update(articleInfo);


        //                    #endregion

        //                    #region 内部链接
        //                    if (articleInfo.IsByWebsiteContent.Equals(0))//内部链接
        //                    {
        //                        if (articleInfo.ArticleTemplate.Equals("1"))// 微信官方模板
        //                        {
        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/weixin/ArticleTemplate/jubit1.htm"), Encoding.UTF8);

        //                        }

        //                        if (articleInfo.ArticleTemplate.Equals("2"))// 聚比特模板
        //                        {

        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit2.htm"), Encoding.UTF8);



        //                        }
        //                        if (articleInfo.IsSignUpJubit > 0)
        //                        {
        //                            //读取个性化模板
        //                            Html = Html.Replace("$JUTP-TPSignForm$", Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/TPSignForm.htm"), Encoding.UTF8));
        //                        }
        //                        else
        //                        {
        //                            Html = Html.Replace("$JUTP-TPSignForm$", "");
        //                        }



        //                    }
        //                    #endregion

        //                    #region 外部链接
        //                    else//外部链接
        //                    {
        //                        //下载外部链接源代码
        //                        Html = Common.MySpider.GetPageSourceForUTF8(articleInfo.ActivityWebsite);

        //                    }
        //                    #endregion

        //                    #region 追加报名表单
        //                    //追加报名表单                
        //                    // 符合条件的情况
        //                    //1:使用外部链接并使用报名.
        //                    //2:使用内部链接并使用微信官方模板并使用报名.
        //                    //if ((articleInfo.IsByWebsiteContent.Equals(1) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)))//使用外部链接并使用报名.
        //                    //    ||
        //                    //    //使用内部链接并使用微信官方模板并使用报名.
        //                    //    (articleInfo.IsByWebsiteContent.Equals(0) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)) && articleInfo.ArticleTemplate.Equals(1))
        //                    //    )

        //                    if (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2))//有报名
        //                    {


        //                        StringBuilder sbCheckForm = new StringBuilder();//追加检查form
        //                        StringBuilder sbAppend = new StringBuilder();//Html后追加的html
        //                        StringBuilder sbSignInHtml = new StringBuilder();//报名表单 格式<tr></tr>
        //                        //FORMDATA
        //                        ActivityInfo planactivityInfo = Get<ActivityInfo>(string.Format("ActivityID='{0}'", articleInfo.SignUpActivityID));
        //                        List<ActivityFieldMappingInfo> activityFieldMappingInfoList = GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' Order by ExFieldIndex ASC", planactivityInfo.ActivityID));
        //                        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"txtName\" >姓名:</label></td></tr>");
        //                        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"Name\" id=\"txtName\" value=\"\" placeholder=\"姓名\" style=\"width:100%;\"></td></tr>");

        //                        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"txtPhone\"  >手机号码:</label></td></tr>");
        //                        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"Phone\" id=\"txtPhone\" value=\"\" placeholder=\"手机号码\" style=\"width:100%;\"></td></tr>");
        //                        //手机号

        //                        //其它报名字段
        //                        foreach (ActivityFieldMappingInfo item in activityFieldMappingInfoList)
        //                        {

        //                            if (item.FieldType != 1)//普通字段
        //                            {

        //                                sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"K{0}\">{1}</label></td></tr>", item.ExFieldIndex, item.MappingName);
        //                                sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"K{0}\" id=\"K{0}\" value=\"\" placeholder=\"{1}\" style=\"width:100%;\"/> </td></tr>", item.ExFieldIndex, item.MappingName);

        //                            }
        //                            else//微信推广字段
        //                            {
        //                                sbSignInHtml.AppendFormat("<tr><td><input type=\"hidden\" name=\"K{0}\" id=\"K{0}\" value=\"$CCWXTG-LINKID$\" /></td></tr> ", item.ExFieldIndex);
        //                            }
        //                            if (item.FieldIsNull.Equals(1))
        //                            {

        //                                sbCheckForm.AppendFormat("if (!document.getElementById(\"K{0}\").value)", item.ExFieldIndex);

        //                                sbCheckForm.AppendLine("{");
        //                                sbCheckForm.AppendFormat("alert(\"请输入{0}\");", item.MappingName);
        //                                sbCheckForm.AppendLine(" return false;");
        //                                sbCheckForm.AppendLine("}");



        //                            }


        //                        }
        //                        //其它报名字段

        //                        //其它报名信息
        //                        //sbSignInHtml.AppendLine("<div  style=\"margin-top: 10px;\">");
        //                        sbSignInHtml.AppendLine("<tr><td>");
        //                        sbSignInHtml.AppendFormat("<input id=\"activityID\" type=\"hidden\" value=\"{0}\" name=\"ActivityID\">", planactivityInfo.ActivityID);//活动ID
        //                        sbSignInHtml.AppendFormat("<input id=\"loginName\" type=\"hidden\" value=\"{0}\" name=\"LoginName\">", ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID));//外部登录名
        //                        sbSignInHtml.AppendFormat("<input id=\"loginPwd\" type=\"hidden\" value=\"{0}\" name=\"LoginPwd\">", ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password));//外部登录密码

        //                        //不允许重复字段
        //                        if (planactivityInfo.DistinctKeys != null)
        //                        {
        //                            if (!string.IsNullOrEmpty(planactivityInfo.DistinctKeys))
        //                            {
        //                                sbSignInHtml.AppendFormat("<input  type=\"hidden\" value=\"{0}\" name=\"DistinctKeys\">", planactivityInfo.DistinctKeys);
        //                            }
        //                        }
        //                        sbSignInHtml.AppendLine("</td></tr>");
        //                        //不允许重复字段

        //                        if (articleInfo.IsByWebsiteContent.Equals(1))//外部链接报名
        //                        {
        //                            sbAppend.Append(Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/weixin/wx_externalsignuptemplate.htm"), Encoding.UTF8));//外部报名表单模板
        //                            sbAppend = sbAppend.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //                            if (Html.Contains("</body>"))
        //                            {
        //                                Html = Html.Replace("</body>", string.Format("{0}</body>", sbAppend));
        //                            }
        //                            else
        //                            {
        //                                Html += sbAppend.ToString();
        //                            }

        //                            Html = Html.Replace("$CCWX-CHECKFORM$", sbCheckForm.ToString());


        //                        }
        //                        else
        //                        {

        //                            //内部链接报名
        //                            Html = Html.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //                        }

        //                    }


        //                    //追加报名表单 
        //                    #endregion

        //                    #region 替换微信远程图片为本地路径

        //                    System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //                    System.Text.RegularExpressions.MatchCollection matches = regImg.Matches(Html);
        //                    foreach (System.Text.RegularExpressions.Match match in matches)
        //                    {
        //                        String name = match.ToString();
        //                        string reomoteurl = match.Groups["imgUrl"].Value;
        //                        if (reomoteurl.StartsWith("http://mmsns.qpic.cn/") || reomoteurl.StartsWith("http://mmbiz.qpic.cn/") || reomoteurl.StartsWith("http://res.wx.qq.com/"))//替换微信官方图片地址为本地 
        //                        {
        //                            Html = Html.Replace(name, string.Format("<img src=\"http://{0}:{1}{2}\">", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, DownLoadRemoteImage(reomoteurl)));

        //                        }


        //                    }

        //                    #endregion

        //                    #region 替换文章标签

        //                    if (Html.Contains("$CCWX-ARTICLEIMAGE$"))//替换分享图片地址
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEIMAGE$", string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, articleInfo.ThumbnailsPath));
        //                    }

        //                    if (Html.Contains("$CCWXCALLBACKURL$"))//替换回调地址 
        //                    {
        //                        Html = Html.Replace("$CCWXCALLBACKURL$", string.Format("http://{0}:{1}{2}?id={3}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.FilePath, articleInfo.JuActivityID));
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLETITLE$"))//替换标题
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLETITLE$", articleInfo.ActivityName);//替换标题

        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLETIME$"))//替换时间
        //                    {
        //                        if (articleInfo.IsSignUpJubit.Equals(0))//普通文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLETIME$", string.Format("{0:f}", articleInfo.CreateDate));//替换时间
        //                        }
        //                        else//活动文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLETIME$", string.Format("{0:f}", articleInfo.ActivityStartDate));//替换时间
        //                        }


        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEADDRESS$"))//替换地址
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEADDRESS$", articleInfo.ActivityAddress == null || articleInfo.ActivityAddress == "" ? userInfo.WeixinPublicName : articleInfo.ActivityAddress);//替换地址
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLECONTENT$"))//替换内容
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLECONTENT$", articleInfo.ActivityDescription);//替换内容
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEOPENCOUNT$"))//替换打开人次
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEOPENCOUNT$", articleInfo.OpenCount.ToString());//替换打开人次
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEUPCOUNT$"))//替换赞人数
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEUPCOUNT$", articleInfo.UpCount.ToString());//替换赞人数
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEID$"))//替换文章ID
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEID$", articleInfo.JuActivityID.ToString());//替换文章ID
        //                    }

        //                    #endregion



        //                }
        //                else
        //                {
        //                    Html = "该文章不显示";
        //                }
        //            }
        //            else
        //            {
        //                Html = "该文章已经删除";
        //            }
        //        }
        //        else
        //        {
        //            Html = "不存在的文章";
        //        }


        //        return Html;//返回最终Html
        //    }
        //    catch (Exception ex)
        //    {

        //        return ex.ToString();
        //    }


        //}

        ///// <summary>
        ///// 检查是否是微信会员
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="wxOpenId"></param>
        ///// <returns></returns>
        //public bool CheckIsWXMember(string userId, string wxOpenId)
        //{
        //    if (GetCount<WXMemberInfo>(string.Format("UserID='{0}' and WeixinOpenID='{1}'", userId, wxOpenId)) > 0)
        //        return true;
        //    return false;
        //}

        //public string GetUserPmsGroup(string openId)
        //{


        //        UserInfo userInfo=new BLLUser("").GetUserInfoByOpenId(openId);
        //        if (userInfo==null)
        //        {
        //            return "无";
        //        }
        //        List<long> hfPmsIdList = new List<long>() { 130273, 130334, 130335, 130388 };
        //        long hfPmsId = 0;
        //        List<UserPmsGroupRelationInfo> pmsGroup = new BLL().GetList<UserPmsGroupRelationInfo>(string.Format(" UserID = '{0}' ", UserID));

        //        foreach (var item in pmsGroup)
        //        {
        //            if (hfPmsIdList.Contains(item.GroupID))
        //            {
        //                hfPmsId = item.GroupID;
        //                break;
        //            }
        //        }
        //        switch (hfPmsId)
        //        {
        //            case 130273:
        //                return "管理员";
        //            case 130334:
        //                return "游客";
        //            case 130335:
        //                return "正式学员";
        //            case 130388:
        //                return "教师";
        //            default:
        //                return " 无";
        //        }


        //}

        ///// <summary>
        ///// 同步所有微信粉丝数量
        ///// </summary>
        ///// <param name="websiteOwner"></param>
        ///// <returns></returns>
        //public string SynchronousAllFollowers(string websiteOwner, string appId, string appSecret)
        //{


        //    BLLUser bllUser = new BLLUser();
        //    int totalCount = 0;
        //    //ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
        //    try
        //    {

        //        string accessToken = GetAccessToken(websiteOwner);
        //        if (accessToken != string.Empty)
        //        {

        //            string nextOpenId = "";
        //            do
        //            {
        //                WeixinFollowers model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(accessToken, nextOpenId));
        //                if (model.count.Equals(0))
        //                {
        //                    break;
        //                }
        //                Dictionary<string, object> dicOpenId = (Dictionary<string, object>)model.data;
        //                object[] openIdArry = (object[])dicOpenId.First().Value;
        //                foreach (var item in openIdArry)
        //                {
        //                    if (item != null)
        //                    {
        //                        if (!string.IsNullOrEmpty(item.ToString()))
        //                        {
        //                            if (GetCount<WeixinFollowersInfo>(string.Format("WebsiteOwner='{0}' and OpenId='{1}'", websiteOwner, item.ToString())) == 0)
        //                            {
        //                                var flowerInfo = GetWeixinUserInfo(accessToken, item.ToString());
        //                                if (flowerInfo != null)
        //                                {
        //                                    if (!string.IsNullOrEmpty(flowerInfo.OpenId))
        //                                    {
        //                                        flowerInfo.WebsiteOwner = websiteOwner;
        //                                        //flowerInfo.UserPmsGroup = GetUserPmsGroup(flowerInfo.OpenId);
        //                                        if (Add(flowerInfo))
        //                                        {
        //                                            totalCount++;
        //                                            UserInfo userInfo = bllUser.GetUserInfoByOpenId(flowerInfo.OpenId);
        //                                            if (userInfo != null)
        //                                            {
        //                                                //userInfo.WXHeadimgurl = flowerInfo.HeadImgUrl;
        //                                                bllUser.Update(userInfo, string.Format("WXHeadimgurl='{0}'", flowerInfo.HeadImgUrl), string.Format(" AutoID={0}", userInfo.AutoID));
        //                                            }


        //                                        }
        //                                    }


        //                                }

        //                            }
        //                        }
        //                    }



        //                }
        //                nextOpenId = model.next_openid;

        //            } while (!string.IsNullOrEmpty(nextOpenId));
        //            //tran.Commit();

        //        }
        //        return string.Format("同步完成，本次同步新增了 {0} 位粉丝信息", totalCount.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        // tran.Rollback();
        //        return ex.Message;
        //    }




        //}

        ///// <summary>
        ///// 更新粉丝信息
        ///// </summary>
        ///// <param name="websiteOwner"></param>
        ///// <param name="appId"></param>
        ///// <param name="appSecret"></param>
        ///// <returns></returns>
        //public string UpdateAllFollowersInfo(string websiteOwner)
        //{

        //    int totalCount = 0;
        //    BLLUser bllUser = new BLLUser();
        //    try
        //    {
        //        string accessToken = GetAccessToken(websiteOwner);
        //        if (accessToken != string.Empty)
        //        {

        //            string nextOpenId = "";
        //            do
        //            {
        //                WeixinFollowers model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(accessToken, nextOpenId));

        //                if (model.count.Equals(0))
        //                {

        //                    break;
        //                }
        //                Dictionary<string, object> dicOpenId = (Dictionary<string, object>)model.data;
        //                object[] openIdArry = (object[])dicOpenId.First().Value;
        //                foreach (var item in openIdArry)
        //                {

        //                    var flowerInfo = GetWeixinUserInfo(accessToken, item.ToString());//粉丝信息    
        //                    if (flowerInfo == null)
        //                    {
        //                        continue;
        //                    }
        //                    UserInfo userInfo = bllUser.GetUserInfoByOpenId(flowerInfo.openid, websiteOwner);
        //                    var oldFollowerInfo = Get<BLLJIMP.Model.WeixinFollowersInfo>(string.Format("WebsiteOwner='{0}' and OpenId='{1}'", websiteOwner, item.ToString()));//旧粉丝信息

        //                    #region  同步到UserInfo表

        //                    if (userInfo == null)//用户表新增
        //                    {
        //                        userInfo = new UserInfo();
        //                        userInfo.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());//Guid
        //                        userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
        //                        userInfo.UserType = 2;
        //                        userInfo.WebsiteOwner = websiteOwner;
        //                        userInfo.Regtime = DateTime.Now;
        //                        userInfo.WXOpenId = item.ToString();
        //                        userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
        //                        userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
        //                        userInfo.LastLoginDate = DateTime.Now;
        //                        userInfo.LoginTotalCount = 1;
        //                        userInfo.WXHeadimgurl = flowerInfo.headimgurl;
        //                        userInfo.WXNickname = flowerInfo.nickname;
        //                        userInfo.WXProvince = flowerInfo.province;
        //                        userInfo.WXCity = flowerInfo.city;

        //                        bllUser.Add(userInfo);
        //                    }

        //                    #endregion



        //                    #region 新增粉丝
        //                    if (oldFollowerInfo == null)
        //                    {
        //                        BLLJIMP.Model.WeixinFollowersInfo newFollower = new Model.WeixinFollowersInfo();
        //                        newFollower.WebsiteOwner = websiteOwner;
        //                        newFollower.City = flowerInfo.city;
        //                        newFollower.Country = flowerInfo.country;
        //                        newFollower.HeadImgUrl = flowerInfo.headimgurl;
        //                        newFollower.Language = flowerInfo.language;
        //                        newFollower.NickName = flowerInfo.nickname;
        //                        newFollower.OpenId = flowerInfo.openid;
        //                        newFollower.Province = flowerInfo.province;
        //                        newFollower.Sex = flowerInfo.sex;
        //                        newFollower.Subscribe_time = flowerInfo.subscribe_time;
        //                        if (Add(newFollower))
        //                        {
        //                            totalCount++;

        //                        }


        //                    }
        //                    #endregion

        //                    #region 更新粉丝信息
        //                    else//更新粉丝信息
        //                    {

        //                        oldFollowerInfo.NickName = flowerInfo.nickname;
        //                        oldFollowerInfo.Sex = flowerInfo.sex;
        //                        oldFollowerInfo.Language = flowerInfo.language;
        //                        oldFollowerInfo.Country = flowerInfo.country;
        //                        oldFollowerInfo.Province = flowerInfo.province;
        //                        oldFollowerInfo.City = flowerInfo.city;
        //                        oldFollowerInfo.Subscribe_time = flowerInfo.subscribe_time;
        //                        oldFollowerInfo.HeadImgUrl = flowerInfo.headimgurl;
        //                        if (Update(oldFollowerInfo))
        //                        {
        //                            totalCount++;

        //                        }


        //                    }
        //                    #endregion

        //                    #region 更新用户表头像昵称 省份城市
        //                    if (userInfo != null)
        //                    {
        //                        bllUser.Update(userInfo, string.Format("WXHeadimgurl='{0}',IsWeixinFollower=1 ,WXNickname='{1}',WXProvince='{2}',WXCity='{3}'", flowerInfo.headimgurl, flowerInfo.nickname, flowerInfo.province, flowerInfo.city), string.Format(" AutoID={0}", userInfo.AutoID));
        //                    }
        //                    #endregion

        //                }
        //                //    }



        //                //}
        //                nextOpenId = model.next_openid;

        //            } while (!string.IsNullOrEmpty(nextOpenId));

        //        }
        //        return string.Format("更新粉丝信息完成，本次同步了 {0} 位粉丝信息", totalCount.ToString());
        //    }
        //    catch (Exception ex)
        //    {

        //        return ex.Message;
        //    }




        //}

        /// <summary>
        /// 更新粉丝
        /// </summary>
        /// <param name="task"></param>
        public void UpdateAllFollowersInfoTask(TimingTask task)
        {

            int totalCount = 0;
            BLLUser bllUser = new BLLUser();
            try
            {
                task.Status = 2;
                bllUser.Update(task);
                if (GetAccessToken(task.WebsiteOwner) != string.Empty)
                {

                    string nextOpenId = "";
                    do
                    {
                        WeixinFollowers model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(GetAccessToken(task.WebsiteOwner), nextOpenId));

                        if (model.count.Equals(0))
                        {

                            break;
                        }
                        Dictionary<string, object> dicOpenId = (Dictionary<string, object>)model.data;
                        object[] openIdArry = (object[])dicOpenId.First().Value;
                        foreach (var item in openIdArry)
                        {

                            var flowerInfo = GetWeixinUserInfo(GetAccessToken(task.WebsiteOwner), item.ToString());//粉丝信息    
                            if (flowerInfo == null)
                            {
                                continue;
                            }
                            UserInfo userInfo = bllUser.GetUserInfoByOpenIdClient(item.ToString());
                            var oldFollowerInfo = Get<BLLJIMP.Model.WeixinFollowersInfo>(string.Format("WebsiteOwner='{0}' and OpenId='{1}'", task.WebsiteOwner, item.ToString()));//旧粉丝信息

                            #region  同步到UserInfo表


                            //判断站点设置是否允许自动注册

                            var websiteWXUserRegType = new BLLWebSite().GetWebsiteWXUserRegType(task.WebsiteOwner);

                            if (userInfo == null)//用户表新增
                            {
                                if (websiteWXUserRegType == Enums.WebsiteWXUserRegType.AutoReg)
                                {
                                    userInfo = new UserInfo();
                                    userInfo.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());//Guid
                                    userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
                                    userInfo.UserType = 2;
                                    userInfo.WebsiteOwner = task.WebsiteOwner;
                                    userInfo.Regtime = DateTime.Now;
                                    userInfo.WXOpenId = item.ToString();
                                    //userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                                    //userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                                    userInfo.LastLoginDate = DateTime.Now;
                                    userInfo.LoginTotalCount = 1;
                                    userInfo.WXHeadimgurl = flowerInfo.headimgurl;
                                    userInfo.WXNickname = flowerInfo.nickname;
                                    userInfo.WXProvince = flowerInfo.province;
                                    userInfo.WXCity = flowerInfo.city;
                                    userInfo.IsWeixinFollower = flowerInfo.subscribe;
                                    if (userInfo.IsWeixinFollower == 1)
                                    {
                                        userInfo.SubscribeTime = bllUser.GetTime(flowerInfo.subscribe_time * 1000).ToString();
                                    }
                                    if (userInfo.IsWeixinFollower == 0)
                                    {
                                        userInfo.UnSubscribeTime = DateTime.Now.ToString();
                                    }
                                    try
                                    {
                                        bllUser.Add(userInfo);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }

                            }

                            #endregion



                            #region 新增粉丝
                            if (oldFollowerInfo == null)
                            {
                                BLLJIMP.Model.WeixinFollowersInfo newFollower = new Model.WeixinFollowersInfo();
                                newFollower.WebsiteOwner = task.WebsiteOwner;
                                newFollower.City = flowerInfo.city;
                                newFollower.Country = flowerInfo.country;
                                newFollower.HeadImgUrl = flowerInfo.headimgurl;
                                newFollower.Language = flowerInfo.language;
                                newFollower.NickName = flowerInfo.nickname;
                                newFollower.OpenId = flowerInfo.openid;
                                newFollower.Province = flowerInfo.province;
                                newFollower.Sex = flowerInfo.sex;
                                newFollower.IsWeixinFollower = flowerInfo.subscribe;
                                if (flowerInfo.subscribe == 1)
                                {
                                    newFollower.Subscribe_time = bllUser.GetTime(flowerInfo.subscribe_time * 1000).ToString();
                                }
                                if (flowerInfo.subscribe == 0)
                                {
                                    newFollower.UnSubscribeTime = DateTime.Now.ToString();
                                }

                                try
                                {


                                    if (Add(newFollower))
                                    {
                                        totalCount++;

                                    }
                                }
                                catch (Exception)
                                {


                                }

                            }
                            #endregion

                            #region 更新粉丝信息
                            else//更新粉丝信息
                            {

                                oldFollowerInfo.NickName = flowerInfo.nickname;
                                oldFollowerInfo.Sex = flowerInfo.sex;
                                oldFollowerInfo.Language = flowerInfo.language;
                                oldFollowerInfo.Country = flowerInfo.country;
                                oldFollowerInfo.Province = flowerInfo.province;
                                oldFollowerInfo.City = flowerInfo.city;
                                oldFollowerInfo.IsWeixinFollower = flowerInfo.subscribe;
                                if (flowerInfo.subscribe == 1)
                                {
                                    oldFollowerInfo.Subscribe_time = bllUser.GetTime(flowerInfo.subscribe_time * 1000).ToString();
                                }
                                if (flowerInfo.subscribe == 0)
                                {
                                    oldFollowerInfo.UnSubscribeTime = DateTime.Now.ToString();
                                }
                                oldFollowerInfo.HeadImgUrl = flowerInfo.headimgurl;

                                try
                                {
                                    if (Update(oldFollowerInfo))
                                    {
                                        totalCount++;

                                    }
                                }
                                catch (Exception)
                                {


                                }



                            }
                            #endregion

                            #region 更新用户表头像昵称
                            if (userInfo != null && userInfo.AutoID > 0)
                            {
                                try
                                {
                                    userInfo.IsWeixinFollower = flowerInfo.subscribe;
                                    if (userInfo.IsWeixinFollower == 1)
                                    {
                                        userInfo.SubscribeTime = bllUser.GetTime(flowerInfo.subscribe_time * 1000).ToString();
                                    }
                                    if (userInfo.IsWeixinFollower == 0)
                                    {
                                        userInfo.UnSubscribeTime = DateTime.Now.ToString();
                                    }


                                    bllUser.Update(userInfo, string.Format("WXHeadimgurl='{0}',IsWeixinFollower=1 ,WXNickname='{1}',WXProvince='{2}',WXCity='{3}'", flowerInfo.headimgurl, flowerInfo.nickname, flowerInfo.province, flowerInfo.city), string.Format(" AutoID={0}", userInfo.AutoID));
                                }
                                catch (Exception)//昵称单引号
                                {

                                    //ToLog(string.Format(" WXHeadimgurl='{0}',IsWeixinFollower=1 ,WXNickname='{1}'", flowerInfo.headimgurl, flowerInfo.nickname));
                                }


                            }
                            #endregion

                        }
                        //    }



                        //}
                        nextOpenId = model.next_openid;
                        System.Threading.Thread.Sleep(100);

                    } while (!string.IsNullOrEmpty(nextOpenId));

                }
                else
                {
                    task.TaskInfo += "未获取到AccessToken";

                }
                task.Status = 3;
                task.FinishDate = DateTime.Now;
                task.TaskInfo += "任务完成,同步了" + totalCount + "名粉丝信息";
                bllUser.Update(task);

            }
            catch (Exception ex)
            {
                task.Status = 3;
                task.FinishDate = DateTime.Now;
                task.TaskInfo += ex.Message;
                bllUser.Update(task);
            }




        }

        /// <summary>
        /// 更新所有站点所有粉丝信息
        /// </summary>
        /// <param name="task"></param>
        public void UpdateAllWesiteAllFollowersInfoTask()
        {
            BLLUser bllUser = new BLLUser();
            //System.Threading.Tasks.Parallel.ForEach(GetList<WebsiteInfo>(), (website) =>
            //{
            //if (!string.IsNullOrEmpty(GetAccessToken(website.WebsiteOwner)))
            foreach (var website in GetList<WebsiteInfo>())
            {
                if (!string.IsNullOrEmpty(GetAccessToken(website.WebsiteOwner)))
                {
                    TimingTask task = new TimingTask();
                    task.WebsiteOwner = website.WebsiteOwner;
                    task.InsertDate = DateTime.Now;
                    task.Status = 1;
                    task.TaskInfo = "同步粉丝";
                    task.TaskType = 3;
                    task.ScheduleDate = DateTime.Now;
                    task.TaskId = GetGUID(TransacType.CommAdd);
                    if (bllUser.Add(task))
                    {
                        task = Get<TimingTask>(string.Format("TaskId={0}", task.TaskId));
                    }
                    if (task != null)
                    {
                        int totalCount = 0;
                        try
                        {
                            task.Status = 2;
                            bllUser.Update(task);
                            //if (!string.IsNullOrEmpty(accessToken))
                            //{

                            string nextOpenId = "";
                            do
                            {
                                WeixinFollowers model = Common.JSONHelper.JsonToModel<WeixinFollowers>(GetFollower(GetAccessToken(task.WebsiteOwner), nextOpenId));

                                if (model.count.Equals(0))
                                {

                                    break;
                                }
                                Dictionary<string, object> dicOpenId = (Dictionary<string, object>)model.data;
                                object[] openIdArry = (object[])dicOpenId.First().Value;
                                foreach (var item in openIdArry)
                                {

                                    var flowerInfo = GetWeixinUserInfo(GetAccessToken(task.WebsiteOwner), item.ToString());//粉丝信息    
                                    if (flowerInfo == null)
                                    {
                                        continue;
                                    }
                                    UserInfo userInfo = bllUser.GetUserInfoByOpenIdClient(item.ToString());
                                    var oldFollowerInfo = Get<BLLJIMP.Model.WeixinFollowersInfo>(string.Format("WebsiteOwner='{0}' and OpenId='{1}'", task.WebsiteOwner, item.ToString()));//旧粉丝信息

                                    #region  同步到UserInfo表

                                    if (userInfo == null)//用户表新增
                                    {
                                        userInfo = new UserInfo();
                                        userInfo.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());//Guid
                                        userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
                                        userInfo.UserType = 2;
                                        userInfo.WebsiteOwner = task.WebsiteOwner;
                                        userInfo.Regtime = DateTime.Now;
                                        userInfo.WXOpenId = item.ToString();
                                        //userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                                        //userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                                        userInfo.LastLoginDate = DateTime.Now;
                                        userInfo.LoginTotalCount = 1;
                                        userInfo.WXHeadimgurl = flowerInfo.headimgurl;
                                        userInfo.WXNickname = flowerInfo.nickname;
                                        userInfo.WXProvince = flowerInfo.province;
                                        userInfo.WXCity = flowerInfo.city;
                                        userInfo.IsWeixinFollower = flowerInfo.subscribe;
                                        if (userInfo.IsWeixinFollower == 1)
                                        {
                                            userInfo.SubscribeTime = bllUser.GetTime(flowerInfo.subscribe_time * 1000).ToString();
                                        }
                                        if (userInfo.IsWeixinFollower == 0)
                                        {
                                            userInfo.UnSubscribeTime = DateTime.Now.ToString();
                                        }
                                        bllUser.Add(userInfo);


                                    }

                                    #endregion



                                    #region 新增粉丝
                                    if (oldFollowerInfo == null)
                                    {
                                        BLLJIMP.Model.WeixinFollowersInfo newFollower = new Model.WeixinFollowersInfo();
                                        newFollower.WebsiteOwner = task.WebsiteOwner;
                                        newFollower.City = flowerInfo.city;
                                        newFollower.Country = flowerInfo.country;
                                        newFollower.HeadImgUrl = flowerInfo.headimgurl;
                                        newFollower.Language = flowerInfo.language;
                                        newFollower.NickName = flowerInfo.nickname;
                                        newFollower.OpenId = flowerInfo.openid;
                                        newFollower.Province = flowerInfo.province;
                                        newFollower.Sex = flowerInfo.sex;
                                        newFollower.IsWeixinFollower = flowerInfo.subscribe;
                                        if (flowerInfo.subscribe == 1)
                                        {
                                            newFollower.Subscribe_time = bllUser.GetTime(flowerInfo.subscribe_time * 1000).ToString();
                                        }
                                        if (flowerInfo.subscribe == 0)
                                        {
                                            newFollower.UnSubscribeTime = DateTime.Now.ToString();
                                        }
                                        if (Add(newFollower))
                                        {
                                            totalCount++;

                                        }


                                    }
                                    #endregion

                                    #region 更新粉丝信息
                                    else//更新粉丝信息
                                    {

                                        oldFollowerInfo.NickName = flowerInfo.nickname;
                                        oldFollowerInfo.Sex = flowerInfo.sex;
                                        oldFollowerInfo.Language = flowerInfo.language;
                                        oldFollowerInfo.Country = flowerInfo.country;
                                        oldFollowerInfo.Province = flowerInfo.province;
                                        oldFollowerInfo.City = flowerInfo.city;
                                        oldFollowerInfo.IsWeixinFollower = flowerInfo.subscribe;
                                        if (flowerInfo.subscribe == 1)
                                        {
                                            oldFollowerInfo.Subscribe_time = bllUser.GetTime(flowerInfo.subscribe_time * 1000).ToString();
                                        }
                                        if (flowerInfo.subscribe == 0)
                                        {
                                            oldFollowerInfo.UnSubscribeTime = DateTime.Now.ToString();
                                        }
                                        oldFollowerInfo.HeadImgUrl = flowerInfo.headimgurl;
                                        if (Update(oldFollowerInfo))
                                        {
                                            totalCount++;

                                        }


                                    }
                                    #endregion

                                    #region 更新用户表头像昵称
                                    if (userInfo != null && userInfo.AutoID > 0)
                                    {
                                        try
                                        {
                                            bllUser.Update(userInfo, string.Format("WXHeadimgurl='{0}',IsWeixinFollower=1 ,WXNickname='{1}',WXProvince='{2}',WXCity='{3}'", flowerInfo.headimgurl, flowerInfo.nickname, flowerInfo.province, flowerInfo.city), string.Format(" AutoID={0}", userInfo.AutoID));
                                        }
                                        catch (Exception)//昵称单引号
                                        {

                                            //ToLog(string.Format(" WXHeadimgurl='{0}',IsWeixinFollower=1 ,WXNickname='{1}'", flowerInfo.headimgurl, flowerInfo.nickname));
                                        }


                                    }
                                    #endregion

                                }
                                //    }



                                //}
                                nextOpenId = model.next_openid;
                                //System.Threading.Thread.Sleep(100);

                            } while (!string.IsNullOrEmpty(nextOpenId));

                            //}
                            //else
                            //{
                            //    task.TaskInfo += "未获取到AccessToken";

                            //}
                            task.Status = 3;
                            task.FinishDate = DateTime.Now;
                            task.TaskInfo += "任务完成,同步了" + totalCount + "名粉丝信息";
                            bllUser.Update(task);





                        }
                        catch (Exception ex)
                        {
                            task.Status = 3;
                            task.FinishDate = DateTime.Now;
                            task.TaskInfo += ex.Message;
                            bllUser.Update(task);
                        }

                    }

                }
            }


            //});








        }

        /// <summary>
        /// 更新微信用户信息
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateWexinUserInfo(object obj)
        {

            System.Threading.Thread.Sleep(5000);
            UserInfo userInfo = (UserInfo)obj;
            string accessToken = GetAccessToken(userInfo.WebsiteOwner);
            var wxUserInfo = GetWeixinUserInfo(accessToken, userInfo.WXOpenId);
            if (wxUserInfo != null)
            {
                userInfo.WXHeadimgurl = wxUserInfo.headimgurl;
                userInfo.WXNickname = wxUserInfo.nickname;
                userInfo.WXProvince = wxUserInfo.province;
                userInfo.WXCity = wxUserInfo.city;
                Update(userInfo);


            }



        }

        /// <summary>
        /// 发送模板消息[批量]
        /// </summary>
        /// <param name="task"></param>
        public void SendTemplateMessageTask(TimingTask task)
        {

            int successCount = 0;
            BLLTimingTask bllTask = new BLLTimingTask();
            BLLUser bllUser = new BLLUser();
            BLLWeixin bllWeixin = new BLLWeixin();
            try
            {
                switch (task.TaskType)
                {
                    case 4://群发模板消息[批量]
                        #region 群发模板消息[批量]
                        task.Status = 2;
                        bllTask.Update(task);
                        SMSPlanInfo plan = bllUser.Get<SMSPlanInfo>(string.Format(" PlanID='{0}' ", task.TaskId));
                        plan.ProcStatus = 2;
                        bllWeixinOpen.Update(plan);

                        #region 通过AutoId发送
                        if (!string.IsNullOrEmpty(task.Receivers))
                        {
                            foreach (string autoId in task.Receivers.Split(','))
                            {

                                UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(autoId));
                                if ((userInfo != null) && (!string.IsNullOrEmpty(userInfo.WXOpenId)))
                                {

                                    if (bllWeixin.SendTemplateMessageNotifyCommTask(userInfo.WXOpenId, task.Title, task.MsgContent, task.Url, CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify), plan.PlanID, userInfo.UserID, task.WebsiteOwner))
                                    {
                                        successCount++;
                                    }

                                }
                            }

                        }
                        #endregion

                        #region 通过标签发送
                        if (!string.IsNullOrEmpty(task.Tags))
                        {
                            foreach (string tag in task.Tags.Split(','))
                            {

                                List<UserInfo> userList = bllUser.GetList<UserInfo>(string.Format(" Websiteowner='{0}' And TagName like '%{1}%'", task.WebsiteOwner, tag));

                                foreach (var userInfo in userList)
                                {
                                    if ((userInfo != null) && (!string.IsNullOrEmpty(userInfo.WXOpenId)))
                                    {
                                        if (bllWeixin.SendTemplateMessageNotifyCommTask(userInfo.WXOpenId, task.Title, task.MsgContent, task.Url, CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify), plan.PlanID, userInfo.UserID, task.WebsiteOwner))
                                        {
                                            successCount++;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        plan.SuccessCount = successCount;
                        plan.ProcStatus = 3;
                        plan.FailCount = (int)plan.SubmitCount - successCount;
                        bllWeixinOpen.Update(plan);
                        #endregion
                        break;
                    case 10://群发模板消息[全部]
                        #region 群发模板消息[全部]
                        int pageIndex = 1;
                        int pageSize = 100;
                        int count = 0;
                        do
                        {
                            string strSql = string.Format(" WebsiteOwner='{0}' AND IsWeixinFollower=1 ", task.WebsiteOwner);

                            List<WeixinFollowersInfo> followerList = bllUser.GetLit<WeixinFollowersInfo>(pageSize, pageIndex, strSql, " AutoID DESC ");//粉丝

                            foreach (var item in followerList)
                            {
                                if (!string.IsNullOrEmpty(item.OpenId))
                                {
                                    UserInfo userInfo = bllUser.GetUserInfoByOpenIdClient(item.OpenId);
                                    if ((userInfo != null) && (!string.IsNullOrEmpty(userInfo.WXOpenId)))
                                    {
                                        if (bllWeixin.SendTemplateMessageNotifyComm(userInfo, task.Title, task.MsgContent, task.Url, CommonPlatform.Helper.EnumStringHelper.ToString(BLLJIMP.Enums.BroadcastType.WXTemplateMsg_Notify), ""))
                                        {
                                            successCount++;
                                        }
                                    }
                                }
                            }
                            count = followerList.Count;
                            pageIndex++;

                        } while (count > 0);
                        #endregion
                        break;
                    default:
                        break;
                }

                task.Status = 3;
                task.FinishDate = DateTime.Now;
                task.TaskInfo += "任务完成,共向" + successCount + "人成功发送了信息";
                bllUser.Update(task);
            }
            catch (Exception ex)
            {
                task.Status = 3;
                task.FinishDate = DateTime.Now;
                task.TaskInfo += "异常" + ex.ToString();
                bllUser.Update(task);
            }

        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="nextOpenId"></param>
        /// <returns></returns>
        public string GetFollower(string accessToken, string nextOpenId)
        {

            Common.HttpInterFace webRequest = new Common.HttpInterFace();//
            string parm = string.Format("access_token={0}&next_openid={1}", accessToken, nextOpenId);
            string result = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/user/get", System.Text.Encoding.GetEncoding("utf-8"));
            return result;

        }
        /// <summary>
        /// 获取所有粉丝总数
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public int GetFollowerTotalCount(string websiteOwner)
        {
            string accessToken = GetAccessToken(websiteOwner);
            if (!string.IsNullOrEmpty(accessToken))
            {
                string result = GetFollower(accessToken, "");
                if (!string.IsNullOrEmpty(result))
                {
                    JToken jResult = JToken.Parse(result);
                    if (jResult["total"] != null && (!string.IsNullOrEmpty(jResult["total"].ToString())))
                    {
                        return int.Parse(jResult["total"].ToString());
                    }

                }
            }

            return 0;

        }

        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WeixinFollowerInfo GetWeixinUserInfo(string accessToken, string openId)
        {
            Common.HttpInterFace webRequest = new Common.HttpInterFace();//
            string parm = string.Format("access_token={0}&openid={1}&lang=zh_CN", accessToken, openId);
            string result = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/user/info", System.Text.Encoding.GetEncoding("utf-8"));

            //if (!result.Contains("errcode"))
            //{
            return Common.JSONHelper.JsonToModel<WeixinFollowerInfo>(result);
            //}


            //return model;

        }
        /// <summary>
        ///微信粉丝信息
        /// </summary>
        public class WeixinFollowerInfo
        {
            /// <summary>
            /// openid
            /// </summary>
            public string openid { get; set; }
            /// <summary>
            /// 昵称
            /// </summary>
            public string nickname { get; set; }
            /// <summary>
            /// 性别
            /// </summary>
            public int sex { get; set; }
            /// <summary>
            /// 语言
            /// </summary>
            public string language { get; set; }
            /// <summary>
            /// 城市
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// 省份
            /// </summary>
            public string province { get; set; }
            /// <summary>
            /// 国家
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// 头像
            /// </summary>
            public string headimgurl { get; set; }
            /// <summary>
            /// 订阅时间
            /// </summary>
            public long subscribe_time { get; set; }
            /// <summary>
            /// 是否已经关注
            /// </summary>
            public int subscribe { get; set; }

        }

        /// <summary>
        /// 判断用户是否是公众平台粉丝
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool IsWeixinFollower(string accessToken, string openId)
        {
            try
            {
                WeixinFollowersInfo model = new WeixinFollowersInfo();
                Common.HttpInterFace webRequest = new Common.HttpInterFace();//
                string parm = string.Format("access_token={0}&openid={1}&lang=zh_CN", accessToken, openId);
                string result = webRequest.GetWebRequest(parm, "https://api.weixin.qq.com/cgi-bin/user/info", System.Text.Encoding.GetEncoding("utf-8"));
                if (result.Contains("\"subscribe\":1"))
                {
                    return true;
                }

            }
            catch (Exception)
            {
                return false;

            }
            return false;

        }

        /// <summary>
        /// 发送模板消息:报名成功通知，返回服务器结果
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string SendTemplateMessage(string accessToken, string openId, TMSignupNotification signup)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.AppendFormat("\"template_id\":\"{0}\",", signup.TemplateId);
            sb.AppendFormat("\"url\":\"{0}\",", signup.Url);
            sb.Append("\"topcolor\":\"#FF0000\",");
            sb.Append("\"data\":{");

            sb.Append("\"first\": {");
            sb.AppendFormat("\"value\":\"{0}\",", signup.First);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keynote1\": {");
            sb.AppendFormat("\"value\":\"{0}\",", signup.Keynote1);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keynote2\": {");
            sb.AppendFormat("\"value\":\"{0}\",", signup.Keynote2);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keynote3\": {");
            sb.AppendFormat("\"value\":\"{0}\",", signup.Keynote3);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"remark\": {");
            sb.AppendFormat("\"value\":\"{0}\",", signup.Remark);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("}");

            sb.Append("}");
            sb.Append("}");

            return SendTemplateMessage(accessToken, sb.ToString());
        }

        /// <summary>
        /// 发送模板消息:积分提醒，返回服务器结果
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string SendTemplateMessage(string accessToken, string openId, TMScoreNotification score)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.AppendFormat("\"template_id\":\"{0}\",", score.TemplateId);
            sb.AppendFormat("\"url\":\"{0}\",", score.Url);
            sb.Append("\"topcolor\":\"#FF0000\",");
            sb.Append("\"data\":{");

            sb.Append("\"first\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.First);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"account\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.Account);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"time\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.Time);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"type\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.Type);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"creditchange\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.CreditChange);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"number\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.Number);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"creditname\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.CreditName);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"amount\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.Amount);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"remark\": {");
            sb.AppendFormat("\"value\":\"{0}\",", score.Remark);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("}");

            sb.Append("}");
            sb.Append("}");

            return SendTemplateMessage(accessToken, sb.ToString());
        }


        /// <summary>
        /// 发送模板消息:任务提醒
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string SendTemplateMessage(string accessToken, string openId, TMTaskNotification msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            string templateId = "6eqP1WZestUZz09275s_FsixR3eblJbTAFkhvUnyxmE";
            if (!string.IsNullOrEmpty(msg.TemplateId))
            {
                templateId = msg.TemplateId;
            }
            sb.AppendFormat("\"template_id\":\"{0}\",", templateId);
            sb.AppendFormat("\"url\":\"{0}\",", msg.Url);
            sb.Append("\"topcolor\":\"#FF0000\",");
            sb.Append("\"data\":{");

            sb.Append("\"first\": {");
            sb.AppendFormat("\"value\":\"{0}\",", msg.First);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keyword1\": {");
            sb.AppendFormat("\"value\":\"{0}\",", msg.Keyword1);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keyword2\": {");
            sb.AppendFormat("\"value\":\"{0}\",", msg.Keyword2);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"remark\": {");
            sb.AppendFormat("\"value\":\"{0}\",", msg.Remark);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("}");

            sb.Append("}");
            sb.Append("}");

            return SendTemplateMessage(accessToken, sb.ToString());
        }


        /// <summary>
        /// 发送模板消息:卡券领取提醒
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string SendTemplateMessage(string accessToken, string openId, TMCardCouponNotification template)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);

            sb.AppendFormat("\"template_id\":\"{0}\",", template.TemplateId);
            sb.AppendFormat("\"url\":\"{0}\",", template.Url);
            sb.Append("\"topcolor\":\"#FF0000\",");
            sb.Append("\"data\":{");

            sb.Append("\"first\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.First);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keyword1\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.Keyword1);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keyword2\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.Keyword2);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"keyword3\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.Keyword3);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"remark\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.Remark);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("}");

            sb.Append("}");
            sb.Append("}");

            string result = SendTemplateMessage(accessToken, sb.ToString());
            return result;
        }

        /// <summary>
        /// 发送模板消息:购票通知
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string SendTemplateMessage(string accessToken, string openId, TMBuyTiketNotification template)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);

            sb.AppendFormat("\"template_id\":\"{0}\",", template.TemplateId);
            sb.AppendFormat("\"url\":\"{0}\",", template.Url);
            sb.Append("\"topcolor\":\"#FF0000\",");
            sb.Append("\"data\":{");

            sb.Append("\"first\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.First);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"lineName\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.lineName);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"date\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.date);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"orderNum\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.orderNum);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"transactionNum\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.transactionNum);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"remark\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.Remark);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("}");

            sb.Append("}");
            sb.Append("}");

            string result = SendTemplateMessage(accessToken, sb.ToString());
            return result;
        }


        /// <summary>
        /// 发送模板消息:任务提醒
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public string SendTemplateMessage(string accessToken, string openId, TMOderStatusUpdateNotification template)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.AppendFormat("\"template_id\":\"{0}\",", "tVTt8IHQwdvXCSKD37OVJKm-i88DlELFScinrcug514");
            sb.AppendFormat("\"url\":\"{0}\",", template.Url);
            sb.Append("\"topcolor\":\"#FF0000\",");
            sb.Append("\"data\":{");

            sb.Append("\"first\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.First);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"OrderSn\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.OrderSn);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"OrderStatus\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.OrderStatus);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("},");

            sb.Append("\"remark\": {");
            sb.AppendFormat("\"value\":\"{0}\",", template.Remark);
            sb.Append("\"color\":\"#173177\"");
            sb.Append("}");

            sb.Append("}");
            sb.Append("}");

            return SendTemplateMessage(accessToken, sb.ToString());
        }


        /// <summary>
        /// 发送模板消息，返回服务器结果
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private string SendTemplateMessage(string accessToken, string msg)
        {
            return PostMPRequest(accessToken, msg, string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accessToken));
        }
        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string SendMassMessage(string accessToken, string msg)
        {
            return PostMPRequest(accessToken, msg, string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}", accessToken));
        }
        /// <summary>
        /// 群发消息预览
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string SendMassMessagePreview(string accessToken, string msg)
        {
            return PostMPRequest(accessToken, msg, string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token={0}", accessToken));
        }
        /// <summary>
        /// 群发消息 任务
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendMassMessage(TimingTask task)
        {

            List<UploadArticle> articleList = new List<UploadArticle>();
            string accessToken = GetAccessToken(task.WebsiteOwner);
            foreach (string id in task.TaskInfo.Split(','))
            {
                WXMassArticle msg = Get<WXMassArticle>(string.Format("AutoID={0}", id));
                msg.ThumbImage = msg.ThumbImage.Replace("/", "\\");
                msg.ThumbImage = string.Format("D:\\WebSite\\CommonPlatform{0}", msg.ThumbImage);//本地磁盘路径
                string mediaId = UploadFileToWeixinModel(accessToken, "image", msg.ThumbImage).media_id;
                var model = new UploadArticle();
                model.thumb_media_id = mediaId;
                model.title = msg.Title;
                model.content_source_url = msg.Content_Source_Url;
                model.content = msg.Content;
                model.digest = msg.Digest;
                model.author = msg.Author;
                model.show_cover_pic = "0";
                //if (model.content.Contains("/FileUpload/"))
                //{
                //    model.content = model.content.Replace("/FileUpload/", "http://comeoncloud.comeoncloud.net/FileUpload/");
                //}
                articleList.Add(model);
            }
            var upLoadObj = new
            {

                articles = articleList

            };
            JToken jt = JToken.FromObject(upLoadObj);
            string sendMsg = jt.ToString();
            string sendMediaId = UploadNews(accessToken, sendMsg).media_id;
            string resultStr = SendMassMessageMpNews(accessToken, sendMediaId);
            var resultModel = GetErrorMessageModel(resultStr);
            return resultModel.errcode.Equals(0) ? true : false;
        }


        /// <summary>
        /// 群发文本消息 全部发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageText(string acceccToken, string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"filter\":{");
            sb.Append("\"is_to_all\":true");
            sb.Append(" },");
            sb.Append("\"text\":{");
            sb.AppendFormat("\"content\":\"{0}\"", msg);
            sb.Append("},");
            sb.Append("\"msgtype\":\"text\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }

        /// <summary>
        /// 群发文本消息 部分发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageText(string acceccToken, string msg, List<string> opeinids)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":[{0}],", Join(opeinids));
            sb.Append("\"text\":{");
            sb.AppendFormat("\"content\":\"{0}\"", msg);
            sb.Append("},");
            sb.Append("\"msgtype\":\"text\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }

        /// <summary>
        /// 群发图文消息 全部发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageMpNews(string acceccToken, string mediaId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"filter\":{");
            sb.Append("\"is_to_all\":true");
            sb.Append(" },");
            sb.Append("\"mpnews\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"mpnews\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }
        /// <summary>
        /// 发送图文预览
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageMpNewsPreview(string acceccToken, string mediaId, string openId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", openId);
            sb.Append("\"mpnews\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"mpnews\"");
            sb.Append("}");
            return SendMassMessagePreview(acceccToken, sb.ToString());
        }







        /// <summary>
        /// 群发图文消息 部分发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageMpNews(string acceccToken, string mediaId, List<string> opeinIds)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":[{0}],", Join(opeinIds));
            sb.Append("\"mpnews\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"mpnews\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }



        /// <summary>
        /// 群发语音消息 全部发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageVoice(string acceccToken, string mediaId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"filter\":{");
            sb.Append("\"is_to_all\":true");
            sb.Append(" },");
            sb.Append("\"voice\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"voice\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }
        /// <summary>
        /// 群发语音消息 部分发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageVoice(string acceccToken, string mediaId, List<string> opeinIds)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":[{0}],", Join(opeinIds));
            sb.Append("\"voice\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"voice\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }

        /// <summary>
        /// 群发图文消息 全部发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageImage(string acceccToken, string mediaId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"filter\":{");
            sb.Append("\"is_to_all\":true");
            sb.Append(" },");
            sb.Append("\"image\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"image\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }
        /// <summary>
        /// 群发图文消息 部分发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageImage(string acceccToken, string mediaId, List<string> opeinIds)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":[{0}],", Join(opeinIds));
            sb.Append("\"image\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"image\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }

        /// <summary>
        /// 群发视频消息 全部发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageMpVideo(string acceccToken, string mediaId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"filter\":{");
            sb.Append("\"is_to_all\":true");
            sb.Append(" },");
            sb.Append("\"mpvideo\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"mpvideo\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }


        /// <summary>
        /// 群发视频消息 部分发送
        /// </summary>
        /// <returns></returns>
        public string SendMassMessageMpVideo(string acceccToken, string mediaId, List<string> opeinIds)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":[{0}],", Join(opeinIds));
            sb.Append("\"mpvideo\":{");
            sb.AppendFormat("\"media_id\":\"{0}\"", mediaId);
            sb.Append("},");
            sb.Append("\"msgtype\":\"mpvideo\"");
            sb.Append("}");
            return SendMassMessage(acceccToken, sb.ToString());
        }
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="acceccToken">token</param>
        /// <param name="keyVauleId"></param>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public string SendTemplateMessage(string acceccToken, string keyVauleId, JToken sendData)
        {
            BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
            KeyVauleDataInfo tmplMsg = Get<KeyVauleDataInfo>(string.Format(" AutoId={0} AND DataType='WXTmplmsg'", keyVauleId));
            if (tmplMsg == null)
            {
                return "找不到消息模板";
            }
            List<KeyVauleDataInfo> tmplMsgData = GetList<KeyVauleDataInfo>(string.Format(" PreKey='{0}' AND DataType='WXTmplmsgData' AND WebsiteOwner='{1}' ", tmplMsg.DataKey, tmplMsg.WebsiteOwner));
            if (tmplMsgData.Count == 0)
            {
                return "模板对应数据字段为空";
            }
            JToken msgJt = JToken.Parse("{}");
            msgJt["touser"] = sendData["touser"];
            msgJt["template_id"] = tmplMsg.DataKey;
            if (sendData["url"] != null) msgJt["url"] = sendData["url"];
            JToken dataJt = JToken.Parse("{}");
            for (int i = 0; i < tmplMsgData.Count; i++)
            {
                JToken tempJt = JToken.Parse("{}");
                if (sendData[tmplMsgData[i].DataKey] != null)
                {
                    tempJt["value"] = sendData[tmplMsgData[i].DataKey];
                    dataJt[tmplMsgData[i].DataValue] = tempJt;
                }
            }
            msgJt["data"] = dataJt;

            //return msgJt.ToString();
            string errStr = SendTemplateMessage(acceccToken, msgJt.ToString());
            try
            {
                JToken reJt = JToken.Parse(errStr);
                if (reJt["errcode"] == null)
                {
                    return "发送出错";
                }
                else if (reJt["errcode"].ToString() == "0")
                {
                    return "";
                }
                else
                {
                    string errmsg = GetCodeMessage(Convert.ToInt32(reJt["errcode"].ToString()));
                    if (errmsg == null)
                    {
                        return "未知错误";
                    }
                    return errmsg;
                }
            }
            catch (Exception)
            {
                return "发送出错";
            }
        }

        ///// <summary>
        ///// 发送模板消息通知
        ///// </summary>
        ///// <param name="openId">openId</param>
        ///// <param name="title">标题</param>
        ///// <param name="content">内容</param>
        ///// <param name="url">链接</param>
        ///// <param name="msgType">消息类型</param>
        ///// <param name="serialNum">群发批次id</param>
        ///// <returns></returns>
        //public bool SendTemplateMessageNotifyComm(string openId, string title, string content, string url = "", string msgType = "", string serialNum = "", string userId = "")
        //{

        //    SMSPlanInfo plan = null;
        //    //serialNum为空，则自动创建一个消息任务
        //    if (string.IsNullOrWhiteSpace(serialNum))
        //    {
        //        plan = new SMSPlanInfo();
        //        plan.ChargeCount = 0;
        //        plan.PlanID = GetGUID(TransacType.CommAdd);
        //        plan.SubmitDate = DateTime.Now;
        //        plan.PlanType = (int)Enums.SMSPlanType.WXTemplateMsg_Notify;
        //        plan.SenderID = "system";
        //        plan.SendContent = content;
        //        plan.SendFrom = "系统自动发送模板通知消息";
        //        plan.SubmitCount = 1;
        //        plan.SubmitDate = DateTime.Now;
        //        plan.Title = title;
        //        plan.Url = url;
        //        plan.UsePipe = "none";
        //        plan.ProcStatus = 1;
        //        plan.WebsiteOwner = WebsiteOwner;
        //        serialNum = plan.PlanID;
        //    }


        //    WXBroadcastHistory historyModel = new WXBroadcastHistory();
        //    historyModel.OpenId = openId;
        //    historyModel.UserId = userId;
        //    historyModel.Title = title;
        //    historyModel.Msg = content;
        //    historyModel.Url = url;
        //    historyModel.BroadcastType = msgType;
        //    historyModel.SerialNum = serialNum;
        //    historyModel.InsertDate = DateTime.Now;
        //    historyModel.WebsiteOwner = WebsiteOwner;

        //    if (!string.IsNullOrEmpty(openId))
        //    {
        //        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        //        KeyVauleDataInfo model = bllKeyValueData.Get<KeyVauleDataInfo>(string.Format(" DataType='WXTemplateMsg' And PreKey='notify' And  WebsiteOwner='{0}'", WebsiteOwner));
        //        if (model != null)
        //        {
        //            string accessTokenMsg = string.Empty;
        //            string accessToken = GetAccessToken();
        //            if (!string.IsNullOrEmpty(accessToken))
        //            {
        //                string templateId = model.DataValue;
        //                StringBuilder sb = new StringBuilder();
        //                sb.Append("{");
        //                sb.AppendFormat("\"touser\":\"{0}\",", openId);

        //                sb.AppendFormat("\"template_id\":\"{0}\",", templateId);
        //                sb.AppendFormat("\"url\":\"{0}\",", url);
        //                sb.Append("\"topcolor\":\"#FF0000\",");
        //                sb.Append("\"data\":{");

        //                sb.Append("\"first\": {");
        //                sb.AppendFormat("\"value\":\"{0}\",", title);
        //                sb.Append("\"color\":\"#173177\"");
        //                sb.Append("},");

        //                sb.Append("\"keyword1\": {");
        //                sb.AppendFormat("\"value\":\"{0}\"", "正常");

        //                sb.Append("},");

        //                sb.Append("\"keyword2\": {");
        //                sb.AppendFormat("\"value\":\"{0}\",", DateTime.Now.ToString());
        //                sb.Append("\"color\":\"#173177\"");
        //                sb.Append("},");

        //                sb.Append("\"remark\": {");
        //                sb.AppendFormat("\"value\":\"{0}\",", content);
        //                sb.Append("\"color\":\"#173177\"");
        //                sb.Append("}");
        //                sb.Append("}");
        //                sb.Append("}");
        //                string result = SendTemplateMessage(accessToken, sb.ToString());
        //                historyModel.StatusMsg = result;
        //                plan.ProcStatus = 3;
        //                JToken reJt = JToken.Parse(result);
        //                if (reJt["errcode"].ToString() == "0")
        //                {
        //                    plan.SuccessCount = 1;
        //                    historyModel.Status = 1;

        //                    return true;
        //                }
        //                else
        //                {
        //                    historyModel.Status = 0;
        //                }
        //            }
        //            else
        //            {
        //                historyModel.StatusMsg = "未接入公众号" + accessTokenMsg;
        //                historyModel.Status = 0;
        //            }

        //        }
        //        else
        //        {
        //            historyModel.StatusMsg = "模板消息配置未找到，请检查是否配置了模板消息";
        //            historyModel.Status = 0;
        //        }
        //    }
        //    else
        //    {
        //        historyModel.StatusMsg = "OpenId不存在";
        //        historyModel.Status = 0;
        //    }

        //    Add(plan);
        //    Add(historyModel);

        //    return false;
        //}


        /// <summary>
        /// 发送模板消息通知
        /// </summary>
        /// <param name="openId">openId</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="url">链接</param>
        /// <param name="msgType">消息类型</param>
        /// <param name="serialNum">群发批次id</param>
        /// <returns></returns>
        public bool SendTemplateMessageNotifyComm(UserInfo userInfo, string title, string content, string url = "", string msgType = "", string serialNum = "")
        {

            SMSPlanInfo plan = null;
            //serialNum为空，则自动创建一个消息任务
            if (string.IsNullOrWhiteSpace(serialNum))
            {
                plan = new SMSPlanInfo();
                plan.ChargeCount = 0;
                plan.PlanID = GetGUID(TransacType.CommAdd);
                plan.SubmitDate = DateTime.Now;
                plan.PlanType = (int)Enums.SMSPlanType.WXTemplateMsg_Notify;
                plan.SenderID = "system";
                plan.SendContent = content;
                plan.SendFrom = "系统自动发送模板通知消息";
                plan.SubmitCount = 1;
                plan.SubmitDate = DateTime.Now;
                plan.Title = title;
                plan.Url = url;
                plan.UsePipe = "none";
                plan.ProcStatus = 1;
                plan.WebsiteOwner = userInfo.WebsiteOwner;
                serialNum = plan.PlanID;
            }


            WXBroadcastHistory historyModel = new WXBroadcastHistory();
            historyModel.OpenId = userInfo.WXOpenId;
            historyModel.UserId = userInfo.UserID;
            historyModel.Title = title;
            historyModel.Msg = content;
            historyModel.Url = url;
            historyModel.BroadcastType = msgType;
            historyModel.SerialNum = serialNum;
            historyModel.InsertDate = DateTime.Now;
            historyModel.WebsiteOwner = userInfo.WebsiteOwner;

            //app发送消息
            SMSPlanInfo appPlan = null;
            WXBroadcastHistory appHistoryModel = null;
            BLLAppManage bllApp = new BLLAppManage();
            WebsiteInfo website = GetWebsiteInfoModelFromDataBase(userInfo.WebsiteOwner);
            if (bllApp.HaveGetuiAppPush(website))
            {
                AppPushClient appClient = bllApp.GetAppPushClient(userInfo.WebsiteOwner, "",website.AppPushAppId,"",userInfo.UserID,"1");
                if(appClient!=null){
                    appPlan = new SMSPlanInfo();
                    appPlan.ChargeCount = 0;
                    appPlan.PlanID = GetGUID(TransacType.CommAdd);
                    appPlan.SubmitDate = DateTime.Now;
                    appPlan.PlanType = (int)Enums.SMSPlanType.AppMsg;
                    appPlan.SenderID = "system";
                    appPlan.SendContent = content;
                    appPlan.SendFrom = "系统自动发送模板通知消息";
                    appPlan.SubmitCount = 1;
                    appPlan.SubmitDate = DateTime.Now;
                    appPlan.Title = title;
                    appPlan.Url = url;
                    appPlan.UsePipe = "none";
                    appPlan.ProcStatus = 1;
                    appPlan.WebsiteOwner = userInfo.WebsiteOwner;

                    appHistoryModel = new WXBroadcastHistory();
                    appHistoryModel.OpenId = userInfo.WXOpenId;
                    appHistoryModel.UserId = userInfo.UserID;
                    appHistoryModel.Title = title;
                    appHistoryModel.Msg = content;
                    appHistoryModel.Url = url;
                    appHistoryModel.BroadcastType = "AppMsg";
                    appHistoryModel.SerialNum = appPlan.PlanID;
                    appHistoryModel.InsertDate = DateTime.Now;
                    appHistoryModel.WebsiteOwner = userInfo.WebsiteOwner;
                    string appmsg = "";
                    if (bllApp.PushMassage(website, title, content, url, userInfo, out appmsg))
                    {
                        appHistoryModel.StatusMsg = appmsg;
                        appPlan.ProcStatus = 3;
                        appPlan.SuccessCount = 1;
                        appHistoryModel.Status = 1;
                    }
                    else
                    {
                        appPlan.ProcStatus = 3;
                        appHistoryModel.Status = 0;
                        appHistoryModel.StatusMsg = appmsg;
                    }
                    Add(appPlan);
                    Add(appHistoryModel);
                }
            }

            if (!string.IsNullOrEmpty(userInfo.WXOpenId))
            {
                BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
                KeyVauleDataInfo model = bllKeyValueData.Get<KeyVauleDataInfo>(string.Format(" DataType='WXTemplateMsg' And PreKey='notify' And  WebsiteOwner='{0}'", userInfo.WebsiteOwner));
                if (model != null)
                {
                    string accessTokenMsg = string.Empty;
                    string accessToken = GetAccessToken(userInfo.WebsiteOwner);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        string templateId = model.DataValue;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("{");
                        sb.AppendFormat("\"touser\":\"{0}\",", userInfo.WXOpenId);

                        sb.AppendFormat("\"template_id\":\"{0}\",", templateId);
                        sb.AppendFormat("\"url\":\"{0}\",", url);
                        sb.Append("\"topcolor\":\"#FF0000\",");
                        sb.Append("\"data\":{");

                        sb.Append("\"first\": {");
                        sb.AppendFormat("\"value\":\"{0}\",", title);
                        sb.Append("\"color\":\"#173177\"");
                        sb.Append("},");

                        sb.Append("\"keyword1\": {");
                        sb.AppendFormat("\"value\":\"{0}\"", "正常");

                        sb.Append("},");

                        sb.Append("\"keyword2\": {");
                        sb.AppendFormat("\"value\":\"{0}\",", DateTime.Now.ToString());
                        sb.Append("\"color\":\"#173177\"");
                        sb.Append("},");

                        sb.Append("\"remark\": {");
                        sb.AppendFormat("\"value\":\"{0}\",", content);
                        sb.Append("\"color\":\"#173177\"");
                        sb.Append("}");
                        sb.Append("}");
                        sb.Append("}");

                        string result = SendTemplateMessage(accessToken, sb.ToString());

                        if (userInfo.WXOpenId == "oWFcfxBoQJ2n1fDtHbS00mMTMCgI")
                        {
                            ToLog("SendMessageToPreUser accessToken:" + accessToken, "D:\\SendMessageToPreUser.txt");
                            ToLog("SendMessageToPreUser sb:" + sb.ToString(), "D:\\SendMessageToPreUser.txt");
                            ToLog("SendMessageToPreUser result:" + result, "D:\\SendMessageToPreUser.txt");
                        }


                        historyModel.StatusMsg = result;

                        plan.ProcStatus = 3;
                        JToken reJt = JToken.Parse(result);
                        if (reJt["errcode"].ToString() == "0")
                        {
                            plan.SuccessCount = 1;
                            historyModel.Status = 1;

                            return true;
                        }
                        else
                        {
                            historyModel.Status = 0;
                        }
                    }
                    else
                    {
                        historyModel.StatusMsg = "未接入公众号" + accessTokenMsg;
                        historyModel.Status = 0;
                    }

                }
                else
                {
                    historyModel.StatusMsg = "模板消息配置未找到，请检查是否配置了模板消息";
                    historyModel.Status = 0;
                }
            }
            else
            {
                historyModel.StatusMsg = "OpenId不存在";
                historyModel.Status = 0;
            }

            Add(plan);
            Add(historyModel);

            return false;
        }



        /// <summary>
        /// 发送自定义模板消息
        /// </summary>
        /// <param name="json"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendTemplateMessageCustomize(string json, out string msg)
        {
            msg = "";
            string accessToken = GetAccessToken();
            string result = SendTemplateMessage(accessToken, json);
            JToken reJt = JToken.Parse(result);
            if (reJt["errcode"].ToString() == "0")
            {

                msg = "ok";
                return true;
            }
            else
            {
                msg = result;
                return false;
            }

        }

        /// <summary>
        /// 发送消息给客服
        /// </summary>
        public void SendTemplateMessageToKefu(string title, string content, string url = "", string msgType = "", string serialNum = "", string userId = "")
        {
            List<WXKeFu> kefuList = GetList<WXKeFu>(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));
            BLLUser bllUser = new BLLUser();
            foreach (var item in kefuList)
            {

                UserInfo userInfo = bllUser.GetUserInfoByOpenId(item.WeiXinOpenID, item.WebsiteOwner);
                if (userInfo != null)
                {
                    SendTemplateMessageNotifyComm(userInfo, title, content, url, msgType, serialNum);

                }
            }

        }
        /// <summary>
        /// 发送消息给客服(有审核权限)
        /// </summary>
        public void SendTemplateMessageToKefuTranAuditPer(string title, string content, string url = "", string msgType = "", string serialNum = "", string userId = "")
        {
            List<WXKeFu> kefuList = GetList<WXKeFu>(string.Format("WebsiteOwner='{0}' And IsTransfersAuditPer=1", WebsiteOwner));
            BLLUser bllUser = new BLLUser();
            foreach (var item in kefuList)
            {

                UserInfo userInfo = bllUser.GetUserInfoByOpenId(item.WeiXinOpenID, item.WebsiteOwner);
                if (userInfo != null)
                {
                    SendTemplateMessageNotifyComm(userInfo, title, content, url, msgType, serialNum);

                }
            }

        }

        /// <summary>
        /// 发送模板消息通知
        /// </summary>
        /// <param name="openId">openId</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="url">链接</param>
        /// <param name="msgType">消息类型</param>
        /// <param name="serialNum">群发批次id</param>
        /// <returns></returns>
        public bool SendTemplateMessageNotifyCommTask(string openId, string title, string content, string url = "", string msgType = "", string serialNum = "", string userId = "", string websiteOwner = "")
        {
            if (string.IsNullOrEmpty(openId) && string.IsNullOrEmpty(userId))
            {
                return false;
            }
            WXBroadcastHistory historyModel = bllWeixinOpen.Get<WXBroadcastHistory>(string.Format(" OpenId='{0}' AND SerialNum='{1}' ", openId, serialNum));
            //historyModel.StatusMsg = "";

            bool isSuccess = false;
            //app发送消息
            if (historyModel.BroadcastType == "AppMsg" || historyModel.BroadcastType == "AppAndWx")
            {
                BLLAppManage bllApp = new BLLAppManage();
                BLLUser bllUser = new BLLUser();
                WebsiteInfo website = GetWebsiteInfoModelFromDataBase(websiteOwner);
                UserInfo userInfo = null;
                if (!string.IsNullOrWhiteSpace(openId))
                {
                   userInfo = bllUser.GetUserInfoByOpenId(openId, websiteOwner);
                }
                else if (!string.IsNullOrWhiteSpace(userId))
                {
                    userInfo = bllUser.GetUserInfo(userId, websiteOwner);
                }
                if (userInfo != null)
                {
                    string appmsg = "";
                    try
                    {
                        isSuccess = bllApp.PushMassage(website, title, content, url, userInfo, out appmsg);
                    }
                    catch (Exception ex)
                    {
                        appmsg = ex.Message;
                    }

                    historyModel.StatusMsg = appmsg;
                    if (historyModel.BroadcastType == "AppMsg" && isSuccess)
                    {
                        historyModel.Status = 1;
                    }
                    else if (historyModel.BroadcastType == "AppAndWx" && isSuccess)
                    {
                        historyModel.Status = 2;
                    }
                }
            }
            if (historyModel.BroadcastType != "AppMsg" && !string.IsNullOrEmpty(openId))
            {
                BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
                KeyVauleDataInfo model = bllKeyValueData.Get<KeyVauleDataInfo>(string.Format(" DataType='WXTemplateMsg' And PreKey='notify' And  WebsiteOwner='{0}'", websiteOwner));
                if (model != null)
                {
                    string accessTokenMsg = string.Empty;
                    string accessToken = GetAccessToken(websiteOwner);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        string templateId = model.DataValue;
                        StringBuilder sb = new StringBuilder();
                        sb.Append("{");
                        sb.AppendFormat("\"touser\":\"{0}\",", openId);

                        sb.AppendFormat("\"template_id\":\"{0}\",", templateId);
                        sb.AppendFormat("\"url\":\"{0}\",", url);
                        sb.Append("\"topcolor\":\"#FF0000\",");
                        sb.Append("\"data\":{");

                        sb.Append("\"first\": {");
                        sb.AppendFormat("\"value\":\"{0}\",", title);
                        sb.Append("\"color\":\"#173177\"");
                        sb.Append("},");

                        sb.Append("\"keyword1\": {");
                        sb.AppendFormat("\"value\":\"{0}\"", "正常");

                        sb.Append("},");

                        sb.Append("\"keyword2\": {");
                        sb.AppendFormat("\"value\":\"{0}\",", DateTime.Now.ToString());
                        sb.Append("\"color\":\"#173177\"");
                        sb.Append("},");

                        sb.Append("\"remark\": {");
                        sb.AppendFormat("\"value\":\"{0}\",", content);
                        sb.Append("\"color\":\"#173177\"");
                        sb.Append("}");
                        sb.Append("}");
                        sb.Append("}");
                        string result = SendTemplateMessage(accessToken, sb.ToString());
                        historyModel.StatusMsg += result;
                        JToken reJt = JToken.Parse(result);

                        if (reJt["errcode"].ToString() == "0")
                        {
                            if (historyModel.BroadcastType != "AppAndWx") { 
                                historyModel.Status = 1;
                            }
                            else
                            {
                                if (historyModel.Status == 2)
                                {
                                    historyModel.Status = 1;
                                }
                                else
                                {
                                    historyModel.Status = 3;
                                }
                            }
                            isSuccess = true;
                        }
                        else
                        {
                            if (historyModel.BroadcastType != "AppAndWx") historyModel.Status = 0;
                        }
                    }
                    else
                    {
                        historyModel.StatusMsg += "accessToken为空:" + accessTokenMsg;
                        if (historyModel.BroadcastType != "AppAndWx") historyModel.Status = 0;
                    }

                }
                else
                {
                    historyModel.StatusMsg += "模板消息配置未找到，请检查是否配置了模板消息";
                    if (historyModel.BroadcastType != "AppAndWx") historyModel.Status = 0;
                }
            }
            
            //if (!string.IsNullOrEmpty(openId))
            //{
            //}
            //else
            //{
            //    historyModel.StatusMsg = "OpenId不存在";
            //    historyModel.Status = 0;
            //}
            if (historyModel.Status == -1) historyModel.Status = 0;
            Update(historyModel);
            return isSuccess;
        }


        ///// <summary>
        ///// 获取默认客服消息
        ///// </summary>
        ///// <returns></returns>
        //public WXKeFu GetDefaultKefu()
        //{
        //    var currWebsiteUserInfo = GetCurrWebSiteUserInfo();
        //    if (!string.IsNullOrEmpty(currWebsiteUserInfo.WeiXinKeFuOpenId))
        //    {
        //        WXKeFu model = new WXKeFu();
        //        model.WeiXinOpenID = currWebsiteUserInfo.WeiXinKeFuOpenId;
        //        return model;
        //    }
        //    return Get<WXKeFu>(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));
        //}
        /// <summary>
        /// 获取客服详情
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="website"></param>
        /// <returns></returns>
        public WXKeFu GetKeFu(string openID, string website)
        {
            return Get<WXKeFu>(string.Format(" WebsiteOwner='{0}' AND WeiXinOpenID='{1}'", website, openID));
        }

        /// <summary>
        /// 获取合成之后的二维码
        /// </summary>
        /// <param name="WXQRCodeImg">二维码图片地址</param>
        /// <param name="WXIconImg">小图标图片地址</param>
        /// <returns></returns>
        public string GetQRCodeImg(string WXQRCodeImg, string WXIconImg)
        {
            BLLJuActivity bllJuActivity = new BLLJuActivity();
            string imgOrgPath = System.Web.HttpContext.Current.Server.MapPath(bllJuActivity.DownLoadRemoteImage(WXQRCodeImg));
            string imgOrgWater = System.Web.HttpContext.Current.Server.MapPath(bllJuActivity.DownLoadRemoteImage(WXIconImg));
            string imgVstr = "/FileUpload/WXMerge/" + Guid.NewGuid().ToString() + ".jpg";
            if (!Directory.Exists(imgVstr)) Directory.CreateDirectory(imgVstr);
            string imgVstrLocal = HttpContext.Current.Server.MapPath(imgVstr);
            ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
            imgHelper.SaveWatermark(imgOrgPath, imgOrgWater, 1f, ZentCloud.Common.ImgWatermarkHelper.WatermarkPosition.Center, 0, imgVstrLocal, 0.3f);//, 0.25f
            return imgVstr;
        }

        /// <summary>
        /// 获取合成之后的二维码
        /// </summary>
        /// <param name="wxQRCodeImg">二维码图片地址</param>
        /// <param name="wxIconImg">小图标图片地址</param>
        /// <returns></returns>
        public string GetQRCodeImgLocal(string wxQRCodeImg, string wxIconImg)
        {
            BLLJuActivity bllJuActivity = new BLLJuActivity();

            string websitePath = Common.ConfigHelper.GetConfigString("WebSitePath");

            if (string.IsNullOrWhiteSpace(websitePath))
            {
                websitePath = "D:\\WebSite\\CommonPlatform";
            }

            string imgOrgPath = wxQRCodeImg;

            if (wxQRCodeImg.ToLower().IndexOf("http") > -1)
            {
                imgOrgPath = websitePath + bllJuActivity.DownLoadRemoteImageLocal(wxQRCodeImg);
            }

            string imgOrgWater = websitePath + bllJuActivity.DownLoadRemoteImageLocal(wxIconImg);
            string imgVstr = "/FileUpload/WXMerge/" + Guid.NewGuid().ToString() + ".jpg";
            if (!Directory.Exists(imgVstr)) Directory.CreateDirectory(imgVstr);
            string imgVstrLocal = websitePath + imgVstr;
            ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
            imgHelper.SaveWatermark(imgOrgPath, imgOrgWater, 1f, ZentCloud.Common.ImgWatermarkHelper.WatermarkPosition.Center, 0, imgVstrLocal, 0.3f);//, 0.25f
            return imgVstr;
        }






        /// <summary>
        /// 上传结果模型
        /// </summary>
        public class UpLoadResult
        {
            /// <summary>
            /// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb），次数为news，即图文消息
            /// </summary>
            public string type { get; set; }
            /// <summary>
            ///媒体文件/图文消息上传后获取的唯一标识 
            /// </summary>
            public string media_id { get; set; }
            /// <summary>
            /// 媒体文件上传时间
            /// </summary>
            public string created_at { get; set; }

        }
        /// <summary>
        /// 群发接口上传图文
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public UpLoadResult UploadNews(string accessToken, string msg)
        {
            return Common.JSONHelper.JsonToModel<UpLoadResult>(PostMPRequest(accessToken, msg, string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}", accessToken)));
        }
        /// <summary>
        /// 获取客服最大id
        /// </summary>
        /// <returns></returns>
        public int GetKeFuMaxID()
        {
            return GetList<WXKeFu>(" 1=1 ").Select(p => p.AutoID).Max();
        }

        /// <summary>
        /// 是否已经通过开放平台授权
        /// true 开放平台授权
        /// false 默认授权
        /// </summary>
        /// <returns></returns>
        private bool IsAuthToOpen()
        {
            var websiteOwnerInfo = GetCurrWebSiteUserInfo();

            if (websiteOwnerInfo == null)
            {
                return false;
            }

            if ((!string.IsNullOrEmpty(websiteOwnerInfo.WeixinAppId)) && (!string.IsNullOrEmpty(websiteOwnerInfo.WeixinAppSecret)))//已经填写 appid appsecret
            {

                return false;
            }
            var currentWebsiteInfo = GetWebsiteInfoModelFromDataBase();
            if (!string.IsNullOrEmpty(currentWebsiteInfo.AuthorizerAppId))
            {

                return true;
            }
            return false;



        }
        /// <summary>
        /// 是否已经通过开放平台授权 指定站点
        /// true 开放平台授权
        /// false 默认授权
        /// </summary>
        /// <returns></returns>
        private bool IsAuthToOpen(string websiteOwner)
        {

            var websiteOwnerInfo = userBll.GetUserInfo(websiteOwner, websiteOwner);
            if ((websiteOwnerInfo != null) && (!string.IsNullOrEmpty(websiteOwnerInfo.WeixinAppId)) && (!string.IsNullOrEmpty(websiteOwnerInfo.WeixinAppSecret)))//已经填写 appid appsecret
            {

                return false;
            }
            var websiteInfo = userBll.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            if (!string.IsNullOrEmpty(websiteInfo.AuthorizerAppId))
            {

                return true;
            }
            return false;
        }

        /// <summary>
        /// 批量获取素材
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="type">
        /// 素材的类型，图片（image）、视频（video）、语音 （voice）、图文（news）</param>
        /// <param name="offSet">从全部素材的该偏移位置开始返回，0表示从第一个素材 返回</param>
        /// <param name="count">返回素材的数量，取值在1到20之间</param>
        /// <returns></returns>
        public string BatchGetMaterial(string websiteOwner, string type, int offSet, int count)
        {


            var dataObj = new
            {
                type = type,
                offset = offSet,
                count = count


            };
            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);
            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + GetAccessToken(websiteOwner), Encoding.UTF8);
            return result;




        }
        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns>
        /// </returns>
        public string GetMaterialCount(string websiteOwner)
        {

            // {
            // "voice_count":COUNT,语音总数量
            // "video_count":COUNT,视频总数量
            // "image_count":COUNT,图片总数量
            // "news_count":COUNT//图文总数量
            //}



            var result = ZentCloud.Common.MySpider.GetPageSourceForUTF8("https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token=" + GetAccessToken(websiteOwner));
            return result;




        }


        /// <summary>
        /// 同步微信图文素材
        /// </summary>
        /// <param name="websiteOwner"></param>
        public int SynWeixinNews(string websiteOwner)
        {
            BLLJuActivity bllJuactivity = new BLLJuActivity();
            int successCount = 0;
            JToken tokenTotalCount = JToken.Parse(GetMaterialCount(websiteOwner));
            int totalCount = int.Parse(tokenTotalCount["news_count"].ToString());
            int pageSize = 20;
            int totalPage = GetTotalPage(totalCount, pageSize);
            int offSet = 0;//从全部素材的该偏移位置开始返回，0表示从第一个素材 返回
            if (totalCount > 0)
            {
                for (int i = 1; i <= totalPage; i++)//总页数
                {
                    JToken token = JToken.Parse(BatchGetMaterial(websiteOwner, "news", offSet, pageSize));
                    foreach (var item in token["item"])
                    {
                        foreach (var newsItem in item["content"]["news_item"])
                        {
                            string title = newsItem["title"].ToString();//标题
                            string digest = newsItem["digest"].ToString();//描述
                            string thumbUrl = newsItem["thumb_url"].ToString();//缩略图
                            string thumbMediaId = newsItem["thumb_media_id"].ToString();//媒体ID
                            string url = newsItem["url"].ToString();//链接
                            WeixinMsgSourceInfo model = Get<WeixinMsgSourceInfo>(string.Format(" ThumbMediaId='{0}' And UserId='{1}' And MediaId='{2}' ", thumbMediaId, websiteOwner, item["media_id"].ToString()));
                            if (model == null)//新增
                            {
                                model = new WeixinMsgSourceInfo();
                                model.SourceID = GetGUID(TransacType.CommAdd);
                                model.UserID = websiteOwner;
                                model.Title = title;
                                model.Description = digest;
                                thumbUrl = bllJuactivity.DownLoadRemoteImage(thumbUrl);
                                thumbUrl = string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, thumbUrl);
                                model.PicUrl = thumbUrl;
                                model.Url = url;
                                model.MediaId = item["media_id"].ToString();
                                model.ThumbMediaId = thumbMediaId;
                                if (Add(model))
                                {
                                    successCount++;
                                }



                            }
                            else//更新
                            {
                                model.Title = title;
                                thumbUrl = bllJuactivity.DownLoadRemoteImage(thumbUrl);
                                thumbUrl = string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, thumbUrl);
                                model.PicUrl = thumbUrl;
                                model.Url = url;
                                model.Description = digest;
                                if (Update(model))
                                {
                                    successCount++;
                                }
                            }


                        }
                    }
                    offSet = i * pageSize;//下一页的偏移位置


                }




            }


            return successCount;


        }



        /// <summary>
        /// 模板消息:报名成功通知
        /// </summary>
        public struct TMSignupNotification
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public string TemplateId { get; set; }
            /// <summary>
            /// 跳转链接
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string First { get; set; }
            /// <summary>
            /// 备注1
            /// </summary>
            public string Keynote1 { get; set; }
            /// <summary>
            /// 备注2
            /// </summary>
            public string Keynote2 { get; set; }
            /// <summary>
            /// 备注3
            /// </summary>
            public string Keynote3 { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

        }

        /// <summary>
        /// 模板消息：积分提醒
        /// </summary>
        public struct TMScoreNotification
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public string TemplateId { get; set; }
            /// <summary>
            /// 跳转链接
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string First { get; set; }
            /// <summary>
            /// 账户
            /// </summary>
            public string Account { get; set; }
            /// <summary>
            /// 时间
            /// </summary>
            public string Time { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 变动通知
            /// </summary>
            public string CreditChange { get; set; }
            /// <summary>
            /// 数额
            /// </summary>
            public string Number { get; set; }
            /// <summary>
            /// 信用卡名称
            /// </summary>
            public string CreditName { get; set; }
            /// <summary>
            /// 金额
            /// </summary>
            public string Amount { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

        }

        /// <summary>
        ///模板消息：任务提醒
        /// </summary>
        public struct TMTaskNotification
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public string TemplateId { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string First { get; set; }
            /// <summary>
            /// 跳转链接
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 关键字1
            /// </summary>
            public string Keyword1 { get; set; }
            /// <summary>
            /// 关键字2
            /// </summary>
            public string Keyword2 { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

        }

        /// <summary>
        ///模板消息：订单状态更改通知
        /// </summary>
        public struct TMOderStatusUpdateNotification
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public string TemplateId { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string First { get; set; }
            /// <summary>
            /// 跳转链接
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderSn { get; set; }
            /// <summary>
            /// 订单状态
            /// </summary>
            public string OrderStatus { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

        }
        /// <summary>
        ///模板消息：卡券通知提醒
        /// </summary>
        public struct TMCardCouponNotification
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public string TemplateId { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string First { get; set; }
            /// <summary>
            /// 跳转链接
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 关键字1
            /// </summary>
            public string Keyword1 { get; set; }
            /// <summary>
            /// 关键字2
            /// </summary>
            public string Keyword2 { get; set; }
            /// <summary>
            /// 关键字3
            /// </summary>
            public string Keyword3 { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

        }

        /// <summary>
        ///模板消息：购票通知提醒
        /// </summary>
        public struct TMBuyTiketNotification
        {
            /// <summary>
            /// 模板ID
            /// </summary>
            public string TemplateId { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string First { get; set; }
            /// <summary>
            /// 跳转链接
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// 线路
            /// </summary>
            public string lineName { get; set; }
            /// <summary>
            /// 发车时间
            /// </summary>
            public string date { get; set; }
            /// <summary>
            /// 订单号
            /// </summary>
            public string orderNum { get; set; }
            /// <summary>
            /// 流水号
            /// </summary>
            public string transactionNum { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

        }


        //public bool isSuccess { get; set; }
    }
}
