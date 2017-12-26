using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZentCloud.ZCBLLEngine
{
    public class BLLTransaction : ZentCloud.ZCDALEngine.DALTranction
    {
        public BLLTransaction(IsolationLevel iso)
            : base(iso)
        {
            
        }
        public BLLTransaction()
        {

        }

    }

}
