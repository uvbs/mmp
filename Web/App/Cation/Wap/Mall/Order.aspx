<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/WXMall.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.Mall.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header class="TopToolbar">
		<a href="" class="BtnBack" onclick="javascript:history.go(-1);return false;">返回</a>
		<a href="list.aspx" class="BtnGoHome">&nbsp;</a>
		<span>购物车</span>
	</header>
    <div class="OrderList PagePanel">
        <h3 class="ProListBoxTitle">
            已选购的商品<%--<div class="btnOrderClear">清空购物车</div>--%>
            </h3>
       
        <ul >
            <%--<li class="OrderItem" data-pid="1">
                <mark class="CartMask"></mark>
                <img src="/img/wxmall/p1.jpg" alt="" />
                <div class="ProText">
                    爱乐贝兜儿 2014春款套装 男童春季运动服 儿童春秋款 小孩休闲装新款衣服潮TZ62 钴蓝色 110码建议身高110cm
                </div>
                <div class="OrderPrinceControl">
                    <div class="ProPrince">
                        ￥168.00
                    </div>
                    <div class="ControlBtnSub">
                        -</div>
                    <div class="ControlTxtNum">
                        1</div>
                    <div class="ControlBtnAdd">
                        +</div>
                </div>
            </li>
            <li class="OrderItem" data-pid="2">
                <mark class="CartMask CartMaskCheck"></mark>
                <img src="/img/wxmall/p2.jpg" alt="" />
                <div class="ProText">
                    爱乐贝兜儿 2014春款套装 男童春季运动服 儿童春秋款 小孩休闲装新款衣服潮TZ62 钴蓝色 110码建议身高110cm
                </div>
                <div class="OrderPrinceControl">
                    <div class="ProPrince">
                        ￥168.00
                    </div>
                    <div class="ControlBtnSub">
                        -</div>
                    <div class="ControlTxtNum">
                        1</div>
                    <div class="ControlBtnAdd">
                        +</div>
                </div>
            </li>
            <li class="OrderItem" data-pid="3">
                <mark class="CartMask"></mark>
                <img src="/img/wxmall/p3.jpg" alt="" />
                <div class="ProText">
                    爱乐贝兜儿 2014春款套装 男童春季运动服 儿童春秋款 小孩休闲装新款衣服潮TZ62 钴蓝色 110码建议身高110cm
                </div>
                <div class="OrderPrinceControl">
                    <div class="ProPrince">
                        ￥168.00
                    </div>
                    <div class="ControlBtnSub">
                        -</div>
                    <div class="ControlTxtNum">
                        1</div>
                    <div class="ControlBtnAdd">
                        +</div>
                </div>
            </li>--%>
        </ul>
    </div>
    <div class="OrderLinkerInfo PagePanel">
        <h3 class="ProListBoxTitle">
            收货人信息</h3>
        <div class="Row">
            <div class="RowTitle">
                收货人</div>
            <div class="Cell">
                <input type="text" id="txtLinkerName" placeholder="请输入收货人姓名"></div>
        </div>
        <div class="Row">
            <div class="RowTitle">
                手机号码</div>
            <div class="Cell">
                <input type="text" id="txtLinkerPhone" placeholder="请输入可联系的手机号码"></div>
        </div>
        
        <div class="Row">
            <div class="RowTitle">
                收货地址</div>
            <div class="Cell">
                <textarea name="" id="txtLinkerAddress" placeholder="请输入收货地址"></textarea></div>
        </div>
        <div class="Row">
            <div class="RowTitle">
                订单备注</div>
            <div class="Cell">
                <textarea name="" id="txtOrderMemo" placeholder="请输入订单备注信息"></textarea></div>
        </div>
    </div>
    <footer class="OrderFooter">
	    <div class="OrderMoneyCalc">
		    合计：
		    <span class="TotalPrince">￥0.00</span>
	    </div>
	    <div class="OrderCash" >
		    提交订单(<span class="TotalCount">0</span>)
	    </div>
    </footer>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderScript" runat="server">
 
</asp:Content>
