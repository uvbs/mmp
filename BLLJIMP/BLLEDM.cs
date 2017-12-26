using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;
using System.IO;
using ZentCloud.BLLJIMP.Model;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    ///邮件逻辑
    /// </summary>
    public class BLLEDM : BLL
    {
        public BLLEDM(string userID)
            : base(userID)
        {

        }

        ///// <summary>
        ///// 获取邮件发送失败列表
        ///// </summary>
        ///// <param name="emailID">邮件ID</param>
        ///// <returns></returns>
        //public List<string> GetEmailDeliveryFailureList(string emailID)
        //{
        //    List<string> result = new List<string>();

        //    result = GetList<Model.EmailDetails>(string.Format(" EmailID = '{0}' And SendStatus = -1", emailID)).Select(p => p.ReceiverEmail).ToList();

        //    return result;
        //}

        ///// <summary>
        ///// 判断是否为退订邮箱
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public bool IsUnsubscribeEmail(string userID, string email)
        //{
        //    if (GetList<ZentCloud.BLLJIMP.Model.EmailUnsubscribeInfo>(string.Format(" UserID = '{0}' and UnsubscribeEmail = '{1}' ", userID, email)).Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 判断邮件是否属于指定用户
        ///// </summary>
        ///// <param name="emailID"></param>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public bool CheckEmailIDAndUser(string emailID, string userID)
        //{
        //    if (string.IsNullOrWhiteSpace(emailID) || string.IsNullOrWhiteSpace(userID))
        //    {
        //        return false;
        //    }

        //    try
        //    {
        //        if (GetCount<Model.EmailInfo>(string.Format(" EmailID = '{0}' and UserID = '{1}' ", emailID, userID)) > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    catch { }
        //    return false;
        //}

        ///// <summary>
        ///// 判断是否是重复组名
        ///// </summary>
        ///// <param name="groupName"></param>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public bool IsRepeatEmailGroupName(string groupName, string userID)
        //{
        //    int count = GetCount<Model.MemberGroupInfo>(string.Format("GroupName='{0}' and UserID = '{1}' and GroupType = 2 ",
        //            groupName,
        //            userID
        //        ));

        //    if (count > 0)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// 更发送列表成员数量字段
        ///// </summary>
        ///// <param name="groupID"></param>
        //public int UpdateEmailGroupMemberCount(string groupID)
        //{
        //    int count = GetCount<Model.EmailAddressInfo>(string.Format("GroupID='{0}'", groupID));
        //    int result = Update(new Model.MemberGroupInfo(), string.Format("MemberCount={0}", count), string.Format("GroupID='{0}'", groupID));
        //    if (result > 0)
        //    {

        //    }

        //    return count;
        //}

        ///// <summary>
        ///// 删除指定邮件ID的指定明细
        ///// </summary>
        ///// <param name="EmailID"></param>
        ///// <returns></returns>
        //public int DeleteEmailDetailsByEmailID(string EmailID)
        //{
        //    try
        //    {

        //        return Delete(new Model.EmailDetails(), string.Format(" EmailID = '{0}' ", EmailID));
        //    }
        //    catch { }

        //    return 0;
        //}

        ///// <summary>
        ///// 查询指定邮件触发数据
        ///// </summary>
        ///// <param name="emailID"></param>
        ///// <param name="eventType">0为打开，1为点击</param>
        ///// <returns></returns>
        //public DataTable QueryEmailEventDetails(string emailID, int eventType)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        StringBuilder strSql = new StringBuilder();

        //        strSql.Append("SELECT EventEmail AS 电子邮件地址,SourseIP AS IP地址,EventBrowserID AS 浏览器版本 ,EventDate AS 触发时间 ");

        //        if (eventType.Equals(1))
        //        {
        //            strSql.Append(",RealLink AS 点击URL ");
        //        }

        //        strSql.Append(" FROM ZCJ_EmailEventDetailsInfo a ");

        //        if (eventType.Equals(1))
        //        {
        //            strSql.Append(" left join dbo.ZCJ_EmailLinkInfo b on a.LinkID = b.LinkID ");
        //        }

        //        strSql.AppendFormat(" WHERE a.EmailID = '{0}' AND a.EventType = {1} ", emailID, eventType);

        //        dt = Query(strSql.ToString()).Tables[0];
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return dt;
        //}

        ///// <summary>
        ///// 查询指定邮件发送详情
        ///// </summary>
        ///// <param name="emailID"></param>
        ///// <param name="sendStatus">空为全部列表，-1为退回列表，4为成功列表</param>
        ///// <returns></returns>
        //public DataTable QueryEmailSendDetails(string emailID, string sendStatus)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        StringBuilder strSql = new StringBuilder();

        //        strSql.Append("SELECT ReceiverEmail as 邮箱 ");


        //        strSql.Append(" FROM ZCJ_EmailDetails a ");


        //        strSql.AppendFormat(" WHERE a.EmailID = '{0}'", emailID);
        //        if (!string.IsNullOrEmpty(sendStatus))
        //        {
        //            strSql.AppendFormat(" And a.SendStatus = '{0}'", sendStatus);
        //        }

        //        dt = Query(strSql.ToString()).Tables[0];
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return dt;
        //}

        ///// <summary>
        ///// 合并列表
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="newEmailID"></param>
        ///// <param name="oldEmailIDListStr"></param>
        ///// <returns></returns>
        //public bool CompoundEmailAddressGroupList(string userID, string newEmailID, string oldEmailIDListStr)
        //{
        //    try
        //    {
        //        string tmpTable = userID + Guid.NewGuid().ToString().Replace("-", "");

        //        StringBuilder strSql = new StringBuilder();

        //        strSql.AppendFormat("select distinct(Email) into #{0} from ZCJ_EmailAddressInfo where ", tmpTable);
        //        strSql.AppendFormat(" GroupID in({0}) and UserID = '{1}'; ", oldEmailIDListStr, userID);
        //        strSql.Append(" insert into ZCJ_EmailAddressInfo(UserID,Email,GroupID) ");
        //        strSql.AppendFormat(" select '{0}',Email,'{1}' from #{2}; ", userID, newEmailID, tmpTable);
        //        strSql.AppendFormat(" drop table #{0}; ", tmpTable);
        //        ExecuteSql(strSql.ToString());

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 联合查询邮件触发信息和会员管理信息
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public List<Model.EmailEventDetailsInfo> QueryEmailEventDetailsUnionMemberInfo(List<Model.EmailEventDetailsInfo> dataList, string userID)
        //{
        //    List<Model.EmailEventDetailsInfo> queryList = dataList.DistinctBy(p => p.EventEmail).ToList();

        //    for (int i = 0; i < queryList.Count; i++)
        //    {
        //        //查出基本信息
        //        Model.MemberInfo member = Get<Model.MemberInfo>(string.Format(" UserID = '{0}' and Email = '{1}' ", userID, queryList[i].EventEmail));

        //        if (member != null)
        //        {
        //            queryList[i].Name = member.Name;
        //            queryList[i].Mobile = member.Mobile;
        //            queryList[i].QQ = member.QQ;
        //            queryList[i].Company = member.Company;
        //            queryList[i].Title = member.Title;
        //            queryList[i].GroupName = member.GroupName;
        //        }
        //    }

        //    for (int i = 0; i < dataList.Count; i++)
        //    {
        //        if (queryList.Count(p => p.EventEmail.Equals(dataList[i].EventEmail)) > 0)
        //        {
        //            List<Model.EmailEventDetailsInfo> tmpQueryList = queryList.Where(p => p.EventEmail.Equals(dataList[i].EventEmail)).ToList();

        //            dataList[i].Name = tmpQueryList[0].Name;
        //            dataList[i].Mobile = tmpQueryList[0].Mobile;
        //            dataList[i].QQ = tmpQueryList[0].QQ;
        //            dataList[i].Company = tmpQueryList[0].Company;
        //            dataList[i].Title = tmpQueryList[0].Title;
        //            dataList[i].GroupName = tmpQueryList[0].GroupName;

        //        }
        //    }

        //    return dataList;
        //}

        ///// <summary>
        ///// url解密
        ///// </summary>
        ///// <param name="urlParameter"></param>
        ///// <returns></returns>
        //public ZentCloud.BLLJIMP.Model.EmailEventParams DecodeUrl(string urlParameter)
        //{
        //    return Common.JSONHelper.JsonToModel<ZentCloud.BLLJIMP.Model.EmailEventParams>(Common.Base64Change.DecodeBase64(urlParameter));

        //}
        ///// <summary>
        ///// url加密
        ///// </summary>
        //public string EncryptUrl(ZentCloud.BLLJIMP.Model.EmailEventParams Params)
        //{
        //    return Common.Base64Change.EncodeBase64(Common.JSONHelper.ObjectToJson(Params));
        //}


        ///// <summary>
        ///// 获取xls邮箱地址库数据
        ///// </summary>
        ///// <param name="fullName"></param>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public List<Model.EmailAddressInfo> GetEmailAddressListFromExcel(string fullName, string userID)
        //{
        //    List<DataSet> dsList = Common.OfficeDataOp.ExcelToDataSetList(fullName);
        //    List<Model.EmailAddressInfo> emailList = new List<Model.EmailAddressInfo>();
        //    foreach (DataSet ds in dsList)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            Model.EmailAddressInfo email = new Model.EmailAddressInfo();
        //            Type t = email.GetType();
        //            PropertyInfo[] properties = t.GetProperties();
        //            foreach (PropertyInfo p in properties)
        //            {
        //                if (ModelAttrToExcelColumnsForEmailAddress.ContainsKey(p.Name) && ds.Tables[0].Columns.Contains(ModelAttrToExcelColumnsForEmailAddress[p.Name]))
        //                {
        //                    //Convert.ChangeType(ds.Tables[0].Rows[i][ModelAttrToExcelColumns[p.Name]].ToString(),p.PropertyType)
        //                    p.SetValue(email, ds.Tables[0].Rows[i][ModelAttrToExcelColumnsForEmailAddress[p.Name]].ToString(), null);
        //                }
        //            }
        //            if (email.Email != null && email.Email.Trim() != string.Empty)
        //            {
        //                //email.em = GetGUID(TransacType.MemberAdd);
        //                //email.UserID = userID;
        //                emailList.Add(email);
        //            }
        //        }
        //    }
        //    return emailList;
        //}

        ///// <summary>
        ///// 获取xls邮件发送明细数据
        ///// </summary>
        ///// <param name="fullName"></param>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public List<Model.EmailDetails> GetEmailListFromExcel(string fullName, string userID)
        //{
        //    List<DataSet> dsList = Common.OfficeDataOp.ExcelToDataSetList(fullName);
        //    List<Model.EmailDetails> emailList = new List<Model.EmailDetails>();
        //    foreach (DataSet ds in dsList)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            Model.EmailDetails email = new Model.EmailDetails();
        //            Type t = email.GetType();
        //            PropertyInfo[] properties = t.GetProperties();
        //            foreach (PropertyInfo p in properties)
        //            {
        //                if (ModelAttrToExcelColumns.ContainsKey(p.Name) && ds.Tables[0].Columns.Contains(ModelAttrToExcelColumns[p.Name]))
        //                {
        //                    //Convert.ChangeType(ds.Tables[0].Rows[i][ModelAttrToExcelColumns[p.Name]].ToString(),p.PropertyType)
        //                    p.SetValue(email, ds.Tables[0].Rows[i][ModelAttrToExcelColumns[p.Name]].ToString(), null);
        //                }
        //            }
        //            if (email.ReceiverEmail != null && email.ReceiverEmail.Trim() != string.Empty)
        //            {
        //                //email.em = GetGUID(TransacType.MemberAdd);
        //                //email.UserID = userID;
        //                emailList.Add(email);
        //            }
        //        }
        //    }
        //    return emailList;
        //}

        //public static Dictionary<string, string> ModelAttrToExcelColumns = new Dictionary<string, string>()
        //{
        //    {"ReceiverName","姓名"},
        //    {"ReceiverEmail","邮箱"}
        //};

        //public static Dictionary<string, string> ModelAttrToExcelColumnsForEmailAddress = new Dictionary<string, string>()
        //{
        //    {"Email","邮箱"}
        //};
        ///// <summary>
        ///// 从客户分组导入邮箱
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="memberGroupIDs"></param>
        ///// <param name="emailGroupId"></param>
        ///// <returns></returns>
        //public int ImportEmailFromCRM(string userID, string memberGroupIDs, string emailGroupId)
        //{
        //    try
        //    {
        //        int result = 0;

        //        string tmpTable = userID + Guid.NewGuid().ToString().Replace("-", "");

        //        StringBuilder strSql = new StringBuilder();

        //        //strSql.AppendFormat("select distinct(Email) into #{0} from ZCJ_MemberInfo where ", tmpTable);
        //        //strSql.AppendFormat(" GroupID in({0}) and UserID = '{1}' and Email is not null and Email <>'' and Email NOT IN (select Email from ZCJ_EmailAddressInfo where ZCJ_EmailAddressInfo.Email=Email  and  UserID ='{1}' and GroupID='{2}'); ", memberGroupIDs, userID, emailGroupId);

        //        //strSql.Append(" insert into ZCJ_EmailAddressInfo(UserID,Email,GroupID) ");
        //        //strSql.AppendFormat(" select '{0}',Email,'{1}' from #{2}; ", userID, emailGroupId, tmpTable);


        //        //strSql.AppendFormat(" drop table #{0}; ", tmpTable);
        //        //return ExecuteSql(strSql.ToString());

        //        ZCBLLEngine.BLLTransaction trans = new ZCBLLEngine.BLLTransaction();

        //        try
        //        {
        //            //构造临时表
        //            strSql.AppendFormat("select distinct(Email) into #{0} from ZCJ_MemberInfo where ", tmpTable);
        //            strSql.AppendFormat(" GroupID in({0}) and UserID = '{1}' and Email is not null and Email <>'' and Email NOT IN (select Email from ZCJ_EmailAddressInfo where ZCJ_EmailAddressInfo.Email=Email  and  UserID ='{1}' and GroupID='{2}'); ", memberGroupIDs, userID, emailGroupId);
        //            int tmp = ExecuteSql(strSql.ToString(), trans);

        //            //插入数据，并保存成功的条数
        //            strSql = new StringBuilder();
        //            strSql.Append(" insert into ZCJ_EmailAddressInfo(UserID,Email,GroupID) ");
        //            strSql.AppendFormat(" select '{0}',Email,'{1}' from #{2}; ", userID, emailGroupId, tmpTable);
        //            result = ExecuteSql(strSql.ToString(), trans);

        //            //删除临时表
        //            strSql = new StringBuilder();
        //            strSql.AppendFormat(" drop table #{0}; ", tmpTable);
        //            tmp = ExecuteSql(strSql.ToString(), trans);

        //            trans.Commit();
        //        }
        //        catch
        //        {
        //            trans.Rollback();
        //        }

        //        return result;

        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        ///// <summary>
        ///// 添加邮件列表数据
        ///// </summary>
        ///// <param name="emailList"></param>
        ///// <returns></returns>
        //public int AddEmailAddressList(List<Model.EmailAddressInfo> emailList, int size = 1000)
        //{
        //    int result = 0;

        //    try
        //    {
        //        //优化，直接分批执行批量语句进行插入
        //        int totalBatch = GetTotalPage(emailList.Count, size);
        //        StringBuilder strSql = new StringBuilder();
        //        //List<string> sqlList = new List<string>();

        //        for (int i = 0; i < totalBatch; i++)
        //        {
        //            strSql = new StringBuilder();
        //            List<Model.EmailAddressInfo> tmpList = emailList.Skip(i * size).Take(size).ToList();

        //            strSql.AppendFormat("insert into ZCJ_EmailAddressInfo (UserID,Email,GroupID) values ");

        //            foreach (var item in tmpList)
        //            {
        //                //查询下不重复后添加到插入语句,检查重复以后在看怎么优化掉
        //                if (Exists(item, new List<string>() { "UserID", "Email", "GroupID" }))
        //                    continue;

        //                //strSql.AppendFormat("insert into ZCJ_EmailAddressInfo (UserID,Email,GroupID) values('{0}','{1}','{2}');",
        //                //        item.UserID,
        //                //        item.Email,
        //                //        item.GroupID
        //                //    );

        //                strSql.AppendFormat("('{0}','{1}','{2}'),",
        //                        item.UserID,
        //                        item.Email,
        //                        item.GroupID
        //                    );

        //                //result++;
        //                //sqlList.Add(strSql.ToString());
        //            }

        //            string tmpSql = strSql.ToString().TrimEnd(',');
        //            if (strSql.ToString() != "" && strSql.ToString().EndsWith(","))
        //            {
        //                tmpSql = tmpSql.TrimEnd(',');
        //                result += ExecuteSql(tmpSql);
        //            }
        //            //result += tmpList.Count;

        //            //result += ExecuteSqlTran(sqlList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return result;
        //}

        //public int ImportEmailAddressList(List<Model.EmailAddressInfo> emailList)
        //{
        //    int result = 0;

        //    if (emailList.Count.Equals(0))
        //        return result;

        //    string basePath = HttpContext.Current.Server.MapPath("~\\FileUpload\\EDM\\Tmp");//获取到根目录，例如  E:\\项目\\聚比特整合平台\\源文件\\JubitIMP\\Web\\Handler\\EDM
        //    string xmlFormatPath = HttpContext.Current.Server.MapPath("~\\Comm\\EmailAddressFormat.xml");

        //    basePath = @"\\192.168.1.143\BulkFiles";

        //    string connstr = Common.ConfigHelper.GetConfigString("ConnectionString");

        //    //构造文件内容存入临时文件
        //    string tmpFileName = Guid.NewGuid().ToString() + ".txt";
        //    string tmpFilePath = string.Format("{0}\\{1}", basePath, tmpFileName);

        //    StringBuilder strContent = new StringBuilder();
        //    foreach (var item in emailList)
        //    {
        //        strContent.AppendFormat("{0},{1},{2}\n", item.UserID, item.Email, item.GroupID);
        //    }
        //    using (StreamWriter sw = new StreamWriter(tmpFilePath, false, Encoding.UTF8))
        //    {
        //        sw.Write(strContent.ToString());
        //    }

        //    //导入数据
        //    Common.ImportDataHelper mimp = new Common.ImportDataHelper();

        //    result = mimp.BulkImport(connstr, @"C:\BulkFiles\" + tmpFileName, @"C:\BulkFiles\EmailAddressFormat.xml", "ZCJ_EmailAddressInfo", " UserID,Email,GroupID ", "UserID,Email,GroupID", "");

        //    //清除临时文件
        //    if (File.Exists(tmpFilePath))
        //        File.Delete(tmpFilePath);

        //    return result;
        //}

        //public int ImportEmailAddressList(List<string> emailList, string userID, string groupID)
        //{
        //    int result = 0;
        //    try
        //    {
        //        if (emailList.Count.Equals(0))
        //            return result;

        //        string basePath = HttpContext.Current.Server.MapPath("~\\FileUpload\\EDM\\Tmp");//获取到根目录，例如  E:\\项目\\聚比特整合平台\\源文件\\JubitIMP\\Web\\Handler\\EDM

        //        basePath = @"\\192.168.1.143\BulkFiles";

        //        string connstr = Common.ConfigHelper.GetConfigString("ConnectionString");

        //        //构造文件内容存入临时文件
        //        string tmpFileName = Guid.NewGuid().ToString() + ".txt";
        //        string tmpFilePath = string.Format("{0}\\{1}", basePath, tmpFileName);

        //        StringBuilder strContent = new StringBuilder();
        //        strContent.Append("email\n");
        //        foreach (var item in emailList)
        //        {
        //            if (ZentCloud.Common.ValidatorHelper.EmailLogicJudge(item))//建起最后一道防线
        //                strContent.AppendFormat("{0}\n", item);
        //        }
        //        using (StreamWriter sw = new StreamWriter(tmpFilePath, false, Encoding.UTF8))
        //        {
        //            sw.Write(strContent.ToString());
        //        }

        //        //Common.IOHelper ioHelper = new Common.IOHelper();
        //        //ioHelper.WriteListToTextFile(emailList, tmpFilePath);

        //        //导入数据
        //        Common.ImportDataHelper impHelper = new Common.ImportDataHelper();

        //        result = impHelper.BulkImport(
        //            connstr,
        //            @"C:\BulkFiles\" + tmpFileName,
        //            @"C:\BulkFiles\EmailAddressFormatOnlyEmail.xml",
        //            "ZCJ_EmailAddressInfo",
        //            " UserID,Email,GroupID ",
        //            string.Format("'{0}',Email,'{1}'", userID, groupID),
        //            "",
        //            80000,
        //            2
        //            );

        //        //清除临时文件
        //        if (File.Exists(tmpFilePath))
        //            File.Delete(tmpFilePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 更新邮件报表处理方法
        ///// </summary>
        ///// <param name="emailidList"></param>
        ///// <returns></returns>
        //public int UpdateEdmReportProc(List<string> emailidList)
        //{
        //    int result = 0;

        //    List<Model.EmailInfo> dataList = new List<EmailInfo>();

        //    if (emailidList != null && emailidList.Count != 0)
        //    {
        //        foreach (var item in emailidList)
        //        {
        //            dataList.Add(Get<EmailInfo>(string.Format(" EmailID = '{0}' ", item)));
        //        }
        //    }
        //    else
        //    {
        //        string dateStr = DateTime.Now.AddMonths(-1).ToString();
        //        dataList = GetList<EmailInfo>(string.Format(" SubmitDate > '{0}' ", dateStr));
        //    }

        //    for (int i = 0; i < dataList.Count; i++)
        //    {
        //        EmailInfo model = dataList[i];
        //        if (this.UpdateEdmReport(ref model))
        //        {
        //            if (Update(model))
        //                result++;
        //        }
        //    }

        //    return result;
        //}



        ///// <summary>
        ///// 更新邮件报表
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //private bool UpdateEdmReport(ref Model.EmailInfo model)
        //{
        //    bool result = false;

        //    try
        //    {
        //        model.OSendTotalCount = GetCount<EmailDetails>(string.Format(" EmailID = '{0}' ", model.EmailID));
        //        model.ODeliverySuccessCount = GetCount<EmailDetails>(string.Format(" EmailID = '{0}' AND SendStatus = 4 ", model.EmailID));
        //        model.ODeliveryFailureCount = GetCount<EmailDetails>(string.Format(" EmailID = '{0}' AND SendStatus = -1 ", model.EmailID));
        //        model.ODistOpensCount = GetCount<EmailEventDetailsInfo>("EventEmail", string.Format(" EmailID = '{0}' AND EventType = 0 ", model.EmailID));
        //        model.OOpensCount = GetCount<EmailEventDetailsInfo>(string.Format(" EmailID = '{0}' AND EventType = 0 ", model.EmailID));
        //        model.ODistOUrlClicksCount = GetCount<EmailEventDetailsInfo>("EventEmail", string.Format(" EmailID = '{0}' AND EventType = 1 ", model.EmailID));
        //        model.OUrlClicksCount = GetCount<EmailEventDetailsInfo>(string.Format(" EmailID = '{0}' AND EventType = 1 ", model.EmailID));

        //        model.LastUpdateReportDate = DateTime.Now;
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return result;
        //}



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">收件人邮件地址</param>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="displayName">发件人显示名</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="userName">登录smtp主机时用到的用户名,注意是邮件地址'@'以前的部分</param>
        /// <param name="password">登录smtp主机时用到的用户密码</param>
        /// <param name="smtpHost">发送邮件用到的smtp主机</param>
        /// <param name="smtpHost">端口默认 25</param>
        public bool Send(string to, string from, string displayName, string subject, string body, string userName, string password, string smtpHost, int port = 25)
        {
            try
            {
                MailAddress fromaddress = new MailAddress(from, displayName);
                MailAddress toaddress = new MailAddress(to);
                MailMessage message = new MailMessage(fromaddress, toaddress);
                message.Subject = subject;//设置邮件主题
                message.BodyEncoding = System.Text.UnicodeEncoding.UTF8;
                message.IsBodyHtml = true;//设置邮件正文为html格式
                message.Body = body;//设置邮件内容
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient(smtpHost, port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //设置发送邮件身份验证方式
                client.Credentials = new NetworkCredential(userName, password);
                //client.EnableSsl = true;//经过ssl加密    
                client.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 五步会发送提醒邮件
        /// </summary>
        /// <param name="to">收件人邮件地址</param>
        /// <param name="displayName">发件人显示名</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        public bool Step5Send(string displayName, string subject, string body)
        {
            
            return Send(ConfigurationManager.AppSettings["Step5ReceiveEmail"], ConfigurationManager.AppSettings["Step5EmaiSendUserName"], displayName, subject, body, ConfigurationManager.AppSettings["Step5EmaiSendUserName"], ConfigurationManager.AppSettings["Step5EmaiSendUserPwd"], ConfigurationManager.AppSettings["Step5SmtpHost"], int.Parse(ConfigurationManager.AppSettings["Step5SmtpPort"]));
           
            
        }

        /// <summary>
        /// 五步会职位申请 邮件提醒
        /// </summary>
        /// <param name="applyInfo">申请职位信息</param>
        /// <param name="postionInfo">职位信息</param>
        /// <param name="userInfo">用户信息</param>
        public void Step5ApplyPostionRemind(ApplyPositionInfo applyInfo,PositionInfo postionInfo,UserInfo userInfo)        {
            string body = GetFileStr("/wubuhui/emailtemplate/template1.htm");
            body = body.Replace("{$POSTIONNAME$}",postionInfo.Title);
            body = body.Replace("{$COMPANYNAME$}", postionInfo.Company);
            body = body.Replace("{$APPLYTRUENAME$}", userInfo.TrueName);
            body = body.Replace("{$APPLYPHONE$}", userInfo.Phone);
            body = body.Replace("{$APPLYTIME$}", DateTime.Now.ToString());
            Step5Send("Step5", "有新的职位申请", body);
        
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetFileStr(string filePath)
        {
            string strOut;
            strOut = "";
            if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(filePath)))
            {
            }
            else
            {
                StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(filePath), System.Text.Encoding.Default);
                String readerResult = reader.ReadToEnd();
                reader.Close();
                strOut = readerResult;
            }
            return strOut;
        }




    }
}
