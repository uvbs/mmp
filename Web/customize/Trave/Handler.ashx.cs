using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using System.Reflection;

namespace ZentCloud.JubitIMP.Web.customize.Trave
{
    /// <summary>
    /// 小小旅行家宝贝处理文件
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
        /// <summary>
        /// 小小旅行家投票ID
        /// </summary>
        int TraveVoteId;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                TraveVoteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TraveVoteId"]);
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
                resp.errmsg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
        outoff:
            context.Response.Write(result);
        }

        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteObjectInfo(HttpContext context)
        {

            string voteObjectName = context.Request["VoteObjectName"];
            string age = context.Request["Age"];
            string introduction = context.Request["Introduction"];
            string number = (bllVote.GetVoteObjectMaxNumber(TraveVoteId) + 1).ToString();
            string phone = context.Request["Phone"];
            string address=context.Request["Address"];
            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];

            if (bllVote.GetVoteObjectInfo(TraveVoteId, CurrentUserInfo.UserID) != null)
            {
                resp.errcode = 1;
                resp.errmsg = "您已经报过名了";
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
                resp.errmsg = "请输入宝贝姓名";
                goto outoff;
            }

            if (string.IsNullOrEmpty(age))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入宝贝年龄";
                goto outoff;
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入联系人电话";
                goto outoff;
            }



            if (bllVote.IsExitsVoteObjectNumber(TraveVoteId, number))
            {
                resp.errcode = 1;
                resp.errmsg = "编号已经存在";
                goto outoff;
            }

            CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();
            if (!string.IsNullOrWhiteSpace(showImage1))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle1 = Convert.ToInt32(context.Request["imgAngle1"]);
                if (imgAngle1 != 0)
                {
                    imgAngle1 = RevertAngle(imgAngle1);
                    //获取源图片地址
                    string imgPath1 = context.Server.MapPath(showImage1);
                    var imgResult = imgHelper.RotateImg(imgPath1, imgAngle1, imgPath1);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage2))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle2 = Convert.ToInt32(context.Request["imgAngle2"]);
                if (imgAngle2 != 0)
                {
                    imgAngle2 = RevertAngle(imgAngle2);
                    //获取源图片地址
                    string imgPath2 = context.Server.MapPath(showImage2);
                    var imgResult = imgHelper.RotateImg(imgPath2, imgAngle2, imgPath2);

                }
            }

            VoteObjectInfo model = new VoteObjectInfo();
            model.VoteID = TraveVoteId;
            model.Number = number;
            model.VoteObjectName = voteObjectName;
            model.VoteObjectHeadImage = showImage1;
            model.Age = age;
            model.Introduction = introduction;
            model.Phone = phone;
            model.CreateUserId = CurrentUserInfo.UserID;
            model.Rank = 0;
            model.Address = address;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = showImage2;

            model.Status = 1;
            if (bllVote.AddVoteObjectInfo(model))
            {
                VoteObjectInfo newModel = bllVote.Get<VoteObjectInfo>(string.Format("VoteID={0} And Number={1}",TraveVoteId,model.Number));
                resp.errmsg =newModel.AutoID.ToString();
                bllVote.UpdateVoteObjectRank(TraveVoteId);
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
        /// 投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateVoteObjectVoteCount(HttpContext context)
        {

            int voteObjectId = int.Parse(context.Request["id"]);
            VoteInfo voteInfo = bllVote.GetVoteInfo(TraveVoteId);
            if (DateTime.Now < (new DateTime(2015, 7, 24, 0, 0, 0)))
            {
                resp.errcode = 2;
                resp.errmsg = "投票还未开始";
                goto outoff;
            }
            if (voteInfo.VoteStatus.Equals(0))
            {
                resp.errcode = 1;
                resp.errmsg = "投票停止";
                goto outoff;

            }
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now > (DateTime.Parse(voteInfo.StopDate)))
                {
                    resp.errcode = 1;
                    resp.errmsg = "投票结束";
                    goto outoff;

                }
            }


            VoteObjectInfo model = bllVote.GetVoteObjectInfo(voteObjectId);
            if (!model.Status.Equals(1))
            {
                resp.errcode = 1;
                resp.errmsg = "审核通过的选手才能投票";
                goto outoff;
            }

            string dateTimeToDay = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTimeTomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //检查是否可以投票 每天5票
            int logCount = bllVote.GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}' And InsertDate>='{2}' And InsertDate<'{3}'", TraveVoteId, CurrentUserInfo.UserID, dateTimeToDay, dateTimeTomorrow));
            if (logCount >=5)
            {
                resp.errcode = 1;
                resp.errmsg = "您今天的票数用完了,明天再来吧";
                goto outoff;
            }
            //检查是否可以投票 每天5票
            if (bllVote.UpdateVoteObjectVoteCount(TraveVoteId, voteObjectId, CurrentUserInfo.UserID, 1))
            {

                if (!bllVote.UpdateVoteObjectRank(TraveVoteId, "1"))
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
        /// 获取选手列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetVoteObjectVoteList(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["Pageindex"]);
            int pageSize = int.Parse(context.Request["Pagesize"]);
            string keyWord = context.Request["KeyWord"];
            string sort = context.Request["Sort"];
            int totalCount = 0;//总数量
            VoteObjectModelApi apiResult = new VoteObjectModelApi();
            apiResult.list = new List<VoteObjectModel>();
            List<VoteObjectInfo> sourceList = bllVote.GetVoteObjectInfoList(TraveVoteId, pageIndex, pageSize, out totalCount, keyWord, "", "1", sort);
            foreach (var item in sourceList)
            {
                VoteObjectModel model = new VoteObjectModel();
                model.id = item.AutoID;
                model.headimg = item.VoteObjectHeadImage;
                model.name = item.VoteObjectName;
                model.number = item.Number;
                model.rank = item.Rank;
                model.votecount = item.VoteCount;
                model.age = item.Age;
                apiResult.list.Add(model);
            }
            apiResult.totalcount = totalCount;
            return Common.JSONHelper.ObjectToJson(apiResult);

        }


        /// <summary>
        /// 顺逆时针旋转角度对应
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int RevertAngle(int value)
        {

            switch (value)
            {
                case 90:
                    return 270;
                case 270:
                    return 90;
                default:
                    return value;

            }
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
        /// 选手模型
        /// </summary>
        private class VoteObjectModel
        {
            /// <summary>
            /// 系统编号
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 编号
            /// </summary>
            public string number { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 头像
            /// </summary>
            public string headimg { get; set; }
            /// <summary>
            /// 得票数
            /// </summary>
            public int votecount { get; set; }
            /// <summary>
            /// 年龄
            /// </summary>
            public string age { get; set; }
            /// <summary>
            /// 排名
            /// </summary>
            public int rank { get; set; }

        }

        private class VoteObjectModelApi
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 选手列表
            /// </summary>
            public List<VoteObjectModel> list { get; set; }


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