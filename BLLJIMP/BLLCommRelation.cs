using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 通用关系
    /// </summary>
    public class BLLCommRelation : BLL
    {
        public BLLCommRelation()
            : base()
        {

        }

        /// <summary>
        /// 关系是否已存在
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public bool ExistRelation(BLLJIMP.Enums.CommRelationType rtype, string mainId, string relationId, string expandId = "", string ex1 = "", string ex2 = "", string ex3 = "", string ex4 = "", string ex5 = "",string websiteOwnner = "")
        {
            return Get<Model.CommRelationInfo>(GetSqlWhereByParm(rtype, mainId, relationId, expandId, ex1, ex2, ex3, ex4, ex5, websiteOwnner)) != null;
        }

        /// <summary>
        /// 添加关系
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public bool AddCommRelation(
                BLLJIMP.Enums.CommRelationType rtype,
                string mainId,
                string relationId,
                string expandId = "",
                string ex1 = "",
                string ex2 = "",
                string ex3 = "",
                string ex4 = "",
                string ex5 = "",
                string websiteOwner = ""
            )
        {
            if (ExistRelation(rtype, mainId, relationId, expandId, ex1, ex2, ex3, ex4, ex5))
            {
                return false;
            }

            Model.CommRelationInfo data = new Model.CommRelationInfo()
            {
                MainId = mainId,
                RelationId = relationId,
                RelationTime = DateTime.Now,
                RelationType = CommonPlatform.Helper.EnumStringHelper.ToString(rtype),
                ExpandId = expandId,
                Ex1 = ex1,
                Ex2 = ex2,
                Ex3 = ex3,
                Ex4 = ex4,
                Ex5 = ex5,
                WebsiteOwner = websiteOwner
            };

            return Add(data);
        }

        /// <summary>
        /// 删除单个关系
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public bool DelCommRelation(BLLJIMP.Enums.CommRelationType rtype, string mainId, string relationId, string expandId = "")
        {
            return Delete(new Model.CommRelationInfo(), GetSqlWhereByParm(rtype, mainId, relationId, expandId)) > 0;
        }

        /// <summary>
        /// 根据参数构造查询语句
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public string GetSqlWhereByParm(Enums.CommRelationType rtype, string mainId, string relationId, string expandId = "",string ex1 = "",string ex2 = "",string ex3 = "",string ex4 = "", string ex5 = "",string websiteOwnner = "")
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            StringBuilder strWhere = new StringBuilder();

            strWhere.AppendFormat(" RelationType = '{0}' ", type);

            if (!string.IsNullOrWhiteSpace(mainId))
                strWhere.AppendFormat(" AND  MainId = '{0}' ", mainId);

            if (!string.IsNullOrWhiteSpace(relationId))
                strWhere.AppendFormat(" AND  RelationId = '{0}' ", relationId);

            if (!string.IsNullOrWhiteSpace(expandId))
                strWhere.AppendFormat(" AND  ExpandId = '{0}' ", expandId);

            if (!string.IsNullOrWhiteSpace(ex1))
                strWhere.AppendFormat(" AND  Ex1 = '{0}' ", ex1);

            if (!string.IsNullOrWhiteSpace(ex2))
                strWhere.AppendFormat(" AND  Ex2 = '{0}' ", ex2);

            if (!string.IsNullOrWhiteSpace(ex3))
                strWhere.AppendFormat(" AND  Ex3 = '{0}' ", ex3);

            if (!string.IsNullOrWhiteSpace(ex4))
                strWhere.AppendFormat(" AND  Ex4 = '{0}' ", ex4);

            if (!string.IsNullOrWhiteSpace(ex5))
                strWhere.AppendFormat(" AND  Ex5 = '{0}' ", ex5);

            if (!string.IsNullOrWhiteSpace(websiteOwnner))
                strWhere.AppendFormat(" AND  WebsiteOwnner = '{0}' ", websiteOwnner);

            return strWhere.ToString();
        }

        /// <summary>
        /// 查询关系数量
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public int GetRelationCount(Enums.CommRelationType rtype, string mainId, string relationId, string expandId = "", string ex1 = "", string ex2 = "", string ex3 = "", string ex4 = "", string ex5 = "",string websiteOwnner = "")
        {
            var strWhere = GetSqlWhereByParm(rtype, mainId, relationId, expandId, ex1, ex2, ex3, ex4, ex5 , websiteOwnner);
            var result = GetCount<Model.CommRelationInfo>(strWhere);
            return result;
        }


        /// <summary>
        /// 查询关系列表
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Model.CommRelationInfo> GetRelationList(Enums.CommRelationType rtype, string mainId, string relationId, 
            int pageIndex, int pageSize, string expandId = "", string ex1 = "", string ex2 = "", string ex3 = "", 
            string ex4 = "", string ex5 = "", string websiteOwnner = "",string colName="")
        {
            var strWhere = GetSqlWhereByParm(rtype, mainId, relationId, expandId, ex1, ex2, ex3, ex4, ex5, websiteOwnner);
            dynamic result;
            if(string.IsNullOrWhiteSpace(colName)){
                result = GetLit<Model.CommRelationInfo>(pageSize, pageIndex, strWhere);
            }
            else
            {
                result = GetColList<Model.CommRelationInfo>(pageSize, pageIndex, strWhere, colName);
            }
            return result;
        }

        public List<Model.CommRelationInfo> GetRelationList(out int totalCount,Enums.CommRelationType rtype, string mainId, string relationId, 
            int pageIndex, int pageSize, string expandId = "", string ex1 = "", string ex2 = "", string ex3 = "", 
            string ex4 = "", string ex5 = "", string websiteOwnner = "")
        {
            var strWhere = GetSqlWhereByParm(rtype, mainId, relationId, expandId, ex1, ex2, ex3, ex4, ex5, websiteOwnner);
            var result = GetLit<Model.CommRelationInfo>(pageSize, pageIndex, strWhere);
            totalCount = GetCount<Model.CommRelationInfo>(strWhere);
            return result;
        }

        /// <summary>
        /// 查询关系列表
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Model.CommRelationInfo> GetRelationListDesc(Enums.CommRelationType rtype, string mainId, 
            string relationId, int pageIndex, int pageSize, out int totalCount, string expandId = "")
        {
            var strWhere = GetSqlWhereByParm(rtype, mainId, relationId, expandId);
            var result = GetLit<Model.CommRelationInfo>(pageSize, pageIndex, strWhere, " AutoId Desc");
            totalCount = GetCount<Model.CommRelationInfo>(strWhere);
            return result;
        }

        /// <summary>
        /// 查询关系列表
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="keyword"></param>
        /// <param name="relationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Model.CommRelationInfo> GetRelationListDescByUserName(Enums.CommRelationType rtype, string mainId, string relationId, string keyword, int pageIndex, int pageSize, out int totalCount)
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" SELECT COUNT(1) FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_UserInfo] ON [RelationId]=[UserID] ");
            strWhere.AppendFormat(" WHERE RelationType = '{0}' ", type);
            if (!string.IsNullOrWhiteSpace(mainId))
                strWhere.AppendFormat(" AND  MainId = '{0}' ", mainId);
            if (!string.IsNullOrWhiteSpace(relationId))
                strWhere.AppendFormat(" AND  RelationId = '{0}' ", relationId);
            if (!string.IsNullOrWhiteSpace(keyword))
                strWhere.AppendFormat(" AND ([RelationId] LIKE '%{0}%' OR [TrueName] LIKE '%{0}%') ", keyword);
            var result1 = Query(strWhere.ToString());
            totalCount = Convert.ToInt32(result1.Tables[0].Rows[0][0]);
            strWhere = new StringBuilder();

            strWhere.AppendFormat(" WITH TEMP AS ( ");
            strWhere.AppendFormat(" SELECT ROW_NUMBER() OVER (ORDER BY A.[AutoId] DESC) NUM, A.* FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_UserInfo] ON [RelationId]=[UserID] ");
            strWhere.AppendFormat(" WHERE RelationType = '{0}' ", type);
            if (!string.IsNullOrWhiteSpace(mainId))
                strWhere.AppendFormat(" AND  MainId = '{0}' ", mainId);
            if (!string.IsNullOrWhiteSpace(relationId))
                strWhere.AppendFormat(" AND  RelationId = '{0}' ", relationId);
            if (!string.IsNullOrWhiteSpace(keyword))
                strWhere.AppendFormat(" AND ([RelationId] LIKE '%{0}%' OR [TrueName] LIKE '%{0}%') ", keyword);
            strWhere.AppendFormat(" )");
            strWhere.AppendFormat(" SELECT [AutoId],[MainId],[RelationId],[RelationType],[RelationTime] FROM TEMP ");
            strWhere.AppendFormat(" WHERE NUM BETWEEN ({0} -1) * {1} + 1 AND {0}*{1}; ", pageIndex, pageSize);

            var result = Query<Model.CommRelationInfo>(strWhere.ToString());
            return result;
        }


        /// <summary>
        /// 查询一个关系对象
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public Model.CommRelationInfo GetRelationInfo(Enums.CommRelationType rtype, string mainId, string relationId, string expandId = "")
        {
            var result = Get<Model.CommRelationInfo>(GetSqlWhereByParm(rtype, mainId, relationId, expandId));
            return result;
        }

        /// <summary>
        /// 0自动注册
        /// 1手动注册(跳转注册页)
        /// 2手动注册(不跳转注册页)
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int GetIsNotAutoRegNewWxUser(string websiteOwner)
        {
            var result = Get<Model.CommRelationInfo>(GetSqlWhereByParm(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, websiteOwner, ""));
            if (result == null) return 0;
            if (result.ExpandId == "1") return 2;
            return 1;
        }
        

    }
}