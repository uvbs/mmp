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
    /// WeiXinFlowStepInfoManage 的摘要说明
    /// </summary>
    public class WeiXinFlowStepInfoManage : IHttpHandler, IRequiresSessionState
    {
        static BLLJIMP.BLL bll;
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
            bll = new BLL("");

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
                case "MoveStep":
                    result = MoveStep(context);
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
            var flowid=int.Parse(context.Request["FlowID"]);
            WXFlowStepInfo model = new WXFlowStepInfo();
            model.FlowID =flowid;
            var stepinfo = bll.Get<WXFlowStepInfo>(string.Format("FlowID='{0}' order by StepID DESC",flowid));//取最后一条记录
            if (stepinfo!=null)
            {
                model.StepID = stepinfo.StepID + 1;
            }
            else
            {
                model.StepID = 1;
            }
          
            model.FlowField = context.Request["FlowField"];
            model.FieldDescription = context.Request["FieldDescription"];
            model.SendMsg = context.Request["SendMsg"];
            model.ErrorMsg = context.Request["ErrorMsg"];
            model.AuthFunc = context.Request["AuthFunc"];
            if (model.AuthFunc.Equals("phone"))
            {
                model.IsVerifyCode = int.Parse(context.Request["IsVerifyCode"]);
                
            }
            return bll.Add(model).ToString().ToLower();



        }

        /// <summary>
        /// 修改
        /// </summary>
        public static string Edit(HttpContext context)
        {
            var userid = Comm.DataLoadTool.GetCurrUserID();
            if (string.IsNullOrEmpty(userid))
            {
                return "请重新登录";
            }
            WXFlowStepInfo model = new WXFlowStepInfo();
            model.FlowID =int.Parse(context.Request["FlowID"]) ;
            model.StepID =int.Parse(context.Request["StepID"]) ;
            model.FlowField = context.Request["FlowField"];
            model.FieldDescription = context.Request["FieldDescription"];
            model.SendMsg = context.Request["SendMsg"];
            model.ErrorMsg = context.Request["ErrorMsg"];
            model.AuthFunc = context.Request["AuthFunc"];
            if (model.AuthFunc.Equals("phone"))
            {
                model.IsVerifyCode = int.Parse(context.Request["IsVerifyCode"]);

            }

            return bll.Update(model).ToString().ToLower();
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

            string stepids = context.Request["StepID"];
            string flowid = context.Request["FlowID"];
            try
            {


                //删除
                if ( bll.Delete(new WXFlowStepInfo(), string.Format(" FlowID={0} And StepID in({1}) ", flowid, stepids))>0)
                {
                    //重排序号
                    var steplist = bll.GetList<WXFlowStepInfo>(string.Format("FlowID={0} Order by StepID ASC", flowid));
                    int index =0;
                    foreach (var item in steplist)
                    {
                        //item.StepID=index;
                        index++;
                       // bll.Update(item,);
                        bll.Update(item, string.Format("StepID='{0}'", index), string.Format("FlowID={0} And StepID={1}", flowid, item.StepID));
                    }
                    //重排序号
                    return "true";
                }

                return "false";
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
            string flowid = context.Request["FlowID"];
            if (bll.Get<WXFlowInfo>(string.Format("FlowID={0} and UserID='{1}'",flowid,userid))==null)
            {
                return "无权查看";
                
            }
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string searchtitle = context.Request["SearchTitle"];
           
            var searchCondition = string.Format("FlowID={0}", flowid);
            if (!string.IsNullOrEmpty(searchtitle))
            {
                searchCondition += "And FlowField like '%" + searchtitle + "%'";
            }

            List<WXFlowStepInfo> list = bll.GetLit<WXFlowStepInfo>(rows, page, searchCondition, "StepID ASC");

            int totalCount = bll.GetCount<WXFlowStepInfo>(searchCondition);

            string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalCount, list);

            return jsonResult;
        }

        /// <summary>
        /// 移动步骤
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string MoveStep(HttpContext context)
        {
            var userid = Comm.DataLoadTool.GetCurrUserID();
            if (string.IsNullOrEmpty(userid))
            {
                return "请重新登录";
            }
            int flowid = int.Parse(context.Request["FlowID"]);//流程ID
            int stepid =int.Parse(context.Request["StepID"]) ;//步骤ID
            string direction=context.Request["Direction"];//移动方向 up:上 down: 下
            if (bll.Get<WXFlowInfo>(string.Format("FlowID={0} and UserID='{1}'", flowid, userid)) == null)
            {
                return "无权查看";

            }
            var searchCondition = string.Format("FlowID={0}", flowid);
            int tagetindex=0;//要移动的步骤的索引;
            List<WXFlowStepInfo> steplist=bll.GetList<WXFlowStepInfo>(searchCondition);//所有步骤列表
            for (int i = 0; i < steplist.Count; i++)
			{
			     if (steplist[i].StepID.Equals(stepid))
	            {
		            tagetindex=i;//获取要移动步骤的索引
                    break;
	            }

			}
            //var tagetstepid = steplist[tagetindex].StepID;
            if (direction.Equals("up"))//上移
	        {
                if (tagetindex==0)
	            {
                    return "已经是第一个步骤了";
	            }
                if (tagetindex>0)
	            {
		            //目标数据的前一条数据
                   var prestep=steplist[tagetindex-1];
                    //更新前一条数据step为别的值
                   bll.Update(prestep, string.Format("StepID={0}", steplist[steplist.Count - 1].StepID + 1), string.Format("FlowID={0} And StepID={1}", flowid, prestep.StepID));
                 
                  //更新目标步骤step 上移
                   bll.Update(prestep, string.Format("StepID={0}", steplist[tagetindex].StepID - 1), string.Format("FlowID={0} And StepID={1}", flowid, steplist[tagetindex].StepID));

                  //
                   //更新前一条数据step数据
                   bll.Update(prestep, string.Format("StepID={0}", steplist[tagetindex].StepID), string.Format("FlowID={0} And StepID={1}", flowid, steplist[steplist.Count - 1].StepID + 1));

                  // return "true";



	            }
		        
	        }
            else if (direction.Equals("down"))//下移
	        {
                if (steplist[tagetindex].StepID ==steplist[steplist.Count-1].StepID)
                {
                    return "已经是最后一个步骤了";
                }

                    //目标数据的后一条数据
                    var nextstep = steplist[tagetindex +1];
                    //更新后一条数据step为别的值
                    bll.Update(nextstep, string.Format("StepID={0}", steplist[steplist.Count - 1].StepID + 1), string.Format("FlowID={0} And StepID={1}", flowid, nextstep.StepID));

                    //更新目标步骤step 下移
                    bll.Update(nextstep, string.Format("StepID={0}", steplist[tagetindex].StepID + 1), string.Format("FlowID={0} And StepID={1}", flowid, steplist[tagetindex].StepID));

                    //
                    //更新后一条step数据
                    bll.Update(nextstep, string.Format("StepID={0}", steplist[tagetindex].StepID), string.Format("FlowID={0} And StepID={1}", flowid, steplist[steplist.Count - 1].StepID + 1));

                    // return "true";



             


	        }
            //重排序号
            var newsteplist = bll.GetList<WXFlowStepInfo>(string.Format("FlowID={0} Order by StepID ASC", flowid));
            int index = 0;
            foreach (var item in newsteplist)
            {
                //item.StepID=index;
                index++;
                // bll.Update(item,);
                bll.Update(item, string.Format("StepID={0}", index), string.Format("FlowID={0} And StepID={1}", flowid, item.StepID));
            }
            //重排序号
            return "true";

          
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