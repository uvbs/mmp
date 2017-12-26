using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// Reward 的摘要说明
    /// </summary>
    public class Reward : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUserScoreDetailsInfo bll = new BLLJIMP.BLLUserScoreDetailsInfo();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLJuActivity bllActivity = new BLLJIMP.BLLJuActivity();
        string scoreName = "积分";
        string actionName = "赠送";
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            double score = Convert.ToDouble(context.Request["score"]);
            if (!string.IsNullOrWhiteSpace(context.Request["score_name"])) scoreName = context.Request["score_name"];
            if (!string.IsNullOrWhiteSpace(context.Request["action_name"])) actionName = context.Request["action_name"];



            int score1 = 0;
            if(!int.TryParse(score.ToString(), out score1)){
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = scoreName + "只能输入正整数";
                bll.ContextResponse(context, apiResp);
                return;
            }

            #region 不能小于等于0
            if (score <= 0)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = actionName + scoreName + "不能小于等于0";
                bll.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            double min = Convert.ToDouble(context.Request["min"]);
            #region 最小值限制
            if (score < min){
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = actionName+ scoreName + "不能少于" + min;
                bll.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            int idInt = Convert.ToInt32(id);

            if(CurrentUserInfo.TotalScore<score){
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "账号"+scoreName+"不足";
                bll.ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.Model.JuActivityInfo activity = bllActivity.GetJuActivity(idInt, false, bllActivity.WebsiteOwner);
            if (activity == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "文章未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.UserID == activity.UserID)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "禁止" + actionName + "自己";
                bll.ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.Model.UserInfo author = bllUser.GetUserInfo(activity.UserID, bllUser.WebsiteOwner);

            string msg = "";
            bool result = bll.RewardJuActivity(CurrentUserInfo, author, activity, score, bll.WebsiteOwner, out msg, scoreName, actionName);

            apiResp.status = result;
            apiResp.code = result ? (int)BLLJIMP.Enums.APIErrCode.IsSuccess :(int)BLLJIMP.Enums.APIErrCode.OperateFail;
            apiResp.msg = msg;
            bll.ContextResponse(context, apiResp);
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