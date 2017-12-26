using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 签到地址
    /// </summary>
    [Serializable]
    public partial class SignInAddress : ModelTable
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 范围(米)
        /// </summary>
        public double Range { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 签到成功后跳转地址
        /// </summary>
        public string SignInSuccessUrl { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        public string SignInTime { get; set; }

        /// <summary>
        /// 签到类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 签到说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 抽奖id[关联]
        /// </summary>
        public string LotteryId { get; set; }

        /// <summary>
        /// 抽奖类型[scratch刮刮奖、shake摇一摇]
        /// </summary>
        public string LotteryType { get; set; }

        /// <summary>
        /// 周一签到得积分
        /// </summary>
        public int MondayScore { get; set; }

        /// <summary>
        /// 周二签到得积分
        /// </summary>
        public int TuesdayScore { get; set; }

        /// <summary>
        /// 周三签到得积分
        /// </summary>
        public int WednesdayScore { get; set; }

        /// <summary>
        /// 周四签到得积分
        /// </summary>
        public int ThursdayScore { get; set; }
        
        /// <summary>
        /// 周五签到得积分
        /// </summary>
        public int FridayScore { get; set; }

        /// <summary>
        /// 周六签到得积分
        /// </summary>
        public int SaturdayScore { get; set; }

        /// <summary>
        /// 周日签到得积分
        /// </summary>
        public int SundayScore { get; set; }

        /// <summary>
        /// 签到按钮颜色
        /// </summary>
        public string ButtonColor { get; set; }

        /// <summary>
        /// 奖品缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 第一次补签
        /// </summary>
        public int RetroactiveToOne { get; set; }

        /// <summary>
        /// 第二次补签
        /// </summary>
        public int RetroactiveToTwo { get; set; }

        /// <summary>
        /// 第三次补签
        /// </summary>
        public int RetroactiveToThree { get; set; }

        /// <summary>
        /// 第四次补签
        /// </summary>
        public int RetroactiveToFour { get; set; }

        /// <summary>
        /// 第五次补签
        /// </summary>
        public int RetroactiveToFive { get; set; }

        /// <summary>
        /// 第六次补签
        /// </summary>
        public int RetroactiveToSix { get; set; }

        /// <summary>
        /// 第七次补签
        /// </summary>
        public int RetroactiveToSeven { get; set; }

        /// <summary>
        /// 幻灯片组名  对应ZCJ_Slide Type
        /// </summary>
        public string SlideGroupName { get; set; }

        /// <summary>
        /// 签到背景图片
        /// </summary>
        public string BackGroundImage { get; set; }

        /// <summary>
        /// 已签到图片
        /// </summary>
        public string HaveSignImage {get;set; }

        /// <summary>
        /// 未签到图片
        /// </summary>
        public string NoHaveSignImage { get; set; }
        /// <summary>
        /// 周一广告
        /// </summary>
        public string MondayAds { get; set; }

        /// <summary>
        /// 周二广告
        /// </summary>
        public string TuesdayAds { get; set; }
        /// <summary>
        /// 周三广告
        /// </summary>
        public string WednesdayAds { get; set; }

        /// <summary>
        /// 周四广告
        /// </summary>
        public string ThursdayAds { get; set; }

        /// <summary>
        /// 周五广告
        /// </summary>
        public string FridayAds { get; set; }

        /// <summary>
        /// 周六广告
        /// </summary>
        public string SaturdayAds { get; set; }

        /// <summary>
        /// 周日广告
        /// </summary>
        public string SundayAds { get; set; }
    }
}
