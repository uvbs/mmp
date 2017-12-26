<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ArticleCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.ArticleCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="http://static-files.socialcrmyun.com/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="http://static-files.socialcrmyun.com/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/static-modules/app/admin/article/style.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/jquery/mCustomScrollbar/jquery.mCustomScrollbar.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/lib/color/spectrum/spectrum.css" rel="stylesheet" />
    <link href="/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.css" rel="stylesheet" />
    <link href="/Weixin/ArticleTemplate/css/weixin.css" rel="stylesheet" />
    <style type="text/css">
        @charset "utf-8";
        @import url('<%= new ZentCloud.BLLJIMP.BLLCompanyWebSite().GetIcoFilePath()%>');

        #divRelationArticle {
            font-weight: bold;
        }

        .divFileClass {
        }

        .divFileParent {
            position: relative;
        }

        .divFileName {
            padding-right: 30px;
            line-height: 18px;
        }

            .divFileName a {
                color: blue;
            }

        .deleteFile {
            position: absolute;
            top: 0px;
            right: 0px;
            cursor: pointer;
        }

            .deleteFile img {
                width: 18px;
                height: 18px;
            }

        .delrelation {
            margin-left: 10px;
            color: Blue;
        }

        .rightSlide {
            position: absolute;
            width: 100px;
            top: 24%;
            left: 80%;
        }

            .rightSlide .listyle {
                width: 82px;
                height: 28px;
                line-height: 26px;
                cursor: pointer;
                background-color: #C6C3BD;
                border-right-radius: 50%;
                -webkit-border-top-right-radius: 40px;
                -webkit-border-bottom-right-radius: 40px;
                text-align: left;
                margin-bottom: 10px;
                color: #fff;
                padding-left: 5px;
            }

            .rightSlide .listyleDeep {
                background-color: #46c8ff;
            }

        #dlgIosSkin {
            width: 300px;
            height: 622px;
            border-radius: 30px;
            padding: 10px;
            box-sizing: border-box;
            position: relative;
            background-color: #fff;
            box-shadow: 0 0 0 2px #eedeca, inset 0 0 4px #eedeca;
            left: 0;
            display: none;
        }

            #dlgIosSkin .preview-title {
                text-align: center;
                height: 10%;
            }

                #dlgIosSkin .preview-title .ico {
                    position: absolute;
                }

                #dlgIosSkin .preview-title .circle {
                    border-radius: 50%;
                }

                #dlgIosSkin .preview-title .ico-1 {
                    width: 6px;
                    height: 6px;
                    background-color: #000;
                    left: 50%;
                }

                #dlgIosSkin .preview-title .ico-2 {
                    width: 50px;
                    height: 6px;
                    background-color: #000;
                    left: 50%;
                    margin-left: -25px;
                    top: 30px;
                    border-radius: 3px;
                }

                #dlgIosSkin .preview-title .ico-3 {
                    width: 10px;
                    height: 10px;
                    background-color: #000;
                    left: 50%;
                    margin-left: -45px;
                    top: 28px;
                }

            #dlgIosSkin .preview-content {
                height: calc(69% - 50px);
                border: 1px solid #ccc;
                width: 92%;
                font-size: 12px !important;
            }

            #dlgIosSkin .preview-footer {
                text-align: center;
                height: 10%;
            }

                #dlgIosSkin .preview-footer .circle {
                    width: 44px;
                    height: 44px;
                    border: 2px solid rgb(238, 222, 202);
                    margin-top: 14px;
                    margin-left: 114px;
                    border-radius: 50%;
                }

        body .layer-ext-iosskin {
            top: 20px !important;
            border-radius: 30px !important;
            overflow: hidden !important;
        }

            body .layer-ext-iosskin .layui-layer-content {
                height: 532px !important;
                overflow: hidden !important;
            }

        .lbTip {
            display: inline-block;
            padding: 0 6px;
            margin: 6px;
            background-color: #636060;
            color: #fff;
            font-size: 14px !important;
            border-radius: 50px;
            cursor: pointer;
            line-height: 1.42857143;
        }

        .layui-layer-tips i.layui-layer-TipsL, .layui-layer-tips i.layui-layer-TipsR {
            border-bottom-color: #5C5566 !important;
        }

        .layui-layer-content {
            background-color: #5C5566 !important;
        }

        .dcolor {
            background-color: #0face0 !important;
        }

            .dcolor:hover {
                background-color: #0face0;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="javascript:;"><%=moduleName %>管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%=moduleName %><%if (model != null && webAction == "edit") { Response.Write("：" + model.ActivityName); } %></span>
    <a href="ArticleManage.aspx" style="float: right; margin-right: 20px;" title="返回文章列表"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%;">
            <table id="mainTable">
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">
                        <%=moduleName %>标题：
                    </td>
                    <td align="left">
                        <input type="text" id="txtActivityName" class="commonTxt" placeholder="<%=moduleName %>标题(必填)" maxlength="150" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">描述：
                    </td>
                    <td align="left">
                        <input type="text" id="txtSummary" class="commonTxt" placeholder="描述" maxlength="300" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">缩略图：
                    </td>
                    <td align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath"
                            class="rounded" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">跳转URL：
                    </td>
                    <td align="left">
                        <input type="text" id="txtRedirectUrl" class="commonTxt" placeholder="设置外部URL，文章将直接跳转到外部链接，忽略编辑器内容" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            <%=moduleName %>详情：</label>
                    </td>
                    <td align="left">
                        <%--<span id="tabs">
                            <button class="button tab dcolor">默认详情</button>
                        </span>

                        <a href="javascript:;" class="easyui-linkbutton" id="addTab" iconcls="icon-add2" plain="true">添加</a>

                        <span class="lbTip" data-tip-msg="<b>示例</b><br><img src='/img/example/article_desc.png'/><br>">?</span>--%>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="vertical-align: top;">
                                    <div id="zcWeixinKindeditor" style="float: right;"></div>
                                </td>
                                <td style="vertical-align: top; position: relative;">
                                    <div id="txtEditor">
                                    </div>
                                    <div class="rightSlide">
                                        <ul>
                                            <%--   <li class="listyle" style="display: none">
                                                <span class="icon iconfont icon-iconfontjihualiebiao"></span>
                                                <span style="margin-left: 5px;">复制</span>
                                            </li>
                                            <li class="listyle" style="display: none">
                                                <span class="icon iconfont icon-xingye"></span>
                                                <span style="margin-left: 5px;">清空</span>
                                            </li>--%>
                                            <li class="listyle listyleDeep" onclick="importMpArticle()" style="height: auto; width: 100px;">
                                                <span style="margin-left: 5px; height: auto;">公众号文章导入</span>
                                            </li>
                                            <li class="listyle listyleDeep" onclick="saveData()">
                                                <span style="margin-left: 25px;">保存</span>
                                            </li>
                                            <li class="listyle listyleDeep" id="btnIosPreview">
                                                <%--<span class="icon iconfont icon-liulan"></span>--%>
                                                <span style="margin-left: 25px;">预览</span>
                                            </li>

                                            <li class="listyle">
                                                <span style="margin-left: 25px;" onclick="ClearEditer()">清空</span>
                                            </li>

                                            <%-- <li class="listyle" style="display: none">
                                                <span class="icon iconfont icon-xinwenzixun"></span>
                                                <span style="margin-left: 5px;">抓取图文</span>
                                            </li>
                                            <li class="listyle" style="display: none">
                                                <span class="icon iconfont icon-article"></span>
                                                <span style="margin-left: 5px;">存为模板</span>
                                            </li>--%>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="file1.click()">附件</a>
                    </td>
                    <td align="left" style="border-bottom: solid grey 1px;">
                        <div class="ui-sortable divFileClass divFileClass1">
                            <% foreach (var item in nFiles.Where(p => p.FileClass == 1))
                               {%>
                            <div class="divFileParent">
                                <div class="divFileName" data-id="<%=item.AutoId %>"><a href="<%=item.FilePath %>" target="_blank"><%=item.FileName %></a></div>
                                <div class="deleteFile">
                                    <img src="/img/delete.png" alt="删除" />
                                </div>
                            </div>
                            <% }%>
                        </div>
                        <div class="clear">
                            <input type="file" id="file1" class="file file1" name="file1" style="display: none;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle"></td>
                    <td align="left">支持格式：doc,docx,xls,xlsx,ppt,pdf,txt,zip,rar。
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

                <%if (WebsiteOwner!="meifan"){%>
                  <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">
                        <%=moduleName %>分类：
                    </td>
                    <td align="left">
                        <%=sbCategory.ToString()%>
                    </td>
                </tr>
                      
                  <%}
                  else
                  {%>
                      <tr style="display:none;">
                    <td style="width: 50px;" align="right" class="tdTitle">
                        <%=moduleName %>分类：
                    </td>
                    <td align="left">
                        <%=sbCategory.ToString()%>
                    </td>
                </tr>


                  <%}%>


                
                <tr <%=isHideTag == 1&&WebsiteOwner!="meifan"? "style=\"display:none\"":"" %>>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr <%=(isHideTag == 1||WebsiteOwner=="meifan")? "style=\"display:none\"":"" %>>
                    <td style="width: 50px; vertical-align: top;" align="right" class="tdTitle">
                        <%=moduleName %>标签：
                    </td>
                    <td align="left">
                        <input id="txtTags" />
                        <a href="javascript:;" class="button button-primary button-rounded button-small"
                            id="btnSelectTags">选择标签</a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>



                <%if (WebsiteOwner!="meifan"){%>
                  
                                      <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">所属地区：
                    </td>
                    <td align="left">
                        <select id="selectProvince">
                        </select>
                        <select id="selectCity">
                        </select>
                        <select id="selectArea">
                        </select>
                    </td>
                </tr>
                  <%}
                  else
                  {%>
                    <tr style="display:none;">
                    <td style="width: 50px;" align="right" class="tdTitle">所属地区：
                    </td>
                    <td align="left">
                        <select id="selectProvince">
                        </select>
                        <select id="selectCity">
                        </select>
                        <select id="selectArea">
                        </select>
                    </td>
                </tr>
                  <%}%>

                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">
                        <%=moduleName %>模板：
                    </td>
                    <td align="left">
                        <select id="ddlarticletemplate" style="width: 200px">
                            <option value="6">清新模板</option>
                            <option value="11">微信模板</option>
                            <option value="1">默认模板</option>
                            <option value="0">无</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">状态：
                    </td>
                    <td align="left">
                        <input type="radio" name="IsHide" id="rdoIsNotHide" checked="checked" v="0" /><label
                            for="rdoIsNotHide">显示</label>
                        <input type="radio" name="IsHide" id="rdoIsHide" v="1" /><label for="rdoIsHide">隐藏</label>
                    </td>
                </tr>
                <tr class="hidden">
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 50px;" align="right" class="tdTitle">显示在 作者的其它发布：
                    </td>
                    <td align="left">
                        <input type="radio" name="IsHideRecommend" id="rdShowRecommend" v="0" /><label for="rdShowRecommend">显示</label>
                        <input type="radio" name="IsHideRecommend" id="rdHideRecommend" v="1" checked="checked" /><label
                            for="rdHideRecommend">不显示</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="right">阅读数:
                    </td>
                    <td>
                        <input type="text" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" id="txtPv" value="<%=model.PV %>" class="commonTxt" placeholder="阅读数"
                            style="width: 100px;" />
                    </td>
                </tr>
                <%for (var i = 1; i <= 10; i++)
                  {%>
                <%
                      var fieldMap = this.tableFieldList.FirstOrDefault(p => p.Field == "K" + i.ToString());
                      var isShow = fieldMap != null;
                      if (!isShow) fieldMap = new ZentCloud.BLLJIMP.Model.TableFieldMapping();            
                %>
                <tr <%=isShow? "":"style=\"display:none\"" %>>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr <%=isShow? "":"style=\"display:none\"" %>>
                    <td style="width: 50px;" align="right" class="tdTitle">
                        <%=isShow? fieldMap.MappingName:"" %>：
                    </td>
                    <td align="left">
                        <input type="<%=fieldMap.FieldType == "6"? "date":"text" %>" id="txtK<%=i %>" class="commonTxt"
                            <%=isShow? "placeholder=\"请输入" + fieldMap.MappingName + "\"":"" %> />
                    </td>
                </tr>
                <%} %>

                <%if (WebsiteOwner!="meifan"){%>
                                  <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">访问权限级别：
                    </td>
                    <td align="left">
                        <input type="text" id="txtAccessLevel" class="commonTxt" value="<%=model.AccessLevel %>"
                            placeholder="访问权限级别" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'') " />&nbsp;权限访问级别为数字，数值越高用户需要的级别越高(默认为0,即所有用户都可以访问)
                        <a href="/App/Cation/UserManage.aspx" style="color: blue;">设置用户访问级别</a>
                    </td>
                </tr>
                      


                  <%}else{%>
                      <tr style="display:none;">
                    <td style="width: 50px;" align="right" class="tdTitle">访问权限级别：
                    </td>
                    <td align="left">
                        <input type="text" id="txtAccessLevel" class="commonTxt" value="<%=model.AccessLevel %>"
                            placeholder="访问权限级别" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'') " />&nbsp;权限访问级别为数字，数值越高用户需要的级别越高(默认为0,即所有用户都可以访问)
                        <a href="/App/Cation/UserManage.aspx" style="color: blue;">设置用户访问级别</a>
                    </td>
                </tr>


                  <%} %>

                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

                <%if (WebsiteOwner!="meifan"){%>
                <tr >
                    <td style="width: 100px;" align="right" valign="middle">有无评论：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <input id="rdoHaveComment0" type="radio" name="rdoHaveComment" <%= model.HaveComment==0?"checked='checked'":"" %>
                            value="0" /><label for="rdoHaveComment0">默认</label>
                        <input id="rdoHaveComment1" type="radio" name="rdoHaveComment" <%= model.HaveComment==1?"checked='checked'":"" %>
                            value="1" /><label for="rdoHaveComment1">有评论</label>
                        <input id="rdoHaveComment2" type="radio" name="rdoHaveComment" <%= model.HaveComment==2?"checked='checked'":"" %>
                            value="2" /><label for="rdoHaveComment2">无评论</label>
                    </td>
                </tr>
                 <%} %>

                <%-- <tr>
                    <td style="width: 50px;" align="right" class="tdTitle"></td>
                    <td  align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat">重置</a>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" class="tdTitle">相关阅读：
                         <a style="padding-left: 10px; padding-right: 10px;padding-left: 10px; padding-right: 10px;" href="javascript:;" class="button button-primary button-rounded button-small"
                        onclick="ShowArticleDlg();" >选择相关阅读</a>
                        <a href="javascript:;" class="button button-primary button-rounded button-small"
                            style="padding-left: 10px; margin-top: 10px; padding-right: 10px;font-size:10px;" onclick="clearRead()">相关阅读全部清除</a>
                    </td>
                    <td align="left">
                        <div id="divRelationArticle">
                            <%
                                StringBuilder sbRelation = new StringBuilder();
                                foreach (var item in RelationArticle)
                                {
                                    sbRelation.AppendFormat("<div data-relationarticleid=\"{0}\">", item.JuActivityID);
                                    sbRelation.AppendFormat(item.ActivityName);
                                    sbRelation.AppendFormat("<label class=\"delrelation\">删除</label> ");
                                    sbRelation.AppendFormat("</div> ");
                                    sbRelation.AppendFormat("<br/>");
                                }
                                Response.Write(sbRelation.ToString());
                            %>
                        </div>
                    </td>
                </tr>
            </table>
            <div style="margin-top: 32px; padding-top: 16px; padding-bottom: 16px; text-align: center; background-color: rgb(245, 245, 245); position: relative;">
                <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;"
                    class="button button-rounded button-flat">重置</a>
            </div>
        </div>
    </div>
    <div class="hidden warpTagDiv" style="border-radius: 8px;">
        <div class="warpTagSelect">
            <div class="warpContent">
                <div class="warpTagDataList hidden">
                    <div class="warpTagSelectBtn">
                        <a href="javascripe:;" class="mLeft15 btnTagSelect" data-op="all">全选</a><a href="javascripe:;"
                            class="mLeft10 btnTagSelect" data-op="reverse">反选</a>
                    </div>
                    <ul class="ulTagList">
                    </ul>
                </div>
                <div class="warpNoData">
                    暂无数据
                </div>
                <div class="clear">
                </div>
            </div>
            <hr />
            <div class="warpOpeate">
                <a href="javascript:;" class="button button-primary button-rounded button-small btnSave">确定</a> <a href="javascript:;" class="button button-rounded button-small btnCancel">取消</a>
            </div>
        </div>
    </div>
    <div id="dlgArticle" class="easyui-dialog" closed="true" modal="true" title="选择文章或活动"
        style="width: 550px; height: 400px;">
        <br />
        <div>
            &nbsp<input type="text" id="txtArticle" placeholder="标题" style="width: 300px; height: 18px;">
            <a class="easyui-linkbutton" iconcls="icon-search" id="btnSearchArticle">搜索</a>
        </div>
        <br />
        <table id="grvArticle" fitcolumns="true">
        </table>
    </div>
    <div id="dlgIosSkin">
        <div class="preview-title">
            <div class="ico ico-1 circle"></div>
            <div class="ico ico-2"></div>
            <div class="ico ico-3 circle"></div>
        </div>
        <div class="preview-content">
            <div class="item">
            </div>
        </div>
        <div class="preview-footer">
            <div class="circle">
            </div>
        </div>
    </div>

    <div id="dlgImportMpArticle" class="easyui-dialog" closed="true" modal="true" title="输入公众号文章链接"
        style="width: 400px; height: 160px;">
        <div>
            <textarea id="txtMpArticleUrl" placeholder="请输入公众号文章链接" rows="5" style="width: 98%;"></textarea>
        </div>

    </div>
    <div id="dlgTabTitle" class="easyui-dialog" closed="true" modal="true" title="请输入Tab标题"
        style="width: 400px; height: 160px;">
        <div style="margin: 25px;">
            <label>Tab标题:</label>
            <input type="text" style="width: 250px; margin-left: 10px;" id="tabTitle" />

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="http://static-files.socialcrmyun.com/static-modules/lib/tagsinput/jquery.tagsinput.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/static-modules/lib/chosen/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/lib/jquery/mCustomScrollbar/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/lib/color/spectrum/spectrum.js" type="text/javascript"></script>
    <script src="/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/lib/layer/2.1/layer.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx",
            currAction = '<%=webAction %>',
            currAcvityID = '<%=model.JuActivityID %>',
            editor,
            currTagsStr = '<%=model.Tags %>',
            currTags = [],
            $document = $(document),
            $txtTags = $document.find('#txtTags'),
            $warpTagSelect = $document.find('.warpTagSelect');
            var cateRootId = '<%=cateRootId%>';


        var zcWeixinKindeditor;

       

        init();

        function init() {

            $('.lbTip').click(function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, '.lbTip');
            });

           




            


            areaBind();

            //处理初始化tags
            if (currTagsStr != '') {
                currTags = currTagsStr.split(',');
            }

            $txtTags.tagsInput({
                height: '60px',
                width: 'auto',
                interactive: false,
                onAddTag: function (tag) {
                    //currTags.push(tag);
                    console.log('添加了' + tag);
                },
                onRemoveTag: function (tag) {
                    currTags.RemoveItem(tag);
                    console.log('删除了' + tag);
                }
            });

            addTagList(currTags);

            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            } else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }

            if (currAction == 'add') {
                GetRandomHb();

            } else {
                ShowEdit();
            }

            bindEvent();

            var selectControls = [
              { id: '#ddlcategory', placeholder: '选择<%=moduleName %>分类', width: '200px' },
              { id: '#ddlarticletemplate', placeholder: '选择<%=moduleName %>模板', width: '200px' },
            //{ id: '#selectProvince', placeholder: '选择省份', width: '100px' },
              { id: '#selectCity', placeholder: '选择城市', width: '100px' },
              { id: '#selectArea', placeholder: '选择地区', width: '100px' }
            ]

            for (var i = 0; i < selectControls.length; i++) {
                chosenBind(selectControls[i]);
            }

            //
            $('#dlgArticle').dialog({
                //top: 1000,
                modal: false,
                buttons: [{
                    text: '添加',
                    handler: function () {
                        var rows = $('#grvArticle').datagrid('getSelections');
                        if (rows.length == 0) {
                            //                            $("#hdRelationArticle").val("");
                            //                            $("#divRelationArticle").html("");
                            //                            $('#dlgArticle').dialog('close');
                            return false;
                        }
                        var sbRelation = new StringBuilder();
                        // var ids = [];
                        for (var i = 0; i < rows.length; i++) {
                            // ids.push(rows[i].JuActivityID);
                            if (i > 10) continue;
                            sbRelation.AppendFormat("<div data-relationarticleid=\"{0}\">", rows[i].JuActivityID);
                            sbRelation.AppendFormat(rows[i].ActivityName);
                            sbRelation.AppendFormat("<label class=\"delrelation\">删除</label> ");
                            sbRelation.AppendFormat("</div> ");
                            sbRelation.AppendFormat("<br/>");
                        }
                        // $("#hdRelationArticle").val(ids.join(','));
                        $("#divRelationArticle").append(sbRelation.ToString());
                        $('#dlgArticle').dialog('close');

                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgArticle').dialog('close');
                    }
                }]
            });

            //
            //
            $('#dlgImportMpArticle').dialog({
                //top: 1000,
                modal: false,
                buttons: [{
                    text: '导入',
                    handler: function () {
                        if ($("#txtMpArticleUrl").val() == "") {
                            $("#txtMpArticleUrl").focus();
                            return;
                        }

                        $.messager.progress({
                            text: '正在导入,请稍候...'
                        });

                        $.ajax({
                            type: "post",//请求方式
                            url: handlerUrl,//发送请求地址
                            data: { Action: "ImportMpArticle", url: $("#txtMpArticleUrl").val() },
                            timeout: 300000,//超时时间：30秒
                            dataType: "text",//设置返回数据的格式
                            //请求成功后的回调函数 data为json格式
                            success: function (result) {
                                $.messager.progress("close");
                                if (result.length > 0) {
                                    //取内容区域的html
                                    result = $.trim($(result).find('#js_content').html());

                                    editor.html(result);
                                    $('#dlgImportMpArticle').dialog('close');
                                }
                                else {
                                    alert("未获取到内容");
                                }
                            },
                            //请求出错的处理
                            error: function () {
                                alert("请求出错");
                                $.messager.progress("close");
                            }
                        });


                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgImportMpArticle').dialog('close');
                    }
                }]
            });



            $("#btnSearchArticle").click(function () {

                $('#grvArticle').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "article", ActivityName: $("#txtArticle").val() }
	            });

            })

            //
            $("#grvArticle").datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityForWeb", ArticleType: "article", ArticleTypeEx1: "", CategoryId: 0 },
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                {
                                    field: 'ThumbnailsPath', title: '缩略图', width: 10, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'ActivityName', title: '标题', width: 85, align: 'left'
                                }

	                ]]
	            }
            );
            //


        }

        function chosenBind(data) {
            $document.find(data.id).attr('data-placeholder', data.placeholder).chosen({
                no_results_text: '没有找到结果',
                width: data.width
            });
        }

        function getAreaSelectOptionDom(data) {
            var strHtml = new StringBuilder();
            strHtml.Append('<option value=""></option>');
            for (var i = 0; i < data.length; i++) {
                strHtml.AppendFormat('<option value="{0}">{1}</option>', data[i].DataKey, data[i].DataValue);
            }
            return strHtml.ToString();
        }

        function areaBind() {

            //初始化处理省份选项
            setProvinceSelect();

            $document.on('change', '#selectProvince', function () {
                //选择省份
                setCitySelect($(this).val());
            });

            $document.on('change', '#selectCity', function () {
                //选择城市
                var selectCode = $(this).val();
                setAreaSelect(selectCode);
            });
        }

        function setProvinceSelect(selectVal) {

            try {
                var data = zymmp.location.getProvince();
                if (selectVal) {
                    $document.find('#selectProvince').chosen('destroy');
                }
                $document.find('#selectProvince').html('').append(getAreaSelectOptionDom(data));
                if (selectVal) {
                    $document.find('#selectProvince').val(selectVal);
                }
                chosenBind({
                    id: '#selectProvince',
                    placeholder: '选择省份',
                    width: '120px'
                });
            } catch (e) {
                console.log('setProvinceSelect', e);
            }

        }

        function setCitySelect(provinceCode, selectVal) {

            try {
                var selectCode = provinceCode, cityData = zymmp.location.getCity(selectCode);

                $document.find('#selectCity').chosen('destroy').html('').append(getAreaSelectOptionDom(cityData));
                $document.find('#selectArea').chosen('destroy').html('<option value=""></option>');

                if (selectVal) {
                    $document.find('#selectCity').val(selectVal);
                }
                chosenBind({
                    id: '#selectCity',
                    placeholder: '选择城市',
                    width: '120px'
                });
                chosenBind({
                    id: '#selectArea',
                    placeholder: '选择地区',
                    width: '120px'
                });

                if (cityData.length > 0) {
                    $('#selectCity_chosen').css({ visibility: 'visible' });
                    $('#selectArea_chosen').css({ visibility: 'visible' });
                } else {
                    $('#selectCity_chosen').css({ visibility: 'hidden' });
                    $('#selectArea_chosen').css({ visibility: 'hidden' });
                }
            } catch (e) {
                console.log('setCitySelect', e);
            }

        }

        function setAreaSelect(cityCode, selectVal) {

            try {
                var selectCode = cityCode, areaData = zymmp.location.getDistrict(selectCode);
                $document.find('#selectArea').chosen('destroy').html('').append(getAreaSelectOptionDom(areaData));
                if (selectVal) {
                    $document.find('#selectArea').val(selectVal);
                }
                chosenBind({
                    id: '#selectArea',
                    placeholder: '选择地区',
                    width: '120px'
                });
            } catch (e) {
                console.log('setCitySelect', e);
            }

        }

        function bindEvent() {

            $document.on('click', '#btnSave', function () {
                saveData();
            });

            $document.on('click', '#btnReset', function () { ResetCurr(); });

            $document.on('change', '#txtThumbnailsPath', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });
            $('.deleteFile').live('click', function () {
                var fobj = $(this).closest('.divFileParent');
                var fname = $(fobj).find('.divFileName a').text();
                $.messager.confirm('系统提示', '确定删除附件 [' + fname + ']？', function (o) {
                    if (o) {
                        $(fobj).remove();
                    }
                });
            });
            $(".file").live('change', function () {
                var fpath = $.trim($(this).val());
                var fns = fpath.lastIndexOf("\\");
                var fname = fpath.substring(fns + 1);
                var fid = $.trim($(this).attr("id"));
                if (fpath == "") return;
                var divClass = $(this).closest('.clear').prev();
                try {
                    $.messager.progress({ text: '正在上传...' });
                    $.ajaxFileUpload(
                     {
                         url: '/serv/api/common/file.ashx?action=Add&dir=file',
                         secureuri: false,
                         fileElementId: fid,
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                                 var appendhtml = new StringBuilder();
                                 appendhtml.AppendFormat('<div class="divFileParent"><div class="divFileName" data-id="0"><a href="{0}" target="_blank">{1}</a></div>', resp.file_url_list[0], fname);
                                 appendhtml.AppendFormat('<div class="deleteFile"><img src="/img/delete.png" alt="删除" /></div></div>');
                                 $(divClass).append(appendhtml.ToString());
                             }
                             else {
                                 $.messager.alert('温馨提示', resp.errmsg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    $.messager.alert('温馨提示', e);
                }
            });
            //标签操作按钮
            $document.on('click', '#btnSelectTags', function () {
                loadSelectTagsData();
            });

            $document.on('click', '.warpTagSelect .warpOpeate .btnSave', function () {

                //构造标签新数组
                currTags = [];
                var chekList = $('.warpTagSelect .tagChk');
                for (var i = 0; i < chekList.length; i++) {
                    if ($(chekList[i]).attr('checked')) {
                        currTags.push($(chekList[i]).val());
                    }
                }

                //显示标签
                tagClear();
                addTagList(currTags);

                layer.closeAll();

            });

            $document.on('click', '.warpTagSelect .warpOpeate .btnCancel', function () {

                layer.closeAll();

            });

            $document.on('click', '.warpTagSelect .btnTagSelect ', function () {
                var op = $(this).attr('data-op');

                if (op == 'all')
                    selectTagAll();

                if (op == 'reverse')
                    selectTagReverse();

            });


            $('.delrelation').live('click', function () {//删除关联文章
                var obj = $(this).closest("div");
                $(obj).remove();

            });

            //预览按钮
            $document.on('click', '#btnIosPreview', function () {
                loadPagePreview();
            });
        }

        //弹出预览框
        var $previewContent = $document.find('#dlgIosSkin').find('.preview-content');
        function loadPagePreview() {
            $previewContent.find('.item').html(editor.html());
            $previewContent.mCustomScrollbar({ theme: "minimal-dark" });
            var previewDiv = layer.open({
                type: 1,
                title: false,
                scrollbar: false,
                closeBtn: 0,
                shadeClose: true,
                skin: 'layer-ext-iosskin',
                content: $('#dlgIosSkin')
            });
        }

        var $ulTagList = $warpTagSelect.find('.ulTagList'), $warpNoTagData = $warpTagSelect.find('.warpNoData'), $warpTagDataList = $warpTagSelect.find('.warpTagDataList');
        function loadSelectTagsData() {
            $.ajax({
                type: 'POST',
                url: '/Handler/App/CationHandler.ashx',
                data: { Action: "QueryMemberTag", TagType: 'all', page: 1, rows: 100000000 },
                success: function (resp) {
                    var data = $.parseJSON(resp);
                    if (data.total == 0) {
                        $warpNoTagData.show();
                        $warpTagDataList.hide();
                    } else {
                        $warpTagDataList.show();
                        $warpNoTagData.hide();

                        //构造数据
                        var strHtml = new StringBuilder();
                        for (var i = 0; i < data.rows.length; i++) {
                            strHtml.Append('<li class="overflow_ellipsis"><label>');
                            strHtml.AppendFormat('<input type="checkbox" name="tag" class="tagChk" value="{0}" {1} />{0}', data.rows[i].TagName, currTags.Contains(data.rows[i].TagName) ? 'checked' : '');
                            strHtml.Append('</label></li>');
                        }

                        $ulTagList.html(strHtml.ToString());


                    }
                    var tagDiv = layer.open({
                        type: 1,
                        shade: [0.2, '#000'],
                        shadeClose: true,
                        area: ['300', '320'],
                        title: ['选择标签', 'background:#1B9AF7; color:#fff;'],
                        border: [0],
                        content: $('.warpTagDiv')
                    });


                }
            });
        }

        function addTagList(list) {
            for (var i = 0; i < list.length; i++) {
                if (!$txtTags.tagExist(list[i]))
                    $txtTags.addTag(list[i]);
            }
        }

        function tagClear() {
            $txtTags.importTags('');
        }

        //标签全选
        function selectTagAll() {
            $('.warpTagSelect .tagChk').attr('checked', true);
        }

        //标签反选
        function selectTagReverse() {
            $('.warpTagSelect .tagChk').each(function () {
                var $this = $(this),
                    v = $this.attr('checked');
                $this.attr('checked', !v);
            });
        }

        function saveData() {
            var $btnSave = $('#btnSave'), $btnReset = $('#btnReset');
            if ($btnSave.hasClass('disabled ')) {
                return;
            }
            if ($("#btnSave").attr("disabled") == "disabled") {
                return;
            }
            $("#btnSave").attr({ "disabled": "disabled" });
            //保存前清空选中
            zcWeixinKindeditor.clearEditorSelect();

            $btnSave.addClass('disabled').text('正在处理...');
            $btnReset.addClass('disabled');

            var HaveComment = $('input[type="radio"][name="rdoHaveComment"]:checked').val();



            var model = {
                IsSignUpJubit: 0,
                SignUpActivityID: 0,
                ActivityName: $.trim($('#txtActivityName').val()),
                ActivityWebsite: $.trim($('#txtActivityWebsite').val()),
                ActivityDescription: editor.html(),
                ThumbnailsPath: $('#imgThumbnailsPath').attr('src'),
                Action: currAction == 'add' ? 'AddJuActivity' : 'EditJuActivity',
                JuActivityID: currAcvityID,
                IsHide: rdoIsHide.checked ? 1 : 0,
                alluser: 1,
                IsByWebsiteContent: 0, //rdoWriteIsOnline.checked ? 0 : 1,
                ArticleType: 'article',
                ArticleTypeEx1: 'hf_article',
                ArticleTemplate: $("#ddlarticletemplate").val(),
                IsSpread: 1,
                RecommendCate: GetCheckGroupVal('RecommendCate', 'v'),
                IsHideRecommend: rdHideRecommend.checked ? 1 : 0,
                CategoryId: $("#ddlcategory").val() == 0 ? cateRootId : $("#ddlcategory").val(),
                Summary: $("#txtSummary").val(),
                RedirectUrl: $("#txtRedirectUrl").val(),
                ActivityIntegral: "0",
                MaxSignUpTotalCount: 0,
                Tags: currTags.join(','),
                PV: $("#txtPv").val(),
                ProvinceCode: $('#selectProvince').val(),
                CityCode: $('#selectCity').val(),
                DistrictCode: $('#selectArea').val(),
                K1: $('#txtK1').val(),
                K2: $('#txtK2').val(),
                K3: $('#txtK3').val(),
                K4: $('#txtK4').val(),
                K5: $('#txtK5').val(),
                K6: $('#txtK6').val(),
                K7: $('#txtK7').val(),
                K8: $('#txtK8').val(),
                K9: $('#txtK9').val(),
                K10: $('#txtK10').val(),
                AccessLevel: $(txtAccessLevel).val(),
                RootCateId: cateRootId,
                HaveComment: HaveComment,
                IsFee: 0,
                RelationArticle: GetRelationArticleIds()
               

            };

            var fileList = [];
            var divFile1 = $('.divFileClass1').children();
            if (divFile1.length > 0) {
                for (var i = 0; i < divFile1.length; i++) {
                    var dobj = $(divFile1[i]).find('.divFileName');
                    var fid = $(dobj).attr('data-id');
                    var fpath = $(dobj).find('a').attr('href');
                    var fname = $(dobj).find('a').text();
                    fileList.push({ id: fid, file_class: 1, file_name: fname, file_path: fpath });
                }
            }
            model.file_list = JSON.stringify(fileList);

            setTimeout(function () {


                if (model.ActivityName == '') {
                    $('#txtActivityName').focus();
                    alert('标题不能为空', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    $("#btnSave").removeAttr("disabled");
                    return;
                }
                if (model.ActivityName.length > 150) {
                    $('#txtActivityName').focus();
                    alert('标题字数过长', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    $("#btnSave").removeAttr("disabled");
                    return;
                }
                if (model.RedirectUrl != '') {
                    if (!IsURL(model.RedirectUrl)) {
                        $('#txtRedirectUrl').focus();
                        alert('跳转URL格式错误!', 3);
                        $btnSave.removeClass('disabled').text('保存');
                        $btnReset.removeClass('disabled');
                        $("#btnSave").removeAttr("disabled");
                        return;
                    }
                }

                //alert(model.ThumbnailsPath);

                //return;

                //$.messager.progress({
                //    text: '正在处理...'
                //});
                //layer.load('正在处理...');
                //var loadi = layer.load(5, 0);
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $btnReset.removeClass('disabled');
                        $btnSave.removeClass('disabled').text('保存');
                        $("#btnSave").removeAttr("disabled");
                        //layer.closeLoad();
                        //$.messager.progress('close');
                        if (resp.Status == 1) {
                            if (currAction == 'add')
                                ResetCurr();
                            alert(resp.Msg);
                        } else {
                            alert(resp.Msg);
                        }
                    }
                });



            }, 400);

        }



        function ShowEdit() {
            $('#txtActivityName').val("<%=model.ActivityName %>");
            $('#txtActivityAddress').val("<%=model.ActivityAddress %>");
            $("#ddlcategory").val("<%=model.CategoryId %>");
            $("#txtSummary").val("<%=model.Summary %>");
            $("#txtRedirectUrl").val("<%=model.RedirectUrl%>");
            $('#imgThumbnailsPath').attr('src', "<%=model.ThumbnailsPath %>");
            $('#ddlarticletemplate').val("<%=model.ArticleTemplate %>");
           
            if ('<%= model.IsHide %>' == '1') {
                rdoIsHide.checked = true;
            } else {
                rdoIsNotHide.checked = true;
            }

            if ("<%=model.IsHideRecommend %>" == "1") {
                rdHideRecommend.checked = true;
            } else {
                rdShowRecommend.checked = true;
            }

            $.ajax({
                type: 'post',
                url: handlerUrl,
                dataType: "json",
                data: {
                    Action: 'GetSingelJuActivity',
                    JuActivityID: currAcvityID
                },
                success: function (resp) {
                    if (resp.Status == 1) {

                        //TODO:展示编辑内容
                        var model = resp.ExObj;
                        setTimeout(function () {

                            editor.html(model.ActivityDescription);

                            if (model.ProvinceCode) {
                                setProvinceSelect(model.ProvinceCode);
                                setCitySelect(model.ProvinceCode, model.CityCode);
                            }
                            if (model.CityCode && model.ProvinceCode) {
                                setAreaSelect(model.CityCode, model.DistrictCode);
                            }

                            $('#txtK1').val(model.K1);
                            $('#txtK2').val(model.K2);
                            $('#txtK3').val(model.K3);
                            $('#txtK4').val(model.K4);
                            $('#txtK5').val(model.K5);
                            $('#txtK6').val(model.K6);
                            $('#txtK7').val(model.K7);
                            $('#txtK8').val(model.K8);
                            $('#txtK9').val(model.K9);
                            $('#txtK10').val(model.K10);

                        }, 1000);
                    } else {
                        alert(resp.Msg);
                    }
                }
            });

        }

        function ResetCurr() {
            ClearAll();
            editor.html('');
            titles=[];
            $('.tab').each(function (k, v) {
                var index = $(v).attr('data-index');
                var item = $(this);
                if (index > 0) {
                    item.remove();
                }
            });
          
        }

        //获取随机海报
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
        }

        function ClearEditer() {
            editor.html('');
        }
        function importWord() {
            editor.clickToolbar('importword');
        }
        function importMpArticle() {

            $("#dlgImportMpArticle").dialog('open');
            $("#dlgImportMpArticle").window("move", { top: $(document).scrollTop() + ($(window).height() - 400) * 0.5 });


        }
        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                afterFocus: function () {
                    GetTabContent();
                },
                items: [
                    'source', '|', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'baidumap', '|', 'table', 'quickformat', 'cleardoc'
                ],                
                extraFileUploadParams: { userID: '<%= new ZentCloud.BLLJIMP.BLL().GetCurrUserID() %>' },               
                filterMode: false,
                width: "80%",
                height: "600px",
                cssPath: ['/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.css', '/Weixin/ArticleTemplate/css/comm.css', '/Weixin/ArticleTemplate/css/weixin.css']//'http://static-files.socialcrmyun.com/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.css'
            });
            zcWeixinKindeditor = $("#zcWeixinKindeditor").zcWeixinKindeditor({ keditor: editor, def_cate: '1216' });
        });
        //KindEditor.ready(function (K) { K.create('#txtEditor', { afterFocus: function () { alert('Focus!!!'); } }); });

        function GetTabContent() {
            setInterval(function () {
                var title = $('.dcolor').text();
                var index = $('.dcolor ').attr('data-index');
                if (index == '1') {
                    desc.TabExTitle1 = title;
                    desc.TabExContent1 = editor.html();
                } else if (index == '2') {
                    desc.TabExTitle2 = title;
                    desc.TabExContent2 = editor.html();
                } else if (index == '3') {
                    desc.TabExTitle3 = title;
                    desc.TabExContent3 = editor.html();
                } else if (index == '4') {
                    desc.TabExTitle4 = title;
                    desc.TabExContent4 = editor.html();
                } else {
                    desc.dContent = editor.html();
                }
            }, 1000);
        }



        function ShowArticleDlg() {
            //文章选择框
            $("#dlgArticle").dialog('open');
            $("#dlgArticle").window("move", { top: $(document).scrollTop() + ($(window).height() - 400) * 0.5 });
        }


        function GetRelationArticleIds() {//获取设置的关联文章
            ids = [];
            $("[data-relationarticleid]").each(function () {

                ids.push($(this).data("relationarticleid"));

            })
            return ids.join(",");
        }
        function clearRead() {
            $("#divRelationArticle").html("");
            var RegUrl = new RegExp();
            RegUrl.compile("^[A-Za-z]+://[A-Za-z0-9-_]+\\.[A-Za-z0-9-_%&\?\/.=]+$");//jihua.cnblogs.com 
            if (!RegUrl.test(str)) {
                return false;
            }
            return true;
        }
        function clearRead() {
            $("#divRelationArticle").html("");
        }

    </script>
</asp:Content>
