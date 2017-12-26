using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    ///商品表
    /// </summary>
    [Serializable]
    public partial class WXMallProductInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 商城-商品表
        /// </summary>
        public WXMallProductInfo()
        { }
        #region Model
        private string _pid;
        private string _pname;
        private string _pdescription;
        private decimal _price;
        private string _userid;
        private DateTime _insertdate = DateTime.Now;
        /// <summary>
        /// 商品ID
        /// </summary>
        public string PID
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string PName
        {
            set { _pname = value; }
            get { return _pname; }
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 商品概要介绍
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// 商品详细介绍
        /// </summary>
        public string PDescription
        {
            set { _pdescription = value; }
            get { return _pdescription; }
        }
        
        /// <summary>
        /// 原价
        /// </summary>
        public decimal PreviousPrice{get;set;}
        
        /// <summary>
        /// 现价
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }

        /// <summary>
        /// 基础价：用以积分计算，（售价-基础价）* 返积分比例 = 获得的积分
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 创建账号
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }

        /// <summary>
        /// 商品主图
        /// </summary>
        public string RecommendImg { get; set; }

        /// <summary>
        /// 是否上架 
        /// 1 上架 
        /// 0 下架
        /// </summary>
        public string IsOnSale { get; set; }
        /// <summary>
        /// 是否删除 
        /// 1 已删除
        /// 0 未删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 总库存
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// PV
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// 微信阅读人数
        /// </summary>
        public int UV { get; set; }
        /// <summary>
        /// 展示图片1
        /// </summary>
        public string ShowImage1 { get; set; }
        /// <summary>
        /// 展示图片2
        /// </summary>
        public string ShowImage2 { get; set; }
        /// <summary>
        /// 展示图片3
        /// </summary>
        public string ShowImage3 { get; set; }
        /// <summary>
        /// 展示图片4
        /// </summary>
        public string ShowImage4 { get; set; }
        /// <summary>
        /// 展示图片5
        /// </summary>
        public string ShowImage5 { get; set; }
        ///// <summary>
        ///// 门店ID 关联 WXMallStores AutoID
        ///// </summary>
        //public string StoreId { get; set; }

        /// <summary>
        /// 展示图片,多个图片用逗号分隔
        /// </summary>
        public string ShowImage { get; set; }

        /// <summary>
        ///标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        ///排序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否限时特卖商品 
        /// 1 是
        /// 0 否
        /// </summary>
        public int IsPromotionProduct { get; set; }
        /// <summary>
        /// 促销开始时间 无用
        /// </summary>
        public double PromotionStartTime { get; set; }
        /// <summary>
        /// 促销结束时间 无用
        /// </summary>
        public double PromotionStopTime { get; set; }
        /// <summary>
        /// 限时特卖价格 无用
        /// </summary>
        public decimal PromotionPrice { get; set; }
        /// <summary>
        /// 限时特卖库存 无用
        /// </summary>
        public int PromotionStock { get; set; }
        /// <summary>
        /// 商品编码、商品货号
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int SaleCount { get; set; }
        /// <summary>
        /// 商品外部id
        /// </summary>
        public string OutGoodsId { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdate { get; set; }
        /// <summary>
        /// 外部分类id
        /// </summary>
        public string OutCateId { get; set; }
        /// <summary>
        /// 外部季节id 无用
        /// </summary>
        public string OutSeasonId { get; set; }
        /// <summary>
        /// 外部系列id 无用
        /// </summary>
        public string OutSeriesId { get; set; }
        /// <summary>
        /// 外部品牌id 无用
        /// </summary>
        public string OutBrandId { get; set; }
        /// <summary>
        /// 统一邮费
        /// </summary>
        public int UnifiedFreight { get; set; }
        /// <summary>
        /// 运费模板Id 关联表 ZCJ_FreightTemplate
        /// </summary>
        public int FreightTemplateId { get; set; }
        /// <summary>
        /// 限时特卖活动ID 关联表 ZCJ_PromotionActivity
        /// </summary>
        public int PromotionActivityId { get; set; }
        /// <summary>
        /// 关联商品ID
        /// </summary>
        public string RelationProductId { get; set; }
        /// <summary>
        /// 拼团-拼团规则ID 多个ID用逗号分隔
        /// </summary>
        public string GroupBuyRuleIds { get; set; }
        /// <summary>
        /// 扩展文章自定义标题
        /// </summary>
        public string ExArticleTitle_1 { get; set; }
        /// <summary>
        /// 扩展文章id
        /// </summary>
        public string ExArticleId_1 { get; set; }
        /// <summary>
        /// 扩展文章自定义标题
        /// </summary>
        public string ExArticleTitle_2 { get; set; }
        /// <summary>
        /// 扩展文章id
        /// </summary>
        public string ExArticleId_2 { get; set; }

        /// <summary>
        /// 扩展文章自定义标题
        /// </summary>
        public string ExArticleTitle_3 { get; set; }
        /// <summary>
        /// 扩展文章id
        /// </summary>
        public string ExArticleId_3 { get; set; }

        /// <summary>
        /// 扩展文章自定义标题
        /// </summary>
        public string ExArticleTitle_4 { get; set; }
        /// <summary>
        /// 扩展文章id
        /// </summary>
        public string ExArticleId_4 { get; set; }

        /// <summary>
        /// 扩展文章自定义标题
        /// </summary>
        public string ExArticleTitle_5 { get; set; }
        /// <summary>
        /// 扩展文章id
        /// </summary>
        public string ExArticleId_5 { get; set; }

        /// <summary>
        /// 访问等级
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// 类型 
        /// Mall或空为普通商品
        /// MeetingRoom 会议室
        /// MeetingRoomAdded 会议室增值
        /// BookingTutor 导师预约
        /// BookingTutorAdded 导师预约增值
        /// Houses  楼盘
        /// </summary>
        private string _article_category_type = "Mall";
        /// <summary>
        /// 类型 
        /// Mall或空为普通商品
        /// MeetingRoom 会议室
        /// MeetingRoomAdded 会议室增值
        /// BookingTutor 导师预约
        /// BookingTutorAdded 导师预约增值
        /// BookingDoctor 医生预约增值
        /// </summary>
        public string ArticleCategoryType
        {
            set { _article_category_type = value; }
            get { return _article_category_type; }
        }
        /// <summary>
        /// 单位 
        /// </summary>
        private string _unit = "元";
        /// <summary>
        /// 单位 默认 元
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 限制购买时间
        /// </summary>
        public string LimitBuyTime { get; set; }
        /// <summary>
        /// 近一个月销量
        /// </summary>
        public int SaleCountOneMonth { get; set; }
        /// <summary>
        /// 近三个月销量
        /// </summary>
        public int SaleCountThreeMonth { get; set; }
        /// <summary>
        /// 近半年销量
        /// </summary>
        public int SaleCountHalfYear{ get; set; }
        /// <summary>
        ///近一年销量
        /// </summary>
        public int SaleCountOneYear { get; set; }
        /// <summary>
        ///相关商品Id 多个ID用逗号分隔
        /// </summary>
        public string RelevantProductIds { get; set; }
        /// <summary>
        /// 使用积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 金额部分是否仅现金支付
        /// </summary>
        public int IsCashPayOnly { get; set; }

        /// <summary>
        /// 无需物流：1是，0否
        /// </summary>
        public int IsNoExpress { get; set; }
        /// <summary>
        /// 评价平均分
        /// </summary>
        public double ReviewScore { get; set; }
        /// <summary>
        /// 评价人数
        /// </summary>
        public int ReviewCount { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
        /// <summary>
        /// 需要姓名手机
        /// </summary>
        public int IsNeedNamePhone { get; set; }
        /// <summary>
        /// 参与返利比例，默认值 100%
        /// </summary>
        public decimal RebatePriceRate { get; set; }
        /// <summary>
        /// 参与返积分比例，默认值 100%
        /// </summary>
        public decimal RebateScoreRate { get; set; }


        /// <summary>
        /// TAB   ex1
        /// </summary>
        public string TabExTitle1 { get; set; }
        /// <summary>
        /// TAB   ex2
        /// </summary>
        public string TabExTitle2 { get; set; }
        /// <summary>
        /// TAB   ex3
        /// </summary>
        public string TabExTitle3 { get; set; }
        /// <summary>
        /// TAB   ex4
        /// </summary>
        public string TabExTitle4 { get; set; }
        /// <summary>
        /// TAB   ex5
        /// </summary>
        public string TabExTitle5 { get; set; }

        /// <summary>
        /// Tab Ex Content 1
        /// </summary>
        public string TabExContent1 { get; set; }
        /// <summary>
        /// Tab Ex Content 2
        /// </summary>
        public string TabExContent2 { get; set; }
        /// <summary>
        /// Tab Ex Content 3
        /// </summary>
        public string TabExContent3 { get; set; }
        /// <summary>
        /// Tab Ex Content 4
        /// </summary>
        public string TabExContent4 { get; set; }
        /// <summary>
        /// Tab Ex Content 5
        /// </summary>
        public string TabExContent5 { get; set; }

        /// <summary>
        /// 是否预购商品
        /// 1 是预购商品
        /// 0 普通商品
        /// </summary>
        public int IsAppointment { get; set; }
        /// <summary>
        /// 预购开始时间
        /// </summary>
        public string AppointmentStartTime { get; set; }
        /// <summary>
        /// 预购结束时间
        /// </summary>
        public string AppointmentEndTime { get; set; }
        /// <summary>
        /// 预购发货时间
        /// </summary>
        public string AppointmentDeliveryTime { get; set; }
        /// <summary>
        /// 重量 单位Kg 四位小数
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 供应商账号
        /// </summary>
        public string SupplierUserId { get; set; }

        ///// <summary>
        ///// 系统开团的订单
        ///// </summary>
        //public string SystemGroupOrderId { get; set; }

        ///// <summary>
        ///// 开团类型  0用户开团 1系统开团
        ///// </summary>
        //public int GroupBuyType { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 省份代码
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 城市代码
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 地区代码
        /// </summary>
        public string DistrictCode { get; set; }

        /// <summary>
        ///   楼盘(Houses): 建筑面积
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 开盘时间
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 楼盘类型(现房,期房)
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 附近学校
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 交房时间
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 装修情况
        /// </summary>
        public string Ex6 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 开发商
        /// </summary>
        public string Ex7 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 产权年限
        /// </summary>
        public string Ex8 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 物业公司
        /// </summary>
        public string Ex9 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 建筑类型
        /// </summary>
        public string Ex10 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 车位比例
        /// </summary>
        public string Ex11 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 规划户数
        /// </summary>
        public string Ex12 { get; set; }
        /// <summary>
        ///   楼盘(Houses): 绿化率
        /// </summary>
        public string Ex13 { get; set; }
        /// <summary>
        ///   楼盘(Houses):  会员佣金
        /// </summary>
        public string Ex14 { get; set; }
        /// <summary>
        ///   楼盘(Houses):  会员优惠
        /// </summary>
        public string Ex15 { get; set; }
        /// <summary>
        ///   楼盘(Houses):  非会员优惠
        /// </summary>
        public string Ex16 { get; set; }
        /// <summary>
        ///  楼盘(Houses):   楼盘位置
        /// </summary>
        public string Ex17 { get; set; }
        /// <summary>
        ///   楼盘(Houses):  房型
        /// </summary>
        public string Ex18 { get; set; }

        /// <summary>
        /// 楼盘 (Houses):  楼盘分类 新房NewHouse、二手房SecondHandHouse
        /// </summary>
        public string Ex19 { get; set; }
        public string Ex20 { get; set; }

        #endregion


        #region 扩展 model
        //public int IP
        //{
        //    get
        //    {
        //        int result = 0;
        //        try
        //        {
        //            string strWhere=string.Format("/App/Cation/wap/mall/Showv1.aspx?action=show&pid={0}",PID);

        //            result = new BLL("").GetCount<WebAccessLogsInfo>(" IP ", string.Format("PageUrl like '%{0}'", strWhere));

        //        }
        //        catch { }
        //        return result;
        //    }
        //}

        //public int PV
        //{
        //    get
        //    {
        //        int result = 0;
        //        try
        //        {
        //            string strWhere = string.Format("/App/Cation/wap/mall/Showv1.aspx?action=show&pid={0}", PID);
        //            result = new BLL("").GetCount<WebAccessLogsInfo>(string.Format("PageUrl like '%{0}'", strWhere));

        //        }
        //        catch { }
        //        return result;

        //    }
        //} 

        #endregion

    }
}

