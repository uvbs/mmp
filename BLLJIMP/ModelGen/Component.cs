using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 页面配置
    /// </summary>
    [Serializable]
    public partial class Component : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 页面id   主键
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 组件库Id
        /// </summary>
        public int ComponentModelId { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string ComponentName { get; set; }
        /// <summary>
        /// 页面分类
        /// </summary>
        public string ComponentType { get; set; }

        /// <summary>
        /// 页面配置
        /// </summary>
        public string ComponentConfig { get; set; }

        /// <summary>
        /// 子页面id   多个用逗号隔开
        /// </summary>
        public string ChildComponentIds { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Decription { get; set; }
        /// <summary>
        /// 页面代码
        /// </summary>
        public string ComponentKey { get; set; }
        /// <summary>
        /// 是否微信高级授权
        /// </summary>
        public int IsWXSeniorOAuth { get; set; }
        /// <summary>
        /// 访问等级
        /// </summary>
        public int AccessLevel { get; set; }
        /// <summary>
        /// 是否初始化数据
        /// </summary>
        public int IsInitData { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public int ComponentTemplateId { get; set; }
    }
}
