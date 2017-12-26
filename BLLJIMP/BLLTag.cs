using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 标签
    /// </summary>
    public class BLLTag : BLL
    {

        /// <summary>
        /// 获取tag列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="keyWord"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Model.MemberTag> GetTags(string websiteOwner, string keyWord, int pageIndex, int pageSize, out int totalCount, string tagType="")
        {
            List<Model.MemberTag> result = new List<Model.MemberTag>();

            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" 1 = 1 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            }
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                strWhere.AppendFormat(" AND TagName LIKE '%{0}%' ", keyWord);
            }

            if (!string.IsNullOrWhiteSpace(tagType))
            {
                strWhere.AppendFormat(" AND TagType = '{0}' ", tagType);
            }
            result = GetLit<Model.MemberTag>(pageSize, pageIndex, strWhere.ToString());

            totalCount = GetCount<Model.MemberTag>(strWhere.ToString());

            return result;
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns>RelationId标签,AutoId暂时标示使用数</returns>
        public List<Model.CommRelationInfo> GetTagList(int? topNum)
        {
            StringBuilder strWhere = new StringBuilder();
            string topStr = topNum.HasValue?"TOP "+topNum:"";
            strWhere.AppendFormat(" SELECT {0} [RelationId],COUNT(1) [AutoId]",topStr);
            strWhere.AppendFormat(" FROM [ZCJ_CommRelationInfo] ");
            strWhere.AppendFormat(" WHERE [RelationType]='JuActivityTag'");
            strWhere.AppendFormat(" GROUP BY [RelationId] ");
            strWhere.AppendFormat(" ORDER BY [AutoId] DESC ");
            List<Model.CommRelationInfo> tags = Query<Model.CommRelationInfo>(strWhere.ToString());
            return tags;
        }
        /// <summary>
        /// 一组id查询标签列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Model.MemberTag> GetTagListByIds(string ids)
        {
            return GetList<Model.MemberTag>(string.Format("AutoId in ({0}) ", ids));
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DelTags(string ids)
        {
            return Delete(new Model.MemberTag(), string.Format("AutoId in ({0})", ids));
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool UpdateTag(string id,string tagName)
        {
            return Update(new Model.MemberTag(), string.Format(" TagName='{0}' ", tagName), string.Format("AutoId={0}", id)) > 0;
        }
        public Model.MemberTag GetTag(int id)
        {
            return Get<Model.MemberTag>(string.Format("AutoId={0}", id));
        }

        /// <summary>
        /// 检查是否存在标签
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ExistsTag(Model.MemberTag tag)
        {
            var modelTagName = Get<Model.MemberTag>(string.Format("TagName='{0}' AND TagType='{1}' AND WebsiteOwner='{2}'", tag.TagName, tag.TagType, tag.WebsiteOwner));
            if (modelTagName != null && modelTagName.AutoId == tag.AutoId) return false;
            return modelTagName != null;
        }

        /// <summary>
        /// 检查是否存在一组id的非该站点标签
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ExistsTag(string ids, string websiteOwner)
        {
            var modelTagName = Get<Model.MemberTag>(string.Format("AutoId in ({0}) AND  WebsiteOwner!='{1}'", ids, websiteOwner));
            return modelTagName != null;
        }


        /// <summary>
        /// 一组标签是否已经被使用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool IsUsingTag(List<Model.MemberTag> tags, string websiteOwner, out string usingTag)
        {
            usingTag = "";
            if (tags.Count == 0) return false;

            foreach (var itm in tags)
            {
                var modelTagName = Get<Model.UserInfo>(string.Format("TagName like '%{0}%' AND WebsiteOwner='{1}'", itm.TagName, websiteOwner));
                if (modelTagName != null)
                {
                    usingTag = itm.TagName;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool AddTag(Model.MemberTag Tag)
        {
            return Add(Tag);
        }


    }
}
