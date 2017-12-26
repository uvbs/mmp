using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.IO;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model.API.step5;
using System.Text;
using AliOss;
using System.Drawing;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// CommHandler 的摘要说明
    /// </summary>
    public class CommHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 响应模型1
        /// </summary>
        DefaultResponse resp1 = new DefaultResponse();
        /// <summary>
        /// BLL基类
        /// </summary>
        BLLJIMP.BLL bll=new BLL();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 图片BLL
        /// </summary>
        BLLImage bllImage = new BLLImage();
        /// <summary>
        /// 系统通知BLL
        /// </summary>
        BLLSystemNotice bllSysNotice = new BLLSystemNotice();
        /// <summary>
        /// 订单BLL
        /// </summary>
        BllOrder bllOrder = new BllOrder();
        /// <summary>
        /// 投票BLL
        /// </summary>
        BLLVote bllVote = new BLLVote();
        BLLUploadOtherServer bllUploadOtherServer = new BLLUploadOtherServer();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo = new UserInfo();
        public void ProcessRequest(HttpContext context)
        {
            string action = context.Request["Action"];
            string result = string.Empty;
            currentUserInfo = bll.GetCurrentUserInfo();
            if (action.Equals("UploadSingelFile1"))
            {
                result = UploadSingelFile1(context);
                context.Response.Write(result);
                return;
            }

            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;

            try
            {
                switch (action)
                {
                    case "GetCodeListInfo":
                        result = GetCodeListInfo(context);
                        break;
                    case "UploadSingelFile":
                        result = UploadSingelFile(context);
                        break;
                    //case "GetLineChartData":
                    //    result = GetLineChartData(context);
                    //    break;

                    case "UploadSingelFile1":
                        result = UploadSingelFile1(context);
                        break;

                    case "UploadSingelFileLocal":
                        result = UploadSingelFileLocal(context);
                        break;


                    case "GetVoteObjectVoteList"://获取选手列表 分页
                        result = GetVoteObjectVoteList(context);
                        break;

                    case "UpdateVoteObjectVoteCount":
                        result = UpdateVoteObjectVoteCount(context);
                        break;

                    case "GetPrize":
                        result = GetPrize(context);
                        break;

                    case "SumbitOrderPayVote"://投票下单
                        result = SumbitOrderPayVote(context);
                        break;
                    case "AddVoteReviewInfo":
                        result = SaveReviewInfo(context);
                        break;
                    case "GetVoteReviewInfo":
                        result = GetReviewInfos(context);
                        break;

                    //case "GetArticleReviewInfo":
                    //    result = GetArticleReviewInfo(context);
                    //    break;
                    //case "AddArticleReviewInfo":
                    //    result = AddArticleReviewInfo(context);
                    //    break;
                    //case "ReplyArticleReviewInfo":
                    //    result = ReplyArticleReviewInfo(context);
                    //    break;
                    //case "DeleteArticleReviewInfo":
                    //    result = DeleteArticleReviewInfo(context);
                    //    break;
                    case "getarticlereviewlist":
                        result = GetArticleReviewlist(context);
                        break;
                    case "addarticlereview":
                        result = AddArticleReview(context);
                        break;
                    case "replyarticlereview":
                        result = ReplyArticleReview(context);
                        break;
                    case "deletearticlereview":
                        result = DeleteArticleReview(context);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.resp.Status = -1;
                //this.resp.Msg = ex.Message;
                if (ex.InnerException != null)
                {
                    this.resp.Msg = ex.InnerException.Message;
                }
                else
                {
                    this.resp.Msg = ex.Message;
                }

                result = ZentCloud.Common.JSONHelper.ObjectToJson(this.resp);

            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 获取评论信息 投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewInfos(HttpContext context)
        {
            
                string voteId = context.Request["voteid"];
                List<BLLJIMP.Model.ReviewInfo> data = bll.GetList<BLLJIMP.Model.ReviewInfo>(6, string.Format(" ForeignkeyId='{0}'", voteId), " InsertDate desc");

                if (data.Count > 0)
                {
                    foreach (BLLJIMP.Model.ReviewInfo item in data)
                    {
                        item.rrInfos = bll.GetList<BLLJIMP.Model.ReplyReviewInfo>(" ReviewID=" + item.AutoId);
                    }
                    resp.Status = 0;
                    resp.ExObj = data;
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "没有数据！！！";
                }

           
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 保存评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveReviewInfo(HttpContext context)
        {
            
                string voteId = context.Request["voteid"];
                string content = context.Request["Content"];
                string reviewType = context.Request["ReviewType"];
                string fName = context.Request["FName"];
                if (string.IsNullOrEmpty(content))
                {
                    resp.Status = -1;
                    resp.Msg = "输入评论内容";
                    goto OutF;
                }
                bool isSuccess = bll.Add(new BLLJIMP.Model.ReviewInfo()
                 {
                     ReviewContent = content,
                     UserId = currentUserInfo.UserID,
                     InsertDate = DateTime.Now,
                     ForeignkeyId = voteId,
                     UserName = currentUserInfo.TrueName ?? currentUserInfo.UserID,
                     ReviewType = reviewType,
                     ForeignkeyName = fName,
                 });
                if (isSuccess)
                {
                    resp.Status = 0;
                    resp.Msg = "评论成功";
                    goto OutF;
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "评论失败";
                }
           
        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }


        #region 文章评论

        /// <summary>
        /// 获取文章评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleReviewlist(HttpContext context)
        {

            string id = context.Request["articleid"];
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            ArticleReviewApi apiResult = new ArticleReviewApi();
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" ReviewID={0}", id);
            apiResult.totalcount = bll.GetCount<ReplyReviewInfo>(sbWhere.ToString());
            List<ReplyReviewInfo> data = bll.GetLit<BLLJIMP.Model.ReplyReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoId desc");
            List<ArticleReview> jsonResult = new List<ArticleReview>();
            foreach (var item in data)
            {
                ArticleReview review = new ArticleReview();
                //目标评论内容
                if (item.PraentId > 0)
                {
                    var targetReply = bll.Get<ReplyReviewInfo>(string.Format("AutoId={0}", item.PraentId)); if (targetReply != null)
                    {
                        review.reply = new ArticleReplyReview();
                        review.reply.reviewcontent = targetReply.ReplyContent;
                        review.reply.nickname = targetReply.UserName;

                    }
                }

                //目标评论内容
                var userInfo = bllUser.GetUserInfo(item.UserId);
                if (userInfo != null)
                {
                    review.headimg = userInfo.WXHeadimgurlLocal;

                }
                review.id = item.AutoId;
                review.nickname = item.UserName;
                review.time = bll.GetTimeStamp(item.InsertDate);
                review.reviewcontent = item.ReplyContent;
                if ((bll.IsLogin) && (item.UserId.Equals(currentUserInfo.UserID)))
                {
                    review.deleteflag = true;

                }


                jsonResult.Add(review);
            }
            apiResult.list = jsonResult;
            return Common.JSONHelper.ObjectToJson(apiResult);


        }

        /// <summary>
        /// 发表评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddArticleReview(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp1.errcode = (int)errcode.UnLogin;
                resp1.errmsg = "尚未登录";
                goto outoff;
            }
            if (!CheckUserIsReg(currentUserInfo))
            {
                resp1.errcode = 5;
                resp1.errmsg = "请先注册";
                goto outoff;
            }
            int id = int.Parse(context.Request["articleid"]);
            string content = context.Request["content"];
            if (string.IsNullOrEmpty(content))
            {
                resp1.errcode = 1;
                resp1.errmsg = "请输入评论内容";
                goto outoff;
            }
            if (!CheckArtickeReviewContent(content))
            {
                resp1.errcode = 2;
                resp1.errmsg = "您的评论含有敏感词,请重新编辑评论内容";
                goto outoff;
            }
            //if (bllJuactivity.GetJuActivity(id) == null)
            //{
            //    resp.errcode = 3;
            //    resp.errmsg = "文章不存在";
            //    goto outoff;
            //}
            bool isSuccess = bll.Add(new BLLJIMP.Model.ReplyReviewInfo
            {
                ReviewID = id,
                ReplyContent = content,
                UserId = currentUserInfo.UserID,
                InsertDate = DateTime.Now,
                WebSiteOwner = bll.WebsiteOwner,
                UserName = currentUserInfo.TrueName ?? "匿名用户",
                ReviewType = "文章评论",

            });
            if (isSuccess)
            {
                resp1.errcode = 0;
                resp1.errmsg = "评论成功";
                goto outoff;
            }
            else
            {
                resp1.errcode = 4;
                resp1.errmsg = "评论失败";
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp1);
        }

        /// <summary>
        /// 删除文章评论
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteArticleReview(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp1.errcode = (int)errcode.UnLogin;
                resp1.errmsg = "尚未登录";
                goto outoff;
            }
            string id = context.Request["id"];
            ReplyReviewInfo review = bll.Get<ReplyReviewInfo>(string.Format(" AutoID={0} And UserId='{1}'", id, currentUserInfo.UserID));
            if (review == null)
            {
                resp1.errcode = 1;
                resp1.errmsg = "无权删除";
                goto outoff;

            }
            if (bll.Delete(review) == 0)
            {
                resp1.errcode = 2;
                resp1.errmsg = "删除失败";
                goto outoff;
            }
            resp1.errmsg = "删除成功";
        //bll.Delete(new ReplyReviewInfo(), string.Format(" PraentId={0}", Review.AutoId));
        outoff:
            return Common.JSONHelper.ObjectToJson(resp1);
        }

        /// <summary>
        /// 发表回复评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReplyArticleReview(HttpContext context)
        {
            if (!bll.IsLogin)
            {
                resp1.errcode = (int)errcode.UnLogin;
                resp1.errmsg = "尚未登录";
                goto outoff;
            }
            if (!CheckUserIsReg(currentUserInfo))
            {
                resp1.errcode = 5;
                resp1.errmsg = "请先注册";
                goto outoff;
            }
            int articleId = int.Parse(context.Request["articleid"]);
            int reviewId = int.Parse(context.Request["id"]);
            string content = context.Request["replycontent"];

            if (string.IsNullOrEmpty(content))
            {
                resp1.errcode = 1;
                resp1.errmsg = "输入评论内容";
                goto outoff;
            }
            if (!CheckArtickeReviewContent(content))
            {
                resp1.errcode = 2;
                resp1.errmsg = "您的评论包含不适合发布的内容,请重新编辑";
                goto outoff;
            }
            bool isSuccess = bll.Add(new BLLJIMP.Model.ReplyReviewInfo
            {
                ReplyContent = content,
                UserId = currentUserInfo.UserID,
                InsertDate = DateTime.Now,
                WebSiteOwner = bll.WebsiteOwner,
                UserName = currentUserInfo.TrueName ?? "匿名用户",
                ReviewType = "文章评论",
                PraentId = reviewId,
                ReviewID = articleId

            });
            if (isSuccess)
            {
                resp1.errcode = 0;
                resp1.errmsg = "回复成功";
                #region 消息提示
                ReplyReviewInfo review = bll.Get<ReplyReviewInfo>(string.Format(" AutoID={0}", reviewId));
                bllSysNotice.SendSystemMessage("您的评论有新的回复", "您的评论有新的回复,点击查看", 1, 2, review.UserId, string.Format("http://{0}/WuBuHui/News/NewsDetail.aspx?id={1}", context.Request.Url.Host, articleId), bll.WebsiteOwner); 
                #endregion
                goto outoff;
            }
            else
            {
                resp1.errcode = 3;
                resp1.errmsg = "回复失败";
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp1);
        }




        /// <summary>
        /// 检查文章评论是否通过
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool CheckArtickeReviewContent(string content)
        {
            bool isSuccess = true;
            foreach (var item in bll.GetList<FilterWord>(string.Format(" WebsiteOwner='{0}' And FilterType=0", bll.WebsiteOwner)))
            {
                if (content.Trim().ToLower().Contains(item.Word))
                {
                    isSuccess = false;
                }
            }

            return isSuccess;

        }

        /// <summary>
        /// 检查用户是否已经注册
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private bool CheckUserIsReg(UserInfo userInfo) {

            if ((!string.IsNullOrEmpty(userInfo.TrueName)) && (!string.IsNullOrEmpty(userInfo.Phone)) && (!string.IsNullOrEmpty(userInfo.Company)) && (!string.IsNullOrEmpty(userInfo.Email)))
            {


                return true;


            }
            return false;
        }


        #endregion


        /// <summary>
        /// 领取奖品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPrize(HttpContext context)
        {
            string id = context.Request["LotteryId"];
            WXLotteryRecord wxlRecord = bll.Get<WXLotteryRecord>(" UserId='" + currentUserInfo.UserID + "' And LotteryId=" + id);
            if (wxlRecord != null)
            {
                wxlRecord.IsGetPrize = "1";
                if (bll.Update(wxlRecord))
                {
                    resp.Status = 1;
                    resp.Msg = "领奖成功！！！";
                }
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        ///// <summary>
        ///// 获取折线图数据
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string GetLineChartData(HttpContext context)
        //{
        //    string chartItemStr = context.Request["ChartItem"];

        //    int weekChangeValue = Convert.ToInt32(context.Request["weekChangeValue"]);

        //    List<DateTime> arrDate = new List<DateTime>();

        //    DateTime monday = ZentCloud.Common.DateTimeHelper.CalculateFirstDateOfWeek(DateTime.Now);
        //    // DateTime monday = DateTime.Now;
        //    //测试需要暂时调数据为上周
        //    monday = monday.AddDays(weekChangeValue);

        //    arrDate.Add(monday);
        //    arrDate.Add(monday.AddDays(1));
        //    arrDate.Add(monday.AddDays(2));
        //    arrDate.Add(monday.AddDays(3));
        //    arrDate.Add(monday.AddDays(4));
        //    arrDate.Add(monday.AddDays(5));
        //    arrDate.Add(monday.AddDays(6));

        //    List<List<object>> result = new List<List<object>>();

        //    List<object> arrTitle = new List<object>() { 
        //        "date"
        //        //,
        //        //"新增用户",
        //        //"新发文章",
        //        //"新发活动",
        //        //"分享数",
        //        //"报名人数"
        //        //,
        //        //"PC主页访问(IP)",
        //        //"PC主页访问(UV)",
        //        //"PC主页访问(PV)",
        //        //"手机主页访问(IP)",
        //        //"手机主页访问(UV)",
        //        //"手机主页访问(PV)",
        //        //"文章活动访问(IP)",
        //        //"文章活动访问(UV)",
        //        //"文章活动访问(PV)"
        //    };//图表显示列

        //    if (string.IsNullOrWhiteSpace(chartItemStr))
        //    {
        //        arrTitle.AddRange(new List<object>() {
        //            "新增用户",
        //            "新发文章",
        //            "新发活动"
        //        });
        //    }
        //    else
        //    {
        //        arrTitle.AddRange(
        //                chartItemStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //            );
        //    }

        //    result.Add(arrTitle);//添加标题行
        //    Random r = new Random();
        //    foreach (var d in arrDate)
        //    {
        //        List<object> valueList = new List<object>();

        //        foreach (var t in arrTitle)
        //        {
        //            if (t.Equals("date"))
        //            {
        //                valueList.Add(string.Format("{0}月{1}号", d.Month.ToString(), d.Day.ToString()));//添加时间轴时间
        //            }
        //            else if (t.Equals("新增用户"))
        //            {
        //                valueList.Add(this.bll.GetNewUserCountByDate(d));
        //            }
        //            else if (t.Equals("新发文章"))
        //            {
        //                valueList.Add(this.bll.GetNewArticleCountByDate(d));
        //            }
        //            else if (t.Equals("新发活动"))
        //            {
        //                valueList.Add(this.bll.GetNewActivityCountByDate(d));
        //            }
        //            else if (t.Equals("分享数"))
        //            {
        //                valueList.Add(this.bll.GetShareCountByDate(d));
        //            }
        //            else if (t.Equals("报名人数"))
        //            {
        //                valueList.Add(this.bll.GetSignUpCountByDate(d));
        //            }
        //            else if (t.Equals("PC主页访问(IP)"))
        //            {
        //                valueList.Add(this.bll.GetPCIndexViewCountByDate(BLLJIMP.StatisticsType.IP, d));
        //            }
        //            else if (t.Equals("PC主页访问(UV)"))
        //            {
        //                valueList.Add(this.bll.GetPCIndexViewCountByDate(BLLJIMP.StatisticsType.UV, d));
        //            }
        //            else if (t.Equals("PC主页访问(PV)"))
        //            {
        //                valueList.Add(this.bll.GetPCIndexViewCountByDate(BLLJIMP.StatisticsType.PV, d));
        //            }
        //            else if (t.Equals("手机主页访问(IP)"))
        //            {
        //                valueList.Add(this.bll.GetMobileIndexViewCountByDate(BLLJIMP.StatisticsType.IP, d));
        //            }
        //            else if (t.Equals("手机主页访问(UV)"))
        //            {
        //                valueList.Add(this.bll.GetMobileIndexViewCountByDate(BLLJIMP.StatisticsType.UV, d));
        //            }
        //            else if (t.Equals("手机主页访问(PV)"))
        //            {
        //                valueList.Add(this.bll.GetMobileIndexViewCountByDate(BLLJIMP.StatisticsType.PV, d));
        //            }
        //            else if (t.Equals("文章活动访问(IP)"))
        //            {
        //                valueList.Add(this.bll.GetArticleViewCountByDate(BLLJIMP.StatisticsType.IP, d));
        //            }
        //            else if (t.Equals("文章活动访问(UV)"))
        //            {
        //                valueList.Add(this.bll.GetArticleViewCountByDate(BLLJIMP.StatisticsType.UV, d));
        //            }
        //            else if (t.Equals("文章活动访问(PV)"))
        //            {
        //                valueList.Add(this.bll.GetArticleViewCountByDate(BLLJIMP.StatisticsType.PV, d));
        //            }
        //            else
        //            {

        //            }


        //        }//添加查询值

        //        result.Add(valueList);//添加数据行
        //    }

        //    resp.Status = 1;
        //    resp.ExObj = result;

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UploadSingelFile(HttpContext context)
        {

            if (context.Request.Files.Count==0)
            {
                resp.Status = 0;
                resp.Msg = "请选择文件!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            HttpPostedFile file = context.Request.Files[0];
            string isNotNewFileName=context.Request["isnotnewfilename"];
            string newFileExtension = Path.GetExtension(file.FileName).ToLower();
            string newFileName = "";
            if (!string.IsNullOrEmpty(isNotNewFileName)){
                newFileName = file.FileName;
            }
            else{
                newFileName = Guid.NewGuid().ToString("N").ToUpper() + newFileExtension;
            }

            if (!newFileName.EndsWith(".jpg") && !newFileName.EndsWith(".png") && !newFileName.EndsWith(".gif"))
            {
                resp.Status = 0;
                resp.Msg = "请上传JPG、PNG和GIF格式图片!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (currentUserInfo == null)
            {
                resp.Status = 0;
                resp.Msg = "账户异常!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (file != null)
            {
                try
                {
                    int maxWidth = 800;
                    int maxHeight = 0;
                    if (!string.IsNullOrWhiteSpace(context.Request["maxWidth"])) maxWidth = Convert.ToInt32(context.Request["maxWidth"]);
                    if (!string.IsNullOrWhiteSpace(context.Request["maxHeight"])) maxHeight = Convert.ToInt32(context.Request["maxHeight"]);
                    if (maxWidth > 0 || maxHeight>0)
                    {
                        Image image = Image.FromStream(file.InputStream);
                        ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
                        if ((image.Width > maxWidth && maxWidth > 0) || (image.Height > maxHeight && maxHeight > 0))
                        {
                            double ratio = imgHelper.GetRatio(image.Width, image.Height, maxWidth, maxHeight);
                            int newWidth = Convert.ToInt32(Math.Round(ratio * image.Width));
                            int newHeight = Convert.ToInt32(Math.Round(ratio * image.Height));
                            image = imgHelper.PhotoSizeChange(image, newWidth, newHeight);
                        }
                        if (ZentCloud.Common.ConfigHelper.GetConfigInt("Oss_Enable") == 0)
                        {
                            String savePath = "/FileUpload/image/" + currentUserInfo.WebsiteOwner + "/" + currentUserInfo.UserID + "/" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "/";
                            String serverPath = context.Server.MapPath(savePath);
                            if (!Directory.Exists(serverPath))
                            {
                                Directory.CreateDirectory(serverPath);
                            }
                            String serverFile = serverPath + newFileName;
                            String saveFile = savePath + newFileName;
                            image.Save(serverFile, imgHelper.GetImageFormat(newFileExtension));
                            resp.ExStr = saveFile;
                        }
                        else
                        {
                            Stream stream = new MemoryStream();
                            image.Save(stream, imgHelper.GetImageFormat(newFileExtension));
                            stream.Position = 0;
                            resp.ExStr = bllUploadOtherServer.upload(stream, newFileExtension);
                                //OssHelper.UploadFileFromStream(newFileName, OssHelper.GetBucket(currentUserInfo.WebsiteOwner), OssHelper.GetBaseDir(currentUserInfo.WebsiteOwner), currentUserInfo.UserID, "image", stream);
                        }
                        resp.ExObj = new
                        {
                            width = image.Width,
                            height = image.Height
                        };
                    }
                    else
                    {
                        resp.ExStr = bllUploadOtherServer.uploadFromHttpPostedFile(file);
                        //if (string.IsNullOrEmpty(isNotNewFileName))
                        //{
                        //    resp.ExStr = OssHelper.UploadFile(OssHelper.GetBucket(currentUserInfo.WebsiteOwner), OssHelper.GetBaseDir(currentUserInfo.WebsiteOwner), currentUserInfo.UserID, "image", file);

                        //}
                        //else
                        //{
                        //    resp.ExStr = OssHelper.UploadFile(OssHelper.GetBucket(currentUserInfo.WebsiteOwner), OssHelper.GetBaseDir(currentUserInfo.WebsiteOwner), currentUserInfo.UserID, "image", file, false);

                        //}

                        Image image = Image.FromStream(file.InputStream);
                        resp.ExObj = new
                        {
                            width = image.Width,
                            height = image.Height
                        };
                    }
                    resp.Status = 1;
                }
                catch (Exception ex)
                {
                    resp.Status = 0;
                    resp.Msg = ex.Message;
                }
            } 
            else
            {
                resp.Status = 0;
                resp.Msg = "找不到文件!";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UploadSingelFile1(HttpContext context)
        {

            if (context.Request.Files.Count == 0)
            {
                resp.Status = 0;
                resp.Msg = "请选择文件!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            HttpPostedFile file = context.Request.Files[0];
            string fd = context.Request["fd"];//存储文件夹

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string newFileName = Guid.NewGuid().ToString("N").ToUpper() + Path.GetExtension(file.FileName).ToLower();
            if (fd.ToLower().Equals("music"))
            {
                if (!newFileName.EndsWith(".mp3"))
                {
                    resp.Status = 0;
                    resp.Msg = "请上传mp3格式的音乐";
                    return Common.JSONHelper.ObjectToJson(resp);
                }
            }
            else if(string.IsNullOrWhiteSpace(fd))
            {
                fd = "image";
            }
            if (!newFileName.EndsWith(".jpg") && !newFileName.EndsWith(".png") && !newFileName.EndsWith(".gif") && !newFileName.EndsWith(".mp3") && !newFileName.EndsWith(".jpeg"))
            {
                resp.Status = 0;
                resp.Msg = "请上传JPG、PNG和GIF格式图片!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (file != null)
            {
                try
                {
                    WebsiteInfo webSite = bllUser.GetWebsiteInfoModel();
                    if (ZentCloud.Common.ConfigHelper.GetConfigInt("Oss_Enable") == 0)
                    {
                        //文件保存目录路径
                        String savePath = "/FileUpload/" + fd + "/" + currentUserInfo.WebsiteOwner + "/" + currentUserInfo.UserID + "/" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "/";
                        String serverPath = context.Server.MapPath(savePath);
                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }
                        String serverFile = serverPath + newFileName;
                        String saveFile = savePath + newFileName;
                        if (!fileExtension.Equals(".mp3"))
                        {
                            ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
                            Image image = Image.FromStream(file.InputStream);
                            image.Save(serverFile, imgHelper.GetImageFormat(fileExtension));
                            resp.ExStr = saveFile;
                            resp.ExObj = new
                            {
                                width = image.Width,
                                height = image.Height
                            };
                        }
                        else
                        {
                            file.SaveAs(serverFile);
                            resp.ExStr = saveFile;
                        }
                        resp.Status = 1;
                    }
                    else
                    {
                        resp.ExStr = bllUploadOtherServer.uploadFromHttpPostedFile(file);
                            //OssHelper.UploadFile(OssHelper.GetBucket(currentUserInfo.WebsiteOwner), OssHelper.GetBaseDir(currentUserInfo.WebsiteOwner), currentUserInfo.UserID, "image", file);

                        if (!fileExtension.Equals(".mp3"))
                        {
                            //获取图片宽高
                            try
                            {
                                Image image = Image.FromStream(file.InputStream);
                                resp.ExObj = new
                                {
                                    width = image.Width,
                                    height = image.Height
                                };
                            }
                            catch (Exception)
                            {
                            }
                        }
                        resp.Status = 1;
                    }
                }
                catch (Exception ex)
                {
                    resp.Status = 0;
                    resp.Msg = ex.Message;
                }
                //file.SaveAs(context.Server.MapPath("~/FileUpload/") + fd + "/" + newFileName);
                //resp.Status = 1;
                //resp.ExStr = "/FileUpload/" + fd + "/" + newFileName;
                //if (!newFileName.EndsWith(".mp3"))
                //{
                //    resp.ExStr = bllImage.CreateThumbImage(resp.ExStr,750,750);
                //}
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "找不到文件!";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 上传文件 本地 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UploadSingelFileLocal(HttpContext context)
        {

            HttpPostedFile file = context.Request.Files[0];

            string fd = context.Request["fd"];//存储文件夹

            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToLower();
            if (fd.ToLower().Equals("music"))
            {
                if (!newFileName.EndsWith(".mp3"))
                {
                    resp.Status = 0;
                    resp.Msg = "请上传mp3格式的音乐";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }
            if (!newFileName.EndsWith(".jpg") && !newFileName.EndsWith(".png") && !newFileName.EndsWith(".gif") && !newFileName.EndsWith(".mp3") && !newFileName.EndsWith(".jpeg"))
            {
                resp.Status = 0;
                resp.Msg = "请上传JPG、PNG和GIF格式图片!";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (file != null)
            {
                //try
                //{
                //    WebsiteInfo webSite = bllUser.GetWebsiteInfoModel();
                //    resp.ExStr = OssHelper.UploadFile(OssHelper.GetBucket(webSite.WebsiteOwner), OssHelper.GetBaseDir(webSite.WebsiteOwner), currentUserInfo.UserID, "image", file);
                //    resp.Status = 1;
                //}
                //catch (Exception ex)
                //{
                //    resp.Status = 0;
                //    resp.Msg = ex.Message;
                //}
                file.SaveAs(context.Server.MapPath("~/FileUpload/") + fd + "/" + newFileName);
                resp.Status = 1;
                resp.ExStr = "/FileUpload/" + fd + "/" + newFileName;
                if (!newFileName.EndsWith(".mp3"))
                {
                    resp.ExStr = bllImage.CreateThumbImage(resp.ExStr, 750, 750);

                    //获取图片宽高
                    try
                    {
                        Image image = Image.FromFile(context.Server.MapPath(resp.ExStr));
                        resp.ExObj = new
                        {
                            width = image.Width,
                            height = image.Height
                        };
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "找不到文件!";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCodeListInfo(HttpContext context)
        {
            string codeType = context.Request["CodeType"];
            string codeValue = context.Request["CodeValue"];

            BLLJIMP.Model.CodeListInfo model = this.bll.Get<BLLJIMP.Model.CodeListInfo>(string.Format(" CodeType = '{0}' AND CodeValue = '{1}' ", codeType, codeValue));

            if (model != null)
            {
                resp.Status = 1;
                resp.ExObj = model;
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "未找到指定数据!";
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
            int voteID = int.Parse(context.Request["voteid"]);
            int voteObjectID = int.Parse(context.Request["voteobjectid"]);
            int count = int.Parse(context.Request["count"]);

            if (count <= 0)
            {
                resp.Status = 0;
                resp.Msg = "票数须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            VoteInfo voteInfo = bllVote.GetVoteInfo(voteID);
            if (voteInfo.VoteStatus.Equals(0))
            {
                resp.Status = 0;
                resp.Msg = "投票已结束";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now >= (Convert.ToDateTime(voteInfo.StopDate)))
                {
                    resp.Status = 0;
                    resp.Msg = "投票已结束";
                    return Common.JSONHelper.ObjectToJson(resp);

                }
            }



            //检查是否可以投票
            if (!bllVote.IsCanVote(voteID, currentUserInfo.UserID, count))
            {
                resp.Status = 0;
                resp.Msg = "您的可用票数不足";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            //
            if (bllVote.UpdateVoteObjectVoteCount(voteID, voteObjectID, currentUserInfo.UserID, count))
            {
                resp.Status = 1;
                //resp.Msg = "投票成功";
                resp.ExInt = bllVote.GetVoteObjectInfo(voteObjectID).VoteCount;
                resp.ExStr = bllVote.GetCanUseVoteCount(voteID, currentUserInfo.UserID).ToString();
            }
            else
            {
                resp.Status = 0;
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取选手列表 分页 搜索
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetVoteObjectVoteList(HttpContext context)
        {
            int voteId = int.Parse(context.Request["voteid"]);
            int pageIndex = int.Parse(context.Request["pageindex"]);
            int pageSize = int.Parse(context.Request["pagesize"]);
            string voteObjectName = context.Request["name"];
            string voteObjectNumber = context.Request["number"];
            string sort=context.Request["Sort"];
            int totalcount = 0;//总数量
            resp.ExObj = bllVote.GetVoteObjectInfoList(voteId, pageIndex, pageSize, out totalcount, voteObjectName, voteObjectNumber,"",sort);
            int totalpage = bll.GetTotalPage(totalcount, pageSize);
            if ((totalpage > pageIndex) && (pageIndex.Equals(1)))
            {
                resp.ExInt = 1;//是否增加下一页按钮
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 投票下单
        /// </summary>
        /// <returns></returns>
        private string SumbitOrderPayVote(HttpContext context)
        {
           
            string voteId = context.Request["VoteID"];//投票ID
            string voteRechargeId = context.Request["VoteRechargeId"];//充值ID
            if (string.IsNullOrEmpty(voteId))
            {
                resp.Status = 0;
                resp.Msg = "投票不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (new BLLVote().GetVoteInfo(int.Parse(voteId)) == null)
            {
                resp.Status = 0;
                resp.Msg = "投票不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrEmpty(voteRechargeId))
            {
                resp.Status = 0;
                resp.Msg = "充值不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            VoteRecharge voteRecharge = bll.Get<VoteRecharge>(string.Format("AutoID={0}", voteRechargeId));
            if (voteRecharge == null)
            {
                resp.Status = 0;
                resp.Msg = "充值不存在";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (voteRecharge.Amount == null)
            {
                resp.Status = 0;
                resp.Msg = "金额不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (voteRecharge.Amount <= 0)
            {
                resp.Status = 0;
                resp.Msg = "金额须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (voteRecharge.RechargeCount == null)
            {
                resp.Status = 0;
                resp.Msg = "票数不正确";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (voteRecharge.RechargeCount <= 0)
            {
                resp.Status = 0;
                resp.Msg = "票数须大于0";
                return Common.JSONHelper.ObjectToJson(resp);

            }

            OrderPay model = new OrderPay();
            model.InsertDate = DateTime.Now;
            model.OrderId = this.bll.GetGUID(TransacType.AddWXMallOrderInfo);
            model.Status = 0;
            model.Subject = "投票购票";
            model.Total_Fee = voteRecharge.Amount;
            model.Type = "1";
            model.UserId = currentUserInfo.UserID;
            model.WebsiteOwner = bllOrder.WebsiteOwner;
            model.Ex1 = voteRecharge.RechargeCount.ToString();
            model.Ex2 = voteId;
            if (bllOrder.Add(model))
            {
                resp.Status = 1;
                resp.ExStr = model.OrderId;
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "下单失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 错误码
        /// </summary>
        private enum errcode
        {
            /// <summary>
            /// 未登录
            /// </summary>
            UnLogin = -2

        }
        public class DefaultResponse
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}