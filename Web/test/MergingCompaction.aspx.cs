using AliOss;
using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Test
{
    public partial class MergingCompaction : System.Web.UI.Page
    {
        BLL bll = new BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string files = this.Request["files"];
            string rfilename = this.Request["rfilename"];
            string module = this.Request["module"];
            if (string.IsNullOrWhiteSpace(files) || string.IsNullOrWhiteSpace(rfilename)) return;

            string ext = Path.GetExtension(rfilename).ToLower();
            string type = ext.Substring(1);

            string filestring = MergingCompactionHelper.ToCompressor(type, files);
            byte[] filebytes = MergingCompactionHelper.ToGzip(filestring);
            string keyDir = "MergingCompaction/Gzip/" + type + "/";
            if (!string.IsNullOrWhiteSpace(module)) keyDir = keyDir + module + "/";
            string rfileurl = OssHelper.UploadFileFromByte("comeoncloud-static", keyDir + rfilename, filebytes,
                 type, false, true);
            
            if(!Directory.Exists(Server.MapPath("/" + keyDir))) Directory.CreateDirectory(Server.MapPath("/" + keyDir));
            File.WriteAllBytes(Server.MapPath("/" + keyDir + rfilename), filebytes);
            this.Response.Write(rfileurl);
        }
    }
}