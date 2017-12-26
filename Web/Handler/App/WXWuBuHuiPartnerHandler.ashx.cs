using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXWuBuHuiPartnerHandler 的摘要说明
    /// </summary>
    public class WXWuBuHuiPartnerHandler : IHttpHandler, IRequiresSessionState
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
        /// <summary>
        /// 当前用户
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;
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
        /// 获取导师列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPartnerInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.WBHPartnerInfo> dataList;
            string voteName = context.Request["VoteName"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(voteName))
            {
                sbWhere.AppendFormat(" AND VoteName lIKE '%{0}%'", voteName);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.WBHPartnerInfo>(sbWhere.ToString());
            dataList = this.bllJuactivity.GetLit<BLLJIMP.Model.WBHPartnerInfo>(pageSize, pageIndex, sbWhere.ToString());
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 获取五伴会列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPartnerInfoList(HttpContext context)
        {
            string pageIndex = context.Request["PageIndex"];
            string pageSize = context.Request["PageSize"];
            string title = context.Request["Title"];
            string type = context.Request["Type"];
            string sort = context.Request["Sort"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" websiteOwner='{0}'", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(title))
            {
                sbWhere.AppendFormat(" AND PartnerName Like '%{0}%'", title);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" AND PartnerType Like '%{0}%'", type);
            }
            string sortF = "InsertDate DESC";
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("zan"))
                {
                    sortF = "ParTnerStep DESC";
                }
            }

            List<BLLJIMP.Model.WBHPartnerInfo> data = bllJuactivity.GetLit<BLLJIMP.Model.WBHPartnerInfo>(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex), sbWhere.ToString(), sortF);

            if (data.Count > 0)
            {
                //foreach (BLLJIMP.Model.WBHPartnerInfo item in PInfos)
                //{
                //    if (!string.IsNullOrEmpty(item.PartnerType))
                //    {
                //        item.Ctype = juActivityBll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='Partner' AND AutoID in ({1})", this.websiteOwner, item.PartnerType + 0));
                //    }
                //}
                for (int i = 0; i < data.Count; i++)
                {
                    if (!string.IsNullOrEmpty(data[i].PartnerType))
                    {
                        if (!data[i].PartnerType.EndsWith(","))
                        {
                            data[i].PartnerType += ",";
                        }
                        data[i].Ctype = bllJuactivity.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='Partner' AND AutoID in ({1})", bll.WebsiteOwner, data[i].PartnerType + 0));
                    }
                    data[i].PartnerContext = null;
                }
                resp.Status = 0;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = 0;
                resp.ExObj = null;
            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加更新职位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AUPartnerInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string partnerName = context.Request["PartnerName"];
            string partnerContext = context.Request["PartnerContext"];
            string partnerAddress = context.Request["PartnerAddress"];
            string partnerType = context.Request["PartnerType"];
            string partnerImg = context.Request["PartnerImg"];
            BLLJIMP.Model.WBHPartnerInfo model = bllJuactivity.Get<BLLJIMP.Model.WBHPartnerInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, autoId));
            if (model != null)
            {
                model.PartnerName = partnerName;
                model.PartnerContext = partnerContext;
                model.PartnerAddress = partnerAddress;
                model.PartnerType = partnerType;
                model.PartnerImg = partnerImg;


                if (bllJuactivity.Update(model))
                {
                    resp.Status = 0;
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
                model = new BLLJIMP.Model.WBHPartnerInfo()
                {
                    PartnerName = partnerName,
                    PartnerContext = partnerContext,
                    PartnerAddress = partnerAddress,
                    PartnerType = partnerType,
                    PartnerImg = partnerImg,
                    InsertDate = DateTime.Now,
                    WebsiteOwner = bll.WebsiteOwner
                };
                if (bllJuactivity.Add(model))
                {
                    resp.Status = 0;
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
        private string GetPartnerInfo(HttpContext context)
        {
            string autoId = context.Request["AutoId"];
            if (string.IsNullOrEmpty(autoId))
            {
                resp.Status = -1;
                resp.Msg = "系统出错，请联系管理员！";
                goto OutF;
            }
            BLLJIMP.Model.WBHPartnerInfo model = bllJuactivity.Get<BLLJIMP.Model.WBHPartnerInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, autoId));
            resp.Status = 0;
            resp.ExObj = model;

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除职位信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelPartnerInfo(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.WBHPartnerInfo(), string.Format(" AutoId in ({0})", ids));
            if (count > 0)
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
        /// 获取导师列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetBannerInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.WBHBannaImg> dataList;
            string voteName = context.Request["VoteName"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(voteName))
            {
                sbWhere.AppendFormat(" AND VoteName lIKE '%{0}%'", voteName);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.WBHBannaImg>(sbWhere.ToString());
            dataList = this.bllJuactivity.GetLit<BLLJIMP.Model.WBHBannaImg>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }


        /// <summary>
        /// 添加更新职位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AUBannerInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string bannaName = "首页";
            string bannaImg = context.Request["BannaImg"];
            string bannerUrl = context.Request["BannaUrl"];

            BLLJIMP.Model.WBHBannaImg model = bllJuactivity.Get<BLLJIMP.Model.WBHBannaImg>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, autoId));
            if (model != null)
            {
                model.BannaName = bannaName;
                model.BannaUrl = bannerUrl;
                model.BannaImg = bannaImg;
                bool isSuccess = bllJuactivity.Update(model);
                if (isSuccess)
                {
                    resp.Status = 0;
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
                model = new BLLJIMP.Model.WBHBannaImg()
                {
                    BannaName = bannaName,
                    BannaUrl = bannerUrl,
                    BannaImg = bannaImg,
                    InsertDate = DateTime.Now,
                    WebsiteOwner = bll.WebsiteOwner
                };
                bool bo = bllJuactivity.Add(model);
                if (bo)
                {
                    resp.Status = 0;
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
        private string GetBannerInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            if (string.IsNullOrEmpty(autoId))
            {
                resp.Status = -1;
                resp.Msg = "系统出错，请联系管理员！";
                goto OutF;
            }
            BLLJIMP.Model.WBHBannaImg model = bllJuactivity.Get<BLLJIMP.Model.WBHBannaImg>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, autoId));
            resp.Status = 0;
            resp.ExObj = model;

        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除职位信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelBannerInfo(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.WBHBannaImg(), string.Format(" AutoId in ({0})", ids));
            if (count > 0)
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
        /// 获取意见列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetOpinionInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.WBHOpinionInfo> dataList;
            string voteName = context.Request["VoteName"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(voteName))
            {
                sbWhere.AppendFormat(" AND VoteName Like  '%{0}%'", voteName);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.WBHOpinionInfo>(sbWhere.ToString());
            dataList = this.bllJuactivity.GetLit<BLLJIMP.Model.WBHOpinionInfo>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 添加更新职位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AUOpinionInfo(HttpContext context)
        {

            string content = context.Request["OContext"];
            string type = context.Request["Otype"];


            BLLJIMP.Model.WBHOpinionInfo model = new BLLJIMP.Model.WBHOpinionInfo()
            {
                OContext = content,
                Otype = type,
                UserId = this.currentUserInfo.UserID,
                UserName = this.currentUserInfo.TrueName ?? this.currentUserInfo.WXNickname,
                InsertDate = DateTime.Now,
                WebsiteOwner = bll.WebsiteOwner

            };
            bool isSuccess = bllJuactivity.Add(model);
            if (isSuccess)
            {
                resp.Status = 0;
                resp.Msg = "感谢您的建议，我们一直在努力做好。谢谢您的支持。";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "系统出错，请联系管理员。";
            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除意见信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelOpinionInfo(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.WBHOpinionInfo(), string.Format(" AutoId in ({0})", ids));
            if (count > 0)
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
        /// 保存五步会赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddParnerPraiseNum(HttpContext context)
        {


            BLLJIMP.Model.ForwardingRecord forRecord = bllJuactivity.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName='五伴会赞'", this.currentUserInfo.UserID, context.Request["id"], bllUser.WebsiteOwner));
            WBHPartnerInfo model = bllUser.Get<WBHPartnerInfo>(string.Format("AutoId={0}", context.Request["id"]));
            if (forRecord == null)
            {
                forRecord = new BLLJIMP.Model.ForwardingRecord()
                {
                    FUserID = this.currentUserInfo.UserID,
                    RUserID = model.AutoId.ToString(),
                    RdateTime = DateTime.Now,
                    WebsiteOwner = bll.WebsiteOwner,
                    TypeName = "五伴会赞"
                };

                bllUser.Add(forRecord);
                model.ParTnerStep++;
                if (bllUser.Update(model))
                {
                    resp.Status = 1;
                    resp.ExInt = model.ParTnerStep;
                    resp.ExStr = "1";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "系统错误，请联系管理员";
                }
            }
            else
            {
                int count = bllUser.Delete(forRecord);
                model.ParTnerStep = model.ParTnerStep - 1;
                if (bllUser.Update(model) && count > 0)
                {
                    resp.Status = 1;
                    resp.ExInt = model.ParTnerStep;
                    resp.ExStr = "0";
                }
                else
                {
                    resp.Status = -1;

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
}
