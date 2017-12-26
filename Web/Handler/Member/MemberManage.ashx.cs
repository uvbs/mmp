using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using System.IO;
using System.Data;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Handler.Member
{
    /// <summary>
    /// MemberManage 的摘要说明
    /// </summary>
    public class MemberManage : IHttpHandler, IRequiresSessionState
    {

        BLLJIMP.BLLMember bllMember;
        AshxResponse resp = new AshxResponse();
        UserInfo userInfo;
        public void ProcessRequest(HttpContext context)
        {
            try
            {

                this.userInfo = Comm.DataLoadTool.GetCurrUserModel();
                bllMember = new BLLJIMP.BLLMember(Comm.DataLoadTool.GetCurrUserID());
                context.Response.ContentType = "text/plain";
                context.Response.Expires = 0;
                string Action = context.Request["Action"];
                string result = "false";
                switch (Action)
                {
                    case "Add":
                        result = Add(context);
                        break;
                    case "Edit":
                        result = Edit(context);
                        break;
                    case "Delete":
                        result = Delete(context);
                        break;
                    case "Query":
                        result = GetAllByAny(context);
                        break;
                    case "GetMemberGroupList":
                        result = GetMemberGroupList();
                        break;
                    case "SendSMS":
                        result = SendSMS(context);
                        break;
                    case "BatchInsertMembers":
                        result = BatchInsertMembers(context);
                        break;





                }
                context.Response.Write(result);
            }
            catch (Exception ex)
            {

                context.Response.Write(ex.Message);
                return;
            }

        }

        /// <summary>
        /// 获取用户分组
        /// </summary>
        private string GetMemberGroupList()
        {

            var UserID = Comm.DataLoadTool.GetCurrUserID();
            string result = " <select id=\"ddlMemberGroup\" style=\"width:180px;\">";
            result += " <option value=\"0\">无分组</option>";
            var GroupList = bllMember.GetList<MemberGroupInfo>(string.Format("UserID='{0}' and GroupType=1", UserID));
            foreach (MemberGroupInfo item in GroupList)
            {
                result += string.Format("<option value=\"{0}\">{1}</option>", item.GroupID, item.GroupName);
            }
            result += "</select>";
            return result.ToString();

        }

        /// <summary>
        /// 添加
        /// </summary>
        private string Add(HttpContext context)
        {
            var model = GetModel(context);
            if (!PageValidate.IsMobile(model.Mobile))
            {
                return "手机号码格式不正确";
            }
            model.MemberID = bllMember.GetGUID(BLLJIMP.TransacType.MemberAdd);
            model.UserID = Comm.DataLoadTool.GetCurrUserID();
            return bllMember.Add(model).ToString().ToLower();


        }


        /// <summary>
        /// 修改
        /// </summary>
        public string Edit(HttpContext context)
        {
            var memberid = context.Request["MemberID"];
            var userid = Comm.DataLoadTool.GetCurrUserID();
            var memberinfo = bllMember.Get<MemberInfo>(string.Format("MemberID='{0}'", memberid));
            if (memberinfo == null)
            {
                return "客户编号不存在";
            }
            if (!memberinfo.UserID.Equals(userid))
            {
                return "无权修改";
            }
            var model = GetModel(context);
            if (!PageValidate.IsMobile(model.Mobile))
            {
                return "手机号码格式不正确";
            }
            model.MemberID = memberid;
            model.UserID = userid;
            //var count= bll.Update(model, string.Format(" Name='{0}',Sex='{1}',Birthday='{2}',Mobile='{3}',Email='{4}',QQ='{5}',Tel='{6}',Website='{7}',Company='{8}',Title='{9}',GroupID='{10}',CardImageUrl='{11}',Address='{12}'",model.Name,model.Sex,model.Birthday,model.Mobile,model.Email,model.QQ,model.Tel,model.Website,model.Company,model.Title,model.GroupID,model.CardImageUrl,model.Address), string.Format("MemberID='{0}'", memberid));
            var count = bllMember.Update(model, string.Format(" Name='{0}',Sex='{1}',Birthday='{2}',Mobile='{3}',Email='{4}',QQ='{5}',Tel='{6}',Website='{7}',Company='{8}',Title='{9}',GroupID='{10}',CardImageUrl='{11}',Address='{12}',WeiboID='{13}',WeiboScreenName='{14}',Remark='{15}',WeixinOpenID='{16}',MemberType={17},Mobile2='{18}',Mobile3='{19}',Mobile4='{20}',WeiboID2='{21}',WeiboID3='{22}',WeiboID4='{23}',WeixinOpenID2='{24}',WeixinOpenID3='{25}',WeixinOpenID4='{26}',Email2='{27}',Email3='{28}',Email4='{29}'", model.Name, model.Sex, model.Birthday, model.Mobile, model.Email, model.QQ, model.Tel, model.Website, model.Company, model.Title, model.GroupID, model.CardImageUrl, model.Address, model.WeiboID, model.WeiboScreenName, model.Remark, model.WeixinOpenID, model.MemberType, model.Mobile2, model.Mobile3, model.Mobile4, model.WeiboID2, model.WeiboID3, model.WeiboID4, model.WeixinOpenID2, model.WeixinOpenID3, model.WeixinOpenID4, model.Email2, model.Email3, model.Email4), string.Format("MemberID='{0}'", memberid));

            if (count > 0)
            {
                return "true";
            }
            return "false";
        }


        /// <summary>
        /// 删除
        /// </summary>
        private string Delete(HttpContext context)
        {
            try
            {
                var userid = Comm.DataLoadTool.GetCurrUserID();
                string ids = context.Request["id"];
                var count = 0;

                if (string.IsNullOrWhiteSpace(ids))
                {
                    resp.Status = 0;
                    resp.Msg = "传入删除对象不能为空!";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

                //ids转成数据库兼容的格式
                ids = Common.StringHelper.ListToStr<string>(ids.Split(',').ToList(), "'", ",");

                count = this.bllMember.Delete(new MemberInfo(), string.Format(" MemberID IN ({0}) AND UserID = '{1}'", ids, userid));

                if (count.Equals(0))
                {
                    resp.Status = 0;
                    resp.Msg = "没有删除任何数据!";
                }
                else
                {
                    resp.Status = 1;
                    resp.Msg = string.Format("成功删除{0}条数据", count);
                }

                return Common.JSONHelper.ObjectToJson(resp);
            }
            catch (Exception ex)
            {
                resp.Status = 0;
                resp.Msg = ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);
            }

            #region 旧方法隐藏 2013.8.28
            //foreach (var item in ids.Split(','))
            //{
            //    var memberinfo = bllMember.Get<MemberInfo>(string.Format("MemberID='{0}'", item));
            //    if (memberinfo == null)
            //    {
            //        return "客户编号不存在";
            //        //break;
            //    }
            //    if (!memberinfo.UserID.Equals(userid))
            //    {
            //        return "无权修改";
            //        //break;
            //    }
            //    count += bllMember.Delete(new MemberInfo(), string.Format("MemberID ='{0}'", item));


            //}

            //if (count.Equals(ids.Split(',').Length))
            //{
            //    return "true";
            //}
            //else
            //{
            //    return "false";
            //}
            #endregion



        }





        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private string GetAllByAny(HttpContext context)
        {

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string searchtitle = context.Request["SearchTitle"];
            string groupid = "";
            try
            {
                groupid = context.Request["GroupID"];

            }
            catch (Exception)
            {


            }
            var searchCondition = string.Format("UserID='{0}'", Comm.DataLoadTool.GetCurrUserID());

            if (!string.IsNullOrEmpty(searchtitle))
            {
                searchCondition += " And Name like '%" + searchtitle + "%'";
            }

            if (!string.IsNullOrEmpty(groupid))
            {
                searchCondition += string.Format(" And GroupID in ({0})", groupid);
            }

            List<MemberInfo> list = bllMember.GetLit<MemberInfo>(rows, page, searchCondition);

            int totalCount = bllMember.GetCount<MemberInfo>(searchCondition);

            string jsonResult = ZentCloud.Common.JSONHelper.ObjectToJson(totalCount, list);

            return jsonResult;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SendSMS(HttpContext context)
        {
            if (context.Request["Type"].Equals("1"))//向勾选号码发送短信
            {
                var mobilelist = context.Request["MobileList"].Replace(",", "\n");
                context.Session[Comm.SessionKey.PageRedirect] = "/Member/MemberList.aspx";
                context.Session[Comm.SessionKey.PageCacheName] = "cache" + bllMember.GetGUID(ZentCloud.BLLJIMP.TransacType.CacheGet);
                Comm.DataCache.SetCache(context.Session[Comm.SessionKey.PageCacheName].ToString(), mobilelist);
                return "true";

            }
            else if (context.Request["Type"].Equals("0"))//向筛选号码发送短信
            {
                string searchtitle = context.Request["SearchTitle"];
                string groupid = context.Request["GroupID"];
                var searchCondition = string.Format("UserID='{0}'", Comm.DataLoadTool.GetCurrUserID());
                if (!string.IsNullOrEmpty(searchtitle))
                {
                    searchCondition += " And Name like '%" + searchtitle + "%'";
                }
                if (!string.IsNullOrEmpty(groupid))
                {
                    searchCondition += string.Format(" And GroupID in ({0})", groupid);
                }

                List<MemberInfo> list = bllMember.GetList<MemberInfo>(searchCondition);
                string mobilelist = "";
                foreach (var item in list)
                {
                    try
                    {
                        mobilelist += item.Mobile + "\n";
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }
                mobilelist = mobilelist.TrimEnd('\n');
                context.Session[Comm.SessionKey.PageRedirect] = "/Member/MemberList.aspx";
                context.Session[Comm.SessionKey.PageCacheName] = "cache" + bllMember.GetGUID(ZentCloud.BLLJIMP.TransacType.CacheGet);
                Comm.DataCache.SetCache(context.Session[Comm.SessionKey.PageCacheName].ToString(), mobilelist);
                return "true";


            }
            return "false";

        }
        /// <summary>
        /// 导入联系人
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatchInsertMembers(HttpContext context)
        {
            try
            {
                HttpPostedFile file = context.Request.Files["BatchInsertFile"];
                var groupIDStr = context.Request["GroupID"];
                var ExtraName = Common.IOHelper.GetExtraName(file.FileName);

                int groupID = 0;

                if ((!ExtraName.Equals("xls")))
                {
                    resp.Status = 0;
                    resp.Msg = "只支持上传xls格式的文件!";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                if (!int.TryParse(groupIDStr, out groupID))
                {
                    resp.Status = 0;
                    resp.Msg = "传入客户分组异常，请重新选择!";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                //判断分组权限
                if (!this.bllMember.CheckGroupIDAndUser(groupIDStr, this.userInfo.UserID))
                {
                    resp.Status = 0;
                    resp.Msg = "没有该客户分组权限!";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                //上传文件
                string dirpath = context.Server.MapPath("/FileUpload/Temp/");
                if (!Directory.Exists(dirpath))
                {
                    Directory.CreateDirectory(dirpath);
                }
                string filename = DateTime.Now.ToString("yyyyMMddHHmmss");

                string savepath = string.Format("{0}{1}.xls", dirpath, filename);

                file.SaveAs(savepath);//上传文件

                List<ZentCloud.BLLJIMP.Model.MemberInfo> memberList = bllMember.GetMemberListFromExcel(savepath, bllMember.UserID);

                memberList = memberList.Distinct().ToList();//去重

                //处理格式出错的实体
                List<ZentCloud.BLLJIMP.Model.MemberInfo> badMemberList = new List<MemberInfo>();
                badMemberList = memberList.Where(
                    p =>
                        (!string.IsNullOrWhiteSpace(p.Email) && !Common.ValidatorHelper.EmailLogicJudge(p.Email))//邮件字段不为空，且格式不正确时
                        ||
                        (!string.IsNullOrWhiteSpace(p.Mobile) && !Common.ValidatorHelper.PhoneNumLogicJudge(p.Mobile))//手机字段不为空，且格式不正确时
                        ||
                        string.IsNullOrWhiteSpace(p.Name)//姓名也不允许为空

                    ).ToList();

                //格式正确的实体
                List<ZentCloud.BLLJIMP.Model.MemberInfo> goodMemberList = new List<MemberInfo>();
                goodMemberList = memberList.Where(
                    p =>
                        !badMemberList.Contains(p)
                    ).ToList();



                int importCount = this.bllMember.ImportMemberList(goodMemberList, Comm.DataLoadTool.GetCurrUserID(), groupIDStr);

                File.Delete(savepath);



                resp.Status = 1;
                resp.Msg = string.Format("本次共导入{0}个联系人", importCount);

                if (badMemberList.Count > 0)
                {
                    //生成错误数据并生成文件供用户下载
                    string tmpBadFileName = Guid.NewGuid().ToString() + ".xls";
                    Common.NPOIHelper.DtToXls(this.bllMember.CreateMemberDataTableByList(badMemberList), dirpath + tmpBadFileName, "手机、邮箱或者姓名格式有误");

                    resp.Msg += string.Format(",导入格式错误有{0}个", badMemberList.Count);
                    resp.Msg += ",请下载错误文件修改后再上传";

                    resp.ExStr = string.Format(@"/DownloadPage.aspx?fn={0}", tmpBadFileName);

                }


                ////检查邮件或者手机格式错误数量
                //int badMobile = memberList.Count(p => !Common.ValidatorHelper.PhoneNumLogicJudge(p.Mobile));
                //int badEmail = memberList.Count(p => !Common.ValidatorHelper.EmailLogicJudge(p.Email));

                //if (badMobile > 0)
                //{
                //    resp.Msg += string.Format(",错误的手机格式有{0}个", badMobile);
                //}
                //if (badEmail > 0)
                //{
                //    resp.Msg += string.Format(",错误的邮箱格式有{0}个", badEmail);
                //}
                //if (badEmail > 0 || badMobile > 0)
                //{
                //    resp.Msg += ",请稍后自行在平台上做修改";
                //}

            }
            catch (Exception ex)
            {
                resp.Status = 0;
                resp.Msg = ex.Message;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }



        /// <summary>
        /// 获取传入的实体
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private MemberInfo GetModel(HttpContext context)
        {
            return ZentCloud.Common.JSONHelper.JsonToModel<BLLJIMP.Model.MemberInfo>(context.Request["JsonData"]);
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