using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;
using Newtonsoft.Json;

namespace ZentCloud.Common
{
    public class JSONHelper
    {
        /*Demo
            JavaScriptSerializer jscvt = new JavaScriptSerializer();
            //将json字符串转变成指定类型对象
            man aman= jscvt.Deserialize<man>(json);
            Response.Write(aman.name);
            //将对象转变成json字符串
            string objstr = jscvt.Serialize(aman);
            Response.Write(objstr);
            //将JSON转变成object对象
            object obj = jscvt.DeserializeObject(json);
            //将给定对象转换成指定类型
            man man= jscvt.ConvertToType<man>(obj);
            Response.Write(man.addr);
        */

        /// <summary>
        /// 对象序列化成json数据
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <returns>json数据</returns>
        public static string ObjectToJson(object obj)
        {
            JavaScriptSerializer jscvt = new JavaScriptSerializer();
            jscvt.MaxJsonLength = int.MaxValue;
            return jscvt.Serialize(obj);
            //return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 生成easyUI datagrid格式 Json
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="totalcount">总数</param>
        /// <param name="IL">传入List</param>
        /// <returns></returns>
        public static string ObjectToJson<T>(int totalcount, IList<T> IL)
        {

            StringBuilder Json = new StringBuilder();
            Json.Append("{");
            Json.Append("\"total\"");
            Json.Append(":");
            Json.Append(totalcount);
            Json.Append(",");

            Json.Append("\"rows\"");

            Json.Append(":[");

            if (IL.Count > 0)
            {
                T obj = Activator.CreateInstance<T>();
                Type type = obj.GetType();
                PropertyInfo[] pis = type.GetProperties();

                for (int i = 0; i < IL.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < pis.Length; j++)
                    {
                        Json.Append("\"");
                        Json.Append(pis[j].Name);
                        Json.Append("\"");
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
            Json.Append("]}");
            return Json.ToString().Replace("\r", null).Replace("\t", null).Replace("\n", "<br/>");

        }

        /// <summary>
        /// 生成easyUI datagrid格式 Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="totalcount"></param>
        /// <param name="IL"></param>
        /// <returns></returns>
        public static string ListToEasyUIJson<T>(int totalcount, IList<T> IL)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{");
            Json.Append("\"total\"");
            Json.Append(":");
            Json.Append(totalcount);
            Json.Append(",");
            Json.Append("\"rows\"");
            Json.Append(":");

            Json.Append(ObjectToJson(IL));

            //Json.Append(ZCJson.JsonConvert.SerializeObject(IL));

            Json.Append("}");
            return Json.ToString();


        }

        /// <summary>
        /// 生成easyUI datagrid格式 Json
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ObjectToJson(int totalcount, object data)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{");
            Json.Append("\"total\"");
            Json.Append(":");
            Json.Append(totalcount);
            Json.Append(",");
            Json.Append("\"rows\"");
            Json.Append(":");
            Json.Append(ObjectToJson(data));
            Json.Append("}");
            return Json.ToString();

        }

        /// <summary>
        /// DataTable转换成EasyUIJson
        /// </summary>
        /// <param name="totalcount"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToEasyUIJson(int totalcount, DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{");
            Json.Append("\"total\"");
            Json.Append(":");
            Json.Append(totalcount);
            Json.Append(",");

            Json.Append("\"rows\"");

            Json.Append(":[");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"");
                        Json.Append(dt.Columns[j].ColumnName);
                        Json.Append("\"");
                        Json.Append(":");
                        Json.Append("\"");
                        Json.Append(dt.Rows[i][j]);
                        Json.Append("\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");


            return Json.ToString().Replace("\r", null).Replace("\t", null).Replace("\n", "<br/>");


        }

        /// <summary>
        /// 数据集序列化成json数据
        /// </summary>
        /// <param name="List<T>">对象实体</param>
        /// <returns>json数据</returns>
        public static string ListToJson<T>(List<T> objList)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T item in objList)
            {
                sb.Append(ObjectToJson(item) + ";");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 数据集序列化成json数据
        /// </summary>
        /// <typeparam name="T">数据集类型</typeparam>
        /// <param name="objList">对象数据集</param>
        /// <param name="splitStr">对象数据分隔符</param>
        /// <returns>json数据集合</returns>
        public static string ListToJson<T>(List<T> objList, string splitStr)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T item in objList)
            {
                sb.Append(ObjectToJson(item) + splitStr);
            }

            string result = sb.ToString().Substring(0, sb.ToString().LastIndexOf(splitStr));

            return result;
        }

        /// <summary>
        /// joson数据集反序列化为数据集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> JsonToObjectList<T>(string json)
        {
            List<T> result = new List<T>();
            String[] arr = Regex.Split(json, ";");
            foreach (string item in arr)
            {
                if (item == "")
                    continue;
                result.Add(JsonToModel<T>(item));
            }
            return result;
        }

        /// <summary>
        /// joson数据集反序列化为字符串集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<string> JsonToStrList(string json)
        {
            List<string> result = new List<string>();

            String[] arr = Regex.Split(json, ";");

            foreach (string item in arr)
            {
                if (item.Equals(""))
                    continue;
                result.Add(item.Replace(";", "；"));
            }

            return result;
        }

        /// <summary>
        /// joson数据集反序列化为字符串集合
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="splitStr">数据分隔符</param>
        /// <returns></returns>
        public static List<string> JsonToStrList(string json, string splitStr)
        {
            List<string> result = new List<string>();

            String[] arr = Regex.Split(json, splitStr);

            foreach (string item in arr)
            {
                if (item.Equals(""))
                    continue;
                result.Add(item.Replace(";", "；"));
            }

            return result;
        }

        /// <summary>
        /// joson数据集反序列化为数据集
        /// </summary>
        /// <typeparam name="T">数据集类型</typeparam>
        /// <param name="json">json数据</param>
        /// <param name="splitStr">数据对象分隔符</param>
        /// <returns></returns>
        public static List<T> JsonToObjectList<T>(string json, string splitStr)
        {
            List<T> result = new List<T>();
            String[] arr = Regex.Split(json, ";");
            foreach (string item in arr)
            {
                if (item == "")
                    continue;
                result.Add(JsonToModel<T>(item));
            }
            return result;
        }

        /// <summary>
        /// 将给定对象转换成指定类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="obj">给定对象</param>
        /// <returns>对象实体</returns>
        public static T ObjectToType<T>(object obj)
        {
            JavaScriptSerializer jscvt = new JavaScriptSerializer();
            return jscvt.ConvertToType<T>(obj);
        }

        /// <summary>
        /// 将json数据反序列化成对象
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns>对象</returns>
        public static object JsonToObject(string json)
        {
            JavaScriptSerializer jscvt = new JavaScriptSerializer();
            return jscvt.DeserializeObject(json.Replace(";", "；"));
        }

        /// <summary>
        /// 将json数据反序列化成指定类型实体
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns>对象实体</returns>
        public static T JsonToModel<T>(string json,bool isReplace=true)
        {
            JavaScriptSerializer jscvt = new JavaScriptSerializer();
            return jscvt.Deserialize<T>(json);
        }




        /// <summary>
        /// 根据索引删除json指定数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string DeleteJsonByIndex(int index, string json)
        {
            string result = string.Empty;
            List<string> list = JsonToStrList(json);
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    if (i.Equals(index))
                    {
                        continue;
                    }
                    sb.AppendFormat("{0};", list[i]);
                }
                result = sb.ToString();
            }
            return result;
        }

        /// <summary>
        /// 根据索引更新json对象数据集
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="json">json对象数据集</param>
        /// <param name="newData">新数据</param>
        /// <returns></returns>
        public static string UpdateJsonByIndex(int index, string json, object newData)
        {
            string result = string.Empty;

            List<string> list = JsonToStrList(json);
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    if (i.Equals(index))
                    {
                        list[i] = ObjectToJson(newData);
                    }
                    sb.AppendFormat("{0};", list[i]);
                }
                result = sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 获取指定json数据集索引的实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T GetModelByIndex<T>(int index, string json)
        {
            List<string> list = JsonToStrList(json);
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    if (i.Equals(index))
                    {
                        return JsonToModel<T>(list[i]);
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// 添加对象数据到json字符串数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="model"></param>
        public static string AddViewStateListValue<T>(string json, T model)
        {
            //获取当前保存的集合
            List<T> list = JsonToObjectList<T>(json);
            list.Add(model);
            //保存添加的集合
            return ListToJson<T>(list);
        }
    }
}
