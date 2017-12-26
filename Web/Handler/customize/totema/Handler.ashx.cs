using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.customize.totema
{
    /// <summary>
    /// TOTEMA
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 投票逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo CurrentUserInfo = new UserInfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                if (bllVote.IsLogin)
                {
                    CurrentUserInfo = bllVote.GetCurrentUserInfo();
                }
                else
                {
                        resp.errcode = 1;
                        resp.errmsg = "尚未登录";
                        result = Common.JSONHelper.ObjectToJson(resp);
                        goto outoff;
                }
                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errmsg = "action不存在";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            outoff:
            context.Response.Write(result);
        }

        /// <summary>
        /// 我爱我班报名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteObjectInfo(HttpContext context)
        {

            string voteObjectName = context.Request["VoteObjectName"];
            string voteObjectHeadImage = context.Request["VoteObjectHeadImage"];
            string area = context.Request["Area"];
            string introduction = context.Request["Introduction"];
            string number = (bllVote.GetVoteObjectMaxNumber(bllVote.TotemaVoteID) + 1).ToString();
            string schoolName = context.Request["SchoolName"];
            string address = context.Request["Address"];//Totema
            string contact = context.Request["Contact"];
            string phone = context.Request["Phone"];
            string comeonShareId = context.Request["ComeonShareId"];
            if (bllVote.GetVoteObjectInfo(bllVote.TotemaVoteID, CurrentUserInfo.UserID) != null)
            {
                resp.errcode = 1;
                resp.errmsg = "你已经报过名了";
                goto outoff;
            }
            if (string.IsNullOrEmpty(introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入参赛口号";
                goto outoff;
            }
            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入班级名称";
                goto outoff;
            }
            if (string.IsNullOrEmpty(schoolName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入学校全称";
                goto outoff;
            }
            if (string.IsNullOrEmpty(area))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入所在区域";
                goto outoff;
            }
            if (string.IsNullOrEmpty(address))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入学校地址";
                goto outoff;
            }
            if (string.IsNullOrEmpty(contact))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人姓名";
                goto outoff;
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人电话";
                goto outoff;
            } 
            if (bllVote.IsExitsVoteObjectNumber(bllVote.TotemaVoteID, number))
            {
                resp.errcode =1;
                resp.errmsg = "班级编号已经存在";
                goto outoff;
            }

            if (!string.IsNullOrWhiteSpace(voteObjectHeadImage))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle = Convert.ToInt32(context.Request["imgAngle"]);
                //imgAngle = 90;
                if (imgAngle != 0)
                {
                    //获取源图片地址
                    string imgPath = context.Server.MapPath(voteObjectHeadImage);

                    CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();

                    var imgResult = imgHelper.RotateImg(imgPath, imgAngle, imgPath);  

                }
            }
            


            VoteObjectInfo model = new VoteObjectInfo();
            model.VoteID = bllVote.TotemaVoteID;
            model.Number = number;
            model.VoteObjectName = voteObjectName;
            model.VoteObjectHeadImage = voteObjectHeadImage;
            model.Area = area;
            model.Introduction = introduction;
            model.SchoolName = schoolName;
            model.Address = address;
            model.Contact = contact;
            model.Phone = phone;
            model.Status =1;
            model.CreateUserId = CurrentUserInfo.UserID;
            model.Rank =0;
            model.ComeonShareId = comeonShareId;
            if (bllVote.AddVoteObjectInfo(model))
            {
                resp.errmsg = "报名成功";
                bllVote.UpdateVoteObjectRank(bllVote.TotemaVoteID);
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "报名失败";
            }
            outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 编辑我们班的资料
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditVoteObjectInfo(HttpContext context)
        {
            string voteObjectName = context.Request["VoteObjectName"];//班级名称
            string area = context.Request["Area"];//地区
            string introduction = context.Request["Introduction"];//口号
            string schoolName = context.Request["SchoolName"];//学校名称
            string address = context.Request["Address"];//Totema
            string contact = context.Request["Contact"];//联系人
            string phone = context.Request["Phone"];//联系人电话
            string voteObjectHeadImage = context.Request["VoteObjectHeadImage"];
            if (string.IsNullOrEmpty(introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入参赛口号";
                goto outoff;
            }
            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入班级名称";
                goto outoff;
            }
            if (string.IsNullOrEmpty(schoolName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入学校全称";
                goto outoff;
            }
            if (string.IsNullOrEmpty(area))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入所在区域";
                goto outoff;
            }
            if (string.IsNullOrEmpty(address))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入学校地址";
                goto outoff;
            }
            if (string.IsNullOrEmpty(contact))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人姓名";
                goto outoff;
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人电话";
                goto outoff;
            } 
            VoteObjectInfo model = bllVote.GetVoteObjectInfo(bllVote.TotemaVoteID, CurrentUserInfo.UserID);
            if (model==null)
            {
                resp.errcode = 1;
                resp.errmsg = "您还没有报名";
                goto outoff;
            }
            if (!string.IsNullOrWhiteSpace(voteObjectHeadImage))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle = Convert.ToInt32(context.Request["imgAngle"]);
                //imgAngle = 90;
                if (imgAngle != 0)
                {
                    //获取源图片地址
                    string imgPath = context.Server.MapPath(voteObjectHeadImage);

                    CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();

                    var imgResult = imgHelper.RotateImg(imgPath, imgAngle, imgPath);

                }
            }
            model.VoteObjectName = voteObjectName;
            model.Area = area;
            model.Introduction = introduction;
            model.SchoolName = schoolName;
            model.Address = address;
            model.Contact = contact;
            model.Phone = phone;
            model.VoteObjectHeadImage=voteObjectHeadImage;
            if (bllVote.Update(model))
            {
                resp.errmsg = "保存成功";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "保存失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateVoteObjectVoteCount(HttpContext context)
        {
            int voteObjectID = int.Parse(context.Request["classid"]);
            VoteInfo voteInfo = bllVote.GetVoteInfo(bllVote.TotemaVoteID);
            if (voteInfo.VoteStatus.Equals(0))
            {
                resp.errcode =1;
                resp.errmsg = "投票停止";
                goto outoff;

            }
            if (DateTime.Now>(DateTime.Parse(voteInfo.StopDate)))
            {
                resp.errcode = 1;
                resp.errmsg = "投票结束";
                goto outoff;

            }
            ////检查是否可以投票
            //if (!bllVote.IsCanVote(bllVote.TotemaVoteID, CurrentUserInfo.UserID, 1))
            //{
            //    resp.errcode=1;
            //    resp.errmsg = "您已经投过票了";
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}
            #region 验证码
            //string VerCode = context.Request["VerCode"];
            //if (string.IsNullOrEmpty(VerCode))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入验证码";
            //    goto outoff;
            //}
            //if (context.Session["CheckCode"] == null)
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入验证码";
            //    goto outoff;
            //}
            //if (!VerCode.Equals(context.Session["CheckCode"].ToString()))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "验证码错误";
            //    goto outoff;
            //} 
            #endregion



             


            //检查是否可以投票
            if (bllVote.GetCount<VoteLogInfo>(string.Format("VoteObjectID={0} And UserID='{1}'",voteObjectID,CurrentUserInfo.UserID))>0)
            {
                resp.errcode = 1;
                resp.errmsg = "您已经投过票了";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //
            if (bllVote.UpdateVoteObjectVoteCount(bllVote.TotemaVoteID, voteObjectID, CurrentUserInfo.UserID, 1))
            {

                if (!bllVote.UpdateVoteObjectRank(bllVote.TotemaVoteID, "1"))
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新排名失败";
                    goto outoff;
                }
                resp.errmsg = "投票成功!";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "投票失败";
            }
            outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取班级列表 分页 搜索
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetVoteObjectVoteList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string voteObjectName = context.Request["name"];
            string voteObjectNumber = context.Request["number"];
            int totalcount = 0;//总数量
            ClassModelApi apiResult = new ClassModelApi();
            apiResult.list = new List<ClassModel>();
            List<VoteObjectInfo> sourceList = bllVote.GetVoteObjectInfoList(bllVote.TotemaVoteID, pageIndex, pageSize, out totalcount, voteObjectName, voteObjectNumber, "1");
            foreach (var item in sourceList)
            {
                ClassModel model = new ClassModel();
                model.classid = item.AutoID;
                model.classimage = item.VoteObjectHeadImage;
                model.classname = item.VoteObjectName;
                model.classnumber = item.Number;
                model.rank = item.Rank;
                model.votecount = item.VoteCount;
                model.watchword = item.Introduction;
                apiResult.list.Add(model);
            }
            apiResult.totalcount = totalcount;
            return Common.JSONHelper.ObjectToJson(apiResult);

        }




        /// <summary>
        /// 默认响应模型
        /// </summary>
        private class DefaultResponse
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }

        }

        /// <summary>
        /// 班级模型
        /// </summary>
        private class ClassModel
        {
            /// <summary>
            /// 班级编号
            /// </summary>
            public int classid { get; set; }
            /// <summary>
            /// 编号
            /// </summary>
            public string classnumber { get; set; }
            /// <summary>
            /// 班级名称
            /// </summary>
            public string classname { get; set; }
            /// <summary>
            /// 班级图片
            /// </summary>
            public string classimage { get; set; }
            /// <summary>
            /// 得票数
            /// </summary>
            public int votecount { get; set; }
            /// <summary>
            /// 口号
            /// </summary>
            public string watchword { get; set; }
            /// <summary>
            /// 排名
            /// </summary>
            public int rank { get; set; }

        }

        private class ClassModelApi{
            /// <summary>
            /// 总数
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 班级列表
            /// </summary>
            public List<ClassModel> list { get; set; }
        
        
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