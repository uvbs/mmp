using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 关注表
    /// </summary>
    public partial class UserFollowChain : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 关注者信息 关注人是导师，理财师
        /// </summary>
        public TutorInfo FromTutorInfo
        {

            get
            {

                try
                {
                    BLL bll = new BLL();
                    return bll.Get<TutorInfo>(string.Format("UserId='{0}'",FromUserId));

                }
                catch (Exception)
                {
                    return null;
                    
                }

            }

        }
        /// <summary>
        /// 被关注者信息 被关注人是导师,理财师
        /// </summary>
        public TutorInfo ToTutorInfo
        {

            get
            {

                try
                {
                    BLL bll = new BLL();
                    return bll.Get<TutorInfo>(string.Format("UserId='{0}'", ToUserId));

                }
                catch (Exception)
                {
                    return null;

                }

            }

        }
        /// <summary>
        /// 关注者信息
        /// </summary>
        public UserInfo FromUserInfo
        {

            get
            {

                try
                {
                    BLLUser bll = new BLLUser("");
                    return bll.GetUserInfo(FromUserId);

                }
                catch (Exception)
                {
                    return null;

                }

            }

        }
        /// <summary>
        /// 被关注者信息
        /// </summary>
        public UserInfo ToUserInfo
        {

            get
            {

                try
                {
                    BLLUser bll = new BLLUser("");
                    return bll.GetUserInfo(ToUserId);

                }
                catch (Exception)
                {
                    return null;

                }

            }

        }
        
    }
}
