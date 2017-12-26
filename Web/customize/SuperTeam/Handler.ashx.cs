using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using System.Reflection;

namespace ZentCloud.JubitIMP.Web.customize.SuperTeam
{
    /// <summary>
    /// 处理文件
    /// </summary>
    public class Handler : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 点赞逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo = new UserInfo();
        /// <summary>
        /// 点赞ID
        /// </summary>
        int voteId;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                voteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SuperTeamVoteId"]);
                if (bllVote.IsLogin)
                {

                    currentUserInfo = bllVote.GetCurrentUserInfo();
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "尚未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    goto outoff;
                }
                string action = context.Request["action"];
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
            // string Age = context.Request["Age"];
            string introduction = context.Request["Introduction"];
            string number = (bllVote.GetVoteObjectMaxNumber(voteId) + 1).ToString();
            string phone = context.Request["Phone"];
            string showImage1 = context.Request["ShowImage1"];
            string ShowImage2 = context.Request["ShowImage2"];

            if (bllVote.GetVoteObjectInfo(voteId, currentUserInfo.UserID) != null)
            {
                resp.errcode = 1;
                resp.errmsg = "您已经报过名了";
                goto outoff;
            }

            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入团队名称";
                goto outoff;
            }

            //if (string.IsNullOrEmpty(Age))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入宝贝年龄";
            //    goto outoff;
            //}


            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入手机号码";
                goto outoff;
            }
            if (string.IsNullOrEmpty(introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "团队宣言";
                goto outoff;
            }
            if (bllVote.IsExitsVoteObjectNumber(voteId, number))
            {
                resp.errcode = 1;
                resp.errmsg = "编号已经存在";
                goto outoff;
            }

            //if (bllVote.GetCount<VoteObjectInfo>(string.Format(" VoteID={0} And Phone='{1}'", voteId, phone)) > 0)
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "此手机号已经报过名了";
            //    goto outoff;

            //}
            //CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();
            //if (!string.IsNullOrWhiteSpace(ShowImage1))
            //{
            //    //图片处理：根据前端传入角度进行图片旋转处理
            //    int imgAngle1 = Convert.ToInt32(context.Request["imgAngle1"]);
            //    if (imgAngle1 != 0)
            //    {
            //        imgAngle1 = RevertAngle(imgAngle1);
            //        //获取源图片地址
            //        string imgPath1 = context.Server.MapPath(ShowImage1);
            //        var imgResult = imgHelper.RotateImg(imgPath1, imgAngle1, imgPath1);

            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(ShowImage2))
            //{
            //    //图片处理：根据前端传入角度进行图片旋转处理
            //    int imgAngle2 = Convert.ToInt32(context.Request["imgAngle2"]);
            //    if (imgAngle2 != 0)
            //    {
            //        imgAngle2 = RevertAngle(imgAngle2);
            //        //获取源图片地址
            //        string imgPath2 = context.Server.MapPath(ShowImage2);
            //        var imgResult = imgHelper.RotateImg(imgPath2, imgAngle2, imgPath2);

            //    }
            //}

            VoteObjectInfo model = new VoteObjectInfo();
            model.VoteID = voteId;
            model.Number = number;
            model.VoteObjectName = voteObjectName;
            model.VoteObjectHeadImage = showImage1;
            model.Introduction = introduction;
            model.Phone = phone;
            model.CreateUserId = currentUserInfo.UserID;
            model.Rank = 0;
            model.ShowImage1 = showImage1;
            model.ShowImage2 = ShowImage2;
            model.Status = 0;

            model.Ex1 = context.Request["Ex1"];
            model.Ex2 = context.Request["Ex2"];
            model.Ex3 = context.Request["Ex3"];
            model.Ex4 = context.Request["Ex4"];
            model.Ex5 = context.Request["Ex5"];
            model.Ex6 = context.Request["Ex6"];
            model.Ex7 = context.Request["Ex7"];
            model.Ex8=context.Request["Ex8"];
            model.Area = context.Request["Area"];
            model.Contact = context.Request["Contact"];

            if (bllVote.AddVoteObjectInfo(model))
            {
                //VoteObjectInfo newModel = bllVote.Get<VoteObjectInfo>(string.Format("VoteID={0} And Number={1}", voteId, model.Number));
                //resp.errmsg = newModel.AutoID.ToString();
                bllVote.UpdateVoteObjectRank(voteId, "1");
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
        /// 更新参赛资料
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateVoteObjectInfo(HttpContext context)
        {

            string voteObjectName = context.Request["VoteObjectName"];
            // string Age = context.Request["Age"];
            string introduction = context.Request["Introduction"];
            string phone = context.Request["Phone"];
            string showImage1 = context.Request["ShowImage1"];
             string ShowImage2 = context.Request["ShowImage2"];
            var voteObjInfo = bllVote.GetVoteObjectInfo(voteId, currentUserInfo.UserID);
            if (voteObjInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "您还未报名";
                goto outoff;
            }
            if (voteObjInfo.Status == 1)
            {
                resp.errcode = 1;
                resp.errmsg = "您的资料已经通过审核，审核通过后不可以修改。";
                goto outoff;
            }
            if (string.IsNullOrEmpty(voteObjectName))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入姓名";
                goto outoff;
            }

            //if (string.IsNullOrEmpty(Age))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请输入宝贝年龄";
            //    goto outoff;
            //}


            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入手机号码";
                goto outoff;
            }
            if (string.IsNullOrEmpty(introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入分享理由";
                goto outoff;
            }
            if (bllVote.GetCount<VoteObjectInfo>(string.Format(" VoteID={0} And Phone='{1}' And CreateUserId!='{2}'", voteId, phone, currentUserInfo.UserID)) > 0)
            {
                resp.errcode = 1;
                resp.errmsg = "此手机号已经被别的用户占用";
                goto outoff;

            }

            //CommonPlatform.Helper.ImageHandler imgHelper = new CommonPlatform.Helper.ImageHandler();
            //if (!string.IsNullOrWhiteSpace(ShowImage1))
            //{
            //    //图片处理：根据前端传入角度进行图片旋转处理
            //    int imgAngle1 = Convert.ToInt32(context.Request["imgAngle1"]);
            //    if (imgAngle1 != 0)
            //    {
            //        imgAngle1 = RevertAngle(imgAngle1);
            //        //获取源图片地址
            //        string imgPath1 = context.Server.MapPath(ShowImage1);
            //        var imgResult = imgHelper.RotateImg(imgPath1, imgAngle1, imgPath1);

            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(ShowImage2))
            //{
            //    //图片处理：根据前端传入角度进行图片旋转处理
            //    int imgAngle2 = Convert.ToInt32(context.Request["imgAngle2"]);
            //    if (imgAngle2 != 0)
            //    {
            //        imgAngle2 = RevertAngle(imgAngle2);
            //        //获取源图片地址
            //        string imgPath2 = context.Server.MapPath(ShowImage2);
            //        var imgResult = imgHelper.RotateImg(imgPath2, imgAngle2, imgPath2);

            //    }
            //}


            voteObjInfo.VoteObjectName = voteObjectName;
            voteObjInfo.VoteObjectHeadImage = showImage1;
            voteObjInfo.Introduction = introduction;
            voteObjInfo.Phone = phone;
            voteObjInfo.ShowImage1 = showImage1;
            voteObjInfo.ShowImage2 = ShowImage2;
            voteObjInfo.Ex1 = context.Request["Ex1"];
            voteObjInfo.Ex2 = context.Request["Ex2"];
            voteObjInfo.Ex3 = context.Request["Ex3"];
            voteObjInfo.Ex4 = context.Request["Ex4"];
            voteObjInfo.Ex5 = context.Request["Ex5"];
            voteObjInfo.Ex6 = context.Request["Ex6"];
            voteObjInfo.Ex7 = context.Request["Ex7"];
            voteObjInfo.Ex8 = context.Request["Ex8"];
            voteObjInfo.Area = context.Request["Area"];
            voteObjInfo.Contact = context.Request["Contact"];
            if (bllVote.Update(voteObjInfo))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
            }

            else
            {
                resp.errcode = 1;
                resp.errmsg = "修改个人资料失败";
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);


        }



        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateVoteObjectVoteCount(HttpContext context)
        {

            int voteObjectId = int.Parse(context.Request["id"]);
            VoteInfo voteInfo = bllVote.GetVoteInfo(voteId);
            //if (DateTime.Now < (new DateTime(2015, 7, 24, 0, 0, 0)))
            //{
            //    resp.errcode = 2;
            //    resp.errmsg = "点赞还未开始";
            //    goto outoff;
            //}
            if (voteInfo.VoteStatus.Equals(0))
            {
                resp.errcode = 1;
                resp.errmsg = "点赞停止";
                goto outoff;

            }
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now > (DateTime.Parse(voteInfo.StopDate)))
                {
                    resp.errcode = 1;
                    resp.errmsg = "点赞结束";
                    goto outoff;

                }
            }


            VoteObjectInfo voteObjInfo = bllVote.GetVoteObjectInfo(voteObjectId);
            if (!voteObjInfo.Status.Equals(1))
            {
                resp.errcode = 1;
                resp.errmsg = "审核通过的选手才能点赞";
                goto outoff;
            }

            string dateTimeToDay = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTimeTomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            //检查是否可以点赞 每天1票
            int logCount = bllVote.GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}' And InsertDate>='{2}' And InsertDate<'{3}' And VoteObjectID={4}", voteId, currentUserInfo.UserID, dateTimeToDay, dateTimeTomorrow,voteObjectId));
            if (logCount >=1)
            {
                resp.errcode = 1;
                resp.errmsg = "您今天已经给该团队点过赞了";
                goto outoff;
            }

            //检查是否可以点赞 每天共5票
            int logCountTotal = bllVote.GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}' And InsertDate>='{2}' And InsertDate<'{3}'", voteId, currentUserInfo.UserID, dateTimeToDay, dateTimeTomorrow));
            if (logCountTotal >= 5)
            {
                resp.errcode = 1;
                resp.errmsg = "您今天已经点过5次赞了,明天再来吧";
                goto outoff;
            }
            //检查是否可以点赞 每天1票
            if (bllVote.UpdateVoteObjectVoteCount(voteId, voteObjectId, currentUserInfo.UserID, 1))
            {

                if (!bllVote.UpdateVoteObjectRank(voteId, "1"))
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新排名失败";
                    goto outoff;
                }
                resp.errmsg = "点赞成功!";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "点赞失败";
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
            string area = context.Request["area"];
            int totalCount = 0;//总数量
            VoteObjectModelApi apiResult = new VoteObjectModelApi();
            apiResult.list = new List<VoteObjectModel>();
            List<VoteObjectInfo> sourceList = bllVote.GetVoteObjectInfoList(voteId, pageIndex, pageSize, out totalCount, keyWord, "", "1", sort, area);
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
                model.intro = item.Introduction;
                apiResult.list.Add(model);
            }
            apiResult.totalcount = totalCount;
            return Common.JSONHelper.ObjectToJson(apiResult);

        }


        ///// <summary>
        ///// 顺逆时针旋转角度对应
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private int RevertAngle(int value)
        //{

        //    switch (value)
        //    {
        //        case 90:
        //            return 270;
        //        case 270:
        //            return 90;
        //        default:
        //            return value;

        //    }
        //}


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
            /// <summary>
            /// 宣言
            /// </summary>
            public string intro { get; set; }


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