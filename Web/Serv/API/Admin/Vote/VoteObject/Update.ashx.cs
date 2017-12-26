﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote.VoteObject
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
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
            if (requestModel.vote_object_id <= 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = " vote_id 为必填项,请检查";
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
            VoteObjectInfo model = bllVote.GetVoteObjectInfo(requestModel.vote_object_id);
            if (model == null)
            {
                resp.errmsg = "投票对象不存在";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            model.Introduction = requestModel.vote_object_introduction;
            model.VoteObjectName = requestModel.vote_object_name;
            model.VoteObjectHeadImage = requestModel.head_img_url;
            model.Area = requestModel.vote_area;
            #region 展示图片
            if (requestModel.show_img_list != null)
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
            #endregion
            model.Phone = requestModel.vote_object_phone;
            model.Ex1 = requestModel.ex1;
            model.Ex2 = requestModel.ex2;
            model.Ex3 = requestModel.ex3;
            model.Ex4 = requestModel.ex4;
            model.Ex5 = requestModel.ex5;
            model.Ex6 = requestModel.ex6;
            model.Ex7 = requestModel.ex7;
            model.Ex8 = requestModel.ex8;
            model.Ex9 = requestModel.ex9;
            model.Ex10 = requestModel.ex10;
            if (bllVote.Update(model))
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "修改投票对象信息出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }

        /// <summary>
        /// 请求模型
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 投票对象编号
            /// </summary>
            public int vote_object_id { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string vote_object_name { get; set; }
            /// <summary>
            /// 联系电话
            /// </summary>
            public string vote_object_phone { get; set; }
            /// <summary>
            /// 头像 主图
            /// </summary>
            public string head_img_url { get; set; }

            /// <summary>
            /// 地区
            /// </summary>
            public string vote_area { get; set; }
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