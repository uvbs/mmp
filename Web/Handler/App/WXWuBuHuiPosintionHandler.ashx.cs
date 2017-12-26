using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Web.SessionState;
using System.Reflection;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXWuBuHuiPosintionHandler 的摘要说明
    /// </summary>
    public class WXWuBuHuiPosintionHandler : IHttpHandler, IRequiresSessionState
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
        /// 基类BLL
        /// </summary>
        BLL bll = new BLL();
        ///// <summary>
        ///// 邮件
        ///// </summary>
        //BLLEDM bllEmail = new BLLEDM("");
        /// <summary>
        /// 当前用户
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
                this.currentUserInfo = bll.GetCurrentUserInfo();
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
        /// 获取职位列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPostitionInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.PositionInfo> data;
            //string voteName = context.Request["VoteName"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            //if (!string.IsNullOrEmpty(voteName))
            //{
            //    sbWhere.AppendFormat(" AND VoteName lIKE '%{0}%'", voteName);
            //}
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.PositionInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.PositionInfo>(pageSize, pageIndex, sbWhere.ToString(), " LastUpdateDate DESC,AutoId DESC");

            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = data
     });

        }

        /// <summary>
        /// 获取职位列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPositiInfoList(HttpContext context)
        {

            string pageIndex = context.Request["PageIndex"];
            string pageSize = context.Request["PageSize"];
            string title = context.Request["Title"];
            string my = context.Request["My"];
            string sort = context.Request["Sort"];
            string tradeIds = context.Request["TradeIds"];
            string professionalIds = context.Request["ProfessionalIds"];


            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbSort = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", bllUser.WebsiteOwner);
            if (!string.IsNullOrEmpty(title))
            {
                sbWhere.AppendFormat(" AND (Title Like '%{0}%' Or Company Like  '%{0}%')", title);
            }
            if (!string.IsNullOrEmpty(tradeIds))
            {
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < tradeIds.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" TradeIds Like '%{0}%' ", tradeIds.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or TradeIds Like '%{0}%' ", tradeIds.Split(',')[i]);

                    }
                }

                sbWhere.AppendFormat(" ) ");
            }
            if (!string.IsNullOrEmpty(professionalIds))
            {
                sbWhere.AppendFormat(" And ( ");
                for (int i = 0; i < professionalIds.Split(',').Length; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" ProfessionalIds Like '%{0}%' ", professionalIds.Split(',')[i]);

                    }
                    else
                    {
                        sbWhere.AppendFormat(" Or ProfessionalIds Like '%{0}%' ", professionalIds.Split(',')[i]);

                    }
                }

                sbWhere.AppendFormat(" ) ");
            }

            if (my.Equals("my"))
            {
                List<BLLJIMP.Model.ApplyPositionInfo> applyList = bllJuactivity.GetList<BLLJIMP.Model.ApplyPositionInfo>(string.Format(" UserId='{0}' and WebsiteOwner='{1}'", this.currentUserInfo.UserID, bll.WebsiteOwner));
                string Pid = "";
                if (applyList.Count > 0)
                {
                    for (int i = 0; i < applyList.Count; i++)
                    {
                        Pid += applyList[i].PositionId + ",";
                    }

                    sbWhere.AppendFormat(" And AutoId in ({0})", Pid.TrimEnd(','));
                }
                else
                {
                    sbWhere.AppendFormat(" And 1=0 ");
                }
            }
            sbSort.Append("LastUpdateDate ");
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("ptiem"))
                {
                    sbSort.Append(" DESC");
                }
                if (sort.Equals("ppv"))
                {
                    sbSort.Clear();
                    sbSort.Append(" Pv DESC");
                }
                if (sort.Equals("time"))
                {
                    sbSort.Clear();
                    sbSort.Append(" LastUpdateDate DESC,InsertDate DESC");
                }

            }
            List<BLLJIMP.Model.PositionInfo> pInfos = bllJuactivity.GetLit<BLLJIMP.Model.PositionInfo>(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex), sbWhere.ToString(), sbSort.ToString());
            if (pInfos != null)
            {


                for (int i = 0; i < pInfos.Count; i++)
                {
                    if ((!string.IsNullOrEmpty(pInfos[i].TradeIds)) || (!string.IsNullOrEmpty(pInfos[i].ProfessionalIds)))
                    {
                        if (string.IsNullOrEmpty(pInfos[i].TradeIds))
                        {
                            pInfos[i].TradeIds = "0";
                        }
                        if (string.IsNullOrEmpty(pInfos[i].ProfessionalIds))
                        {
                            pInfos[i].ProfessionalIds = "0";
                        }
                        pInfos[i].Ctype = bllUser.GetList<ArticleCategory>(string.Format("AutoID in({0}) Or AutoID in({1})", pInfos[i].TradeIds, pInfos[i].ProfessionalIds));

                    }
                }
                resp.Status = 0;
                resp.ExObj = pInfos;
            }
            else
            {
                resp.Status = 0;
                resp.ExObj = null;
            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 保存申请人信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavaApplyPositionInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string skill = context.Request["Skill"];
            string expectedSalary = context.Request["ExpectedSalary"];
            string comeTime = context.Request["ComeTime"];
            string reasonLeaving = context.Request["ReasonLeaving"];
            string trade = context.Request["Trade"];
            string professional = context.Request["Professional"];
            if (bllUser.GetCount<BLLJIMP.Model.ApplyPositionInfo>(string.Format("UserId='{0}' And PositionId={1}", currentUserInfo.UserID, autoId)) > 0)
            {
                resp.Msg = "您已经申请过了";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            BLLJIMP.Model.ApplyPositionInfo applyInfo = new BLLJIMP.Model.ApplyPositionInfo()
            {
                Skill = skill,
                ExpectedSalary = expectedSalary,
                ComeTime = DateTime.Now,
                ReasonLeaving = reasonLeaving,
                UserId = this.currentUserInfo.UserID,
                UserName = this.currentUserInfo.TrueName != null ? this.currentUserInfo.TrueName : this.currentUserInfo.WXNickname,
                WebsiteOwner = bll.WebsiteOwner,
                PositionId = Convert.ToInt32(autoId),
                Phone = currentUserInfo.Phone,
                Trade = trade,
                Professional = professional

            };
            if (bllJuactivity.Add(applyInfo))
            {
                BLLJIMP.Model.PositionInfo pInfo = bllJuactivity.Get<BLLJIMP.Model.PositionInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bllUser.WebsiteOwner, autoId));
                pInfo.PersonNum = pInfo.PersonNum + 1;
                bllJuactivity.Update(pInfo);
                resp.Status = 1;
                resp.Msg = "申请成功";
   
                    //     BLLWeixin bllWeixin = new BLLWeixin("");
                    //    //提醒客服
                    //    var CurrWebSiteUserInfo = bllWeixin.GetCurrWebSiteUserInfo();
                    //    if (!string.IsNullOrEmpty(CurrWebSiteUserInfo.WeiXinKeFuOpenId))
                    //    {
                    //    BLLWeixin.TMTaskNotification notificaiton = new BLLWeixin.TMTaskNotification();
                    //    var acctoken = bllWeixin.GetAccessToken(CurrWebSiteUserInfo.UserID);
                    //    notificaiton.First = "有新的职位申请";
                    //    notificaiton.Keyword1 = "职位:" + PInfo.Title;
                    //    notificaiton.Keyword2 = "职位申请";
                    //    notificaiton.Remark = string.Format("申请人:{0}手机:{1}", apInfo.UserName, apInfo.Phone);
                    //   var result= bllWeixin.SendTemplateMessage(acctoken, CurrWebSiteUserInfo.WeiXinKeFuOpenId, notificaiton);
                    //    }
                    ////提醒客服



                    //发送提醒邮件
                    //bllEmail.Step5ApplyPostionRemind(applyInfo, pInfo, currentUserInfo);
                    //发送提醒邮件

                
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "申请失败";
            }



            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 添加更新职位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AUPositionInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string title = context.Request["Title"];
            string iocnImg = context.Request["IocnImg"];
            string personal = context.Request["Personal"];
            string salaryRange = context.Request["SalaryRange"];
            string enterpriseScale = context.Request["EnterpriseScale"];
            string address = context.Request["Address"];
            string tutorExplain = context.Request["TutorExplain"];
            string workYear = context.Request["WorkYear"];
            string education = context.Request["Education"];
            string tradeIds = context.Request["TradeIds"];
            string professionalIds = context.Request["ProfessionalIds"];
            string city = context.Request["City"];
            string company = context.Request["Company"];
            BLLJIMP.Model.PositionInfo pInfo = bllJuactivity.Get<BLLJIMP.Model.PositionInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, autoId));
            if (pInfo != null)
            {
                pInfo.Title = title;
                pInfo.IocnImg = iocnImg;
                pInfo.Personal = personal;
                pInfo.SalaryRange = salaryRange;
                pInfo.EnterpriseScale = enterpriseScale;
                pInfo.Address = address;
                pInfo.Context = tutorExplain;
                pInfo.WebsiteOwner = bll.WebsiteOwner;
                pInfo.InsertDate = DateTime.Now;
                pInfo.WorkYear = workYear;
                pInfo.Education = education;
                pInfo.TradeIds = tradeIds;
                pInfo.ProfessionalIds = professionalIds;
                pInfo.City = city;
                pInfo.Company = company;
                pInfo.LastUpdateDate = DateTime.Now;
                if (bllJuactivity.Update(pInfo))
                {
                    resp.Status = 1;
                    resp.Msg = "更新成功";
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "更新失败";
                }
            }
            else
            {
                pInfo = new BLLJIMP.Model.PositionInfo()
                {
                    Title = title,
                    IocnImg = iocnImg,
                    Personal = personal,
                    SalaryRange = salaryRange,
                    EnterpriseScale = enterpriseScale,
                    Address = address,
                    InsertDate = DateTime.Now,
                    Context = tutorExplain,
                    WebsiteOwner = bll.WebsiteOwner,
                    Education = education,
                    WorkYear = workYear,
                    TradeIds = tradeIds,
                    ProfessionalIds = professionalIds,
                    City = city,
                    Company = company,
                    LastUpdateDate = DateTime.Now


                };
                if (bllJuactivity.Add(pInfo))
                {
                    resp.Status = 1;
                    resp.Msg = "添加成功";
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "添加失败";
                }

            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取职位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPositionInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            if (string.IsNullOrEmpty(autoId))
            {
                resp.Status = -1;
                resp.Msg = "系统出错，请联系管理员！";
                goto OutF;
            }
            BLLJIMP.Model.PositionInfo pInfo = bllJuactivity.Get<BLLJIMP.Model.PositionInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, autoId));
            resp.Status = 0;
            resp.ExObj = pInfo;

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除职位信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelPositionInfo(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            if (bllJuactivity.Delete(new BLLJIMP.Model.PositionInfo(), string.Format(" AutoId in ({0})", ids)) > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败。";
            }

        Outf:
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取申请职位列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryApplyPostition(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.ApplyPositionInfo> data;

            string postionId = context.Request["PostionId"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}' And PositionId={1}", bll.WebsiteOwner, postionId));
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ApplyPositionInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.ApplyPositionInfo>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = data
    });

        }


        /// <summary>
        /// 删除申请职位信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelApplyPositionInfo(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            if (bllJuactivity.Delete(new BLLJIMP.Model.ApplyPositionInfo(), string.Format(" AutoId in ({0})", ids)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "删除成功。";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除失败。";
            }

        Outf:
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
}
