using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class ArticleCategoryTypeConfig : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string CategoryType { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        
        /// <summary>
        /// 微信标题
        /// </summary>
        public string CategoryTypeTitle { get; set; }
        /// <summary>
        /// 首页标题
        /// </summary>
        public string CategoryTypeHomeTitle { get; set; }
        /// <summary>
        /// 订单列表标题
        /// </summary>
        public string CategoryTypeOrderListTitle { get; set; }
        /// <summary>
        /// 订单详情标题
        /// </summary>
        public string CategoryTypeOrderDetailTitle { get; set; }
        /// <summary>
        /// 商品别名
        /// </summary>
        public string CategoryTypeDispalyName { get; set; }
        /// <summary>
        /// 分类别名
        /// </summary>
        public string CategoryTypeExDispalyName { get; set; }
        /// <summary>
        /// 库存别名
        /// </summary>
        public string CategoryTypeStockName { get; set; }
        /// <summary>
        /// 消费方式 0金额和积分组合 1金额支付 2积分支付
        /// </summary>
        public int SpendMethod { get; set; }
        /// <summary>
        /// 时间设置方式 1勾选固定时间段 2直接选择时间段
        /// 
        /// 通用网点 表示客户端地图显示方式 0无地图 1详情跳转地图 2列表跳转地图
        /// </summary>
        public int TimeSetMethod { get; set; }
        /// <summary>
        /// 计费方式
        /// </summary>
        public int BillingMethod { get; set; }
        /// <summary>
        /// 列表字段
        /// 
        ///通用网点 后台查询条件 选项字段涉及
        /// </summary>
        public string ListFields { get; set; }
        /// <summary>
        /// 编辑字段
        /// 
        ///通用网点 后台查询条件 指定字段涉及
        /// </summary>
        public string EditFields { get; set; }
        /// <summary>
        /// 必填字段
        /// 
        ///通用网点 后台查询条件 关键字涉及
        /// </summary>
        public string NeedFields { get; set; }
        /// <summary>
        /// 幻灯片宽
        /// </summary>
        public int SlideWidth { get; set; }
        /// <summary>
        /// 幻灯片高
        /// </summary>
        public int SlideHeight { get; set; }
        /// <summary>
        /// 分享标题
        /// </summary>
        public string ShareTitle { get; set; }
        /// <summary>
        /// 分享图片
        /// </summary>
        public string ShareImg { get; set; }
        /// <summary>
        /// 分享描述
        /// </summary>
        public string ShareDesc { get; set; }
        /// <summary>
        /// 分享链接
        /// </summary>
        public string ShareLink { get; set; }
        /// <summary>
        /// 时间展示方式
        /// </summary>
        public int TimeSetStyle { get; set; }

        /// <summary>
        ///通用网点 前端查询条件 选项字段 
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        ///通用网点 前端查询条件 Keyword涉及字段
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        ///后台列表字段
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        ///前端列表字段
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        ///前端详情字段
        /// </summary>
        public string Ex5 { get; set; }
        
        /// <summary>
        ///前端页面
        /// </summary>
        public string AppPagePath { get; set; }
    }
}