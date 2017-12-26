using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Reflection;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 捐款接口 无用
    /// </summary>
    public class DonationsApi : IHttpHandler,IRequiresSessionState
    {
        protected AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {

                string Action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(Action))
                {
                    MethodInfo method = this.GetType().GetMethod(Action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "action is null";
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
        /// 获取中奖记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getdonationrecord(HttpContext context)
        {
            DonationsModel result = new DonationsModel();
            List<Person> list = new List<Person>();
            int PeopleCount=0;
            decimal TotalAmount=0;
            if (string.IsNullOrEmpty(context.Request["pageindex"]))
            {
                //resp.Msg = "pageindex不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(context.Request["pagesize"]))
            {
                //resp.Msg = "pagesize不能为空";
                goto outoff;
            }
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            var source = bll.GetLit<WXMallOrderInfo>(pageSize, pageIndex, string.Format("WebsiteOwner='qianwei' And OrderUserID='system' And PaymentStatus=1")," InsertDate DESC");

            PeopleCount= bll.GetCount<WXMallOrderInfo>(string.Format("WebsiteOwner='qianwei' And OrderUserID='system' And PaymentStatus=1"));
            TotalAmount= bll.GetList<WXMallOrderInfo>(string.Format("WebsiteOwner='qianwei' And OrderUserID='system' And PaymentStatus=1")).Sum(p => p.TotalAmount);


            foreach (var item in source)
            {
                Person model = new Person();
                model.showname = item.Consignee;
                model.money=item.TotalAmount;
                list.Add(model);
            }
        outoff:
            result.list = list;
            result.totalcount = PeopleCount;
            result.totalmoney = TotalAmount;
            return Common.JSONHelper.ObjectToJson(result);

        }

        /// <summary>
        /// 捐款模型
        /// </summary>
        public class DonationsModel
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 总捐款金额
            /// </summary>
            public decimal totalmoney { get; set; }
            /// <summary>
            /// 中奖名单
            /// </summary>
            public List<Person> list { get; set; }
        }
        /// <summary>
        /// 捐款人物模型
        /// </summary>
        public class Person
        {
            /// <summary>
            /// 显示名称
            /// </summary>
            public string showname { get; set; }
            /// <summary>
            /// 金额
            /// </summary>
            public decimal money { get; set; }

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