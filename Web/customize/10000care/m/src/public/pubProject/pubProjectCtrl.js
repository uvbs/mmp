comeonModule.controller('pubProjectCtrl',['$scope','commService',function ($scope,commService) {
	var pageData = $scope.pageData = {
		title:'发布项目 - ' + baseData.slogan,
		category:'',
		projectCycle:0,//项目周期
		logistics:0,//项目物流 0企业负责配送  1基地负责配送 
		timeRequirement:'',//工作要求
		introduction:'',//项目介绍
		projectName:''
	};
	var pageFunc = $scope.pageFunc = {};

	document.title = pageData.title;

	/**
	 * [save 保存]
	 * @return {[type]}          [description]
	 */
	pageFunc.save = function () {
		var jsonData = {
			AutoID: 0,
			Action: 'AddProjectInfo',
			ProjectName:pageData.projectName,
			Category: pageData.category,
			Logistics: pageData.logistics,
			ProjectCycle: pageData.projectCycle,
			TimeRequirement: pageData.timeRequirement,
			Introduction: pageData.introduction
		};

		if (jsonData.ProjectName == '') {
			// layer.open({content:'项目名不能为空'});
			alert('项目名不能为空');
			return;
		};
		if (jsonData.Category == '') {
			alert('请选择分类');
			return;
		};
		if (jsonData.ProjectCycle == '') {
			alert('请选择周期');
			return;
		};

		commService.postData(baseData.handlerUrl,jsonData,function (data) {
			if (data.Status == 1) {
				alert('保存成功');
				setTimeout(function () {
					window.location.href = '#/projectList';
				},1000);
			}else{
				alert(data.Msg);
			}
		},function (data) {
			// body...
		});

	};

	pageFunc.changeLogistics = function(value){
		pageData.logistics = value;
	};

}]);