using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 积分商品 
    /// </summary>
    [Serializable]
    public partial class WXMallScoreProductInfo : ZCBLLEngine.ModelTable
    {
        public WXMallScoreProductInfo()
        { }
        #region Model
        private int _autoid;
        private string _pname;
        private string _pdescription;
        private int _score;
        private string _userid;
        private DateTime _insertdate = DateTime.Now;
        /// <summary>
        /// 积分商品ID
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
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
        /// 所需积分
        /// </summary>
        public int Score
        {
            set { _score = value; }
            get { return _score; }
        }
        /// <summary>
        /// 打折积分
        /// </summary>
        public int DiscountScore { get; set; }
        /// <summary>
        /// 用户ID
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
        /// 推荐图片
        /// </summary>
        public string RecommendImg { get; set; }

        /// <summary>
        /// 是否上架 1 上架 0 下架
        /// </summary>
        public string IsOnSale { get; set; }
        /// <summary>
        /// 是否删除标识 1 已删除 0未删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 网站所有者
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

        public string ScoreLine { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public int TypeId { get; set; }
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
        /// <summary>
        /// 排序号 数值越大越靠前
        /// </summary>
        public int Sort { get; set; }

        #endregion Model




    }
}

