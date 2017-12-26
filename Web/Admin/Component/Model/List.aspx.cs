using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Component.Model
{
    public partial class List : System.Web.UI.Page
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        protected string keyvalue_list;
        protected void Page_Load(object sender, EventArgs e)
        {
            //下拉分类
            List<KeyVauleDataInfo> KeyVauleList = bllKeyValueData.GetKeyVauleDataInfoList("ComponentType", null, "Common");
            dynamic keyvalues = from p in KeyVauleList
                                select new
                                {
                                    name = p.DataValue,
                                    value = p.DataKey
                                };
            keyvalue_list = JsonConvert.SerializeObject(keyvalues);
        }
    }
}