using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ShUpdateLoginPhone 的摘要说明
    /// </summary>
    public class ShUpdateLoginPhone : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            ShUpdate.RequestModel requestModel = new ShUpdate.RequestModel();
            try
            {
                requestModel = bllUser.ConvertRequestToModel<ShUpdate.RequestModel>(requestModel);
            }
            catch (Exception ex)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "参数错误";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            UserInfo updateUser = bllUser.GetUserInfoByAutoID(requestModel.id);
            if (updateUser == null)
            {
                apiResp.msg = "会员未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo otherUser = bllUser.GetUserInfoByPhone(requestModel.phone,bllUser.WebsiteOwner);
            if (otherUser != null && otherUser.AutoID != updateUser.AutoID)
            {
                apiResp.msg = "该登录手机已有账号使用";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            string remark = "修改会员登录手机：";
            string oldPhone = "";
            if (updateUser.Phone != requestModel.phone)
            {
                remark += string.Format(" 登录手机[{0}-{1}]", updateUser.Phone, requestModel.phone);
                oldPhone = updateUser.Phone;
                updateUser.Phone = requestModel.phone;
            }
            if (bllUser.Update(updateUser))
            {
                if (remark == "修改会员登录手机：") remark += "仅保存，信息未检测到更改";
                bllLog.Add(EnumLogType.ShMember, EnumLogTypeAction.Update, currentUserInfo.UserID, remark, targetID: updateUser.UserID);

                //异步修改积分明细表
                Thread th1 = new Thread(delegate()
                {
                    bllUser.Update(new UserScoreDetailsInfo(),
                        string.Format("[AddNote] = REPLACE([AddNote],'[{0}]','[{1}]')", oldPhone, updateUser.Phone),
                        string.Format(" WebsiteOwner='{1}' And ScoreType='TotalAmount' And [AddNote] like '%[{0}]%' ", oldPhone, bllUser.WebsiteOwner));
                });
                th1.Start();

                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "修改会员登录手机完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "修改会员登录手机失败";
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}