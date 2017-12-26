using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class SetPubID : System.Web.UI.Page
    {
        BLLJIMP.BLLUser userBll;
        BLLJIMP.BLLWeixin weixinBll;
        public UserInfo user;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Comm.DataLoadTool.GetCurrUserModel();

            userBll = new BLLJIMP.BLLUser(user.UserID);
            weixinBll = new BLLJIMP.BLLWeixin(user.UserID);

            if (!IsPostBack)
            {
                SystemSet systemset = weixinBll.Get<SystemSet>("");
                //this.lbURL.Text = user.WeixinAPIUrl;
                this.txtToken.Text = user.WeixinToken;
                this.txtWeinxinPublicName.Text = user.WeixinPublicName;
                this.lbURL.Text = string.Format("{0}/Weixin/OAuthPage.aspx?u={1}", systemset.weiXinAdDomain,Common.Base64Change.EncodeBase64ByUTF8(user.UserID));
                ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo menuModel = weixinBll.Get<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' and RuleType = 4 ", user.UserID));
                if (menuModel != null)
                {
                    this.txtMenuContent.Text = menuModel.ReplyContent;
                }
                txtAppId.Text = user.WeixinAppId;
                txtAppSecret.Text = user.WeixinAppSecret;

                this.rblIsWeixinVerify.SelectedValue = user.IsWeixinVerify.ToString();
                
                if (user.WeixinIsEnableMenu!=null)
	            {
                    if (user.WeixinIsEnableMenu==1)
                    {
                        rblEnableMenu.SelectedIndex = 0;
                    }
                    else
                    {
                        rblEnableMenu.SelectedIndex =1;
                    }
                    
	            }
                else
                {
                    rblEnableMenu.SelectedIndex =1;
                }
               

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

        protected void btnApiSet_Click(object sender, EventArgs e)
        {
            //链接：http://www.jubit.org/Weixin/OAuthPage.aspx?u=base64(userID)

            string token = this.txtToken.Text.Trim();

            if (string.IsNullOrEmpty(token))
            {
                this.ShowMessge("Token不能为空!");
                this.txtToken.Focus();
                return;
            }

            user = Comm.DataLoadTool.GetCurrUserModel();



            user.WeixinAPIUrl = this.lbURL.Text;
            user.WeixinToken = token;

            if (this.userBll.Update(user))
            {



                this.ShowMessge("保存成功!");
            }
            else
            {
                this.ShowMessge("保存失败!");
                this.lbURL.Text = "";
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            /*
            * 1.保存Token、链接、公众号名称，并且设置注册也验证状态都为开启状态；
            * 2.保存或者更改注册回复；
            * 3.保存或者更改菜单回复；
            * 
            */

            try
            {
                user = Comm.DataLoadTool.GetCurrUserModel();
                user.WeixinPublicName = this.txtWeinxinPublicName.Text.Trim();
                user.WeixinAPIUrl = this.lbURL.Text.Trim();
                user.WeixinToken = this.txtToken.Text.Trim();

                user.WeixinIsEnableMenu = int.Parse(rblEnableMenu.SelectedValue);
                user.IsWeixinVerify = int.Parse(rblIsWeixinVerify.SelectedValue);

                user.WeixinAppId = txtAppId.Text;
                user.WeixinAppSecret = txtAppSecret.Text;
                if (user.WeixinIsEnableMenu==0)
                {

                    weixinBll.DeleteWeixinClientMenu(weixinBll.GetAccessToken(user.UserID));

                }



                //关闭内定注册模块
                user.WeixinIsOpenReg = 0;
                user.WeixinRegIsVerifySMS = 0;

                this.userBll.Update(user);

                ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo regModel = weixinBll.Get<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' and RuleType = 3 ", user.UserID));

                if (regModel != null)
                {
                    regModel.MsgKeyword = this.txtRegKeyWord.Text.Trim();
                    this.weixinBll.Update(regModel);
                }
                else
                {
                    regModel = new WeixinReplyRuleInfo();
                    regModel.ReplyContent = "请输入手机号码进行注册";
                    regModel.ReplyType = "text";
                    regModel.ReceiveType = "text";
                    regModel.MatchType = "全文匹配";
                    regModel.UserID = user.UserID;
                    regModel.UID = this.weixinBll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
                    regModel.RuleType = 3;
                    regModel.MsgKeyword = this.txtRegKeyWord.Text.Trim();
                    regModel.CreateDate = DateTime.Now;
                    this.weixinBll.Add(regModel);
                }

                ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo menuModel = weixinBll.Get<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' and RuleType = 4 ", user.UserID));

                if (menuModel != null)
                {
                    menuModel.MsgKeyword = this.txtMenuKeyWord.Text.Trim();
                    menuModel.ReplyContent = this.txtMenuContent.Text.Trim();
                    this.weixinBll.Update(menuModel);
                }
                else
                {
                    menuModel = new WeixinReplyRuleInfo();
                    menuModel.ReplyContent = this.txtMenuContent.Text.Trim();
                    menuModel.ReplyType = "text";
                    menuModel.ReceiveType = "text";
                    menuModel.MatchType = "全文匹配";
                    menuModel.UserID = user.UserID;
                    menuModel.UID = this.weixinBll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
                    menuModel.RuleType = 4;
                    menuModel.MsgKeyword = this.txtMenuKeyWord.Text.Trim();
                    menuModel.CreateDate = DateTime.Now;
                    this.weixinBll.Add(menuModel);
                }

                //添加默认注册流程
                if (this.weixinBll.GetList<WXFlowInfo>(string.Format(" UserID = '{0}' and FlowSysType = 3", user.UserID)).Count.Equals(0))
                {
                    this.weixinBll.AddUserDefaultFlow(user.UserID);
                }

                this.ShowMessge("保存成功！");

            }
            catch (Exception ex)
            {
                this.ShowMessge("保存失败:" + ex.Message);
            }
        }
    }
}