<%@ Page Title="发布中心" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="ReleaseContent.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ReleaseContent.ReleaseContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/UserCenter/ReleaseContent/ReleaseContent.css?v=2016102002" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <%
        if (curUser.UserType == 6)
        {
            //公司发布
    %>
    <div class="Width1000 mTop40 company-release">
        <div class="head">
            <%
            if (string.IsNullOrEmpty(jid))
            {
                    %>
             <button type="button" name="btn" value="blogs" class="btn btn-default selected">公司发布</button>
                    <%
                }    else{
                    %>
            <div class="notice-head">
                       <span id="sTitle">编辑公司发布</span>
             </div>
                   <%
                }
             %>
           
            
        </div>
        <div class="content row">
            <form name="comment">
                <div class="col-xs-3 textRight">
                    <label class="control-label title">标题：</label></div>
                <div class="col-xs-6">
                    <input type="text" id="title" maxlength="50" class="form-control width400" placeholder="请输入标题" /></div>
                <div class="col-xs-3"></div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">图片：</label></div>
                <div class="col-xs-9 textLeft">
                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/companyPublish-img.png" id="companypublish_thumbnailsPath" class="img-rounded companypublish-img">
                    <input type="file" id="thumbnailsPath3" accept="image/*" name="file1" />
                </div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">内容：</label></div>
                <div class="col-xs-9 body-content">
                    <script id="companypublish" class="textarea" type="text/plain">
                          <%
                        if (model != null)
                        {
                            %>
                                <%=model.ActivityDescription %>
                            <%
                        }
                         %>

                    </script>
                </div>
            </form>
        </div>

        <div class="button">
            <button type="button" id="BtnSave" class="btn btn-default btn-add">发布</button>
            <button type="button" id="BtnClear" class="btn btn-default btn-reset">清空</button>
        </div>
    </div>
    <%
        }
        else
        {

            //个人发布
    %>
    <div class="Width1000 mTop40 user-release">
        <div class="head">
            <%
            if (!string.IsNullOrEmpty(jid))
            {
                %>
                    <div class="notice-head">
                       <span id="EditTitle"></span>
                    </div>
                    <button type="button" name="btn" value="blogs" class="btn btn-default" style="display:none;">发博客</button>
                    <button type="button" name="btn" value="stock" class="btn btn-default" style="display:none;">发股权</button>
                    <button type="button" name="btn" value="comment" class="btn btn-default" style="display:none;">发时评</button>
                <%
            }
            else
            {
                %>
                    <button type="button" name="btn" value="blogs" class="btn btn-default selected">发博客</button>
                    <button type="button" name="btn" value="stock" class="btn btn-default">发股权</button>
                    <button type="button" name="btn" value="comment" class="btn btn-default">发时评</button>
                <%
            }     
            %>
            
        </div>
        <div class="content-blogs row">
            <form name="blogs" onkeydown="if(event.keyCode==13)return false;">
                <div class="col-xs-3 textRight">
                    <label class="control-label title">标题：</label></div>
                <div class="col-xs-6">
                    <input type="text" id="blogs_title" maxlength="50" class="form-control width400" placeholder="请输入博客标题" /></div>
                <div class="col-xs-3"></div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">图片：</label></div>
                <div class="col-xs-9 textLeft">
                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/blogs-img.png" id="blogs_thumbnailsPath" class="img-rounded bowen-img">
                    <input type="file" id="thumbnailsPath1" accept="image/*" name="file1" />
                </div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">内容：</label></div>
                <div class="col-xs-9 body-content">
                    <script id="blogs" class="textarea" type="text/plain">
                     <%
                    if (model != null)
                    {
                         %>
                          <%=model.ActivityDescription %>
                         <%
                     }
                     %>
                    </script>
                </div>
            </form>
        </div>
        <div class="content-stock row" style="display: none;">
            <form name="stock">
                <div class="col-xs-3 textRight">
                    <label class="control-label title">标题：</label></div>
                <div class="col-xs-6">
                    <input type="text" id="stock_title" maxlength="50" class="form-control width400" placeholder="请输入股权交易标题" /></div>
                <div class="col-xs-3"></div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">股权数：</label></div>
                <div class="col-xs-6">
                    <input type="number" id="stock_number" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"  class="form-control width400" placeholder="请输入股权数" oninput="if(value.length>12)value=value.slice(0,12)" />
                </div>
                <div class="col-xs-3"></div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">类型：</label></div>
                <div class="col-xs-6">
                    <select class="form-control width400" id="stock_type">
                        <option value="">请选择类型</option>
                        <option>卖</option>
                        <option>买</option>
                    </select>
                </div>
                <div class="col-xs-3"></div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">图片：</label></div>
                <div class="col-xs-9 textLeft">
                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/stock-img.png" id="stock_thumbnailsPath" class="img-rounded stock-img">
                    <input type="file" id="thumbnailsPath2" accept="image/*" name="file1" />
                </div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">内容：</label></div>
                <div class="col-xs-9 body-content">
                    <script id="stocks" class="textarea" type="text/plain">
                     <%
                    if (model != null)
                    {
                         %>
                          <%=model.ActivityDescription %>
                         <%
                     }
                     %>
                        
                    </script>
                </div>
            </form>
        </div>
        <div class="content-comment row" style="display: none;">
            <form name="comment">
                <div class="col-xs-3 textRight">
                    <label class="control-label title">内容：</label></div>
                <div class="col-xs-9 body-content">
                    <textarea class="form-control textarea" id="comment_content" maxlength="200" placeholder="时评内容只能在200字以内" rows="15"></textarea>
                </div>
            </form>
        </div>

        <div class="button">
            <button type="button" id="BtnAdd"  class="btn btn-default btn-add">发布</button>
            <button type="button" id="BtnReset" class="btn btn-default btn-reset">清空</button>
        </div>

    </div>

    <%
        }     
    %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript">
    var jid='';
    <%
        if (!string.IsNullOrEmpty(jid))
        {
            %>
                    jid='<%=jid %>';
            <%
        }
    %>
    </script>

    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js"></script>
    <script type="text/javascript" src="/lib/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/lib/ueditor/ueditor.all.min.js"> </script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/UserCenter/ReleaseContent/ReleaseContent.js?v=20161123"></script>

</asp:Content>
