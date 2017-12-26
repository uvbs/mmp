var app = angular.module('storeMap', []);
app.controller('mapController', function ($rootScope,$scope, $http, $timeout) {
    var vm = $scope.vm = {
    	provinceList:[{ code: "0", name: "请选择省份" }],//获取省份
    	province:{},//当前选中省份
    	cityList: [{ code: "0", name: "请选择城市" }],//所有城市列表
	    city:{},//当前选中城市
	    currentCity:'',//当前城市
    	storeMapList:[],//获取门店列表
    	longitude:0,//经度
    	latitude:0,//纬度
    	isShowSelectCity:false,//是否显示城市选择框
    	range:500,//搜索范围
    	page:1,//当前页码
    	rows: 10000,//每页显示多少条数据
        currentTab:1,//当前显示的Tab 1地图 2门店
        isShowStoreInfo:false,//是否显示门店信息
        storeInfo: {},//门店信息
        limitRange: 1,//是否限制搜索范围 0不限制 1 限制
        markerList:[]//当前标注点集合
	};

	var vf = $scope.vf = {
		getProvince:getProvince,//获取省份
		getCity:getCity,//所有城市列表
		mapCoordinate:mapCoordinate,//显示周边门店
		onComplete:onComplete,//解析定位结果
		onError:onError,//解析定位错误信息
		setPostion:setPostion,//选择省市区地图定位
		getCityName:getCityName,//获取城市名称
		goProductList: goProductList,//跳转到门店商品列表
		setCurrentTab: setCurrentTab,//设置当前Tab 1地图 2门店
		showStoreInfo: showStoreInfo,//显示门店信息
		formartDistance: formartDistance,//格式化
		selectCity:selectCity,//选择城市
		init:init//初始化
    };
    var map;//地图
    var geolocation;//位置插件
    var geocoder;
    //初始化
    function init(){
    	map = new AMap.Map('container',{
            resizeEnable: true,
            zoom: 14,
            // center: [116.480983, 40.0958]
        });
    	map.plugin(['AMap.Geolocation'], function () {
            
    	   
	        geolocation = new AMap.Geolocation({
	            enableHighAccuracy: true,//是否使用高精度定位，默认:true
	            timeout: 10000,          //超过10秒后停止定位，默认：无穷大
	            buttonOffset: new AMap.Pixel(20, 180),//定位按钮与设置的停靠位置的偏移量，默认：Pixel(10, 20)
	            zoomToAccuracy: false,//定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
	            buttonPosition:'LB',
	            showMarker:true,
	            resizeEnable: true,
	            showCircle: false,
	            buttonDom: "<image src=\"images/geo.png\" class=\"geo-icon\" />",
	            markerOptions: {
	                map: map,
	                //position: [vm.longitude, vm.latitude],
	                icon: new AMap.Icon({
	                    size: new AMap.Size(20, 20),  //图标大小
	                    image: "images/postion.png"
	                    //imageOffset: new AMap.Pixel(0, -60)
	                })

	            }
	        });
	        map.addControl(geolocation);
	        geolocation.getCurrentPosition();
	        AMap.event.addListener(map,'zoomend',function(){
		        //document.getElementById('info').innerHTML = '当前缩放级别：' + map.getZoom();
		    });
	        AMap.event.addListener(geolocation, 'complete', onComplete);//返回定位信息
	        AMap.event.addListener(geolocation, 'error', onError);      //返回定位出错信息
	    });

	    AMap.service('AMap.Geocoder',function(){//回调函数
		    //实例化Geocoder
		    geocoder = new AMap.Geocoder({
		        city: "010"//城市，默认：“全国”
		    });
		    //TODO: 使用geocoder 对象完成相关功能
		})
	    
        vf.getProvince();//获取省份
       // vm.city = vm.cityList[0];
    }

    //获取城市名称
    function getCityName(){
    	geocoder.getAddress([vm.longitude, vm.latitude], function(status, result) {
    		console.log(["result0",result]);
		    if (status === 'complete' && result.info === 'OK') {
		       //获得了有效的地址信息:
		       //即，result.regeocode.formattedAddress
		       if(result.regeocode.addressComponent.city==""){
		       		vm.currentCity=result.regeocode.addressComponent.province;
		       }else{
		       		vm.currentCity=result.regeocode.addressComponent.city;
		       }
		      
		    }else{
		       //获取地址失败
		    }
		});  
    }

    //解析定位结果
    function onComplete(data) {
        //var str=['定位成功'];
        //str.push('经度：' + data.position.getLng());
        //str.push('纬度：' + data.position.getLat());
        vm.isShowSelectCity = false;
        vm.longitude=data.position.getLng();
        vm.latitude = data.position.getLat();
        $("#divStoreInfo").hide();
        vm.limitRange = 1;//定位时限制搜索范围
        vm.currentCity = ""; 
        vf.getCityName();
        vf.mapCoordinate();//显示周边门店
    }

    //解析定位错误信息
    function onError(data) {
        vm.currentCity = "";
    }

    //显示周边门店
    function mapCoordinate(){
    	//var submitData={
		//	longitude:vm.longitude,
	    //	latitude:vm.latitude,
	    //	range: vm.range,
        //    limit_range:vm.limitRange,
	    //	page:vm.page,
	    //	rows:vm.rows
		//}
		//console.log(submitData);
		var storeMapUrl="/serv/api/outlets/list.ashx";
		if(window.location.host.indexOf('localhost')>-1||window.location.host==""){
			storeMapUrl="http://sandbox.comeoncloud.net/serv/api/outlets/list.ashx"
		}
		
		$http({url:storeMapUrl,params:{
			'longitude':vm.longitude,
			'latitude':vm.latitude,
			'range':vm.range,
			'page':vm.page,
			'rows': vm.rows,
			'limit_range': vm.limitRange,
			'city': vm.currentCity
		}}).success(function(resp, status, headers, config){
			vm.storeMapList=resp.result.list;
			console.log("vm.storeMapList",vm.storeMapList);
			if(resp.status){
				try{
					if(resp.result.list.length==0){
						layer.open({
						  content: '当前城市附近没有门店'
						});   
					}
					vm.markerList = [];
					for (var i = 0; i < resp.result.list.length; i++) {
						//添加点标记，并使用自己的icon
					   var marker= new AMap.Marker({
					        map: map,
					        position: [resp.result.list[i].longitude, resp.result.list[i].latitude],
					        icon: new AMap.Icon({            
					            size: new AMap.Size(42, 53),  //图标大小
					            //image: "http://sandbox.comeoncloud.net/img/way_btn2.png",
					            image: "images/marker.png",
					            //imageOffset: new AMap.Pixel(0, -60)
					        }),
					        extData:resp.result.list[i]
					    });
					    //marker.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
					    //    offset: new AMap.Pixel(35, 20),//修改label相对于maker的位置
					    //    content: resp.result.list[i].title
					    //});
					   vm.markerList.push(marker);
						 marker.on('click', function(item) {
							var exData=this.getExtData();
							vf.showStoreInfo(exData);
							for (var k = 0; k < vm.markerList.length; k++) {

							    vm.markerList[k].setIcon("images/marker.png");
							}
							this.setIcon("images/markerselect.png");




						 });

						 
					}

				}
				catch(e){
					alert(e);
				}
			}
        }).error(function(data, status, headers, config){
        	
        })
    }

    //获取省份
    function getProvince(){
    	
		var provinceUrl="/serv/api/mall/area.ashx?action=provinces";
		if(window.location.host.indexOf('localhost')>-1||window.location.host==""){
			provinceUrl="http://sandbox.comeoncloud.net/serv/api/mall/area.ashx?action=provinces"
		}
		var proChoose = { code: "0", name: "选择省份" };
		$http({url:provinceUrl}).success(function(resp, status, headers, config){
			vm.provinceList=resp.list;
			if(vm.provinceList instanceof Array==false){
				console(vm.provinceList);
			}
			vm.provinceList.unshift(proChoose);
			vm.province = vm.provinceList[0];
        }).error(function(data, status, headers, config){
        	
        })
    }

    //获取城市
    function getCity(){
    	var submitData={
			province_code:vm.province.code
		}
		console.log(submitData);
		var cityUrl="/serv/api/mall/area.ashx?action=cities";
		if(window.location.host.indexOf('localhost')>-1||window.location.host==""){
			cityUrl="http://sandbox.comeoncloud.net/serv/api/mall/area.ashx?action=cities"
		}
		var cityChoose = { code: "0", name: "请选择城市" };
		$http({url:cityUrl,params:{
			'province_code':submitData.province_code
		}}).success(function(resp, status, headers, config){
			vm.cityList=resp.list;
			vm.cityList.unshift(cityChoose);
			vm.city = vm.cityList[0];
        }).error(function(data, status, headers, config){
        	
        })
    }

    //选择省市区地图定位
    function setPostion(){
    	if(vm.city.name==""||vm.city.name=="请选择城市"||vm.city.name==undefined){
	   		layer.open({
				content: '请选择省份城市'
			});
			return;
	   	}
    	vm.isShowSelectCity = false;
    	vm.currentTab = 1;
    	vm.limitRange = 0;
	    vm.currentCity=vm.city.name;
	    map.setCity(vm.city.name);
	    
	   	$("#divStoreInfo").hide();
	   	$timeout(function() {

	   		var position=map.getCenter();
	   		console.log(map.getCenter());
	   	  	vm.longitude=position.lng;
	   	  	vm.latitude = position.lat;
	   	  	map.setZoom(13);
    		vf.mapCoordinate();//显示周边门店

	   }, 1000);  
    }

    //跳转到商品列表
    function goProductList(item,storeUrl){
       
    	var storeListUrl="/serv/api/user/bindsupplierid.ashx";
		if(window.location.host.indexOf('localhost')>-1||window.location.host==""){
			storeListUrl="http://sandbox.comeoncloud.net/serv/api/user/bindsupplierid.ashx"
		}
		$http({url:storeListUrl,params:{
			'supplier_id':item.supplier_id
			
		}}).success(function(resp, status, headers, config){
		    if (resp.status) {
		        window.location.href = "/customize/comeoncloud/Index.aspx?cgid=1595";
		        //if (storeUrl==true) {
		        //    window.location.href = "/customize/comeoncloud/Index.aspx?cgid=1595";
		        //}
		        //else {
		        //    window.location.href = "/customize/comeoncloud/Index.aspx?cgid=1594";
		        //}
			    
			}
        }).error(function(data, status, headers, config){

        })
    	
    }

    //设置选中的Tab
    function setCurrentTab(par) {

        vm.currentTab = par;
        vm.isShowSelectCity = false;
        $("#divStoreInfo").hide();
    }

    //显示门店信息
    function showStoreInfo(storeInfo) {
        //vm.isShowStoreInfo = true;
        vm.storeInfo = storeInfo;
       
        $("#txtStoreName").html(vm.storeInfo.title);
        var storeAddress = vm.storeInfo.province + vm.storeInfo.city + vm.storeInfo.district + vm.storeInfo.address;
        $("#txtStoreAddress").html(storeAddress);
        var distance = formartDistance(vm.storeInfo.distance);
        $("#lblDistance").html(distance);
        //console.log(["vm.storeInfo", vm.storeInfo]);
        $("#divStoreInfo").show();
        
        //$scope.$apply(function () {
        //    //wrapped this within $apply
        //    vm.isShowStoreInfo = true; 
        //    vm.storeInfo = storeInfo;
        //    console.log(vm.isShowStoreInfo);
            
        //});
    }
    //格式化距离
    function formartDistance(value) {

        if (!value || value == "") return "";
        if (value < 0) return "";
        var m = value * 1000;
        if (m < 10) return "<10m";
        if (m < 1000) return Math.round(m) + "m";
        return Math.round(value * 100) / 100 + "km";
    }

    //选择城市
    function selectCity() {
        vm.isShowSelectCity = !vm.isShowSelectCity;
        

    }

    
    vf.init();//初始化
});