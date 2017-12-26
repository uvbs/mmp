using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Account
{
    /// <summary>
    /// 添加账户
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            UserInfo tUser = new UserInfo();
            tUser = bllUser.ConvertRequestToModel<UserInfo>(tUser);
            tUser.UserType = 2;
            tUser.RegIP = ZentCloud.Common.MySpider.GetClientIP();
            tUser.Regtime = DateTime.Now;
            tUser.LoginTotalCount = 0;
            tUser.IsSubAccount = "1";
            tUser.WebsiteOwner = bllUser.WebsiteOwner;
            tUser.LastLoginDate = DateTime.Now;
            if (string.IsNullOrWhiteSpace(tUser.UserID) || string.IsNullOrWhiteSpace(tUser.Password))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "账户和密码不能为空";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(tUser.Phone) && !ZentCloud.Common.MyRegex.PhoneNumLogicJudge(tUser.Phone))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入正确的手机号码";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(tUser.Phone3) && !ZentCloud.Common.MyRegex.PhoneNumLogicJudge(tUser.Phone3))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入正确的手机号码";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo oUser = bllUser.GetUserInfo(tUser.UserID,bllUser.WebsiteOwner);
            if (oUser != null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "账户名已存在";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(tUser.Phone))
            {
                oUser = bllUser.GetUserInfoByPhone(tUser.Phone, bllUser.WebsiteOwner);
                if (oUser != null)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "联系手机，已有账号";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                } 
            }
            if (!string.IsNullOrWhiteSpace(tUser.Email) && !ZentCloud.Common.MyRegex.EmailLogicJudge(tUser.Email))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入正确的邮箱地址";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (!tUser.PermissionGroupID.HasValue)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请选择角色";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            ZentCloud.BLLPermission.Model.PermissionGroupInfo perGroup = bllUser.Get<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(string.Format(" GroupID={0}",tUser.PermissionGroupID));
            if (perGroup.GroupType==4)
            {
                tUser.DistributionOwner = bllUser.WebsiteOwner; 
            }
            BLLTransaction tran = new BLLTransaction();
            if (!bllUser.Add(tUser, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "添加用户出错";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            var group = new BLLPermission.Model.UserPmsGroupRelationInfo()
            {
                UserID = tUser.UserID,
                GroupID = tUser.PermissionGroupID.Value
            };

            if (!bllUser.Add(group, tran))//添加权限组
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "添加权限组出错";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            tran.Commit();
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "添加完成";
            bllUser.ContextResponse(context, apiResp);
        }
    }
}