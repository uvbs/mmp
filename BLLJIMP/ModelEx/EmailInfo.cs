using System;
using System.Collections.Generic;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 邮件表
    /// </summary>
    public partial class EmailInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 邮件状态说明
        /// </summary>
        public string EmailSendStatusDescription
        {
            get
            {

                return Common.DictionaryClump.EmailSendStatusDictionary[_emailsendstatus];
            }
        }

       

        /// <summary>
        /// 投递成功率：投递成功量/总量
        /// </summary>
        public string DeliverySuccessPercentage
        {
            get
            {

                if (OSendTotalCount.Equals(0))
                    return "0.00%";

                return ((decimal)ODeliverySuccessCount / (decimal)OSendTotalCount).ToString("P");
            }
        }

        

        /// <summary>
        /// 投递失败率：投递失败量/总量
        /// </summary>
        public string DeliveryFailurePercentage
        {
            get
            {
                if (OSendTotalCount.Equals(0))
                    return "0.00%";
                return ((decimal)ODeliveryFailureCount / (decimal)OSendTotalCount).ToString("P");
            }
        }

        

        /// <summary>
        /// 阅读人数率：阅读人数/投递成功量
        /// </summary>
        public string DistOpensPercentage
        {
            get
            {
                if (ODeliverySuccessCount.Equals(0))
                    return "0.00%";
                return ((decimal)ODistOpensCount / (decimal)ODeliverySuccessCount).ToString("P");
            }
        }

        

        /// <summary>
        /// 阅读人次率：阅读人次/投递成功量
        /// </summary>
        public string OOpensCountPercentage
        {
            get
            {
                if (ODeliverySuccessCount.Equals(0))
                    return "0.00%";
                return ((decimal)OOpensCount / (decimal)ODeliverySuccessCount).ToString("P");
            }
        }

       

        /// <summary>
        /// 链接点击人数率:链接点击人数/投递成功量
        /// </summary>
        public string DistUrlClicksPercentage
        {
            get
            {
                if (ODeliverySuccessCount.Equals(0))
                    return "0.00%";
                return ((decimal)ODistOUrlClicksCount / (decimal)ODeliverySuccessCount).ToString("P");
            }
        }

       

        /// <summary>
        /// 链接点击人次率: 链接点击人次/投递成功量
        /// </summary>
        public string OUrlClicksCountPercentage
        {
            get
            {
                if (ODeliverySuccessCount.Equals(0))
                    return "0.00%";
                return ((decimal)OUrlClicksCount / (decimal)ODeliverySuccessCount).ToString("P");
            }
        }

        /// <summary>
        /// 邮件内容URL
        /// </summary>
        public string EmailBodyUrl
        {
            get
            {
                return "/EDM/EmailBody.aspx?EmailID=" + _emailid;
            }
        }

        /// <summary>
        /// 邮件具体统计报告URL
        /// </summary>
        public string EmailStatisticsUrl
        {
            get
            {
                return "/EDM/Report/EmailStatistics.aspx?EmailID=" + _emailid;
            }
        }

        /// <summary>
        /// 邮件发送明细列表URL(所有)
        /// </summary>
        public string EmailSentAllDetailsUrl
        {
            get
            {
                return "/EDM/Report/EmailSendDetailsV2.aspx?EmailID=" + _emailid + "&SendStatus=";
            }
        }

        /// <summary>
        /// 邮件发送明细列表URL(成功发送)
        /// </summary>
        public string EmailSentSucessDetailsUrl
        {
            get
            {
                return "/EDM/Report/EmailSendDetailsV2.aspx?EmailID=" + _emailid + "&SendStatus=4";
            }
        }

        /// <summary>
        /// 邮件发送明细列表URL(发送失败)
        /// </summary>
        public string EmailSentFailureDetailsUrl
        {
            get
            {
                return "/EDM/Report/EmailSendDetailsV2.aspx?EmailID=" + _emailid + "&SendStatus=-1";
            }
        }

        /// <summary>
        /// 邮件打开明细列表URL
        /// </summary>
        public string EmailEventDetailsOpenUrl
        {
            get
            {
                return "/EDM/Report/EmailEventDetailsV2.aspx?EmailID=" + _emailid + "&EventType=0";
            }
        }

        /// <summary>
        /// 邮件点击明细列表URL
        /// </summary>
        public string EmailEventDetailsClickUrl
        {
            get
            {
                return "/EDM/Report/EmailEventDetailsV2.aspx?EmailID=" + _emailid + "&EventType=1";
            }
        }

    }
}

