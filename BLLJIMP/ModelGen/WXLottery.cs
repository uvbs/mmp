using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 抽奖活动列表
    /// </summary>
   public class WXLottery : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号标识
       /// </summary>
       public int AutoID { get; set; }

       /// <summary>
       /// 抽奖活动名称
       /// </summary>
       public string LotteryName { get; set; }

       /// <summary>
       /// 抽奖活动标题
       /// </summary>
       public string LotteryTitle { get; set; }

       /// <summary>
       /// 缩略图
       /// </summary>
       public string ThumbnailsPath { get; set; }

       /// <summary>
       /// 刮奖上方内容
       /// </summary>
       public string ScratchUpAreaContent { get; set; }

       /// <summary>
       /// 刮奖下方内容
       /// </summary>
       public string ScratchDownAreaContent { get; set; }
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
       /// 奖项设置
       /// </summary>
       public string PrizeSet { get; set; }
       /// <summary>
       /// 每个用户最多刮奖数量
       /// </summary>
       public int MaxCount { get; set; }

       /// <summary>
       /// 未中奖提示信息
       /// </summary>
       public string NotWinMessage { get; set; }
       /// <summary>
       /// 未开启提示信息
       /// </summary>
       public string StopMessage { get; set; }
       /// <summary>
       /// 关联活动ID 关联表ZCJ_JuActivityInfo JuActivityID 只有签到过该活动的人才可以刮奖(选填 如果为空则忽略)
       /// </summary>
       public string LotteryActivityID { get; set; }




    }
}
