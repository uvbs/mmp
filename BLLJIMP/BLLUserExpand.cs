using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.ModelGen.API.User.Expand;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 用户扩展
    /// </summary>
    public class BLLUserExpand : BLL
    {
        public static Dictionary<string, string> dicTypes = new Dictionary<string, string>() { { "InvoicingInformation", "开票资料" } };
        public static Dictionary<string, List<Field>> dicDefColumns = new Dictionary<string, List<Field>>() { 
            { "InvoicingInformation", 
                new List<Field>(){
                    new Field(){ field="phone",name="会员手机"},
                    new Field(){ field="nickname",name="会员姓名"}
                } 
            } 
        };
        public static Dictionary<string, List<Field>> dicColumns = new Dictionary<string, List<Field>>() { 
            { "InvoicingInformation", 
                new List<Field>(){
                    new Field(){ field="value",mfield="DataValue",name="发票类型"},
                    new Field(){ field="ex1",mfield="Ex1",name="公司名称"},
                    new Field(){ field="ex2",mfield="Ex2",name="信用代码"},
                    new Field(){ field="ex3",mfield="Ex3",name="开户银行"},
                    new Field(){ field="ex4",mfield="Ex4",name="银行帐号"},
                    new Field(){ field="ex5",mfield="Ex5",name="公司注册地址"},
                    new Field(){ field="ex6",mfield="Ex6",name="公司电话"},
                    new Field(){ field="ex7",mfield="Ex7",name="一般纳税人资格证书",type="img"}
                } 
            } 
        };

        public BLLUserExpand()
            : base()
        {

        }

        /// <summary>
        /// 根据参数构造查询语句
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSqlWhereByParm(Enums.UserExpandType rtype, string userId,string websiteOwner="")
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            StringBuilder strWhere = new StringBuilder();

            strWhere.AppendFormat(" DataType = '{0}' ", type);

            if (!string.IsNullOrWhiteSpace(userId))
                strWhere.AppendFormat(" AND  UserId = '{0}' ", userId);
            if (!string.IsNullOrEmpty(websiteOwner))
                strWhere.AppendFormat(" AND  WebsiteOwner = '{0}' ", websiteOwner);

            return strWhere.ToString();
        }


        /// <summary>
        /// 关系是否已存在
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ExistUserExpand(BLLJIMP.Enums.UserExpandType rtype, string userId)
        {
            return GetUserExpand(rtype, userId) != null;
        }


        /// <summary>
        /// 查询关系数量
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserExpandCount(Enums.UserExpandType rtype, string userId)
        {
            var strWhere = GetSqlWhereByParm(rtype, userId);
            var result = GetCount<Model.UserExpand>(strWhere);
            return result;
        }

        /// <summary>
        /// 查询关系数量
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Model.UserExpand GetUserExpand(Enums.UserExpandType rtype, string userId,string websiteOwner="")
        {
            var strWhere = GetSqlWhereByParm(rtype, userId, websiteOwner);
            return Get<Model.UserExpand>(strWhere);
        }

        /// <summary>
        /// 查询关系数量
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserExpandValue(Enums.UserExpandType rtype, string userId)
        {
            var userExpand = GetUserExpand(rtype, userId);
            if (userExpand == null) return null;
            return userExpand.DataValue;
        }
        /// <summary>
        /// 添加扩展
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool AddUserExpand(BLLJIMP.Enums.UserExpandType rtype, string userId, string dataValue, string ex1="", string ex2="",
            string ex3 = "", string ex4 = "", string ex5 = "", string ex6 = "", string ex7 = "", string ex8 = "", string ex9 = "", string ex10 = "")
        {
            Model.UserExpand data = new Model.UserExpand()
            {
                UserId = userId,
                DataValue = dataValue,
                CreateTime = DateTime.Now,
                DataType = CommonPlatform.Helper.EnumStringHelper.ToString(rtype),
                Ex1 = ex1,
                Ex2 = ex2,
                Ex3 = ex3,
                Ex4 = ex4,
                Ex5 = ex5,
                Ex6 = ex6,
                Ex7 = ex7,
                Ex8 = ex8,
                Ex9 = ex9,
                Ex10 = ex10,
                WebsiteOwner=WebsiteOwner
            };
            return Add(data);
        }

        /// <summary>
        /// 更新扩展
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <param name="DataValue"></param>
        /// <returns></returns>
        public bool UpdateUserExpand(BLLJIMP.Enums.UserExpandType rtype, string userId, string dataValue,string ex1 = "",string ex2 = "",string ex3 = "",
            string ex4 = "", string ex5 = "", string ex6 = "", string ex7 = "", string ex8 = "", string ex9 = "", string ex10 = "")
        {
            Model.UserExpand data = GetUserExpand(rtype, userId);
            if (data == null)
                return AddUserExpand(rtype,userId,dataValue,ex1,ex2,ex3,ex4,ex5,ex6,ex7,ex8,ex9,ex10);

            data.DataValue = dataValue;
            data.Ex1 = ex1;
            data.Ex2 = ex2;
            data.Ex3 = ex3;
            data.Ex4 = ex4;
            data.Ex5 = ex5;
            data.Ex6 = ex6;
            data.Ex7 = ex7;
            data.Ex8 = ex8;
            data.Ex9 = ex9;
            data.Ex10 = ex10;
            data.WebsiteOwner = WebsiteOwner;
            return Update(data);
        }

        /// <summary>
        /// 更新扩展
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="userId"></param>
        /// <param name="DataValue"></param>
        /// <returns></returns>
        public bool DeleteUserExpand(UserExpandType rtype, string userId,string websiteOwner="")
        {
            UserExpand data = GetUserExpand(rtype, userId, websiteOwner);
            if (data!=null)
            {
                return Delete(data) > 0;
            }
            return false;
           
        }

        public string GetExpandParam(string type, string userIds)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" DataType = '{0}' ", type);
            if (!string.IsNullOrWhiteSpace(userIds))
            {
                userIds = "'" +userIds.Replace(",", "','") + "'";
                sbSql.AppendFormat(" And UserId In ({0}) ", userIds);
            }
            return sbSql.ToString();
        }
        public int GetExpandListCount(UserExpandType rtype, string userIds)
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            return GetCount<UserExpand>(GetExpandParam(type, userIds));
        }
        public List<UserExpand> GetExpandList(int rows,int page,UserExpandType rtype, string userIds)
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            return GetLit<UserExpand>(rows, page, GetExpandParam(type, userIds));
        }

        public DataTable GetUserExpands(string pms, string userId)
        { 
            StringBuilder sbSql = new StringBuilder();
            pms = "[" + pms.Replace(",","],[") + "]";
            sbSql.AppendFormat(" SELECT [UserId],{0} ",pms);
            sbSql.AppendFormat(" FROM( ");
            sbSql.AppendFormat("    SELECT [UserId],[DataType],[DataValue] ");
            sbSql.AppendFormat("    FROM [ZCJ_UserExpand] ");
            sbSql.AppendFormat("    WHERE [UserId]='{0}' ",userId);
            sbSql.AppendFormat(" ) AS TEMP PIVOT ( MAX([DataValue]) FOR [DataType] "); 
            sbSql.AppendFormat(" IN ({0})) X ",pms);
            DataSet ds = Query(sbSql.ToString());
            return ds.Tables[0];
        }
        
        #region 扩展信息-用户车主信息
        /// <summary>
        /// 设置用户车主信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="carModelId"></param>
        /// <param name="carNumber"></param>
        /// <param name="carNumberTime"></param>
        /// <param name="VIN"></param>
        /// <param name="drivingLicenseType"></param>
        /// <param name="drivingLicenseTime"></param>
        /// <returns></returns>
        public bool SetUserCarOwnerInfo(
            string userId,
            int carModelId,
            string carNumber,
            DateTime? carNumberTime,
            string VIN,
            string drivingLicenseType,
            DateTime? drivingLicenseTime
            )
        {
            /*车主信息：
               ex1 我的车型、
               ex2 车牌号码、
               ex3 vin号、
               ex4 车辆上牌时间（yyyy-MM-dd）、
               ex5 驾照领取时间 (yyyy-MM-dd)、
               ex6 驾照类型
            */
            bool result = false;

            var key = CommonPlatform.Helper.EnumStringHelper.ToString(Enums.UserExpandType.CarOwnerInfo);

            Model.UserExpand data = new Model.UserExpand()
            {
                UserId = userId,
                CreateTime = DateTime.Now,
                Ex1 = carModelId.ToString(),
                Ex2 = carNumber,
                Ex3 = VIN,
                Ex4 = carNumberTime == null ? "" : carNumberTime.Value.ToShortDateString(),
                Ex5 = drivingLicenseTime == null ? "" : drivingLicenseTime.Value.ToShortDateString(),
                Ex6 = drivingLicenseType,
                DataType = key
            };

            var oldData = Get<Model.UserExpand>(string.Format(" UserId = '{0}' AND DataType = '{1}' ",
                    userId,
                    key
                ));

            //查询是否有，有则更新无则新增
            if (oldData != null)
            {
                oldData.Ex1 = data.Ex1;
                oldData.Ex2 = data.Ex2;
                oldData.Ex3 = data.Ex3;
                oldData.Ex4 = data.Ex4;
                oldData.Ex5 = data.Ex5;
                oldData.Ex6 = data.Ex6;

                result = Update(oldData);
            }
            else
            {
                result = Add(data);
            }

            return result;
        }

        /// <summary>
        /// 设置用户车型
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="carModelId"></param>
        /// <returns></returns>
        public bool SetUserCarModel(string userId, int carModelId)
        {
            /*车主信息：
               ex1 我的车型、
               ex2 车牌号码、
               ex3 vin号、
               ex4 车辆上牌时间（yyyy-MM-dd）、
               ex5 驾照领取时间 (yyyy-MM-dd)、
               ex6 驾照类型
            */
            bool result = false;

            var key = CommonPlatform.Helper.EnumStringHelper.ToString(Enums.UserExpandType.CarOwnerInfo);

            Model.UserExpand data = new Model.UserExpand()
            {
                UserId = userId,
                CreateTime = DateTime.Now,
                Ex1 = carModelId.ToString(),
                DataType = key
            };

            var oldData = Get<Model.UserExpand>(string.Format(" UserId = '{0}' AND DataType = '{1}' ",
                    userId,
                    key
                ));

            //查询是否有，有则更新无则新增
            if (oldData != null)
            {
                oldData.Ex1 = data.Ex1;

                result = Update(oldData);
            }
            else
            {
                result = Add(data);
            }

            return result;
        }
        /// <summary>
        /// 读取用户车主信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Model.CarOwnerInfo GetUserCarOwnerInfo(string userId)
        {
            Model.CarOwnerInfo result = null;

            var key = CommonPlatform.Helper.EnumStringHelper.ToString(Enums.UserExpandType.CarOwnerInfo);

            var data = Get<Model.UserExpand>(string.Format(" UserId = '{0}' AND DataType = '{1}' ",
                    userId,
                    key
                ));

            result = Model.CarOwnerInfo.GetDataByUserExpand(data);

            return result;
        }
        /// <summary>
        /// 读取当前用户的车主信息
        /// </summary>
        /// <returns></returns>
        public Model.CarOwnerInfo GetCurrUserCarOwnerInfo()
        {
            return GetUserCarOwnerInfo(GetCurrUserID());
        } 
        #endregion


    }
}
