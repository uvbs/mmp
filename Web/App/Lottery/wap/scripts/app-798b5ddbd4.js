!function(t){"use strict";function e(e,a){this.$el=t(e),this.options=a,this.init=!1,this.enabled=!0,this._generate()}e.prototype={_generate:function(){return t.support.canvas?(this.canvas=document.createElement("canvas"),this.ctx=this.canvas.getContext("2d"),"static"===this.$el.css("position")&&this.$el.css("position","relative"),this.$img=t('<img src=""/>').attr("crossOrigin","").css({position:"absolute",width:"100%",height:"100%"}),this.$scratchpad=t(this.canvas).css({position:"absolute",width:"100%",height:"100%"}),this.$scratchpad.bindMobileEvents(),this.$scratchpad.mousedown(t.proxy(function(e){return this.enabled?(this.canvasOffset=t(this.canvas).offset(),this.scratch=!0,void this._scratchFunc(e,"Down")):!0},this)).mousemove(t.proxy(function(t){this.scratch&&this._scratchFunc(t,"Move")},this)).mouseup(t.proxy(function(t){this.scratch&&(this.scratch=!1,this._scratchFunc(t,"Up"))},this)),this._setOptions(),this.$el.append(this.$img).append(this.$scratchpad),this.init=!0,void this.reset()):(this.$el.append("Canvas is not supported in this browser."),!0)},reset:function(){var e=this,a=Math.ceil(this.$el.innerWidth()),n=Math.ceil(this.$el.innerHeight()),s=window.devicePixelRatio||1;this.pixels=a*n,this.$scratchpad.attr("width",a).attr("height",n),this.canvas.setAttribute("width",a*s),this.canvas.setAttribute("height",n*s),this.ctx.scale(s,s),this.pixels=a*s*n*s,this.$img.hide(),this.options.bg&&("#"===this.options.bg.charAt(0)?this.$el.css("backgroundColor",this.options.bg):(this.$el.css("backgroundColor",""),this.$img.attr("src",this.options.bg))),this.options.fg&&("#"===this.options.fg.charAt(0)?(this.ctx.fillStyle=this.options.fg,this.ctx.beginPath(),this.ctx.rect(0,0,a,n),this.ctx.fill(),this.$img.show()):t(new Image).attr("src",this.options.fg).load(function(){e.ctx.drawImage(this,0,0,a,n),e.$img.show()}))},clear:function(){this.ctx.clearRect(0,0,Math.ceil(this.$el.innerWidth()),Math.ceil(this.$el.innerHeight()))},enable:function(t){this.enabled=t===!0?!0:!1},destroy:function(){this.$el.children().remove(),t.removeData(this.$el,"wScratchPad")},_setOptions:function(){var t,e;for(t in this.options)this.options[t]=this.$el.attr("data-"+t)||this.options[t],e="set"+t.charAt(0).toUpperCase()+t.substring(1),this[e]&&this[e](this.options[t])},setBg:function(){this.init&&this.reset()},setFg:function(){this.setBg()},setCursor:function(t){this.$el.css("cursor",t)},_scratchFunc:function(t,e){t.pageX=Math.floor(t.pageX-this.canvasOffset.left),t.pageY=Math.floor(t.pageY-this.canvasOffset.top),this["_scratch"+e](t),(this.options.realtime||"Up"===e)&&this.options["scratch"+e]&&this.options["scratch"+e].apply(this,[t,this._scratchPercent()])},_scratchPercent:function(){for(var t=0,e=this.ctx.getImageData(0,0,this.canvas.width,this.canvas.height),a=0,n=e.data.length;n>a;a+=4)0===e.data[a]&&0===e.data[a+1]&&0===e.data[a+2]&&0===e.data[a+3]&&t++;return t/this.pixels*100},_scratchDown:function(t){this.ctx.globalCompositeOperation="destination-out",this.ctx.lineJoin="round",this.ctx.lineCap="round",this.ctx.strokeStyle=this.options.color,this.ctx.lineWidth=this.options.size,this.ctx.beginPath(),this.ctx.arc(t.pageX,t.pageY,this.options.size/2,0,2*Math.PI,!0),this.ctx.closePath(),this.ctx.fill(),this.ctx.beginPath(),this.ctx.moveTo(t.pageX,t.pageY)},_scratchMove:function(t){this.ctx.lineTo(t.pageX,t.pageY),this.ctx.stroke()},_scratchUp:function(){this.ctx.closePath()}},t.support.canvas=document.createElement("canvas").getContext,t.fn.wScratchPad=function(a,n){function s(){var n=t.data(this,"wScratchPad");return n||(n=new e(this,t.extend(!0,{},a)),t.data(this,"wScratchPad",n)),n}if("string"==typeof a){var i,c=[],o=(void 0!==n?"set":"get")+a.charAt(0).toUpperCase()+a.substring(1),r=function(){i.options[a]&&(i.options[a]=n),i[o]&&i[o].apply(i,[n])},h=function(){return i[o]?i[o].apply(i,[n]):i.options[a]?i.options[a]:void 0},u=function(){i=t.data(this,"wScratchPad"),i&&(i[a]?i[a].apply(i,[n]):void 0!==n?r():c.push(h()))};return this.each(u),c.length?1===c.length?c[0]:c:this}return a=t.extend({},t.fn.wScratchPad.defaults,a),this.each(s)},t.fn.wScratchPad.defaults={size:5,bg:"#cacaca",fg:"#6699ff",realtime:!0,scratchDown:null,scratchUp:null,scratchMove:null,cursor:"crosshair"},t.fn.bindMobileEvents=function(){t(this).on("touchstart touchmove touchend touchcancel",function(t){var e=t.changedTouches||t.originalEvent.targetTouches,a=e[0],n="";switch(t.type){case"touchstart":n="mousedown";break;case"touchmove":n="mousemove",t.preventDefault();break;case"touchend":n="mouseup";break;default:return}var s=document.createEvent("MouseEvent");s.initMouseEvent(n,!0,!0,window,1,a.screenX,a.screenY,a.clientX,a.clientY,!1,!1,!1,!1,0,null),a.target.dispatchEvent(s)})}}(jQuery),function(){"use strict";angular.module("scratchCard",["ngAnimate","ngCookies","ngTouch","ngSanitize","ngMessages","ngAria","ui.router","clientapi"])}(),function(){"use strict";function t(){return function(t){var t=parseInt(t)/1e3,e=Math.floor(t/86400),a=Math.floor((t-24*e*60*60)/3600),n=Math.floor((t-24*e*60*60-3600*a)/60),s=Math.floor(t-24*e*60*60-3600*a-60*n);return e>0?e+"天":(10>a&&(a="0"+a),10>n&&(n="0"+n),10>s&&(s="0"+s),a+":"+n+":"+s)}}function e(t,e,a,n,s,i,c,o,r,h){function u(){y.myIsAward?(y.canvasbg="assets/images/bg.png",angular.element(".maincanvas h2").css({"padding-top":"16%"})):(y.canvasbg="assets/images/bg2.png",y.myAwardName="")}function l(t){y.scratchInfo=t,angular.element("body").css({"background-color":t.background_color}),i.share({title:t.name,desc:t.share_desc,imgUrl:t.share_img_url,link:window.location.href})}function m(){var e=s.show({title:"请注意！",template:"点击领奖即视为已领奖，将无法再到服务台领取奖品",scope:t,buttons:[{text:"<b>直接领奖</b>",onTap:function(t){return!0}},{text:"<b>找客服领奖</b>",type:"button-assertive"}]});e.then(function(t){t&&(console.log(c),c.scratchReward(f("id")).then(function(t){y.myCashed=!0})["catch"](function(t){console.log(t),s.alert(JSON.stringify(t))}))})}function d(){t.userInfo=y.userInfo;var e=s.show({template:'<input type="text" ng-model="userInfo.user_truename" placeholder="姓名"><input type="tel" ng-model="userInfo.user_phone" placeholder="手机">',title:"请输入姓名跟手机",subTitle:"录入信息即可领奖",scope:t,buttons:[{text:"取消"},{text:"<b>提交</b>",type:"button-assertive",onTap:function(e){return t.userInfo.user_truename&&t.userInfo.user_phone?t.userInfo:(console.log(e),s.alert("姓名或手机没有填写"),e.preventDefault(),void 0)}}]});e.then(function(t){return c.userUpdate(t)}).then(function(t){y.myPageCash=2})["catch"](function(t){console.log(t),s.alert(JSON.stringify(t))})}function g(){y.dtime<=1e3||(y.nowTime=(new Date).getTime(),y.dtime=y.myStartTime-y.nowTime,a(function(){g()},1e3))}function f(t){var e=new RegExp("(^|&)"+t+"=([^&]*)(&|$)","i"),a=window.location.search.substr(1).match(e);return null!=a?unescape(a[2]):null}function p(){angular.element(".maincanvas").css({height:angular.element(".maincanvas").width()/2.8}).wScratchPad({size:20,bg:y.canvasbg,fg:y.canvasfg,realtime:!0,scratchUp:null,cursor:"crosshair",scratchDown:function(){y.isstart||(y.isstart=!0,w())},scratchMove:function(t,e){if(e>y.percent){if(this.clear(),"boolean"===y.myIsAward)return;y.myIsAward=y.getdata.isAward}}}),angular.element(".maincanvas").css({opacity:1})}function v(t){return o(function(e,a){var n=new Image;n.src=t,n.onload=function(){e(n)}})}function w(){location.host.match("localhost")||location.host.match("192.168.1")?y.awardurl="assets/main.json":y.awardurl="/serv/awardapi.ashx?action=scratch&id="+f("id"),e({method:"get",url:y.awardurl}).success(function(t,e,a,n){y.getdata=t,y.myAwardName=t.awardName}).error(function(t,e,a,n){y.myAwardName="抽奖失败，请刷新后重新抽奖"})}angular.element(".maincanvas").css({opacity:0}),h.show("加载中...");var y=this;y.myAwardGameOver=n.myAwardGameOver,y.myStartTime=new Date(n.myStartTime).getTime(),y.myIsAward=n.myIsAward,y.myAwardName=n.myAwardName,y.myCashed=n.myCashed,y.myPageCash=n.myPageCash,y.nowTime=(new Date).getTime(),y.isstart=!1,y.canvasfg="assets/images/fg.png",y.canvasbg="#fff",y.percent=90,y.getAward=m,y.updateMyInfo=d,y.userInfo={user_truename:"",user_phone:""},y.myAwardGameOver?(y.isstart=!0,y.canvasfg="",y.myAwardName="对不起,活动已结束！"):y.nowTime<y.myStartTime?g():"boolean"==typeof y.myIsAward&&(u(),y.canvasfg="assets/images/fg2.png",y.isstart=!0,y.percent=100);c.scratchInfo(f("id")).then(function(t){l(t);var e=v(t.img_url),a=v("assets/images/fg.png"),n=v("assets/images/fg2.png");o.all([e,a,n]).then(function(){h.hide(),p()})})["catch"](function(t){console.log(t),s.alert(JSON.stringify(t))});if(3===y.myPageCash){c.userInfo().then(function(t){y.userInfo.user_truename=t.truename,y.userInfo.user_phone=t.phone,t.truename&&t.phone&&(y.myPageCash=2)})["catch"](function(t){console.log(t),s.alert(JSON.stringify(t))})}}angular.module("scratchCard").controller("MainController",e).filter("countDown",t),e.$inject=["$scope","$http","$timeout","scratchCardStatus","wPopup","wxAPI","nAPI","$q","$filter","wLoading"]}(),function(){"use strict";function t(t){t.debug("runBlock end")}angular.module("scratchCard").run(t),t.$inject=["$log"]}(),function(){"use strict";function t(t,e){t.state("home",{url:"/",templateUrl:"app/main/main.html",controller:"MainController",controllerAs:"main"}),e.otherwise("/")}angular.module("scratchCard").config(t),t.$inject=["$stateProvider","$urlRouterProvider"]}(),function(){"use strict";angular.module("scratchCard").constant("scratchCardStatus",{myAwardGameOver:!1,myStartTime:"2015-11-26 17:35:00",myPageCash:2})}(),function(){"use strict";function t(t){t.debugEnabled(!0)}angular.module("scratchCard").config(t),t.$inject=["$logProvider"]}(),angular.module("scratchCard").run(["$templateCache",function(t){t.put("app/main/main.html",'<div class="mainbox" style=""><img class="mainlogo" ng-src="{{main.scratchInfo.img_url}}" alt=""><div class="maincanvas"><h2 ng-bind="main.myAwardName" ng-class="{cashed:main.myCashed}"></h2><div class="gamenotstart" ng-if="main.dtime>1000" ng-bind="main.dtime | countDown"></div></div><span ng-if="main.myPageCash==2&&main.myIsAward&&!main.myCashed" class="cashbtn" ng-click="main.getAward()">领奖(非客服勿点)</span> <span ng-if="main.myPageCash==3&&main.myIsAward&&!main.myCashed" class="cashbtn" ng-click="main.updateMyInfo()">完善资料后才可领奖</span><div class="article container" ng-bind-html="main.scratchInfo.content"></div></div>')}]);
//# sourceMappingURL=../maps/scripts/app-798b5ddbd4.js.map
