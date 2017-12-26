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
    /// ShUpdateLock 的摘要说明
    /// </summary>
    public class ShUpdateLock : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            int is_lock = Convert.ToInt32(context.Request["is_lock"]);
            UserInfo curUser = bllUser.GetColByKey<UserInfo>("AutoID", id, 
                "AutoID,UserID,TrueName,Phone",
                websiteOwner: bllUser.WebsiteOwner);
            string event_name = is_lock ==1?"锁定":"解锁";
            if(bllUser.Update(new UserInfo(),
                string.Format("IsLock={0}", is_lock),
                string.Format("WebsiteOwner='{0}' And AutoID={1} ", bllUser.WebsiteOwner, curUser.AutoID)) > 0)
            {
                string addNote = string.Format("{0}会员{1}[{2}]", event_name, curUser.TrueName, curUser.Phone);
                bllLog.Add(EnumLogType.ShMember, EnumLogTypeAction.Update, currentUserInfo.UserID, addNote, targetID: curUser.UserID);
                apiResp.status = true;
                apiResp.msg = event_name+"成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = event_name + "失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllUser.ContextResponse(context, apiResp);
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