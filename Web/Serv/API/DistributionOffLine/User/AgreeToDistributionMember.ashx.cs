using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.User
{
    /// <summary>
    /// 同意成为分销员
    /// </summary>
    public class AgreeToDistributionMember : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(CurrentUserInfo.DistributionOffLinePreUserId))
            {
                apiResp.msg = "您已经是分销员";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            string activityId = bll.GetDistributionOffLineApplyActivityID();
            ActivityDataInfo model = bllActivity.GetActivityDataInfo(activityId,CurrentUserInfo.UserID);
            if (model==null)
            {
                apiResp.msg = "您还未申请";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (model.Status!=4003)//不是微转发审核通过的不给继续操作
            {
                apiResp.msg = "审核未通过";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            UserInfo commendUserInfo = bllUser.GetUserInfoByAutoID(int.Parse(model.DistributionOffLineRecommendCode));
            if (commendUserInfo == null)
            {
                apiResp.msg = "推荐人不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
           
            CurrentUserInfo.DistributionOffLinePreUserId = commendUserInfo.UserID;
            CurrentUserInfo.TrueName = model.Name;
            if (bllUser.Update(CurrentUserInfo))
            {
                //申请通过向申请人和上级提醒通过申请
                bllWeixin.SendTemplateMessageNotifyComm(commendUserInfo, string.Format("恭喜您的会员“{0}”申请财富会员成功", model.Name), "请提醒他关注公众号并帮助他熟悉系统操作吧。", string.Format("http://{0}/App/Distribution/m/index.aspx", context.Request.Url.Host));

                apiResp.status = true;
                apiResp.msg = "ok ";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

            }
            else
            {
                apiResp.msg = "操作失败";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

            }



        }


    }
}