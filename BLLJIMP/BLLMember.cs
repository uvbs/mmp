using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.IO;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLMember : BLL
    {
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();

        public BLLMember() { }
        public BLLMember(string userID)
            : base(userID)
        {

        }
        //        public static string GetMemberUID()
        //        {
        //            return GetZCUID("MemberUID");
        //        }

        //        public string GetGUID()
        //        {
        //            return GetZCUID("GUID");
        //        }

        //        protected string GetZCUID(string colName)
        //        {
        //            string strSql = string.Format(@"update ZCUID set {0} = {0} + 1
        //                             select {0} from ZCUID", colName);
        //            BLLTransaction tran = new BLLTransaction();
        //            DataSet ds = ZCDALEngine.DALEngine.Query(strSql);
        //            tran.Commit();
        //            return ds.Tables[0].Rows[0][colName].ToString();
        //        }

        //public static string GetName(string mobile, string userID)
        //{
        //    object name = GetSingle("ZCJ_MemberInfo", "Name", string.Format("Mobile = '{0}' and UserID = '{1}'", mobile, userID));
        //    return name == null ? "" : name.ToString();
        //}

        //public static List<Model.MemberInfo> GetMemberListFromExcel(string fullName, string sheetName, string ownerid)
        //{
        //    DataSet ds = Common.OfficeDataOp.ExcelToDataSet(fullName, sheetName);
        //    List<Model.MemberInfo> memberList = new List<Model.MemberInfo>();
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++ )
        //    {
        //        Model.MemberInfo member = new Model.MemberInfo();
        //        Type t = member.GetType();
        //        PropertyInfo[] properties = t.GetProperties();
        //        foreach (PropertyInfo p in properties)
        //        {
        //            if (ModelAttrToExcelColumns.ContainsKey(p.Name) && ds.Tables[0].Columns.Contains(ModelAttrToExcelColumns[p.Name]))
        //            {
        //                //Convert.ChangeType(ds.Tables[0].Rows[i][ModelAttrToExcelColumns[p.Name]].ToString(),p.PropertyType)
        //                p.SetValue(member, ds.Tables[0].Rows[i][ModelAttrToExcelColumns[p.Name]].ToString(), null);
        //            }
        //        }
        //        if (member.Mobile != null && member.Mobile.Trim() != string.Empty)
        //        {
        //            member.MemberId = GetMemberUID();
        //            member.OwnerId = ownerid;
        //            memberList.Add(member);
        //        }

        //    }
        //    return memberList;
        //}

        ///// <summary>
        ///// 检查用户是否拥有指定客户分组权限
        ///// </summary>
        ///// <param name="groupID"></param>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public bool CheckGroupIDAndUser(string groupID, string userID)
        //{

        //    if (string.IsNullOrWhiteSpace(groupID))
        //        return false;

        //    if (groupID != "0")
        //    {
        //        if (GetCount<Model.MemberGroupInfo>(string.Format(" GroupID = '{0}' AND UserID = '{1}'", groupID, userID)) < 1)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //public List<Model.MemberInfo> GetMemberListFromExcel(string fullName, string userID)
        //{
        //    List<Model.MemberInfo> memberList = new List<Model.MemberInfo>();

        //    try
        //    {
        //        List<DataSet> dsList = Common.OfficeDataOp.ExcelToDataSetList(fullName);
        //        foreach (DataSet ds in dsList)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                bool tmpInsert = false;
        //                Model.MemberInfo member = new Model.MemberInfo();
        //                Type t = member.GetType();
        //                PropertyInfo[] properties = t.GetProperties();
        //                foreach (PropertyInfo p in properties)
        //                {
        //                    if (ModelAttrToExcelColumns.ContainsKey(p.Name) && ds.Tables[0].Columns.Contains(ModelAttrToExcelColumns[p.Name]))
        //                    {
        //                        //Convert.ChangeType(ds.Tables[0].Rows[i][ModelAttrToExcelColumns[p.Name]].ToString(),p.PropertyType)
        //                        string strValue = ds.Tables[0].Rows[i][ModelAttrToExcelColumns[p.Name]].ToString();
        //                        if (string.IsNullOrWhiteSpace(strValue))
        //                            continue;
        //                        strValue = strValue.Trim();
        //                        p.SetValue(member, strValue, null);
        //                        if (!tmpInsert)
        //                            tmpInsert = true;
        //                    }

        //                    //if (member.Mobile != null && member.Mobile.Trim() != string.Empty)
        //                    //{
        //                    //    string tmp = ds.Tables[0].Rows[i][2].ToString();
        //                    //    if (tmp != "")
        //                    //    {

        //                    //    }
        //                    //}
        //                }

        //                //if (member.Mobile != null && member.Mobile.Trim() != string.Empty)
        //                //{
        //                //    member.MemberID = GetGUID(TransacType.MemberAdd);
        //                //    member.UserID = userID;
        //                //    memberList.Add(member);
        //                //}

        //                if (tmpInsert)
        //                {
        //                    //member.MemberID = GetGUID(TransacType.MemberAdd);

        //                    try
        //                    {
        //                        member.Mobile = Common.StringHelper.RemoveSpace(member.Mobile);
        //                        member.Email = Common.StringHelper.RemoveSpace(member.Email);

        //                        member.UserID = userID;
        //                        memberList.Add(member);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        throw ex;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return memberList;
        //}


        //public int ImportMemberList(List<ZentCloud.BLLJIMP.Model.MemberInfo> list, string userID, string groupID)
        //{
        //    int result = 0;
        //    try
        //    {
        //        if (list.Count.Equals(0))
        //            return result;

        //        string basePath = @"\\192.168.1.143\BulkFiles";

        //        string connstr = Common.ConfigHelper.GetConfigString("ConnectionString");

        //        //构造文件内容存入临时文件
        //        string tmpFileName = Guid.NewGuid().ToString() + ".txt";
        //        string tmpFilePath = string.Format("{0}\\{1}", basePath, tmpFileName);

        //        StringBuilder strContent = new StringBuilder();
        //        strContent.Append("Name,Sex,Mobile,Email,QQ,Tel,Company,Title,WeiboID,WeiboScreenName, Address,MemberID,Mobile2,Mobile3,Email2,Email3\n");
        //        foreach (ZentCloud.BLLJIMP.Model.MemberInfo item in list)
        //        {
        //            strContent.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}\n",
        //                    item.Name != null ? item.Name.Replace(",", "，") : "",
        //                    item.Sex != null ? item.Sex.Replace(",", "，") : "",
        //                    item.Mobile != null ? item.Mobile.Replace(",", "，") : "",
        //                    item.Email != null ? item.Email.Replace(",", "，") : "",
        //                    item.QQ != null ? item.QQ.Replace(",", "，") : "",
        //                    item.Tel != null ? item.Tel.Replace(",", "，") : "",
        //                    item.Company != null ? item.Company.Replace(",", "，") : "",
        //                    item.Title != null ? item.Title.Replace(",", "，") : "",
        //                    item.WeiboID != null ? item.WeiboID.Replace(",", "，") : "",
        //                    item.WeiboScreenName != null ? item.WeiboScreenName.Replace(",", "，") : "",
        //                    item.Address != null ? item.Address.Replace(",", "，") : "",
        //                    GetGUID(TransacType.MemberAdd),//item.MemberID
        //                    item.Mobile2 != null ? item.Mobile2.Replace(",", "，") : "",
        //                    item.Mobile3 != null ? item.Mobile3.Replace(",", "，") : "",
        //                    item.Email2 != null ? item.Email2.Replace(",", "，") : "",
        //                    item.Email3 != null ? item.Email3.Replace(",", "，") : ""
        //                );
        //        }

        //        using (StreamWriter sw = new StreamWriter(tmpFilePath, false, Encoding.GetEncoding("gb2312")))
        //        {
        //            sw.Write(strContent.ToString());
        //        }

        //        //导入数据
        //        Common.ImportDataHelper impHelper = new Common.ImportDataHelper();

        //        result = impHelper.BulkImport(
        //            connstr,
        //            @"C:\BulkFiles\" + tmpFileName,
        //            @"C:\BulkFiles\MemberFormat.xml",
        //            "ZCJ_MemberInfo",
        //            " MemberID,Name,Sex,Mobile,Email,QQ,Tel,Company,Title,WeiboID,WeiboScreenName, Address,MemberType,UserID,GroupID,Mobile2,Mobile3,Email2,Email3",
        //            string.Format("MemberID,Name,Sex,Mobile,Email,QQ,Tel,Company,Title,WeiboID,WeiboScreenName, Address,0,'{0}','{1}',Mobile2,Mobile3,Email2,Email3", userID, groupID),
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
        ///// 根据会员集合创建datatable
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public DataTable CreateMemberDataTableByList(List<ZentCloud.BLLJIMP.Model.MemberInfo> list)
        //{
        //    DataTable dt = new DataTable();

        //    foreach (var item in ModelAttrToExcelColumns)
        //    {
        //        dt.Columns.Add(item.Value);
        //    }

        //    foreach (var i in list)
        //    {
        //        DataRow dr = dt.NewRow();
        //        foreach (var j in i.GetType().GetProperties())//利用反射赋值
        //        {
        //            object value = j.GetValue(i, null);
        //            if (ModelAttrToExcelColumns.Keys.Contains(j.Name))
        //            {
        //                dr[ModelAttrToExcelColumns[j.Name]] = value;
        //            }
        //        }
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //public static Dictionary<string, string> ModelAttrToExcelColumns = new Dictionary<string, string>()
        //{
        //    {"Name","姓名"},
        //    {"Sex","性别"},
        //    {"Mobile","手机"},
        //    {"Email","电子邮件"},
        //    {"QQ","QQ"},
        //    {"Tel","电话"},
        //    {"Company","公司"},
        //    {"Title","职务"},
        //    {"WeiboID","微博ID"},
        //    {"WeiboScreenName","微博昵称"},
        //    {"Address","地址"},
        //    {"Email2","电子邮件2"},
        //    {"Email3","电子邮件3"},
        //    {"Mobile2","手机2"},
        //    {"Mobile3","手机3"}
        //};

        public string GetMemberWhere(string websiteOwner, string keyWord, string tagName, string haveTrueName,
            string haveWxNickNameAndTrueName, string isFans, string isReg, string isDisOnLineUser, string isDisOffLineUser, string isPhoneReg, string isName,
            string isPhone, string isEmail, string isWxnickName, string isMember, string userAutoId, string isOrAnd, string userType = "", 
            string noDistributionOwner="", string isApp="")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner = '{0}' And UserId !='{0}' And ISNULL(PermissionGroupID,'') = ''  ", websiteOwner);

            if (!string.IsNullOrEmpty(userAutoId))//用户自动编号
            {
                sbWhere.AppendFormat(" And AutoId={0} ", userAutoId);
            }
            if (!string.IsNullOrEmpty(userType))
            {
                sbWhere.AppendFormat(" And UserType={0} ", userType);
            }
            if (!string.IsNullOrEmpty(noDistributionOwner) && noDistributionOwner == "1")//查出不是分销员
            {
                sbWhere.AppendFormat(" AND MemberLevel=0 ");
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                //sbWhere.AppendFormat("And ( UserID  like'%{0}%' ", keyWord);
                sbWhere.AppendFormat("And ( ");
                sbWhere.AppendFormat(" TrueName  like'{0}%' ", keyWord);
                sbWhere.AppendFormat(" OR Company  like'{0}%' ", keyWord);
                //sbWhere.AppendFormat("OR Postion  like'{0}%' ", keyWord);
                sbWhere.AppendFormat(" OR Phone  like'{0}%' ", keyWord);
                sbWhere.AppendFormat(" OR WXOpenId  ='{0}' ", keyWord);
                int tmpAutoId = 0;
                if (int.TryParse(keyWord,out tmpAutoId))
                {
                    sbWhere.AppendFormat(" OR AutoId  = {0} ", tmpAutoId);
                }
                
                if (keyWord.Contains(","))//逗号分隔
                {
                    foreach (var item in keyWord.Split(','))
                    {
                        sbWhere.AppendFormat(" OR WXProvince  like'{0}%' ", item);
                        sbWhere.AppendFormat(" OR WXCity  like'{0}%' ", item);
                        sbWhere.AppendFormat(" OR TrueName  like'{0}%' ", item);

                    }
                }
                else
                {
                    sbWhere.AppendFormat(" OR WXProvince  like'{0}%' ", keyWord);
                    sbWhere.AppendFormat(" OR WXCity  like'{0}%' ", keyWord);
                }
                sbWhere.AppendFormat("OR WXNickName  like'%{0}%' )", keyWord);
                //sbWhere.AppendFormat("OR Email  like'{0}%' )", keyWord);
            }

            #region 标签搜索
            if (!string.IsNullOrEmpty(tagName))
            {
                List<string> tagNameArray = tagName.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                sbWhere.AppendFormat(" AND( ");
                for (int i = 0; i < tagNameArray.Count; i++)
                {
                    if (i == 0)
                    {
                        sbWhere.AppendFormat(" TagName like '%{0}%' ", tagNameArray[i]);
                    }
                    else
                    {
                        sbWhere.AppendFormat(" OR TagName like '%{0}%' ", tagNameArray[i]);
                    }
                }
                sbWhere.AppendFormat(") ");
            }
            #endregion
            
            

            StringBuilder advancedQuery = new StringBuilder();

            string advancedQueryOrAnd = isOrAnd == "1" ? "AND" : "OR";



            #region 高级筛选
            if (isOrAnd == "1")
            {
                #region AND查询

                if (!string.IsNullOrEmpty(haveTrueName))
                {
                    advancedQuery.AppendFormat(" {0} TrueName > ''", advancedQueryOrAnd);
                }

                //if (!string.IsNullOrEmpty(isDisOffLineUser))//业务分销会员
                //{
                //    advancedQuery.AppendFormat(" {0} DistributionOffLinePreUserId > ''", advancedQueryOrAnd);
                //}

                if (!string.IsNullOrEmpty(isDisOnLineUser))//商城分销会员
                {
                    advancedQuery.AppendFormat(" {0} DistributionOwner > ''", advancedQueryOrAnd);
                }
                if (!string.IsNullOrEmpty(isFans))//粉丝
                {
                    advancedQuery.AppendFormat(" {0} IsWeixinFollower={1} ", advancedQueryOrAnd, isFans);
                }
                if (!string.IsNullOrEmpty(isReg))//注册会员
                {
                    advancedQuery.AppendFormat(" {0} (TrueName>'' or WXNickname>'')", advancedQueryOrAnd);
                }
                if (!string.IsNullOrEmpty(isPhoneReg))//是否手机认证会员
                {
                    advancedQuery.AppendFormat(" {0} (Phone > '' And IsPhoneVerify=1)", advancedQueryOrAnd);
                }
                if (!string.IsNullOrEmpty(isName))//姓名
                {
                    advancedQuery.AppendFormat(" {0} TrueName>''  ", advancedQueryOrAnd);
                }
                if (!string.IsNullOrEmpty(isPhone))//手机
                {
                    advancedQuery.AppendFormat(" {0} Phone>'' ", advancedQueryOrAnd);
                }
                if (!string.IsNullOrEmpty(isEmail))//邮箱
                {
                    advancedQuery.AppendFormat(" {0} Email>'' ", advancedQueryOrAnd);
                }
                if (!string.IsNullOrEmpty(isWxnickName))//昵称
                {
                    advancedQuery.AppendFormat(" {0} WxnickName>'' ", advancedQueryOrAnd);
                }

                if (!string.IsNullOrEmpty(haveWxNickNameAndTrueName))
                {
                    advancedQuery.AppendFormat(" AND TrueName > '' And WXNickname > ''");
                }
                if (!string.IsNullOrEmpty(isMember))//昵称
                {
                    CompanyWebsite_Config nWebsiteConfig = GetByKey<CompanyWebsite_Config>("WebsiteOwner", websiteOwner);
                    int memberStandard = nWebsiteConfig == null ? 0 : nWebsiteConfig.MemberStandard;
                    string memberStandardFields = "";
                    if (memberStandard == 2 || memberStandard == 3)
                    {
                        List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo");
                        if (listFieldList.Count > 0) memberStandardFields = ZentCloud.Common.MyStringHelper.ListToStr(listFieldList.Select(p => p.Field).ToList(), "", ",");
                    }
                    List<string> memberStandardFieldList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(memberStandardFields))
                    {
                        memberStandardFieldList = memberStandardFields.Split(',').Where(p => p.Trim().Equals("")).ToList();
                    }

                    StringBuilder strIsMember = new StringBuilder();

                    if (memberStandard == 1)
                    {
                        strIsMember.AppendFormat(" AND Phone > '' ");
                        strIsMember.AppendFormat(" AND IsPhoneVerify = 1 ");
                    }
                    else if (memberStandard == 2 || memberStandard == 3)
                    {
                        strIsMember.AppendFormat(" AND IsPhoneVerify = 1 ");
                        if (memberStandard == 3)
                            strIsMember.AppendFormat(" AND MemberApplyStatus = 9 ");

                        foreach (string field in memberStandardFieldList)
                        {
                            strIsMember.AppendFormat(" AND {0} > '' ", field);
                        }
                    }
                    strIsMember.AppendFormat(" And AccessLevel>0 ");

                    if (strIsMember.Length > 0)
                    {
                        advancedQuery.AppendFormat(" {0} (** ", advancedQueryOrAnd);
                        advancedQuery.Append(strIsMember.ToString().TrimStart());
                        advancedQuery.Append(" )");
                    }
                }
                if (!string.IsNullOrWhiteSpace(isApp))
                {
                    advancedQuery.AppendFormat(" {0} EXISTS (SELECT 1 FROM [ZCJ_AppPushClient] WHERE UserID=[ZCJ_UserInfo].UserID AND WebsiteOwner='{1}') ", 
                        advancedQueryOrAnd, websiteOwner);
                }
                if (advancedQuery.Length > 0)
                {
                    sbWhere.Append(" And (** ");
                    sbWhere.Append(advancedQuery.ToString().TrimStart());
                    sbWhere.Append(" )");
                } 
                #endregion

            }
            else
            {
                //or查询

                //有姓名  注册会员
                if (!string.IsNullOrEmpty(haveTrueName) || !string.IsNullOrEmpty(isReg) || !string.IsNullOrEmpty(isName))
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  [TrueName] > '' And WebsiteOwner='{0}' ", websiteOwner);
                }

                //昵称
                if (!string.IsNullOrEmpty(isWxnickName))
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  [WxnickName] > '' And WebsiteOwner='{0}' ", websiteOwner);
                }

                //if (!string.IsNullOrEmpty(haveWxNickNameAndTrueName))
                //{
                //    advancedQuery.AppendFormat(" AND TrueName > '' And WXNickname > ''");
                //}

                //商城分销会员
                if (!string.IsNullOrEmpty(isDisOnLineUser))
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  [DistributionOwner] > '' And WebsiteOwner='{0}' ", websiteOwner);
                }


                if (!string.IsNullOrEmpty(isFans) && isFans == "1")//粉丝
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  [IsWeixinFollower] = 1  And WebsiteOwner='{0}' ", websiteOwner);
                }

                if (!string.IsNullOrEmpty(isPhoneReg))//是否手机认证会员
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  [Phone] > '' And [IsPhoneVerify] = 1  And WebsiteOwner='{0}' ", websiteOwner);
                }

                if (!string.IsNullOrEmpty(isPhone))//手机
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  [Phone] > ''  And WebsiteOwner='{0}' ", websiteOwner);
                }

                if (!string.IsNullOrEmpty(isEmail))//邮箱
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  [Email] > ''  And WebsiteOwner='{0}' ", websiteOwner);
                }

                if (!string.IsNullOrEmpty(isMember))//昵称
                {
                    CompanyWebsite_Config nWebsiteConfig = GetByKey<CompanyWebsite_Config>("WebsiteOwner", websiteOwner);
                    int memberStandard = nWebsiteConfig == null ? 0 : nWebsiteConfig.MemberStandard;
                    string memberStandardFields = "";
                    if (memberStandard == 2 || memberStandard == 3)
                    {
                        List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo");
                        if (listFieldList.Count > 0) memberStandardFields = ZentCloud.Common.MyStringHelper.ListToStr(listFieldList.Select(p => p.Field).ToList(), "", ",");
                    }
                    List<string> memberStandardFieldList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(memberStandardFields))
                    {
                        memberStandardFieldList = memberStandardFields.Split(',').Where(p => p.Trim().Equals("")).ToList();
                    }

                    StringBuilder strIsMember = new StringBuilder();

                    if (memberStandard == 1)
                    {
                        strIsMember.AppendFormat(" AND Phone > '' ");
                        strIsMember.AppendFormat(" AND IsPhoneVerify = 1 ");
                    }
                    else if (memberStandard == 2 || memberStandard == 3)
                    {
                        strIsMember.AppendFormat(" AND IsPhoneVerify = 1 ");
                        if (memberStandard == 3)
                            strIsMember.AppendFormat(" AND MemberApplyStatus = 9 ");

                        foreach (string field in memberStandardFieldList)
                        {
                            strIsMember.AppendFormat(" AND {0} > '' ", field);
                        }
                    }
                    strIsMember.AppendFormat(" And AccessLevel>0 ");

                    if (strIsMember.Length > 0)
                    {
                        advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE  WebsiteOwner='{1}' {0} ",
                                strIsMember.ToString(), websiteOwner );
                    }

                }
                if (!string.IsNullOrEmpty(isApp))//有登录App
                {
                    advancedQuery.AppendFormat(" UNION SELECT AutoId  FROM [ZCJ_UserInfo]  WHERE WebsiteOwner='{0}' And EXISTS (SELECT 1 FROM [ZCJ_AppPushClient] WHERE UserID=[ZCJ_UserInfo].UserID AND WebsiteOwner='{0}') ", websiteOwner);
                }
                if (advancedQuery.Length > 0)
                {
                    sbWhere.AppendFormat(" AND AutoId IN ( {0} ) ", advancedQuery.ToString().Substring(6));
                }

            }
            #endregion

            string result = sbWhere.ToString().ToUpper();

            result = result.Replace("  "," ").Replace("(** AND", " ( ").Replace("(** OR", " ( ");

            return result;
        }

        public List<UserInfo> GetMemberList(int rows, int page, string mapping_type, List<string> defFields, string websiteOwner, string keyWord, string tagName, string haveTrueName,
            string haveWxNickNameAndTrueName, string isFans, string isReg, string isDisOnLineUser, string isDisOffLineUser, string isPhoneReg, string isName,
            string isPhone, string isEmail, string isWxnickName, string isMember, string userAutoId, string isOrAnd, string userType = "", string noDistributionOwner = "", 
            string isApp="")
        {
            string sqlWhere = GetMemberWhere(websiteOwner, keyWord, tagName, haveTrueName, haveWxNickNameAndTrueName, isFans,
                isReg, isDisOnLineUser, isDisOffLineUser, isPhoneReg, isName, isPhone, isEmail, isWxnickName, isMember, userAutoId, isOrAnd, userType,
                noDistributionOwner, isApp);

            List<TableFieldMapping> formField = bllTableFieldMap.GetTableFieldMapByWebsite(websiteOwner, "ZCJ_UserInfo", null, null, mapping_type, "AutoId,Field");
            defFields.AddRange(formField.Where(p => !defFields.Contains(p.Field)).Select(p => p.Field).Distinct());
            string fields = ZentCloud.Common.MyStringHelper.ListToStr(defFields, "", ",");

            return GetColList<UserInfo>(rows, page, sqlWhere, "AutoID DESC", fields);
        }


        public int GetMemberCount(string websiteOwner, string keyWord, string tagName, string haveTrueName,
            string haveWxNickNameAndTrueName, string isFans, string isReg, string isDisOnLineUser, string isDisOffLineUser, string isPhoneReg, string isName,
            string isPhone, string isEmail, string isWxnickName, string isMember, string userAutoId,string isOrAnd,string userType="",string noDistribution="",
            string isApp="")
        {
            return GetCount<UserInfo>(GetMemberWhere(websiteOwner, keyWord, tagName, haveTrueName, haveWxNickNameAndTrueName, isFans,
                isReg, isDisOnLineUser, isDisOffLineUser, isPhoneReg, isName, isPhone, isEmail, isWxnickName, isMember, userAutoId, isOrAnd,
                userType, noDistribution, isApp));
        }

        /// <summary>
        /// 拼查询条件（颂和会员）
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="member"></param>
        /// <param name="minLevel"></param>
        /// <param name="distributionOwner"></param>
        /// <param name="regUserId"></param>
        /// <param name="bill">0实单 1空单</param>
        /// <param name="apply">0还未通过审核 1通过审核 </param>
        /// <param name="hasImg">0未传执照 1已传执照</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public string GetShMemberWhere(string websiteOwner, string minLevel, string member, string distributionOwners, string regUserIds,
            string bill, string apply, string hasImg, string start, string end, string level_num, string apply_cancel = "", 
            string is_cancel = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" websiteOwner='{0}' ", websiteOwner);
            //sbWhere.AppendFormat(" and UserID In ({0}) ", "'ZYUser201608107FA3D217DC7C4780919F03FFC80CB7A5','ZYUser20160820A6C954D705A24AC48B608FC6A5DDC106','ZYUser20160810DAE2EC97A634448F9765B4A27841AA50','ZYUser201608101B3538E09D604471BBA03656B9B1F112','ZYUser20160810BC10B4E467584436A45D3E32B7ABD6D8','ZYUser20160810EB23BE6138324BA2AED2748489EC1149','ZYUser2016081037F8FBE2D3AE49F8860ED380E95CDC23','ZYUser201608106271185AC8994DCEB13335A19FAAC2AF','ZYUser201608100E37FD2EDCC24067B0FD9F636CA03C3E','ZYUser201610281630D3F531064768A9CAB8A819DBE1E8','ZYUser201608135A78D072770447CCAC056F8C7982187D','ZYUser201608102EFBDF9A82E14CCC92D2D0B1135580B3','ZYUser201608109D45225208FA4F57AE2CB4161AFC5B2D','ZYUser20160810425FB7887B234975BCF73855873F64CF','ZYUser201608102C8404444D26466699AEBD4E120A7DD9','ZYUser201608105E847336EE89432ABB5D4ADEF1C63ECD','ZYUser201608109F9DD39D4C2D40AA8EF4B07358BF5074','ZYUser20160810DE134F6F01D94C558B3D128AA7D29562','ZYUser20160810A757CC2661BB43A8829EC975F65AE3D6','ZYUser2016081050C74121F3634CAA9DA02EA7D286746E','ZYUser201608224C0D9854D40E4CCB8F370B7AD1608A8F','ZYUser201608201B480B1E983849A88401CB225BA84E47','ZYUser201608200CA24FFDBD3346BA98AD60134A52A2A9','ZYUser20160820C41CD204E71B4070BAC082A9E53C9CD8','ZYUser20160820D1D98D1C2C0045E8A1C3BF2D66F1F9BC','ZYUser2016082085FC5D8B2F1A4280A63F393BB981DD69','ZYUser20160820B62A48E135B54DC3AE6F5EBF75A401BB','ZYUser201608103381A83658474B148ACFF3BB1E7C5822','ZYUser201608107485B28A980A4D8A8F380F3657B5AA90','ZYUser201608100760BAB58DA848CFBAC7019DB33AD96C','ZYUser2016081098067C109F3B4D80A53E4718EAA425BF','ZYUser2016101668F5F2CF435E4CB4AA2EE22F6E64CBD7','ZYUser201609012AC0A45C74F4480697C6C4526E17F30C','ZYUser2016082160EDFA15E5FE4BE984F14E08275849A1','ZYUser201608205E385BCB91234B148BA1A9378B6675AC','ZYUser20160820025BB83A8F1D4F199CB232E35B91A485','ZYUser20160820D2766FCE5045419FB91FE3F15D629999','ZYUser20160820E79058F43D3C43B7A5BDE5C3C639B52A','ZYUser201608203C04B34A35E64EB8A2AC175B05FC32C6','ZYUser20160820623A809085D245B982CB61FA5EC77A3B','ZYUser20160810553D525AA92A479D86674C48E00E85F1','ZYUser2016081079A38ECE71164FC8824AA2E79CF7F45B','ZYUser20160825DC33208012CA46508839869329C689A5','ZYUser201608211FDB2039B8BE4239AC0BBF221F30BDB7','ZYUser201608217FE4576C6D9A477C8C26F4B5245BE051','ZYUser20160821DD3EE93E6E58498D947AB37F7714C1E7','ZYUser20160820C3E5BD9EB88B4F6A8379C8E99B28168D','ZYUser20160820EA40EA35E0D74E7BB24346E0F4AAF4B9','ZYUser20160820AF64EC6D35C2460192263FBFD722DC2A','ZYUser20160820AF0C8A9494764784B5B4C4D0B57EC96C','ZYUser2016082002F07F87911448F293E85FA04F9241A2','ZYUser20160820C089717273F64EDA817247017D8AE1C4','ZYUser20160820D59AEFE5F3C8487C9F1817F420643260','ZYUser201608205E0BA7F401DA481B9030DF0063DBD0DB','ZYUser20160820ACFDA6BE389B4D96819CFF93FE6C8E76','ZYUser20160820382E9D5BAEFF4348AFAD1254119184C8','ZYUser201608300C36B2318FE94B92BF7014E91951D813','ZYUser2016082577CD6F4D8578479CA54137A5F8019A9D','ZYUser2016082180C1E73B848A48B583E6CE2C3313A0AE','ZYUser20161031CAB876A3169A4092AE4E6DF1D2D69292','ZYUser20161028CC4945023B964B4F88D7563597F54540','ZYUser2016083046902158A7C44D9AB6E673FCB69E6277','ZYUser201608281B58FD6D1B7F494CAD313345BFD86721','ZYUser20160914B74A65BAB4B5447D9C03FD9B8C9B0FA9','ZYUser20160914BF42B39943D64A2C942491F89C44D6DD','ZYUser2016091200FF55AC1E08438EAC384A60309D8DF6','ZYUser201609085F895A83DE534FE899F18514CFBBD5A5','ZYUser2016090798574B87D4F94D8E995EEB808240CBAF','ZYUser20160907ADEBF29107DD48A28FC9EB056943E245','ZYUser2016083165B9DB45D8F64350863FA41D0CB62B0D','ZYUser201611130DD86D75FCAE475D85B1698744D127D4','ZYUser20161112D160497294F34556B0FD36C61660ADF1','ZYUser201611023AD9CD3B46B04314A9EF2E23F0095CA0','ZYUser20161028EEEBA797ADA94F67B5997AB66685BD44','ZYUser20161010A663D1F3F89F45FD981A04144CCB6DAF','ZYUser201610106F2E9FB0FFDF40D1955B3D39A771A88A','ZYUser2016093016BB3950064B4A6D944575D15A35C2DB','ZYUser2016091965E860698CB146DFBA4BA3F976D496BF','ZYUser2016091545AB3161BCA742629DD7D249EEA8EEE2','ZYUser2016091425A1A29AC04F4007888C477A9DDDDC64','ZYUser20160914BE85AAFA56B248DCA6C1BFE101DABDA8','ZYUser201609146D16DA5851E44F2FBF3CDE2E5ECB90AC','ZYUser201609128020E8A95A764546B79BBCED16ED2F9A','ZYUser201608317A37A68623F540CA972F757E17E3CDE7','ZYUser20170117A5CC04DCB5744E1295BAC6D384E9A76C','ZYUser20161130EBEACA2F35E7460EAB41F539D02B1411','ZYUser2016111473934E18AFB1450BADC28186CAD1435E','ZYUser20161112D9FBD54B9C6E49D38DD06BDF1B638C46','ZYUser20161112B4A98126D2144B05882D644932D61F70','ZYUser201610170C0BBDE3E5F045838D44F0F4B9C83BAB','ZYUser20160920213F977BB6D44648A5D43309EC6E2C79','ZYUser20160920E3890EDA3E584EAA88A9BB42EDA86BEA','ZYUser20160914865F43468A6C4433AE414E0714A2D7B7','ZYUser201609143A6AF1928714496F9696BF691D7A290C','ZYUser20161114AA443FBCB35C4F199AAF30F04E3BEF15','ZYUser20161030425B879E6C074676BDC04454D763EC61','ZYUser201610303DF8440E3ECF426CBE043BF4D7B93F81','ZYUser20161028BB03556BB7504300981BAD0B0084F51B','ZYUser2016102036131AB630754DF89D7DABE721D8E844','ZYUser20161014A48989ABBC6C4156A8CA317067EF0D31','ZYUser2016092993701FA2F4444578B5686E7BC6B47B3F','ZYUser20160929EBAA6503C4D94C608127DC3BBB3A9D37','ZYUser20170106BB7E9D7AC7B74F0BA44E9F91B9D5C3AF','ZYUser20161123BA7D4E0EF2AD4CA9A367ECA30A6DC613','ZYUser201611129438EF5C8A6648D39C8F04F64A8E4532','ZYUser201611091D361DD8CD5B4664A1ACE1E12B2941F7','ZYUser20161020AF750726FDC544AAB09F2F5D7F6C274F'");
            //sbWhere.AppendFormat(" and UserID In ({0}) ", "'ZYUser201610203A0CDB6007C8493492673FEA7CF55565','ZYUser20161107C5850B2036564880968AEFD434247BB0','ZYUser2016101335593BC3D9304A34B1D0A85DD947BAEB','ZYUser20161107C0CEC7689C57447290DBE28001C22FE5','ZYUser201610135978FDFEF71D4BEEB3E3BB0E5C20D92B','ZYUser20161107FECE444E6D914998BB0E055E59B53A29','ZYUser201611075B798EB27BD44CD6A9623946929AABA5','ZYUser201610130C810537C27E409483286AB05F59BDE0','ZYUser201610134372E80899FF4D7CB45706092142CDA5','ZYUser2016120308AEEFC6A38B436788DA5954A43E563C','ZYUser201612019AA64F4693D544E786C814390001F182','ZYUser2016112964213BEA77BB4238874506BBC7207F34','ZYUser20161013F85606DAE8F04F4BB9982448E59F72CA','ZYUser20170114DB8CFDD6215C45F8B822DE3CE0AF67DF','ZYUser2017010449717917692442A68E4E869C2C446E28','ZYUser20161221D2A5A7C2D47C4B1C817855D75E847911','ZYUser201612179BDB594311544B4488ABF48CD843644A','ZYUser2016112999B95E4E8C7C494AB93160622873C00B','ZYUser20161013945C8F480B394958AEA63691815A99B0','WXUser51aa031b-2ef4-4559-968f-01ff663c8392','ZYUser2017010516D6897E391C4695B2B4962F0F566779','ZYUser20161230D5E6857E3F4C46E6969F86390E9180CB','ZYUser20161229E1F651FBB5BA4FE684A30C8FDDD92163','ZYUser201612019D078FBBFC01420190821BB4DD8D8748','ZYUser201612303A3827BE069A424895C54749E059CCA2','ZYUser201612299A13467CF66C4B0E84647A0A466E184E','ZYUser201612249FF911E98305496B8FC84F105F2F4517','ZYUser2016120685FD9429E1174668A062850113FE5EE5','ZYUser20170104F3A0945C2373418BB1D859E80B5BB4DB','ZYUser201701045A902A2FD6FC4DCEB472E3832E1D7EEE','ZYUser2016122932ED490CAF0B4346BDF1CF4A10693BB5','ZYUser2016120610236DA17DAD4E499C91C7E243494DAE','ZYUser20170213DA8B95F3234A49CA8C942F1FFD2B0CE9','ZYUser201701108FC08CF6261948CB86B7FFD28C0F080F','ZYUser201612175585120AE943427ABD2FF4107C13ADEF','ZYUser20170104D2F140C7E5724B61BED815DCE4516B75','ZYUser20161226F6F0BF961A184112BC4610C5D543320D','ZYUser20170224852A56FEC3C44D2695281EB856AD5B3B','ZYUser2017010473F2B8D1905043EFA6FF82637DFE0861','ZYUser20161229A416012456D24EEB9788EB06DABEA48B','ZYUser20170225670939BCF45246A6B1D7DDB827AE5961','ZYUser20170224E7E6707A19D143CE87F876FA7618DD73','ZYUser20170104487EE5B1284B47EE979E0FE0A6782C40','ZYUser2017010475BA324E9B3D4DDCB1E8DAF0ABF93D23','ZYUser201702082104A34EE187415B8D56EF10A01E7A0D','ZYUser201701140DA68DEF3B7A4DE6959C55E8C2CCEA97','ZYUser20170104DC675AB25AC540999FC97D65FD42BFF1','ZYUser2017010474B63526259E46C4B9AC249795EB24AA','ZYUser201702252DA5B54BC50F4D0EBFC1266CD3823C65','ZYUser201702163B8D4AB7AB244560A8D8CBB80BD6AC96','ZYUser20170114CD8734DEB3EF4C5AA3E5A48641AD92EF','ZYUser2017010567CD91125FF743EFBF3D18E98F91DA15','ZYUser201701044C7E21F454D341B4AE2EDEB5F68A8D0A','ZYUser201702258113909CF3F541179782B6226593F1F9','WXUser6659c62f-bfde-4383-b92d-e433c7c9f12f','WXUser287955ed-a45f-4cd6-904c-00eed956fec8','ZYUser20170118F00521E590CD4F1F98C11FFE4743E5AD','WXUser60b4ddc0-a777-4bd0-bae6-6ec0db42ba2c','WXUser21c2e4c2-cbf5-4203-8cc0-7de654d48fca','WXUsere03cbd03-abc1-429d-807f-d26b093226c4','WXUser46744f16-71dd-4785-9b18-6d879f65f26c','ZYUser2017011873434A112A194B538A3206D18410F849','WXUserc526bfa0-2221-4191-a609-d79632f73cef','ZYUser20170208194E597C39DD4A3DB1663C6E5B3A82FC','ZYUser2017011838AC538B2C0B46AF99731C02690DEE63','ZYUser2017022545EA7D88ADCD4BCAAE40D2E2587306EB','WXUsere6448537-9c1e-4ee6-8b4b-eb339e3525f2','ZYUser201702083FD0CFB24F8644F984BDA3E1DDFCB3B6','ZYUser20170119E418B66BBFDC4F888CA525AAF4C71511','ZYUser201702209A4B24E1F64B454E9A481D052E973393','WXUsere9dc8f1c-1974-41f0-9b79-1598e1ea28b7','ZYUser20170119444730B79DC2486CBB2B4A5FAF76098F','ZYUser201702225087DA7094BB4B5EB1A55B4ED1BDD045','ZYUser20170216D77EE0F488684DFF84FC287637A8C6C3','ZYUser2017011931B1CE2923C7492BBA8D2BF2D0407B99','WXUser195717f1-64b4-41f8-956f-cd6204134354','ZYUser201701193F19CE9744144BB5880E06DF619D4521','ZYUser20170224FFA48A12C1DD414A8E544E72EB30173B','ZYUser20170119DD7515DC220F492C82A99E078DB9ED6E','ZYUser20170119E2ACF5662F274B1A9A3971EB3BD39D4F','WXUser96849a2f-b8b3-4029-830d-570f59ad9554','ZYUser20170123204E4E03EEE048049B9CC01840D1BB0B','ZYUser20170217550CA1062FE3473C98D64D69109B1BBB'");
            
            
            if (!string.IsNullOrWhiteSpace(member)) { 
                int id =0;
                if (int.TryParse(member, out id))
                {
                    sbWhere.AppendFormat(" And ( AutoID={0} Or Phone='{0}') ", member);
                }
                else
                {
                    sbWhere.AppendFormat(" And (Phone='{0}' Or TrueName Like '{0}%') ", member);
                }
            }
            if (!string.IsNullOrWhiteSpace(level_num)) { 
                sbWhere.AppendFormat(" And MemberLevel={0} ", level_num);
            }

            if (apply_cancel == "1" && is_cancel == "1")
            {
                sbWhere.AppendFormat(" And IsDisable=1 ");
            }
            else if (apply_cancel == "1")
            {
                if (!string.IsNullOrWhiteSpace(minLevel))
                {
                    sbWhere.AppendFormat(" And IsDisable=1 And MemberLevel>={0} ", minLevel);
                }
                else
                {
                    sbWhere.AppendFormat(" And IsDisable=1 And MemberLevel>0  ");
                }
            }
            else if (is_cancel == "1")
            {
                sbWhere.AppendFormat(" And IsDisable=1 And MemberLevel=0  ");
            }
            else if (string.IsNullOrWhiteSpace(level_num) && !string.IsNullOrWhiteSpace(minLevel))
            {
                sbWhere.AppendFormat(" And MemberLevel>={0} ", minLevel);
            }

            if (!string.IsNullOrWhiteSpace(distributionOwners)) sbWhere.AppendFormat(" And DistributionOwner In ({0}) ", "'"+distributionOwners.Replace(",","','")+"'");
            if (!string.IsNullOrWhiteSpace(regUserIds)) sbWhere.AppendFormat(" And RegUserID In ({0}) ", "'" + regUserIds.Replace(",", "','") + "'");
            if (!string.IsNullOrWhiteSpace(bill)) {
                if (bill == "1") sbWhere.AppendFormat(" And EmptyBill=1 ");
                else if (bill == "0") sbWhere.AppendFormat(" And IsNull(EmptyBill,0)=0 ");
            }
            if (!string.IsNullOrWhiteSpace(apply))
            {
                if (apply == "1") sbWhere.AppendFormat(" And MemberApplyStatus=9 ");
                else if (apply == "0") sbWhere.AppendFormat(" And IsNull(MemberApplyStatus,0)!=9 ");
            }
            if (!string.IsNullOrWhiteSpace(hasImg))
            {
                if (hasImg == "1") sbWhere.AppendFormat(" And IsNull(Ex1,'')+IsNull(Ex2,'')+IsNull(Ex3,'')+IsNull(Ex4,'')+IsNull(Ex5,'')!='' ");
                else if (hasImg == "0") sbWhere.AppendFormat(" And IsNull(Ex1,'')+IsNull(Ex2,'')+IsNull(Ex3,'')+IsNull(Ex4,'')+IsNull(Ex5,'')='' ");
            }
            if (!string.IsNullOrWhiteSpace(start)) sbWhere.AppendFormat(" And MemberApplyTime>='{0}' ", start);
            if (!string.IsNullOrWhiteSpace(end)) sbWhere.AppendFormat(" And MemberApplyTime<'{0}' ", end);
            return sbWhere.ToString();
        }
        /// <summary>
        /// 获取会员数量（颂和）
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="member"></param>
        /// <param name="minLevel"></param>
        /// <param name="distributionOwner"></param>
        /// <param name="regUserId"></param>
        /// <param name="bill"></param>
        /// <param name="apply"></param>
        /// <param name="hasImg"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetShMemberCount(string websiteOwner, string minLevel = "", string member = "", string distributionOwners = "", string regUserIds = "",
            string bill = "", string apply = "", string hasImg = "", string start = "", string end = "", string level_num = "", string apply_cancel = "",
            string is_cancel = "")
        {
            return GetCount<UserInfo>(GetShMemberWhere(websiteOwner, minLevel, member, distributionOwners, regUserIds, bill, apply, hasImg,
                start, end, level_num, apply_cancel, is_cancel));
        }
        /// <summary>
        /// 获取会员列表（颂和）
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="minLevel"></param>
        /// <param name="member"></param>
        /// <param name="distributionOwner"></param>
        /// <param name="regUserId"></param>
        /// <param name="bill"></param>
        /// <param name="apply"></param>
        /// <param name="hasImg"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<UserInfo> GetShMemberList(int rows, int page, string websiteOwner, string minLevel = "", string member = "", string distributionOwners = "", string regUserIds = "",
            string bill = "", string apply = "", string hasImg = "", string start = "", string end = "", string level_num = "", string colName = "", string apply_cancel = "",
            string is_cancel = "")
        {
            if (!string.IsNullOrWhiteSpace(colName))
            {
                return GetColList<UserInfo>(rows, page,
                    GetShMemberWhere(websiteOwner, minLevel, member, distributionOwners, regUserIds, bill, apply, hasImg, start, end, level_num, apply_cancel, is_cancel),
                    "MemberApplyTime Desc,AutoID Desc", 
                    colName);
            }
            return GetLit<UserInfo>(rows, page,
                GetShMemberWhere(websiteOwner, minLevel, member, distributionOwners, regUserIds, bill, apply, hasImg, start, end, level_num, apply_cancel, is_cancel),
                "MemberApplyTime Desc,AutoID Desc");
        }
    }
}
