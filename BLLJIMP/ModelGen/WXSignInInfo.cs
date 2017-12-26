using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信签到
    /// </summary>
    public class WXSignInInfo : ZCBLLEngine.ModelTable
    {
        #region ModelBase
        public long? AutoID { get; set; }
        public int JuActivityID { get; set; }
        public string SignInUserID { get; set; }
        public string SignInOpenID { get; set; }
        public DateTime SignInTime { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        #endregion


        #region ModelEx
        /// <summary>
        /// 微信头像
        /// </summary>
        public string WXHeadimgurlLocal
        {
            get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SignInUserID))
                        return "";
                    return new BLLUser("").GetUserInfo(SignInUserID).WXHeadimgurlLocal;
                }
                catch
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
                    if (string.IsNullOrWhiteSpace(SignInUserID))
                        return "";
                    return new BLLUser("").GetUserInfo(SignInUserID).WXNickname;
                }
                catch
                {
                    return "";
                }
            }
        }


        //扩展查询报名的姓名和手机


        #endregion

    }
}
