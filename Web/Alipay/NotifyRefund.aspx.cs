using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Payment.Alipay;
using System.Xml;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using System.Text;
using ZentCloud.Common;


namespace ZentCloud.JubitIMP.Web.Alipay
{
    /// <summary>
    /// 支付宝退款通知
    /// </summary>
    public partial class NotifyRefund : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //通知参数示例
                //sign=de7ab71a24075f3d96d2619fac60f16c
                //result_details=2017020721001004830269248522^0.01^SUCCESS
                //notify_time=2017-02-10 14:43:38
                //sign_type=MD5
                //notify_type=batch_refund_notify
                //notify_id=f87d0a63daa030e732173f3f568d6e4i0a
                //batch_no=201702101486709017
                //success_num=1
                Dictionary<string, string> parametersAll = bllMall.GetRequestParameter();
                StringBuilder sb = new StringBuilder();
                foreach (var item in parametersAll)
                {
                    sb.AppendFormat(string.Format("{0}={1}&", item.Key, item.Value));

                }
                Tolog("退款通知参数:" + sb.ToString());
                if (parametersAll.Count > 0)//判断是否有带返回参数
                {
                    Notify aliNotify = new Notify();
                    PayConfig payConfig = bllPay.GetPayConfig();
                    bool verifyResult = aliNotify.Verify(parametersAll, Request["sign"], payConfig.Partner, payConfig.PartnerKey);
                    if (verifyResult)//验证成功
                    {
                        Tolog("退款验签成功");
                        string batchNo = Request["batch_no"];//退款批次号
                        string successNum = Request["success_num"];//成功数
                        int updateCount = 0;//更新条数
                        WXMallRefund refundInfo = bllMall.Get<WXMallRefund>(string.Format("OutRefundId='{0}'", batchNo));
                        if (successNum == "1")
                        {
                            updateCount = bllMall.Update(refundInfo, string.Format("Status=6"), string.Format("RefundId='{0}'", refundInfo.RefundId));

                        }
                        else
                        {
                            updateCount = bllMall.Update(refundInfo, string.Format("Status=8"), string.Format("RefundId='{0}'", refundInfo.RefundId));
                        }
                        if (updateCount > 0)
                        {
                            Response.Write("success");
                        }
                        else
                        {
                            Response.Write("fail");
                        }

                    }
                    else//验证失败
                    {
                        Tolog("退款验签失败");
                        Response.Write("fail");
                    }
                }
                else
                {
                    Response.Write("无通知参数");
                }
            }
            catch (Exception ex)
            {
                Tolog("退款异常:" + ex.ToString());
                Response.Write("fail");
            }







        }


        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        private void Tolog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\alipaylog.txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString()+msg);
                }
            }
            catch { }
        }
    }
}