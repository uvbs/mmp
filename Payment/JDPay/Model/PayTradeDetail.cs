using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Payment.JDPay.Model
{
    [XmlRootAttribute("detail", Namespace = "", IsNullable = false)]
    public class PayTradeDetail
    {
        /**
     * 持卡人人姓名  掩码显示（隐去第一位）
     */
        public String cardHolderName { set; get; }
        /**
         * 持卡人手机号  掩码显示（手机号的前三位与后四位）
         */
        public String cardHolderMobile { set; get; }
        /**
         * 证件类型   ID("0", "身份证"), PASSPORT("1", "护照"), OFFICER("2", "军官证"), SOLDIER("3", "士兵证"), TWHK_PASSPORT("4", "港奥台通行证"), TEMP_ID("5", "临时身份证"), HOUSEHOLDREGISTER("6", "户口本"), OTHER("7", "其它类型证件")
         */
        public String cardHolderType { set; get; }
        /**
         * 身份证号
         */
        public String cardHolderId;
        /**
         * 卡号  掩码显示（前六位及后四位）
         */
        public String cardNo { set; get; }
        /**
         * 银行编码
         */
        public String bankCode;
        /**
         * 银行卡类型   DEBIT_CARD：借记卡CREDIT_CARD：信用卡SEMI_CREDIT_CARD：准贷记卡
         */
        public String cardType { set; get; }
        /**
         * 支付金额
         */
        public long payMoney { set; get; }

    }
}