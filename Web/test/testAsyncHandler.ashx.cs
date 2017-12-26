using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.test
{
    /// <summary>
    /// testAsyncHandler 的摘要说明
    /// </summary>
    public class testAsyncHandler : IHttpAsyncHandler,IRequiresSessionState
    {
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 活动业务逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo; 
        /// <summary>
        /// 网站所有者
        /// </summary>
        private string webSiteOwner;
        /// <summary>
        /// 基路径 形式如 http://dev.comeoncloud.net
        /// </summary>
        private string basePath;

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
        {
            string Action = context.Request["action"];
            string result;
            //利用反射找到未知的调用的方法
            if (!string.IsNullOrEmpty(Action))
            {
                MethodInfo method = this.GetType().GetMethod(Action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法

                ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
            }
            else
            {
                resp.errmsg = "action not exist";
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            System.Runtime.Remoting.Messaging.AsyncResult ss = new System.Runtime.Remoting.Messaging.AsyncResult();
            ss .rresult;
            AsynchOperation asynch = new AsynchOperation(cb, context, extraData);
            asynch.StartAsyncWork();
            return asynch;
        }
        public void BaseAction(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bll.IsLogin)
                {
                    currentUserInfo = bll.GetCurrentUserInfo();
                }
                webSiteOwner = bll.WebsiteOwner;
                //WebSiteOwner = "forbes";
                basePath = string.Format("http://{0}{1}", context.Request.Url.Host,context.Request.Url.Port != 80? ":" + context.Request.Url.Port.ToString() :"");
                string Action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(Action))
                {
                    MethodInfo method = this.GetType().GetMethod(Action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errmsg = "action not exist";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }

            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }

        }

        private string testAction1(HttpContext context) 
        {
            return "1111111";
        }
        private string testAction2(HttpContext context) 
        {
            return "22222";
        }

        public void EndProcessRequest(IAsyncResult result)
        {
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class AsynchOperation : IAsyncResult
    {
        private bool _completed;
        private Object _state;
        private AsyncCallback _callback;
        private HttpContext _context;

        bool IAsyncResult.IsCompleted { get { return _completed; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        Object IAsyncResult.AsyncState { get { return _state; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } }

        public AsynchOperation(AsyncCallback callback, HttpContext context, Object state)
        {
            _callback = callback;
            _context = context;
            _state = state;
            _completed = false;
        }

        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
        }

        private void StartAsyncTask(Object workItemState)
        {
            testAsyncHandler testhandler = new testAsyncHandler();
            testhandler.BaseAction(_context);
            _completed = true;
            _callback(this);
        }
    }
}