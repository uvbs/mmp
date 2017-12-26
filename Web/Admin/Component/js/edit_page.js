
var editComponentModule = angular.module("editComponentModule", ['comm-services', 'comm-filters', 'cache-page', 'activitylist',
    'cardlist', 'content', 'foottool', 'headerbar', 'malls', 'goods', 'navs', 'notice', 'search', 'shareinfo', 'sidemenu', 'slide', 'tabs',
    'totop', 'userinfo', 'block', 'linetext', 'linebutton', 'linehead', 'headsearch', 'zcSlider', 'zcSelectLink', 'zcShop', 'angular-carousel']);
editComponentModule.controller("editComponentCtrl", ['$scope', '$sce', '$parse', 'commService', '$timeout','$compile',
    function ($scope, $sce, $parse, commService, $timeout, $compile) {
        var vm = $scope.$root.vm = {
        load_def_model_id: 13,//22,//13,
        handlerUrl: '/serv/api/admin/component',
        is_edit: true,
        backlist: backlist,
        iconclasses: iconclasses,
        component: component,
        component_model_list: [],
        component_model: component_model,
        login_info: login_info,
        only_contorls: {
            pageinfo: {
                title: '',
                bg_img: '',
                bg_color: ''
            },
            shareinfo: {
                title: '',
                desc: '',
                img_url: '',
                link: ''
            },
            headerbar: null,
            foottool: null,
            sidemenubox: null,
            totop: null,
            headsearch: null
        },
        right_controls: [],
        only_value_controls: {

        },
        control_rels: {
            slides: 'slides',
            searchbox: 'search',
            head_bar: 'headerbar',
            userinfo: 'userinfo',
            navs: 'navs',
            foottool_list: 'foottool',
            tab_list: 'tabs',
            activitys: 'activitylist',
            malls: 'malls',
            goods: 'goods',
            sidemenubox: 'sidemenubox',
            cards: 'cardlist',
            notice: 'notice',
            totop: 'totop',
            content: 'content',
            block: 'block',
            linetext: 'linetext',
            linebutton: 'linebutton',
            linehead: 'linehead',
            headsearch: 'headsearch'
        },
        control_data_types: [
            'slides',
            'navs',
            'foottool',
            'tabs',
            'activitys',
            'goods',
            'malls',
            'cardlist',
            'activitylist',
            'headsearch'
        ],
        exclude_props: [
            'mall_list',
            'mall_total',
            'card_data',
            'activity_list'
        ],
        can_move_controls: ['headerbar', 'foottool', 'totop'],
        limit: {
            headerbar: 1,
            sidemenubox: 1,
            headsearch: 1,
            foottool: 1,
            userinfo: 1,
            search: 1,
            totop: 1,
            linehead:1
        },
        show_tr_model: false,
        cur_component: null,
        pageConfig: pageConfig,
        mallConfig: mallConfig.mallconfignew,
        slides: slides,
        toolbars: toolbars,
        mall_cates: mall_cates,
        course_cates: course_cates,
        mall_tags: mall_tags,
        art_cates: art_cates,
        act_cates: act_cates,
        art_data: [
        ],
        act_data: [
        ],
        toolbar_data: [
        ],
        slide_data: [
        ],
        mall_data: [
        ],
        good_data: [
        ],  
        temp_data: [{key:"_",data:[],total:-1}],
        edit_template: edit_template,
        template_select_set: {
            page: 1,
            rows: 50,
            cate: '',
            keyword : ''
        },
        template_types: template_types,
        template_config: null,
        template_data: null,
        template_img: null,
        template_cate: null,
        user_data: {},
        cur_select_controls: [],
        rightPaddingTop: 5,
        sortableOptions: {},
        show_more_option: false,
        show_page_set: false,  //是否显示页面配置
        kindeditorControl: null,//显示富文本框
        editor: null,
        //show_high_choose:false  //是否显示高级选项
        qrcode: '',  //生成的二维码
        layerAddType: null,
        layerSelectToolbar: null,
        layerSaveTemp: null,
        layerSelectTemp: null,
        curAddTypeModel: null,
        curSelectToolbarModel: null,
        curSelectToolbarIndex: null,
        curSelectToolbarListKey:null,
        curSelectAttrModel: null,
        curSelectAttrKey: null,
        strDomain: strDomain
    };
    var vmFunc = $scope.$root.vmFunc = {
        init: init,
        selectRightControl: selectRightControl,
        editControlData: editControlData,
        editControlConfig: editControlConfig,
        addControl: addControl,
        deleteControl: deleteControl,
        submitConfig: submitConfig,
        toListPage: toListPage,
        addSidemenuControl: addSidemenuControl,
        createQrCode: createQrCode,
        editExData: editExData,
        addExData: addExData,
        deleteExData: deleteExData,
        showAddType: showAddType,
        showSelectToolbarClass: showSelectToolbarClass,
        showSelectAttrClass: showSelectAttrClass,
        saveTemplateDialog: saveTemplateDialog,
        selectTemplateDialog: selectTemplateDialog,
        isChecked: isChecked,
        checkboxControlData: checkboxControlData,
        GetControlRow: GetControlRow,
        moveModelUp: moveModelUp,
        moveModelDown: moveModelDown,
        moveUpExData: moveUpExData,
        moveDownExData: moveDownExData,
        selectTempSearch: selectTempSearch,
        setHideColor: setHideColor,
        setMoveColor: setMoveColor,
        setChangeColor: setChangeColor,
        bindColor: bindColor,
        getPageBg: getPageBg,
        getgagebg: getPageBg,
        clearBgImg: clearBgImg
    };
    vmFunc.init();
    //初始化页面加载
    function init() {
        if (vm.edit_template == 0) vmFunc.createQrCode();
        //initTemplateCateList();
        initKindEditor();
        initBind();
        if (!vm.component.ComponentConfig) {
            vm.component.ComponentConfig = {};
            selectTemplateDialog(1);
        }
        else {
            vm.component.ComponentConfig = angular.fromJson(vm.component.ComponentConfig);
            if (vm.component.ComponentConfig.first_edit) selectTemplateDialog(1);
        }
        if (template && template.Data) {
            vm.template_data = angular.fromJson(template.Data);
            if (!vm.template_data.good_temp_data) vm.template_data.good_temp_data = [];
        }
        else {
            vm.template_data = {
                art_temp_data: [],
                act_temp_data: [],
                toolbar_temp_data: [],
                slide_temp_data: [],
                mall_temp_data: [],
                good_temp_data: []
            };
        }
        if (template && template.ThumbnailsPath) vm.template_img = template.ThumbnailsPath;
        if (template && template.CateId) vm.template_cate = template.CateId;
        
        vm.component.IsWXSeniorOAuth = vm.component.IsWXSeniorOAuth === 1;
        vm.component.IsInitData = vm.component.IsInitData === 1;
        if (vm.component_model.component_model_id) {
            getRightControls();
            vm.show_tr_model = false;
        }
        else {
            //vm.show_tr_model = true;
            //LoadSelectModelData();
            vm.show_tr_model = false;
            LoadDefModelData();
        }
        $timeout(function () {
            selectRightControl(1);
            bindColor();
        });
    }
    function initKindEditor() {
        KindEditor.ready(function (K) {
            vm.editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table', 'cleardoc'],
                filterMode: false,
                extraFileUploadParams: { userID: 'songhe' },
                width: '96%',
                minHeight: "500px",
                cssData: ' body, html{overflow: auto !important;}img{ max-width: 100%;}',
                cssPath: ['/customize/comeoncloud/m2/dist/all.min.css?v=20160715'],
                afterChange: function (ex1, ex2) {
                    var tempHtml = false;
                    if (!!vm.editor) tempHtml = vm.editor.html();
                    if (!!vm.kindeditorControl && vm.kindeditorControl.config && vm.kindeditorControl.config.thtml != tempHtml) {
                        $timeout(function () {
                            $scope.$apply(function () {
                                vm.kindeditorControl.config.thtml = tempHtml;
                            });
                        });
                        //$scope.$apply(function () {
                        //    var curControl = GetControlRow(vm.right_controls, vm.kindeditorControl.control_id);
                        //    if (!!curControl) curControl.config.thtml = vm.kindeditorControl.config.thtml;
                        //});
                    }
                }
            });
        });
    }
    function baseReloadArray(obj) {
        var tempTb = angular.copy(obj);
        obj = [];
        $timeout(function () {
            $scope.$apply(function () {
                obj = tempTb;
            });
        });
    }
    function baseReload(type) {
        var tempTb = [];
        if (type == 1) {
            tempTb = angular.copy(vm.right_controls);
            vm.right_controls = [];
        }
        else if (type == 2) {
            tempTb = angular.copy(vm.cur_select_controls);
            vm.cur_select_controls = [];
        }
        $timeout(function () {
            $scope.$apply(function () {
                if (type == 1) {
                    vm.right_controls = tempTb;
                }
                else if (type == 2) {
                    vm.cur_select_controls = tempTb;
                }
            });
        });
    }
    function baseReloadOnly(control) {
        var tempTb = null;
        if (control == 'headerbar') {
            tempTb = angular.copy(vm.only_contorls.headerbar);
            vm.only_contorls.headerbar = null;
        }
        else if (control == 'foottool') {
            tempTb = angular.copy(vm.only_contorls.foottool);
            vm.only_contorls.foottool = null;
        }
        $timeout(function () {
            $scope.$apply(function () {
                if (control == 'headerbar') {
                    vm.only_contorls.headerbar = tempTb;
                }
                else if (control == 'foottool') {
                    vm.only_contorls.foottool = tempTb;
                }
            });
        });
    }
    function addType(cModel, newName, key) {
        if (cModel.control == 'slides') {
            if (!key) key = 'slide_list';
            $timeout(function () {
                $scope.$apply(function () {
                    vm.slides.unshift(newName);
                    cModel.data = [];
                    cModel.config.slide_list = newName;
                    vm.slide_data.push({ key: newName, data: cModel.data });
                });
            });
        }
        else if (cModel.control == 'foottool' || cModel.control == 'tabs' || cModel.control == 'navs') {
            if (cModel.control == 'navs') {
                if (!key) key = 'nav_list';
            }
            else if (cModel.control == 'foottool' || cModel.control == 'tabs') {
                if (!key) key = 'key_type';
            }
            $timeout(function () {
                $scope.$apply(function () {
                    vm.toolbars.unshift({ key_type: newName, use_type: 'nav' });
                    cModel.data = [];
                    cModel.config[key] = newName;
                    vm.toolbar_data.push({ key: newName, data: cModel.data });
                });
            });
        }
        else if (cModel.control == 'headsearch') {
            $timeout(function () {
                $scope.$apply(function () {
                    //vm.toolbars.unshift({ key_type: newName, use_type: 'nav' });
                    if (key == "nav_left") {
                        cModel.left_list = [];
                    } else {
                        cModel.right_list = [];
                    }
                    cModel.config[key] = newName;
                    //vm.toolbar_data.push({ key: newName, data: cModel.data });
                });
            });
        }
        layer.close(vm.layerAddType);
    }
    //绑定页面jquery事件
    function initBind() {
        //二维码弹出 收起
        $(".qrcode").mouseover(function () {
            $(this).stop().animate({ "right": 0 }, 300);
        })
        $(".qrcode").mouseleave(function () {
            $(this).stop().animate({ "right": -130 }, 300);
        })

        $(".left-col,.right-col").on('click', '.check-color', function () {
            if (this.checked) {
                $(this).closest("td").find('input[type="color"]').hide();
            }
            else {
                $(this).closest("td").find('input[type="color"]').show();
            }
        });
        //$('.left-col input[type="color"],.right-col input[type="color"]').each(function () {
        //    console.log($(this).val());
        //});
        $(".left-col,.right-col").on('click', '.imgUpload', function () {
            $(this).next().click();
        });
        $(".left-col,.right-col").on('change', '.txtFile', function () {
            var _src = $(this).val();
            if (_src == '') {
                $(this).prev().prev().prev().prev().removeAttr("src");
            }
            else {
                $(this).prev().prev().prev().prev().attr("src", _src);
            }
        });
        $(document).on('click', '.dialogDiv .confirm', function () {
            var newName = $.trim($(this).closest("table").find('.txt').val());
            var key = $(this).closest("table").find('.txt').attr('data-key');
            if (newName == "") {
                alert('请输入类型名称！');
                return;
            }
            var hasName = false;
            if (vm.curAddTypeModel.control == 'slides') {
                hasName = $.inArray(newName, vm.slides) >= 0;
            }
            else if (vm.curAddTypeModel.control == 'foottool' || vm.curAddTypeModel.control == 'tabs' || vm.curAddTypeModel.control == 'navs') {
                var nl = $.grep(vm.toolbars, function (cur, i) {
                    return cur['key_type'] == newName;
                });
                if (nl.length > 0) hasName = true;
            }
            else if (vm.curAddTypeModel.control == 'headsearch') {
                var nl = $.grep(vm.toolbars, function (cur, i) {
                    return cur['key_type'] == newName;
                });
                if (nl.length > 0) hasName = true;
            }
            if (hasName) {
                alert('该类型名称已存在！');
                return;
            }
            addType(vm.curAddTypeModel, newName, key);
        });

        $(document).on('click', '.dialogSelectToolbarDiv .liIco', function () {
            var newClass = $.trim($(this).attr("data-ico"));
            //if (newClass == "") {
            //    alert('请选择图标！');
            //    return;
            //}
            newClass = newClass.replace('iconfont ', '');
            var parentTable = $(this).closest('.data-table');
            $scope.$apply(function () {
                if (vm.curSelectToolbarListKey) {
                    vm.curSelectToolbarModel[vm.curSelectToolbarListKey][vm.curSelectToolbarIndex].ico = newClass;
                    vm.curSelectToolbarModel.is_edit = 1;
                    vm.curSelectToolbarModel[vm.curSelectToolbarListKey][vm.curSelectToolbarIndex].img = '';
                } else {
                    vm.curSelectToolbarModel.data[vm.curSelectToolbarIndex].ico = newClass;
                    vm.curSelectToolbarModel.is_edit = 1;
                    vm.curSelectToolbarModel.data[vm.curSelectToolbarIndex].img = '';
                    if (vm.curSelectToolbarModel.control == 'linehead') {
                        vm.curSelectToolbarModel.data[vm.curSelectToolbarIndex].type = 'ico';
                    }
                }
            });
            //SetControlRowIndexData(vm.cur_select_controls, vm.curSelectToolbarModel.control_id, vm.curSelectToolbarIndex, 'img', '');
            //SetControlRowIndexData(vm.cur_select_controls, vm.curSelectToolbarModel.control_id, vm.curSelectToolbarIndex, 'ico', newClass);
            //baseReload(2);
            //if (vm.curSelectToolbarModel.control == 'foottool') {
            //    vm.only_contorls.foottool.is_edit = 1;
            //    vm.only_contorls.foottool.data[vm.curSelectToolbarIndex].img = '';
            //    vm.only_contorls.foottool.data[vm.curSelectToolbarIndex].ico = newClass;
            //    //baseReloadOnly(vm.only_contorls.foottool);
            //}
            //else {
            //    SetControlRowIndexData(vm.right_controls, vm.curSelectToolbarModel.control_id, vm.curSelectToolbarIndex, 'img', '');
            //    SetControlRowIndexData(vm.right_controls, vm.curSelectToolbarModel.control_id, vm.curSelectToolbarIndex, 'ico', newClass);
            //    //baseReload(1);
            //}
            //$(parentTable).find('.toolbarIco').find('use').attr('xlink:href', newClass)
            if (!$(parentTable).find('.toolbarImgDiv').hasClass('ng-hide')) {
                $(parentTable).find('.toolbarImgDiv').addClass('ng-hide');
            }
            if ($(parentTable).find('.toolbarIcoDiv').hasClass('ng-hide')) {
                $(parentTable).find('.toolbarIcoDiv').removeClass('ng-hide');
            }
            layer.close(vm.layerSelectToolbar);
        });
        $(document).on('click', '.dialogSelectAttrDiv .liIco', function () {
            var newClass = $.trim($(this).attr("data-ico"));
            //if (newClass == "") {
            //    alert('请选择图标！');
            //    return;
            //}
            newClass = newClass.replace('iconfont ', '');
            $scope.$apply(function () {
                vm.curSelectAttrModel.config[vm.curSelectAttrKey] = newClass;
            });
            //SetControlRowConfig(vm.cur_select_controls, vm.curSelectAttrModel.control_id, vm.curSelectAttrKey, newClass);
            //baseReload(2);

            //if (vm.curSelectAttrModel.control == 'headerbar') {
            //    vm.only_contorls.headerbar.config[vm.curSelectAttrKey] = newClass;
            //    //baseReloadOnly('headerbar');
            //}
            //else {
            //    SetControlRowConfig(vm.right_controls, vm.curSelectAttrModel.control_id, vm.curSelectAttrKey, newClass);
            //    //baseReload(1);
            //}
            layer.close(vm.layerSelectToolbar);
        });
        $(document).on('click', '.dialogTemplateDiv .tempImg', function () {
            $(this).next().click();
        });
        $(document).on('click', '.dialogTemplateDiv .imgTempBgDiv', function () {
            $(this).closest('div').next().find('.tempImgFile').click();
        });
        $(document).on('change', '.dialogTemplateDiv .tempImgFile', function () {
            var fpath = $.trim($(this).val());
            if (fpath == "") return;
            var tObj = $(this);
            var imgObj = $(this).prev();
            try {
                progress();
                $.ajaxFileUpload(
                 {
                     url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                     secureuri: false,
                     fileElement: $(this),
                     dataType: 'json',
                     success: function (resp) {
                         layer.close(layerIndex);
                         if (resp.Status == 1) {
                             $(tObj).val('');
                             $(imgObj).attr('src', resp.ExStr);
                             $('.imgTempBgDiv').hide();
                             $('.imgBgDiv').show();
                         }
                         else {
                             alert(resp.Msg);
                         }
                     }
                 });

            } catch (e) {
                alert(e);
            }
        });
        $(document).on('click', '.dialogTemplateDiv .confirm', function () {
            var tempName = $.trim($(this).closest("table").find('.txt').val());
            var tempImg = $.trim($(this).closest("table").find('.tempImg').attr('src'));
            var tempCate = $.trim($(this).closest("table").find('.ddlTempCate').val());
            
            if (tempName == "") {
                alert('请输入模板名称');
                return;
            }
            //if (tempCate == "") {
            //    alert('请选择分类');
            //    return;
            //}
            if (tempImg == "") {
                alert('请输入模板图片');
                return;
            }
            saveTemplate(tempName, tempImg, tempCate);
        });

        $(document).on('click', '.dialogTemplateDiv .cancel', function () {
            layer.close(vm.layerSaveTemp);
        });
        $(document).on('click', '.dialogSelectTemplateDiv .templateClose', function () {
            layer.close(vm.layerSelectTemp);
        });
        $(document).on('click', '.dialogSelectTemplateDiv .confirm', function () {
            var tempId = $(this).closest(".templateDiv").attr('data-temp-id');
            var tempName = $(this).closest(".templateDiv").find('.templateTitle').text();
            selectTemplateConfirm(tempId, tempName);
        });
        $(document).on('click', '.dialogSelectTemplateDiv .reset', function () {
            layer.close(vm.layerSelectTemp);
        });
        $(document).on('change', '.dialogSelectTemplateDiv .selectTemplateCate', function () {
            vm.template_select_set.cate = $.trim($(this).val());
            vm.template_select_set.keyword = $.trim($(this).closest(".searchDiv").find('.txtTemplateName').val());
            vm.template_select_set.page = 1;
            selectTempSearch();
        });
        $(document).on('click', '.dialogSelectTemplateDiv .btnSearchTemplate', function () {
            vm.template_select_set.keyword = $.trim($(this).closest(".searchDiv").find('.txtTemplateName').val());
            vm.template_select_set.page = 1;
            selectTempSearch();
        });
        $(document).on('click', '.dialogSelectTemplateDiv .btnSearchCateTemplate', function () {
            if (!$(this).hasClass('button-calm')) {
                $(this).prevAll('.button-calm').removeClass('button-calm');
                $(this).nextAll('.button-calm').removeClass('button-calm');
                $(this).addClass('button-calm');
                vm.template_select_set.cate = $(this).attr('data-value');
                vm.template_select_set.page = 1;
                selectTempSearch();
            }
        });
        

        $(".left-col,.right-col").on('click', '.toolbarImg', function () {
            $(this).closest('td').find('.file').click();
        });
        $(".left-col,.right-col").on('change', '.file', function () {
            var fpath = $.trim($(this).val());
            if (fpath == "") return;
            var tObj = $(this);
            var ngModelString = $.trim($(tObj).attr('data-model'));
            var txtObj = $('input[type="text"][ng-model="' + ngModelString + '"]');
            var attrKey = $(this).attr('attr-key');
            var attrIndex = $(this).attr('attr-index');
            var attrListKey = $(this).attr('attr-list-key');
            var controlId = $(this).attr('control-id');
            var controlRight = $(this).attr('control-right');
            var attrToolbarImg = $(this).attr('attr-toolbar-img');
            var imgObj = $(this).prev();
            var parentTable = $(this).closest('.data-table');
            try {
                progress();
                $.ajaxFileUpload(
                 {
                     url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                     secureuri: false,
                     fileElement: $(this),
                     dataType: 'json',
                     success: function (resp) {
                         layer.close(layerIndex);
                         if (resp.Status == 1) {
                             $(tObj).val('');
                             if (txtObj.length > 0) $(txtObj).val(resp.ExStr);
                             if (imgObj.length > 0) {
                                 $(imgObj).attr("src", resp.ExStr);
                             }
                             if (ngModelString == "") {
                                 if (controlRight == "data") {
                                     if ($(tObj).hasClass('toolbar')) {
                                         if ($(parentTable).find('.toolbarImgDiv').hasClass('ng-hide')) {
                                             $(parentTable).find('.toolbarImgDiv').removeClass('ng-hide');
                                         }
                                         if (!$(parentTable).find('.toolbarIcoDiv').hasClass('ng-hide')) {
                                             $(parentTable).find('.toolbarIcoDiv').addClass('ng-hide');
                                         }
                                         $(parentTable).find('.toolbarImg.img').attr("src", resp.ExStr);
                                     }
                                     else if ($(tObj).hasClass('slide')) {
                                         $(parentTable).find('.imgDiv').attr("src", resp.ExStr);
                                     }
                                     
                                     if (typeof (attrIndex) == 'string') attrIndex = parseInt(attrIndex);
                                     //var selectControlIndex = GetControlRowIndex(vm.cur_select_controls, controlId);
                                     var nControl;
                                     if(controlId=='foottool'){
                                         nControl = vm.only_contorls.foottool;
                                     }else{
                                         nControl = GetControlRow(vm.right_controls, controlId);
                                     }
                                     if (nControl.control == 'linehead') {
                                         nControl.data[attrIndex].type = 'img';
                                     }
                                     //var cModel = vm.cur_select_controls[selectControlIndex];
                                     if (attrListKey) {
                                         nControl[attrListKey][attrIndex][attrKey] = resp.ExStr;
                                     } else {
                                         nControl.data[attrIndex][attrKey] = resp.ExStr;
                                     }
                                     nControl.is_edit = 1;
                                     //editExData(cModel, attrIndex, attrKey);
                                     if (attrToolbarImg == '1') {
                                         if (attrListKey) {
                                             nControl[attrListKey][attrIndex].ico = '';
                                         }
                                         else {
                                             nControl.data[attrIndex].ico = '';
                                         }
                                         //cModel.data[attrIndex].ico = '';
                                         //editExData(cModel, attrIndex, 'ico');
                                     }
                                     if (resp.ExObj) {
                                         if (resp.ExObj.width) {
                                             if (attrListKey) {
                                                 nControl[attrListKey][attrIndex].width = resp.ExObj.width;
                                             }
                                             else {
                                                 nControl.data[attrIndex].width = resp.ExObj.width;
                                             }
                                             //cModel.data[attrIndex].width = resp.ExObj.width;
                                             //editExData(cModel, attrIndex, 'width');
                                         }
                                         if (resp.ExObj.height) {
                                             if (attrListKey) {
                                                 nControl[attrListKey][attrIndex].height = resp.ExObj.height;
                                             }
                                             else {
                                                 nControl.data[attrIndex].height = resp.ExObj.height;
                                             }
                                             //cModel.data[attrIndex].height = resp.ExObj.height;
                                             //editExData(cModel, attrIndex, 'height');
                                         }
                                     }
                                     //baseReload(1);
                                 }
                                 else {
                                     var nControl = GetControlRow(vm.right_controls, controlId);
                                     if (nControl) {
                                         nControl.config[attrKey] = resp.ExStr;
                                     } else {
                                         vm.only_contorls[controlId].config[attrKey] = resp.ExStr;
                                     }
                                     //var selectControlIndex = GetControlRowIndex(vm.cur_select_controls, controlId);
                                     //var controlIndex = GetControlRowIndex(vm.right_controls, controlId);
                                     //$parse('vm.cur_select_controls[' + selectControlIndex + '].config.' + attrKey).assign($scope, resp.ExStr);
                                     //$parse('vm.right_controls[' + controlIndex + '].config.' + attrKey).assign($scope, resp.ExStr);
                                     //baseReload(1);
                                 }
                             } else {
                                 $parse(ngModelString).assign($scope, resp.ExStr);
                             }
                             $scope.$apply();
                         }
                         else {
                             alert(resp.Msg);
                         }
                     }
                 }
                );

            } catch (e) {
                alert(e);
            }
        });
        $(".js-config-region").on('click', '.app-field', function () {
            if ($(this).hasClass("editing")) return;

            $(".editing").removeClass("editing");
            $(this).addClass("editing");
        });
        $(".ui-sortable").sortable({
            axis: 'y',
            delay: 300,
            forcePlaceholderSize: true,
            opacity: 0.6,
            scroll: false,//关闭滚动事件
            placeholder: 'ui-state-highlight',
            update: function (event, ui) {
                var tempControls = [];
                $('.ui-sortable [control-id]').each(function () {
                    var cid = $(this).attr('control-id');
                    var nc = $.grep(vm.right_controls, function (cur, i) {
                        return cur['control_id'] == cid;
                    });
                    tempControls.push(nc[0]);
                });
                vm.right_controls = [];

                $scope.$apply(function () {
                    vm.right_controls = tempControls;
                });
            }
        });
    }

    function GetControlRowIndexByKey(controls, key, value) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i][key] == value) return i;
        }
        return -1;
    }
    function GetControlRowByKey(controls, key, value) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i][key] == value) return controls[i];
        }
        return null;
    }
    function GetControlRowIndex(controls, controlId) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i].control_id == controlId) return i;
        }
        return -1;
    }
    function GetControlRow(controls, controlId) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i].control_id == controlId) return controls[i];
        }
        return null;
    }
    
    function SetControlRowEidt(controls, controlId) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i].control_id == controlId) {
                controls[i].is_edit = 1;
            }
        }
    }
    function SetControlRowConfig(controls, controlId, key, value) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i].control_id == controlId) {
                if (key == 'config') {
                    controls[i].config = value;
                }
                else {
                    controls[i].config[key] = value;
                }
            }
        }
    }
    function SetControlRowData(controls, controlId, key, value) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i].control_id == controlId) {
                for (var j = 0; j < controls[i].data.length; j++) {
                    controls[i].data[j][key] = value;
                }
            }
        }
    }
    function SetControlRowIndexData(controls, controlId, cIndex, key, value) {
        for (var i = 0; i < controls.length; i++) {
            if (controls[i].control_id == controlId) {
                controls[i].is_edit = 1;
                controls[i].data[cIndex][key] = value;
            }
        }
    }

    function SetControlIndexData(control, cIndex, key, value) {
        control.is_edit = 1;
        control.data[cIndex][key] = value;
    }
    //加载模板列表
    function LoadSelectModelData() {
        progress();
        $.ajax({
            type: 'post',
            url: vm.handlerUrl + "/model/list.ashx",
            data: { rows: 2000, page: 1 },
            dataType: "json",
            success: function (resp) {
                layer.close(layerIndex);
                if (resp.status) {
                    if (resp.result.list.length == 0) {
                        alert("请先录入页面模板");
                    }
                    $scope.$apply(function () {
                        vm.component_model_list = resp.result.list;
                    });
                }
                else {
                    alert(resp.msg);
                }
            }
        });
    }
    function LoadDefModelData() {
        vm.component.ComponentModelId = vm.load_def_model_id;
        progress();
        $.ajax({
            type: 'post',
            url: vm.handlerUrl + "/model/get.ashx",
            data: { component_model_id: vm.component.ComponentModelId },
            dataType: "json",
            success: function (resp) {
                layer.close(layerIndex);
                if (resp.status) {
                    $scope.$apply(function () {
                        vm.component_model = resp.result;
                        vm.component_model.component_model_fields = resp.result.component_model_fields;
                    });
                }
                else {
                    alert(resp.msg);
                }
            }
        });
    }

    //判断组件是否支持
    function checkControlRel(prop) {
        var relKeys = Object.keys(vm.control_rels);
        for (var i = 0; i < relKeys.length; i++) {
            if (prop.indexOf(relKeys[i]) == 0) return true;
        }
        return false;
    }
    function setMallConfigToPage() {
        if ($.trim(vm.only_contorls.pageinfo.title) == '') {
            vm.only_contorls.pageinfo.title = $.trim(vm.component.ComponentName);
        }
        if ($.trim(vm.only_contorls.shareinfo.title) == '') {
            vm.only_contorls.shareinfo.title = $.trim(vm.mallConfig.mall_name);
        }
        if ($.trim(vm.only_contorls.shareinfo.desc) == '') {
            vm.only_contorls.shareinfo.desc = $.trim(vm.mallConfig.mall_desc);
        }
        if ($.trim(vm.only_contorls.shareinfo.img_url) == '') {
            vm.only_contorls.shareinfo.img_url = $.trim(vm.mallConfig.mall_logo);
        }
    }
    //加载页面时获取组件配置
    function getRightControls() {
        var tempProps = Object.keys(vm.component.ComponentConfig);
        for (var i = 0; i < tempProps.length; i++) {
            if (tempProps[i] == 'pageinfo') {
                vm.only_contorls.pageinfo.title = $.trim(vm.component.ComponentConfig.pageinfo.title);
                vm.only_contorls.pageinfo.bg_img = $.trim(vm.component.ComponentConfig.pageinfo.bg_img);
                if ($.trim(vm.only_contorls.pageinfo.bg_img) == '') {
                    $('.pgBgImg').removeAttr('src');
                }
                vm.only_contorls.pageinfo.bg_color = $.trim(vm.component.ComponentConfig.pageinfo.bg_color);
                vm.only_contorls.pageinfo.bg_img_style = $.trim(vm.component.ComponentConfig.pageinfo.bg_img_style);
                continue;
            }
            else if (tempProps[i] == 'shareinfo') {
                vm.only_contorls.shareinfo.img_url = $.trim(vm.component.ComponentConfig.shareinfo.img_url);
                vm.only_contorls.shareinfo.title = $.trim(vm.component.ComponentConfig.shareinfo.title);
                vm.only_contorls.shareinfo.desc = $.trim(vm.component.ComponentConfig.shareinfo.desc);
                vm.only_contorls.shareinfo.link = $.trim(vm.component.ComponentConfig.shareinfo.link);
                if ($.trim(vm.only_contorls.shareinfo.img_url) == '') {
                    $('.shareImg').removeAttr('src');
                }
                continue;
            }
            else if (tempProps[i] == 'foottool_home_show') {
                vm.only_value_controls.foottool_home_show = $.trim(vm.component.ComponentConfig.foottool_home_show);
                continue;
            }
            var nProp = tempProps[i];
            if (!checkControlRel(nProp)) continue;
            var nConfigs = vm.component.ComponentConfig[nProp];
            if (!nConfigs) continue;
            var nControls = $.grep(vm.component_model.component_model_fields, function (cur, i) { return nProp.indexOf(cur['component_field']) == 0; });
            if (nControls.length == 0) continue;
            var nControl = nControls[0];
            var nt = typeof (nConfigs);
            if (nt == 'object' && nConfigs.length && nControl.component_field != 'foottool_list' && nControl.component_field != 'foottool_list' && nControl.component_field.indexOf('tab_list') < 0) {
                for (var z = 0; z < nConfigs.length; z++) {
                    var nConfig = pareObjectPropsToString(nConfigs[z]);
                    addControl(nControl, nConfig);
                }
            }
            else {
                var nConfig = pareObjectPropsToString(nConfigs);
                addControl(nControl, nConfig);
            }
            //console.log(vm.right_controls);
        }
    }
    function pareObjectPropsToString(nConfigs) {
        if (typeof (nConfigs) == 'string') return $.trim(nConfigs);
        if (typeof (nConfigs) == 'number') return nConfigs.toString();
        if (typeof (nConfigs) == 'object' && nConfigs.length) {
            return nConfigs;
        }
        if (typeof (nConfigs) == 'object' && !nConfigs.length) {
            var tempProps = Object.keys(nConfigs);
            var tempObject = {};
            for (var i = 0; i < tempProps.length; i++) {
                if (nConfigs[tempProps[i]] === null) continue;
                tempObject[tempProps[i]] = pareObjectPropsToString(nConfigs[tempProps[i]]);
            }
            return tempObject;
        }
        return nConfigs;
    }
    //清空当前组件
    function clearRightAllControls() {
        vm.component.ComponentModelId = vm.component_model.component_model_id;
        vm.right_controls = [];
    }
    function clearSelects() {
        if (vm.only_contorls.foottool) vm.only_contorls.foottool.selected = false;
        if (vm.only_contorls.headerbar) vm.only_contorls.headerbar.selected = false;
        if (vm.only_contorls.totop) vm.only_contorls.totop.selected = false;
        for (var i = 0; i < vm.right_controls.length; i++) {
            vm.right_controls[i].selected = false;
        }
    }
    //滚动到目标位置
    function scrollBodyTo(num) {
        var ntp = $('.app-inner').scrollTop();
        var abs = Math.abs(ntp - num);
        if (abs > 1000) {
            $('.app-inner').stop().animate({ scrollTop: num }, 1000);
        }
        else if (abs < 100) {
            $('.app-inner').stop().animate({ scrollTop: num }, 100);
        }
        else {
            $('.app-inner').stop().animate({ scrollTop: num }, abs);
        }
    }
    //选中组件
    function selectRightControl(cModel) {
        //sModel 1为页面设置；2为高级选项
        //console.log(cModel);
        if (cModel == 1) {
            vm.show_page_set = true;
            vm.show_more_option = false;
            //vm.rightPaddingTop = 95;
            //scrollBodyTo(10);
            clearSelects();
            vm.cur_select_controls = [];
            //setRightExScrollTop(0);
            $timeout(function () {
                bindColor();
            });
            return;
        }
        else {
            vm.show_page_set = false;
        }
        if (cModel == 2) {
            vm.show_more_option = true;
            vm.show_page_set = false;
            //vm.rightPaddingTop = 95;
            //scrollBodyTo(10);
            clearSelects();
            vm.cur_select_controls = [];
            //setRightExScrollTop(0);
            return;
        }
        else {
            vm.show_more_option = false;
        }

        if (cModel.control == 'content') {
            clearSelects();
            cModel.selected = true;
            vm.cur_select_controls = [];
            vm.editor.html(cModel.config.thtml);
            vm.kindeditorControl = cModel;
            //setRightExScrollTop(0);
            setRightPaddingTop(cModel);
            return;
        }
        else {
            vm.kindeditorControl = null
        }

        if (cModel.selected) return;
        clearSelects();
        cModel.selected = true;
        //if (cModel.control == 'headerbar') vm.only_contorls.headerbar.selected = true;
        //if (cModel.control == 'foottool') vm.only_contorls.foottool.selected = true;
        //if (cModel.control == 'totop') vm.only_contorls.totop.selected = true;
        //cModel = angular.copy(cModel);
        //console.log(cModel);
        vm.cur_select_controls = [];
        pushControl(vm.cur_select_controls, cModel);
        if ((cModel.control == 'search' || cModel.control == 'headerbar' || cModel.control == 'headsearch') && cModel.config.sidemenu_button == '1') {
            pushControl(vm.cur_select_controls, vm.only_contorls.sidemenubox);
        }
        //setRightExScrollTop(0);
        if (vm.cur_select_controls.length > 0) setRightPaddingTop(vm.cur_select_controls[0]);
        //baseReload(2);
        $timeout(function () {
            bindColor();
        },100);
    }
    function deleteControl(cModel) {
        //询问框
        if (window.confirm('确认删除?')) {
            deleteControlEvent(cModel);
        };
    }
    function deleteControlEvent(cModel) {
        //console.log(cModel);
        //console.log(vm.cur_select_controls);
        clearCurSelectControls(cModel.control_id);
        //if (vm.cur_select_controls.length > 0) setRightPaddingTop(vm.cur_select_controls[0]);
        if (cModel.control == 'foottool') {
            vm.only_contorls.foottool = null;
            setComponentFieldDisabledFalse('foottool_list');
        }
        else if (cModel.control == 'headerbar') {
            vm.only_contorls.headerbar = null;
            setComponentFieldDisabledFalse('head_bar');
            setComponentFieldDisabledFalse('sidemenubox');
        }
        else if (cModel.control == 'headsearch') {
            setComponentFieldDisabledFalse('headsearch');
        }
        else if (cModel.control == 'sidemenubox') {
            vm.only_contorls.sidemenubox = null;
            setComponentFieldDisabledFalse('sidemenubox');
        }
        else if (cModel.control == 'totop') {
            vm.only_contorls.totop = null;
            setComponentFieldDisabledFalse('totop');
        }
        else {
            if (cModel.control == 'userinfo') {
                setComponentFieldDisabledFalse('userinfo');
                clearRightControls(cModel.control_id);
            }
            else if (cModel.control == 'search') {
                setComponentFieldDisabledFalse('searchbox');
                setComponentFieldDisabledFalse('sidemenubox');
                clearRightControls(cModel.control_id);
            }
            else if (cModel.control == 'linehead') {
                setComponentFieldDisabledFalse('linehead');
                clearRightControls(cModel.control_id);
            }
            else if (cModel.control == 'content') {
                setComponentFieldDisabledFalse('content');
                vm.kindeditorControl = null;
                clearRightControls(cModel.control_id);
            }
            else {
                clearRightControls(cModel.control_id);
            }
        }
    }

    //设置右边选中控件属性的位置
    function setRightPaddingTop(cModel) {
        //$(window.top.tabpanel.getActiveTab().content).find('iframe')
        $timeout(function () {
            $scope.$apply(function () {
                vm.rightPaddingTop = $('.main-col [control-id="' + cModel.control_id + '"]').offset().top + $('.app-inner').scrollTop();
                scrollBodyTo(vm.rightPaddingTop - 50);
            });
        });
    }
    //设置右边选中控件属性的位置
    function setRightExScrollTop(num) {
        //$(window.top.tabpanel.getActiveTab().content).find('iframe')
        $timeout(function () {
            $scope.$apply(function () {
                $('.right-inner').scrollTop(num);
            });
        });
    }
    //如果删除选中 清空当前选择
    function clearCurSelectControls(control_id) {
        if (vm.cur_select_controls.length == 0) return;
        var nc = $.grep(vm.cur_select_controls, function (cur, i) { return cur['control_id'] == control_id; });
        if (nc.length > 0) vm.cur_select_controls = [];
    }
    //清除右边 组件
    function clearRightControls(control_id) {
        if (vm.right_controls.length == 0) return;
        for (var i = vm.right_controls.length - 1; i >= 0; i--) {
            if (control_id == vm.right_controls[i].control_id) {
                vm.right_controls.splice(i, 1);
                break;
            }
        }
    }
    //清空禁止添加
    function clearComponentFieldDisabledFalse() {
        $.each(vm.component_model.component_model_fields, function () {
            this.disabled = false;
        })
    }
    //取消禁止添加
    function setComponentFieldDisabledFalse(component_field) {
        $.each(vm.component_model.component_model_fields, function () {
            if (this.component_field == component_field) this.disabled = false;
        })
    }
    //添加组件
    function addControl(nControl, nConfig, autoSelect) {
        if (nControl.disabled) return;
        var control = vm.control_rels[nControl.component_field];
        if (!control) {
            alert('组件[' + nControl.component_field_name + ']不支持');
            return;
        }
        var nlimit = vm.limit[control];
        if (nlimit && getContorlNum(control) >= nlimit) {
            alert('组件[' + nControl.component_field_name + ']限制添加' + nlimit + '个');
            return;
        }
        var cModel = {
            control_id: getControlID(control, nControl.component_field),
            control: control,
            control_name: nControl.component_field_name,
            type: nControl.component_field_type,
            selected: false
        }
        if (nConfig) {
            setControlConfig(cModel, nConfig);
        }
        else {
            setControlDefConfig(cModel);
        }
        cModel = addAutoField(cModel, nControl);
        if (['tabs', 'foottool', 'slides', 'navs','headsearch'].indexOf(cModel.control) >= 0) {
            cModel.is_edit = 0;
        }
        //console.log(cModel);
        if (nlimit) {
            nControl.disabled = true;
        }
        if (cModel.control == 'foottool') {
            vm.only_contorls.foottool = cModel;
        }
        else if (cModel.control == 'headerbar') {
            vm.only_contorls.headerbar = cModel;
        }
        else if (cModel.control == 'sidemenubox') {
            vm.only_contorls.sidemenubox = cModel;
        }
        else if (cModel.control == 'totop') {
            vm.only_contorls.totop = cModel;
        }
        else {
            var startIndex = getAddStartIndex();
            vm.right_controls.splice(startIndex, 0, cModel);
        }
        if ($.inArray(cModel.control, vm.control_data_types) >= 0) {
            setControlData(cModel, autoSelect);
        }
        else {
            if (autoSelect) selectRightControl(cModel);
        }
    }
    function pushControl(controls, cModel) {
        if (!cModel) return;
        var tempModel = cModel;
        //var tempModel = angular.copy(cModel);
        var oldControl = $.grep(controls, function (cur, i) { return cur['control_id'] == tempModel.control_id; });
        if (oldControl.length > 0) return;
        controls.push(tempModel);
    }
    function spliceControl(controls, cModel) {
        if (!cModel) return;
        for (var i = controls.length - 1; i >= 0; i--) {
            if (controls[i].control_id == cModel.control_id) {
                controls.splice(i, 1);
            }
        }
    }
    //添加侧边栏
    function addSidemenuControl(cModel) {
        //console.log(cModel);
        //设置对应值
        //SetControlRowConfig(vm.cur_select_controls, cModel.control_id, 'sidemenu_button', cModel.config.sidemenu_button);
        //if (cModel.control == 'headerbar') {
        //    vm.only_contorls.headerbar.config.sidemenu_button = cModel.config.sidemenu_button;
        //} else {
        //    SetControlRowConfig(vm.right_controls, cModel.control_id, 'sidemenu_button', cModel.config.sidemenu_button);
        //    //baseReload(1);
        //}

        var sidemenuboxField = $.grep(vm.component_model.component_model_fields, function (cur, i) { return cur['component_field'] == 'sidemenubox'; });
        if (sidemenuboxField.length == 0) return;
        if (cModel.config.sidemenu_button == '0') {
            spliceControl(vm.cur_select_controls, vm.only_contorls.sidemenubox)
            if (vm.only_contorls.sidemenubox) vm.only_contorls.sidemenubox.disabled = true;
            setComponentFieldDisabledFalse('sidemenubox');
        }
        else {
            if (vm.only_contorls.sidemenubox) {
                vm.only_contorls.sidemenubox.disabled = false;
                pushControl(vm.cur_select_controls, vm.only_contorls.sidemenubox);
                return;
            }

            vm.only_contorls.sidemenubox = {
                control_id: 'sidemenubox',
                control: 'sidemenubox',
                control_name: sidemenuboxField[0].component_field_name,
                type: sidemenuboxField[0].component_field_type,
                selected: false
            }
            setControlDefConfig(vm.only_contorls.sidemenubox);
            vm.only_contorls.sidemenubox = addAutoField(vm.only_contorls.sidemenubox, sidemenuboxField[0]);
            pushControl(vm.cur_select_controls, vm.only_contorls.sidemenubox);

            $timeout(function () {
                bindColor();
            }, 100);
        }
        //baseReload(2);
    }
    //获取添加的组件ID
    function getAddStartIndex() {
        if (vm.cur_select_controls.length > 0) {
            for (var i = 0; i < vm.right_controls.length; i++) {
                if (vm.right_controls[i].control_id == vm.cur_select_controls[vm.cur_select_controls.length - 1].control_id) {
                    return i + 1;
                }
            }
            return vm.right_controls.length;
        }
        else {
            return vm.right_controls.length;
        }
    }
    //设置组件数据
    function setControlData(oModel, autoSelect) {
        //var cModel = angular.copy(oModel);
        var cModel =oModel;
        var tempKey;
        if (cModel.control == 'slides') {
            if (typeof (cModel.config.slide_list) == 'string') {
                tempKey = cModel.config.slide_list;
                setSlideRightControlData(tempKey, cModel, autoSelect);
            }
            else {
                var idSp = cModel.control_id.split('_');
                var slideKey = vm.component.ComponentName + '_' + '幻灯片';
                if (cModel.config.slide_list && cModel.config.slide_list.length > 0 && cModel.config.slide_list[0].type) {
                    slideKey = cModel.config.slide_list[0].type;
                }
                else {
                    if (idSp.length > 1) slideKey = slideKey + idSp[idSp.length - 1];
                }
                if ($.inArray(slideKey, vm.slides) < 0) {
                    vm.slides.push(slideKey);
                    vm.slide_data.push({ key: slideKey, data: cModel.config.slide_list });
                }
                cModel.is_edit = 1;
                cModel.data = cModel.config.slide_list;
                cModel.config.slide_list = slideKey;
                //vm.template_data.slide_temp_data.push({ key: slideKey, data: cModel.config.slide_list });
                //setSlideRightControlData(slideKey, cModel, autoSelect);
            }
        }
        else if (cModel.control == 'malls') {
            tempKey = $.trim(cModel.config.is_group_buy) + '_' + $.trim(cModel.config.cate) + '_' + $.trim(cModel.config.tag) + '_' + '_' + $.trim(cModel.config.count) + '_' + $.trim(cModel.config.sort) + '_' + $.trim(cModel.config.sort_tag);
            setMallRightControlData(tempKey, cModel, autoSelect);
        }
        else if (cModel.control == 'goods') {
            tempKey = $.trim(cModel.config.ids);
            setGoodRightControlData(tempKey, cModel, autoSelect);
        }
        else if (cModel.control == 'cardlist') {
            tempKey = $.trim(cModel.config.cate_id) + '_' + $.trim(cModel.config.rows) + '_' + $.trim(cModel.config.data_type);
            setArticleRightControlData(tempKey, cModel, autoSelect);
        }
        else if (cModel.control == 'activitylist') {
            tempKey = $.trim(cModel.config.cate_id) + '_' + $.trim(cModel.config.rows) + '_' + $.trim(cModel.config.sort) + '_' + $.trim(cModel.config.data_type);
            setActivityRightControlData(tempKey, cModel, autoSelect);
        }
        else if (cModel.control == 'foottool' || cModel.control == 'tabs') {
            if (!cModel.config.length) {
                tempKey = cModel.config.key_type;
                setToolbarRightControlData(tempKey, cModel, autoSelect);
            }
            else {
                var idSp = cModel.control_id.split('_');
                var toolbarKey = vm.component.ComponentName + '_' +( cModel.control == 'foottool' ? '底部导航' : '选项卡导航');
                if (cModel.config.length > 0 && cModel.config[0].key_type) {
                    toolbarKey = cModel.config[0].key_type;
                }
                else {
                    if (idSp.length > 2) toolbarKey = toolbarKey + idSp[idSp.length - 1];
                }
                var nl = $.grep(vm.toolbars, function (cur, i) {
                    return cur['key_type'] == toolbarKey;
                });
                if (nl.length <= 0) {
                    var data = angular.fromJson(angular.toJson(cModel.config));
                    vm.toolbars.push({ key_type: toolbarKey, use_type: 'nav' });
                    vm.toolbar_data.push({ key: toolbarKey, data: data });
                }

                cModel.is_edit = 1;
                cModel.data = data;
                cModel.config.key_type = toolbarKey;
                //vm.template_data.toolbar_temp_data.push({ key: toolbarKey, data: cModel.config });
                //setToolbarRightControlData(toolbarKey, cModel, autoSelect);
            }
        }
        else if (cModel.control == 'navs') {
            if (typeof (cModel.config.nav_list) == 'string') {
                tempKey = cModel.config.nav_list;
                setToolbarRightControlData(tempKey, cModel, autoSelect);
            }
            else {
                var idSp = cModel.control_id.split('_');
                var toolbarKey = vm.component.ComponentName + '_' + '导航列表';
                if (cModel.config.nav_list.length > 0 && cModel.config.nav_list[0].key_type) {
                    toolbarKey = cModel.config.nav_list[0].key_type;
                }
                else {
                    
                    if (idSp.length > 1) toolbarKey = toolbarKey + idSp[idSp.length - 1];
                }

                var nl = $.grep(vm.toolbars, function (cur, i) {
                    return cur['key_type'] == toolbarKey;
                });
                if (nl.length <= 0) {
                    vm.toolbars.push({ key_type: toolbarKey, use_type: 'nav' });
                    vm.toolbar_data.push({ key: toolbarKey, data: cModel.config.nav_list });
                }
                cModel.is_edit = 1;
                cModel.data = cModel.config.nav_list;
                cModel.config.nav_list = toolbarKey;
                //vm.template_data.toolbar_temp_data.push({ key: toolbarKey, data: cModel.config.nav_list });
                //setToolbarRightControlData(toolbarKey, cModel, autoSelect);
            }
        }
        else if (cModel.control == 'headsearch') {
            if (cModel.config.nav_left_type == 1 && cModel.config.nav_left) {
                tempKey = cModel.config.nav_left;
                setToolbarHeadSearchData(tempKey, "nav_left", cModel, autoSelect);
            }
            if (cModel.config.nav_right) {
                tempKey = cModel.config.nav_right;
                setToolbarHeadSearchData(tempKey, "nav_right", cModel, autoSelect);
            }
        }
        else {
            reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        //console.log(cModel);
    }
    //编辑后设置Data
    function eidtSetControlData(oModel,atttKey) {
        var cModel = oModel;
        var tempKey;
        if (cModel.control == 'slides') {
            tempKey = cModel.config.slide_list;
            setSlideRightControlData(tempKey, cModel);
            //var nControl = GetControlRow(vm.right_controls, cModel.control_id);
            //setSlideRightControlData(tempKey, nControl);
        }
        else if (cModel.control == 'malls') {
            tempKey = $.trim(cModel.config.is_group_buy) + '_' + $.trim(cModel.config.cate) + '_' + $.trim(cModel.config.tag) + '_' + $.trim(cModel.config.count) + '_' + $.trim(cModel.config.sort) + '_' + $.trim(cModel.config.sort_tag);
            setMallRightControlData(tempKey, cModel);
        }
        else if (cModel.control == 'goods') {
            tempKey = $.trim(cModel.config.ids);
            //setGoodRightControlData(tempKey, cModel);
        }
        else if (cModel.control == 'cardlist') {
            tempKey = $.trim(cModel.config.cate_id) + '_' + $.trim(cModel.config.rows) + '_' + $.trim(cModel.config.data_type);
            setArticleRightControlData(tempKey, cModel);
        }
        else if (cModel.control == 'activitylist') {
            tempKey = $.trim(cModel.config.cate_id) + '_' + $.trim(cModel.config.rows) + '_' + $.trim(cModel.config.sort) + '_' + $.trim(cModel.config.data_type);
            setActivityRightControlData(tempKey, cModel);
        }
        else if (cModel.control == 'foottool' || cModel.control == 'tabs') {
            tempKey = cModel.config.key_type;
            setToolbarRightControlData(tempKey, cModel);
        }
        else if (cModel.control == 'navs') {
            tempKey = cModel.config.nav_list;
            setToolbarRightControlData(tempKey, cModel);
        }
        else if (cModel.control == 'headsearch') {
            tempKey = cModel.config[atttKey];
            setToolbarHeadSearchData(tempKey, atttKey, cModel);
        }
        else if (cModel.control == 'block') {
            $timeout(function () {
                $scope.$apply(function () {
                    if (cModel.config.type == 1) {
                        cModel.config.max_rate = 10;
                    } else {
                        cModel.attrs[1].slider = '{"min":1,"max":100,"step":1,"value":10}';
                    }
                });
            })
            //console.log(cModel);
        }
        else {
            //reloadControl(cModel);
        }
        //console.log(cModel);
    }
    //获取文章数据
    function setArticleRightControlData(key, cModel, autoSelect) {
        key = $.trim(key);
        var tempData = getKeyData(vm.art_data, key);
        if (tempData != null) {
            cModel.config.card_data = tempData;
            //reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        else {
            var model = {
                action: "getArticleList",
                type: "Article",
                cateId: $.trim(cModel.config.cate_id),
                pageIndex: 1,
                pageSize: $.trim(cModel.config.rows) == '' ? 10 : cModel.config.rows,
                hasStatistics: 0,
                hasAuthor: 0,
                data_type: $.trim(cModel.config.data_type),
                column: 'JuActivityID,ActivityName,Summary,CreateDate,CategoryId,ThumbnailsPath,PV,Tags,RedirectUrl'
            };
            commService.postData(
                baseDomain + "serv/pubapi.ashx",
                model,
                function (data) {
                    if (data.list) {
                        if (vm.template_data && data.list.length == 0) {
                            var nTemp = $.grep(vm.template_data.art_temp_data, function (cur, i) {
                                return cur['key'] == key;
                            });
                            if (nTemp.length > 0) {
                                vm.art_data.push(nTemp[0]);
                                cModel.config.card_data = nTemp[0].data;
                                reloadControl(cModel);
                            } else {
                                cModel.config.card_data = [];
                            }
                        }
                        else {
                            var keyData = { key: key, data: data };
                            vm.art_data.push(keyData);
                            cModel.config.card_data = data;
                            //reloadControl(cModel);
                        }
                    }
                    if (autoSelect) selectRightControl(cModel);
                }, function (data) { });
        }
    }
    //获取活动数据
    function setActivityRightControlData(key, cModel, autoSelect) {
        key = $.trim(key);
        var tempData = getKeyData(vm.act_data, key);
        if (tempData != null) {
            cModel.config.activity_list = tempData;
            //reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        else {
            var model = {
                category_id: $.trim(cModel.config.cate_id),
                pageindex: 1,
                column: "JuActivityID,ActivityName,ActivityStartDate,ActivityEndDate,CreateDate,ActivityAddress,CategoryId,ThumbnailsPath,PV,SignUpCount,Tags,RedirectUrl,ActivityIntegral,IsHide,SignUpActivityID,MaxSignUpTotalCount",
                pagesize: $.trim(cModel.config.rows) == '' ? 6 : cModel.config.rows,
                is_forward: $.trim(cModel.config.data_type)
            };
            if (cModel.config.sort) model.activity_sort = cModel.config.sort;
            commService.postData(
                baseDomain + "serv/api/activity/list.ashx",
                model,
                function (data) {
                    if (data.list) {
                        if (vm.template_data && data.list.length == 0) {
                            var nTemp = $.grep(vm.template_data.act_temp_data, function (cur, i) {
                                return cur['key'] == key;
                            });
                            if (nTemp.length > 0) {
                                vm.act_data.push(nTemp[0]);
                                cModel.config.activity_list = nTemp[0].data;
                                //reloadControl(cModel);
                            } else {
                                cModel.config.activity_list = [];
                            }
                        }
                        else {
                            var keyData = { key: key, data: data.list };
                            vm.act_data.push(keyData);
                            cModel.config.activity_list = data.list;
                            //reloadControl(cModel);
                        }
                    }
                    if (autoSelect) selectRightControl(cModel);
                }, function (data) { });
        }
    }
    //获取商品分类数据
    function setMallRightControlData(key, cModel, autoSelect) {
        key = $.trim(key);
        var tempData = getKeyData(vm.mall_data, key);
        if (tempData != null) {
            cModel.config.mall_list = tempData.list;
            cModel.config.mall_total = tempData.totalcount;
            //reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        else {
            var model = {
                action: 'list',
                pageindex: 1,
                pagesize: $.trim(cModel.config.count) == '' ? 6 : cModel.config.count
            };
            if ($.trim(cModel.config.cate) != '') model.category_id = cModel.config.cate;
            if ($.trim(cModel.config.tag) != '') model.tags = cModel.config.tag;
            if ($.trim(cModel.config.sort) != '') model.sort = cModel.config.sort;
            if ($.trim(cModel.config.sort_tag) != '') model.sort_tag = cModel.config.sort_tag;
            var strGroupType = cModel.config.is_group_buy == 1 ? 1 : 0;
            if ($.trim(cModel.config.is_group_buy) != '') model.is_group_buy = strGroupType;
            if (cModel.config.is_group_buy == 2) {
                model.type = 'Course';
            } else if(cModel.config.is_group_buy==4){
                model.type = 'Houses';
            } else {
                model.type = 'Mall';
            }

            if (cModel.config.is_group_buy == 3) {
                model.has_cate_name = "";
                model.group_buy_type = 1;
                model.is_group_buy = "";
                model.order_type = 2;
                model.group_buy_status = "";
                model.is_disable_curr_user = "1";
                model.group_buy_status = "0";
                commService.postData(
                baseDomain + "serv/api/mall/order.ashx",
                model,
                function (data) {
                    if (data.list) {
                        if (vm.template_data && data.list.length == 0) {
                            var nTemp = $.grep(vm.template_data.mall_temp_data, function (cur, i) {
                                return cur['key'] == key;
                            });
                            if (nTemp.length > 0) {
                                vm.mall_data.push(nTemp[0]);
                                cModel.config.mall_list = nTemp[0].data.list;
                                cModel.config.mall_total = nTemp[0].data.totalcount;
                                //reloadControl(cModel);
                            } else {
                                cModel.config.mall_list = [];
                                cModel.config.mall_total = 0;
                            }
                        }
                        else {
                            var keyData = { key: key, data: data };
                            vm.mall_data.push(keyData);
                            cModel.config.mall_list = data.list;
                            cModel.config.mall_total = data.totalcount;
                            //reloadControl(cModel);
                        }
                    }
                    if (autoSelect) selectRightControl(cModel);

                }, function (data) { });

            } else {

                commService.postData(
                    baseDomain + "serv/api/mall/product.ashx",
                    model,
                    function (data) {
                        if (data.list) {
                            if (vm.template_data && data.list.length == 0) {
                                var nTemp = $.grep(vm.template_data.mall_temp_data, function (cur, i) {
                                    return cur['key'] == key;
                                });
                                if (nTemp.length > 0) {
                                    vm.mall_data.push(nTemp[0]);
                                    cModel.config.mall_list = nTemp[0].data.list;
                                    cModel.config.mall_total = nTemp[0].data.totalcount;
                                    //reloadControl(cModel);
                                } else {
                                    cModel.config.mall_list = [];
                                    cModel.config.mall_total = 0;
                                }
                            }
                            else {
                                var keyData = { key: key, data: data };
                                vm.mall_data.push(keyData);
                                cModel.config.mall_list = data.list;
                                cModel.config.mall_total = data.totalcount;
                                //reloadControl(cModel);
                            }
                        }
                        if (autoSelect) selectRightControl(cModel);
                    }, function (data) { });
            }
        }
    }
    //获取商品数据
    function setGoodRightControlData(key, cModel, autoSelect) {
        key = $.trim(key);
        var tempData = getKeyData(vm.good_data, key);
        if (tempData != null) {
            cModel.config.good_list = tempData.list;
            //reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        else {
            var ids = cModel.config.ids;
            if (!ids) {
                cModel.config.good_list = [];
                if (autoSelect) selectRightControl(cModel);
            }
            else {
                var model = {
                    action: 'list',
                    product_ids: ids,
                    sort_ids: 1,
                    can_repeat: 1
                };
                commService.postData(
                    baseDomain + "serv/api/mall/product.ashx",
                    model,
                    function (data) {
                        if (data.list) {
                            if (vm.template_data && data.list.length == 0) {
                                var nTemp = $.grep(vm.template_data.good_temp_data, function (cur, i) {
                                    return cur['key'] == key;
                                });
                                if (nTemp.length > 0) {
                                    vm.good_data.push(nTemp[0]);
                                    cModel.config.good_list = nTemp[0].data.list;
                                } else {
                                    cModel.config.good_list = [];
                                }
                            }
                            else {
                                var keyData = { key: key, data: data };
                                vm.mall_data.push(keyData);
                                cModel.config.good_list = data.list;
                                //reloadControl(cModel);
                            }
                        }
                        if (autoSelect) selectRightControl(cModel);
                    }, function (data) { });
            }
        }
    }
    //获取幻灯片数据
    function setSlideRightControlData(key, cModel, autoSelect) {
        key = $.trim(key);
        var tempData = getKeyData(vm.slide_data, key);
        if (tempData != null) {
            setRightControlData(cModel, tempData);
            //reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        else {
            commService.postData(
                baseDomain + "serv/api/common/slidelist.ashx",
                { type: key },
                function (data) {
                    if (data.status) {
                        if (vm.template_data && data.result.length == 0) {
                            var nTemp = $.grep(vm.template_data.slide_temp_data, function (cur, i) {
                                return cur['key'] == key;
                            });
                            if (nTemp.length > 0) {
                                vm.slides.push(key);
                                vm.slide_data.push(nTemp[0]);
                                cModel.is_edit = 1;
                                setRightControlData(cModel, nTemp[0].data);
                                //reloadControl(cModel);
                            }
                        }
                        else {
                            var keyData = { key: key, data: data.result };
                            vm.slide_data.push(keyData);
                            setRightControlData(cModel, keyData.data);
                            //reloadControl(cModel);
                        }
                    }
                    if (autoSelect) selectRightControl(cModel);
                }, function (data) { });
        }
    }
    //获取导航数据
    function setToolbarRightControlData(key, cModel, autoSelect) {
        key = $.trim(key);
        var tempData = getKeyData(vm.toolbar_data, key);
        if (tempData != null) {
            setRightControlData(cModel, tempData);
            //reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        else {
            commService.postData(
                baseDomain + "serv/api/common/toolbarlist.ashx",
                { key_type: key, show_all:1 },
                function (data) {
                    if (data.status) {
                        if (vm.template_data && data.result.length == 0) {
                            var nTemp = $.grep(vm.template_data.toolbar_temp_data, function (cur, i) {
                                return cur['key'] == key;
                            });
                            if (nTemp.length > 0) {
                                vm.toolbars.push({ key_type: key, use_type: 'nav' });
                                vm.toolbar_data.push(nTemp[0]);
                                cModel.is_edit = 1;
                                setRightControlData(cModel, nTemp[0].data);
                                //reloadControl(cModel);
                            }
                        }
                        else {
                            var keyData = { key: key, data: data.result };
                            vm.toolbar_data.push(keyData);
                            setRightControlData(cModel, keyData.data);
                            //reloadControl(cModel);
                        }
                    }
                    if (autoSelect) selectRightControl(cModel);
                }, function (data) { });
        }
    }
        //获取导航数据
    function setToolbarHeadSearchData(key, atttKey, cModel, autoSelect) {
        key = $.trim(key);
        var tempData = getKeyData(vm.toolbar_data, key);
        if (tempData != null) {
            var data = angular.fromJson(angular.toJson(tempData));
            var data = $.grep(data, function (cur, i) {
                return cur.ico || cur.img;
            });
            if (data.length > 3) {
                list = data.splice(0, 3);
            } else {
                list = data;
            }
            if (atttKey == "nav_left") {
                cModel.left_list = list;
            } else {
                cModel.right_list = list;
            }
            //reloadControl(cModel);
            if (autoSelect) selectRightControl(cModel);
        }
        else {
            commService.postData(
                baseDomain + "serv/api/common/toolbarlist.ashx",
                { key_type: key, show_all: 1 },
                function (data) {
                    if (data.status) {
                        if (vm.template_data && data.result.length == 0) {
                            var nTemp = $.grep(vm.template_data.toolbar_temp_data, function (cur, i) {
                                return cur['key'] == key;
                            });
                            if (nTemp.length > 0) {
                                vm.toolbars.push({ key_type: key, use_type: 'nav' });
                                vm.toolbar_data.push(nTemp[0]);
                                var data = angular.fromJson(angular.toJson(nTemp[0]));
                                $.grep(vm.template_data.toolbar_temp_data, function (cur, i) {
                                    return cur['key'] == key;
                                });
                                var data = $.grep(data, function (cur, i) {
                                    return cur.ico || cur.img;
                                });

                                if (data.length > 3) {
                                    data = data.splice(0, 3);
                                } else {
                                    data = data;
                                }
                                if (atttKey == "nav_left") {
                                    cModel.left_list = [];
                                    $timeout(function () {
                                        $scope.$apply(function () {
                                            var cControl =  vmFunc.GetControlRow(vm.right_controls, 'headsearch');
                                            cControl.left_list = data;
                                        });
                                    });
                                } else {
                                    cModel.right_list = [];
                                    $timeout(function () {
                                        $scope.$apply(function () {
                                            var cControl = vmFunc.GetControlRow(vm.right_controls, 'headsearch');
                                            cControl.right_list = data;
                                        });
                                    });
                                }
                                cModel.is_edit = 1;
                            }
                        }
                        else {
                            var keyData = { key: key, data: data.result };
                            vm.toolbar_data.push(keyData);
                            var data = angular.fromJson(angular.toJson(data.result));
                            var data = $.grep(data, function (cur, i) {
                                return cur.ico || cur.img;
                            });
                            if (data.length > 3) {
                                data = data.splice(0, 3);
                            } else {
                                data = data;
                            }
                            if (atttKey == "nav_left") {
                                cModel.left_list = [];
                                $timeout(function () {
                                    $scope.$apply(function () {
                                        var cControl = vmFunc.GetControlRow(vm.right_controls, 'headsearch');
                                        cControl.left_list = data;
                                    });
                                });
                            } else {
                                cModel.right_list = [];
                                $timeout(function () {
                                    $scope.$apply(function () {
                                        var cControl = vmFunc.GetControlRow(vm.right_controls, 'headsearch');
                                        cControl.right_list = data;
                                    });
                                });
                            }
                            cModel.is_edit = 1;
                        }
                    }
                    if (autoSelect) selectRightControl(cModel);
                }, function (data) { });
        }
    }
    //设置组件数据到呈现对象
    function setRightControlData(cModel, data) {
        cModel.data = data;
    }
    function editControlConfig(cModel) {
        //console.log($scope);
        //if (checkIsNoEditConfig(cModel)) return;
        //reloadControl(cModel);
    }
    function editControlData(cModel, attrKey) {
        //console.log(cModel.config);
        //console.log(cModel);
        //if (checkIsNoEditConfig(cModel)) return;
        //console.log(cModel);
        eidtSetControlData(cModel, attrKey);
        $timeout(function () {
            bindColor();
        },100);
    }
    function isObjectValueEqual(a, b) {
        if (typeof (a) == typeof (b) == 'string') {
            return a === b;
        }
        if (typeof (a) == 'undefined' || typeof (b) == 'undefined') {
            if (typeof (a) == typeof (b)) return true;
            return false;
        }
        if (!a || !b) {
            if (a == b) return true;
            return false;
        }
        var aProps = Object.getOwnPropertyNames(a);
        var bProps = Object.getOwnPropertyNames(b);
        if (aProps.length != bProps.length) {
            return false;
        }
        for (var i = 0; i < aProps.length; i++) {
            var propName = aProps[i];
            if (a[propName] && typeof (a[propName]) == 'object') continue;
            if (a[propName] !== b[propName]) {
                return false;
            }
        }
        return true;
    }
    function checkIsNoEditConfig(cModel) {
        var tempConfig = angular.copy(cModel.config);
        var oldConfig;
        if (cModel.control == 'foottool') {
            oldConfig = angular.copy(vm.only_contorls.foottool.config);
        }
        else if (cModel.control == 'headerbar') {
            oldConfig = angular.copy(vm.only_contorls.headerbar.config);
        }
        else if (cModel.control == 'sidemenubox') {
            oldConfig = angular.copy(vm.only_contorls.sidemenubox.config);
        }
        else {
            for (var i = 0; i < vm.right_controls.length; i++) {
                if (vm.right_controls[i].control_id == cModel.control_id) {
                    oldConfig = angular.copy(vm.right_controls[i].config);
                    break;
                }
            }
        }
        //console.log(tempConfig);
        //console.log(oldConfig);
        return isObjectValueEqual(tempConfig, oldConfig);
    }
    function checkIsNoEditControlData(cModel, attrIndex) {
        var cModel = angular.copy(cModel);
        var newData = cModel.data;
        var oldData;
        if (cModel.control == 'foottool') {
            oldData = angular.copy(vm.only_contorls.foottool.data);
        }
        else if (cModel.control == 'headerbar') {
            oldData = angular.copy(vm.only_contorls.headerbar.data);
        }
        else if (cModel.control == 'sidemenubox') {
            oldData = angular.copy(vm.only_contorls.sidemenubox.data);
        }
        else {
            for (var i = 0; i < vm.right_controls.length; i++) {
                if (vm.right_controls[i].control_id == cModel.control_id) {
                    oldData = angular.copy(vm.right_controls[i].data);
                    break;
                }
            }
        }

        if (typeof (newData) != 'object' || typeof (oldData) != typeof (newData) || newData.length != oldData.length) return false;
        if (newData.length <= attrIndex) return false;
        return (isObjectValueEqual(newData[attrIndex], oldData[attrIndex]));
    }
    //设置控件配置
    function reloadControl(cModel) {
        tempModel = cModel;
        //var tempModel = angular.copy(cModel);
        var or =['totop','sidemenubox']
        if ($.inArray(cModel.control, or) >= 0) {
            return;
        }
        else if (tempModel.control == 'foottool') {
            vm.only_contorls.foottool = tempModel;
            if (tempModel.is_edit == 1) vm.only_contorls.foottool.is_edit = 1;
            baseReloadOnly('foottool');
        }
        else if (tempModel.control == 'headerbar') {
            vm.only_contorls.headerbar = tempModel;
            baseReloadOnly('headerbar');
        }
        else if (tempModel.control == 'totop') {
            vm.only_contorls.totop = tempModel;
            baseReloadOnly('sidemenubox');
        }
        else {
            var controlIndex = GetControlRowIndex(vm.right_controls, cModel.control_id);
            if (controlIndex >= 0) {
                var tempConfig = angular.copy(tempModel.config);
                vm.right_controls[controlIndex].config = null;
                if (tempModel.is_edit == 1) vm.right_controls[controlIndex].is_edit = 1;
                $timeout(function () {
                    $scope.$apply(function () {
                        vm.right_controls[controlIndex].config = tempConfig;
                    });
                });
            }
            //var selectControlIndex = GetControlRowIndex(vm.cur_select_controls, cModel.control_id);
            //if (selectControlIndex >= 0 && vm.cur_select_controls[selectControlIndex].data) {
            //    vm.cur_select_controls[selectControlIndex].data = tempModel.data;
            //}
        }
    }
    //检查数组中key的数据是否存在
    function getKeyData(list, key) {
        for (var i = 0; i < list.length; i++) {
            if (list[i].key == key) return list[i].data;
        }
        return null;
    }
    //设置组件配置
    function setControlConfig(cModel, fconfig) {
        if (typeof(fconfig) =='string' && (cModel.control == 'foottool' || cModel.control == 'tabs')){
            cModel.config = { key_type: fconfig };
        }else{
            cModel.config = fconfig;
        }
    }
    //设置组件默认配置
    function setControlDefConfig(cModel) {
        if (cModel.control == 'navs' && vm.toolbars.length > 0) {
            cModel.config = { nav_list: vm.toolbars[0].key_type, col_count: '4', style: '1', border_show: '0', radius: '1' };
        }
        else if (cModel.control == 'malls') {
            cModel.config = { count: '6', style: '1', is_group_buy: '0', sort: 'def', auto_load: '0', title_show: '0' };
        }
        else if (cModel.control == 'cardlist') {
            cModel.config = { cate_id: '', style: '2', image_show: '1', image_align: '0', image_shape: '0', detail_href: '{article_id_16}/details.chtml', type_show: '1', describe_show: '1', pv_show: '1', tag_show: '1', createdate_show: '1', title_word_break: '0', summary_word_break: '0', auto_load: '0', rows: '10', data_type:'0' };
        }
        else if (cModel.control == 'activitylist') {
            cModel.config = { cate_id: '', rows: '10', sort: '999' };
        }
        else if (cModel.control == 'slides' && vm.slides.length > 0) {
            cModel.config = { slide_list: vm.slides[0], show_type: '1', size_height: 0, size_width: 0 };
        }
        else if (cModel.control == 'headerbar') {
            cModel.config = { show: 1, title: '头部标题', left_btn_ico: 'iconfont icon-fanhui', left_btn: 'javascript:history.go(-1);', right_btn: '/customize/comeoncloud/Index.aspx?key=MallHome', right_btn_ico: 'iconfont icon-shouye' };
        }
        else if (cModel.control == 'userinfo') {
            cModel.config = { show: 1, style: '1', bgcolor: '#38A9FF', btn_text: '账号管理', btn_url: '/customize/shop/?v=1.0#/userMange', border_color: '#FFFFFF' };
        }
        else if (cModel.control == 'search') {
            cModel.config = { show: 1, color: '#11c1f3', style: '2', type: '1', target: '1', sidemenu_button: '0' };
        }
        else if (cModel.control == 'sidemenubox') {
            cModel.config = { show: 0, type: '1', data_key: '', head_bgcolor: '#6BD3FF', item_bgcolor: '#F563FF' };
        }
        else if (cModel.control == 'foottool' && vm.toolbars.length > 0) {
            cModel.config = { key_type: vm.toolbars[0].key_type };
        }
        else if (cModel.control == 'tabs' && vm.toolbars.length > 0) {
            cModel.config = {key_type: vm.toolbars[0].key_type};
        }
        else if (cModel.control == 'linehead') {
            cModel.config = {
                text: '店铺名称', bg_img: 'http://open-files.comeoncloud.net/www/xikaiye/jubit/image/20160819/32C89F314F924D0CB4CB659421145A94.jpg', logo: 'http://open-files.comeoncloud.net/www/xikaiye/jubit/image/20160819/ED5BB6A058E845ACB266414C79257861.jpg', item_bgcolor: '#F563FF',
                data: [{ color: 'rgb(102, 102, 102)', ico: "iconfont icon-sale", img: "", s_type: "自定义", text: "全部商品", type: "ico" },
                    { color: 'rgb(102, 102, 102)', ico: "iconfont icon-hot", img: "", s_type: "自定义", text: "最新热卖", type: "ico" },
                    { color: 'rgb(102, 102, 102)', ico: "iconfont icon-shangchengicon26", img: "", s_type: "自定义", text: "我的订单", type: "ico" },
                    { color: 'rgb(102, 102, 102)', ico: "iconfont icon-userreg", img: "", s_type: "自定义", text: "个人中心", type: "ico" }]
            };
        }
        else if (cModel.type >= 4 && cModel.type != 7 && cModel.type != 8) {
            cModel.config = {};
        }
    }
    //查询该类组件数量
    function getContorlNum(control) {
        var num = 0;
        if (control == 'foottool') {
            if (vm.only_contorls.foottool) num++;
        }
        else if (control == 'headerbar') {
            if (vm.only_contorls.headerbar) num++;
        }
        else {
            for (var i = 0; i < vm.right_controls.length; i++) {
                if (control == vm.right_controls[i].control) num++;
            }
        }
        return num;
    }
    //生成组件ID
    function getControlID(control, component_field) {
        if (control == 'userinfo') return 'userinfo';
        if (control == 'headsearch') return 'headsearch';
        if (control == 'search') return 'searchbox';
        if (control == 'sidemenubox') return 'sidemenubox';
        if (control == 'totop') return 'totop';
        if (control == 'headerbar') return 'headerbar';
        if (control == 'foottool') return 'foottool';
        var num = getContorlNum(control);
        return component_field + '_' + (num + 1);
    }
    //生成组件字段属性
    function addAutoField(cModel, nControl) {
        //console.log(nControl.component_field_data_value);
        var nValue = $.trim(nControl.component_field_data_value);
        if (cModel.type < 4) {
            if (cModel.type == 1) {
                var data_options = nValue.split("@");
                cModel.options = [];
                for (var j = 0; j < data_options.length; j++) {
                    var datakv = data_options[j].split("|");
                    cModel.options.push({ value: datakv[0], text: datakv[1] });
                }
                if ($.trim(cModel.config) == '' && cModel.options.length > 0) cModel.config = cModel.options[0].value;
            }
            else {
                if ($.trim(cModel.config) == '') cModel.config = nValue;
            }
        }
        else if (cModel.type >= 4 && cModel.type != 7 && cModel.type != 8) {
            var data_options = nValue.split("@");
            cModel.attrs = [];
            if (cModel.control == "navs") {
                cModel.attrs.push({ key: 'nav_list', name: '导航数组', type: -1 });
                if ($.trim(cModel.config.nav_list) == '' && vm.toolbars.length > 0) cModel.config.nav_list = vm.toolbars[0].key_type;
            }
            else if (cModel.control == "slides") {
                cModel.attrs.push({ key: 'slide_list', name: '幻灯片', type: -1 });
                if ($.trim(cModel.config.slide_list) == '' && vm.slides.length > 0) cModel.config.slide_list = vm.slides[0];
            }
            else if (cModel.control == "cardlist") {
                cModel.attrs.push({ key: 'cate_id', name: '分类', type: -1 });
                if ($.trim(cModel.config.cate_id) == '') cModel.config.cate_id = '';
            }
            else if (cModel.control == "activitylist") {
                cModel.attrs.push({ key: 'cate_id', name: '分类', type: -1 });
                if ($.trim(cModel.config.cate_id) == '') cModel.config.cate_id = '';
            }
            else if (cModel.control == "linehead") {
                if (!cModel.config.data) {
                    cModel.data = [];
                }
                else {
                    cModel.data = cModel.config.data;
                }
            }
            

            for (var j = 0; j < data_options.length; j++) {
                var datakv = data_options[j].split("|");
                var attr = {};
                attr.key = datakv[0];

                if (cModel.control == 'navs' && attr.key == 'nav_list') {
                    continue;
                }
                else if (cModel.control == "slides" && attr.key == 'slide_list') {
                    continue;
                }
                else if (cModel.control == "malls") {
                    if (attr.key == 'cate') {
                        cModel.attrs.push({ key: 'cate', name: datakv[1], type: -1 });
                        if ($.trim(cModel.config.cate) == '') cModel.config.cate = '';
                        continue;
                    }
                    if (attr.key == 'tag') {
                        cModel.attrs.push({ key: 'tag', name: datakv[1], type: -1 });
                        if ($.trim(cModel.config.tag) == '') cModel.config.tag = '';
                        continue;
                    }
                    if (attr.key == 'sort_tag') {
                        cModel.attrs.push({ key: 'sort_tag', name: datakv[1], type: -1 });
                        if ($.trim(cModel.config.sort_tag) == '') cModel.config.sort_tag = '';
                        continue;
                    }
                }
                else if (cModel.control == "cardlist" && attr.key == 'cate_id') {
                    continue;
                }
                else if (cModel.control == "activitylist" && attr.key == 'cate_id') {
                    continue;
                }
                else if (cModel.control == "sidemenubox" && attr.key == 'data_key') {
                    cModel.attrs.push({ key: 'data_key', name: datakv[1], type: -1 });
                    continue;
                }
                else if (attr.key == 'search_url' && (cModel.control == "sidemenubox" || cModel.control == "search")) {
                    attr.rmk = datakv[1] + '，关键字占位符{search_key}';
                }

                attr.name = datakv[1];
                attr.type = parseInt(datakv[2]);
                var kv3 = $.trim(datakv[3]);
                if (attr.type == 1) {
                    var li_options = kv3.split("$");
                    attr.options = [];
                    for (var i = 0; i < li_options.length; i++) {
                        var lv = li_options[i].split("#");
                        attr.options.push({ value: lv[1], text: lv[0] });
                    }
                    if ($.trim(cModel.config[attr.key]) == '' && attr.options.length > 0) cModel.config[attr.key] = attr.options[0].value;
                }
                else if (attr.type == 5) {
                    var slider = {};
                    var li_options = kv3.split("$");
                    if (li_options.length > 0 && !isNaN(li_options[0])) slider.min = parseInt(li_options[0]);
                    if (li_options.length > 1 && !isNaN(li_options[1])) slider.max = parseInt(li_options[1]);
                    if (li_options.length > 2 && !isNaN(li_options[2])) slider.step = parseInt(li_options[2]);
                    if (li_options.length > 3 && !isNaN(li_options[3])) {
                        slider.value = parseInt(li_options[3]);
                        if ($.trim(cModel.config[attr.key]) == '') cModel.config[attr.key] = slider.value;
                    }
                    attr.slider = angular.toJson(slider);
                    slider.max = slider.max * 10;//10倍选像素
                    attr.slider_1 = angular.toJson(slider);
                }
                else {
                    attr.value = datakv[3];
                    if ($.trim(cModel.config[attr.key]) == '') cModel.config[attr.key] = attr.value;
                }
                cModel.attrs.push(attr);
            }
        } else if (cModel.type == 7) {

        }
        return cModel;
    }
    function addExData(cModel,listKey) {
        var pushModel = {};
        if (cModel.control == "slides") {
            pushModel = { id: 0, img: '', title: '', link: '', type: cModel.config.slide_list, websiteOwner: '' }
        }
        var nControl;
        if(cModel.control =="foottool"){
            nControl = vm.only_contorls.foottool;
        }
        else{
            nControl = GetControlRow(vm.right_controls, cModel.control_id);
        }
        nControl.is_edit = 1;
        if (listKey) {
            if (!nControl[listKey] || !nControl[listKey].length) {
                nControl[listKey] = [];
                nControl[listKey].push(pushModel);
            }
            else {
                nControl[listKey].push(pushModel);
            }
        } else {
            if (!nControl.data || !nControl.data.length) {
                nControl.data = [];
                nControl.data.push(pushModel);
            }
            else {
                nControl.data.push(pushModel);
            }
        }
        
        //cModel.is_edit = 1;
        //cModel.data.push(pushModel);
        //console.log(vm.right_controls);
        if (cModel.control != "slides") {
            $timeout(function () {
                bindColor();
            });
        }
    }
    function editListData(cModel, data, key, attrIndex, attrKey, cValue) {
        for (var j = 0; j < data.length; j++) {
            if (data[j].key == key) {
                data[j].is_edit = 1;
                if (data[j].data.length == attrIndex) {
                    data[j].data.push(cModel.data[attrIndex]);
                }
                else {
                    data[j].data[attrIndex][attrKey] == cValue;
                }
            }
        }
    }
    function editExData(cModel, attrIndex, attrKey, listKey) {
        cModel.is_edit = 1;
    }
    function deleteExData(cModel, attrIndex, listKey) {
        if (typeof (attrIndex) == 'string') attrIndex = parseInt(attrIndex);
        //if (cModel.control == "slides") {
        //    var key = cModel.config.slide_list;
        //    for (var j = 0; j < vm.slide_data.length; j++) {
        //        if (vm.slide_data[j].key == key) {
        //            vm.slide_data[j].data.splice(attrIndex, 1);
        //        }
        //    }
        //}
        //else if (cModel.type == '7' || cModel.type == '9') {
        //    var key = '';
        //    if (cModel.control == 'navs') {
        //        key = cModel.config.nav_list;
        //    }
        //    else {
        //        key = cModel.config;
        //    }
        //    for (var j = 0; j < vm.toolbar_data.length; j++) {
        //        if (vm.toolbar_data[j].key == key) {
        //            vm.toolbar_data[j].data.splice(attrIndex, 1);
        //        }
        //    }
        //}
        if (cModel.control == 'foottool') {
            vm.only_contorls.foottool.is_edit = 1;
            vm.only_contorls.foottool.data.splice(attrIndex, 1);
        }
        else if (listKey) {
            var nControl = GetControlRow(vm.right_controls, cModel.control_id);
            nControl.is_edit = 1;
            nControl[listKey].splice(attrIndex, 1);
        }
        else{
            var nControl = GetControlRow(vm.right_controls, cModel.control_id);
            nControl.is_edit = 1;
            nControl.data.splice(attrIndex, 1);
        }
        //cModel.is_edit = 1;
        //cModel.data.splice(attrIndex, 1);
        //reloadControl(cModel);
    }
    function getSubmitModel(delProperty, isTemp) {
        var component_config = {};
        if (isTemp) {
            component_config.pageinfo = vm.only_contorls.pageinfo;
        } else {
            component_config.pageinfo = vm.only_contorls.pageinfo;
            component_config.shareinfo = vm.only_contorls.shareinfo;
        }
        var slides = [];
        var toolbars = [];
        if (vm.only_contorls.headerbar && vm.only_contorls.headerbar.config) component_config.head_bar = vm.only_contorls.headerbar.config;
        if (vm.only_contorls.foottool && vm.only_contorls.foottool.config) {
            if (vm.only_contorls.foottool.data) {
                var is_show = $.trim(vm.only_value_controls.foottool_home_show);
                component_config.foottool_home_show = is_show == "" ? "1" : is_show;
                var tempToolbars = [];
                for (var j = 0; j < vm.only_contorls.foottool.data.length; j++) {
                    vm.only_contorls.foottool.data[j].key_type = vm.only_contorls.foottool.config.key_type;
                    tempToolbars.push(angular.fromJson(angular.toJson(vm.only_contorls.foottool.data[j])));
                }
                if (isTemp) {
                    for (var j = 0; j < tempToolbars.length; j++) {
                        //if (tempToolbars[j].key_type) delete tempToolbars[j].key_type;
                        if (tempToolbars[j].type) delete tempToolbars[j].type;
                        if (tempToolbars[j].id) delete tempToolbars[j].id;
                        if (tempToolbars[j].websiteOwner) delete tempToolbars[j].websiteOwner;
                    }
                    component_config.foottool_list = tempToolbars;
                }
                else {
                    if (vm.only_contorls.foottool.is_edit == 1) {
                        if (tempToolbars.length == 0) {
                            toolbars = toolbars.concat({ key_type: vm.only_contorls.foottool.config.key_type, id: -1 });
                        }
                        else {
                            toolbars = toolbars.concat(tempToolbars);
                        }
                    }
                    var nCopyConfig = angular.fromJson(angular.toJson(vm.only_contorls.foottool.config));
                    if (nCopyConfig.list) delete nCopyConfig.list;
                    component_config.foottool_list = nCopyConfig;
                }
            }
        }
        if (vm.only_contorls.totop && vm.only_contorls.totop.config) component_config.totop = vm.only_contorls.totop.config;
        if (vm.only_contorls.sidemenubox && vm.only_contorls.sidemenubox.config) {
            if (!vm.only_contorls.sidemenubox.disabled) {
                component_config.sidemenubox = vm.only_contorls.sidemenubox.config;
            }
        }
        var tempRightControls = angular.fromJson(angular.toJson(vm.right_controls));
        for (var i = 0; i < vm.right_controls.length; i++) {
            var tempRightControlConfig = angular.fromJson(angular.toJson(vm.right_controls[i].config));
            if (delProperty) {
                if (vm.right_controls[i].control == "malls") {
                    delete tempRightControlConfig.mall_list;
                    delete tempRightControlConfig.mall_total;
                    delete tempRightControlConfig.col_list;
                    delete tempRightControlConfig.row_list;
                    delete tempRightControlConfig.sort_list;
                    delete tempRightControlConfig.cate_id;
                    delete tempRightControlConfig.total;
                    delete tempRightControlConfig.rows;
                    delete tempRightControlConfig.page;
                    delete tempRightControlConfig.style_img_bg_array;
                    delete tempRightControlConfig.style_img_last;
                    delete tempRightControlConfig.style_img_last_long;
                }
                else if (vm.right_controls[i].control == "goods") {
                    delete tempRightControlConfig.good_list;
                    delete tempRightControlConfig.style_img_bg_array;
                    delete tempRightControlConfig.style_img_last;
                    delete tempRightControlConfig.style_img_last_long;
                }
                else if (vm.right_controls[i].control == "cardlist") {
                    delete tempRightControlConfig.card_data;
                    delete tempRightControlConfig.cate;
                    delete tempRightControlConfig.page;
                    delete tempRightControlConfig.total;
                }
                else if (vm.right_controls[i].control == "activitylist") {
                    delete tempRightControlConfig.activity_list;
                    delete tempRightControlConfig.cate;
                    delete tempRightControlConfig.page;
                    delete tempRightControlConfig.total;
                }
                else if (vm.right_controls[i].control == "content") {
                    delete tempRightControlConfig.showhtml;
                }
            }
            if (vm.right_controls[i].control == "userinfo") {
                component_config['userinfo'] = tempRightControlConfig;
            }
            else if (vm.right_controls[i].control == "search") {
                component_config['searchbox'] = tempRightControlConfig;
            }
            else if (vm.right_controls[i].control == "linehead") {
                component_config['linehead'] = tempRightControlConfig;
                var tempRightControlData = angular.fromJson(angular.toJson(vm.right_controls[i].data));
                component_config['linehead'].data = tempRightControlData;
            }
            else if (vm.right_controls[i].control == "tabs") {
                if (vm.right_controls[i].data) {
                    var tempToolbars = [];
                    for (var j = 0; j < vm.right_controls[i].data.length; j++) {
                        vm.right_controls[i].data[j].key_type = tempRightControlConfig.key_type;
                        var tempRightControlDataRow = angular.fromJson(angular.toJson(vm.right_controls[i].data[j]));
                        if (isTemp) {
                            if (tempRightControlDataRow.type) delete tempRightControlDataRow.type;
                            if (tempRightControlDataRow.id) delete tempRightControlDataRow.id;
                            if (tempRightControlDataRow.websiteOwner) delete tempRightControlDataRow.websiteOwner;
                        }
                        tempToolbars.push(tempRightControlDataRow);
                    }
                    if (isTemp) {
                        component_config[vm.right_controls[i].control_id] = tempToolbars;
                    }
                    else {
                        if (vm.right_controls[i].is_edit == 1) {
                            if (tempToolbars.length == 0) {
                                toolbars = toolbars.concat({ key_type: tempRightControlConfig.key_type, id: -1 });
                            }
                            else {
                                toolbars = toolbars.concat(tempToolbars);
                            }
                        }
                        if (tempRightControlConfig.list) delete tempRightControlConfig.list;
                        component_config[vm.right_controls[i].control_id] = tempRightControlConfig;
                    }
                }
            }
            else {
                component_config[vm.right_controls[i].control_id] = [];
                if (vm.right_controls[i].control == "slides" && vm.right_controls[i].data) {
                    var tempSlides = [];
                    for (var j = 0; j < vm.right_controls[i].data.length; j++) {
                        vm.right_controls[i].data[j].type = tempRightControlConfig.slide_list;
                        var tempRightControlDataRow = angular.fromJson(angular.toJson(vm.right_controls[i].data[j]));
                        if (isTemp) {
                            if (tempRightControlDataRow.id) delete tempRightControlDataRow.id;
                            if (tempRightControlDataRow.websiteOwner) delete tempRightControlDataRow.websiteOwner;
                        }
                        if ($.trim(tempRightControlDataRow.img) != '') tempSlides.push(tempRightControlDataRow);
                    }
                    if (isTemp) {
                        tempRightControlConfig.slide_list = tempSlides;
                    }
                    else {
                        if (vm.right_controls[i].is_edit == 1) {
                            if (tempSlides.length == 0) {
                                slides = slides.concat({ type: tempRightControlConfig.slide_list, id: -1 });
                            }
                            else {
                                slides = slides.concat(tempSlides);
                            }
                        }
                    }
                }
                if (vm.right_controls[i].control == "navs" && vm.right_controls[i].data) {
                    var tempToolbars = [];
                    for (var j = 0; j < vm.right_controls[i].data.length; j++) {
                        vm.right_controls[i].data[j].key_type = tempRightControlConfig.nav_list;
                        var tempRightControlDataRow = angular.fromJson(angular.toJson(vm.right_controls[i].data[j]));
                        if (isTemp) {
                            if (tempRightControlDataRow.type) delete tempRightControlDataRow.type;
                            if (tempRightControlDataRow.id) delete tempRightControlDataRow.id;
                            if (tempRightControlDataRow.websiteOwner) delete tempRightControlDataRow.websiteOwner;
                            if (tempRightControlDataRow.bg_img_num) delete tempRightControlDataRow.bg_img_num;
                        }
                        tempToolbars.push(tempRightControlDataRow);
                    }
                    if (isTemp) {
                        tempRightControlConfig.nav_list = tempToolbars;
                    }
                    else {
                        if (vm.right_controls[i].is_edit == 1) {
                            if (tempToolbars.length == 0) {
                                toolbars = toolbars.concat({ key_type: tempRightControlConfig.nav_list, id: -1 });
                            }
                            else {
                                toolbars = toolbars.concat(tempToolbars);
                            }
                        }
                    }
                }
                if (vm.right_controls[i].control == "headsearch" && vm.right_controls[i].left_list) {
                    var tempToolbars = [];
                    for (var j = 0; j < vm.right_controls[i].left_list.length; j++) {
                        vm.right_controls[i].left_list[j].key_type = tempRightControlConfig.nav_left;
                        var tempRightControlDataRow = angular.fromJson(angular.toJson(vm.right_controls[i].left_list[j]));
                        if (isTemp) {
                            if (tempRightControlDataRow.type) delete tempRightControlDataRow.type;
                            if (tempRightControlDataRow.id) delete tempRightControlDataRow.id;
                            if (tempRightControlDataRow.websiteOwner) delete tempRightControlDataRow.websiteOwner;
                            if (tempRightControlDataRow.bg_img_num) delete tempRightControlDataRow.bg_img_num;
                        }
                        tempToolbars.push(tempRightControlDataRow);
                    }
                    if (isTemp) {
                        tempRightControlConfig.nav_left_list = tempToolbars;
                    }
                    else {
                        if (vm.right_controls[i].is_edit == 1) {
                            if (tempToolbars.length == 0) {
                                toolbars = toolbars.concat({ key_type: tempRightControlConfig.nav_left, id: -1 });
                            }
                            else {
                                toolbars = toolbars.concat(tempToolbars);
                            }
                        }
                    }
                }
                if (vm.right_controls[i].control == "headsearch" && vm.right_controls[i].right_list) {
                    var tempToolbars = [];
                    for (var j = 0; j < vm.right_controls[i].right_list.length; j++) {
                        vm.right_controls[i].right_list[j].key_type = tempRightControlConfig.nav_right;
                        var tempRightControlDataRow = angular.fromJson(angular.toJson(vm.right_controls[i].right_list[j]));
                        if (isTemp) {
                            if (tempRightControlDataRow.type) delete tempRightControlDataRow.type;
                            if (tempRightControlDataRow.id) delete tempRightControlDataRow.id;
                            if (tempRightControlDataRow.websiteOwner) delete tempRightControlDataRow.websiteOwner;
                            if (tempRightControlDataRow.bg_img_num) delete tempRightControlDataRow.bg_img_num;
                        }
                        tempToolbars.push(tempRightControlDataRow);
                    }
                    if (isTemp) {
                        tempRightControlConfig.nav_right_list = tempToolbars;
                    }
                    else {
                        if (vm.right_controls[i].is_edit == 1) {
                            if (tempToolbars.length == 0) {
                                toolbars = toolbars.concat({ key_type: tempRightControlConfig.nav_right, id: -1 });
                            }
                            else {
                                toolbars = toolbars.concat(tempToolbars);
                            }
                        }
                    }
                }
                component_config[vm.right_controls[i].control_id].push(tempRightControlConfig);
            }
        }
        console.log(component_config);
        var postDataModel = {
            component_id: $.trim(vm.component.AutoId),
            component_key: $.trim(vm.component.ComponentKey),
            component_name: $.trim(vm.component.ComponentName),
            component_model_id: $.trim(vm.component.ComponentModelId),
            component_template_id: $.trim(vm.component.ComponentTemplateId),
            component_config: angular.toJson(component_config),
            is_oauth: vm.component.IsWXSeniorOAuth == true ? 1 : 0,
            is_init_data: vm.component.IsInitData == true ? 1 : 0,
            access_level: $.trim(vm.component.AccessLevel) == '' ? 0 : $.trim(vm.component.AccessLevel),
            slides: angular.toJson(slides),
            toolbars: angular.toJson(toolbars)
        };
        return postDataModel;
    }
    function submitConfig() {
        //{"AutoId":33,"ComponentModelId":22,"ComponentName":"商城首页","ComponentType":"page","ComponentConfig":"{\"slides\":[{\"slide_list\":\"promotion\",\"show_type\":1,\"size_width\":640,\"size_height\":300}],\"navs\":[{\"nav_list\":\"通用投票\",\"col_count\":3,\"style\":1,\"border_show\":0,\"bg_color\":\"#D1D189\",\"radius\":1,\"margin\":5,\"top\":5,\"bottom\":0},{\"nav_list\":-999,\"col_count\":2,\"style\":5,\"border_show\":0,\"bg_color\":\"#D1D189\",\"radius\":1,\"margin\":5,\"top\":0,\"bottom\":5}],\"malls\":[{\"title\":\"T恤\",\"style\":1,\"count\":10,\"sort\":\"def\",\"is_group_buy\":0,\"auto_load\":1,\"title_show\":0}],\"foottool_list\":\"活动导航\"}","ChildComponentIds":null,"WebsiteOwner":"hf","Decription":"","ComponentKey":"MallHome","IsWXSeniorOAuth":0,"AccessLevel":0}
        //模型
        var postDataModel = getSubmitModel(true, false);
        if (postDataModel.component_name == "") {
            alert('请输入页面名称');
            return;
        }
        if ($.trim(vm.only_contorls.pageinfo.title) == "") {
            alert('请输入标题');
            return;
        }
        if ($.trim(vm.only_contorls.shareinfo.title) == "") {
            alert('请输入分享标题');
            return;
        }
        if ($.trim(vm.only_contorls.shareinfo.desc) == "") {
            alert('请输入分享描述');
            return;
        }
        if ($.trim(vm.only_contorls.shareinfo.img_url) == "") {
            alert('请上传分享图片');
            return;
        }
        //console.log(postDataModel.toolbars);
        //return;
        progress();
        commService.postData(
            baseDomain + "serv/api/admin/component/" + (postDataModel.component_id > 0 ? "update.ashx" : "add.ashx"),
            postDataModel,
            function (data) {
                layer.close(layerIndex);
                alert(data.msg);
                if (data.status) {
                    if (postDataModel.component_id == 0) {
                        if (vm.backlist == 1) {
                            window.location.href = "List.aspx";
                        }
                        else {
                            vm.component.AutoId = data.result.component_id;
                        }
                    }
                    //window.location.href = "List.aspx";
                }
            }, function (data) { });
    }
    function toListPage() {
        window.location.href = "List.aspx";
    }
    //生成二维码
    function createQrCode() {
        var qrcode_url = '';
        var keyParm = $.trim(GetParm('key'));
        if (keyParm != '') {
            qrcode_url = 'http://' + vm.strDomain + '/customize/comeoncloud/Index.aspx?key=' + keyParm;
        }
        else {
            var idParm = $.trim(GetParm('component_id'));
            if (idParm != '' && idParm != '0') {
                qrcode_url = 'http://' + vm.strDomain + '/customize/comeoncloud/Index.aspx?cgid=' + idParm;
            }
        }
        if (qrcode_url == '') return;
        var postDataModel = {
            code: qrcode_url
        };
        commService.postData(baseDomain + "serv/api/common/qrcode.ashx", postDataModel, function (data) {
            if (data.status) {
                vm.qrcode = data.result.qrcode_url;
                $(".qrcodeImg").attr('src', vm.qrcode);
                $(".qrcodeCopy").attr('data-clipboard-text', qrcode_url);
                var clip = new ZeroClipboard($(".qrcodeCopy"));
                $('.qrcode').show();
                $(".qrcodeCopy").bind('click', function () {
                    alert('复制完成');
                });
            }
        });
    }
    function showAddType(cModel,key) {
        var dlgHtml = new StringBuilder();
        dlgHtml.AppendFormat('<table class="dialogTable">');
        dlgHtml.AppendFormat('<tr>');
        dlgHtml.AppendFormat('<td><input class="txt" type="text" data-key="' + key + '" placeholder="类型名称" /></td>');
        dlgHtml.AppendFormat('<td><button class="button button-calm button-small confirm">确认</button></td>');
        dlgHtml.AppendFormat('</tr>');
        dlgHtml.AppendFormat('</table>');
        vm.curAddTypeModel = cModel;
        vm.layerAddType = layer.open({
            type: 1,
            content: dlgHtml.ToString(),
            className: 'dialogDiv'
        });
    }
    function showSelectToolbarClass(cModel, cIndex,listKey) {
        vm.curSelectToolbarModel = cModel;
        vm.curSelectToolbarIndex = cIndex;
        vm.curSelectToolbarListKey = listKey;
        var appendhtml = new StringBuilder();
        appendhtml.AppendFormat('<div>');
        appendhtml.AppendFormat('<ul>');
        appendhtml.AppendFormat('<li class="liIco" data-ico="{0}" onmouseover="this.style.background=\'#11c1f3\'" onmouseout="this.style.background=\'\'">', '');
        appendhtml.AppendFormat('&nbsp;');
        appendhtml.AppendFormat('</li>');
        appendhtml.AppendFormat('</ul>');
        appendhtml.AppendFormat('<ul>');
        for (var i = 0; i < vm.iconclasses.length; i++) {
            appendhtml.AppendFormat('<li class="liIco" data-ico="{0}" onmouseover="this.style.background=\'#11c1f3\'" onmouseout="this.style.background=\'\'">', vm.iconclasses[i]);
            appendhtml.AppendFormat('<svg class="icon" aria-hidden="true">');
            appendhtml.AppendFormat('<use xlink:href="#{0}"></use>', vm.iconclasses[i]);
            appendhtml.AppendFormat('</svg>');
            appendhtml.AppendFormat('</li>');
        }
        appendhtml.AppendFormat('</ul>');
        appendhtml.AppendFormat('</div>');
        vm.layerSelectToolbar = layer.open({
            type: 1,
            content: appendhtml.ToString(),
            className: 'dialogSelectToolbarDiv'
        });
    }
    function showSelectAttrClass(cModel, cKey) {
        vm.curSelectAttrModel = cModel;
        vm.curSelectAttrKey = cKey;
        var appendhtml = new StringBuilder();
        appendhtml.AppendFormat('<div>');
        appendhtml.AppendFormat('<ul>');
        appendhtml.AppendFormat('<li class="liIco" data-ico="{0}" onmouseover="this.style.background=\'#11c1f3\'" onmouseout="this.style.background=\'\'">', '');
        appendhtml.AppendFormat('&nbsp;');
        appendhtml.AppendFormat('</li>');
        appendhtml.AppendFormat('</ul>');
        appendhtml.AppendFormat('<ul>');
        for (var i = 0; i < vm.iconclasses.length; i++) {
            appendhtml.AppendFormat('<li class="liIco" data-ico="{0}" onmouseover="this.style.background=\'#11c1f3\'" onmouseout="this.style.background=\'\'">', vm.iconclasses[i]);
            appendhtml.AppendFormat('<svg class="icon" aria-hidden="true">');
            appendhtml.AppendFormat('<use xlink:href="#{0}"></use>', vm.iconclasses[i]);
            appendhtml.AppendFormat('</svg>');
            appendhtml.AppendFormat('</li>');
        }
        appendhtml.AppendFormat('</ul>');
        appendhtml.AppendFormat('</div>');
        vm.layerSelectToolbar = layer.open({
            type: 1,
            content: appendhtml.ToString(),
            className: 'dialogSelectAttrDiv'
        });
    }
    function getTempData() {
        var tempData = {
            art_temp_data: [],
            act_temp_data: [],
            toolbar_temp_data: [],
            slide_temp_data: [],
            mall_temp_data: [],
            good_temp_data: []
        };
        if (vm.only_contorls.foottool && vm.only_contorls.foottool.config) {
            var rowData = getKeyData(tempData.toolbar_temp_data, vm.only_contorls.foottool.config);
            if (rowData == null) tempData.toolbar_temp_data.push({ key: vm.only_contorls.foottool.config, data: vm.only_contorls.foottool.data });
        }
        var tempRightControls = angular.copy(vm.right_controls);
        var dataControlTypes = ["slides", "navs", "tabs", "activitylist", "malls", "cardlist"];
        for (var i = 0; i < tempRightControls.length; i++) {
            if ($.inArray(tempRightControls[i].control, dataControlTypes) == -1) continue;

            if (tempRightControls[i].control == "malls") {
                var tempKey = $.trim(tempRightControls[i].config.cate) + '_' + $.trim(tempRightControls[i].config.tag) + '_' + $.trim(tempRightControls[i].config.is_group_buy) + '_' + $.trim(tempRightControls[i].config.count) + '_' + $.trim(tempRightControls[i].config.sort) + '_' + $.trim(tempRightControls[i].config.sort_tag);
                tempData.mall_temp_data.push({ key: tempKey, data: { list: tempRightControls[i].config.mall_list, totalcount: tempRightControls[i].config.mall_total } });
            }
            else if (tempRightControls[i].control == "goods") {
                var tempKey = $.trim(tempRightControls[i].config.ids);
                tempData.good_temp_data.push({ key: tempKey, data: tempRightControls[i].config.good_list });
            }
            else if (tempRightControls[i].control == "cardlist") {
                var tempKey = $.trim(tempRightControls[i].config.cate_id) + '_' + $.trim(tempRightControls[i].config.rows);
                tempData.art_temp_data.push({ key: tempKey, data: tempRightControls[i].config.card_data });
            }
            else if (tempRightControls[i].control == "activitylist") {
                var tempKey = $.trim(tempRightControls[i].config.cate_id) + '_' + $.trim(tempRightControls[i].config.sort);
                tempData.act_temp_data.push({ key: tempKey, data: tempRightControls[i].config.activity_list });
            }
            else if (tempRightControls[i].control == "slides") {
                var rowData = getKeyData(tempData.slide_temp_data, tempRightControls[i].config.slide_list);
                if (rowData == null) tempData.slide_temp_data.push({ key: tempRightControls[i].config.slide_list, data: tempRightControls[i].data });
            }
            else if (tempRightControls[i].control == "navs") {
                var rowData = getKeyData(tempData.toolbar_temp_data, tempRightControls[i].config.nav_list);
                if (rowData == null) tempData.toolbar_temp_data.push({ key: tempRightControls[i].config.nav_list, data: tempRightControls[i].data });
            }
            else if (tempRightControls[i].control == "tabs") {
                var rowData = getKeyData(tempData.toolbar_temp_data, tempRightControls[i].config);
                if (rowData == null) tempData.toolbar_temp_data.push({ key: tempRightControls[i].config, data: tempRightControls[i].data });
            }
        }
        return angular.toJson(tempData);
    }
    function saveTemplate(tempName, tempImg, tempCate) {
        var tempData = getTempData();
        var submitModel = getSubmitModel(true, true);
        var tempModel = {
            name: tempName,
            img: tempImg,
            cate: $.trim(tempCate) == ""?0:$.trim(tempCate),
            config: submitModel.component_config,
            data: tempData,
            component_id: submitModel.component_id,
            component_model_id: submitModel.component_model_id
        }
        var postUrl = baseDomain + "serv/api/admin/component/template/add.ashx"
        if (vm.edit_template == 1) {
            vm.component.ComponentName = tempName;
            vm.template_img = tempImg;
            vm.template_cate = tempCate;
            tempModel.id = vm.component.ComponentTemplateId;
            postUrl = baseDomain + "serv/api/admin/component/template/update.ashx"
        }
        commService.postData(
            postUrl,
            tempModel,
            function (data) {
                alert(data.msg);
                if (data.status) {
                    layer.close(vm.layerSaveTemp);
                }
            }, function (data) { });
    }
    //保存模板
    function saveTemplateDialog() {
        var dlgHtml = new StringBuilder();
        dlgHtml.AppendFormat('<table class="dialogTable">');
        dlgHtml.AppendFormat('<tr>');
        if (vm.edit_template == 1) {
            dlgHtml.AppendFormat('<td><input class="txt" type="text" placeholder="模板名称" value="{0}" /></td>', vm.component.ComponentName);
        }
        else {
            dlgHtml.AppendFormat('<td><input class="txt" type="text" placeholder="模板名称" /></td>');
        }
        dlgHtml.AppendFormat('</tr>');
        dlgHtml.AppendFormat('<tr>');
        dlgHtml.AppendFormat('<td><select class="ddlTempCate">');
        dlgHtml.AppendFormat('<option value="" {0}> -- 分类 -- </option>', $.trim(vm.template_cate) == '' ? 'selected="selected"' : '');
        for (var i = 0; i < vm.template_types.length; i++) {
            dlgHtml.AppendFormat('<option value="{0}" {2}>{1}</option>', vm.template_types[i].value, vm.template_types[i].text, $.trim(vm.template_cate) == vm.template_types[i].value ? 'selected="selected"' : '');
        }
        dlgHtml.AppendFormat('</select></td>');
        dlgHtml.AppendFormat('</tr>');
        dlgHtml.AppendFormat('<tr>');
        
        if (vm.edit_template == 1) {
            dlgHtml.AppendFormat('<td class="imgBgTd"><div class="imgTempBgDiv" style="display:none;"><div class="tipDiv">模板图片<br/>240 X 350</div></div><div class="imgBgDiv"><img class="tempImg" src="{0}" /><input class="tempImgFile" name="file1" type="file"></div></td>', vm.template_img);
        } else {
            dlgHtml.AppendFormat('<td class="imgBgTd"><div class="imgTempBgDiv"><div class="tipDiv">模板图片<br/>240 X 350</div></div><div class="imgBgDiv" style="display:none;"><img class="tempImg" /><input class="tempImgFile" name="file1" type="file"></div></td>');
        }
        dlgHtml.AppendFormat('</tr>');
        dlgHtml.AppendFormat('<tr>');
        dlgHtml.AppendFormat('<td class="tdCenter"><button class="button button-calm button-small confirm mAll5">确认</button><button class="button button-stable button-small cancel mAll5">取消</button></td>');
        dlgHtml.AppendFormat('</tr>');
        dlgHtml.AppendFormat('</table>');
        vm.layerSaveTemp = layer.open({
            type: 1,
            shadeClose: false,
            content: dlgHtml.ToString(),
            className: 'dialogTemplateDiv'
        });
    }
    //查询模板列表
    function getTemplateList(page, rows, cate, keyword, callback) {
        commService.postData(
            baseDomain + "serv/api/admin/component/template/list.ashx",
            { page: page, rows: rows ,cate :cate, keyword:keyword},
            function (data) {
                if (data.status) {
                    callback(data.result);
                }
                else {
                    alert(data.msg);
                }
            }, function (data) { });
    }
    //查询模板配置信息
    function getTemplateConfig(id, callback) {
        commService.postData(
            baseDomain + "serv/api/admin/component/template/get.ashx",
            { id: id },
            function (data) {
                if (data.status) {
                    callback(data.result);
                }
                else {
                    alert(data.msg);
                }
            }, function (data) { });
    }
    //选择模板对话框
    function selectTemplateDialog(page) {
        vm.template_select_set.page = page;
        var dlgHtml = new StringBuilder();
        dlgHtml.AppendFormat('<div class="templateClose"><img src="/img/delete.png" /></div>');
        dlgHtml.AppendFormat('<div class="searchDiv">');
        dlgHtml.AppendFormat('<button type="button" class="btnbox button {2} btnSearchCateTemplate" data-value="{0}">{1}</button>', '', '全部', $.trim(vm.template_select_set.cate) == "" ? 'button-calm' : '');
        for (var i = 0; i < vm.template_types.length; i++) {
            dlgHtml.AppendFormat('<button type="button" class="btnbox button {2} btnSearchCateTemplate" data-value="{0}">{1}</button>', vm.template_types[i].value, vm.template_types[i].text, $.trim(vm.template_select_set.cate) == vm.template_types[i].value ? 'button-calm' : ''); 
        }
        //dlgHtml.AppendFormat('分类：<select class="selectbox selectTemplateCate">');
        //dlgHtml.AppendFormat('<option value="" {0}></option>', $.trim(vm.template_select_set.cate) == '' ? 'selected="selected"' : '');
        //for (var i = 0; i < vm.template_types.length; i++) {
        //    dlgHtml.AppendFormat('<option value="{0}" {2}>{1}</option>', vm.template_types[i].value, vm.template_types[i].text, $.trim(vm.template_select_set.cate) == vm.template_types[i].value ? 'selected="selected"' : '');
        //}
        //dlgHtml.AppendFormat('</select>');
        //dlgHtml.AppendFormat('名称：<input type="text" class="txtbox txtTemplateName" value="{0}"/>', vm.template_select_set.keyword);
        //dlgHtml.AppendFormat('<button type="button" class="btnbox button button-calm btnSearchTemplate" />查询</button>');
        dlgHtml.AppendFormat('</div>');
        dlgHtml.AppendFormat('<div class="listDiv">');
        dlgHtml.AppendFormat('<div class="listContentDiv">');
        dlgHtml.AppendFormat('</div>');
        dlgHtml.AppendFormat('</div>');
        vm.layerSelectTemp = layer.open({
            type: 1,
            shadeClose: false,
            content: dlgHtml.ToString(),
            className: 'dialogSelectTemplateDiv'
        });
        $('.dialogSelectTemplateDiv .listDiv').unbind().bind('scroll', function () { scrollTempDiv(this) });
        selectTempSearch();
    }
    //选择使用模板
    function selectTemplateConfirm(tempId, tempName) {
        var n_key = vm.template_select_set.cate + "_" + vm.template_select_set.keyword;
        var key_temp_data = GetControlRowByKey(vm.temp_data, 'key', n_key);
        var nTemp = $.grep(key_temp_data.data, function (cur, i) {
            return cur['id'] == tempId;
        });
        if (nTemp[0].config) {
            selectTemplateAction(tempId, tempName, nTemp[0].config, nTemp[0].data);
        }
        else {
            getTemplateConfig(tempId, function (data) {
                nTemp[0].config = data.config;
                nTemp[0].data = data.data;
                selectTemplateAction(tempId, tempName, nTemp[0].config, nTemp[0].data);
            })
        }
    }
    function selectTemplateAction(tempId, tempName, config, data) {
        vm.component.ComponentTemplateId = tempId;
        vm.template_config = angular.fromJson(config);
        vm.component.ComponentConfig = vm.template_config;
        if (vm.component.AutoId == 0) {
            vm.component.ComponentName = tempName;
        }

        vm.template_data = angular.fromJson(data);
        if (!vm.template_data.good_temp_data) vm.template_data.good_temp_data = [];

        clearTemplateData();
        clearComponentFieldDisabledFalse();
        getRightControls();
        //console.log(vm.component);
        $timeout(function () {
            setMallConfigToPage();
            $scope.$apply();
        });
        layer.close(vm.layerSelectTemp);
    }
    //清除上一次模板选择的数据
    function clearTemplateData() {
        vm.only_contorls = {
            pageinfo: {
                title: '',
                bg_img: '',
                bg_color: ''
            },
            shareinfo: {
                title: '',
                desc: '',
                img_url: '',
                link: ''
            }
        };
        //vm.only_contorls.pageinfo = null;
        vm.only_contorls.headerbar = null;
        vm.only_contorls.foottool = null;
        vm.only_contorls.sidemenubox = null;
        vm.only_contorls.totop = null;
        vm.slides = slides;
        vm.toolbars = toolbars;
        vm.mall_cates = mall_cates;
        vm.mall_tags = mall_tags;
        vm.art_cates = art_cates;
        vm.act_cates = act_cates;
        vm.art_data = [];
        vm.act_data = [];
        vm.toolbar_data = [];
        vm.slide_data = [];
        vm.mall_data = [];
        vm.only_value_controls = {};
        vm.right_controls = [];
        vm.cur_select_controls = [];
    }
    //多选是否选中
    function isChecked(nValue, ckValue) {
        //console.log(nValue);
        //console.log(ckValue);
        if (!nValue) return false;

        var lst = nValue.split(',');
        //console.log(lst);
        return ($.inArray(ckValue, lst) >= 0);
    }
    //多选当前选中事件
    function checkboxControlData(cModel, key, event) {
        var lst = [];
        $(event.target).closest('div').parent().find('input[name="chkmall' + key + '"]:checked').each(function () {
            lst.push($(this).val());
        });
        cModel.config[key] = lst.join(',');
        editControlData(cModel)
    }
    function moveModelUp(cModel) {
       var nindex = GetControlRowIndex(vm.right_controls, cModel.control_id);
       if (nindex > 0) {
           $timeout(function () {
               $scope.$apply(function () {
                   vm.right_controls.splice(nindex - 1, 0, vm.right_controls[nindex]);
                   vm.right_controls.splice(nindex + 1, 1);
                   setRightPaddingTop(cModel);
               });
           });
       }
    }
    function moveModelDown(cModel) {
        var nindex = GetControlRowIndex(vm.right_controls, cModel.control_id);
        if (nindex >= 0 && nindex < vm.right_controls.length - 1) {
            $timeout(function () {
                $scope.$apply(function () {
                    vm.right_controls.splice(nindex + 2, 0, vm.right_controls[nindex]);
                    vm.right_controls.splice(nindex, 1);
                    setRightPaddingTop(cModel);
                });
            });
        }
    }
        
    function moveUpExData(cModel, nindex,listKey) {
        if(listKey){
            if (nindex > 0) {
                cModel.is_edit = 1;
                cModel[listKey].splice(nindex - 1, 0, cModel[listKey][nindex]);
                cModel[listKey].splice(nindex + 1, 1);
            }
        }else{
            if (nindex > 0) {
                cModel.is_edit = 1;
                cModel.data.splice(nindex - 1, 0, cModel.data[nindex]);
                cModel.data.splice(nindex + 1, 1);
            }
        }
    }
    function moveDownExData(cModel, nindex, listKey) {
        if (listKey) {
            if (nindex >= 0 && nindex < cModel[listKey].length - 1) {
                cModel.is_edit = 1;
                cModel[listKey].splice(nindex + 2, 0, cModel[listKey][nindex]);
                cModel[listKey].splice(nindex, 1);
            }
        } else {
            if (nindex >= 0 && nindex < cModel.data.length - 1) {
                cModel.is_edit = 1;
                cModel.data.splice(nindex + 2, 0, cModel.data[nindex]);
                cModel.data.splice(nindex, 1);
            }
        }
    }

    function initTemplateCateList() {
        $.ajax({
            type: "Post",
            url: "/serv/api/article/category/selectlist.ashx",
            data: { type: 'CompTempType', websiteowner: 'Common' },
            success: function (result) {
                if (result.status) {
                    vm.template_types = result.result;
                }
            }
        });
    }
    function scrollTempDiv(tob) {
        if ((($(tob).get(0).scrollHeight - $(tob).height()) - $(tob).scrollTop() <= 20)) {
            var key_temp_data = GetControlRowByKey(vm.temp_data, 'key', vm.template_select_set.cate + "_" + vm.template_select_set.keyword);
            if (key_temp_data && key_temp_data.data.length < key_temp_data.total) {
                vm.template_select_set.page++;
                selectTempSearch();
            }
        }
    }
    function selectTempSearch() {
        var n_key = vm.template_select_set.cate + "_" + vm.template_select_set.keyword;
        var key_temp_data = GetControlRowByKey(vm.temp_data, 'key', n_key);
        if (!key_temp_data) {
            vm.temp_data.push({ key: n_key, data: [], total: -1 });
            key_temp_data = GetControlRowByKey(vm.temp_data, 'key', n_key);
        }
        if (vm.template_select_set.page == 1) {
            $('.dialogSelectTemplateDiv .listDiv .listContentDiv').html('');
        }

        var startIndex = (vm.template_select_set.page - 1) * vm.template_select_set.rows;
        if (key_temp_data.total == 0) {

        }
        else if (key_temp_data.total > 0 && startIndex < key_temp_data.data.length) {
            var maxLength = vm.template_select_set.page * vm.template_select_set.rows < key_temp_data.total ? vm.template_select_set.page * vm.template_select_set.rows : key_temp_data.total;
            if (key_temp_data.data.length > 0) {
                var dlgHtml = new StringBuilder();
                for (var i = startIndex; i < maxLength; i++) {
                    dlgHtml.AppendFormat('<div class="templateDiv" data-temp-id="{0}">', key_temp_data.data[i].id);
                    dlgHtml.AppendFormat('<div class="templateImgDiv">');
                    dlgHtml.AppendFormat('<img src="{0}" class="templateImg">', key_temp_data.data[i].img);
                    dlgHtml.AppendFormat('</div>');
                    dlgHtml.AppendFormat('<div class="templateTitle">{0}</div>', key_temp_data.data[i].name);
                    dlgHtml.AppendFormat('<div class="confirmDiv">');
                    if (vm.component.ComponentTemplateId == key_temp_data.data[i].id) {
                        dlgHtml.AppendFormat('<div class="botton reset">正在使用</div>'); //(重置)
                    }
                    else {
                        dlgHtml.AppendFormat('<div class="botton confirm">使用该模板</div>');
                    }
                    dlgHtml.AppendFormat('</div>');
                    dlgHtml.AppendFormat('</div>');
                }
                $('.dialogSelectTemplateDiv .listDiv .listContentDiv').append(dlgHtml.ToString());
            }
        }
        else {
            progress();
            getTemplateList(vm.template_select_set.page, vm.template_select_set.rows, vm.template_select_set.cate, vm.template_select_set.keyword, function (data) {
                layer.close(layerIndex);
                key_temp_data.data = key_temp_data.data.concat(data.list);
                key_temp_data.total = data.totalcount;
                if(data.list.length >0){
                    var dlgHtml = new StringBuilder();
                    for (var i = 0; i < data.list.length; i++) {
                        dlgHtml.AppendFormat('<div class="templateDiv" data-temp-id="{0}">', data.list[i].id);
                        dlgHtml.AppendFormat('<div class="templateImgDiv">');
                        dlgHtml.AppendFormat('<img src="{0}" class="templateImg">', data.list[i].img);
                        dlgHtml.AppendFormat('</div>');
                        dlgHtml.AppendFormat('<div class="templateTitle">{0}</div>', data.list[i].name);
                        dlgHtml.AppendFormat('<div class="confirmDiv">');
                        if (vm.component.ComponentTemplateId == data.list[i].id) {
                            dlgHtml.AppendFormat('<div class="botton reset">正在使用</div>'); //(重置)
                        }
                        else {
                            dlgHtml.AppendFormat('<div class="botton confirm">使用该模板</div>');
                        }
                        dlgHtml.AppendFormat('</div>');
                        dlgHtml.AppendFormat('</div>');
                    }
                    $('.dialogSelectTemplateDiv .listDiv .listContentDiv').append(dlgHtml.ToString());
                }
            });
        }
    }
    function setHideColor(color) {
        var colorString = "";
        if (color) colorString = color.toRgbString();
        setColorChange(this, colorString, false);
    }
    function setMoveColor(color) {
        var colorString = "";
        if (color) colorString = color.toRgbString();
        setColorChange(this, colorString, false);
    }
    function setChangeColor(color) {
        var colorString = "";
        if (color) colorString = color.toRgbString();
        setColorChange(this, colorString, true);
    }
    function setColorChange(ob, colorString,isChange) {
        var control = $(ob).attr('control-control');
        var controlId = $(ob).attr('control-id');
        var attrIndex = $.trim($(ob).attr('attr-index'));
        var attrKey = $(ob).attr('attr-key');
        var nControl;
        if (control == "foottool") {
            nControl = vm.only_contorls.foottool;
        }
        else if (control == "headerbar") {
            nControl = vm.only_contorls.headerbar;
        }
        else if (control == "pageinfo") {
            nControl = vm.only_contorls.pageinfo;
        }
        else if (control == "sidemenubox") {
            nControl = vm.only_contorls.sidemenubox;
        }
        else {
            nControl = GetControlRow(vm.right_controls, controlId);
        }
        if (isChange) nControl.is_edit = 1;
        if (attrIndex != "") {
            attrIndex = parseInt(attrIndex);
            $scope.$apply(function () {
                nControl.data[attrIndex][attrKey] = colorString;
            })
        }
        else {
            if (!nControl.config){
                $scope.$apply(function () {
                    nControl[attrKey] = colorString;
                });
            }
            else {
                $scope.$apply(function () {
                    nControl.config[attrKey] = colorString;
                });
            }
        }
    }
    function bindColor() {
        if ($('.color').length > 0) {
            $('.color').spectrum("destroy");
            $('.color').spectrum({ hide: setHideColor, move: setMoveColor, change: setChangeColor });
        }
    }
    function getPageBg(bg_img, bg_img_style, bg_color) {
        if(bg_img && bg_img_style==2){
            return "background:url(" + bg_img + ") no-repeat; background-size:320px 568px;";
        }
        else if(bg_img){
            return "background:url("+bg_img+") 100% bottom no-repeat;";
        }
        else if(bg_color){
            return "background:"+bg_color;
        }
        return "";
    }
    function clearBgImg() {
        vm.only_contorls.pageinfo.bg_img = '';
        $('.pgBgImg').removeAttr('src');
    }
}]);
