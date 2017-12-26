using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Text;
using System.Reflection;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXTheVoteInfoHandler 的摘要说明
    /// </summary>
    public class WXTheVoteInfoHandler : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuActivity = new BLLJuActivity();  //活动数据
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();  //用户数据
        /// <summary>
        /// BLL基类
        /// </summary>
        BLL bll = new BLL();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo; //当前登陆的用户
        /// <summary>
        /// 日志模块
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";

            try
            {

                if (bllUser.IsLogin)
                {
                    this.currentUserInfo = bll.GetCurrentUserInfo();
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
        /// 保存投票信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveDataInfo(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            string ids = context.Request["Ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请选择投票内容";
                goto OutF;
            }
            ids = ids.Replace("0,", "");
            BLLJIMP.Model.TheVoteInfo voteInfo = bllUser.Get<BLLJIMP.Model.TheVoteInfo>(string.Format(" AutoId=" + autoId));
            if (voteInfo.MaxSelectItemCount > 0)
            {
                if (ids.Split(',').Length > voteInfo.MaxSelectItemCount)
                {
                    resp.Status = -1;
                    resp.Msg = "最多可以同时选中" + voteInfo.MaxSelectItemCount + "个选项";
                    goto OutF;
                }
            }
            if (DateTime.Now > voteInfo.TheVoteOverDate)
            {
                resp.Status = -1;
                resp.Msg = "投票结束";
                goto OutF;
            }


            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            int count = 0;
            if (currentUserInfo != null)
            {
                count = bllJuActivity.GetCount<BLLJIMP.Model.UserVoteInfo>(string.Format(" UserId='{0}' AND VoteId='{1}'", currentUserInfo.UserID, autoId));
            }
            else
            {
                //currentUserInfo = new BLLJIMP.Model.UserInfo()
                //{

                //    UserID = "user"
                //};
            }
            if (count > 0)
            {
                resp.Status = -1;
                resp.Msg = "您已经投过票，不可再次投票。";
                goto OutF;
            }
            bool isSuccess = bllUser.Add(new BLLJIMP.Model.UserVoteInfo()
             {
                 UserId = currentUserInfo.UserID,
                 VoteId = autoId,
                 DiInfoId = ids
             }, tran);
            string[] idsArry = ids.Split(',');
            voteInfo.PNumber = voteInfo.PNumber + 1;
            //int num = idsArry.Length;
            //if (idsArry.Length > 1)
            //{
            //    num = idsArry.Length - 1;
            //}
            //else
            //{
            //    num = idsArry.Length;
            //}
           
            //voteInfo.VoteNumbers = (voteInfo.VoteNumbers + num);
            bll.Update(voteInfo, string.Format(" PNumber+=1,VoteNumbers+={0}", idsArry.Length), string.Format(" AutoId={0}", voteInfo.AutoId));


            //bllUser.Update(voteInfo, tran);
            for (int i = 0; i < idsArry.Length; i++)
            {
                BLLJIMP.Model.DictionaryInfo diInfo = bllUser.Get<BLLJIMP.Model.DictionaryInfo>(" AutoID=" + idsArry[i]);
                if (diInfo != null)
                {
                    diInfo.VoteNums = diInfo.VoteNums + 1;
                    bll.Update(diInfo, string.Format("VoteNums+=1"), string.Format(" AutoID={0}", diInfo.AutoID));
                    //bllUser.Update(diInfo, tran);
                }
            }
            if (isSuccess)
            {
                resp.Status = 0;
                resp.Msg = "投票成功";
                tran.Commit();
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "投票失败";
                tran.Rollback();
            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取投票人信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetVoteTheUserInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.UserInfo> data;
            string voteId = context.Request["VoteId"];
            string itemId = context.Request["ItemId"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(voteId))
            {
                sbWhere.AppendFormat(" VoteId='{0}' ", voteId);
            }
            if (!string.IsNullOrEmpty(itemId))
            {
                sbWhere.AppendFormat(" And DiInfoId like'%{0}%' ", itemId);
            }
            List<BLLJIMP.Model.UserVoteInfo> uVInfo = this.bllJuActivity.GetList<BLLJIMP.Model.UserVoteInfo>(sbWhere.ToString());
            string userId = "'0'";
            foreach (BLLJIMP.Model.UserVoteInfo item in uVInfo)
            {
                userId += ",'" + item.UserId + "'";
            }
            string webUserId = string.Format("  UserID in ({0})", userId);
            totalCount = this.bllJuActivity.GetCount<BLLJIMP.Model.UserInfo>(webUserId);
            data = this.bllJuActivity.GetLit<BLLJIMP.Model.UserInfo>(pageSize, pageIndex, webUserId);

            foreach (var user in data)
            {
                user.Ex1 = "";
                var item = uVInfo.FirstOrDefault(p => p.UserId == user.UserID);
                if (item != null)
                {

                    foreach (var voteItemId in item.DiInfoId.Split(','))
                    {
                        DictionaryInfo dic = bll.Get<DictionaryInfo>(string.Format(" ForeignKey={0} And AutoId={1}", voteId, voteItemId));
                        if (dic != null)
                        {
                            user.Ex1+= dic.ValueStr+" ";
                        }

                    }
    


                }




            }


            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = data
    });
        }

        /// <summary>
        /// 获取投票信息
        /// </summary>
        /// <param name="context"></param>
        private string GetTheVoteInfo(HttpContext context)
        {
            string id = context.Request["Autoid"];
            if (string.IsNullOrEmpty(id))
            {
                resp.Status = -1;
                resp.Msg = "系统出错，请联系管理员";
                goto OutF;
            }
            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}'", bll.WebsiteOwner));
            sbWhere.AppendFormat(" AND AutoId ={0}", id);
            BLLJIMP.Model.TheVoteInfo tvInfo = bllJuActivity.Get<BLLJIMP.Model.TheVoteInfo>(sbWhere.ToString());
            tvInfo.diInfos = bllJuActivity.GetList<BLLJIMP.Model.DictionaryInfo>(" ForeignKey=" + tvInfo.AutoId);
            if (tvInfo != null)
            {
                resp.Status = 0;
                resp.ExObj = tvInfo;
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "系统出错，请联系管理员！！！";
            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 插入或修改数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string InsertTheVoteInfo(HttpContext context)
        {
            string autoId = string.IsNullOrEmpty(context.Request["AutoId"]) ? "0" : context.Request["AutoId"];
            string voteName = context.Request["VoteName"];
            string isVoteOpen = context.Request["IsVoteOpen"];
            string votePosition = context.Request["VotePosition"];
            string answer = context.Request["Answer"];
            string aotoId = context.Request["Aid"];
            string voteOverDate = context.Request["VoteOverDate"];
            string maxSelectItemCount = context.Request["MaxSelectItemCount"];
            string summary = context.Request["Summary"];
            string thumbnailsPath = context.Request["ThumbnailsPath"];
            if (string.IsNullOrEmpty(maxSelectItemCount))
            {
                maxSelectItemCount = "0";
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            if (string.IsNullOrEmpty(voteName))
            {
                resp.Status = -1;
                resp.Msg = "请填写完整信息";
                goto OutF;
            }

            BLLJIMP.Model.TheVoteInfo voteInfo = bllJuActivity.Get<BLLJIMP.Model.TheVoteInfo>(" AutoId=" + autoId);
            if (voteInfo != null)
            {
                List<BLLJIMP.Model.DictionaryInfo> dics = new List<BLLJIMP.Model.DictionaryInfo>();
                voteInfo.VoteName = voteName;
                voteInfo.VoteSelect = votePosition;
                voteInfo.UserId = currentUserInfo.UserID;
                voteInfo.websiteOwner = bll.WebsiteOwner;
                voteInfo.MaxSelectItemCount = int.Parse(maxSelectItemCount);
                voteInfo.TheVoteOverDate = Convert.ToDateTime(voteOverDate);
                voteInfo.Summary = summary;
                voteInfo.ThumbnailsPath = thumbnailsPath;
                if (bllJuActivity.Update(voteInfo, tran))
                {
                    string[] answersArry = answer.Split(',');
                    string[] aotoidArry = aotoId.Split(',');
                    if (answersArry.Length > 0)
                    {
                        for (int i = 0; i < answersArry.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(answersArry[i]))
                            {

                                if (string.IsNullOrEmpty(aotoidArry[i]))
                                {
                                    bllJuActivity.Add(new BLLJIMP.Model.DictionaryInfo
                                    {
                                        KeyStr = Guid.NewGuid().ToString(),
                                        ValueStr = answersArry[i],
                                        ForeignKey = voteInfo.AutoId.ToString()
                                    }, tran);
                                }
                                else
                                {

                                    BLLJIMP.Model.DictionaryInfo model = bll.Get<DictionaryInfo>(string.Format("AutoID={0}", aotoidArry[i]));
                                    if (model!=null)
                                    {
                                        model.ValueStr = answersArry[i];
                                        if (bll.Update(model))
                                        {




                                        }
                                    }


                                    //bllJuActivity.Update(new BLLJIMP.Model.DictionaryInfo
                                    //{
                                    //    AutoID = Convert.ToInt32(aotoidArry[i]),
                                    //    KeyStr = Guid.NewGuid().ToString(),
                                    //    ValueStr = answersArry[i],
                                    //    ForeignKey = voteInfo.AutoId.ToString()
                                    //}, tran);

                                }

                            }
                        }
                    }

                    resp.Status = 1;
                    resp.Msg = "修改成功！！！";
                    tran.Commit();
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "编辑选题投票[id=" + autoId + "]");
                }
                else
                {

                    resp.Status = 0;
                    resp.Msg = "修改失败！！！";
                    tran.Rollback();
                }
            }
            else
            {
                voteInfo = new BLLJIMP.Model.TheVoteInfo
                {
                    VoteName = voteName,
                    IsVoteOpen = isVoteOpen,
                    VoteSelect = votePosition,
                    TheVoteOverDate = Convert.ToDateTime(voteOverDate),
                    InsetDate = DateTime.Now,
                    websiteOwner = bll.WebsiteOwner,
                    UserId = currentUserInfo.UserID,
                    TheVoteGUID = this.bllJuActivity.GetGUID(BLLJIMP.TransacType.AddVoteId),
                    MaxSelectItemCount = int.Parse(maxSelectItemCount),
                    Summary = summary,
                    ThumbnailsPath = thumbnailsPath
                };
                if (bllJuActivity.Add(voteInfo, tran))
                {
                    tran.Commit();
                    voteInfo = bllJuActivity.Get<BLLJIMP.Model.TheVoteInfo>(string.Format(" VoteName='{0}' and UserId='{1}'", voteName, currentUserInfo.UserID
                        ));
                    string[] answers = answer.Split(',');
                    if (answers.Length > 0)
                    {
                        for (int i = 0; i < answers.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(answers[i]))
                            {
                                bllJuActivity.Add(new BLLJIMP.Model.DictionaryInfo
                                {
                                    KeyStr = Guid.NewGuid().ToString(),
                                    ValueStr = answers[i],
                                    ForeignKey = voteInfo.AutoId.ToString()
                                });
                            }
                        }
                    }
                    resp.Status = 1;
                    resp.Msg = "添加成功！！！";
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "添加选题投票");
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "添加失败！！！";
                    tran.Rollback();
                }

            }

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取所用投票
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTheVoteInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.TheVoteInfo> data;
            string voteName = context.Request["VoteName"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(voteName))
            {
                sbWhere.AppendFormat(" AND VoteName lIKE '%{0}%'", voteName);
            }
            totalCount = this.bllJuActivity.GetCount<BLLJIMP.Model.TheVoteInfo>(sbWhere.ToString());
            data = this.bllJuActivity.GetLit<BLLJIMP.Model.TheVoteInfo>(pageSize, pageIndex, sbWhere.ToString()," AutoID DESC");

            return Common.JSONHelper.ObjectToJson(
            new
            {
                total = totalCount,
                rows = data
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteTheVoteInfo(HttpContext context)
        {
            string ids = context.Request["ids"];
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    resp.Status = -1;
                    resp.Msg = "请至少选择一条！！！";
                    goto Outf;
                }
                int count = bllJuActivity.Delete(new BLLJIMP.Model.TheVoteInfo(), string.Format(" AutoId in ({0})", ids), tran);
                bllJuActivity.Delete(new BLLJIMP.Model.DictionaryInfo(), string.Format(" ForeignKey in ({0})", ids), tran);
                if (count > 0)
                {
                    resp.Status = 0;
                    resp.Msg = "删除成功。";
                    tran.Commit();
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除选题投票[id=" + ids + "]");
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "删除失败。";
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Status = -1;
                resp.Msg = ex.Message;
            }
        Outf:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取结果图信息
        /// </summary>
        /// <param name="contex"></param>
        /// <returns></returns>
        private string GetDiInfos(HttpContext contex)
        {
            string autoId = contex.Request["autoId"];
            ChartDataModel chartData = new ChartDataModel();
            ChartDataModelSub chartDataSub = new ChartDataModelSub();
            chartData.categories = new List<string>();
            chartData.series = new List<ChartDataModelSub>();
            BLLJIMP.Model.TheVoteInfo voteInfo = bllJuActivity.Get<BLLJIMP.Model.TheVoteInfo>(string.Format(" AutoId={0}", autoId));
            if (voteInfo == null)
            {
                resp.Status = -1;
                resp.Msg = "系统出错！！！";
                goto OutF;
            }
            chartDataSub.name = voteInfo.VoteName;
            chartDataSub.data = new List<int>();
            List<BLLJIMP.Model.DictionaryInfo> diInfo = bllJuActivity.GetList<BLLJIMP.Model.DictionaryInfo>(string.Format(" ForeignKey={0}", autoId));
            foreach (BLLJIMP.Model.DictionaryInfo item in diInfo)
            {
                chartData.categories.Add(item.ValueStr);
                chartDataSub.data.Add(item.VoteNums);
            }
            chartData.series.Add(chartDataSub);
            resp.Status = 0;
            resp.ExObj = chartData;


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 设置标签
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SetTheVoteUserTag(HttpContext context)
        {
            string autoIds = context.Request["autoIds"];
            string tags = context.Request["TagName"];
            if (bll.Update(currentUserInfo, string.Format("TagName='{0}'", tags), string.Format(" AutoID in({0})", autoIds)) > 0)
            {
                resp.Status = 1;
            }
            else
            {
                resp.Msg = "操作失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 清空投票数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ClearTheVoteInfoData(HttpContext context)
        {
            string autoIds = context.Request["ids"];

            if (bll.Delete(new UserVoteInfo(), string.Format(" VoteId in({0})", autoIds)) > 0)
            {

                if (bll.Update(new TheVoteInfo(), string.Format("VoteNumbers=0,PNumber=0"), string.Format(" AutoID in({0})", autoIds)) > 0)
                {
                    autoIds = "'" + autoIds.Replace(",", "','") + "'";
                    bll.Update(new DictionaryInfo(), string.Format("VoteNums=0"), string.Format(" ForeignKey in({0})", autoIds));
                    resp.Status = 1;
                }
                else
                {
                    resp.Msg = "操作失败";
                }

            }





            return Common.JSONHelper.ObjectToJson(resp);
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 图表数据模型
    /// </summary>
    [Serializable]
    public class ChartDataModel
    {
        /// <summary>
        /// X轴数据
        /// </summary>
        public List<string> categories { get; set; }
        /// <summary>
        /// Y轴数据集合
        /// </summary>
        public List<ChartDataModelSub> series { get; set; }


    }

    /// <summary>
    /// Y轴数据
    /// </summary>
    public class ChartDataModelSub
    {
        /// <summary>
        /// Y轴名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Y轴数据
        /// </summary>
        public List<int> data { get; set; }
    }
}