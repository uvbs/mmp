using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.WeiXin
{
    /// <summary>
    /// WeiXinFlowInfoManage 的摘要说明
    /// </summary>
    public class WeiXinFlowInfoManage : IHttpHandler, IRequiresSessionState
    {

        static BLLJIMP.BLLWeixin bll;
        /// <summary>
        /// 增删改权限
        /// </summary>
        private static bool _isedit;
        /// <summary>
        /// 查看权限
        /// </summary>
        private static bool _isview;

        public void ProcessRequest(HttpContext context)
        {

          //  BLLMenuPermission perbll = new BLLMenuPermission("");
            //_isedit = perbll.CheckUserAndPms(Comm.DataLoadTool.GetCurrUserID(), 255);
            //_isview = perbll.CheckUserAndPms(Comm.DataLoadTool.GetCurrUserID(), 250);
            bll = new BLLWeixin("");
            
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string Action = context.Request["Action"];
            string result = "false";
            switch (Action)
            {
                case "Add":
                    result = Add(context);
                    break;
                case "Edit":
                    result = Edit(context);
                    break;
                case "Delete":
                    result = Delete(context);
                    break;
                case "Query":
                    result = GetAllByAny(context);
                    break;
                case "BatChangState":
                    result = BatChangState(context);
                    break;

            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        private static string Add(HttpContext context)
        {
            //if (!_isedit)
            //{
            //    return null;
            //}


            var userid = Comm.DataLoadTool.GetCurrUserID();
            if (string.IsNullOrEmpty(userid))
            {
                return "请重新登录";
            }
            var flowname = context.Request["FlowName"];
            var flowkeyword = context.Request["FlowKeyword"];
            if (!bll.CheckUserKeyword(userid,flowkeyword))
            {
                return "关键字重复";
            }
            WXFlowInfo model = new WXFlowInfo();
            model.FlowID = int.Parse(bll.GetGUID(TransacType.WeixinFlowAdd));
            model.UserID = userid;
            model.FlowName = flowname;
            model.FlowKeyword = flowkeyword;
            model.FlowEndMsg = context.Request["FlowEndMsg"];
            model.MemberLimitState =int.Parse(context.Request["MemberLimitState"]) ;
            model.FlowLimitMsg = context.Request["FlowLimitMsg"];
            model.FlowSysType = 1;
            model.IsEnable = int.Parse(context.Request["IsEnable"]);
            return bll.Add(model).ToString().ToLower();



        }

        /// <summary>
        /// 修改
        /// </summary>
        public static string Edit(HttpContext context)
        {
            //if (!_isedit)
            //{
            //    return null;
            //}
            var userid = Comm.DataLoadTool.GetCurrUserID();
            if (string.IsNullOrEmpty(userid))
            {
                return "请重新登录";
            }
            var flowid=int.Parse(context.Request["FlowID"]);
            var flowname = context.Request["FlowName"];
            var flowkeyword = context.Request["FlowKeyword"];
            var oldflowinfo = bll.Get<WXFlowInfo>(string.Format("FlowID={0}", flowid));
            if (oldflowinfo.FlowKeyword!=flowkeyword)//对比关键字是否改变
	        {
              //关键字改变,检查关键字是否重复
            if (!bll.CheckUserKeyword(userid,flowkeyword))
            {
                return "关键字重复";
            }
		    
	        }
            WXFlowInfo model = new WXFlowInfo();
            model.FlowID = flowid;
            model.UserID = userid;
            model.FlowName = flowname;
            model.FlowKeyword = flowkeyword;
            model.FlowEndMsg = context.Request["FlowEndMsg"];
            model.MemberLimitState = int.Parse(context.Request["MemberLimitState"]);
            model.FlowLimitMsg = context.Request["FlowLimitMsg"];
            model.IsEnable = int.Parse(context.Request["IsEnable"]);
            if (bll.Update(model, string.Format("FlowName='{0}',FlowKeyword='{1}',FlowEndMsg='{2}',MemberLimitState='{3}',FlowLimitMsg='{4}',IsEnable='{5}'", model.FlowName, model.FlowKeyword, model.FlowEndMsg, model.MemberLimitState, model.FlowLimitMsg,model.IsEnable), "FlowID=" + model.FlowID) > 0)
            {
                return "true";
            }
            return "false";
        }

        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {
        //    if (!_isedit)
        //    {
        //        return null;
        //    }

            string ids = context.Request["id"];
            try
            {
        
                
                //删除流
                bll.Delete(new WXFlowInfo(), string.Format("FlowID in({0}) ", ids));
                //删除
                bll.Delete(new WXFlowStepInfo(), string.Format("FlowID  in({0}) ", ids));

                bll.Delete(new WXFlowDataInfo(), string.Format("FlowID  in({0}) ", ids));

                

                return "true";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }


        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private static string GetAllByAny(HttpContext context)
        {
            //if (!_isview)
            //{
            //    return null;
            //}
            var userid = Comm.DataLoadTool.GetCurrUserID();
            if (string.IsNullOrEmpty(userid))
            {
                return "请重新登录";
            }

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string searchtitle = context.Request["SearchTitle"];
            var searchCondition = string.Format("UserID='{0}'", userid);
            if (!string.IsNullOrEmpty(searchtitle))
            {
                searchCondition += "And FlowName like '%" + searchtitle + "%'";
            }

            List<WXFlowInfo> list = bll.GetLit<WXFlowInfo>(rows, page, searchCondition, "FlowID");

            int totalCount = bll.GetCount<WXFlowInfo>(searchCondition);

            string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalCount, list);

            return jsonResult;
        }


        /// <summary>
        /// 批量启用禁用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string BatChangState(HttpContext context)
        {


            string ids = context.Request["id"];
            var state = context.Request["IsEnable"];
            if (bll.Update(new WXFlowInfo(), string.Format("IsEnable='{0}'", state), string.Format("FlowID in ({0})", ids)) > 0)
            {
                return "true";
            }
            ;
            return "false";


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