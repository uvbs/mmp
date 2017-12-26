comeonModule.controller('internetDetailCtrl',['$scope','$routeParams',function ($scope,$routeParams) {
	var pageData = $scope.pageData = {
		list:[],
		currObj:null
	};
	var pageFunc = $scope.pageFunc = {};


	pageFunc.init = function () {
		pageData.list.push({
			id:'zy',
			logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzylogo.png',
			companyName:'上海至云信息科技有限公司',
			projectName: '微信号内容编辑推广',
			linker:'杜先生',
			phone:'18121076290',
			adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzyimg.jpg',
			content: '领先的移动电商、社群、营销一体化解决方案专家，由原IBM,SAP技术专家创立，基于移动端（微信）为主，适配全屏幕（PC网站，Android/IOS APP），为企业提供开发、运营、推广一体化解决方案，帮组企业实现线上线下(O2O），全渠道整合营销。自主研发具有完全知识产权的至云移动营销平台，基于移动互联网场景化，社群化的特征，为客户提供移动内容管理系统（MCMS），移动会员管理系统（MCRM），移动社群管理系统（MCommunity），移动电商系统（MShop），移动营销系统（MMS），移动业务系统（MBS），数据统计系统（DMS），消息推送系统（MDS）六大模块服务，为企业移动互联网转型保驾护航。现已累计为20多个行业的典型客户服务，其中包括上海联通、华为通信、三星电子、新闻晨报、华东理工大学、海马汽车、福布斯等一大批知名企业。<br>2015年10月，上海至云信息科技有限公司CEO杜鸿飞当选上海市第四届十佳创业新秀。上海至云将进一步领先企业级移动互联网服务市场。'
		}, {
		    id: 'zhuyang',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzhupao.png',
		    companyName: '上海助扬信息科技有限公司',
		    projectName: '新媒体运营',
		    linker: '孙先生',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzupaoneiye.jpg',
		    content: '运营关注大学生创业的信息服务网站---助跑网，希望利用新媒体的力量帮助大学生创业者稳健起步并快速成长！助跑网主要为创业者提供业界资讯、经验分享、政策查询、服务商查询、在线路演及线上社区。定期举办各种创业讲座、经验分享会、政策宣讲、创业沙龙等各类线下活动，帮助创业者更好的拓展人脉，为创业者提供一个有全面、实用的的创业服务平台。'
		}, {
		    id: 'dianli',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fdianlilog.jpg',
		    companyName: '上海点立广告公司',
		    projectName: '网络段子手，微博微信营销稿',
		    linker: '靳女士',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fdianlineiye.jpg',
		    content: '成立于2012年12月，隶属于掌尚酷媒集团。以专业的服务、创新的手段，为客户提供基于手机媒体的全面营销推广方案。公司整合无线移动营销平台的资源与技术，提供短信、彩信、WAP等新媒体的精准营销渠道，结合广告行业市场营销计划和策略，提供包括有针对性的无线营销方案和基于无线营销平台的线下活动策划与执行等一系列立体无线营销的方案，帮助客户迅速找到市场，抓住市场，占有市场！在为客户提供个人化的一站式专业服务的同时，也为无线媒体渠道资源供应商提供了广阔的业务空间。<br>中国最专业的无线服务媒体<br>中国最具资质的运营商代理<br>中国最具专业技术的平台研发公司<br>中国产品体系最完善的无线营销媒体。'
		}, {
		    id: 'quda',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqudalog.jpg',
		    companyName: '上海趣搭网络科技有限公司',
		    projectName: '电子商务',
		    linker: '肖先生',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqudaneiye.jpg',
		    content: '是一家主做3D试衣产品研究与开发的创新型互联网企业。拥有专业的3D核心技术及管理团队，致力将3D试衣技术应用到在线购物平台。通过动感3D试衣功能，打造集购物、娱乐、体验于一体的全新B2C平台。<br>趣搭在线试衣插件：是趣搭网络科技有限公司旗下设计研发的一个可集成的3D试衣插件。趣搭试衣插件主要服务于服装品牌商城。通过搭载趣搭试衣插件，快速提升你的B2C商城的购物体验<br>趣搭魔镜试衣：趣搭魔镜是一款集成趣搭3D在线试衣功能的大屏展示设备依托趣搭3D在线试衣的优势，互动展示高效、逼真的服装搭配效果，给消费者带来强力的视觉冲击力<br>趣搭手机客户端：专门为iPhone、安卓手机用户以及iPad用户提供在线3D试衣移动应用。依托自主研发的3D试衣技术，为用户全新打造集购物、娱乐、体验于一体的全新平台'
		}, {
		    id: 'qianhao',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqianhaologo.jpg',
		    companyName: '上海千浩网络科技有限公司',
		    projectName: '运营推广',
		    linker: '陈先生',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqianhaoneiye.jpg',
		    content: '公司旗下品牌，“Mazone”（玛族）采用自主设计研发为一体的主导经营方式，并且拥有500人的箱包生产基地，配备了专业的技术人员，拥有完备的生产线，加上严格的管理体制，使得千浩网络科技有限公司成为了行业内的一颗闪亮的新星。经过30年的箱包经营和生产，公司规模不断壮大，产品远销世界五大洲40多个国家和地区，深受全球客商的一致信赖和好评。同时，全新打造的国外年轻箱包品牌“Mazone”（玛族）也在天猫、京东、唯品会等电子商务平台上取得了可观的成绩，并不断蓬勃发展，越来越受到年轻人的追捧，成为极具潜力的网商品牌之一。'
		});

		if ($routeParams.id) {
			for (var i = 0; i < pageData.list.length; i++) {
				if (pageData.list[i].id == $routeParams.id) {
					pageData.currObj = pageData.list[i];
				};
			};
		}

	};

	pageFunc.init();

}]);