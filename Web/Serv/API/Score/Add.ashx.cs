using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// 添加积分
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        /// <summary>
        ///文章活动 BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            string id=context.Request["id"];
            string remark = "";
            if (!string.IsNullOrEmpty(id))
            {
                JuActivityInfo juActivityInfo = bllJuactivity.GetJuActivity(int.Parse(id));
                if (juActivityInfo!=null)
                {
                    if (bllUser.AddUserScoreDetail(CurrentUserInfo.UserID, "ShareArticle", bllUser.WebsiteOwner, out remark, null, "《" + juActivityInfo.ActivityName + "》", juActivityInfo.JuActivityID.ToString(), true, juActivityInfo.JuActivityID.ToString(), juActivityInfo.ArticleType))
                    {

                        apiResp.code = (int)APIErrCode.IsSuccess;
                        apiResp.msg = "添加完成";
                        apiResp.status = true;
                    }
                    else
                    {

                    }
                    bllUser.ContextResponse(context, apiResp);
                    return;

                }



            }

            string msg = "";
            if (bllUser.AddUserScoreDetail(CurrentUserInfo.UserID, type, bllUser.WebsiteOwner, out msg,null,remark))
            //if (bllUser.AddUserScoreDetail(CurrentUserInfo.UserID, type, bllUser.WebsiteOwner,out msg))
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "添加完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = msg;
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}