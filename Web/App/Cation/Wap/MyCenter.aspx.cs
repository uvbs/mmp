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
    public partial class MyCenter : System.Web.UI.Page
    {
        /// <summary>
        /// 活动主id
        /// </summary>
        public string jid;
        /// <summary>
        ///活动ID
        /// </summary>
        public string activityid;
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name;
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();  //活动数据
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 活动模型
        /// </summary>
        public BLLJIMP.Model.JuActivityInfo juActivityInfo;
        /// <summary>
        /// 
        /// </summary>
        public string Uid;
        /// <summary>
        /// 
        /// </summary>
        public string CodeStr;
        /// <summary>
        /// 签到二维码链接
        /// </summary>
        public string Code = " ";
        /// <summary>
        /// 是否显示转赠
        /// </summary>
        public bool ShowDonation;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo CurrentUserInfo;
        /// <summary>
        /// 接收用户信息
        /// </summary>
        public UserInfo ReceiveUserInfo;
        /// <summary>
        /// 赠送用户信息
        /// </summary>
        public UserInfo FromUserInfo;
        /// <summary>
        /// 活动配置
        /// </summary>
        public BLLJIMP.Model.ActivityConfig ActivityConfig;
        /// <summary>
        /// 是否已经签到
        /// </summary>
        public bool IsSignIn;
        protected void Page_Load(object sender, EventArgs e)
        {

            activityid = Request["activityid"];
            Name = Request["name"];
            Uid = Request["Uid"];
            jid = Request["jid"];
            CurrentUserInfo = bllUser.GetCurrentUserInfo();
            ActivityDataInfo activityData = bllJuactivity.Get<ActivityDataInfo>(string.Format(" ActivityID='{0}' And (WeixinOpenID='{1}' Or UserId='{2}') Order By InsertDate DESC", activityid, CurrentUserInfo.WXOpenId, CurrentUserInfo.UserID));
            ActivityConfig = bllJuactivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format("WebsiteOwner='{0}'", bllUser.WebsiteOwner));
            if (CurrentUserInfo.UserID != activityData.UserId)
            {
                Response.Write("无权查看");
                Response.End();
            }
            string[] showFields = new string[] { };//显示字段
            if (ActivityConfig != null)
            {
                showFields = ActivityConfig.RegisterCode.Split(',');
            }
            else
            {
                ActivityConfig = new BLLJIMP.Model.ActivityConfig();
            }
            Name = activityData == null ? Request["name"] : activityData.Name;
            Uid = activityData == null ? Request["Uid"] : activityData.UID.ToString();
            foreach (string item in showFields)//文本型签到
            {
                if (!string.IsNullOrEmpty(item))
                {
                    switch (item)
                    {
                        case "0":
                            CodeStr = activityid + Uid;
                            Code += "   " + CodeStr;
                            break;
                        case "1":
                            Code += "   " + activityData.Name;
                            break;
                        case "2":
                            Code += "   " + activityData.Phone;
                            break;
                        case "3":
                            Code += "   " + activityData.ActivityID;
                            break;
                    }
                }
            }//文本型签到

            if (ActivityConfig.QCodeType.Equals(1))//二维码签到
            {
                Code = string.Format("http://{0}/App/Cation/Wap/SignInV1.aspx?activityid={1}&uid={2}", Request.Url.Host, activityData.ActivityID, Uid);
            }

            CodeStr = activityid + Uid;
            juActivityInfo = bllJuactivity.Get<JuActivityInfo>(string.Format(" SignUpActivityID='{0}'", activityid));
            if (juActivityInfo != null)
            {

                txtTitle.Text = juActivityInfo.ActivityName;
                txtStartDate.Text = (juActivityInfo.ActivityEndDate != null ? "开始时间:&nbsp;" : "") + string.Format("{0:f}", juActivityInfo.ActivityStartDate);
                if (juActivityInfo.ActivityEndDate != null)
                {
                    txtEndDate.Text = "结束时间:&nbsp;" + string.Format("{0:f}", juActivityInfo.ActivityEndDate);

                }
                else
                {
                    txtEndDate.Visible = false;
                }
                txtAddress.Text = juActivityInfo.ActivityAddress;

            }
            WXSignInInfo signInfo = bllJuactivity.Get<WXSignInInfo>(string.Format(" JuActivityID='{0}' And SignInUserID='{1}'", juActivityInfo.JuActivityID, CurrentUserInfo.UserID));
            if (activityData.PaymentStatus == 1 && (string.IsNullOrEmpty(activityData.ToUserId)) && (string.IsNullOrEmpty(activityData.FromUserId)))
            {

                if (signInfo == null)
                {
                    ShowDonation = true;
                }

            }
            if (signInfo!=null)
            {
                IsSignIn = true;//已签到
            }

            if (!string.IsNullOrEmpty(activityData.ToUserId))
            {
                ReceiveUserInfo = bllUser.GetUserInfo(activityData.ToUserId);
                ReceiveUserInfo.TrueName = bllUser.GetUserDispalyName(ReceiveUserInfo);

            }
            if (!string.IsNullOrEmpty(activityData.FromUserId))
            {
                FromUserInfo = bllUser.GetUserInfo(activityData.FromUserId);
                FromUserInfo.TrueName = bllUser.GetUserDispalyName(FromUserInfo);
            }






        }




    }
}