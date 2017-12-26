using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// WeixinReplyRuleInfoNewsManage 的摘要说明
    /// </summary>
    public class WeixinReplyRuleInfoNewsManage : IHttpHandler, IRequiresSessionState
    {


        static BLLJIMP.BLLWeixin bll;

        ///// <summary>
        ///// 增删改权限
        ///// </summary>
        //private static bool _isedit;
        ///// <summary>
        ///// 查看权限
        ///// </summary>
        //private static bool _isview;

        private static string websiteOwner;//设定该站点所有者

        public void ProcessRequest(HttpContext context)
        {
            websiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
            context.Response.ContentType = "text/plain";
            bll = new BLLJIMP.BLLWeixin("");
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            string result = "false";
            switch (action)
            {
                case "Add":
                    result = Add(context);
                    break;
                case "Edit":
                    result = Edit(context);
                    break;
                case "GetImageList":
                    result = GetImageList(context);
                    break;
                case "Delete":
                    result = Delete(context);
                    break;
                case "QuerySource":
                    result = GetSourceNotAdd(context);
                    break;
                case "Query":
                    result = GetAllByAny(context);
                    break;
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        private static string Add(HttpContext context)
        {

            //if (!_isedit)
            //{
            //    return null;
            //}

            try
            {
                //var userid = websiteOwner; //Comm.DataLoadTool.GetCurrUserID();
                //if (string.IsNullOrEmpty(userid))
                //{
                //    return "请重新登录";
                //}
                var keyword = context.Request["MsgKeyword"];
                var matchtype = context.Request["MatchType"];
                if (!bll.CheckUserKeyword(websiteOwner, keyword))
                {
                    return "关键字重复";
                }
                var model = new WeixinReplyRuleInfo();
                model.MsgKeyword = keyword;
                model.MatchType = matchtype;
                model.ReceiveType = "news";
                model.ReplyType = "news";
                model.CreateDate = DateTime.Now;
                model.RuleType = 1;
                var sourceIds = context.Request["SourceIds"];
                model.UID = bll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
                model.UserID = websiteOwner;
                WeixinReplyRuleImgsInfo ruleImgsInfo;
                if (bll.Add(model))//规则表添加成功，往图文表插入
                {
                    if (!string.IsNullOrEmpty(sourceIds))
                    {


                        foreach (var item in sourceIds.Split(','))
                        {
                            var sourceinfo = bll.Get<WeixinMsgSourceInfo>(string.Format("SourceID='{0}'", item));
                            ruleImgsInfo = new WeixinReplyRuleImgsInfo();
                            ruleImgsInfo.UID = bll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleImgAdd);
                            ruleImgsInfo.RuleID = model.UID;
                            ruleImgsInfo.Title = sourceinfo.Title;
                            ruleImgsInfo.Description = sourceinfo.Description;
                            ruleImgsInfo.PicUrl = sourceinfo.PicUrl;
                            ruleImgsInfo.Url = sourceinfo.Url;
                            bll.Add(ruleImgsInfo);

                        }
                        return "true";


                    }

                }


            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            return "false";

        }

        private static string Edit(HttpContext context)
        {

            //if (!_isedit)
            //{
            //    return null;
            //}

            try
            {
                var uid = context.Request["UID"];
                //if (string.IsNullOrEmpty(userid))
                //{
                //    return "请重新登录";
                //}
                var keyword = context.Request["MsgKeyword"];
                var matchType = context.Request["MatchType"];
                var oldInfo = bll.Get<WeixinReplyRuleInfo>(string.Format("UID={0}", uid));
                if (oldInfo.MsgKeyword != keyword)//对比关键字已经改变
                {
                    //关键字改变,检查关键字是否重复
                    if (!bll.CheckUserKeyword(websiteOwner, keyword))
                    {
                        return "关键字重复";
                    }

                }
                var sourceType = context.Request["SourceType"];
                var sourceIds = context.Request["SourceIds"];
                var model = new WeixinReplyRuleInfo();
                model.UID = uid;
                model.MsgKeyword = keyword;
                model.MatchType = matchType;
                model.ReceiveType = "news";
                model.ReplyType = "news";
                model.RuleType = 1;
                model.UID = context.Request["UID"];
                model.UserID = websiteOwner;
                WeixinReplyRuleImgsInfo ruleImgsInfo;
                if (bll.Update(model))//规则表更新成功，更新图文表信息
                {
                    if (!string.IsNullOrEmpty(sourceIds))
                    {
                        if (sourceType.Equals("imagelist"))//原有的图文表
                        {
                            //更新图文列表
                            bll.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID='{0}' And UID Not IN ({1})", uid, sourceIds));
                            return "true";


                        }
                        else if (sourceType.Equals("source"))//从素材表中选择
                        {

                            //删除旧图片
                            bll.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID='{0}'", uid));
                            //添加新图片 
                            foreach (var item in sourceIds.Split(','))
                            {
                                var sourceInfo = bll.Get<WeixinMsgSourceInfo>(string.Format("SourceID='{0}'", item));
                                ruleImgsInfo = new WeixinReplyRuleImgsInfo();
                                ruleImgsInfo.UID = bll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleImgAdd);
                                ruleImgsInfo.RuleID = model.UID;
                                ruleImgsInfo.Title = sourceInfo.Title;
                                ruleImgsInfo.Description = sourceInfo.Description;
                                ruleImgsInfo.PicUrl = sourceInfo.PicUrl;
                                ruleImgsInfo.Url = sourceInfo.Url;
                                bll.Add(ruleImgsInfo);

                            }
                            return "true";


                        }






                    }
                    else
                    {
                        return "请选择素材";
                    }


                }
                else
                {
                    return "更新规则表失败";
                }


            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            return "false";

        }

        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {

            try
            {
                string ids = context.Request["id"];
                bll.Delete(new WeixinReplyRuleInfo(), string.Format("UID in({0}) ", ids));
                bll.Delete(new WeixinReplyRuleImgsInfo(), string.Format("RuleID in({0}) ", ids));
                return "true";

            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }

        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private static string GetAllByAny(HttpContext context)
        {
            //if (!_isview)
            //{
            //    return null;
            //}

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            var strWhere = string.Format("UserID='{0}' And ReplyType='news' And RuleType=1", websiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                strWhere += " And MsgKeyword like '%" + keyWord + "%'";
            }

            List<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo> list = bll.GetLit<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(pageSize, pageIndex, strWhere, "UID");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(strWhere);

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = list
    });
        }

        /// <summary>
        /// 获取未添加的素材
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetSourceNotAdd(HttpContext context)
        {
            //if (!_isview)
            //{
            //    return null;
            //}

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            var strWhere = string.Format("UserID='{0}'", websiteOwner);

            if (!string.IsNullOrEmpty(keyWord))
            {
                strWhere += " and Title like '%" + keyWord + "%'";
            }

            List<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo> dataList = bll.GetLit<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(pageSize, pageIndex, strWhere, "SourceID");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(strWhere);

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });
        }

        /// <summary>
        /// 获取规则图文列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetImageList(HttpContext context)
        {
            var data = bll.GetList<WeixinReplyRuleImgsInfo>(string.Format("RuleID='{0}'", context.Request["UID"]));
            if (data.Count > 0)
            {
                return string.Format("[{0}]", ZentCloud.Common.JSONHelper.ListToJson<WeixinReplyRuleImgsInfo>(data, ","));

            }
            return "";
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