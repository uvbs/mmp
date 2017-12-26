using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using ZentCloud.BLLJIMP;
using System.Web.SessionState;
using System.Text;

using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXWuBuHuiUserHandler 的摘要说明
    /// </summary>
    public class WXWuBuHuiUserHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();  //活动数据
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();  //用户数据
        /// <summary>
        /// 
        /// </summary>
        BLLUserShare bllUserShare = new BLLUserShare();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo; //当前登陆的用户
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                string action = context.Request["Action"];
                this.currentUserInfo = bllUser.GetCurrentUserInfo();
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "请联系管理员";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 分享文章加积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveShareNew(HttpContext context)
        {

            string id = context.Request["id"];
            string wxShareType = context.Request["wxsharetype"];
            ReturnValue returnValue;
            BLLUserShare.ShareType shareType;
            if (wxShareType.Equals("1"))    //分享到朋友圈
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorShareArticleToWXFriendGroup : BLLUserShare.ShareType.ShareArticleToWXFriendGroup;
            }
            else //分享到朋友
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorShareArticleToWXFriendGroup : BLLUserShare.ShareType.ShareArticleToWXFriend;
            }
            returnValue = bllUserShare.RecordUserShare(currentUserInfo, shareType, id, wxShareType, bllJuactivity.WebsiteOwner, true);
            resp.Status = returnValue.Code;
            resp.Msg = returnValue.Msg;
            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 分享职位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveSharePosition(HttpContext context)
        {

            string id = context.Request["id"];
            string wxShareType = context.Request["wxsharetype"];
            ReturnValue returnValue;
            BLLUserShare.ShareType shareType;
            if (wxShareType.Equals("1"))    //分享到朋友圈
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorSharePositionToWXFriendGroup : BLLUserShare.ShareType.SharePositionToWXFriendGroup;
            }
            else //分享到朋友
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorSharePositionToWXFriendGroup : BLLUserShare.ShareType.SharePositionToWXFriend;
            }
            returnValue = bllUserShare.RecordUserShare(currentUserInfo, shareType, id, wxShareType, bllJuactivity.WebsiteOwner, true);
            resp.Status = returnValue.Code;
            resp.Msg = returnValue.Msg;
            return Common.JSONHelper.ObjectToJson(resp);



        }

        /// <summary>
        /// 分享活动加积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveShareActivity(HttpContext context)
        {

            string id = context.Request["id"];
            string wxShareType = context.Request["wxsharetype"];

            ReturnValue returnValue;
            BLLUserShare.ShareType shareType;
            if (wxShareType.Equals("1"))    //分享到朋友圈
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorShareActivityToWXFriendGroup : BLLUserShare.ShareType.ShareActivityToWXFriendGroup;
            }
            else //分享到朋友
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorShareActivityToWXFriendGroup : BLLUserShare.ShareType.ShareActivityToWXFriend;
            }
            returnValue = bllUserShare.RecordUserShare(currentUserInfo, shareType, id, wxShareType, bllJuactivity.WebsiteOwner, true);
            resp.Status = returnValue.Code;
            resp.Msg = returnValue.Msg;
            return Common.JSONHelper.ObjectToJson(resp);



        }

        /// <summary>
        /// 分享导师加积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveShareTotor(HttpContext context)
        {

            string id = context.Request["id"];
            string wxShareType = context.Request["wxsharetype"];

            ReturnValue returnValue;
            BLLUserShare.ShareType shareType;
            if (wxShareType.Equals("1"))    //分享到朋友圈
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorShareTutorToWXFriendGroup : BLLUserShare.ShareType.ShareTutorToWXFriendGroup;
            }
            else //分享到朋友
            {
                shareType = IsTutor(currentUserInfo.UserID) ? BLLUserShare.ShareType.TutorShareTutorToWXFriendGroup : BLLUserShare.ShareType.ShareTutorToWXFriend;
            }
            returnValue = bllUserShare.RecordUserShare(currentUserInfo, shareType, id, wxShareType, bllJuactivity.WebsiteOwner, true);
            resp.Status = returnValue.Code;
            resp.Msg = returnValue.Msg;
            return Common.JSONHelper.ObjectToJson(resp);



        }


        ///// <summary>
        ///// 呼朋唤友添加积分
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string SavaJifen(HttpContext context)
        //{
        //    try
        //    {
        //        BLLJIMP.Model.TutorInfo tInfo = juActivityBll.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", this.userInfo.UserID));
        //        BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
        //        {
        //            NameStr = "呼朋唤友",
        //            Nums = "b55",
        //            InsertDate = DateTime.Now,
        //            ScoreNum = "+10",
        //            WebsiteOwner = this.websiteOwner,
        //            UserId = this.userInfo.UserID,
        //            RecordType = "2"
        //        };
        //        if (tInfo != null)
        //        {
        //            UpdateUserToTalScore(this.userInfo.UserID, 20);
        //            srInfo.ScoreNum = "+20";
        //        }
        //        else
        //        {
        //            UpdateUserToTalScore(this.userInfo.UserID, 10);
        //        }
        //        bool bo = juActivityBll.Add(srInfo);
        //        if (bo)
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "转发成功";
        //        }
        //        else
        //        {
        //            resp.Status = -1;
        //            resp.Msg = "转发成功";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = ex.Message;
        //    }
        //    return Common.JSONHelper.ObjectToJson(resp);
        //}

        /// <summary>
        /// 获取话题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReviewInfos(HttpContext context)
        {

            string pageIndex = context.Request["PageIndex"];
            string pageSize = context.Request["PageSize"];
            List<BLLJIMP.Model.ReviewInfo> data = bllJuactivity.GetLit<BLLJIMP.Model.ReviewInfo>(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex), string.Format(" websiteOwner='{0}' AND ReviewType='话题'", bllUser.WebsiteOwner), " RDataTime DESC");
            if (data != null && data.Count > 0)
            {
                resp.Status = 0;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
                resp.ExObj = data;

            }

            return Common.JSONHelper.ObjectToJson(resp);



        }

        /// <summary>
        /// 获取个人关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MyReviewInfo(HttpContext context)
        {

            string pageIndex = context.Request["PageIndex"];
            string pageSize = context.Request["PageSize"];
            List<BLLJIMP.Model.ReviewInfo> data = bllJuactivity.GetLit<BLLJIMP.Model.ReviewInfo>(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex), string.Format(" websiteOwner='{0}' AND ReviewType='话题' AND UserId='{1}'", bllUser.WebsiteOwner, this.currentUserInfo.UserID), " RDataTime DESC");
            if (data != null)
            {
                resp.Status = 0;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
                resp.ExObj = data;

            }

            return Common.JSONHelper.ObjectToJson(resp);




        }

        /// <summary>
        /// 获取导师列表
        /// </summary>
        /// <returns></returns>
        private string TutorInfos(HttpContext context)
        {
            string pageIndex = context.Request["PageIndex"];
            string pageSize = context.Request["PageSize"];
            // string type = context.Request["type"];
            string title = context.Request["Title"];
            string sort = context.Request["Sort"];

            string tradeIds = context.Request["Tradeids"];

            string professionalIds = context.Request["ProfessionalIds"];

            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrderBy = new StringBuilder();

            sbWhere.AppendFormat(" WebsiteOwner='{0}' And UserId!='' ", bllJuactivity.WebsiteOwner);

            if ((!string.IsNullOrEmpty(tradeIds)))
            {
                // wsb.AppendFormat(" AND (TradeStr like '%{0}%'", type);
                //wsb.AppendFormat(" or ProfessionalStr like '%{0}%')", type);


                //
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < tradeIds.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" TradeStr Like '%{0}%' ", tradeIds.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or TradeStr Like '%{0}%' ", tradeIds.Split(',')[i]);

                    }




                }

                sbWhere.AppendFormat(" ) ");

            }

            if ((!string.IsNullOrEmpty(professionalIds)))
            {
                // wsb.AppendFormat(" AND (TradeStr like '%{0}%'", type);
                //wsb.AppendFormat(" or ProfessionalStr like '%{0}%')", type);


                //
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < professionalIds.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" ProfessionalStr Like '%{0}%' ", professionalIds.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or ProfessionalStr Like '%{0}%' ", professionalIds.Split(',')[i]);

                    }




                }

                sbWhere.AppendFormat(" ) ");

            }




            if (!string.IsNullOrEmpty(title))
            {
                sbWhere.AppendFormat(" AND (TutorName like '%{0}%' Or Company like '%{0}%' Or Position like '%{0}%' Or City = '{0}')", title);
            }
            sbOrderBy.Append(" RDataTime DESC");
            if (sort.Equals("rdateTime"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" ReviewDateTime DESC");
            }
            if (sort.Equals("rMany"))
            {
                sbOrderBy.Clear();
                //ssb.Append(" HTNums DESC");
                sbOrderBy.Append("TutorLikes DESC");
            }

            if (sort.Equals("wzMany"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" WZNums DESC");
            }

            List<BLLJIMP.Model.TutorInfo> tutorList = bllJuactivity.GetLit<BLLJIMP.Model.TutorInfo>(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex), sbWhere.ToString(), sbOrderBy.ToString());

            if (tutorList != null)
            {
                foreach (BLLJIMP.Model.TutorInfo item in tutorList)
                {
                    if (!string.IsNullOrEmpty(item.TradeStr))
                    {
                        string str = item.TradeStr + item.ProfessionalStr;
                        item.actegory = bllJuactivity.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" AutoID IN ({0}) AND WebsiteOwner='{1}'", str.Substring(0, str.Length - 1), bllUser.WebsiteOwner));
                    }
                }
                resp.Status = 0;
                resp.ExObj = tutorList;
            }
            else
            {
                resp.Status = -1;
                resp.ExObj = tutorList;

            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取话题列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.ReviewInfo> data;
            string reviewTitle = context.Request["ReviewTitle"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);

            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}' AND ReviewType='话题'", bllUser.WebsiteOwner));
            if (!string.IsNullOrEmpty(reviewTitle))
            {
                sbWhere.AppendFormat(" AND ReviewTitle lIKE '%{0}%'", reviewTitle);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ReviewInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.ReviewInfo>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = data
    });
        }

        /// <summary>
        /// 删除话题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelReviewInfos(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请选择一行数据";
                goto OutF;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.ReviewInfo(), " AutoId in (" + ids + ")");
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除失败";
            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }



        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveInfo(HttpContext context)
        {
            string name = context.Request["Uname"];
            string phone = context.Request["UPhone"];
            string email = context.Request["UEmail"];
            string company = context.Request["UCompanyl"];
            string recommendUserId = context.Request["UserId"];//推荐人用户名
            //string SmsVerCode = context.Request["SmsVerCode"];
            string position = context.Request["Postion"];

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(company))
            {
                resp.Status = -1;
                resp.Msg = "请填写完整信息";
                goto outF;
            }

            if (name.Length >= 15)
            {
                resp.Status = -1;
                resp.Msg = "姓名长度在15字内";
                goto outF;

            }
            if (bllUser.GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And Phone='{1}'", bllUser.WebsiteOwner, phone)) > 0)
            {
                resp.Status = -1;
                resp.Msg = "手机号已经注册过";
                goto outF;
            }

            //if (string.IsNullOrEmpty(SmsVerCode))
            //{
            //    resp.Status = -1;
            //    resp.Msg = "请输入通关密码";
            //    goto outF;

            //}
            //if (context.Session["SmsVerificationCode"] == null)
            //{
            //    resp.Status = -1;
            //    resp.Msg = "请输入通关密码";
            //    goto outF;

            //}
            //if (!context.Session["SmsVerificationCode"].ToString().Equals(SmsVerCode))
            //{
            //    resp.Status = -1;
            //    resp.Msg = "通关密码不正确";
            //    goto outF;
            //}

            //注册会员加积分
            BLLUserScore bllScore = new BLLUserScore(this.currentUserInfo.UserID);
            bllScore.UpdateUserScoreWithWXTMNotify(bllScore.GetDefinedUserScore(BLLUserScore.UserScoreType.RegistWebSite), bllWeixin.GetAccessToken());

            //转发人加积分
            if (!string.IsNullOrEmpty(recommendUserId))//有转发人
            {
                SavaForwardingRecord(recommendUserId);

                //转发人加积分
                bllScore = new BLLUserScore(recommendUserId);
                if (IsTutor(recommendUserId))
                {



                    bllScore.UpdateUserScoreWithWXTMNotify(bllScore.GetDefinedUserScore(BLLUserScore.UserScoreType.TutorInviteFriendRegister), new ZentCloud.BLLJIMP.BLLWeixin("").GetAccessToken());
                }
                else
                {
                    bllScore.UpdateUserScoreWithWXTMNotify(bllScore.GetDefinedUserScore(BLLUserScore.UserScoreType.InviteFriendRegister), new ZentCloud.BLLJIMP.BLLWeixin("").GetAccessToken());
                }

                //SaveCurrUser();//注册会员加积分
                //SaveRecordUser(UserId);  //转发人加积分
            }

            currentUserInfo = bllUser.GetCurrentUserInfo();
            currentUserInfo.TrueName = name;
            currentUserInfo.Phone = phone;
            currentUserInfo.Email = email;
            currentUserInfo.Company = company;
            currentUserInfo.Regtime = DateTime.Now;
            currentUserInfo.Postion = position;
            bool isSuccess = bllUser.Update(currentUserInfo);
            if (isSuccess)
            {
                resp.Status = 0;
                resp.Msg = "添加成功";


            }
            else
            {
                resp.Status = -1;
                resp.Msg = "添加失败";
            }

        outF:
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateMyInfo(HttpContext context)
        {
            string name = context.Request["Uname"];
            string phone = context.Request["UPhone"];
            string email = context.Request["UEmail"];
            string company = context.Request["UCompanyl"];

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(company) || string.IsNullOrEmpty(email))
            {
                resp.Status = 0;
                resp.Msg = "请填写完整信息";
                goto outF;
            }

            this.currentUserInfo.TrueName = name;
            this.currentUserInfo.Phone = phone;
            this.currentUserInfo.Email = email;
            this.currentUserInfo.Company = company;
            if (bllUser.Update(currentUserInfo))
            {
                resp.Status = 1;
                resp.Msg = "更新成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "更新失败";
            }

        outF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 转发人加积分
        /// </summary>
        /// <param name="UserId"></param>
        //private void SaveRecordUser(string UserId)
        //{
        //    BLLJIMP.Model.TutorInfo tInfo = juActivityBll.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserId='{0}'", UserId));
        //    BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
        //    {
        //        NameStr = "好友注册成功",
        //        Nums = "b55",
        //        InsertDate = DateTime.Now,
        //        ScoreNum = "+10",
        //        WebsiteOwner = this.websiteOwner,
        //        UserId = UserId,
        //        RecordType = "2"
        //    };
        //    if (tInfo != null)
        //    {
        //        UpdateUserToTalScore(UserId, 20);
        //        srInfo.ScoreNum = "+20";
        //    }
        //    else
        //    {
        //        UpdateUserToTalScore(UserId, 10);
        //    }
        //    bool bo = juActivityBll.Add(srInfo);
        //}

        /// <summary>
        /// 当前用户加积分
        /// </summary>
        //private void SaveCurrUser()
        //{
        //    UpdateUserToTalScore(this.userInfo.UserID, 10);
        //    BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
        //    {
        //        NameStr = "恭喜成为会员",
        //        Nums = "b55",
        //        InsertDate = DateTime.Now,
        //        ScoreNum = "+25",
        //        WebsiteOwner = this.websiteOwner,
        //        UserId = this.userInfo.UserID,
        //        RecordType = "2"

        //    };
        //    juActivityBll.Add(srInfo);
        //}

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCurrUserInfo(HttpContext context)
        {

            resp.ExObj = this.currentUserInfo;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取转发列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetForwardingRecordInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.ForwardingRecord> data;
            //string voteName = context.Request["VoteName"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}'", bllUser.WebsiteOwner));
            //if (!string.IsNullOrEmpty(voteName))
            //{
            //    sbWhere.AppendFormat(" AND VoteName lIKE '%{0}%'", voteName);
            //}
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ForwardingRecord>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.ForwardingRecord>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = data
    });
        }

        /// <summary>
        /// 保存分享记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool SavaForwardingRecord(string userId)
        {


            if (currentUserInfo != null)
            {
                BLLJIMP.Model.ForwardingRecord record = new BLLJIMP.Model.ForwardingRecord()
                {
                    RUserID = this.currentUserInfo.UserID,
                    RUserName = this.currentUserInfo.TrueName,
                    RdateTime = DateTime.Now,
                    FUserID = currentUserInfo.UserID,
                    FuserName = currentUserInfo.TrueName,
                    WebsiteOwner = bllUser.WebsiteOwner,
                    TypeName = "分享"
                };
                return bllUser.Add(record);
            }

            return false;

        }

        /// <summary>
        /// 获取用户等级配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetUserLevelList(HttpContext context)
        {
            var strWhere = string.Format("WebsiteOwner='{0}'", bllUser.WebsiteOwner);
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            List<ZentCloud.BLLJIMP.Model.UserLevelConfig> data = bllUser.GetLit<ZentCloud.BLLJIMP.Model.UserLevelConfig>(pageSize, pageIndex, strWhere, "AutoId ASC");
            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = data.Count,
                    rows = data
                });
        }

        /// <summary>
        /// 删除转发列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelFRecordInfo(HttpContext context)
        {
            string ids = context.Request["ids"];

            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.ForwardingRecord(), string.Format(" AutoId in ({0})", ids));
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
            }

        Outf:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="p"></param>
        private bool UpdateUserToTalScore(string userId, int score)
        {


            BLLJIMP.Model.UserInfo userInfo = bllUser.GetUserInfo(userId);
            userInfo.TotalScore += score;
            return bllJuactivity.Update(userInfo, string.Format(" TotalScore={0}", userInfo.TotalScore), string.Format(" AutoID={0}", userInfo.AutoID)) > 0;


        }

        /// <summary>
        /// 获取列表 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNewsList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string activityName = context.Request["ArticleName"];
            string categoryId = context.Request["CategoryId"];
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}' And ArticleType='article' And IsHide=0 And IsDelete=0", bllUser.WebsiteOwner));
            List<int> listJieDu = new List<int>();//市场解读
            //if (context.Request.Url.Host.Equals("xixinxian.comeoncloud.net"))
            //{
            //    ListJieDu.Add(169);
            //}
            //else
            //{
            //    ListJieDu.Add(240);
            //}
            listJieDu.Add(240);
            List<ZentCloud.BLLJIMP.Model.ArticleCategory> jieDuCategoryList = bllUser.GetList<ZentCloud.BLLJIMP.Model.ArticleCategory>(string.Format("PreID={0}", listJieDu[0].ToString()));
            for (int i = 0; i < jieDuCategoryList.Count; i++)
            {

                listJieDu.Add(jieDuCategoryList[i].AutoID);
            }

            if (!string.IsNullOrEmpty(activityName))
            {
                sbWhere.AppendFormat(" And ActivityName like '%{0}%'", activityName);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                ZentCloud.BLLJIMP.Model.ArticleCategory category = bllUser.Get<ZentCloud.BLLJIMP.Model.ArticleCategory>(string.Format("AutoID={0} And WebsiteOwner='{1}'", categoryId, bllUser.WebsiteOwner));
                if ((category != null) && category.PreID.Equals(0))
                {
                    List<ZentCloud.BLLJIMP.Model.ArticleCategory> subCategoryList = bllUser.GetList<ZentCloud.BLLJIMP.Model.ArticleCategory>(string.Format("PreID={0}", categoryId));
                    if (subCategoryList.Count > 0)
                    {
                        string strCategoryIds = "";
                        for (int i = 0; i < subCategoryList.Count; i++)
                        {
                            strCategoryIds += subCategoryList[i].AutoID.ToString() + ",";

                        }
                        strCategoryIds += categoryId;
                        sbWhere.AppendFormat(" And CategoryId in({0})", strCategoryIds);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" And CategoryId={0}", categoryId);

                    }
                }
                else
                {
                    sbWhere.AppendFormat(" And CategoryId={0}", categoryId);
                }

            }
            totalCount = bllUser.GetCount<ZentCloud.BLLJIMP.Model.JuActivityInfo>(sbWhere.ToString());
            List<ZentCloud.BLLJIMP.Model.JuActivityInfo> data = bllUser.GetLit<ZentCloud.BLLJIMP.Model.JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), " Sort DESC,LastUpdateDate DESC,JuActivityID DESC");

            for (int i = 0; i < data.Count; i++)
            {
                BLLJIMP.Model.TutorInfo tutorInfo = bllUser.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserID='{0}'", data[i].UserID));
                if (tutorInfo != null)
                {
                    data[i].ThumbnailsPath = tutorInfo.TutorImg ?? data[i].ThumbnailsPath;
                }
                data[i].ActivityDescription = null;
            }


            resp.ExObj = data;
            resp.ExStr = "";
            int TotalPage = bllUser.GetTotalPage(totalCount, pageSize);
            if ((TotalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }



        /// <summary>
        /// 获取关注的导师发布的文章
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetGuanZhuArticleList(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            int totalCount = 0;
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}' And ArticleType='article' And IsHide=0 And IsDelete=0", bllUser.WebsiteOwner));

            List<UserFollowChain> list = bllUser.GetList<UserFollowChain>(string.Format("FromUserId='{0}'", currentUserInfo.UserID));

            if (list.Count > 0)
            {
                string strJoin = "";
                foreach (var item in list)
                {
                    strJoin += string.Format("'{0}',", item.ToUserId);
                }
                strJoin = strJoin.TrimEnd(',');
                sbWhere.AppendFormat(" And UserID in({0})", strJoin);
            }
            else
            {
                sbWhere.AppendFormat(" And 1=0");
            }
            totalCount = bllUser.GetCount<ZentCloud.BLLJIMP.Model.JuActivityInfo>(sbWhere.ToString());
            List<ZentCloud.BLLJIMP.Model.JuActivityInfo> data = bllUser.GetLit<ZentCloud.BLLJIMP.Model.JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), " Sort DESC,JuActivityID DESC");
            for (int i = 0; i < data.Count; i++)
            {
                BLLJIMP.Model.TutorInfo tutorInfo = bllUser.Get<BLLJIMP.Model.TutorInfo>(string.Format(" UserID='{0}'", data[i].UserID));
                if (tutorInfo != null)
                {
                    data[i].ThumbnailsPath = tutorInfo.TutorImg ?? data[i].ThumbnailsPath;
                }
                data[i].ActivityDescription = null;
            }
            resp.ExObj = data;
            int TotalPage = bllUser.GetTotalPage(totalCount, pageSize);
            if ((TotalPage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExStr = "1";//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        ///// <summary>
        ///// 检查当前用户是否是导师
        ///// </summary>
        ///// <returns></returns>
        //public bool IsTutor(ZentCloud.BLLJIMP.Model.UserInfo userInfo)
        //{
        //    return bllUser.GetCount<TutorInfo>(string.Format("UserId='{0}' And websiteOwner='{1}' ", userInfo.UserID, bllUser.WebsiteOwner)) > 0 ? true : false;
        //}


        /// <summary>
        /// 是否是导师
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsTutor(string userId)
        {
            return bllUser.GetCount<TutorInfo>(string.Format("UserId='{0}' And websiteOwner='{1}' ", currentUserInfo.UserID, bllUser.WebsiteOwner)) > 0 ? true : false;
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
