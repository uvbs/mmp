using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 万邦 基地信息表
    /// </summary>
    public class WBBaseInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 基地名称
        /// </summary>
        public string BaseName { get; set; }
        /// <summary>
        /// 基地缩略图
        /// </summary>
        public string Thumbnails { get; set; }
        /// <summary>
        /// 基地地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 基地所属区县
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 基地电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 基地负责人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// 基地面积
        /// </summary>
        public string Acreage { get; set; }
        /// <summary>
        ///援助对象人数 
        /// </summary>
        public int HelpCount { get; set; }
        /// <summary>
        /// 基地介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否禁用 0正常 1已禁用
        /// </summary>
        public int IsDisable { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }


    }
}
