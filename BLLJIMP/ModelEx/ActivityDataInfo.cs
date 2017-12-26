using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ActivityDataInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public partial class ActivityDataInfo : ZCBLLEngine.ModelTable
    {
        BLLUser bllUser = new BLLUser();
        public string InsertDateStr { get { return InsertDate.ToString(); } set { } }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string WXHeadimgurlLocal
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(WeixinOpenID))
                        return "";
                    return new BLLUser("").GetUserInfoByOpenId(WeixinOpenID).WXHeadimgurlLocal;

                }
                catch (Exception)
                {

                    return "";
                }
            }
        }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WXNickname
        {

            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(WeixinOpenID))
                        return "";
                    return new BLLUser("").GetUserInfoByOpenId(WeixinOpenID).WXNickname;
                }
                catch (Exception)
                {
                    return "";

                }
            }

        }

        ///// <summary>
        ///// 报名用户所属鸿风用户组 无用
        ///// </summary>
        //public string HFUserPmsGroup
        //{
        //    get
        //    {
        //        //try
        //        //{
        //        //    if (string.IsNullOrWhiteSpace(WeixinOpenID))
        //        //        return "";
        //        //    return new BLLUser("").GetUserInfoByOpenId(WeixinOpenID).HFUserPmsGroup;

        //        //}
        //        //catch (Exception)
        //        //{

        //        //    return "";
        //        //}
        //        return "";
        //    }
        //}

        /// <summary>
        /// 报名用户ID
        /// </summary>
        public string SignUpUserID
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(WeixinOpenID))
                        return "";
                    return bllUser.GetUserInfoByOpenId(WeixinOpenID).UserID;

                }
                catch (Exception)
                {

                    return "";
                }
            }
        }

        public string SignInDateStr
        {
            get
            { //显示签到时间
                string result = "";
                try
                {

                    BLLJuActivity bll = new BLLJuActivity();

                    int jid = bll.GetJuActivityByActivityID(this.ActivityID).JuActivityID;

                    WXSignInInfo signUpData = bll.Get<WXSignInInfo>(string.Format(" (SignInOpenID = '{0}' Or SignInUserID='{1}') AND JuActivityID = {2} ",
                            this.WeixinOpenID,
                            UserId,
                            jid
                        ));

                    if (signUpData != null)
                        result = signUpData.SignInTime.ToString();

                }
                catch
                {
                    result = "";
                }

                return result;
            }
        }
        /// <summary>
        /// 距离
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// 线下分销推荐人名字
        /// </summary>
        public string DistributionOffLineRecommendName { get; set; }
        /// <summary>
        /// 推荐人名字
        /// </summary>
        public string SpreadUserTrueName
        {
            get
            {
                if (!string.IsNullOrEmpty(SpreadUserID))
                {
                    if (SpreadUserID=="system")
                    {
                        return "帮会员报名";
                    }
                    var user = bllUser.GetUserInfo(SpreadUserID);
                    return user==null? "":user.TrueName;
                }
                return "";
            }
        }
        /// <summary>
        /// 分享推荐人用户名
        /// </summary>
        public string ShareUserName
        {
            get
            {
                if (!string.IsNullOrEmpty(ShareUserID))
                {
                    var shareUser = bllUser.GetUserInfo(ShareUserID);
                    return bllUser.GetUserDispalyName(shareUser);
                }
                return "";
            }
        }

        public int AutoID { 
            
            get 
            {
                int result = 0;
                if (!string.IsNullOrEmpty(UserId))
                {
                    var user = bllUser.GetUserInfo(UserId);
                    result = user == null ? 0 : user.AutoID;
                }
                return result;
            }
        }

    }
}
