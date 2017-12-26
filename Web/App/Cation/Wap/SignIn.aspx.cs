using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{

    public partial class SignIn : System.Web.UI.Page
    {
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bll = new BLLJuActivity();
        /// <summary>
        /// 当前用户
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 当前活动信息
        /// </summary>
        public JuActivityInfo juActivityInfo;
        /// <summary>
        /// 是否签到成功
        /// </summary>
        public bool IsSignInSuccess;
        /// <summary>
        /// 是否需要先报名
        /// </summary>
        public bool NeedSignUp;
        /// <summary>
        /// 是否已经签过到了
        /// </summary>
        public bool IsHaveSignIn;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!bll.IsLogin)
                {
                    Response.Write("该活动不支持扫码签到，请联系管理员");
                    Response.End();
                }
                //取得当前访问用户
                //取得要签到的活动            
                //记录签到日志并入库
                int juActivityID = Convert.ToInt32(Request["id"]);
                UserInfo currUser = bll.GetCurrentUserInfo();
                //查询活动是否存在
                 juActivityInfo = bll.GetJuActivity(juActivityID);
                //if (juActivityInfo == null)
                //{
                //    juActivityInfo = bll.GetJuActivityByActivityID(juActivityID.ToString());
                //}
                if (juActivityInfo == null)
                {
                    Response.Write("对不起，您签到的活动不存在！");
                    Response.End();
                }
                ActivityDataInfo dataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID={0} And IsDelete=0 And (WeixinOpenID='{1}' Or UserId='{2}')", juActivityInfo.SignUpActivityID, currUser.WXOpenId,currUser.UserID));
                if (dataInfo == null)
                {
                    //lbMsg.InnerText = "请先报名再签到";
                    NeedSignUp = true;
                    return;
                }


                WXSignInInfo signInInfo = new WXSignInInfo();
                signInInfo.SignInUserID = currUser.UserID;
                signInInfo.Name = dataInfo.Name;
                signInInfo.Phone = dataInfo.Phone;
                signInInfo.JuActivityID = juActivityID;
                signInInfo.SignInOpenID = currUser.WXOpenId;
                signInInfo.SignInTime = DateTime.Now;
                //判断是否已经签到过
                if (this.bll.Exists(signInInfo, new List<string>() { "SignInUserID", "JuActivityID" }))
                {
                    //lbMsg.InnerText = "您已经签过到了！";
                   // return;
                    IsHaveSignIn = true;
                    return;

                }

                if (this.bll.Add(signInInfo))
                {
                    bllUser.AddUserScoreDetail(currUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.SignIn), currUser.WebsiteOwner, null, null);
                    //lbMsg.InnerText = "签到成功！";
                    IsSignInSuccess = true;
                }
                else
                {
                    Response.Write("签到失败");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
            }

        }
    }

}