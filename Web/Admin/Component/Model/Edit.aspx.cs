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
    public partial class Edit : System.Web.UI.Page
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        BLLCompanyWebSite bllCompanyWebSite = new BLLCompanyWebSite();
        protected string keyvalue_list;
        protected string tool_list;
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

            //List<string> ToolBarList = bllCompanyWebSite.GetToolBarUseTypeList(bllCompanyWebSite.WebsiteOwner);
            //改成写死
            List<string> ToolBarList = new List<string>() { "foottool", "tab", "button", "headtool", "nav" };
            dynamic tool_result = from p in ToolBarList
                                  select new
                                  {
                                      value = p,
                                      name = bllCompanyWebSite.GetToolBarUseTypeName(p)
                                  };
            tool_list = JsonConvert.SerializeObject(tool_result);
        }
    }
}