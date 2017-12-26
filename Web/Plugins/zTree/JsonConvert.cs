using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZentCloud.JubitIMP.Web.zTree
{
    public class JsonConvert
    {
        /// <summary>
        /// 获取映射字段所在索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">映射的id</param>
        /// <param name="parentid">映射的上级id</param>
        /// <param name="name">映射的分类名称</param>
        /// <param name="IL"></param>
        /// <returns></returns>
        private static List<int> GetColIndex<T>(string id, string parentid, string name, IList<T> IL)
        {
            List<int> lst = new List<int>() { 0, 0, 0 };
            T obj = Activator.CreateInstance<T>();
            Type type = obj.GetType();
            PropertyInfo[] pis = type.GetProperties();
            for (int j = 0; j < pis.Length; j++)
            {
                if (pis[j].Name.Equals(id))
                {
                    lst[0] = j;
                }
                if (pis[j].Name.Equals(parentid))
                {
                    lst[1] = j;
                }
                if (pis[j].Name.Equals(name))
                {
                    lst[2] = j;
                }

            }
            return lst;
        }

        /// <summary>
        /// 获取Ztree Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">映射的id字段</param>
        /// <param name="parent_id">映射的上级id字段</param>
        /// <param name="name">映射的显示名称字段</param>
        /// <param name="IL">传入泛型集合</param>
        /// <returns></returns>
        public static string GetTreeJson<T>(string id, string parent_id, string name, IList<T> IL)
        {
            T obj = Activator.CreateInstance<T>();
            Type type = obj.GetType();
            PropertyInfo[] pis = type.GetProperties();
            List<int> lstIndex = GetColIndex(id, parent_id, name, IL);
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    Json.Append("{");
                    Json.Append("id");
                    Json.Append(":");
                    Json.Append(pis[lstIndex[0]].GetValue(IL[i], null));
                    Json.Append(",");
                    Json.Append("pId");
                    Json.Append(":");
                    Json.Append(pis[lstIndex[1]].GetValue(IL[i], null));
                    Json.Append(",");
                    Json.Append("name");
                    Json.Append(":");
                    Json.Append("\"");
                    Json.Append(pis[lstIndex[2]].GetValue(IL[i], null));
                    Json.Append("\"");
                    Json.Append(",");
                    for (int j = 0; j < pis.Length; j++)
                    {

                        Json.Append(pis[j].Name);
                        Json.Append(":");
                        Json.Append("\"");
                        Json.Append(pis[j].GetValue(IL[i], null));
                        Json.Append("\"");
                        if (j < pis.Length - 1)
                        {
                            Json.Append(",");
                        }

                    }
                    Json.Append("}");
                    if (i < IL.Count - 1)
                    {
                        Json.Append(",");
                    }

                }
            }

            Json.Append("]");
            var a= Json.ToString();
            return a;
        }

    }
}