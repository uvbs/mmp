using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.kuaidi100
{
    /// <summary>
    /// CallBack 的摘要说明
    /// </summary>
    public class CallBack : IHttpHandler, IReadOnlySessionState
    {

        BLLJIMP.BllKuaidi100 bll = new BLLJIMP.BllKuaidi100();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string param = context.Request["param"];
            string sign = context.Request["sign"];
            bool isSuccess = false;
            string respResult = "";//响应结果
            try
            {
                var requestResult = ZentCloud.Common.JSONHelper.JsonToModel<CallBackModel>(param);
                ExpressResult result = bll.GetExpressResult(requestResult.lastResult.nu,requestResult.lastResult.com);
                if (result == null)//还没有快递单号,添加
                {
                    ExpressResult model = new ExpressResult();
                    model.ExpressNumber = requestResult.lastResult.nu;
                    model.ExpressCompanyCode = requestResult.lastResult.com;
                    model.ExpressContent = ZentCloud.Common.JSONHelper.ObjectToJson(requestResult.lastResult.data);
                    model.InsertDate = DateTime.Now;
                    model.LastUpdateDate = DateTime.Now;
                    model.WebsiteOwner = bll.WebsiteOwner;
                    model.Status = requestResult.status;
                    model.Message = requestResult.message;
                    model.ResultJson = param;
                    if (bll.Add(model))
                    {
                        isSuccess = true;
                    }

                }
                else
                {
                    //已经有快递单号,更新
                    result.ExpressContent = ZentCloud.Common.JSONHelper.ObjectToJson(requestResult.lastResult.data);
                    result.ResultJson = param;
                    result.Status = requestResult.status;
                    result.Message = requestResult.message;
                    result.LastUpdateDate = DateTime.Now;
                    if (bll.Update(result))
                    {
                        isSuccess = true;
                    }


                }
                
                if (isSuccess)
                {
                    respResult = ZentCloud.Common.JSONHelper.ObjectToJson(new
                    {
                        result = "true",
                        returnCode = "200",
                        message = "成功"
                    });
                }
                else
                {
                    respResult = ZentCloud.Common.JSONHelper.ObjectToJson(new
                    {
                        result = "false",
                        returnCode = "500",
                        message = "失败"
                    });
                }
            }
            catch (Exception ex)
            {
                respResult = ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    result = "false",
                    returnCode = "500",
                    message = "失败"
                });

               
            }
            context.Response.Write(respResult);

        }
        /// <summary>
        /// 回调模型
        /// </summary>
        public class CallBackModel
        {
            /// <summary>
            /// 状态
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 消息
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 结果集合
            /// </summary>
            public LastResultModel lastResult { get; set; }

        }
        /// <summary>
        /// 查询结果模型
        /// </summary>
        public class LastResultModel
        {
            public string message { get; set; }
            public string state { get; set; }
            public string ischeck { get; set; }
            /// <summary>
            /// 快递公司代码
            /// </summary>
            public string com { get; set; }
            /// <summary>
            /// 快递单号
            /// </summary>
            public string nu { get; set; }
            /// <summary>
            /// 查询数据
            /// </summary>
            public List<DataModel> data { get; set; }

        }
        /// <summary>
        /// 快递查询结果模型
        /// </summary>
        public class DataModel
        {
            /// <summary>
            /// 内容
            /// </summary>
            public string context { get; set; }
            /// <summary>
            /// 时间
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// 格式化后的时间
            /// </summary>
            public string ftime { get; set; }


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