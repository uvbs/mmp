using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXGeneralHandler 的摘要说明
    /// </summary>
    public class WXGeneralHandler : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuActivity=new BLLJuActivity();  //活动数据
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";

            try
            {
                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "请联系管理员";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
        }


        /// <summary>
        /// 根据条形码查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetByBarCode(HttpContext context)
        {
            string barCode = context.Request["BarCode"];
            //BLLJIMP.Model.ConfigBarCodeInfo cbcInfo = juActivityBll.Get<BLLJIMP.Model.ConfigBarCodeInfo>(string.Format(" websiteOwner='{0}'", websiteOwner));
                if (string.IsNullOrEmpty(barCode))
                {
                    resp.Status = -1;
                    resp.Msg = "您好，请填写你查询的编号！！！";
                    goto OurF;
                }
                BLLJIMP.Model.BarCodeInfo barCodeInfo = bllJuActivity.Get<BLLJIMP.Model.BarCodeInfo>(string.Format(" BarCode='{0}' ORDER BY AutoId DESC", barCode));
                if (barCodeInfo != null)
                {
                    resp.Status = 0;
                    resp.ExObj = barCodeInfo;
                    if (string.IsNullOrEmpty(barCodeInfo.TimeOne))
                    {
                        barCodeInfo.TimeOne = DateTime.Now.ToString("yyyy-MM-dd");
                        resp.Msg = "您输入的号码，已于" + barCodeInfo.TimeOne + "被查询过，至今被查询1次。";
                        resp.Msg += "还可以查询2次。该产品与" + barCodeInfo.InsetDatastr + "发货至我们蝴蝶" + barCodeInfo.Agency + "经销商处。";
                        resp.Msg += "该号码所对应的产品为" + barCodeInfo.ModelCode + "。如有疑问可与购买的经销商联系。谢谢";

                    }
                    else if (string.IsNullOrEmpty(barCodeInfo.TimeTwo))
                    {
                        barCodeInfo.TimeTwo = DateTime.Now.ToString("yyyy-MM-dd");
                        resp.Msg = "您输入的号码，已于" + barCodeInfo.TimeTwo + "被查询过，至今被查询2次。";
                        resp.Msg += "还可以查询1次。该产品与" + barCodeInfo.InsetDatastr + "发货至我们蝴蝶" + barCodeInfo.Agency + "经销商处。";
                        resp.Msg += "该号码所对应的产品为" + barCodeInfo.ModelCode + "。如有疑问可与购买的经销商联系。谢谢";

                    }
                    else if (string.IsNullOrEmpty(barCodeInfo.TimeThree))
                    {
                        barCodeInfo.TimeThree = DateTime.Now.ToString("yyyy-MM-dd");
                        resp.Msg = "您输入的号码，已于" + barCodeInfo.TimeThree + "被查询过，至今被查询3次。";
                        resp.Msg += "还可以查询0次。该产品与" + barCodeInfo.InsetDatastr + "发货至我们蝴蝶" + barCodeInfo.Agency + "经销商处。";
                        resp.Msg += "该号码所对应的产品为" + barCodeInfo.ModelCode + "。如有疑问可与购买的经销商联系。谢谢";
                    }
                    else
                    {
                        resp.ExObj = "";
                        resp.Msg = "您输入的号码，已于" + barCodeInfo.TimeThree + "被被查查询过，至今被查询3次。不可再查询";
                        //resp.Msg += "还可以查询0次。该产品与" + BCInfo.InsetDatastr + "发货至我们蝴蝶北京利生经销商处";
                        //resp.Msg += "该号码所对应的产品为" + BCInfo.ModelCode + "。如有疑问可与购买的经销商联系。谢谢";
                    }
                    bool isSuccess = bllJuActivity.Update(barCodeInfo);
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "没有查询到数据";
                }
            
        OurF:
            return Common.JSONHelper.ObjectToJson(resp);

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