using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CrowdFund.Mobile
{
    /// <summary>
    /// MobileHandler 的摘要说明
    /// </summary>
    public class MobileHandler : IHttpHandler, IRequiresSessionState
    {

        BLLJIMP.BLL bllBase = new BLLJIMP.BLL();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        protected AshxResponse resp = new AshxResponse(); // 统一回复相应数据
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
                    resp.Status = 0;
                    resp.Msg = "Action为空！";
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
        /// 查询付款人员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryPayPersion(HttpContext context)
        {
            int pageIndex=int.Parse(context.Request["PageIndex"]);
            int pageSize=int.Parse(context.Request["PageSize"]);
            int crowdFundId=int.Parse(context.Request["CrowdFundID"]);
            List<PayPersionModel> result = new List<PayPersionModel>();
            System.Text.StringBuilder sbWhere=new System.Text.StringBuilder();
            sbWhere.AppendFormat(" CrowdFundID={0} And Status=1",crowdFundId);
            List<CrowdFundRecord> sourceList = bllBase.GetLit<CrowdFundRecord>(pageSize, pageIndex, sbWhere.ToString(), " RecordID DESC");
            foreach (var item in sourceList)
            {
                UserInfo userInfo=bllUser.GetUserInfo(item.UserID);
                PayPersionModel model = new PayPersionModel();
                model.HeadIimg = userInfo.WXHeadimgurlLocal;
                if (string.IsNullOrEmpty(model.HeadIimg))
                {
                    model.HeadIimg = "/img/persion.png";
                }
                model.ShowName =item.Name;
                model.Amount = item.Amount;
                model.InsertDate = item.InsertDate.ToString("MM-dd");
                model.Review = item.Review;
                result.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(result);


        }

        /// <summary>
        /// 微信分享完成触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string WXShareComlete(HttpContext context) {
            int id = int.Parse(context.Request["id"]);
            var model = bllBase.Get<CrowdFundInfo>(string.Format(" AutoID={0}",id));
            model.ShareCount++;
            bllBase.Update(model);
          return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加付款记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCrowdFundRecord(HttpContext context)
        {
            CrowdFundRecord model = bllBase.ConvertRequestToModel<CrowdFundRecord>(new CrowdFundRecord());
            model.UserID = bllBase.GetCurrUserID();
            model.RecordID = int.Parse(bllBase.GetGUID(ZentCloud.BLLJIMP.TransacType.CommAdd));
            model.InsertDate = DateTime.Now;
            if (model.Amount<=0)
            {
                resp.Msg = "金额需大于0";
                goto outoff;
            }
            if (bllBase.Add(model))
            {
                resp.Status = 1;
                resp.ExInt = model.RecordID;

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "付款失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 参与人员模型
        /// </summary>
        public class PayPersionModel {
            /// <summary>
            /// 头像
            /// </summary>
            public string HeadIimg { get; set; }
            /// <summary>
            /// 显示的姓名
            /// </summary>
            public string ShowName { get; set; }
            /// <summary>
            /// 付款金额
            /// </summary>
            public decimal Amount { get; set; }
            /// <summary>
            /// 日期
            /// </summary>
            public string InsertDate { get; set; }
            /// <summary>
            /// 一句话评论
            /// </summary>
            public string Review { get; set; }
        
        
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