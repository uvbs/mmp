using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 键值对数据结构类型
    /// </summary>
    public enum KeyVauleDataType
    {
        /// <summary>
        /// 省份
        /// </summary>
        Province,
        /// <summary>
        /// 城市
        /// </summary>
        City,
        /// <summary>
        /// 地区
        /// </summary>
        District,
        /// <summary>
        /// 公开公告
        /// </summary>
        OpenClassNotice,
        /// <summary>
        /// 好数据接口APPKEY
        /// </summary>
        HaoServiceAppKey,
        /// <summary>
        /// 模板消息 DataKey微信模板ID DataValue模板名称
        /// </summary>
        WXTmplmsg,
        /// <summary>
        /// 模板消息字段 PreKey模板消息ID DataValue字段名称
        /// </summary>
        WXTmplmsgData,
        /// <summary>
        /// 积分规则类型
        /// </summary>
        ScoreDefineType,
        /// <summary>
        /// 默认开始至结束时长 DataKey 约会Appointment
        /// </summary>
        StartToEndHourNum,
        /// <summary>
        /// 缓存的站点
        /// </summary>
        CacheWebsiteOwner,
        /// <summary>
        /// 全局缓存的表
        /// </summary>
        GlobalCacheTable,
        /// <summary>
        /// 积分别名
        /// </summary>
        ScoreDispalyName
    }
}
