var WeixinShare={
		appid:"",
		imgUrl:"imgUrl",
		imgHidth:"200px",
		imgHeight:"200px",
		shareUrl:document.location.href,
		descContent:"descContent",
		shareTitle:"shareTitle",
		shareFriend:function(){

			var _this=this;

			WeixinJSBridge.invoke('sendAppMessage',{

				"appid":_this.appid,

				"img_url":_this.imgUrl,

				"img_width":_this.imgWidth,

				"img_height":_this.imgHeight,

				"link":_this.shareUrl,

				"desc":_this.descContent,

				"title":_this.shareTitle

			},function(res){
				//_report('send_msg',res.err_msg);
			})
		},
		shareTimeline:function(){

			var _this=this;

			WeixinJSBridge.invoke('shareTimeline',{

				"img_url":_this.imgUrl,

				"img_width":_this.imgWidth,

				"img_height":_this.imgHeight,

				"link":_this.shareUrl,

				"desc":_this.descContent,

				"title":_this.timelineTitle?_this.timelineTitle:_this.shareTitle

			},function(res){
				//_report('timeline',res.err_msg);
			});
		},
		shareWeibo:function(){

			var _this=this;

			WeixinJSBridge.invoke('shareWeibo',{

				"content":_this.descContent,

				"url":_this.shareUrl,

			},function(res){
				//_report('weibo',res.err_msg);
			});
		},
		init:function(arr){
			var _this=this;
			for(key in arr){
				this[key]=arr[key];
			}
			document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
				// alert(WeixinJSBridge);
				// 发送给好友
				WeixinJSBridge.on('menu:share:appmessage', function(argv){
					_this.shareFriend();
				});
				// 分享到朋友圈
				WeixinJSBridge.on('menu:share:timeline', function(argv){
					_this.shareTimeline();
				});
				// 分享到微博
				WeixinJSBridge.on('menu:share:weibo', function(argv){
					_this.shareWeibo();
				});
			}, false);
		}
}