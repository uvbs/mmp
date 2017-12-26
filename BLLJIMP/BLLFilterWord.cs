using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLFilterWord : BLL
    {
        /// <summary>
        /// 提交关键词
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool PutFilterWord(FilterWord word) {
            if (word.AutoID == 0)
            {
                return Add(word);
            }
            else
            {
                return Update(word);
            }
        }
        /// <summary>
        /// 拼接条件
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private string GetParams(string filterType, string word, string websiteOwner)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(filterType)) sbWhere.AppendFormat(" AND FilterType={0} ", filterType);
            if (!string.IsNullOrWhiteSpace(word)) sbWhere.AppendFormat(" AND Word LIKE '%{0}%' ", word);
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            return sbWhere.ToString();
        }
        /// <summary>
        /// 获取关键词列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="FilterType"></param>
        /// <param name="Word"></param>
        /// <returns></returns>
        public List<FilterWord> GetFilterWordList(int pageSize, int pageIndex, string FilterType, string Word, string WebsiteOwner,out int total)
        { 
            string whereParams = GetParams(FilterType,Word,WebsiteOwner);
            total = GetCount<FilterWord>(whereParams);
            return GetLit<FilterWord>(pageSize, pageIndex, whereParams, "Word asc");
        }
        /// <summary>
        /// 敏感词检查
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="content"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool CheckFilterWord(string content, string websiteOwner, out string errorMsg, string filterType = "0")
        {
            errorMsg = "";
            int total = 0;
            content = System.Web.HttpUtility.HtmlDecode(content);
            List<FilterWord> lstWord = GetFilterWordList(int.MaxValue, 1, filterType, null, websiteOwner,out total);
            List<FilterWord> lstHaveWord = lstWord.Where(p => content.ToLower().Contains(p.Word.ToLower())).Take(3).ToList();
            if (lstHaveWord.Count > 0)
            {
                errorMsg += "内容存在";
                foreach (FilterWord item in lstHaveWord)
                {
                    errorMsg += "[" + item.Word + "]";
                }
                errorMsg += "等敏感词！";
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 检查词是否存在
        /// </summary>
        /// <param name="FilterType"></param>
        /// <param name="Word"></param>
        /// <param name="WebsiteOwner"></param>
        /// <returns></returns>
        public FilterWord GetFilterWord(int autoId)
        {
            return Get<FilterWord>(string.Format("AutoID={0}",autoId));
        }
        /// <summary>
        /// 检查词是否存在
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="word"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool ExistsFilterWord(string filterType, string word, string websiteOwner,string autoId="")
        {
            string whereParams = GetParams(filterType, word, websiteOwner);
            FilterWord old = Get<FilterWord>(whereParams);
            if (old == null || old.AutoID.ToString() == autoId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 批量删除敏感词
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteFilterWords(string ids) {
            return Delete(new FilterWord(),string.Format("AutoID IN ({0})", ids));
        }
    }
}
