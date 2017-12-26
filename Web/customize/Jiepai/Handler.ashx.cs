using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using System.Reflection;

namespace ZentCloud.JubitIMP.Web.customize.Jiepai
{
    /// <summary>
    /// 世界街拍处理文件
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
        UserInfo currentUserInfo = new UserInfo();
        /// <summary>
        /// 世界街拍投票ID
        /// </summary>
        int jiePaiVoteId;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                jiePaiVoteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["JiePaiVoteId"]);
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
            string ex1 = context.Request["Ex1"];
            string ex2 = context.Request["Ex2"];
            string number = (bllVote.GetVoteObjectMaxNumber(jiePaiVoteId) + 1).ToString();
            string introduction = context.Request["Introduction"];
            string address = context.Request["Address"];
            string showImage1 = context.Request["ShowImage1"];
            string showImage2 = context.Request["ShowImage2"];
            string showImage3 = context.Request["ShowImage3"];
            string showImage4 = context.Request["ShowImage4"];
            VoteObjectInfo voteObjectInfo = bllVote.GetVoteObjectInfo(jiePaiVoteId, currentUserInfo.UserID);

          
            if (string.IsNullOrEmpty(address))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入所在地";
            }
            if (string.IsNullOrEmpty(ex1))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入微信号";
            }

            if (string.IsNullOrEmpty(introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入粉丝祝福接力";
            }
            if (string.IsNullOrEmpty(showImage1))
            {
                resp.errcode = 1;
                resp.errmsg = "请上传第一张照片";
            }
            if (string.IsNullOrEmpty(showImage2))
            {
                resp.errcode = 1;
                resp.errmsg = "请上传第二张照片";
            }
            if (string.IsNullOrEmpty(showImage3))
            {
                resp.errcode = 1;
                resp.errmsg = "请上传第三张照片";
            }
            if (string.IsNullOrEmpty(showImage4))
            {
                resp.errcode = 1;
                resp.errmsg = "请上传第四张照片";
            }
            if (bllVote.IsExitsVoteObjectNumber(jiePaiVoteId, number))
            {
                resp.errcode = 1;
                resp.errmsg = "编号已经存在";
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
            if (!string.IsNullOrWhiteSpace(showImage3))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle3 = Convert.ToInt32(context.Request["imgAngle3"]);
                if (imgAngle3 != 0)
                {
                    imgAngle3 = RevertAngle(imgAngle3);
                    //获取源图片地址
                    string imgPath3 = context.Server.MapPath(showImage3);
                    var imgResult = imgHelper.RotateImg(imgPath3, imgAngle3, imgPath3);

                }
            }
            if (!string.IsNullOrWhiteSpace(showImage4))
            {
                //图片处理：根据前端传入角度进行图片旋转处理
                int imgAngle4 = Convert.ToInt32(context.Request["imgAngle4"]);
                if (imgAngle4 != 0)
                {
                    imgAngle4 = RevertAngle(imgAngle4);
                    //获取源图片地址
                    string imgPath4 = context.Server.MapPath(showImage4);
                    var imgResult = imgHelper.RotateImg(imgPath4, imgAngle4, imgPath4);

                }
            }
           

            if (voteObjectInfo != null)
            {

                VoteObjectInfo model = bllVote.GetVoteObjectInfo(voteObjectInfo.AutoID);
                model.ShowImage1 = showImage1;
                model.ShowImage2 = showImage2;
                model.ShowImage3 = showImage3;
                model.ShowImage4 = showImage4;
                model.VoteObjectHeadImage = showImage4;
                model.Ex2 = ex2;
                if (bllVote.Update(model))
                {
                    resp.errcode = 0;
                    resp.Ex1 = model.AutoID.ToString();
                    resp.errmsg = "修改图片成功";
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "修改失败";
                }
            }
            else
            {

                VoteObjectInfo model = new VoteObjectInfo();
                model.VoteID = jiePaiVoteId;
                model.Number = number;
                model.Ex1 = ex1;
                model.VoteObjectHeadImage = showImage4;
                model.Introduction = introduction;
                model.CreateUserId = currentUserInfo.UserID;
                model.Rank = 0;
                model.VoteObjectName = number.ToString();
                model.Address = address;
                model.ShowImage1 = showImage1;
                model.ShowImage2 = showImage2;
                model.ShowImage3 = showImage3;
                model.ShowImage4 = showImage4;
                model.Ex2 = ex2;
                model.Status = 1;
                if (bllVote.AddVoteObjectInfo(model))
                {
                    VoteObjectInfo newModel = bllVote.Get<VoteObjectInfo>(string.Format("VoteID={0} And Number={1}", jiePaiVoteId, model.Number));
                    resp.Ex1 = newModel.AutoID.ToString();
                    bllVote.UpdateVoteObjectRank(jiePaiVoteId);
                    resp.errcode = 0;
                    resp.errmsg = "报名成功";
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "报名失败";
                }
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateVoteObjectVoteCount(HttpContext context)
        {

            int voteObjectID = int.Parse(context.Request["id"]);
            VoteInfo voteInfo = bllVote.GetVoteInfo(jiePaiVoteId);
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


            VoteObjectInfo model = bllVote.GetVoteObjectInfo(voteObjectID);
            if (!model.Status.Equals(1))
            {
                resp.errcode = 1;
                resp.errmsg = "审核通过的选手才能投票";
                goto outoff;
            }

            string dateTimeToDay = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTimeTomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //检查是否可以投票 每天5票
            int logCount = bllVote.GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}' And InsertDate>='{2}' And InsertDate<'{3}'", jiePaiVoteId, currentUserInfo.UserID, dateTimeToDay, dateTimeTomorrow));
            if (logCount >= 5)
            {
                resp.errcode = 1;
                resp.errmsg = "您今天的票数用完了,明天再来吧";
                goto outoff;
            }
            //检查是否可以投票 每天5票
            if (bllVote.UpdateVoteObjectVoteCount(jiePaiVoteId, voteObjectID, currentUserInfo.UserID, 1))
            {

                if (!bllVote.UpdateVoteObjectRank(jiePaiVoteId, "1"))
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
            List<VoteObjectInfo> sourceList = bllVote.GetVoteObjectInfoList(jiePaiVoteId, pageIndex, pageSize, out totalCount, keyWord, "", "1", sort);
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
                model.address = item.Address;
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
            /// <summary>
            /// 扩展ID
            /// </summary>
            public string Ex1 { get; set; }

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
            /// 所在地
            /// </summary>
            public string address { get; set; }


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