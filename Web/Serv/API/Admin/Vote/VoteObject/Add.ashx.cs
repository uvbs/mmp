using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote.VoteObject
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 投票逻辑层
        /// </summary>
        /// <param name="context"></param>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(data);
            }
            catch (Exception)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "json格式错误,请检查。";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.vote_id <= 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = " vote_id 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var model = bllVote.GetVoteObjectInfo(requestModel.vote_id, currentUserInfo.UserID);
            if (model != null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                resp.errmsg = " 你已经报过名了";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.vote_object_name))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "vote_object_name 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.head_img_url))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = " head_img_url 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            string number = (bllVote.GetVoteObjectMaxNumber(requestModel.vote_id) + 1).ToString();
            VoteObjectInfo voteObject = new VoteObjectInfo();
            voteObject.VoteID = requestModel.vote_id;
            voteObject.Number = number;
            voteObject.VoteObjectName = requestModel.vote_object_name;
            voteObject.VoteObjectHeadImage = requestModel.head_img_url;
            voteObject.Area = requestModel.vote_area;
            voteObject.Introduction = requestModel.vote_object_introduction;
            voteObject.CreateUserId = currentUserInfo.UserID;
            if (requestModel.show_img_list!=null)
            {
                for (int i = 0; i < requestModel.show_img_list.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            model.ShowImage1 = requestModel.show_img_list[0];
                            break;
                        case 1:
                            model.ShowImage2 = requestModel.show_img_list[1];
                            break;
                        case 2:
                            model.ShowImage3 = requestModel.show_img_list[2];
                            break;
                        case 3:
                            model.ShowImage4 = requestModel.show_img_list[3];
                            break;
                        case 4:
                            model.ShowImage5 = requestModel.show_img_list[4];
                            break;
                        default:
                            break;
                    }
                }
            }
            voteObject.Phone = requestModel.vote_object_phone;
            voteObject.Ex1 = requestModel.ex1;
            voteObject.Ex2 = requestModel.ex2;
            voteObject.Ex3 = requestModel.ex3;
            voteObject.Ex4 = requestModel.ex4;
            voteObject.Ex5 = requestModel.ex5;
            voteObject.Ex6 = requestModel.ex6;
            voteObject.Ex7 = requestModel.ex7;
            voteObject.Ex8 = requestModel.ex8;
            voteObject.Ex9 = requestModel.ex9;
            voteObject.Ex10 = requestModel.ex10;
            if (bllVote.Add(voteObject))
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "添加投票对象出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
        /// <summary>
        /// 请求模型
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 投票活动编号
            /// </summary>
            public int vote_id { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string vote_object_name { get; set; }

            /// <summary>
            ///地区
            /// </summary>
            public string vote_area { get; set; }

            /// <summary>
            /// 联系电话
            /// </summary>
            public string vote_object_phone { get; set; }
            /// <summary>
            /// 头像 主图
            /// </summary>
            public string head_img_url { get; set; }
            /// <summary>
            /// 比赛宣言
            /// </summary>
            public string vote_object_introduction { get; set; }
            /// <summary>
            /// 展示图片
            /// </summary>
            public List<string> show_img_list { get; set; }
            /// <summary>
            /// 扩展字段1
            /// </summary>
            public string ex1 { get; set; }
            /// <summary>
            /// 扩展字段2
            /// </summary>
            public string ex2 { get; set; }
            /// <summary>
            /// 扩展字段3
            /// </summary>
            public string ex3 { get; set; }
            /// <summary>
            /// 扩展字段4
            /// </summary>
            public string ex4 { get; set; }
            /// <summary>
            /// 扩展字段5
            /// </summary>
            public string ex5 { get; set; }
            /// <summary>
            /// 扩展字段6
            /// </summary>
            public string ex6 { get; set; }
            /// <summary>
            /// 扩展字段7
            /// </summary>
            public string ex7 { get; set; }
            /// <summary>
            /// 扩展字段8
            /// </summary>
            public string ex8 { get; set; }
            /// <summary>
            /// 扩展字段9
            /// </summary>
            public string ex9 { get; set; }
            /// <summary>
            /// 扩展字段10
            /// </summary>
            public string ex10 { get; set; }


        }
    }
}