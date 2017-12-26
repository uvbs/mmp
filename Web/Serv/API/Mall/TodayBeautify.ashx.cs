using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    ///今日我最美
    /// </summary>
    public class TodayBeautify : BaseHandler
    {
        /// <summary>
        /// 今日我最美投票ID
        /// </summary>
        int voteId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TodayBeautifyVoteId"]);
        /// <summary>
        /// 投票业务逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();

        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 卡券业务逻辑
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// yike 
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// 图片BLL
        /// </summary>
        BLLJIMP.BLLImage bllImage = new BLLJIMP.BLLImage();
        /// <summary>
        /// 获取选手列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string VoteObjectList(HttpContext context)
        {


            if (bllUser.IsLogin)
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string sort = context.Request["sort"];
            int totalCount = 0;
            var sourceData = bllVote.GetVoteObjectInfoList(voteId, pageIndex, pageSize, out totalCount, keyWord, "", "1", sort);
            var list = from p in sourceData
                       select new
                       {
                           vote_object_id = p.AutoID,
                           vote_object_number = p.Number,
                           vote_object_name = p.VoteObjectName,
                           head_img_url = bllVote.GetImgUrl(p.VoteObjectHeadImage),
                           rank = p.Rank,
                           vote_count = p.VoteCount,
                           vote_object_status = p.Status,
                           is_my = (currentUserInfo != null && currentUserInfo.UserID == p.CreateUserId) ? 1 : 0,
                           vote_object_city = p.Area,
                           vote_object_age = p.Age,
                           vote_object_introduction = p.Introduction,
                           img_width = GetImageWidthHeight(context, p.VoteObjectHeadImage).Width,
                           img_height = GetImageWidthHeight(context, p.VoteObjectHeadImage).Height
                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list

            });

        }

        /// <summary>
        /// 获取选手详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string VoteObjectDetail(HttpContext context)
        {

            int voteObjectId = int.Parse(context.Request["vote_object_id"]);
            var voteObjectInfo = bllVote.GetVoteObjectInfo(voteObjectId);
            var firstRankObjectInfo = bllVote.GetFirstRankVoteObjectInfo(voteId);
            int diffFirstRank = 0;//距离第一名差多少票
            if (firstRankObjectInfo != null)
            {
                diffFirstRank = firstRankObjectInfo.VoteCount - voteObjectInfo.VoteCount;
            }
            List<string> showImgList = new List<string>();
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage1))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage1));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage2))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage2));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage3))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage3));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage4))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage4));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage5))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage5));
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                vote_object_id = voteObjectInfo.AutoID,
                vote_object_number = voteObjectInfo.Number,
                vote_object_name = voteObjectInfo.VoteObjectName,
                head_img_url = bllVote.GetImgUrl(voteObjectInfo.VoteObjectHeadImage),
                rank = voteObjectInfo.Rank,
                vote_count = voteObjectInfo.VoteCount,
                vote_object_status = voteObjectInfo.Status,
                vote_object_city = voteObjectInfo.Area,
                vote_object_age = voteObjectInfo.Age,
                vote_object_introduction = voteObjectInfo.Introduction,
                diff_first_rank = diffFirstRank,
                show_img_list = showImgList,
                is_my = (currentUserInfo != null && currentUserInfo.UserID == voteObjectInfo.CreateUserId) ? 1 : 0,
                is_vote =currentUserInfo!=null?IsVote(currentUserInfo.UserID,voteObjectInfo.AutoID):0
            });

        }


        /// <summary>
        /// 获取我参加的所有报名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyInfoList(HttpContext context)
        {

            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            List<VoteObjectInfo> myVoteObjectInfoList = bllVote.GetList<VoteObjectInfo>(string.Format(" VoteID={0} And CreateUserId='{1}'", voteId, currentUserInfo.UserID));

            var list = from p in myVoteObjectInfoList
                       select new
                       {
                           vote_id=p.VoteID,
                           vote_name=bllVote.GetVoteInfo(p.VoteID).VoteName,
                           vote_object_id = p.AutoID,
                           vote_object_number = p.Number,
                           vote_object_name = p.VoteObjectName,
                           head_img_url = bllVote.GetImgUrl(p.VoteObjectHeadImage),
                           rank = p.Rank,
                           vote_count = p.VoteCount,
                           vote_object_status = p.Status,
                           vote_object_city = p.Area,
                           vote_object_age = p.Age,
                           show_img_list = GetVoteObjectImageList(p),
                           vote_object_introduction =p.Introduction
                       };
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = myVoteObjectInfoList.Count,
                list = list

            });

        }

        /// <summary>
        /// 更新我的信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateMyInfo(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            string data = context.Request["data"];
            VoteObjectInfoReq voteObjectInfoReq;
            try
            {
                voteObjectInfoReq = ZentCloud.Common.JSONHelper.JsonToModel<VoteObjectInfoReq>(data);
            }
            catch (Exception ex)
            {

                resp.errcode = 1;
                resp.errmsg = "json格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //var myInfo = bllVote.GetVoteObjectInfo(voteId, currentUserInfo.UserID);
            var myInfo = bllVote.GetVoteObjectInfo(voteObjectInfoReq.vote_object_id);
            if (myInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "vote_object_id 参数不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(voteObjectInfoReq.vote_object_introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入参赛宣言";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (voteObjectInfoReq.show_img_list == null || voteObjectInfoReq.show_img_list.Count < 2)
            {
                resp.errcode = 1;
                resp.errmsg = "请上传至少两张照片";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            myInfo.Introduction = voteObjectInfoReq.vote_object_introduction;
            myInfo.Age = voteObjectInfoReq.vote_object_age;
            myInfo.Area = voteObjectInfoReq.vote_object_city;
            //myInfo.VoteObjectName = currentUserInfo.WXNickname;
            //if (string.IsNullOrEmpty(myInfo.VoteObjectName))

            //{
            //    myInfo.VoteObjectName = "用户" + currentUserInfo.AutoID.ToString();

            //}
            //myInfo.VoteObjectHeadImage = currentUserInfo.WXHeadimgurlLocal;
            for (int i = 0; i < voteObjectInfoReq.show_img_list.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        myInfo.ShowImage1 = voteObjectInfoReq.show_img_list[0];
                        break;
                    case 1:
                        myInfo.ShowImage2 = voteObjectInfoReq.show_img_list[1];
                        break;
                    case 2:
                        myInfo.ShowImage3 = voteObjectInfoReq.show_img_list[2];
                        break;
                    case 3:
                        myInfo.ShowImage4 = voteObjectInfoReq.show_img_list[3];
                        break;
                    case 4:
                        myInfo.ShowImage5 = voteObjectInfoReq.show_img_list[4];
                        break;
                    default:
                        break;
                }

            }

            if (bllVote.Update(myInfo))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 选手报名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteObject(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            string data = context.Request["data"];
            VoteObjectInfoReq voteObjectInfoReq;
            try
            {
                voteObjectInfoReq = ZentCloud.Common.JSONHelper.JsonToModel<VoteObjectInfoReq>(data);
            }
            catch (Exception ex)
            {

                resp.errcode = 1;
                resp.errmsg = "json格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //if (bllVote.GetVoteObjectInfo(voteId, currentUserInfo.UserID) != null)
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "你已经报过名了";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //} 
            if (string.IsNullOrEmpty(voteObjectInfoReq.vote_object_introduction))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入参赛宣言";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (voteObjectInfoReq.show_img_list == null || voteObjectInfoReq.show_img_list.Count < 2)
            {
                resp.errcode = 1;
                resp.errmsg = "请上传至少两张照片";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string number = (bllVote.GetVoteObjectMaxNumber(voteId) + 1).ToString();
            VoteObjectInfo model = new VoteObjectInfo();
            model.CreateUserId = currentUserInfo.UserID;
            model.Introduction = voteObjectInfoReq.vote_object_introduction;
            model.VoteID = voteId;
            model.Number = number;
            model.VoteObjectName = currentUserInfo.WXNickname;
            model.Area = voteObjectInfoReq.vote_object_city;
            model.Age = voteObjectInfoReq.vote_object_age;
            if (string.IsNullOrEmpty(model.VoteObjectName))
            {
                model.VoteObjectName = "用户" + currentUserInfo.AutoID.ToString();

            }
            model.VoteObjectHeadImage = currentUserInfo.WXHeadimgurlLocal;
            if (string.IsNullOrEmpty(model.VoteObjectHeadImage))
            {
                model.VoteObjectHeadImage = "/img/persion.png";
            }
            for (int i = 0; i < voteObjectInfoReq.show_img_list.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        model.ShowImage1 = voteObjectInfoReq.show_img_list[0];
                        break;
                    case 1:
                        model.ShowImage2 = voteObjectInfoReq.show_img_list[1];
                        break;
                    case 2:
                        model.ShowImage3 = voteObjectInfoReq.show_img_list[2];
                        break;
                    case 3:
                        model.ShowImage4 = voteObjectInfoReq.show_img_list[3];
                        break;
                    case 4:
                        model.ShowImage5 = voteObjectInfoReq.show_img_list[4];
                        break;
                    default:
                        break;
                }

            }

            if (bllVote.Add(model))
            {

               return ZentCloud.Common.JSONHelper.ObjectToJson(new { 
                errcode=0,
                errmsg="ok",
                vote_object_id=bllVote.Get<VoteObjectInfo>(string.Format(" VoteId={0} And Number={1}",voteId,model.Number)).AutoID
                });
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "提交失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteCount(HttpContext context)
        {

            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            int voteObjectId = int.Parse(context.Request["vote_object_id"]);
            VoteInfo voteInfo = bllVote.GetVoteInfo(voteId);
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
            if (voteObjInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "选手编号不存在";
                goto outoff;

            }
            if (voteObjInfo.CreateUserId == currentUserInfo.UserID)
            {
                resp.errcode = 1;
                resp.errmsg = "不可以为自己点赞哟";
                goto outoff;
            }

            if (!voteObjInfo.Status.Equals(1))
            {
                resp.errcode = 1;
                resp.errmsg = "审核通过的选手才能点赞";
                goto outoff;
            }



            string dateTimeToDay = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTimeTomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            //检查是否可以投票每天1个选手1票
            int logCount = bllVote.GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}' And InsertDate>='{2}' And InsertDate<'{3}' And VoteObjectID={4}", voteId, currentUserInfo.UserID, dateTimeToDay, dateTimeTomorrow,voteObjectId));
            if (logCount >= 1)
            {
                resp.errcode = -2;
                resp.errmsg = "已经点过赞,请确认是否使用积分点赞";
                goto outoff;
            }

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
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 使用积分点赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddVoteCountByScore(HttpContext context)
        {

            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            int voteObjectId = int.Parse(context.Request["vote_object_id"]);
            VoteInfo voteInfo = bllVote.GetVoteInfo(voteId);
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
            if (voteInfo.UseScore <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "暂未设置使用积分点赞";
                goto outoff;
            }
            #region 使用yike 积分

            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
            {

                if (!string.IsNullOrEmpty(currentUserInfo.Phone))
                {
                    Open.EZRproSDK.Entity.BonusGetResp yikeUser = yiKeClient.GetBonus(currentUserInfo.Ex1, currentUserInfo.Ex2, currentUserInfo.Phone);
                    if (yikeUser != null)
                    {

                        currentUserInfo.TotalScore = yikeUser.Bonus;

                    }

                }


            }
            #endregion
            if (currentUserInfo.TotalScore < voteInfo.UseScore)
            {
                resp.errcode = 1;
                resp.errmsg = "很遗憾,您的积分不足";
                goto outoff;
            }
            VoteObjectInfo voteObjInfo = bllVote.GetVoteObjectInfo(voteObjectId);
            if (voteInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "选手编号不存在";
                goto outoff;

            }
            if (voteObjInfo.CreateUserId == currentUserInfo.UserID)
            {
                resp.errcode = 1;
                resp.errmsg = "不可以为自己点赞哟";
                goto outoff;
            }
            if (!voteObjInfo.Status.Equals(1))
            {
                resp.errcode = 1;
                resp.errmsg = "审核通过的选手才能点赞";
                goto outoff;
            }
            if (bllVote.UpdateVoteObjectVoteCount(voteId, voteObjectId, currentUserInfo.UserID, 1))
            {

                if (!bllVote.UpdateVoteObjectRank(voteId, "1"))
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新排名失败";
                    goto outoff;
                }
                resp.errmsg = "点赞成功!";
                currentUserInfo.TotalScore -= voteInfo.UseScore;
                if (bllUser.Update(currentUserInfo, string.Format("TotalScore-={0}", currentUserInfo.TotalScore), string.Format(" AutoID={0}", currentUserInfo.AutoID)) > 0)
                {

                    //插入积分记录
                    WBHScoreRecord scoreRecord = new WBHScoreRecord();
                    scoreRecord.InsertDate = DateTime.Now;
                    scoreRecord.NameStr = string.Format("参加今日我最美点赞,使用{0}积分", voteInfo.UseScore);
                    scoreRecord.RecordType = "1";
                    scoreRecord.ScoreNum = voteInfo.UseScore.ToString();
                    scoreRecord.UserId = currentUserInfo.UserID;
                    scoreRecord.WebsiteOwner = bllUser.WebsiteOwner;
                    if (!bllUser.Add(scoreRecord))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "插入积分记录失败";
                        goto outoff;
                    }
                    //积分记录
                    #region 扣除yike 积分

                    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                    {

                        if (!string.IsNullOrEmpty(currentUserInfo.Phone))
                        {

                            yiKeClient.BonusUpdate(currentUserInfo.Ex2, -(voteInfo.UseScore), "参加今日我最美使用积分" + voteInfo.UseScore);

                        }


                    }
                    #endregion


                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新用户积分失败";
                    goto outoff;
                }


            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "点赞失败";
            }
        outoff:
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 获取奖品接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPrize(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            VoteInfo voteInfo = bllVote.GetVoteInfo(voteId);//主投票信息
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now > DateTime.Parse(voteInfo.StopDate))
                {

                    VoteObjectInfo myInfo = bllVote.GetVoteObjectInfo(voteId, currentUserInfo.UserID);
                    if (myInfo == null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "未参加";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }
                    if (myInfo.Ex1 == "1")//Ex1 表示是否已领奖
                    {
                        resp.errcode = 1;
                        resp.errmsg = "已领奖";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    if (myInfo.Rank == 1)
                    {
                        //领取奖品
                        ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                        try
                        {

                            #region 获得积分
                            if (voteInfo.Ex1 == "0")//获得积分
                            {
                                if (string.IsNullOrEmpty(voteInfo.Ex2) && (int.Parse(voteInfo.Ex2) > 0))
                                {
                                    int addScore = int.Parse(voteInfo.Ex2);//增加的积分
                                    if (bllUser.Update(currentUserInfo, string.Format(" TotalScore+={0}", addScore), string.Format(" AutoID={0}", currentUserInfo.AutoID), tran) <= 0)
                                    {
                                        tran.Rollback();
                                        resp.errcode = 1;
                                        resp.errmsg = "更新用户积分失败";
                                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


                                    }
                                    WBHScoreRecord scoreRecord = new WBHScoreRecord();
                                    scoreRecord.InsertDate = DateTime.Now;
                                    scoreRecord.NameStr = string.Format("参加今日我最美点赞,使用{0}积分", voteInfo.UseScore);
                                    scoreRecord.RecordType = "1";
                                    scoreRecord.ScoreNum = voteInfo.UseScore.ToString();
                                    scoreRecord.UserId = currentUserInfo.UserID;
                                    scoreRecord.WebsiteOwner = bllUser.WebsiteOwner;
                                    if (!bllUser.Add(scoreRecord, tran))
                                    {
                                        tran.Rollback();
                                        resp.errcode = 1;
                                        resp.errmsg = "插入积分记录表失败";
                                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                    }

                                    #region 增加yike 积分

                                    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                                    {

                                        if (!string.IsNullOrEmpty(currentUserInfo.Phone))
                                        {

                                            yiKeClient.BonusUpdate(currentUserInfo.Ex2, (voteInfo.UseScore), "参加今日我最美获得积分" + voteInfo.UseScore);

                                        }


                                    }
                                    #endregion


                                }
                            }

                            #endregion



                            #region 获得优惠券
                            if (voteInfo.Ex1 == "1")//获得积分
                            {
                                if (string.IsNullOrEmpty(voteInfo.Ex2) && (int.Parse(voteInfo.Ex2) > 0))
                                {

                                    int cardCouponId = int.Parse(voteInfo.Ex2);//优惠券编号
                                    CardCoupons cardCoupon = bllCardCoupon.GetCardCoupon(cardCouponId);//主卡券编号
                                    MyCardCoupons myCard = new MyCardCoupons();
                                    myCard.CardCouponNumber = string.Format("No.{0}{1}", DateTime.Now.ToString("yyyyMMdd"), new Random().Next(11111, 99999).ToString());
                                    myCard.CardCouponType = cardCoupon.CardCouponType;
                                    myCard.CardId = cardCouponId;
                                    myCard.InsertDate = DateTime.Now;
                                    myCard.UserId = currentUserInfo.UserID;
                                    myCard.WebSiteOwner = bllUser.WebsiteOwner;
                                    if (!bllCardCoupon.Add(myCard, tran))
                                    {
                                        tran.Rollback();
                                        resp.errcode = 1;
                                        resp.errmsg = "领取优惠券失败";
                                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                    }

                                }
                            }

                            #endregion


                        }
                        catch (Exception)
                        {

                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "领取失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }


                        //最后更新选手领奖状态为已经领奖
                        myInfo.Ex1 = "1";
                        if (!bllVote.Update(myInfo))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "更新领奖状态失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                        }
                        tran.Commit();
                        resp.errcode = 0;
                        resp.errmsg = voteInfo.Prize;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }

                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "还未到截止时间";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }


            }
            resp.errcode = 1;
            resp.errmsg = "";
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 检查是否中奖了
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string IsWin(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            VoteInfo voteInfo = bllVote.GetVoteInfo(voteId);//主投票信息
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now > DateTime.Parse(voteInfo.StopDate))
                {

                    VoteObjectInfo model = bllVote.GetVoteObjectInfo(voteId, currentUserInfo.UserID);
                    if (model == null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "未参加";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }
                    if (model.Ex1 == "1")//Ex1 表示是否已领奖
                    {
                        resp.errcode = 1;
                        resp.errmsg = "已领奖";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    if (model.Rank == 1)
                    {
                        resp.errcode = 0;
                        resp.errmsg = voteInfo.Prize;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }

                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "还未到截止时间";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }


            }
            resp.errcode = 1;
            resp.errmsg = "";
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 评论列表接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CommentList(HttpContext context)
        {
            string voteObjectId = context.Request["vote_object_id"];//选手id
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(" 1=1 ");
            sbWhere.AppendFormat(" And ReviewType='todaybeautify' And WebsiteOwner='{0}' And ForeignkeyId='{1}'", bllVote.WebsiteOwner, voteObjectId);
            int totalCount = bllVote.GetCount<ReviewInfo>(sbWhere.ToString());
            var sourceData = bllVote.GetLit<ReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            var list = from p in sourceData
                       select new
                       {
                           comment_id = p.AutoId,
                           comment_user_head_img_url = !string.IsNullOrEmpty(p.Expand1) ? bllVote.GetImgUrl(p.Expand1) : string.Format("http://{0}/img/persion.png", context.Request.Url.Host),
                           comment_user_nickname = p.Ex2,
                           comment_content = p.ReviewContent,
                           comment_time = bllVote.GetTimeStamp(p.InsertDate)
                       };
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list
            });

        }

        /// <summary>
        /// 增加评论接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddComment(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.errcode = (int)APIErrCode.UserIsNotLogin;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
            }
            string voteObjectId = context.Request["vote_object_id"];//选手id
            string commentContent = context.Request["comment_content"];//评论内容
            if (string.IsNullOrEmpty(voteObjectId))
            {
                resp.errcode = 1;
                resp.errmsg = "选手id不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(commentContent))
            {
                resp.errcode = 1;
                resp.errmsg = "评论内容不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            ReviewInfo model = new ReviewInfo();
            model.ReviewType = "todaybeautify";
            model.ForeignkeyId = voteObjectId;
            model.Expand1 = currentUserInfo.WXHeadimgurlLocal;
            model.Ex2 = currentUserInfo.WXNickname;
            model.ReviewContent = commentContent;
            model.WebsiteOwner = bllVote.WebsiteOwner;
            model.InsertDate = DateTime.Now;
            if (bllVote.Add(model))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "评论失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取使用积分
        /// </summary>
        /// <returns></returns>
        private string GetUseScore(HttpContext context)
        {

            VoteInfo voteInfo = bllVote.GetVoteInfo(voteId);//主投票信息
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {

                errcode = 0,
                use_score = voteInfo.UseScore


            });

        }

        /// <summary>
        /// 获取投票信息
        /// </summary>
        /// <returns></returns>
        private string GetVoteInfo(HttpContext context)
        {

            VoteInfo voteInfo = bllVote.GetVoteInfo(voteId);//主投票信息
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {

                vote_name = voteInfo.VoteName,
                vote_status = voteInfo.VoteStatus,
                stop_time = bllVote.GetTimeStamp(DateTime.Parse(voteInfo.StopDate))

            });

        }

        /// <summary>
        /// 获取投票信息
        /// </summary>
        /// <returns></returns>
        private string GetVoteRecordList(HttpContext context)
        {
            string voteObjectId = context.Request["vote_object_id"];
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            List<VoteLogInfo> sourceList = bllVote.GetLit<VoteLogInfo>(pageSize, pageIndex, string.Format(" VoteId={0} And VoteObjectID={1}", voteId, voteObjectId), " AutoId DESC");

            var totalCount = bllVote.GetCount<VoteLogInfo>(string.Format(" VoteId={0} And VoteObjectID={1}", voteId, voteObjectId));
            List<object> list = new List<object>();
            foreach (var item in sourceList)
            {
                UserInfo userInfo = bllUser.GetUserInfo(item.UserID);
                if (userInfo != null)
                {
                    list.Add(
                        new
                        {
                            head_img_url = userInfo.WXHeadimgurl
                        }

                        );
                }
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list

            });

        }


        private List<string> GetVoteObjectImageList(VoteObjectInfo voteObjectInfo)
        {
            List<string> showImgList = new List<string>();
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage1))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage1));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage2))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage2));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage3))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage3));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage4))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage4));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage5))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage5));
            }
            return showImgList;



        }

        /// <summary>
        /// 检查是否已经给选手投过票
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="voteObjectId"></param>
        /// <returns></returns>
        public int IsVote(string userId, int voteObjebctId)
        {
            if (bllVote.GetCount<VoteLogInfo>(string.Format(" VoteObjectID={0} And UserID='{1}'", voteObjebctId, userId)) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }



        }
        /// <summary>
        /// 请求模型
        /// </summary>
        private class VoteObjectInfoReq
        {
            /// <summary>
            /// 投票对象编号
            /// </summary>
            public int vote_object_id { get; set; }
            /// <summary>
            /// 城市
            /// </summary>
            public string vote_object_city { get; set; }
            /// <summary>
            /// 年龄
            /// </summary>
            public string vote_object_age { get; set; }
            /// <summary>
            /// 宣员
            /// </summary>
            public string vote_object_introduction { get; set; }

            /// <summary>
            /// 展示图片
            /// </summary>
            public List<string> show_img_list { get; set; }


        }

        /// <summary>
        /// 获取图片宽高
        /// </summary>
        /// <param name="context"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ImageWidthHeightModel GetImageWidthHeight(HttpContext context, string path)
        {

            try
            {
                if (path.StartsWith("http://"))
                {
                    path = path.Substring(path.IndexOf("/FileUpload"));
                }
                path = context.Server.MapPath(path);
                ImageWidthHeightModel model = new ImageWidthHeightModel();
                int width = 0;
                int height = 0;
                bllImage.GetImageWidthHeight(path, out width, out height);
                model.Width = width;
                model.Height = height;
                return model;
            }
            catch (Exception)
            {

                return new ImageWidthHeightModel();
            }




        }
        /// <summary>
        /// 图片宽高模型
        /// </summary>
        public class ImageWidthHeightModel
        {
            /// <summary>
            /// 宽
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// 高
            /// </summary>
            public int Height { get; set; }

        }




    }
}