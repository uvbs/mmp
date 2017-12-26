using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ZentCloud.Common;
using System.Text;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// IconSymbols 的摘要说明
    /// </summary>
    public class IconSymbols : BaseHandlerNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string idsString = context.Request["ids"];
            List<string> ids = idsString.Split(',').ToList();
            string path = context.Server.MapPath("/JsonConfig/xmlConfig/至云平台通用图标.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            StringBuilder sbSymbols = new StringBuilder();
            foreach (string id in ids)
	        {
                XmlNode node = doc.SelectSingleNode(string.Format("{0}/{1}[@{2}='{3}']", "symbols", "symbol","id", id));
                if (node != null) sbSymbols.AppendFormat(node.OuterXml);
	        }
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = sbSymbols.ToString();

            bll.ContextResponse(context, apiResp);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}