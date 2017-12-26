using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 抽奖活动列表
    /// </summary>
    public class WXLotteryV1 : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号标识
        /// </summary>
        public int LotteryID { get; set; }

        /// <summary>
        /// 抽奖活动名称
        /// </summary>
        public string LotteryName { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumbnailsPath { get; set; }

        /// <summary>
        /// 刮奖内容
        /// </summary>
        public string LotteryContent { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 状态 0表示已经停止 1表示正在进行
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 每个用户最多刮奖数量
        /// </summary>
        public int MaxCount { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public string BackGroundColor { get; set; }
        /// <summary>
        /// 微信分享图片
        /// </summary>
        public string ShareImg { get; set; }
        /// <summary>
        /// 微信分享描述
        /// </summary>
        public string ShareDesc { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 是否在手机端领奖 0 后台设置已领奖 1移动端直接领奖 2提交信息领奖
        /// </summary>
        public int IsGetPrizeFromMobile { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 抽奖上限类型：默认0每用户多少次，1为每天多少次
        /// </summary>
        public int LuckLimitType { get; set; }
        /// <summary>
        /// 每次消耗积分
        /// </summary>
        public int UsePoints { get; set; }
        /// <summary>
        /// 中奖上限类型：默认0为一次，1为多次
        /// </summary>
        public int WinLimitType { get; set; }
        /// <summary>
        /// 奖项
        /// </summary>
        public List<WXAwardsV1> Awards { get; set; }

        /// <summary>
        /// 底部工具栏
        /// </summary>
        public string ToolbarButton { get; set; }


        /// <summary>
        /// 类型  shake摇一摇   scratch刮刮奖   luckydraw 抽奖
        /// </summary>
        public string LotteryType { get; set; }

        /// <summary>
        /// 参与人数
        /// </summary>
        public int PersionCount
        {

            get
            {
                return (int)ZentCloud.ZCBLLEngine.BLLBase.GetSingle(string.Format("select count(distinct UserId) from ZCJ_WXLotteryLogV1 where lotteryid={0}", LotteryID));

            }
        }

        public int PV { get; set; }
        public int IP { get; set; }
        public int UV { get; set; }

        /// <summary>
        /// 中奖名单  0隐藏 1左侧显示 2 右侧显示
        /// </summary>
        public int IsHideWinningList { get; set; }

        /// <summary>
        /// 抽奖人数
        /// </summary>
        public int WinnerCount { get; set; }

        /// <summary>
        /// 背景图片
        /// </summary>
        public string BackGroudImg { get; set; }

        /// <summary>
        /// 是否显示二维码  0显示 1隐藏
        /// </summary>
        public int IsHideQRCode { get; set; }


        /// <summary>
        /// 是否显示标题
        /// </summary>
        public int IsHideTitle { get; set; }

        /// <summary>
        /// 主题字体颜色
        /// </summary>
        public string TitleFontColor { get; set; }
        /// <summary>
        /// 抽奖用户背景颜色
        /// </summary>

        public string UserBackGroudColor { get; set; }

        /// <summary>
        /// 一次抽中数量,最高10个
        /// </summary>
        public int OneWinnerCount { get; set; }

        /// <summary>
        /// 二维码  0关注公众号二维码 1页面二维码
        /// </summary>
        public int QRCode { get; set; }


        /// <summary>
        /// 分销员用户id
        /// </summary>

        public string DistributorUserId { get; set; }

        /// <summary>
        /// 抽奖中音乐
        /// </summary>
        public string StartMusic { get; set; }

        /// <summary>
        /// 中奖中音乐
        /// </summary>
        public string StopMusic { get; set; }

    }
}
