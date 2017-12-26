using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;


namespace ZentCloud.JubitIMP.Web.Member
{
    public partial class MemberUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PanelImport.Visible = true;
        }


        private void ShowMessge(string str)
        {
            Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), str);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.Session[Comm.SessionKey.UserType].ToString() == "0")
            {
                this.ShowMessge("未审核用户不能进行该项操作！");
                return;
            }

            //上传文件并读取内容
            //string filePath = Server.MapPath("~/FileUpload/") + this.Session[Comm.SessionKey.UserID].ToString() + Guid.NewGuid();
            //this.AjaxFileUpload1.SaveAs(filePath);

            string fileName = FileUpload1.FileName;
            string extraName = fileName.Substring(fileName.LastIndexOf(".") + 1);

            if (fileName == "" || fileName == null)
            {
                ZentCloud.Common.WebMessageBox.Show(this.Page, "未选择任何文件!");
                //this.ShowMessge("未选择任何文件");
                return;
            }

            if (extraName.ToLower() == "xls")
            {
                string tmpPath = Server.MapPath("~/FileUpload/MemberInfo/" + Guid.NewGuid().ToString());
                //建立临时目录
                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);
                fileName = tmpPath + "/" + fileName;

                this.FileUpload1.SaveAs(fileName);

                ZentCloud.BLLJIMP.BLLMember bllMember = new ZentCloud.BLLJIMP.BLLMember(Session[Comm.SessionKey.UserID].ToString());
                List<ZentCloud.BLLJIMP.Model.MemberInfo> memberList = bllMember.GetMemberListFromExcel(fileName, bllMember.UserID);
                int importCount = 0;
                foreach (ZentCloud.BLLJIMP.Model.MemberInfo member in memberList)
                {
                    member.UserID = bllMember.UserID;
                    if (!bllMember.Exists(member, new List<string>() { "Mobile", "UserID"}))
                    {
                        if (!bllMember.Add(member))
                        {
                            this.ShowMessge("导入数据失败，请检查EXCEL文件内容。");
                            return;
                        }
                        ++importCount;
                    }
                }
                

                //if (!bllMember.AddList<ZentCloud.BLLJIMP.Model.MemberInfo>(memberList))
                //{
                //    this.ShowMessge("导入数据失败，请检查EXCEL文件内容。");
                //}

                grvData.DataSource = memberList;
                grvData.DataBind();
                lbUploadResult.Text = string.Format("导入成功，共读取{0}条记录，导入{1}条记录，其他记录已经存在！", memberList.Count,importCount);
                lbUploadResult.Visible = true;

                //删除临时目录
                Directory.Delete(tmpPath, true);
            }
            else
            {
                this.ShowMessge("只支持.xls文件");
                return;
            }
        }

        
    }
}