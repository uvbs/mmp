using ZentCloud.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
namespace ZentCloud.Common
{
	public class MyCategories
	{
        /// <summary>
        /// 获取分类树集合
        /// </summary>
        /// <param name="cateList">分类数据</param>
        /// <param name="rootID">根节点</param>
        /// <param name="blankStr"></param>
        /// <param name="currDepth">当前深度</param>
        /// <param name="maxDepth">最大深度</param>
        /// <returns></returns>
		public List<ListItem> GetCateListItem(List<MyCategoryModel> cateList, int rootID, string blankStr = "",int currDepth = 1,int maxDepth = 10000)
		{
			List<ListItem> list = new List<ListItem>();
            foreach (MyCategoryModel current in (
                from p in cateList
                where p.PreID.Equals(rootID)
                select p).ToList<MyCategoryModel>())
            {
                ListItem li = new ListItem(){
                    Text = ((blankStr != "") ? (blankStr + "└") : blankStr) + current.CateName,
                    Value = current.CateID.ToString()
                };
                li.Attributes.Add("PreID",current.PreID.ToString());
                list.Add(li);
                if (currDepth < maxDepth)
                {
                    list.AddRange(this.GetCateListItem(cateList, current.CateID, blankStr + "\u3000", currDepth, maxDepth));
                }
            }
			return list;
		}
		public List<MyCategoryModel> GetCateList(List<MyCategoryModel> cateList, int rootID, string blankStr = "")
		{
			List<MyCategoryModel> list = new List<MyCategoryModel>();
			foreach (MyCategoryModel current in (
				from p in cateList
				where p.PreID.Equals(rootID)
				select p).ToList<MyCategoryModel>())
			{
				list.Add(new MyCategoryModel
				{
					CateNewName = ((blankStr != "") ? (blankStr + "└") : blankStr) + current.CateName,
					CateID = current.CateID,
					CateName = current.CateName,
					PreID = current.PreID,
					SortIndex = current.SortIndex
				});
				list.AddRange(this.GetCateList(cateList, current.CateID, blankStr + "\u3000"));
			}
			return list;
		}
		public string CreateSelectOptionHtml(List<MyCategoryModel> cateList, int rootID, string selectID, string selectStyle, string selectValue, string defaultValue = "0", string defaultText = "",int maxDepth = 10000)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<select id=\"{0}\" style=\"{1}\">", selectID, selectStyle);
			List<ListItem> cateListItem = this.GetCateListItem(cateList, rootID, "",1,maxDepth);
			stringBuilder.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
			foreach (ListItem current in cateListItem)
			{
				stringBuilder.AppendFormat("<option value=\"{0}\" {1} >", current.Value, (current.Value == selectValue) ? "selected" : "");
				stringBuilder.Append(current.Text);
				stringBuilder.Append("</option>");
			}
			stringBuilder.Append("</select>");
			return stringBuilder.ToString();
		}
        /// <summary>
        /// 构建下拉列表
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="selectID"></param>
        /// <param name="selectStyle"></param>
        /// <param name="selectValue"></param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        public string CreateSelectOptionHtml(List<ListItem> itemList, string selectID, string selectStyle, string selectValue, string defaultValue = "0", string defaultText = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<select id=\"{0}\" style=\"{1}\">", selectID, selectStyle);
            stringBuilder.AppendFormat("<option value=\"{0}\">{1}</option>", defaultValue, defaultText);
            foreach (ListItem current in itemList)
            {
                stringBuilder.AppendFormat("<option value=\"{0}\" {1} >", current.Value, (current.Value == selectValue) ? "selected" : "");
                stringBuilder.Append(current.Text);
                stringBuilder.Append("</option>");
            }
            stringBuilder.Append("</select>");
            return stringBuilder.ToString();
        }
		private List<int> GetColIndex<T>(string id, string parentid, string name, IList<T> IL)
		{
			List<int> list = new List<int>
			{
				0,
				0,
				0
			};
			T t = Activator.CreateInstance<T>();
			Type type = t.GetType();
			PropertyInfo[] properties = type.GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].Name.Equals(id))
				{
					list[0] = i;
				}
				if (properties[i].Name.Equals(parentid))
				{
					list[1] = i;
				}
				if (properties[i].Name.Equals(name))
				{
					list[2] = i;
				}
			}
			return list;
		}
		public List<MyCategoryModel> GetCommCateModelList<T>(string id, string parent_id, string name, IList<T> IL)
		{
			List<MyCategoryModel> list = new List<MyCategoryModel>();
			List<int> colIndex = this.GetColIndex<T>(id, parent_id, name, IL);
			T t = Activator.CreateInstance<T>();
			Type type = t.GetType();
			PropertyInfo[] properties = type.GetProperties();
			if (IL.Count > 0)
			{
				for (int i = 0; i < IL.Count; i++)
				{
                    var CateNameObj = properties[colIndex[2]].GetValue(IL[i], null);
                    list.Add(new MyCategoryModel
                    {
                        CateID = int.Parse(properties[colIndex[0]].GetValue(IL[i], null).ToString()),
                        PreID = int.Parse(properties[colIndex[1]].GetValue(IL[i], null).ToString()),
                        CateName = CateNameObj == null ? "" : CateNameObj.ToString(),
                        SortIndex = 0
                    });
				}
			}
			return list;
		}
        public List<MyCategoryModel> GetCommCateModelListStr<T>(string id, string parent_id, string name, IList<T> IL)
        {
            List<MyCategoryModel> list = new List<MyCategoryModel>();
            List<int> colIndex = this.GetColIndex<T>(id, parent_id, name, IL);
            T t = Activator.CreateInstance<T>();
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    var CateNameObj = properties[colIndex[2]].GetValue(IL[i], null);
                    list.Add(new MyCategoryModel
                    {
                        CateID = int.Parse(properties[colIndex[0]].GetValue(IL[i], null).ToString()),
                        PreID = int.Parse(properties[colIndex[1]].GetValue(IL[i], null).ToString()),
                        CateName = CateNameObj == null ? "" : CateNameObj.ToString(),
                        SortIndex = 0
                    });
                }
            }
            return list;
        }
		public string GetSelectOptionHtml<T>(IList<T> IL, string relate_id, string relate_parentid, string relate_name, int rootID, string selectID, string selectStyle, string selectValue, string defaultValue = "0", string defaultText = "",int maxDepth = 10000)
		{
			return this.CreateSelectOptionHtml(
                    this.GetCommCateModelList<T>(relate_id, relate_parentid, relate_name, IL), 
                    rootID, 
                    selectID,
                    selectStyle, 
                    selectValue,
                    defaultValue,
                    defaultText,
                    maxDepth
                );
		}
	}
}
