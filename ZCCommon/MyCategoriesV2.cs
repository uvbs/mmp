using ZentCloud.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
namespace ZentCloud.Common
{
    public class MyCategoriesV2
    {
        /// <summary>
        /// 获取分类树集合
        /// </summary>
        /// <param name="cateList">分类数据</param>
        /// <param name="rootId">根节点</param>
        /// <param name="blankStr"></param>
        /// <param name="currDepth">当前深度</param>
        /// <param name="maxDepth">最大深度</param>
        /// <returns></returns>
        public List<ListItem> GetCateListItem(List<MyCategoryV2Model> cateList, string rootId, string blankStr = "", int currDepth = 1, int maxDepth = 10000)
        {
            foreach (var item in cateList)
            {
                if (item.CateID==null)
                {
                    item.CateID = "";
                }
                if (item.CateName == null)
                {
                    item.CateName = "";
                }
                if (item.PreID == null)
                {
                    item.PreID = "";
                }
            }
            List<ListItem> list = new List<ListItem>();
            foreach (MyCategoryV2Model current in (
                from p in cateList
                where p.PreID.Equals(rootId)
                select p).ToList<MyCategoryV2Model>())
            {
                var item = new ListItem();
                list.Add(new ListItem
                {
                    Text = ((blankStr != "") ? (blankStr + "└") : blankStr) + current.CateName,
                    Value = current.CateID.ToString()
                });
                if (currDepth < maxDepth)
                {
                    list.AddRange(this.GetCateListItem(cateList, current.CateID, blankStr + "\u3000", currDepth, maxDepth));
                }
            }
            return list;
        }
        public List<MyCategoryV2Model> GetCateList(List<MyCategoryV2Model> cateList, string rootId, string blankStr = "")
        {
            List<MyCategoryV2Model> list = new List<MyCategoryV2Model>();
            foreach (MyCategoryV2Model current in (
                from p in cateList
                where p.PreID.Equals(rootId)
                select p).ToList<MyCategoryV2Model>())
            {
                list.Add(new MyCategoryV2Model
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
        public string CreateSelectOptionHtml(List<MyCategoryV2Model> cateList, string rootId, string selectID, string selectStyle, string selectValue, string defaultValue = "0", string defaultText = "", int maxDepth = 10000)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<select id=\"{0}\" style=\"{1}\">", selectID, selectStyle);
            List<ListItem> cateListItem = this.GetCateListItem(cateList, rootId, "", 1, maxDepth);
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
        public List<MyCategoryV2Model> GetCommCateModelList<T>(string id, string parent_id, string name, IList<T> IL)
        {
            List<MyCategoryV2Model> list = new List<MyCategoryV2Model>();
            List<int> colIndex = this.GetColIndex<T>(id, parent_id, name, IL);
            T t = Activator.CreateInstance<T>();
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    var model = new MyCategoryV2Model();
                    var cateID = properties[colIndex[0]].GetValue(IL[i], null);
                    var preID = properties[colIndex[1]].GetValue(IL[i], null);
                    var  cateName = properties[colIndex[2]].GetValue(IL[i], null);
                    if (cateID!=null)
                    {
                        model.CateID = cateID.ToString();
                    }
                    if (preID != null)
                    {
                        model.PreID = preID.ToString();
                    }
                    if (cateName != null)
                    {
                        model.CateName = cateName.ToString();
                    }
                    list.Add(model);
                    //list.Add(new MyCategoryV2Model
                    //{
                    //    CateID = properties[colIndex[0]].GetValue(IL[i], null).ToString(),
                    //    PreID = properties[colIndex[1]].GetValue(IL[i], null).ToString(),
                    //    CateName = properties[colIndex[2]].GetValue(IL[i], null).ToString(),
                    //    SortIndex = 0
                    //});
                }
            }
            return list;
        }
        public string GetSelectOptionHtml<T>(IList<T> IL, string relateId, string relateParentid, string relateName, string rootId, string selectId, string selectStyle, string selectValue, string defaultValue = "0", string defaultText = "", int maxDepth = 10000)
        {
            return this.CreateSelectOptionHtml(
                    this.GetCommCateModelList<T>(relateId, relateParentid, relateName, IL),
                    rootId,
                    selectId,
                    selectStyle,
                    selectValue,
                    defaultValue,
                    defaultText,
                    maxDepth
                );
        }
    }
}
