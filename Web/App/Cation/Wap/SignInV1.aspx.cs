using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    /// <summary>
    /// 只有客服才能签到
    /// </summary>
    public partial class SignInV1 : System.Web.UI.Page
    {
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLActivity bllActivity = new BLLActivity("");
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 活动报名数据
        /// </summary>
        public ActivityDataInfo data = new ActivityDataInfo();
        /// <summary>
        /// 字段
        /// </summary>
        public List<ActivityFieldMappingInfo> Mapping = new List<ActivityFieldMappingInfo>();
        /// <summary>
        /// 活动信息
        /// </summary>
        public JuActivityInfo juActivity;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!bllJuactivity.IsLogin)
                {
                    Response.End();
                    return;
                }
                int activityId = Convert.ToInt32(Request["activityid"]);
                int uid = Convert.ToInt32(Request["uid"]);
                UserInfo currentUser = bllUser.GetCurrentUserInfo();
                if (bllJuactivity.GetCount<WXKeFu>(string.Format("WebsiteOwner='{0}' And WeiXinOpenID='{1}'", bllUser.WebsiteOwner, currentUser.WXOpenId)) == 0)
                {
                    Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"/></head><body><h1>对不起，您不能执行签到操作！</h1></body></html>");
                    Response.End();
                    return;
                }

                //查询活动是否存在
                juActivity = bllJuactivity.Get<JuActivityInfo>(string.Format("SignUpActivityID={0}", activityId));
                if (juActivity == null)
                {
                    Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"/></head><body><h1>对不起，您签到的活动不存在！</h1></body></html>");
                    Response.End();
                    return;
                }
                data = bllJuactivity.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' And UID={1} And IsDelete=0", activityId, uid));
                if (data == null)
                {
                    Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"/></head><body><h1>请先报名再签到.</h1></body></html>");
                    Response.End();
                    return;
                }

                UserInfo signUserInfo = bllUser.GetUserInfoByOpenId(data.WeixinOpenID);
                if (signUserInfo == null)
                {
                    signUserInfo = bllUser.GetUserInfo(data.UserId);
                }
                if (signUserInfo == null)
                {
                    Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"/></head><body><h1>不存在的报名数据！</h1></body></html>");
                    Response.End();
                    return;
                }
                if (!string.IsNullOrEmpty(data.ToUserId))
                {
                     Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"/></head><body><h1>此活动已转赠,不能签到!</h1></body></html>");
                    Response.End();
                    return;
                }

                WXSignInInfo signInInfo = new WXSignInInfo();
                signInInfo.SignInUserID = signUserInfo.UserID;
                signInInfo.JuActivityID = juActivity.JuActivityID;
                signInInfo.SignInOpenID = signUserInfo.WXOpenId;
                signInInfo.SignInTime = DateTime.Now;
                signInInfo.Name = data.Name;
                signInInfo.Phone = data.Phone;
                //判断是否已经签到过
                if (this.bllJuactivity.Exists(signInInfo, new List<string>() { "SignInUserID", "JuActivityID" }))
                {
                    Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"/></head><body><h1>已经签过到了！</h1></body></html>");
                    Response.End();
                    return;
                }
                if (this.bllJuactivity.Add(signInInfo))
                {

                    //显示信息
                    Mapping = bllActivity.GetActivityFieldMappingList(activityId.ToString());
                    bllUser.AddUserScoreDetail(signUserInfo.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.SignIn), signUserInfo.WebsiteOwner, null, null);


                }
                else
                {
                    Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\"/></head><body><h1>签到失败！</h1></body></html>");
                    Response.End();
                    return;
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                Response.End();
            }

        }
    }
}