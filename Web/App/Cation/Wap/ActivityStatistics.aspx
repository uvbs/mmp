<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMain.Master" AutoEventWireup="true" CodeBehind="ActivityStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.ActivityStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div data-role="page" id="page-title" data-theme="b">
  <style type="text/css">

  .th
  {
     white-space:nowrap; 
    font-size: 16px;
   line-height : 36px;
   color:Black;
   font: 12px/1.5 "microsoft yahei",arial,\5b8b\4f53;
   font-weight:bold;

   }
   
   .td
   {
   font-size: 16px;
   line-height : 36px;
   color: #666;
   font: 12px/1.5 "microsoft yahei",arial,\5b8b\4f53;
       
   }
   .lblsigncount
   {
    color:Red;
    font-weight:bold;
       
    }
  
  </style>
        <div data-role="header" data-theme="b" data-position="fixed" style="" id="divTop">
            <a href="/FShare/Wap/MyPub.aspx" data-role="button"  data-icon="arrow-l" data-ajax="false">返回</a>
            <h1>
                <%Response.Write(juActivityModel.ActivityName); %>
                
            </h1>
            
            <div data-role="navbar">
        <ul>
            <li><a href="#" id="atitle"  >活动报名(<label class="lblsigncount"><%=signUpDataList.Count.ToString() %></label>人)<img style="vertical-align:middle;" src="/img/reload.png" width="18" height="18" /></a></li>
            
        </ul>
    </div>
        </div>
        <div data-role="content">
        <ul data-role="listview" data-filter="true" data-filter-placeholder="按姓名、手机查找..." data-inset="true">
            
            <%
                StringBuilder sbStr = new StringBuilder();
                int i = 1;
                foreach (var item in signUpDataList)
                {
                    sbStr.Append("<li>");

                    sbStr.AppendFormat("<a href=\"javascript:;\" onclick=\"ShowInfo({3},{4})\">{2}.{0}<label style=\"float:right;\">{1}</label></a>",
                            item.Name,
                            item.Phone,
                            i,
                            item.ActivityID,
                            item.UID
                        );
                    
                    sbStr.Append("</li>");
                    i++;
                }
                Response.Write(sbStr.ToString());
                %>

        </ul>
        <script type="text/javascript">

            function ShowInfo(ActivityID, UID) {
                //return;
                //var resp = $.parseJSON(data);
                $.mobile.loading('show', { textVisible: true, text: '加载中...' });
                $.ajax({
                    type: 'post',
                    url: "/Handler/JuActivity/JuActivityHandler.ashx",
                    data: { Action: "GetSingleJuActivitySignupInfo", aid: ActivityID, uid: UID },
                    timeout: 10000,
                    success: function (result, status) {
                        try {
                            $.mobile.loading('hide');
                            if (status = "success") {
                                $('#lbDlgMsg').html(result);
                                $('#dlgMsg').popup();
                                $('#dlgMsg').popup('open');

                            }
                            else {
                                alert("网络超时，请重试");
                            }



                        } catch (e) {
                            alert(e);
                        }
                    },
                    error: function () {
                        $.mobile.loading('hide');
                        alert("网络超时，请重试");
                    }
                });


            }
            $(function () {
                $("#atitle").bind("click", function (event, ui) {
                    $.mobile.loading('show', { textVisible: true, text: '正在刷新...' });
                    window.location = "/App/Cation/Wap/ActivityStatistics.aspx?jid=" + "<%=pubjid%>";

                });


            })
        </script>
        
        <div data-role="popup" id="dlgMsg" data-theme="d" data-overlay-theme="b" class="ui-content" style="padding: 20px; text-align: left; ">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
            <div id="lbDlgMsg" >


            </div>

        </div>
        </div>
    </div>
    <script type="text/javascript">
        
    </script>
</asp:Content>
