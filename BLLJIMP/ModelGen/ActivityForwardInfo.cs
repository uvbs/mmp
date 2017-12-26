using System;


namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 活动转发
    /// </summary>
    [Serializable]
    public class ActivityForwardInfo : ZCBLLEngine.ModelTable
    {
        public ActivityForwardInfo() { }

        #region Model
        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 转发人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 转发时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 转发时间
        /// </summary>
        public string InsertDateStr
        {
            get { return InsertDate.ToString("yyyy-MM-dd"); }
        }


        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadNum { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumbnailsPath { get; set; }

        /// <summary>
        /// 转发人数
        /// </summary>
        public int ForwarNum { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public int Mid { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// 是否转发
        /// </summary>
        public string IsForwar { get; set; }
        /// <summary>
        /// 类型  微吸粉fans   微转发:默认为null  问卷:questionnaire
        /// </summary>
        public string ForwardType { get; set; }

        /// <summary>
        /// /粉丝人数
        /// </summary>
        public int FansCount { get; set; }

        /// <summary>
        /// 微信阅读数量
        /// </summary>

        public int UV { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int PV { get; set; }

        /// <summary>
        /// 推广人用户id
        /// </summary>
        public string CurrentUserId { get; set; }

        #endregion

    }
}
