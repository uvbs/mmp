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
    /// 更新账户
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int uid = int.Parse(context.Request["AutoID"]);
            UserInfo tUser = bllUser.GetUserInfoByAutoID(uid);
            long oPermissionGroupID = tUser.PermissionGroupID.HasValue ? tUser.PermissionGroupID.Value : 0;
            string oPhone = tUser.Phone;
            string oEmail = tUser.Email;
            tUser = bllUser.ConvertRequestToModel<UserInfo>(tUser);
            if (string.IsNullOrWhiteSpace(tUser.UserID) || string.IsNullOrWhiteSpace(tUser.Password))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "账户和密码不能为空";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            //if (!string.IsNullOrWhiteSpace(tUser.Phone) && !ZentCloud.Common.MyRegex.PhoneNumLogicJudge(tUser.Phone))
            //{
            //    apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
            //    apiResp.msg = "请输入正确的手机号码";
            //    bllUser.ContextResponse(context, apiResp);
            //    return;
            //}
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
            BLLTransaction tran = new BLLTransaction();
            if (!bllUser.Update(tUser, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "编辑用户出错";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (oPermissionGroupID != tUser.PermissionGroupID.Value)
            {
                var ogroup = new BLLPermission.Model.UserPmsGroupRelationInfo()
                {
                    UserID = tUser.UserID,
                    GroupID = oPermissionGroupID
                };
                if (bllUser.Delete(ogroup) < 0)//删除权限组
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "删除旧权限组出错";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                var group = new BLLPermission.Model.UserPmsGroupRelationInfo()
                {
                    UserID = tUser.UserID,
                    GroupID = tUser.PermissionGroupID.Value
                };

                if (bllUser.GetCount<BLLPermission.Model.UserPmsGroupRelationInfo>(string.Format("UserID='{0}' And GroupID={1}",group.UserID,group.GroupID))==0)
                {
                    if (!bllUser.Add(group))//添加权限组
                    {

                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "添加权限组出错";
                        bllUser.ContextResponse(context, apiResp);
                        return;
                    }
                }


            }
            tran.Commit();
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "修改完成";
            bllUser.ContextResponse(context, apiResp);
        }


    }
}