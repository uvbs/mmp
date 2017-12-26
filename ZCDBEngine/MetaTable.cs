using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ZentCloud.ZCDALEngine
{
    public class MetaTable : MetaBase
    {
        public MetaTable(string name)
        {
            Name = name;
               //从XML文件中读取表结构生成表元数据。
               //构造中必须保证表有主键。
        }

        public MetaTable()
        {
            //从XML文件中读取表结构生成表元数据。
            //构造中必须保证表有主键。
        }

        private string _name;   //table name
        private Dictionary<string, SqlDbType> _colums = new Dictionary<string,SqlDbType>(); //表字段名，字段类型列表
        List<List<string>> _keys = new List<List<string>>();//keys[0]为主键，其他为索引

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Dictionary<string, SqlDbType> Columns
        {
            get { return _colums; }
            set { _colums = value; }
        }

        public List<List<string>> Keys
        {
            get { return _keys; }
            set { _keys = value; }
        }
    }
}
