using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.Member
{
    /// <summary>
    /// MemberGroupManage 的摘要说明
    /// </summary>
    public class MemberGroupManage : IHttpHandler, IRequiresSessionState
    {

        BLLJIMP.BLL bll;
        UserInfo userInfo;
        AshxResponse resp = new AshxResponse();

        public void ProcessRequest(HttpContext context)
        {
            try
            {

                userInfo = Comm.DataLoadTool.GetCurrUserModel();
                bll = new BLLJIMP.BLL("");
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


                }
                context.Response.Write(result);
            }
            catch (Exception ex)
            {

                context.Response.Write(ex.Message);
                return;
            }

        }

        /// <summary>
        /// 获取用户分组
        /// </summary>
        private string GetMemberGroupList()
        {

            var UserID = Comm.DataLoadTool.GetCurrUserID();
            string result = " <select id=\"ddlMemberGroup\" style=\"width:180px;\">";
            result += " <option value=\"\">无</option>";
            var GroupList = bll.GetList<MemberGroupInfo>(string.Format("UserID='{0}' and GroupType=1", UserID));
            foreach (MemberGroupInfo item in GroupList)
            {
                result += string.Format("<option value=\"{0}\">{1}</option>", item.GroupID, item.GroupName);
            }
            result += "</select>";
            return result.ToString();

        }

        /// <summary>
        /// 添加
        /// </summary>
        private string Add(HttpContext context)
        {
            var model = new MemberGroupInfo();
            model.UserID = Comm.DataLoadTool.GetCurrUserID();
            model.GroupName = context.Request["GroupName"];
            model.AddDate = DateTime.Now;
            model.GroupType = 1;
            model.GroupID = bll.GetGUID(BLLJIMP.TransacType.MemberGroupAdd);
            return bll.Add(model).ToString().ToLower();


        }


        /// <summary>
        /// 修改
        /// </summary>
        public string Edit(HttpContext context)
        {
            var groupid = context.Request["GroupID"];
            var userid = Comm.DataLoadTool.GetCurrUserID();
            var groupinfo = bll.Get<MemberGroupInfo>(string.Format("GroupID='{0}'", groupid));
            if (groupinfo == null)
            {
                return "分组编号不存在";
            }
            if (!groupinfo.UserID.Equals(userid))
            {
                return "无权修改";
            }
            var model = new MemberGroupInfo();
            model.UserID = userid;
            model.GroupID = groupid;
            model.GroupName = context.Request["GroupName"];
            model.GroupType = 1;
            model.AddDate = DateTime.Now;
            return bll.Update(model).ToString().ToLower();
        }


        /// <summary>
        /// 删除
        /// </summary>
        private string Delete(HttpContext context)
        {
            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            try
            {
                string ids = context.Request["id"];
                string isDeleteData = context.Request["isDeleteData"];
                int deleteGroupCount = 0, dataCount = 0;
                if (string.IsNullOrWhiteSpace(ids))
                {
                    resp.Status = 0;
                    resp.Msg = "传入删除对象不能为空!";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                //ids转成数据库兼容的格式
                ids = Common.StringHelper.ListToStr<string>(ids.Split(',').ToList(), "'", ",");

                //删除分组
                deleteGroupCount = this.bll.Delete(
                        new MemberGroupInfo(),
                        string.Format(" GroupID IN ({0}) AND UserID = '{1}' AND GroupType = 1 ", ids, this.userInfo.UserID),
                        tran
                    );

                //更新数据或者删除数据
                if (isDeleteData == "1")
                    dataCount = this.bll.Delete(new MemberInfo(), string.Format(" GroupID IN ({0}) AND UserID = '{1}'", ids, this.userInfo.UserID), tran);
                else
                    dataCount = this.bll.Update(new MemberInfo(), " GroupID = '0' ", string.Format(" GroupID IN ({0}) AND UserID = '{1}'", ids, this.userInfo.UserID), tran);

                tran.Commit();

                resp.Status = 1;
                resp.Msg = "删除成功!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            catch (Exception ex)
            {
                tran.Rollback();

                resp.Status = 0;
                resp.Msg = ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            #region 旧方法隐藏 2013.8.28
            //var userid = Comm.DataLoadTool.GetCurrUserID();
            //string ids = context.Request["id"];
            //var count = 0;
            //foreach (var item in ids.Split(','))
            //{
            //    var memberinfo = bll.Get<MemberGroupInfo>(string.Format("GroupID='{0}'", item));
            //    if (memberinfo == null)
            //    {
            //        return "分组编号不存在";
            //        break;
            //    }
            //    if (!memberinfo.UserID.Equals(userid))
            //    {
            //        return "无权修改";
            //        break;
            //    }
            //    count += bll.Delete(new MemberGroupInfo(), string.Format("GroupID ='{0}'", item));
            //}
            //bll.Update(new MemberInfo(), string.Format("GroupID=0"), string.Format("GroupID in({0}) ", ids));

            //if (count.Equals(ids.Split(',').Length))
            //{
            //    return "true";
            //}
            //else
            //{
            //    return "false";
            //}

            #endregion


        }





        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private string GetAllByAny(HttpContext context)
        {

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            var searchCondition = string.Format("UserID='{0}' And GroupType=1", Comm.DataLoadTool.GetCurrUserID());
            List<MemberGroupInfo> list = bll.GetLit<MemberGroupInfo>(rows, page, searchCondition, "GroupName DESC");
            MemberGroupInfo DefaultGroup = new MemberGroupInfo();
            DefaultGroup.GroupID = "0";
            DefaultGroup.GroupName = "无分组";
            list.Add(DefaultGroup);

            int totalCount = bll.GetCount<MemberGroupInfo>(searchCondition);
            string jsonResult = ZentCloud.Common.JSONHelper.ObjectToJson(totalCount, list);
            return jsonResult;

            //var searchCondition = string.Format("UserID='{0}' And GroupType=1", Comm.DataLoadTool.GetCurrUserID());
            //List<MemberGroupInfo> list = bll.GetList<MemberGroupInfo>(searchCondition);
            //MemberGroupInfo DefaultGroup = new MemberGroupInfo();
            //DefaultGroup.GroupID = "-2";
            //DefaultGroup.GroupName="无分组";
            //list.Add(DefaultGroup);
            //int totalCount = bll.GetCount<MemberGroupInfo>(searchCondition);
            //string jsonResult = ZentCloud.Common.JSONHelper.ObjectToJson(totalCount, list);
            //return jsonResult;


        }

        /// <summary>
        /// 获取传入的实体
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private MemberInfo GetModel(HttpContext context)
        {
            return ZentCloud.Common.JSONHelper.JsonToModel<BLLJIMP.Model.MemberInfo>(context.Request["JsonData"]);
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