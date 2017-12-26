using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.LiveChat.RoomUser
{
    /// <summary>
    /// 下线
    /// </summary>
    public class OffLine : BaseHandlerNeedLoginAdminNoAction
    {

        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {

            apiResp.status=bllUser.Update(new UserInfo(), "IsOnLine=0", string.Format("AutoID={0}", currentUserInfo.AutoID))>0;
            bllUser.ContextResponse(context, apiResp);

        }

       
    }
}