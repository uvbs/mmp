using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
  
    public partial class WeixinReplyRuleInfo : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动回复html
        /// </summary>
        public string RelpayImageList
        {

            get
            {
               
                StringBuilder sb = new StringBuilder();
                var list=new BLL("").GetList<WeixinReplyRuleImgsInfo>(string.Format("RuleID = '{0}'", UID));
                if (list.Count>0)
	            {  

                    sb.AppendLine("<div style=\"border:1px solid;border-radius:5px;border-color:#CCCCCC;overflow:hidden;margin-top:10px;\">");

                    sb.Append("<table style=\"width:100%;margin-top:10px;margin-left:10px;\">");
                    sb.AppendLine("<tbody>");
			    
			        
			      for(int i = 0; i < list.Count; i++)
                
                {
                    try
                    {


                        sb.AppendFormat("<tr onclick=\"window.open('{0}')\">", list[i].Url);
                        sb.AppendFormat("<td style=\"width:80px;\" onclick=\"window.open('{0}')\">",list[i].Url);
                        sb.AppendFormat("<img src=\"{0}\" width=\"80px;\" height=\"80px;\" style=\"border-radius:5px;\" >", list[i].PicUrl);
                          sb.AppendLine("</td>");
                          sb.AppendLine("<td>");
                          sb.AppendFormat("<label style=\"margin-left:10px;\">{0}</label>", list[i].Title);
                          sb.AppendLine("</td>");
                          sb.AppendLine("</tr>");
                          if (i < list.Count - 1)
                          {
                              sb.AppendLine("<tr>");
                              sb.AppendLine("<td colspan=\"2\">");
                            
                              sb.AppendLine("<hr style=\"height:1px;border:0;border-bottom:1px solid #cccccc;\" />");
                              sb.AppendLine("</td>");
                              sb.AppendLine("</tr>");
                          }

                      
                      


                    }
                    catch (Exception)
                    {
                        continue;

                    }


                }

		          
                   // sb.Append("</table>");
                  sb.AppendLine("</tbody>");
                  sb.Append("</table>");
                  sb.AppendLine("</div>");
	            }

              

                return sb.ToString();
            }



        }


    }
}
