using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 微信卡券实体
    /// </summary>
    public class WeixinCard
    {
        /// <summary>
        /// 类型
        /// GENERAL_COUPON 优惠券
        /// GIFT 兑换券
        /// DISCOUNT 折扣券
        /// CASH 代金券
        /// GROUPON 团购券
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 卡券的商户logo，建议像素为300*300。
        /// </summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 码型：
        ///CODE_TYPE_TEXT文本；
        ///CODE_TYPE_BARCODE一维码 
        ///CODE_TYPE_QRCODE二维码
        ///CODE_TYPE_ONLY_QRCODE,二维码无code显示；
        ///CODE_TYPE_ONLY_BARCODE,一维码无code显示；
        ///CODE_TYPE_NONE，不显示code和条形码类型
        /// </summary>
        public string CodeType { get; set; }
        /// <summary>
        /// 商户名字,字数上限为12个汉字
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 卡券名，字数上限为9个汉字。
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 券颜色。按色彩规范标注填写Color010-Color100。
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 卡券使用提醒，字数上限为16个汉字。
        /// </summary>
        public string Notice { get; set; }
        /// <summary>
        /// 卡券使用说明，字数上限为1024个汉字。
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 卡券名称
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// DATE_TYPE_FIX_TIME_RANGE 
        ///表示固定日期区间，
        ///DATE_TYPE_FIX_TERM
        ///表示固定时长（自领取后按天算。)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Type为DATE_TYPE_FIX_TIME_RANGE时专用，表示起用时间。
        /// </summary>
        public int BeginTimeStamp { get; set; }
        /// <summary>
        /// Type为DATE_TYPE_FIX_TIME_RANGE时专用，表示起用时间。 失效时间
        /// </summary>
        public int EndTimeStamp { get; set; }
        /// <summary>
        /// Type为DATE_TYPE_FIX_TERM时专用，表示自领取后多少天内有效，不支持填写0。
        /// </summary>
        public int FixedTerm { get; set; }
        /// <summary>
        /// Type为DATE_TYPE_FIX_TERM时专用，表示自领取后多少天开始生效，领取后当天生效填写0。（单位为天）
        /// </summary>
        public int FixedBeginTerm { get; set; }
        /// <summary>
        /// 是否自定义Code码
        /// </summary>
        public bool UseCustomCode { get; set; }
        /// <summary>
        /// 是否指定用户领取，填写true或false
        ///。默认为false。通常指定特殊用户群体
        ///投放卡券或防止刷券时选择指定用户领取。
        /// </summary>
        public bool BindOpenId { get; set; }
        /// <summary>
        /// 客服电话。
        /// </summary>
        public string ServicePhone { get; set; }
        /// <summary>
        /// 第三方来源名，例如同程旅游、大众点评。
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 卡券领取页面是否可分享。
        /// </summary>
        public bool CanShare { get; set; }
        /// <summary>
        /// 卡券是否可转赠。
        /// </summary>
        public bool CanGiveFriend { get; set; }
        /// <summary>
        /// 每人可核销的数量限制,不填写默认为50。
        /// </summary>
        public int UseLimit { get; set; }
        /// <summary>
        /// 每人可领券的数量限制,不填写默认为50。
        /// </summary>
        public int GetLimit { get; set; }
        /// <summary>
        /// 卡券顶部居中的按钮，仅在卡券状态正常(可以核销)时显示
        /// </summary>
        public string CenterTitle { get; set; }
        /// <summary>
        /// 显示在入口下方的提示语，仅在卡券状态正常(可以核销)时显示。
        /// </summary>
        public string CenterSubTitle { get; set; }
        /// <summary>
        /// 顶部居中的url，仅在卡券状态正常(可以核销)时显示。
        /// </summary>
        public string CenterUrl { get; set; }
        /// <summary>
        /// 自定义跳转外链的入口名字
        /// </summary>
        public string CustomUrlName { get; set; }
        /// <summary>
        /// 自定义跳转的URL。
        /// </summary>
        public string CustomUrl { get; set; }
        /// <summary>
        /// 显示在入口右侧的提示语。
        /// </summary>
        public string CustomUrlSubTitle { get; set; }
        /// <summary>
        /// 营销场景的自定义入口名称。
        /// </summary>
        public string PromotionUrlName { get; set; }
        /// <summary>
        /// 入口跳转外链的地址链接。
        /// </summary>
        public string PromotionUrl { get; set; }
        /// <summary>
        /// 显示在营销入口右侧的提示语。
        /// </summary>
        public string PromotionUrlSubTitle { get;set;}
        /// <summary>
        /// 优惠券专用，填写优惠详情
        /// </summary>
        public string DefaultDetail { get; set; }
        /// <summary>
        /// 门店列表
        /// </summary>
        public List<int> LocationIdList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Image { get; set; }


    }
}
