using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.IO;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class ReplyRuleSet : System.Web.UI.Page
    {
        UserInfo userInfo;
        BLLWeixin weixinBll;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userInfo = Comm.DataLoadTool.GetCurrUserModel();
            this.weixinBll = new BLLWeixin(userInfo.UserID);

            if (!IsPostBack)
            {
                this.btnDelete.Attributes.Add("onclick","return confirm(\"确定删除选中的规则?\");");

                this.LoadData();
            }

        }

        #region 显示提示框
        private void ShowMessge(string str)
        {
            Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.updatePanelMain, this.GetType(), str);
        }

        private void ShowMessge(string str, string url)
        {
            Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.updatePanelMain, this.GetType(), str, url);
        }
        #endregion

        protected void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = ((CheckBox)(grvData.HeaderRow.Cells[0].FindControl("cbAll"))).Checked;
            foreach (GridViewRow row in grvData.Rows)
            {
                ((CheckBox)(row.Cells[0].FindControl("cbRow"))).Checked = isChecked;
            }
        }

        /// <summary>
        /// 获取选择的UID集合
        /// </summary>
        /// <returns></returns>
        private List<string> GetSelectIds()
        {
            List<string> result = new List<string>();
            for (int i = 0; i <= grvData.Rows.Count - 1; i++)
            {
                CheckBox cbRow = (CheckBox)grvData.Rows[i].FindControl("cbRow");
                if (cbRow.Checked == true)
                {
                    result.Add(grvData.DataKeys[i].Value.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            this.grvData.DataSource = this.weixinBll.GetList<WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' ", userInfo.UserID));
            this.grvData.DataBind();
        }

        private void LoadImgData()
        {
            if (this.ViewState["imgData"] != null)
            {
                List<ZentCloud.BLLJIMP.Model.Weixin.Article> list = (List<ZentCloud.BLLJIMP.Model.Weixin.Article>)this.ViewState["imgData"];
                this.grvTmpImgData.DataSource = list;
                this.grvTmpImgData.DataBind();
            }
        }

        /// <summary>
        /// 保存规则
        /// </summary>
        private void SaveRule()
        {
            WeixinReplyRuleInfo model = new WeixinReplyRuleInfo();

            model.MsgKeyword = this.txtKeyWord.Text.Trim();

            if (CheckIsEmpty(model.MsgKeyword, "关键字不能为空!"))
            {
                this.txtKeyWord.Focus();
                return;
            }

            //判断关键字不能重复
            if (this.weixinBll.Exists(model, "MsgKeyword"))
            {
                this.ShowMessge("该关键字已经存在!");
                return;
            }

            switch (this.rdoListReplyType.SelectedItem.Text)
            {
                case "文本":
                    model.ReplyContent = this.txtTextContent.Text.Trim();
                    if (CheckIsEmpty(model.ReplyContent, "回复内容不能为空!"))
                    {
                        this.txtTextContent.Focus();
                        return;
                    }
                    model.ReplyType = "text";
                    model.UID = this.weixinBll.GetGUID(TransacType.WeixinReplyRuleAdd);

                    break;
                case "图文":
                    model.ReplyType = "news";
                    // 保存图文
                    List<BLLJIMP.Model.Weixin.Article> imgList = this.GetVistateImgList();

                    if (imgList.Count <= 0)
                    {
                        this.ShowMessge("图文消息图片数量不能为 0 !");
                        return;
                    }
                    model.UID = this.weixinBll.GetGUID(TransacType.WeixinReplyRuleAdd);

                    List<WeixinReplyRuleImgsInfo> imgModelList = new List<WeixinReplyRuleImgsInfo>();
                    int tmpOrderIndex = 0;
                    foreach (var item in imgList)
                    {
                        WeixinReplyRuleImgsInfo imgModel = new WeixinReplyRuleImgsInfo();
                        imgModel.UID = this.weixinBll.GetGUID(TransacType.WeixinReplyRuleImgAdd);
                        imgModel.RuleID = model.UID;
                        imgModel.Description = item.Description;
                        imgModel.OrderIndex = tmpOrderIndex;
                        imgModel.PicUrl = item.PicUrl;
                        imgModel.Title = item.Title;
                        imgModel.Url = item.Url;
                        tmpOrderIndex++;
                        imgModelList.Add(imgModel);
                    }

                    if (!this.weixinBll.AddList<WeixinReplyRuleImgsInfo>(imgModelList))
                    {
                        this.ShowMessge("保存图片失败!");
                        return;
                    }

                    break;
                default:
                    break;
            }

           
            model.UserID = this.userInfo.UserID;
            model.ReceiveType = "text";//目前默认只接收文本推送信息处理
            model.MatchType = this.ddlMatchType.SelectedItem.Text;
            model.CreateDate = DateTime.Now;

            if (this.weixinBll.Add(model))
            {
                //this.ShowMessge("添加成功!");

                this.ViewState["imgData"] = null;

                this.LoadData();

                this.ClearAddPanel();
                //this.HideAddPanel();
            }
            else
            {
                this.ShowMessge("添加失败!");
            }
        }

        /// <summary>
        /// 检查空值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool CheckIsEmpty(string value, string msg)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                this.ShowMessge(msg);
                return true;
            }
            return false;
        }

        #region 面板控制
        /// <summary>
        /// 显示添加面板
        /// </summary>
        private void ShowAddPanel()
        {
            this.panelAdd.Visible = true;
        }

        /// <summary>
        /// 隐藏添加面板
        /// </summary>
        private void HideAddPanel()
        {
            this.txtKeyWord.Text = "";
            this.txtTextContent.Text = "";
            this.panelAdd.Visible = false;
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void ClearAddPanel()
        {
            this.txtKeyWord.Text = "";
            this.txtTextContent.Text = "";
            this.txtImgDescription.Text = "";
            this.txtImgNavUrl.Text = "";
            this.txtImgTitle.Text = "";
        }

        private void ShowTextPanel()
        {
            this.panelText.Visible = true;
        }

        private void HideTextPanel()
        {
            this.panelText.Visible = false;
        }

        private void ShowNewsPanel()
        {
            this.panelNews.Visible = true;
        }

        private void HideNewsPanel()
        {
            this.panelNews.Visible = false;
        }
        #endregion

        protected void btnCreateNew_Click(object sender, EventArgs e)
        {
            //this.ShowAddPanel();
            this.SaveRule();
        }


        protected void btnSaveRule_Click(object sender, EventArgs e)
        {
            this.SaveRule();
        }

        protected void btnCancelRule_Click(object sender, EventArgs e)
        {
            this.HideAddPanel();
        }

        protected void rdoListReplyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.rdoListReplyType.SelectedItem.Text)
            {
                case "文本":
                    this.ShowTextPanel();
                    this.HideNewsPanel();
                    break;
                case "图文":
                    this.ShowNewsPanel();
                    this.HideTextPanel();
                    break;
                default:
                    break;
            }
        }

        protected void btnAddImg_Click(object sender, EventArgs e)
        {
            List<ZentCloud.BLLJIMP.Model.Weixin.Article> imgList = new List<BLLJIMP.Model.Weixin.Article>();

            if (this.ViewState["imgData"] != null)
            {
                imgList = (List<ZentCloud.BLLJIMP.Model.Weixin.Article>)this.ViewState["imgData"];
            }

            ZentCloud.BLLJIMP.Model.Weixin.Article article = new BLLJIMP.Model.Weixin.Article();

            if (!Common.IOHelper.CheckFileName(this.fileImg.FileName, "jpg|png"))
            {
                this.ShowMessge("图片错误!");
                return;
            }
            if (this.CheckIsEmpty(this.txtImgTitle.Text.Trim(), "图片标题不能为空!"))
            {
                this.txtImgTitle.Focus();
                return;
            }
            if (this.CheckIsEmpty(this.txtImgDescription.Text.Trim(), "图片描述不能为空!"))
            {
                this.txtImgDescription.Focus();
                return;
            }
            if (this.CheckIsEmpty(this.txtImgNavUrl.Text.Trim(), "跳转链接不能为空!"))
            {
                this.txtImgNavUrl.Focus();
                return;
            }

            string extName = Common.IOHelper.GetExtraName(this.fileImg.FileName);
            string filePath = Server.MapPath("~/FileUpload/Weixin/img/" + this.userInfo.UserID);
            string fileName = Guid.NewGuid() + "." + extName;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            this.fileImg.SaveAs(string.Format("{0}/{1}", filePath, fileName));


            article.PicUrl = "http://" + this.Request.Url.Host + ":" + this.Request.Url.Port.ToString() + "/FileUpload/Weixin/img/" + this.userInfo.UserID + "/" + fileName;
            article.Title = this.txtImgTitle.Text.Trim();
            article.Description = this.txtImgDescription.Text.Trim();
            article.Url = this.txtImgNavUrl.Text.Trim();

            imgList.Add(article);

            this.ViewState["imgData"] = imgList;

            this.txtImgDescription.Text = "";
            this.txtImgNavUrl.Text = "";
            this.txtImgTitle.Text = "";

            this.LoadImgData();

        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string ids = Common.StringHelper.ListToStr<string>(this.GetSelectIds(), "'", ",");
            this.ShowMessge(ids);

            this.weixinBll.Delete(new BLLJIMP.Model.WeixinReplyRuleInfo(), string.Format(" UID IN ({0})", ids));//删除规则

            //TODO:查出所有图片并删除文件

            this.weixinBll.Delete(new BLLJIMP.Model.WeixinReplyRuleImgsInfo(), string.Format(" RuleID IN ({0}) ",ids)); ;//删除图片

            this.LoadData();
            //Common.MessageBox.Show(this, ids);

        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvTmpImgData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            List<ZentCloud.BLLJIMP.Model.Weixin.Article> imgList = this.GetVistateImgList();

            imgList.RemoveAt(e.RowIndex);

            this.SetVistateImgList(imgList);

            this.LoadImgData();

        }

        /// <summary>
        /// 获取缓存图片
        /// </summary>
        /// <returns></returns>
        private List<BLLJIMP.Model.Weixin.Article> GetVistateImgList()
        {
            List<ZentCloud.BLLJIMP.Model.Weixin.Article> imgList = new List<BLLJIMP.Model.Weixin.Article>();

            if (this.ViewState["imgData"] != null)
            {
                imgList = (List<ZentCloud.BLLJIMP.Model.Weixin.Article>)this.ViewState["imgData"];
            }

            return imgList;
        }
        /// <summary>
        /// 设置缓存图片
        /// </summary>
        /// <param name="imgList"></param>
        private void SetVistateImgList(List<BLLJIMP.Model.Weixin.Article> imgList)
        {
            this.ViewState["imgData"] = imgList;
        }


    }
}