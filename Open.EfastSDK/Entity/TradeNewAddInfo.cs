using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public class TradeNewAddInfo
    {
        //'sd_id'=>19, //对应 efast 店铺 id
        //'province_name'=>'北京', //省份
        //'city_name'=>'北京市', //城市
        //'district_name'=>'西城区', //地区
        //'shipping_name'=>'yto', //快递公司代码
        //'pay_name'=>'alipay', //支付方式代码
        //'oid'=>'oid123457', //交易号
        //'consignee'=>'zzm', //收货人
        //'address'=>'xxxxxxx', //收货地址
        //'zipcode'=>'123456', //邮编
        public int sd_id { get; set; }
        public string province_name { get; set; }
        public string city_name { get; set; }
        public string district_name { get; set; }
        public string shipping_name { get; set; }
        public string pay_name { get; set; }
        public string oid { get; set; }
        public string consignee { get; set; }
        public string address { get; set; }
        public string zipcode { get; set; }

        //'mobile'=>'136767', //手机
        //'tel'=>'02889', //电话
        //'user_name'=>'jiming', //买家账号
        //'email'=>'boy76@sina.com', //email
        //'postscript'=>'postscript', //买家留言
        //'to_buyer'=>'to_buyer', //商家备注
        public string mobile { get; set; }
        public string tel { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string postscript { get; set; }
        public string to_buyer { get; set; }

        //'add_time'=>'2012-07-06 10:16:41', //创建时间
        //'pay_time'=>'2012-07-06 12:16:41', //支付时间
        //'goods_count'=>2, //商品总数量
        //'goods_amount'=>120, //商品金额
        //'total_amount'=>125, //总金额
        //'shipping_fee'=>5, //快递费
        //'order_amount'=>125, //应付款
        //'money_paid'=>125, //已付款
        public string add_time { get; set; }
        public string pay_time { get; set; }
        public string goods_count { get; set; }
        public string goods_amount { get; set; }
        public string total_amount { get; set; }
        public string shipping_fee { get; set; }
        public string order_amount { get; set; }
        public string money_paid { get; set; }

        //‘shipping_time’=>’2012-07-16 10:16:41,// 计划发货时间
        //‘shipping_days’=>10,// 承诺发货天数
        //‘is_yushou’=>0,// 是否预售
        public string shipping_time { get; set; }
        public string is_yushou { get; set; }
        public string shipping_days { get; set; }

        //'inv_status'=>1, //是否需要发票可不传
        //'inv_payee'=>'个人', //发票抬头，依赖于 inv_status
        //'inv_content'=>'电器', //发票内容，依赖于 inv_status
        //'weigh'=>250, //订单重量（克）
        public int inv_status { get; set; }
        public string inv_payee { get; set; }
        public string inv_content { get; set; }
        public double weigh { get; set; }
        
        public List<Entity.TradeNewAddOrdersInfo> orders { get; set; }

        //'orders'=>array(
        ////商品明细 outer_sku：匹配条码 goods_name：商品名称 goods_number：数量 goods_price：价格 payment_ft: 商
        //品分摊价 is_gift：是否礼品
        //0=>array('outer_sku'=>'zzm00105000','goods_name'=>'zzm','goods_number'=>1,'goods_price'=>60,
        //'payment_ft'=>60,’is_gift’=>1),
        //1=>array('outer_sku'=>'6953913956831','goods_name'=>'zzm','goods_number'=>2,'goods_price'=>60,'payment_ft'=>
        //120,’is_gift’=>0),
        //),
        //)
    }
}
