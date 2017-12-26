using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ZentCloud.ZCDALEngine
{
    public class MetaTables
    {
        private Dictionary<string, MetaTable> _metas = new Dictionary<string, MetaTable>();

        public Dictionary<string, MetaTable> Tables
        {
            get { return _metas; }
            set { _metas = value; }
        }


        public void Load(string filePath)
        {
            //throw exception;
        }
    }
}
