using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;
using ZentCloud.BLLJIMP.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
namespace ZentCloud.BLLJIMP
{


    /// <summary>
    /// 短信BLL
    /// </summary>
    public class BLLSMS : BLL
    {
        public BLLSMS(string userID)
            : base(userID)
        {

        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="verificationCode"></param>
        /// <param name="isSuccess"></param>
        /// <param name="msg"></param>
        public void SendSmsVerificationCode(string phone, string msgContent, string smsSignature, string verificationCode, out bool isSuccess, out string msg)
        {
            isSuccess = false;
            msg = "";
            if (string.IsNullOrEmpty(smsSignature))
            {
                smsSignature = "至云";
            }
            string sendContent = string.Format("{0}【{1}】", msgContent, smsSignature);
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();
            SmsVerificationCode model = new SmsVerificationCode();
            model.Phone = phone;
            model.InsertDate = DateTime.Now;
            model.VerificationCode = verificationCode;
            model.WebsiteOwner = WebsiteOwner;
            if (Add(model, tran))
            {

                if (SendSms(WebsiteOwner, phone, sendContent))
                {

                    isSuccess = true;
                    tran.Commit();
                }
                else
                {
                    msg = "短信发送失败";
                    tran.Rollback();
                }

            }
            else
            {
                tran.Rollback();
            }

        }

        /// <summary>
        /// 发送定时短信
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="msgContent">短信内容</param>
        /// <param name="sendTime">发送时间</param>
        /// <param name="smsSignature">短信签名</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="msg"></param>
        public void SendSmsMisson(string phone, string msgContent, string sendTime, string smsSignature, out bool isSuccess, out string msg)
        {
            isSuccess = false;
            msg = "";
            if (string.IsNullOrEmpty(smsSignature))
            {
                smsSignature = "至云";
            }
            string sendContent = string.Format("{0}【{1}】", msgContent, smsSignature);
            UserInfo websiteOwnerUserInfo = GetCurrWebSiteUserInfo();
            Common.HttpInterFace webRequest = new Common.HttpInterFace();
            string parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membermission&attime={4}", websiteOwnerUserInfo.UserID, websiteOwnerUserInfo.Password, phone, sendContent, sendTime);
            string returnCode = webRequest.PostWebRequest(parm, "http://sms.comeoncloud.net/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));
            if (!string.IsNullOrEmpty(returnCode) && (returnCode.ToString().Equals("0")))
            {

                isSuccess = true;

            }
            else
            {
                msg = returnCode;

            }

        }

        /// <summary>
        /// 获取最后发送的手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public SmsVerificationCode GetLastSmsVerificationCode(string phone)
        {
            return Get<SmsVerificationCode>(string.Format("Phone='{0}' And WebSiteOwner='{1}' Order by AutoID DESC", phone, WebsiteOwner));
        }


        ///// <summary>
        ///// 发送短信 大汉三通
        ///// </summary>
        ///// <param name="mobile">手机号，多个手机号用,分隔</param>
        ///// <param name="message">短信内容</param>
        ///// <param name="sign">短信签名示例【大汉三通】</param>
        ///// <returns></returns>
        //public bool SendSmsdh3t(string mobile, string message, string sign)
        //{
        //    string userId = System.Configuration.ConfigurationManager.AppSettings["DahanSantongUserId"];
        //    string password = System.Configuration.ConfigurationManager.AppSettings["DahanSantongPassword"];
        //    var postData = new
        //    {
        //        account = userId,
        //        password = ZentCloud.Common.DEncrypt.GetMD5(password),
        //        phones = mobile,
        //        content = message,
        //        sign = sign
        //    };
        //    ZentCloud.Common.HttpInterFace request = new Common.HttpInterFace();
        //    string result = request.PostWebRequest(ZentCloud.Common.JSONHelper.ObjectToJson(postData), "http://www.dh3t.com/json/sms/Submit", Encoding.UTF8);
        //    JToken jToken = JToken.Parse(result);
        //    if (jToken["result"].ToString() == "0")
        //    {
        //        return true;
        //    }
        //    return false;


        //}
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="mobile">手机号</param>
        /// <param name="message">短信内容</param>
        /// <returns></returns>
        public bool SendSms(string websiteOwner, string mobile, string message)
        {

            //BLLJIMP.BLLUser bllUser = new BLLUser();

            //UserInfo websiteOwnerUserInfo = bllUser.GetUserInfo(websiteOwner, websiteOwner);

            //Common.HttpInterFace webRequest1 = new Common.HttpInterFace();
            //string parm = string.Format("userName={0}&userPwd={1}&mobile={2}&content={3}&pipeID=membertrigger", websiteOwnerUserInfo.UserID, websiteOwnerUserInfo.Password, mobile, message);
            //string returnCode = webRequest1.PostWebRequest(parm, "http://sms.comeoncloud.net/Serv/SubmitSMSAPI.aspx", System.Text.Encoding.GetEncoding("gb2312"));
            //if (!string.IsNullOrEmpty(returnCode) && (returnCode.ToString().Equals("0")))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}


            //}

            string timeStamp = Math.Round(GetTimeStamp(DateTime.Now), 0).ToString();
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userName", websiteOwner);
            dic.Add("mobile", mobile);
            dic.Add("content", message);
            dic.Add("pipeID", "membertrigger");
            dic.Add("timestamp", timeStamp);
            string sign = CreateSign(timeStamp);
            dic.Add("sign", sign);
            ZentCloud.Common.HttpInterFace request = new Common.HttpInterFace();
            string par = LinkPar(dic);
            JToken result = JToken.Parse(request.GetWebRequest(par, string.Format("http://{0}/serv/api/sms/send.ashx", ZentCloud.Common.ConfigHelper.GetConfigString("SmsDomain")), Encoding.UTF8));
            if (bool.Parse(result["status"].ToString()))
            {

                return true;
            }
            else
            {

                return false;
            }


            //string userName = "api";
            //string key = System.Configuration.ConfigurationManager.AppSettings["LUOSIMAOKEY"];
            //string url = "http://sms-api.luosimao.com/v1/send.json";
            //byte[] byteArray = Encoding.UTF8.GetBytes("mobile=" + mobile + "&message=" + message);
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(userName + ":" + key));
            //webRequest.Headers.Add("Authorization", auth);
            //webRequest.Method = "POST";
            //webRequest.ContentType = "application/x-www-form-urlencoded";
            //webRequest.ContentLength = byteArray.Length;

            //Stream newStream = webRequest.GetRequestStream();
            //newStream.Write(byteArray, 0, byteArray.Length);
            //newStream.Close();

            //HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            //StreamReader srReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            //string result = srReader.ReadToEnd();
            //if (result.Contains("\"error\":0"))
            //{
            //    return true;
            //}
            //else
            //{
            //    try
            //    {
            //        using (StreamWriter sw = new StreamWriter(@"D:\SmsLog.txt", true, Encoding.GetEncoding("gb2312")))
            //        {
            //            sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), result + message));
            //        }
            //    }
            //    catch { }
            //}
            //return false;



        }
        /// <summary>
        /// 查询短信任务历史
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<SMSPlanInfo> QuerySMSPlan(out int totalCount, int pageIndex, int pageSize, string websiteOwner)
        {
            List<SMSPlanInfo> result = new List<SMSPlanInfo>();

            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}' And PlanType = {1} ", websiteOwner, (int)Enums.SMSPlanType.WXTemplateMsg_Notify));

            List<SMSPlanInfo> dataList = GetLit<SMSPlanInfo>(pageSize, pageIndex, sbWhere.ToString(), "SubmitDate DESC");

            totalCount = GetCount<SMSPlanInfo>(sbWhere.ToString());

            result = dataList;

            return result;
        }

        /// <summary>
        /// 获取短信余额
        /// </summary>
        /// <param name="websiteowner">站点</param>
        /// <returns></returns>
        public bool GetSmsDeposit(string websiteowner, out int deposit, out string msg)
        {
            deposit = 0;
            msg = "";
            string timeStamp = Math.Round(GetTimeStamp(DateTime.Now), 0).ToString();
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("userName", websiteowner);
            dic.Add("timestamp", timeStamp);
            string sign = CreateSign(timeStamp);
            dic.Add("sign", sign);
            ZentCloud.Common.HttpInterFace request = new Common.HttpInterFace();
            string par = LinkPar(dic);
            JToken result = JToken.Parse(request.GetWebRequest(par, string.Format("http://{0}/serv/api/sms/status.ashx", ZentCloud.Common.ConfigHelper.GetConfigString("SmsDomain")), Encoding.UTF8));
            if (bool.Parse(result["status"].ToString()))
            {
                deposit = int.Parse(result["result"]["deposit"].ToString());
                msg = "获取短信余额成功";
                return true;
            }
            else
            {
                msg = result["msg"].ToString();
                return false;
            }



        }

        /// <summary>
        /// 短信充值
        /// </summary>
        /// <param name="websiteowner">站点</param>
        /// <param name="orderId">订单号</param>
        /// <param name="point">短信条数</param>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public bool SmsRecharge(string websiteowner, string orderId, int point, out string msg)
        {
            msg = "";
            try
            {


                string timeStamp = Math.Round(GetTimeStamp(DateTime.Now), 0).ToString();
                SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
                dic.Add("userName", websiteowner);
                dic.Add("timestamp", timeStamp);
                dic.Add("orderId", orderId);
                dic.Add("point", point.ToString());
                string sign = CreateSign(timeStamp);
                dic.Add("sign", sign);
                ZentCloud.Common.HttpInterFace request = new Common.HttpInterFace();
                string par = LinkPar(dic);
                JToken result = JToken.Parse(request.GetWebRequest(par, string.Format("http://{0}/serv/api/sms/recharge.ashx", ZentCloud.Common.ConfigHelper.GetConfigString("SmsDomain")), Encoding.UTF8));
                if (bool.Parse(result["status"].ToString()))
                {
                    msg = "充值成功";
                    return true;
                }
                else
                {
                    msg = result["msg"].ToString();
                    return false;
                }



            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return false;
            }

        }
        /// <summary>
        /// 把参数转成 key=value&连接的形式
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string LinkPar(SortedDictionary<string, string> dic)
        {
            string str = "";
            foreach (var item in dic)
            {
                str += string.Format("{0}={1}&", item.Key, item.Value);
            }
            str += string.Format("appid={0}", ZentCloud.Common.ConfigHelper.GetConfigString("ComeoncloudAppId"));
            return str;

        }

        /// <summary>
        ///生成签名
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns></returns>
        public string CreateSign(string timeStamp)
        {
            string sign = ZentCloud.Common.SHA1.SHA1_Encrypt(string.Format("appid={0}&appkey={1}&timestamp={2}", ZentCloud.Common.ConfigHelper.GetConfigString("ComeoncloudAppId"), ZentCloud.Common.ConfigHelper.GetConfigString("ComeoncloudAppKey"), timeStamp)).ToUpper();
            return sign;

        }

        /// <summary>
        /// 
        /// </summary>
        public void SendTimingSms()
        {
            var websiteList = GetList<WebsiteInfo>(string.Format(" SmsAccountRemindValue>0 "));
            foreach (var item in websiteList)
            {
                int count = 0;
                string msg = string.Empty;
                bool isSmsAccount = GetSmsDeposit(item.WebsiteOwner, out  count, out msg);
                if (!isSmsAccount || item.SmsAccountRemindValue < count) return;
                if (string.IsNullOrEmpty(item.SmsAccountRemindPhones)) return;

                string[] phones = item.SmsAccountRemindPhones.Split(',');

                for (int i = 0; i < phones.Length; i++)
                {
                    var phone = phones[i];
                    string message = "您的短信余额已不足" + item.SmsAccountRemindValue + ",请尽快充值。【至云科技】";
                    SendSms(item.WebsiteOwner, phone, message);
                }
            }
        }

        //private void ToLog(string log)
        //{
        //    try
        //    {
        //        using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("gb2312")))
        //        {
        //            sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
        //        }
        //    }
        //    catch { }
        //}

    }



    ///// <summary>
    ///// 发送类型枚举
    ///// </summary>
    //public enum SMSType
    //{
    //    /// <summary>
    //    /// 批量立即发送
    //    /// </summary>
    //    BatchImmediately = 0,
    //    /// <summary>
    //    /// 批量定时发送
    //    /// </summary>
    //    BatchTimer,
    //    /// <summary>
    //    /// 批量触发发送
    //    /// </summary>
    //    BatchTrigger,
    //    /// <summary>
    //    /// 单条立即发送
    //    /// </summary>
    //    SingleImmediately,
    //    /// <summary>
    //    /// 单条定时发送
    //    /// </summary>
    //    SingleTimer,
    //    /// <summary>
    //    /// 单条触发发送
    //    /// </summary>
    //    SingleTrigger
    //};

    //public enum SMSTriggerType
    //{
    //    Meeting = 0,
    //    Website,
    //    OutSide
    //}




}
