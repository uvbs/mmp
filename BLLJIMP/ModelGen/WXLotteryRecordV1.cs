using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 中奖记录表
    /// </summary>
    public class WXLotteryRecordV1 : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 
        /// </summary>
        BLLUser bllUser = new BLLUser("");
      
        /// <summary>
        /// 自动编号标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 抽奖活动 id
        /// </summary>
        public int LotteryId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 奖项Id 对应ZCJ_WXAwardsV1
        /// </summary>
        public int WXAwardsId { get; set; }

        /// <summary>
        /// 中奖码
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 中奖日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 中奖日期
        /// </summary>
        public string InsertDateStr { get { return InsertDate.ToString(); } }

        /// <summary>
        /// 中奖奖项
        /// </summary>
        public WXAwardsV1 WXAward
        {
            get
            {
                try
                {

                    return bllUser.Get<WXAwardsV1>(string.Format("AutoID={0}", WXAwardsId));
                }
                catch (Exception)
                {

                    return null;
                }

            }
        }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string WXAwardName
        {
            get
            {
                return WXAward == null ? "" : WXAward.PrizeName;

            }
        }
        /// <summary>
        /// 中奖用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get
            {
                var data = bllUser.GetUserInfo(UserId);
                if (data != null)
                {
                    data.Password = null;
                    data.WeixinAPIUrl = null;
                    data.WeixinAppId = null;
                    data.WeixinAppSecret = null;
                    data.WXOpenId = null;
                    data.Address = null;
                    data.Province = null;
                    data.City = null;
                    data.District = null;
                    data.AutoID = 0;
                    data.Email = null;
                    data.Postion = null;
                    data.Company = null;

                    return data;
                }
                else
                {
                    return new UserInfo();
                }


            }

        }

        /// <summary>
        /// 是否已经领奖 0未领 1已领
        /// </summary>
        public int IsGetPrize { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string WXNickName { get { return UserInfo.WXNickname; } }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get { return UserInfo.WXHeadimgurlLocal; } }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        public string ShowUserName
        {
            get
            {

                if (string.IsNullOrEmpty(Name))
                {

                    return bllUser.GetUserDispalyName(UserInfo);

                }
                else
                {
                    return Name;
                }

            }
        }
        public string ShowUserPhone
        {
            get
            {

                if (string.IsNullOrEmpty(Phone))
                {
                    return UserInfo.Phone;

                }
                else
                {
                    return Phone;
                }

            }
        }

    }
}
