using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class Log : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自增ID
        /// </summary>
        public long AutoID
        {
            set;
            get;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set;
            get;
        }
        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            set;
            get;
        }
        /// <summary>
        /// IP所在地
        /// </summary>
        public string IPLocation
        {
            set;
            get;
        }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime InsertDate
        {
            set;
            get;
        }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser
        {
            set;
            get;
        }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string BrowserID
        {
            set;
            get;
        }
        /// <summary>
        /// 浏览器版本
        /// </summary>
        public string BrowserVersion
        {
            set;
            get;
        }
        /// <summary>
        /// 浏览器是否测试版
        /// </summary>
        public string BrowserIsBata
        {
            set;
            get;
        }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string SystemPlatform
        {
            set;
            get;
        }
        /// <summary>
        /// 系统位数
        /// </summary>
        public string SystemByte
        {
            set;
            get;
        }
        /// <summary>
        /// UA
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///操作
        /// </summary>
        public string Action { get; set; }


        /// <summary>
        /// 订单id
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 微信openId
        /// </summary>
        public string WxOpenId { get; set; }
        /// <summary>
        /// 目标ID
        /// </summary>
        public string TargetID { get; set; }
    }
}
