using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    /// <summary>
    /// 微信自动回复
    /// </summary>
    public partial class OAuthPage : System.Web.UI.Page
    {
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin("");
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 微信开放平台BLL
        /// </summary>
        BLLJIMP.BLLWeixinOpen bllWeixinOpen = new BLLJIMP.BLLWeixinOpen();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Tolog("Into WX OAuthPage");

                //if (!IsPostBack)
                //{
                #region 网站接入
                try
                {
                    if(bllWeixinOpen.IsAuthToOpen()&&bllUser.WebsiteOwner!="study")
                    {
                        Response.Write("");
                        return;
                    }
                }
                catch (Exception)
                {


                }


                string signature = Request["signature"];
                string timestamp = Request["timestamp"];
                string nonce = Request["nonce"];
                string echostr = Request["echostr"];
                string userIdEncode = Request["u"];

                if (string.IsNullOrWhiteSpace(userIdEncode))
                    return;

                //判断接入
                string userID = Common.Base64Change.DecodeBase64ByUTF8(userIdEncode);
                BLLJIMP.Model.UserInfo user = bllUser.GetUserInfo(userID);

                if (user == null)
                {
                    Response.Write("用户不存在！");
                    return;
                }

                if (string.IsNullOrWhiteSpace(user.WeixinToken))
                {
                    Response.Write("平台未绑定成功！");
                    return;
                }

                bool checkResult = BLLJIMP.BLLWeixin.Check(signature, timestamp, nonce, user.WeixinToken);

                //Tolog(string.Format("echostr:{0},checkResult:{1},Token:{2}", echostr, checkResult.ToString(),user.WeixinToken));

                if (!string.IsNullOrWhiteSpace(echostr))
                {
                    if (checkResult)
                    {
                        Response.Write(echostr);
                        return;
                    }
                    else
                    {
                        Response.Write("接入失败!");
                        return;
                    }
                }

                #endregion

                #region 测试1
                //XmlNode xmlNode;
                //using (StreamReader sr = new StreamReader(Request.InputStream))
                //{
                //    XmlDocument xmlDoc = new XmlDocument();
                //    xmlDoc.Load(sr.BaseStream);
                //    xmlNode = xmlDoc.SelectSingleNode("xml");
                //}

                //string ToUserName = xmlNode.SelectSingleNode("ToUserName").InnerText;
                //string FromUserName = xmlNode.SelectSingleNode("FromUserName").InnerText;
                //string CreateTime = xmlNode.SelectSingleNode("CreateTime").InnerText;
                //string MsgType = xmlNode.SelectSingleNode("MsgType").InnerText;
                //string Content = xmlNode.SelectSingleNode("Content").InnerText;
                //string MsgId = xmlNode.SelectSingleNode("MsgId").InnerText;

                //using (StreamWriter sw = new StreamWriter("C:\\test\\test.txt", true, Encoding.UTF8))
                //{
                //    sw.WriteLine(string.Format("【接收】ToUserName:{0},FromUserName:{1},CreateTime:{2},MsgType:{3},Content:{4},MsgId:{5}",
                //            ToUserName,
                //            FromUserName,
                //            CreateTime,
                //            MsgType,
                //            Content,
                //            MsgId
                //        ));
                //}

                //StringBuilder strResp = new StringBuilder();

                //strResp.AppendLine("<xml>");

                //strResp.AppendFormat("<ToUserName>{0}</ToUserName>", FromUserName);
                //strResp.AppendFormat("<FromUserName>{0}</FromUserName>", ToUserName);
                //strResp.AppendFormat("<CreateTime>{0}</CreateTime>", CreateTime);
                //strResp.AppendFormat("<MsgType>{0}</MsgType>", "news");
                //strResp.AppendFormat("<ArticleCount>4</ArticleCount>");

                //strResp.AppendFormat("<Articles>");
                ////图片信息
                //strResp.AppendFormat("<item>");
                //strResp.AppendFormat("<Title>{0}</Title>", "小小青柠檬壁纸世界");
                //strResp.AppendFormat("<Description>{0}</Description>", "分享收集生活中的美好，找到与你气味...");
                //strResp.AppendFormat("<PicUrl>{0}</PicUrl>", "http://www.jubit.org/weixin/wellcome2.jpg");
                //strResp.AppendFormat("<Url>{0}</Url>", "http://www.jubit.org/weixin/wellcome2.jpg");
                //strResp.AppendFormat("</item>");

                //strResp.AppendFormat("<item>");
                //strResp.AppendFormat("<Title>{0}</Title>", "风儿吹来 是我和天空的对白 其实幸福 一直与我们同在");
                //strResp.AppendFormat("<Description>{0}</Description>", "风儿吹来 是我和天空的对白 其实幸福 一直与我们同在");
                //strResp.AppendFormat("<PicUrl>{0}</PicUrl>", "http://www.jubit.org/weixin/1.jpeg");
                //strResp.AppendFormat("<Url>{0}</Url>", "http://www.jubit.org/weixin/a1.jpeg");
                //strResp.AppendFormat("</item>");

                //strResp.AppendFormat("<item>");
                //strResp.AppendFormat("<Title>{0}</Title>", "爱情就像沙漠被一片绿色淹没 开出最绚烂的花朵");
                //strResp.AppendFormat("<Description>{0}</Description>", "爱情就像沙漠被一片绿色淹没 开出最绚烂的花朵");
                //strResp.AppendFormat("<PicUrl>{0}</PicUrl>", "http://www.jubit.org/weixin/2.jpeg");
                //strResp.AppendFormat("<Url>{0}</Url>", "http://www.jubit.org/weixin/a2.jpeg");
                //strResp.AppendFormat("</item>");

                //strResp.AppendFormat("<item>");
                //strResp.AppendFormat("<Title>{0}</Title>", "千杯不醉 怎么我看你会千遍不厌");
                //strResp.AppendFormat("<Description>{0}</Description>", "千杯不醉 怎么我看你会千遍不厌");
                //strResp.AppendFormat("<PicUrl>{0}</PicUrl>", "http://www.jubit.org/weixin/3.jpeg");
                //strResp.AppendFormat("<Url>{0}</Url>", "http://www.jubit.org/weixin/a3.jpeg");
                //strResp.AppendFormat("</item>");

                //strResp.AppendFormat("</Articles>");

                //strResp.AppendFormat("<FuncFlag>1</FuncFlag>");

                //strResp.AppendLine("</xml>");

                //using (StreamWriter sw = new StreamWriter("C:\\test\\test.txt", true, Encoding.UTF8))
                //{
                //    sw.WriteLine(string.Format("【回复】{0}", strResp.ToString()));
                //}

                //Response.Write(strResp.ToString());
                #endregion

                string result = string.Empty;

                //checkResult = true;

                if (checkResult)
                {
                    result = bllWeixin.ActionResult(Request.InputStream, user.UserID);

                    bllWeixin.ToBLLWeixinLog("空处理前 result:" + result);

                    if (result.Contains("<Content><![CDATA[]]></Content>") && result.ToLower().Contains("<msgtype>text</msgtype>"))
                    {
                        result = result.Replace("<Content><![CDATA[]]></Content>", "").Replace("<MsgType>Text</MsgType>", "<MsgType><![CDATA[transfer_customer_service]]></MsgType>");
                       
                    }

                    bllWeixin.ToBLLWeixinLog("最终 result:" + result);

                    Response.Write(result);


                }
                else
                {
                    Response.Write("意外的推送信息！");
                    return;
                }

                //result = weixinBll.ActionResult(Request.InputStream, user.UserID);

                // Response.Write(result);

                //}
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);

                Tolog(string.Format("【{1}异常】{0}", ex.Message, DateTime.Now.ToString()));
            }
        }

        private void Tolog(string msg)
        {
            using (StreamWriter sw = new StreamWriter("D:\\log.txt", true, Encoding.UTF8))
            {
                sw.WriteLine(msg);
            }
        }


    }
}