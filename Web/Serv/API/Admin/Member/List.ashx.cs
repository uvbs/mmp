using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMember bll = new BLLMember();
        BLLMeifan bllMeifan = new BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            List<string> defFields = new List<string>() { "AutoID", "WXHeadimgurl", "UserID", "TotalScore", "UserType", "AccessLevel", "AccountAmount", "DistributionOwner", "WXNickname", "WebsiteOwner","IsWeixinFollower" };
            
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string mapping_type = context.Request["mapping_type"];
            string keyWord = context.Request["KeyWord"];
            string tagName = context.Request["TagName"];
            string haveTrueName = context.Request["HaveTrueName"];
            string haveWxNickNameAndTrueName = context.Request["HaveWxNickNameAndTrueName"];
            string isFans = context.Request["IsFans"];//是否是粉丝
            string isReg = context.Request["IsReg"];//是否是会员
            string isDisOnLineUser = context.Request["IsDisOnLineUser"];//是否商城分销会员
            string isDisOffLineUser = context.Request["IsDisOffLineUser"];//是否业务分销会员
            string isPhoneReg = context.Request["IsPhoneReg"];//是否手机验证会员
            string isName = context.Request["isName"];
            string isPhone = context.Request["isPhone"];
            string isEmail = context.Request["isEmail"];
            string isWxnickName = context.Request["isWxnickName"];
            string isMember = context.Request["isMember"];
            string isApp = context.Request["isApp"];
            string userAutoId = context.Request["autoId"];//用户AutoId
            string isOrAnd = context.Request["isOrAnd"];
            string userType=context.Request["user_type"];
            string noDistributionOwner = context.Request["noDistributionOwner"];//不是分销员

            List<UserInfo> userList = bll.GetMemberList(rows, page, mapping_type, defFields, bll.WebsiteOwner, keyWord, tagName, haveTrueName, haveWxNickNameAndTrueName, isFans,
                isReg, isDisOnLineUser, isDisOffLineUser, isPhoneReg, isName, isPhone, isEmail, isWxnickName, isMember, userAutoId, isOrAnd, userType, noDistributionOwner, isApp);

            int totalCount = bll.GetMemberCount(bll.WebsiteOwner, keyWord, tagName, haveTrueName, haveWxNickNameAndTrueName, isFans,
                isReg, isDisOnLineUser, isDisOffLineUser, isPhoneReg, isName, isPhone, isEmail, isWxnickName, isMember, userAutoId, isOrAnd, userType, noDistributionOwner, isApp);
            foreach (var item in userList)
            {
                if (string.IsNullOrWhiteSpace(item.WXNickname) && !string.IsNullOrWhiteSpace(item.WXOpenId))
                {
                    item.WXNickname="微信用户";
                }
                item.Password = "";
                #region 美帆
                if (item.WebsiteOwner == "meifan")
                {
                    item.Ex1 = bllMeifan.GetMyDefualtCardNumber(item.UserID);//会员卡号
                    if (!string.IsNullOrEmpty(item.Ex1))
                    {
                        var myCard = bllMeifan.Get<MeifanMyCard>(string.Format(" CardNum='{0}'", item.Ex1));
                        if (myCard != null)
                        {
                            var card = bllMeifan.GetCard(myCard.CardId);
                            if (card != null)
                            {
                                item.Ex2 = card.CardType;
                            }

                        }

                    }


                }  
                #endregion
            }

            apiResp.result = new
            {
                totalcount = totalCount,
                list = userList
            };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }


    }
}