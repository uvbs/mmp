comeonModule.controller('companyEditCtrl',['$scope','commService',function ($scope,commService) {
	var pageData = $scope.pageData = {
		user:JSON.parse(sessionStorage.getItem('user'))
	};
	var pageFunc = $scope.pageFunc = {};

	pageFunc.save = function (argument) {
		var model =
            {
                Action: "UpdateCompanyInfo",
                CompanyName: pageData.user.CompanyName,
                Thumbnails: pageData.user.Thumbnails,
                Address: pageData.user.Address,
                Area: pageData.user.Area,
                Tel: pageData.user.Tel,
                Phone: pageData.user.Phone,
                QQ: pageData.user.QQ,
                Contacts: pageData.user.Contacts,
                BusinessLicenseNumber: pageData.user.BusinessLicenseNumber,
                Introduction: pageData.user.Introduction
            };


        if (model.CompanyName == '') {
            alert('企业名称不能为空');
            return;
        }
        if (model.Contacts == '') {
            alert('负责人不能为空');
            return;
        }

        commService.postData(baseData.handlerUrl,model,function (data) {
			if (data.Status == 1) {
				alert('保存成功');
				sessionStorage.setItem('user',JSON.stringify(pageData.user));
				
				setTimeout(function() {
					window.location.href = "#/my";
				},1000);
				
			}else{
				alert(data.Msg);
			}
		},function (data) {
			// body...
			console.log(data);
		});


	}

}]);