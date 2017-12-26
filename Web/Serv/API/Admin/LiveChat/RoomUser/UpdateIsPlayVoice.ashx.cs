using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.LiveChat.RoomUser
{
    /// <summary>
    /// 下线下线
    /// </summary>
    public class UpdateIsPlayVoice : BaseHandlerNeedLoginAdminNoAction
    {

        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string isPlayVoice = context.Request["is_play_voice"];
            apiResp.status = bllUser.Update(new UserInfo(), string.Format("Ex15='{0}'", isPlayVoice), string.Format("AutoID={0}", currentUserInfo.AutoID)) > 0;
            bllUser.ContextResponse(context, apiResp);

        }


    }
}