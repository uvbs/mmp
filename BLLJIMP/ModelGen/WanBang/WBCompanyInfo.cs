using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 万邦 企业信息
    /// </summary>
    public class WBCompanyInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 企业缩略图
        /// </summary>
        public string Thumbnails { get; set; }
        /// <summary>
        /// 企业地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 企业所属区县
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 企业电话
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
        /// 企业负责人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// 企业营业执照或证件号码
        /// </summary>
        public string BusinessLicenseNumber{get;set;}
        /// <summary>
        /// 企业介绍
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
