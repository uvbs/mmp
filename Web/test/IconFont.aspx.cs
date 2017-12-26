using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ZentCloud.JubitIMP.Web.test
{
    public partial class IconFont : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public string icoScript;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!bll.IsLogin)
                Response.Redirect("/Home/logout.aspx");
            if (bll.GetCurrentUserInfo().UserType!=1)
                Response.Redirect("/Home/logout.aspx");

            //图标引用js
            icoScript = bll.GetIcoScript();
        }
        private void buildIconFont(string fileStrng)
        {
            string timestamp = ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();

            JToken jt = JToken.Parse(fileStrng);
            if (jt["data"]!=null) jt = jt["data"];
            JArray ja = (JArray)jt["icons"];
            List<string> lst = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<svg></svg>");

            XmlDocument xmlTempDoc = new XmlDocument();
            foreach (var item in ja)
            {
                string xId = "icon-" + item["font_class"].ToString();
                XmlNode xn = xmlDoc.CreateElement("symbol");
                xmlTempDoc.LoadXml(item["show_svg"].ToString());
                XmlElement xEm = xmlTempDoc.DocumentElement;

                XmlAttribute xAid = xmlDoc.CreateAttribute("id");
                xAid.Value = xId;

                XmlAttribute xBox = xmlDoc.CreateAttribute("viewBox");
                if (xEm.Attributes["viewBox"] != null)
                {
                    xBox.Value = xEm.Attributes["viewBox"].Value;
                }else{
                   xBox.Value = "0 0 1024 1024";
                }

                xn.Attributes.Append(xAid);
                xn.Attributes.Append(xBox);

                xn.InnerXml = xEm.InnerXml;

                lst.Add(xId);
                xmlDoc.DocumentElement.AppendChild(xn);
            }

            string js_file = string.Format( "/JsonConfig/iconFontJs/font_{0}.js",timestamp);

            string jsContent = File.ReadAllText(this.Server.MapPath("/JsonConfig/iconFontJsTemplate/font_temp.js"));
            jsContent = jsContent.Replace("$svgSprite$",xmlDoc.InnerXml);
            dynamic result = new
            {
                js_file = js_file,
                icon_class = lst
            };
            File.WriteAllText(this.Server.MapPath(js_file), jsContent);
            File.WriteAllText(this.Server.MapPath("/JsonConfig/至云平台通用图标.json"), JsonConvert.SerializeObject(result));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFile)
            {
                ZentCloud.Common.WebMessageBox.Show(this, "请选择文件");
                return;
            }
            StreamReader reader = new StreamReader(FileUpload1.PostedFile.InputStream);
            string fileStrng = reader.ReadToEnd();
            buildIconFont(fileStrng);
        }
    }
}