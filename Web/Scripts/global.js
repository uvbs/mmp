var zymmp = {};

zymmp.location = {
    getAllLocationData: function() {
        var result = null,
            allLocationData = localStorage.getItem('allLocationData');

        if (allLocationData && allLocationData.length > 1000) {
            result = JSON.parse(allLocationData);
        } else {
            $.ajax({
                type: 'post',
                url: '/Handler/App/CationHandler.ashx',
                async: false,
                data: {
                    action: 'QueryKeyValueData',
                    dataType: 'Province,City,District'
                },
                success: function(data) {
                    if (data.Status && data.Status == -1) {
                    }else{
                        result = data;
                        localStorage.setItem('allLocationData', result);
                    }
                    
                }
            });
        }
        return result;
    },
    getProvince: function() {
        var result = [],
            allLocationData = zymmp.location.getAllLocationData();

        if (allLocationData) {
            for (var i = 0; i < allLocationData.length; i++) {
                if (allLocationData[i].DataType == 'Province') {
                    result.push(allLocationData[i]);
                }
            }
        }
        return result;
    },
    getCity: function(provinceCode) {
        var result = [],
            allLocationData = zymmp.location.getAllLocationData();

        if (allLocationData) {
            for (var i = 0; i < allLocationData.length; i++) {
                if (allLocationData[i].DataType == 'City' && allLocationData[i].PreKey == provinceCode) {
                    result.push(allLocationData[i]);
                }
            }
        }
        return result;
    },
    getDistrict: function (cityCode) {
        var result = [],
            allLocationData = zymmp.location.getAllLocationData();

        if (allLocationData) {
            for (var i = 0; i < allLocationData.length; i++) {
                if (allLocationData[i].DataType == 'District' && allLocationData[i].PreKey == cityCode) {
                    result.push(allLocationData[i]);
                }
            }
        }
        return result;
    },

};

zymmp.init = function () {
    zymmp.location.getAllLocationData();
};

zymmp.init();