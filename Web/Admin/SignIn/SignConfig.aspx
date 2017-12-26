<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SignConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SignIn.SignConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        input {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        .delButton {
            color: #fff;
            border-color: #ef473a;
            background-color: #ef473a;
            height: 20px;
            border-radius: 0;
            padding: 0;
            line-height: 20px;
            text-align: center;
            cursor: pointer;
        }

        .Height30 {
            height: 30px;
            width: 22%;
        }

        .sp-replacer {
            width: 20% !important;
        }

        .img {
            width: 140px;
            height: 100px;
            border: 1px solid #ddd;
        }

        .floatL {
            float: left;
        }

        .imgDiv {
            margin-left: 10px;
        }

        .mLeft {
            margin-left: 20px;
        }

        .img1 {
            width: 100px;
            height: 80px;
            border: 1px solid #e5e5e5;
        }

        .textHiehgt {
            height: 25px;
            width: 270px;
        }

        .delBtn {
            border: 1px solid #ef473a;
            color: #fff;
            padding: 5px 46px;
            margin-left: 30px;
            border-color: #28a54c;
            background-color: #33cd5f;
        }

        .delBtn1 {
            border: 1px solid #ef473a;
            color: #fff;
            padding: 5px 48px;
            border-color: #28a54c;
            background-color: #33cd5f;
        }
        .delBtn2{
            margin-left:6px;
        }
        .delBtn:hover {
            color: #fff;
        }

        .delBtn1:hover {
            color: #fff;
        }

        .img2 {
            padding: 9px;
        }

        .jiaStyle {
            border: 1px solid #ddd;
            float: left;
            margin-top: 4px;
            cursor: pointer;
	    display:none;
        }

        .solid-warp {
            background: #F8F8F8;
            border: 1px solid #e5e5e5;
            border-radius: 5px;
            padding: 10px 0px 10px 10px;
        }

        .help {
            margin: 0 6px;
        }
        .colorGray{
            color:#ccc;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>签到配置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">



        <table width="100%" id="tbMain">
            <tr>
                <td style="width: 200px;" align="right" valign="middle">手机端链接：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <div style="padding: 20px;">
                        http://<%=Request.Url.Authority %>/App/Cation/Wap/UserSignIn/UserSignIn.aspx
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">签到名称：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtAddress" value="<%=model!=null&&!string.IsNullOrEmpty(model.Address)?model.Address:""%>" style="width: 100%;" />
                </td>
            </tr>

            <tr>
                <td style="width: 200px;" align="right" valign="middle">签到按钮颜色：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" class="color" id="txtButtonColor" value="<%=model!=null&&!string.IsNullOrEmpty(model.ButtonColor)?model.ButtonColor:"#43AEE8"%>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">选择抽奖：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" readonly="readonly" id="txtLotteryId" class="Height30" value="<%=model.LotteryId %>" />
                    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="selectLottery()">选择抽奖</a>                    <a href="javascript:;" style="color: blue;" class="clearLottery">清除</a>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">幻灯片组名：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" readonly="readonly" id="txtSlideGroupName" value="<%=model.SlideGroupName %>" class="Height30" />
                    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="selectSlide()">选择幻灯片</a>                    <a href="javascript:;" class="clearSlide" style="color: blue;">清除</a>                </td>
            </tr>




            <tr>
                <td style="width: 200px;" align="right" valign="middle">签到背景图片：
                </td>
                <td style="width: *;" align="left" valign="middle">

                    <div class="floatL">

                        <img class="img" src="<%=model!=null&&!string.IsNullOrEmpty(model.BackGroundImage)?model.BackGroundImage:"" %>" onclick="txtThumbnail1.click();" id="imgThumbnail1" alt="签到背景图片" />
                        
                        <div class="delButton">清除图片</div><span class="colorGray">建议像素：375*250</span>
                        <input type="file" id="txtThumbnail1" name="file1" style="display: none;" />

                    </div>

                    <div class="floatL imgDiv">
                        <div class="floatL mTop50">已签到显示图片：</div>
                        <div class="floatL">
                            <img class="img" src="<%=model!=null&&!string.IsNullOrEmpty(model.HaveSignImage)?model.HaveSignImage:"" %>" onclick="txtThumbnail2.click();" id="imgThumbnail2" alt="奖品缩略图" />
                            <div class="delButton">清除图片</div><span class="colorGray">建议像素：64*64</span>
                            <input type="file" id="txtThumbnail2" name="file1" style="display: none;" />
                        </div>

                    </div>

                    <div class="floatL imgDiv">
                        <div class="floatL mTop50">
                            未签到显示图片：
                        </div>
                        <div class="floatL">
                            <img class="img" src="<%=model!=null&&!string.IsNullOrEmpty(model.NoHaveSignImage)?model.NoHaveSignImage:"" %>" onclick="txtThumbnail3.click();" id="imgThumbnail3" alt="奖品缩略图" />
                            <div class="delButton">清除图片</div><span class="colorGray">建议像素：64*64</span>
                            <input type="file" id="txtThumbnail3" name="file1" style="display: none;" />
                        </div>

                    </div>

                    <div class="floatL imgDiv">
                        <div class="floatL mTop50">
                            奖品缩略图：	
                        </div>
                        <div class="floatL">
                            <img class="img" onclick="txtThumbnail.click()" src="<%=model!=null&&!string.IsNullOrEmpty(model.Thumbnail)?model.Thumbnail:"" %>" id="imgThumbnail" alt="奖品缩略图" />
                            <div class="delButton">清除图片</div><span class="colorGray">建议像素：70*55</span>
                            <input type="file" id="txtThumbnail" name="file1" style="display: none;" />
                        </div>
                    </div>
                </td>
            </tr>

            <tr>
                <td style="width: 200px;" align="right" valign="middle">周一广告图：
                </td>
                <td class="solid-warp" align="left" valign="middle">
                    <div class="monday-warp">

                        <%
                            if (adsList1.Count == 0)
                            {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                            }
                            else
                            {
                                foreach (var item in adsList1)
                                {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="<%=item.img %>" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" value="<%=item.title %>" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input value="<%=item.url %>" class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                                }
                            }     
                        %>
                    </div>
                    <div class="jiaStyle" data-week="monday-warp">
                        <img class="img2" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20170510/32345BFBEDEE44F59A33B9FA67E8667D.png" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周二广告图：
                </td>
                <td class="solid-warp" align="left" valign="middle">
                    <div class="tuesday-warp">
                        <%
                            if (adsList2.Count == 0)
                            {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" class="textHiehgt title mTop10" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                            }
                            else
                            {
                                foreach (var item in adsList2)
                                {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="<%=item.img %>" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" name="file1" class="txtImgweek1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" value="<%=item.title %>" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input value="<%=item.url %>" class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                                }
                            }     
                        %>
                    </div>
                    <div class="jiaStyle" data-week="tuesday-warp">
                        <img class="img2" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20170510/32345BFBEDEE44F59A33B9FA67E8667D.png" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周三广告图：
                </td>
                <td class="solid-warp" align="left" valign="middle">
                    <div class="wednesday-warp">

                        <%
                            if (adsList3.Count == 0)
                            {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" name="file1" class="txtImgweek1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                            }
                            else
                            {
                                foreach (var item in adsList3)
                                {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="<%=item.img %>" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" value="<%=item.title %>" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input value="<%=item.url %>" class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                                }
                            }  
                        %>
                    </div>

                    <div class="jiaStyle" data-week="wednesday-warp">
                        <img class="img2" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20170510/32345BFBEDEE44F59A33B9FA67E8667D.png" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周四广告图：
                </td>
                <td class="solid-warp" align="left" valign="middle">

                    <div class="thursday-warp">
                        <%
                            if (adsList4.Count == 0)
                            {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                            }
                            else
                            {
                                foreach (var item in adsList4)
                                {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="<%=item.img %>" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" value="<%=item.title %>" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input value="<%=item.url %>" class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                                }
                            }  
                        %>
                    </div>

                    <div class="jiaStyle" data-week="thursday-warp">
                        <img class="img2" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20170510/32345BFBEDEE44F59A33B9FA67E8667D.png" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周五广告图：
                </td>
                <td class="solid-warp" align="left" valign="middle">

                    <div class="friday-warp">
                        <%
                            if (adsList5.Count == 0)
                            {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                            }
                            else
                            {
                                foreach (var item in adsList5)
                                {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="<%=item.img %>" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" value="<%=item.title %>" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" value="<%=item.url %>" type="text" />
                            </div>
                        </div>
                        <%
                                }
                            }
                        %>
                    </div>
                    <div class="jiaStyle" data-week="friday-warp">
                        <img class="img2" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20170510/32345BFBEDEE44F59A33B9FA67E8667D.png" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周六广告图：
                </td>
                <td class="solid-warp" align="left" valign="middle">
                    <div class="saturday-warp">
                        <%
                            if (adsList6.Count == 0)
                            {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                            }
                            else
                            {
                                foreach (var item in adsList6)
                                {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="<%=item.img %>" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" value="<%=item.title %>" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" value="<%=item.url %>" type="text" />
                            </div>
                        </div>
                        <%
                                }
                            }     
                        %>
                    </div>
                    <div class="jiaStyle" data-week="saturday-warp">
                        <img class="img2" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20170510/32345BFBEDEE44F59A33B9FA67E8667D.png" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周日广告图：
                </td>
                <td class="solid-warp" align="left" valign="middle">
                    <div class="sunday-warp">
                        <%
                            if (adsList7.Count == 0)
                            {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" type="text" />
                            </div>
                        </div>
                        <%
                            }
                            else
                            {
                                foreach (var item in adsList7)
                                {
                        %>
                        <div class="floatL mTop10 each">
                            <div class="floatL">
                                <img class="img1" src="<%=item.img %>" alt="点击上传" />
                                <div class="colorGray">建议像素：260*240</div>
                                <input type="file" class="txtImgweek1" name="file1" style="display: none;" />
                            </div>
                            <div class="floatL">
                                <a href="javascript:;" class="delBtn">清除图片</a>
                                <a href="javascript:;" class="delBtn1">删除</a>
                                <br />
                                <span>标题</span>
                                <input type="text" value="<%=item.title %>" class="textHiehgt mTop10 title" />
                                <br />
                                <span>链接</span>
                                <input class="textHiehgt mTop3 url" value="<%=item.url %>" type="text" />
                            </div>
                        </div>
                        <%
                                }
                            }
                        %>
                    </div>

                    <div class="jiaStyle" data-week="sunday-warp">
                        <img class="img2" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20170510/32345BFBEDEE44F59A33B9FA67E8667D.png" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周一签到得积分：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtMondayScore" value="<%=model!=null&&model.MondayScore>0?model.MondayScore:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周二签到得积分：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtTuesdayScore" value="<%=model!=null&&model.TuesdayScore>0?model.TuesdayScore:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周三签到得积分：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtWednesdayScore" value="<%=model!=null&&model.WednesdayScore>0?model.WednesdayScore:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周四签到得积分：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtThursdayScore" value="<%=model!=null&&model.ThursdayScore>0?model.ThursdayScore:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周五签到得积分：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtFridayScore" value="<%=model!=null&&model.FridayScore>0?model.FridayScore:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周六签到得积分：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtSaturdayScore" value="<%=model!=null&&model.SaturdayScore>0?model.SaturdayScore:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">周日签到得积分：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtSundayScore" value="<%=model!=null&&model.SundayScore>0?model.SundayScore:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">第一次补签：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtRetroactiveToOne" value="<%=model!=null&&model.RetroactiveToOne>0?model.RetroactiveToOne:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">第二次补签：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtRetroactiveToTwo" value="<%=model!=null&&model.RetroactiveToTwo>0?model.RetroactiveToTwo:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">第三次补签：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtRetroactiveToThree" value="<%=model!=null&&model.RetroactiveToThree>0?model.RetroactiveToThree:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">第四次补签：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtRetroactiveToFour" value="<%=model!=null&&model.RetroactiveToFour>0?model.RetroactiveToFour:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">第五次补签：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtRetroactiveToFive" value="<%=model!=null&&model.RetroactiveToFive>0?model.RetroactiveToFive:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">第六次补签：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtRetroactiveToSix" value="<%=model!=null&&model.RetroactiveToSix>0?model.RetroactiveToSix:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">第七次补签：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="number" id="txtRetroactiveToSeven" value="<%=model!=null&&model.RetroactiveToSeven>0?model.RetroactiveToSeven:0%>" style="width: 100%;" />
                </td>
            </tr>
            <tr class="">
                <td style="width: 200px;" align="right" valign="middle">签到说明：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <div id="txtEditor">
                        <%=model!=null?model.Description:"" %>
                    </div>
                </td>
            </tr>

        </table>
    </div>
    <br />
    <br />
    <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 60px; line-height: 60px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 16px; left: 0;">
        <a href="javascript:;" style="width: 200px; font-weight: bold; text-decoration: none;"
            id="btnSave" class="button button-rounded button-primary">保存</a>
    </div>






    <div id="dlgLottery" class="easyui-dialog" closed="true" modal="true"
        style="width: 550px; height: 400px; padding: 10px;">
        <br />
        <div>
            <select id="lotteryType">
                <option value="all">全部</option>
                <option value="shake">摇一摇</option>
                <option value="scratch">刮刮奖</option>
            </select>
            &nbsp;<input type="text" id="txtName" placeholder="标题" style="width: 300px; height: 18px;">
            <a class="easyui-linkbutton" iconcls="icon-search" onclick="Search()">搜索</a>
        </div>
        <br />
        <table id="grvLotteryInfo" fitcolumns="true"></table>
    </div>


    <div id="dlgSlide" class="easyui-dialog" closed="true" modal="true"
        style="width: 450px; height: 400px; padding: 10px;">
        <br />
        <div>
            关键字：
            <input type="text" id="txtSlideName" placeholder="标题" style="width: 190px; height: 25px;">
            <a class="easyui-linkbutton" iconcls="icon-search" onclick="SearchSlide()">搜索</a>
            <a href="javascript:;" style="color: blue;" id="gotoSlide">新增幻灯片</a>
        </div>
        <br />
        <table id="slidegrvData" fitcolumns="true"></table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js"></script>
    <script type="text/javascript">
        var editor;//编辑器
        var id = '<%=model!=null?model.AutoID:0%>';
        var lotteryType = '';
        $(function () {


            ////加载抽奖列表
            $("#grvLotteryInfo").datagrid(
	            {
	                method: "Post",
	                url: '/Handler/App/CationHandler.ashx',
	                queryParams: { Action: "QueryWXLotteryV1", LotteryName: '', LotteryType: 'all' },
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: true,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                 { field: 'LotteryID', title: '抽奖编号', width: 50, align: 'left' },
                                {
                                    field: 'ThumbnailsPath', title: '缩略图', width: 80, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                        return str.ToString();
                                    }
                                },
                                { field: 'LotteryName', title: '标题', width: 100, align: 'left' },
                                {
                                    field: 'Status', title: '状态', width: 60, align: 'left', formatter: function (value) {
                                        var str = new StringBuilder();
                                        if (value == 1) {
                                            str.AppendFormat("<span style='color:green'>进行中</span>");
                                        } else {
                                            str.AppendFormat("<span style='color:red'>已停止</span>");
                                        }
                                        return str.ToString();
                                    }
                                }
	                ]]
	            }
            );
            //加载幻灯片列表
            $("#slidegrvData").datagrid(
	            {
	                method: "Post",
	                url: '/serv/api/admin/mall/slide/listall.ashx',
	                height: 400,
	                pagination: true,
	                striped: true,
	                loadFilter: pagerFilter,
	                pageSize: 50,
	                singleSelect: true,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'slide_type', title: '幻灯片组名', width: 100, align: 'left' }

	                ]]
	            }
            );
            $('#dlgSlide').dialog({
                modal: false,
                buttons: [{
                    text: '确定',
                    handler: function () {

                        var rows = $('#slidegrvData').datagrid('getSelections');
                        if (rows.length == 0) {
                            return false;
                        }
                        $('#txtSlideGroupName').val(rows[0].slide_type);
                        $('#dlgSlide').dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgSlide').dialog('close');
                    }
                }]
            });

            $('#dlgLottery').dialog({
                modal: false,
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvLotteryInfo').datagrid('getSelections');
                        if (rows.length == 0) {
                            return false;
                        }
                        $('#txtLotteryId').val(rows[0].LotteryID);
                        lotteryType = rows[0].LotteryType;
                        $('#dlgLottery').dialog('close');
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgLottery').dialog('close');
                    }
                }]
            });


            $('#btnSave').click(function () {


                var model = GetData();
                if (model.address == '') {
                    alert('请输入签到名称');
                    return;
                }
                if ($.trim(model.lottery_type) == '') {
                    model.lottery_type = 'scratch';
                }
                var url = '';
                if (model.id == 0) {
                    url = '/serv/api/admin/signin/address/add.ashx';
                } else {
                    url = '/serv/api/admin/signin/address/update.ashx';
                }
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: model,
                    dataType: 'json',
                    success: function (result) {
                        if (result.status) {
                            layerAlert('操作完成');
                            if (id == 0) {
                                window.location.reload();
                            }
                        } else {
                            layerAlert('操作出错');
                        }
                    }
                });
            });


            $(document).on('click', '.delButton', function () {
                var img = $(this).siblings(".img").attr("src", "");
            });

            //上传缩略图
            $(document).on('change', '#txtThumbnail', function () {
                var layerIndex = layer.load(0, { shade: false });
                $.ajaxFileUpload({
                    url: '/serv/api/common/file.ashx?action=Add',
                    secureuri: false,
                    fileElementId: 'txtThumbnail',
                    dataType: 'json',
                    success: function (result) {
                        layer.close(layerIndex);
                        if (result.errcode == 0) {
                            $('#imgThumbnail').attr('src', result.file_url_list[0]);
                        }
                        else {
                            alert(result.errmsg);
                        }
                    }
                });
            });
            //上传背景图片
            $(document).on('change', '#txtThumbnail1', function () {
                var layerIndex = layer.load(0, { shade: false });
                $.ajaxFileUpload({
                    url: '/serv/api/common/file.ashx?action=Add',
                    secureuri: false,
                    fileElementId: 'txtThumbnail1',
                    dataType: 'json',
                    success: function (result) {
                        layer.close(layerIndex);
                        if (result.errcode == 0) {
                            $('#imgThumbnail1').attr('src', result.file_url_list[0]);
                        }
                        else {
                            alert(result.errmsg);
                        }
                    }
                });
            });
            //上传已签到图片
            $(document).on('change', '#txtThumbnail2', function () {
                var layerIndex = layer.load(0, { shade: false });
                $.ajaxFileUpload({
                    url: '/serv/api/common/file.ashx?action=Add',
                    secureuri: false,
                    fileElementId: 'txtThumbnail2',
                    dataType: 'json',
                    success: function (result) {
                        layer.close(layerIndex);
                        if (result.errcode == 0) {
                            $('#imgThumbnail2').attr('src', result.file_url_list[0]);
                        }
                        else {
                            alert(result.errmsg);
                        }
                    }
                });
            });
            //上传未签到图片
            $(document).on('change', '#txtThumbnail3', function () {
                var layerIndex = layer.load(0, { shade: false });
                $.ajaxFileUpload({
                    url: '/serv/api/common/file.ashx?action=Add',
                    secureuri: false,
                    fileElementId: 'txtThumbnail3',
                    dataType: 'json',
                    success: function (result) {
                        layer.close(layerIndex);
                        if (result.errcode == 0) {
                            $('#imgThumbnail3').attr('src', result.file_url_list[0]);
                        }
                        else {
                            alert(result.errmsg);
                        }
                    }
                });
            });
            $(document).on('click', '#gotoSlide', function () {
                top.addTab('幻灯片管理', '/customize/mmpadmin/index.aspx?hidemenu=1#/index/newAdList');
            });

            $(document).on('click', '.clearLottery', function () {
                $('#txtLotteryId').val('');
            });

            $(document).on('click', '.clearSlide', function () {
                $('#txtSlideGroupName').val('');
            });

            /*
                广告区域 周一至周日
            */
            $(document).on('click', '.jiaStyle', function () {
                layerAlert('暂未开放');
                return;

                var week = $(this).attr('data-week');

                var html = "";
                html += '<div class="floatL mTop10 each">';
                html += '<div class="floatL">';
                html += '<img class="img1" src="" alt="点击上传" />';
                html += '<div class="colorGray">建议像素：260*240</div>';
                html += '<input type="file" class="txtImgweek1"  name="file1" style="display: none;" />';
                html += '</div>';
                html += '<div class="floatL">';
                html += '<a href="javascript:;" class="delBtn">清除图片</a>';
                html += '<a href="javascript:;" class="delBtn1 delBtn2">删除</a>';
                html += '<br /><span>标题</span>';
                html += '<input type="text"  class="textHiehgt help mTop10 title"/>';
                html += '<br /><span>链接</span>';
                html += '<input class="textHiehgt help mTop3 url" type="text" />';
                html += '</div>';
                html += '</div>';
                $('.' + week + '').append(html);
            });
            //增加广告
            $(document).on('click', '.img1', function () {
                $(this).siblings('.txtImgweek1').click();
            });
            //清除图片广告图
            $(document).on('click', '.delBtn', function () {
                $(this).parent('.floatL').prev('.floatL').find('img').attr('src', '');
            });
            //删除整块
            $(document).on('click', '.delBtn1', function () {
                $(this).parent().parent().remove();
            });
            //上传广告
            $(document).on('change', '.txtImgweek1', function () {
                var layerIndex = layer.load(0, { shade: false });
                var imgObj = $(this).siblings('.img1');
                $.ajaxFileUpload({
                    url: '/serv/api/common/file.ashx?action=Add',
                    secureuri: false,
                    fileElement: this,
                    dataType: 'json',
                    success: function (result) {
                        layer.close(layerIndex);
                        if (result.errcode == 0) {
                            $(imgObj).attr('src', result.file_url_list[0]);
                        }
                        else {
                            alert(result.errmsg);
                        }
                    }
                });
            });

        });

        //获取周一广告信息

        function GetAds(selector) {
            var mData = [];
            $('.' + selector + ' .each').each(function (k, v) {
                console.log('v', v);
                var img = $(this).find(".img1").attr('src');
                var title = $(this).find('.title').val();
                var url = $(this).find('.url').val();
                var model = {
                    img: img,
                    title: title,
                    url: url
                };
                if (!model.img) {
                    return false;
                }
                mData.push(model);
            });
            if (mData.length == 0) return "";
            console.log(JSON.stringify(mData));
            return JSON.stringify(mData);
        }














        function pagerFilter(result) {
            if (result == null) {
                return {
                    total: 0,
                    rows: []
                };
            }
            return {
                total: result.totalcount,
                rows: result.list
            };
        }

        function GetData() {
            var model = {
                address: $('#txtAddress').val(),
                desc: editor.html(),
                lottery_id: $.trim($('#txtLotteryId').val()),
                lottery_type: lotteryType,
                id: id,
                slide_name: $('#txtSlideGroupName').val(),
                monday_score: $('#txtMondayScore').val(),
                tuesday_score: $('#txtTuesdayScore').val(),
                wednesday_score: $('#txtWednesdayScore').val(),
                thursday_score: $('#txtThursdayScore').val(),
                friday_score: $('#txtFridayScore').val(),
                saturday_score: $('#txtSaturdayScore').val(),
                sunday_score: $('#txtSundayScore').val(),
                button_color: $('#txtButtonColor').val(),
                thumbnail: $('#imgThumbnail').attr('src'),
                background_img: $('#imgThumbnail1').attr('src'),
                have_sign_image: $('#imgThumbnail2').attr('src'),
                no_have_sign_image: $('#imgThumbnail3').attr('src'),
                retroactive_one: $('#txtRetroactiveToOne').val(),
                retroactive_two: $('#txtRetroactiveToTwo').val(),
                retroactive_three: $('#txtRetroactiveToThree').val(),
                retroactive_four: $('#txtRetroactiveToFour').val(),
                retroactive_five: $('#txtRetroactiveToFive').val(),
                retroactive_six: $('#txtRetroactiveToSix').val(),
                retroactive_seven: $('#txtRetroactiveToSeven').val(),
                type: 'Sign'
            };
            model.monday_ads = GetAds('monday-warp');
            model.tuesday_ads = GetAds('tuesday-warp');
            model.wednesday_ads = GetAds('wednesday-warp');
            model.thursday_ads = GetAds('thursday-warp');
            model.friday_ads = GetAds('friday-warp');
            model.saturday_ads = GetAds('saturday-warp');
            model.sunday_ads = GetAds('sunday-warp');
            return model;

        }

        //选择抽奖
        function selectLottery() {
            $('#dlgLottery').dialog({ title: "刮刮乐和摇摇奖列表", top: $(document).scrollTop() + ($(window).height() - 420) * 0.5 });
            $("#dlgLottery").dialog('open');

        }
        //选择幻灯片
        function selectSlide() {
            $('#dlgSlide').dialog({ title: "幻灯片列表", top: $(document).scrollTop() + ($(window).height() - 420) * 0.5 });
            $("#dlgSlide").dialog('open');
        }
        //搜索抽奖
        function Search() {

            $('#grvLotteryInfo').datagrid(
                   {
                       method: "Post",
                       url: '/Handler/App/CationHandler.ashx',
                       queryParams: { Action: "QueryWXLotteryV1", LotteryType: $('#lotteryType').val(), LotteryName: $("#txtName").val() }
                   });
        }
        //搜索幻灯片
        function SearchSlide() {
            $('#slidegrvData').datagrid(
               {
                   method: "Post",
                   url: '/serv/api/admin/mall/slide/listall.ashx',
                   queryParams: { keyword: $("#txtSlideName").val() }
               });
        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'importword', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'baidumap', '|', 'template', '|', 'table', 'cleardoc'],
                filterMode: false,
                width: "100%",
                height: "400px",
                cssPath: ['/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.css', '/Weixin/ArticleTemplate/css/comm.css'],
            });
        });
    </script>
</asp:Content>
