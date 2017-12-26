using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 活动全局配置
    /// </summary>
    public class ActivityConfig : ZCBLLEngine.ModelTable
    {
        public ActivityConfig()
        { }
        public int AutoId { get; set; }
        /// <summary>
        /// 主办方名称
        /// </summary>
        public string OrganizerName { get; set; }
        /// <summary>
        /// 主办方链接
        /// </summary>
        public string TheOrganizers { get; set; }
        /// <summary>
        /// 活动列表名称
        /// </summary>
        public string ActivitiesName { get; set; }
        /// <summary>
        /// 活动列表链接
        /// </summary>
        public string Activities { get; set; }
        /// <summary>
        ///个人报名活动列表名称
        /// </summary>
        public string MyRegistrationName { get; set; }
        /// <summary>
        ///个人活动报名活动链接
        /// </summary>
        public string MyRegistration { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 文本签到码 显示字段
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 二维码类型 0文本二维码 1链接二维码
        /// </summary>
        public int QCodeType { get; set; }
        /// <summary>
        /// 活动样式 0普版 1紧凑版
        /// </summary>
        public int ActivityStyle { get; set; }
        /// <summary>
        /// 颜色主题：0默认，1绿色版
        /// </summary>
        public int ColorTheme { get; set; }
        /// <summary>
        /// 是否显示停止的活动 0显示  1不显示
        /// </summary>
        public int IsShowHideActivity { get; set; }
        /// <summary>
        /// 活动 课程 显示名称
        /// </summary>
        public string ShowName { get; set; }
        /// <summary>
        /// 入场券显示名称
        /// </summary>
        public string TicketShowName { get; set; }
        /// <summary>
        /// 导航分组名称
        /// </summary>
        public string ToolBarGroups { get; set; }


    }
}
