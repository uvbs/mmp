<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AddressAdd.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SignIn.AddressAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=QfOTnQOrYQgnwWp6TrV2hg5q"></script>
    <style type="text/css">
        body, html {
            width: 100%;
            height: 100%;
            margin: 0;
            font-family: "微软雅黑";
        }

        #baiduMap {
            height: 350px;
            width: 700px;
        }

        #size {
            display: block;
            margin: 0.5rem auto 0 auto;
        }

        /*body, html{width: 100%;height: 100%;margin:0;font-family:"微软雅黑";font-size:14px;}
		#l-map{width:100%;}
		#r-result{width:100%;}*/
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%if (action == "add") {
      %>
     新增签到地点
    <%
      }else{
            %>
     编辑签到地点
    <%
      } %>
     <span class="l-btn-right pointer" style="float: right;margin-right: 30px;"><span class="l-btn-text icon-back" id="go" style="padding-left: 20px;">返回</span></span>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div title="" style="width: 400px; padding: 15px;">

        <div id="baiduMap"></div>

        <div id="l-map"></div>
        <div id="r-result">请输入地址:<input type="text" id="suggestId" size="49"  style="height:20px;"/></div>
        <div id="searchResultPanel" style="border: 1px solid #C0C0C0; width: 150px; height: auto; display: none;"></div>



        <table width="100%" class="mTop10">
            <tr>
                <td>地址:
                </td>
                <td>
                    <input id="txtAddress" type="text" style="width: 200px;" value="<%=model.Address %>" />
                </td>
            </tr>
            <tr class="pTop10">
                <td>经度:
                </td>
                <td>
                    <input id="txtLongitude" readonly="readonly" type="text" style="width: 200px;" value="<%=model.Longitude %>" />

                </td>
            </tr>
            <tr>
                <td>纬度:
                </td>
                <td>
                    <input id="txtLatitude" readonly="readonly" type="text" style="width: 200px;" value="<%=model.Latitude %>" />

                </td>
            </tr>
            <tr>
                <td>范围:
                </td>
                <td>
                    <input id="txtRange" type="text" style="width: 200px;" value="<%=model.Range %>" />米
                </td>
            </tr>
            <tr>
                <td>签到成功跳转链接:
                </td>
                <td>
                    <input id="txtSignInSuccessUrl" type="text" style="width: 200px;" value="<%=model.SignInSuccessUrl %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <div class="warpOpeate">
                        <a href="javascript:;" id="btnSave" class="button button-primary button-rounded button-small btnSave">确定</a>


                        <a href="AddressList.aspx" class="button button-rounded button-small btnCancel">取消</a>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/SignIn/Address/";
        $(function () {

            $("#btnSave").click(function () {
                var dataModel =
                {
                    address: $("#txtAddress").val(),
                    longitude: $("#txtLongitude").val(),
                    latitude: $("#txtLatitude").val(),
                    range:$("#txtRange").val(),
                    id:<%=model.AutoID%>,
                    successurl:$("#txtSignInSuccessUrl").val()
                    };
                if (dataModel.address.trim() == "") {
                    Alert("请输入地址");
                    return;
                }
                if (dataModel.longitude.trim() == "") {
                    Alert("请输入经度");
                    return;
                }
                if (dataModel.latitude.trim() == "") {
                    Alert("请输入纬度");
                    return;
                }


                var submitUrl = "";
                if ("<%=action%>"=="add") {
                    submitUrl =handlerUrl + "Add.ashx";
                }
                else {
                    submitUrl = handlerUrl + "Update.ashx";
                }
                $.ajax({
                    type: 'POST',
                    url:submitUrl,
                    data: dataModel,
                    dataType: 'json',
                    success: function (data) {
                        if (data.status) {
                            location.href = "AddressList.aspx";
                        } else {
                            Alert(data.msg);
                        }
                    }
                });
            });

            $("#go").click(function () {
                history.go(-1);
            });
        });

    </script>

    <script>

	// 百度地图API功能
	    //var map = new BMap.Map("allmap");  
        //map.centerAndZoom(new BMap.Point(116.4035,39.915),8); 
        //setTimeout(function(){
        //    map.setZoom(14);   
        //}, 2000);  //2秒后放大到14级
        //map.enableScrollWheelZoom(true);


        var gc = new BMap.Geocoder();
        var map = new BMap.Map("baiduMap"); //创建地图对象
        var point = new BMap.Point(121.546457, 31.201715); //默认坐标为天安门
        var tag; //定义状态标记，使得同一时间只能创建一个点和圆
        map.centerAndZoom(point, 12); //暂时切换到默认坐标
        setTimeout(function(){
            map.setZoom(14);   
        }, 2000);  //2秒后放大到14级
        map.enableScrollWheelZoom(true);

        pageInit();
       


       
            <%
                if (action=="update")
                {
                    Response.Write(string.Format("SetPoint({0}, {1})", model.Longitude, model.Latitude));
                }
            %>
        

       function SetPoint(lng,lat){
           
           point = new BMap.Point(lng, lat); //将坐标进行刷新
           var marker = new BMap.Marker(new BMap.Point(lng,lat)); // 创建点

           try {
    

           var circle = new BMap.Circle(point, 100, {strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5}); //创建圆
           if (!tag) { //如果之前没有点击过地图
               map.addOverlay(marker); //增加点
               map.addOverlay(circle); //增加圆
               tag = true; //状态标记为已经点击过
              
           } else { //如果之前点击过地图
               map.clearOverlays(); //清除之前的数据
               map.addOverlay(marker); //增加点
               map.addOverlay(circle); //增加圆
              

           }

           map.panTo(point);
           //map.centerAndZoom(point, 15); 

           } catch (e) {
               alert(e);
           }

          

       }



       function pageInit() {
           //基础数据
           //var map = new BMap.Map("baiduMap"); //创建地图对象
           //var point = new BMap.Point(116.331398, 39.897445); //默认坐标为天安门
           //var tag; //定义状态标记，使得同一时间只能创建一个点和圆
           //map.centerAndZoom(point, 12); //暂时切换到默认坐标

           //根据IP切换到当前城市
           //function switchMap(result) { //切换地图函数
           //    var cityName = result.name; //获取城市
           //    map.setCenter(cityName); //设置地图中心点
           //}

           //var myCity = new BMap.LocalCity(); //创建地图坐标对象
           //myCity.get(switchMap); //切换地图

           //鼠标点击效果
           map.addEventListener("click", function (data) { //定义点击事件与数据
               var size = document.getElementById('txtRange').value;
               if (!size) {
                   alert('请填写圆圈的范围');
                   return
               } else {
                   point = new BMap.Point(data.point.lng, data.point.lat); //将坐标进行刷新
                   var marker = new BMap.Marker(new BMap.Point(data.point.lng, data.point.lat)); // 创建点
                   var circle = new BMap.Circle(point, size, {strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5}); //创建圆
                   if (!tag) { //如果之前没有点击过地图
                       map.addOverlay(marker); //增加点
                       map.addOverlay(circle); //增加圆
                       tag = true; //状态标记为已经点击过
                   } else { //如果之前点击过地图
                       map.clearOverlays(); //清除之前的数据
                       map.addOverlay(marker); //增加点
                       map.addOverlay(circle); //增加圆
                   }
                   $("#txtLongitude").val(point.lng);
                   $("#txtLatitude").val(point.lat);
                   gc.getLocation(point, function (rs) {
                       var addComp = rs.addressComponents;
                       $("#txtAddress").val(addComp.province+addComp.city+addComp.district+addComp.street+addComp.streetNumber);
                   })
               }
           });
       }

        



       ///////搜索////////////////////////////////////////

       function G(id){
           return document.getElementById(id);
       }

       //var map=new BMap.Map("l-map");
       //map.centerAndZoom("北京",12);
        
       var ac=new BMap.Autocomplete(
           {
               "input":"suggestId",
               "location":map
           });

       ac.addEventListener("onhighlight",function(e){
           var str="";
           var _value=e.fromitem.value;
           var value="";
           if(e.fromitem.index>-1){
               value=_value.province+_value.city+_value.district+_value.street+_value.business;
           }
           str="FromItem<br />index="+e.fromitem.index+"<br />value="+value;

           value="";
           G("searchResultPanel").innerHTML = str;
       });

        
       var myValue;
       ac.addEventListener("onconfirm", function(e) {    //鼠标点击下拉列表后的事件
           var _value = e.item.value;
           myValue = _value.province +  _value.city +  _value.district +  _value.street +  _value.business;
           G("searchResultPanel").innerHTML ="onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;
		
           setPlace();
       });

       function setPlace(){
           map.clearOverlays();    //清除地图上所有覆盖物
           function myFun(){
               var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
               map.centerAndZoom(pp, 18);
               map.addOverlay(new BMap.Marker(pp));    //添加标注
           }
           var local = new BMap.LocalSearch(map, { //智能搜索
               onSearchComplete: myFun
           });
           local.search(myValue);
       }
    </script>






</asp:Content>
