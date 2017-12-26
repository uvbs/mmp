using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微秀
    /// </summary>
    public class WXShowInfo : ZCBLLEngine.ModelTable
    {
        public WXShowInfo() { }

        public int AutoId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string ShowDescription { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ShowImg { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string ShowUrl { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        ///站点所有者
        /// </summary>
        public string websiteOwner { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 音乐
        /// </summary>
        public string ShowMusic { get; set; }
        /// <summary>
        /// 阅读数
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// 发送给朋友数量
        /// </summary>
        public int ShareAppMessageCount { get; set; }
        /// <summary>
        /// 分享到朋友圈数量
        /// </summary>
        public int ShareTimelineCount { get; set; }
        /// <summary>
        /// 自动播放时间间隔秒数：1、2、3，默认0为不自动播放
        /// </summary>
        public int AutoPlayTimeSpan { get; set; }

        public List<BLLJIMP.Model.WXShowImgInfo> WXShowImgInfos { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// 微信阅读人数
        /// </summary>
        public int UV { get; set; }
    }
}
