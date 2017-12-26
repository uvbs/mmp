using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen
{
    /// <summary>
    /// 父处理文件
    /// </summary>
    public class BaseHanderOpen : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 返回响应
        /// </summary>
        protected BaseResponse resp = new BaseResponse();
        /// <summary>
        /// BLL基类
        /// </summary>
        BLLJIMP.BLL bllBase = new BLLJIMP.BLL();
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            try
            {
                bllBase.ToLog("进入 openbase 接口:" + context.Request.Url, "D:\\jikudevlog.txt");

                #region 授权验证
                var websiteInfo = bllBase.GetWebsiteInfoModelFromDataBase();
                if (!string.IsNullOrEmpty(websiteInfo.WhiteIP))
                {
                    var whiteIpList = websiteInfo.WhiteIP.Split(',');
                    if ((!whiteIpList.Contains(context.Request.UserHostAddress)) || (string.IsNullOrEmpty(context.Request.UserHostAddress)))
                    {
                        resp.code = (int)APIErrCode.InadequatePermissions;
                        resp.msg = "拒绝访问";

                        bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }


                string appId = context.Request["appid"];
                string timeStamp = context.Request["timestamp"];
                string sign = context.Request["sign"];
                if (string.IsNullOrEmpty(appId))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "appid 参数必传";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (bllBase.WebsiteOwner != appId)
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "appid 错误";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (string.IsNullOrEmpty(timeStamp))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "timestamp 参数必传";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                DateTime requestTime = bllBase.GetTime(long.Parse(timeStamp));
                if ((DateTime.Now - requestTime).TotalMinutes >= 3)
                {

                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "时间戳已过期";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if ((DateTime.Now - requestTime).TotalMinutes <= -3)
                {

                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "时间戳不能晚于当前日期";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (string.IsNullOrEmpty(sign))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "sign 参数必传";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                string appKey = string.Empty;
                if (websiteInfo != null && !string.IsNullOrWhiteSpace(websiteInfo.ComeoncloudOpenAppKey))
                {
                    appKey = websiteInfo.ComeoncloudOpenAppKey;
                }
                else
                {
                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "appkey 未分配";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                if (sign != CreateSign(appId, timeStamp, appKey))
                {
                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "签名错误";

                    bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                #endregion

                this.GetType().GetMethod("ProcessRequest").Invoke(this, new[] { context });


            }
            catch (Exception ex)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = ex.ToString();
                bllBase.ToLog("进入 openbase 接口异常 ： " + ZentCloud.Common.JSONHelper.ObjectToJson(resp), "D:\\jikudevlog.txt");
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }
        }
        
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="appkey">AppKey</param>
        /// <returns></returns>
        public string CreateSign(string appId, string timeStamp, string appkey)
        {
            string sign = ZentCloud.Common.SHA1.SHA1_Encrypt(string.Format("appid={0}&appkey={1}&timestamp={2}", appId, appkey, timeStamp)).ToUpper();

            return sign;

        }
        
        /// <summary>
        /// 卡券类型转换
        /// </summary>
        /// <param name="cardCouponType"></param>
        /// <returns></returns>
        public int ConvertCardCouponType(string cardCouponType)
        {

            switch (cardCouponType)
            {

                case "MallCardCoupon_Discount"://>折扣券：凭折扣券对指定商品（全场）打折
                    return 0;
                case "MallCardCoupon_Deductible"://抵扣券：支付时可以抵扣现金
                    return 1;
                case "MallCardCoupon_FreeFreight"://免邮券：满一定金额包邮
                    return 2;
                case "MallCardCoupon_Buckle"://满扣券：消费满一定金额减去一定金额
                    return 3;
                case "MallCardCoupon_BuckleGive"://满送券
                    return 4;
                default:
                    break;
            }
            return 0;


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}