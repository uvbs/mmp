using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Relation
{
    /// <summary>
    /// AddCommUserRelation 的摘要说明
    /// </summary>
    public class AddCommUserRelation : BaseHandlerNoAction
    {
        /// <summary>
        /// 消息中心模块
        /// </summary>
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        /// <summary>
        /// 通用关系业务
        /// </summary>
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 文章逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            string rtype = context.Request["rtype"],
                mainId = context.Request["mainId"],
                expandId = context.Request["expandId"],
                ex1 = context.Request["ex1"],
                exchange = context.Request["exchange"];//1时mainId，relationId互换

            BLLJIMP.Enums.CommRelationType nType = new BLLJIMP.Enums.CommRelationType();
            if (!Enum.TryParse(rtype, out nType))
            {
                apiResp.code = 1;
                apiResp.msg = "类型格式不能识别";
                bLLCommRelation.ContextResponse(context, apiResp);
                return;
            }
            if (mainId == "0" || string.IsNullOrWhiteSpace(mainId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                apiResp.msg = "关联主Id错误";
                bLLCommRelation.ContextResponse(context, apiResp);
                return ;
            }

            currentUserInfo = bLLCommRelation.GetCurrentUserInfo();

            if (this.currentUserInfo == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                apiResp.msg = "请先登录";
                bLLCommRelation.ContextResponse(context, apiResp);
                return;
            }
            if (nType == CommRelationType.FriendApply)
            {
                if (currentUserInfo.UserType != 2)
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.AddCommRelationError;
                    apiResp.msg = "公司不能申请好友";
                    bLLCommRelation.ContextResponse(context, apiResp);
                    return;
                }
            }
            string relationId = this.currentUserInfo.AutoID.ToString();
            if (mainId == relationId)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "不能跟自己建立关系";
                bLLCommRelation.ContextResponse(context, apiResp);
                return;
            }
            if (exchange == "1")
            {
                relationId = mainId;
                mainId = this.currentUserInfo.AutoID.ToString();
            }
            if (nType == CommRelationType.FriendApply)
            {
                if (this.bLLCommRelation.ExistRelation(CommRelationType.Friend, mainId, relationId))
                {
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.HasFriend;
                    apiResp.msg = "已经是好友";
                    bLLCommRelation.ContextResponse(context, apiResp);
                    return;
                }
            }
            if (this.bLLCommRelation.ExistRelation(nType, mainId, relationId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                apiResp.msg = "已存在关系";
                bLLCommRelation.ContextResponse(context, apiResp);
                return;
            }
            if (this.bLLCommRelation.AddCommRelation(nType, mainId, relationId, expandId, ex1))
            {
                if (nType == CommRelationType.FriendApply)
                {
                    UserInfo toUser = bllUser.GetUserInfoByAutoID(int.Parse(mainId));
                    bLLCommRelation.AddCommRelation(CommRelationType.FriendApply,relationId,mainId);
                    //bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FriendApply, this.currentUserInfo, null, new List<UserInfo>() { toUser }, null);
                }
                else if (nType == CommRelationType.Friend)
                {
                    UserInfo toUser = bllUser.GetUserInfoByAutoID(int.Parse(mainId));
                    //添加相关好友关系
                    bLLCommRelation.AddCommRelation(CommRelationType.Friend, relationId, mainId);
                    //通过好友申请删除申请关系
                    bLLCommRelation.DelCommRelation(CommRelationType.FriendApply, relationId, mainId);

                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.PassFriendApply, this.currentUserInfo, null, new List<UserInfo>() { toUser }, null);
                }
                else if (nType == CommRelationType.JuActivityPraise)
                {
                    JuActivityInfo article = bll.GetJuActivity(int.Parse(mainId), false);

                    //点赞数直接修改到主表
                    int praiseCount = bLLCommRelation.GetRelationCount(nType, mainId, null);
                    bll.Update(article, string.Format("PraiseCount={0}", praiseCount), string.Format("JuActivityID={0}", article.JuActivityID));
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.JuActivityPraise, this.currentUserInfo, article, article.UserID, null);
                }

                apiResp.status = true;
                apiResp.msg = "添加完成";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "添加失败";
            }
            bLLCommRelation.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}