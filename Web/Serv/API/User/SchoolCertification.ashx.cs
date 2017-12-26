using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// SchoolCertification 的摘要说明    学籍认证  
    /// </summary>
    public class SchoolCertification : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLUserExpand bllUserExpand = new BLLJIMP.BLLUserExpand();
        public void ProcessRequest(HttpContext context)
        {

            string name = context.Request["name"];
            string idCard = context.Request["id_card"];
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(idCard))
            {
                apiResp.msg = "姓名和身份证为必填信息";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            UserExpand userExpand=bllUserExpand.GetUserExpand(BLLJIMP.Enums.UserExpandType.StudentStatus,idCard);
            if (userExpand != null)
            {
                RespBody body = new RespBody();
                body.STUDYSTYLE = userExpand.Ex1;
                body.EDUCATIONDEGREE = userExpand.Ex2;
                body.GRADUATETIME = userExpand.Ex3;
                body.STUDYRESULT = userExpand.Ex4;
                body.GRADUATE = userExpand.Ex5;
                body.SPECIALITYNAME = userExpand.Ex6;
                body.ENROLDATE = userExpand.Ex7;
                bllUserExpand.ContextResponse(context, body);
                return;
            }
            Request model = new Request();
            model.header = new Header();
            model.body = new Body();
            model.header.version = "1.0";
            model.header.product = "B1001093";
            model.header.merchant = "201512179102";
            model.header.outOrderId = Math.Ceiling(bllUser.GetTimeStamp(DateTime.Now)).ToString();
            model.body.realName = name;
            model.body.cardId = idCard;
            var sign1 = new
            {
                body = model.body,
                header = model.header
            };

            string singStr = ZentCloud.Common.JSONHelper.ObjectToJson(sign1);

            string key = "sFpCmChE9VLpVcSrefDh";

            string sign = Payment.Alipay.AlipayMD5.Sign(singStr, key, "utf-8").ToLower();
            var postData = new
            {
                request = model,
                sign = sign
            };

            string strPostdata = ZentCloud.Common.JSONHelper.ObjectToJson(postData);

            string sResult = string.Empty;

            string sError = string.Empty;

            string sResponseStatusCode = string.Empty;

            string sResponseStatusDescription = string.Empty;

            HttpWebResponse oHttpWebResponse = null;
            HttpWebRequest oHttpWebRequest = null;
            Stream oStream = null;
            StreamReader oStreamReader = null;
            byte[] bytes = Encoding.UTF8.GetBytes(strPostdata);
            try
            {
                oHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.udcredit.com/api/credit/v1/education_checkinfo");
                oHttpWebRequest.Method = "POST";
                oHttpWebRequest.ContentType = "application/json";
                oHttpWebRequest.ContentLength = bytes.Length;

                oHttpWebRequest.Timeout = 1000 * 50;

                oStream = oHttpWebRequest.GetRequestStream();
                oStream.Write(bytes, 0, bytes.Length);
                oStream.Close();

                oHttpWebResponse = (HttpWebResponse)oHttpWebRequest.GetResponse();
                oStreamReader = new StreamReader(oHttpWebResponse.GetResponseStream());
                sResponseStatusCode = oHttpWebResponse.StatusCode.ToString();
                sResponseStatusDescription = oHttpWebResponse.StatusDescription;

                sResult = oStreamReader.ReadToEnd();
                ResponseModel resultModel = ZentCloud.Common.JSONHelper.JsonToModel<ResponseModel>(sResult);
                if (resultModel.response.header.retCode == "0000")
                {
                    bllUserExpand.AddUserExpand(BLLJIMP.Enums.UserExpandType.StudentStatus,idCard,name, resultModel.response.body.STUDYSTYLE,resultModel.response.body.EDUCATIONDEGREE,
                        resultModel.response.body.GRADUATETIME,resultModel.response.body.STUDYRESULT,resultModel.response.body.GRADUATE,resultModel.response.body.SPECIALITYNAME,
                        resultModel.response.body.ENROLDATE
                        );
                    bllUser.ContextResponse(context, resultModel.response.body);
                }
                else
                {
                    apiResp.status = false;
                    apiResp.msg = resultModel.response.header.retMsg;
                    bllUser.ContextResponse(context, apiResp);

                }
            }
            catch (Exception ex)
            {
                apiResp.msg = ex.ToString();
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);

            }
            finally
            {
                oStream = null;
            }
        }



        #region request
        public class Header
        {    /// <summary>
            /// 商户号码
            /// </summary>
            public string merchant { get; set; }
            /// <summary>
            /// 外部订单号
            /// </summary>
            public string outOrderId { get; set; }
            /// <summary>
            /// 业务类型
            /// </summary>
            public string product { get; set; }
            /// <summary>
            /// 版本号
            /// </summary>
            public string version { get; set; }


        }
        public class Body
        {
            /// <summary>
            /// 身份证号码
            /// </summary>
            public string realName { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string cardId { get; set; }

        }

        public class Request
        {
            public Body body { get; set; }
            public Header header { get; set; }
        }
        #endregion

        #region response

        public class RespHeader 
        {
            public string version { get; set; }

            public string product { get; set; }

            public string reqTime { get; set; }

            public string resTime { get; set; }

            public string retCode { get; set; }

            public string retMsg { get; set; }
        }

        public class RespBody
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string STUDYSTYLE { get; set; }

            /// <summary>
            /// 学位
            /// </summary>
            public string EDUCATIONDEGREE { get; set; }

            /// <summary>
            /// 毕业时间
            /// </summary>

            public string GRADUATETIME { get; set; }

            /// <summary>
            /// 毕业
            /// </summary>
            public string STUDYRESULT { get; set; }

            /// <summary>
            /// 学校
            /// </summary>
            public string GRADUATE { get; set; }

            /// <summary>
            /// 专业名称
            /// </summary>
            public string SPECIALITYNAME { get; set; }

            /// <summary>
            /// 入学时间
            /// </summary>
            public string ENROLDATE { get; set; }
        }

        public class Response 
        {
            public RespBody body { get; set; }

            public RespHeader header { get; set; }
        }

        public class ResponseModel 
        {
            public Response response { get; set; }

            public string sign { get; set; }
        }
        #endregion
    }
}