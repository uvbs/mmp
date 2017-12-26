using Payment.Alipay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 实名验证
    /// </summary>
    public class NameCertification : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLCommRelation bLLCommRelation = new BLLJIMP.BLLCommRelation();
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
            CommRelationInfo commRelation= bLLCommRelation.GetRelationInfo(BLLJIMP.Enums.CommRelationType.NameCertification,name,idCard);
            if (commRelation != null)
            {
                apiResp.status = true;
                apiResp.msg = "认证通过";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            Request model = new Request();
            model.header = new Header();
            model.body = new Body();
            model.header.version = "1.0";
            model.header.product = "B10002";
            model.header.merchant = "201512179102";
            model.header.outOrderId = Math.Ceiling(bllUser.GetTimeStamp(DateTime.Now)).ToString();
            model.body.name_card = name;
            model.body.id_card = idCard;
            var sign1 = new
            {
                body=model.body,
                header=model.header
            };

            string singStr = ZentCloud.Common.JSONHelper.ObjectToJson(sign1);

            string key = "sFpCmChE9VLpVcSrefDh";

            string sign=Payment.Alipay.AlipayMD5.Sign(singStr, key, "utf-8").ToLower();
           
            var postData = new
            {
                request=model,
                sign=sign
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
                oHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.udcredit.com/api/credit/v1/get_nauth");
                oHttpWebRequest.Method = "POST";
                oHttpWebRequest.ContentType = "application/json";
                oHttpWebRequest.ContentLength = bytes.Length;

                oHttpWebRequest.Timeout = 1000 * 5;

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
                    if (resultModel.response.body.status == "1")
                    {
                        bLLCommRelation.AddCommRelation(BLLJIMP.Enums.CommRelationType.NameCertification,name,idCard);
                        apiResp.status = true;
                        apiResp.msg = "认证通过";
                    }
                    else if (resultModel.response.body.status == "2")
                    {
                        apiResp.status = false;
                        apiResp.msg = "认证不通过";
                    }
                    else
                    {
                        apiResp.status = false;
                        apiResp.msg = "查不到";
                    }
                }
                else
                {
                    apiResp.status = false;
                    apiResp.msg = resultModel.response.header.retMsg;
                }
                bllUser.ContextResponse(context, apiResp);
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

        #region request 请求所需
        public class Header 
        {    /// <summary>
            /// 商户号码
            /// </summary>
            public string merchant{get;set;} 
            /// <summary>
            /// 外部订单号
            /// </summary>
            public string outOrderId{get;set;} 
            /// <summary>
            /// 业务类型
            /// </summary>
            public string product{get;set;}
            /// <summary>
            /// 版本号
            /// </summary>
            public string version{get;set;}


        }
        public class Body 
        {
            /// <summary>
            /// 身份证号码
            /// </summary>
            public string id_card { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string name_card{get;set;}

        }

        public class Request 
        {
            public Body body { get; set; }
            public Header header { get; set; }
        }
        #endregion


        #region response 返回所需
        public class RespHeader 
        {
            /// <summary>
            /// 业务类型
            /// </summary>
            public string product { get; set; }

            /// <summary>
            /// 返回编码
            /// </summary>
            public string retCode { get; set; }

            /// <summary>
            /// 完成时间
            /// </summary>
            public string resTime { get; set; }

            /// <summary>
            /// 返回内容
            /// </summary>
            public string retMsg { get; set; }

            /// <summary>
            /// 发起时间
            /// </summary>
            public string reqTime { get; set; }

            /// <summary>
            /// 版本号
            /// </summary>
            public string version { get; set; }
        }
        public class RespBody 
        {
            /// <summary>
            /// 认证结果
            /// 返回结果：1-认证一致，2-认证不一致，3-无结果（在公安数据库中查询不到此条数据）
            /// </summary>
            public string status { get; set; }

            /// <summary>
            /// 内部订单号
            /// </summary>
            public string order_no { get; set; }
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