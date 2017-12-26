/**
 * @���� merauy@gmail.com
 * @Copyright(c)2012~2014 
 * @��Դ http://www.goodxyx.com/jquery/plugins/pager.html
 * @version 1.0.0
 * @direction jQuery��ҳ���
 * @comment ֧��ajax��ҳ����������ҳ������̬��ҳ����
 */
$.extend({
    pager: function (param) {
        var _isAppend = false;
        if (this === jQuery) {
            return new $.pager(param);
        }

        for (var i in $.pagerSettings) {
            eval(['this["set',
			i.charAt(0).toUpperCase(),
			i.substr(1),
			'"]=function(v){return this.setOption("', i, '",v)}'].join(''));
        }

        this.toString = function () {
            if (this['styleType']) {
                return this.toString2();
            }
            var _pagerHtml = [];
            var key = this._prepare().getRegisterKey();

            if (this.showNumsOption) {
                if (this.numsOption.length > 1) {
                    //ÿҳ��ʾ����
                    _pagerHtml[_pagerHtml.length] = '<SPAN>\u6bcf\u9875\u663e\u793a\u6761\u6570</SPAN>';
                    _pagerHtml[_pagerHtml.length] = '<SELECT onchange="$.pagerChange(' + key + ',null,this.value);">';
                    for (var i = 0, j = this.numsOption.length; i < j; ++i) {
                        if (this.numsOption[i] == this.nums) {
                            _pagerHtml[_pagerHtml.length] =
                                '<OPTION value="' + this.numsOption[i] + '" selected>' + this.numsOption[i] + '</OPTION>';
                        } else {
                            _pagerHtml[_pagerHtml.length] =
                                '<OPTION value="' + this.numsOption[i] + '">' + this.numsOption[i] + '</OPTION>';
                        }
                    }
                    _pagerHtml[_pagerHtml.length] = '</SELECT>';
                } else {
                    //ÿҳ��ʾ + N + ��
                    _pagerHtml[_pagerHtml.length] =
                        '<SPAN>\u6bcf\u9875\u663e\u793a' + this.nums + '\u6761</SPAN>';
                }
            }

            if (this.showPagerInfo) {
                _pagerHtml[_pagerHtml.length] = '<SPAN>';
                _pagerHtml[_pagerHtml.length] =
                  this.startNum + '\uff5e' + this.endNum + "/" + this.count;
                _pagerHtml[_pagerHtml.length] = '</SPAN>';
            }

            if (this.page == 1) {
                //��һҳ
                _pagerHtml[_pagerHtml.length] =
                    '<span class="current prev">\u4e0a\u4e00\u9875</span>';
            } else {
                _pagerHtml[_pagerHtml.length] =
                    '<A href="#" class="prev" onclick="return $.pagerChange(' + key + ',' + this.prevPage + ')">\u4e0a\u4e00\u9875</A>';
            }

            if (this.showButtons) {
                if (this.numberOfButtons) {//���ָ������ʾ�İ�ť��������ô�Ͱ�����ʾ
                    var _fpage = this.page - Math.floor(this.numberOfButtons / 2);
                    var _epage;
                    if (_fpage < 1) {
                        _fpage = 1;
                        _epage = this.numberOfButtons;
                        if (_epage > this.pages) {
                            _epage = this.pages;
                        }
                    } else if (this.pages < this.page + this.numberOfButtons / 2) {
                        _epage = this.pages;
                        _fpage = this.pages - this.numberOfButtons + 1;
                        if (_fpage < 1) {
                            _fpage = 1;
                        }
                    } else {
                        _epage = _fpage + this.numberOfButtons - 1;
                    }
                    _epage++;
                    for (var i = _fpage; i < _epage ; ++i) {
                        if (i == this.page) {
                            _pagerHtml[_pagerHtml.length] = '<span class="current">' + this.page + '</span>';
                        } else {
                            _pagerHtml[_pagerHtml.length] =
                            '<a href="#" onclick="return $.pagerChange(' + key + ',' + i + ')">' + i + '</a>';
                        }
                    }
                } else {
                    _pagerHtml[_pagerHtml.length] = '<SELECT onchange="$.pagerChange(' + key + ',this.value);">';
                    for (var i = 1, j = this.pages + 1; i < j; ++i) {
                        if (i == this.page) {
                            _pagerHtml[_pagerHtml.length] = '<OPTION value="' + i + '" selected>' + i + '</OPTION>';
                        } else {
                            _pagerHtml[_pagerHtml.length] = '<OPTION value="' + i + '">' + i + '</OPTION>';
                        }
                    }
                    _pagerHtml[_pagerHtml.length] = '</SELECT>';
                }
            }
            if (this.showCurrentPageInfo) {
                _pagerHtml[_pagerHtml.length] = '<SPAN class="currentPageInfo">';
                _pagerHtml[_pagerHtml.length] = this.page + " / " + this.pages;
                _pagerHtml[_pagerHtml.length] = '</SPAN>';
            }
            if (this.page != this.nextPage) {
                //��һҳ
                _pagerHtml[_pagerHtml.length] =
				'<A href="#" class="next" onclick="return $.pagerChange(' + key + ',' + this.nextPage + ')">\u4e0b\u4e00\u9875</A>';
            } else {
                _pagerHtml[_pagerHtml.length] = '<span class="current next">\u4e0b\u4e00\u9875</span>';
            }
            return _pagerHtml.join('');
        };

        this.toString2 = function () {
            var _pagerHtml = [];
            var key = this._prepare().getRegisterKey();

            if (this.count <= this.nums) {
                return '';
            }

            if (this.page > 1) {
                _pagerHtml[_pagerHtml.length] =
				['<a href="#" onclick="return $.pagerChange(', key, ',', this.prevPage, ')">&lt; Prev</a>'].join('');
            } else {
                _pagerHtml[_pagerHtml.length] = '<span class="disabled">&lt; Prev</span>';
            }
            if (this.pages <= 10) {
                for (var i = 1; i <= this.pages; ++i) {
                    if (i == this.page) {
                        _pagerHtml[_pagerHtml.length] = ['<span class="current">', i, '</span>'].join('');
                    } else {
                        _pagerHtml[_pagerHtml.length] =
							['<a href="#" onclick="return $.pagerChange(', key, ',', i, ')">', i, '</a>'].join('');
                    }
                }

            } else {
                if (this.page <= 5) {
                    for (var i = 1; i <= 7; ++i) {
                        if (i == this.page) {
                            _pagerHtml[_pagerHtml.length] =
								['<span class="current">', i, '</span>'].join('');
                        } else {
                            _pagerHtml[_pagerHtml.length] =
								['<a href="#" onclick="return $.pagerChange('
								, key, ',', i, ')">', i, '</a>'].join('');
                        }
                    }
                    _pagerHtml[_pagerHtml.length] =
						['...<a href="#" onclick="return $.pagerChange('
						, key, ',', this.pages - 1, ')">', (this.pages - 1), '</a>'].join('');
                    _pagerHtml[_pagerHtml.length] =
						['<a href="#" onclick="return $.pagerChange('
						, key, ',', this.pages, ')">', this.pages, '</a>'].join('');
                } else if (this.page >= this.pages - 4) {
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange('
						, key, ',1)">1</a>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange('
						, key, ',2)">2</a>...'
                    ].join('');
                    for (var i = this.pages - 6; i <= this.pages; ++i) {
                        if (i == this.page) {
                            _pagerHtml[_pagerHtml.length] = [
								'<span class="current">', i, '</span>'
                            ].join('');
                        } else {
                            _pagerHtml[_pagerHtml.length] = [
								'<a href="#" onclick="return $.pagerChange(', key, ',', i, ')">', i, '</a>'
                            ].join('');
                        }
                    }
                } else {
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange(', key, ',1)">1</a>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange(', key, ',2)">2</a>...'
                    ].join('');

                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange(', key, ',', this.page - 2, ')">', (this.page - 2), '</a>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange(', key, ',', this.page - 1, ')">', this.page - 1, '</a>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'<span class="current">', this.page, '</span>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange(', key, ',', this.page + 1, ')">', (this.page + 1), '</a>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="# onclick="return $.pagerChange(', key, ',', this.page + 2, ')"">', (this.page + 2), '</a>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'...<a href="#" onclick="return $.pagerChange(', key, ',', this.pages - 1, ')">', (this.pages - 1), '</a>'
                    ].join('');
                    _pagerHtml[_pagerHtml.length] = [
						'<a href="#" onclick="return $.pagerChange(', key, ',', this.pages, ')">', this.pages, '</a>'
                    ].join('');
                }
            }

            if (this.page < this.pages) {
                _pagerHtml[_pagerHtml.length] = [
					'<a href="#" onclick="return $.pagerChange(', key, ',', this.nextPage, ')">Next &gt;</a>'
                ].join('');
            } else {
                _pagerHtml[_pagerHtml.length] = '<span class="disabled">Next &gt;</span>';
            }

            //Ҳ�����˻��ʣ�Ϊʲô����ô��.length����ֻ��˵Ч������
            //û�е��в��ԣ���û�з���Ȩ
            return _pagerHtml.join('');
        };

        this._prepare = function () {
            var anumsOption = this.numsOption;
            if (anumsOption.length == 1) {
                this.nums = anumsOption[0];
            } else if (anumsOption.length > 1) {
                if ($.inArray(this.nums - 0, anumsOption) == -1) {
                    this.nums = anumsOption[0];
                }
            } else if (this.nums < 1) {
                this.nums = 20;
            }

            this.pages = Math.ceil(this.count / this.nums);
            if (!this.pages) {
                this.pages = 1;
            }

            if (this.page < 1) {
                this.page = 1;
            } else if (this.page > this.pages) {
                this.page = this.pages;
            }
            if (this.count) {
                this.startNum = (this.page - 1) * this.nums + 1;
                this.endNum = this.page * this.nums;
                if (this.endNum > this.count) {
                    this.endNum = this.count;
                }
            } else {
                this.startNum = this.endNum = 0;
            }
            this.prevPage = this.page > 1 ? (this.page - 1) : 1;
            this.nextPage = this.page < this.pages ? this.page - 0 + 1 : this.pages;
            if (!this.ajax.url) {
                this.ajax.url = document.URL;
            }
            return this;
        };

        this.setOption = function (option, value) {
            if (typeof option == 'object') {
                for (var i in option) {
                    this.setOption(i, option[i]);
                }
            } else if (option == 'ajax') {
                $.extend(this.ajax, value);
            } else if (option == 'ajaxContainer') {
                this.ajaxContainer = $(value);
            } else if (option == 'pageContainer') {
                if (this.pageContainer == $(this.pageContainer)) {
                    this.pageContainer.html('');
                }
                if (_isAppend) {
                    this.pageContainer = $(value);
                    this.reloadBar();
                } else {
                    this.appendTo(value);
                }
            } else {
                this[option] = value;
            }
            return this;
        };

        this.reloadBar = function () {
            this.pageContainer.html(this.toString());
            return this;
        }

        this.reload = function () {
            var _pageString = this.toString();
            //���ָ����Ҫ�첽ˢ�µ����ݿ飬��ִ���첽��ȡ����
            if (this.ajaxContainer) {
                var _this = this;
                var ajax = this.ajax;

                $.extend(ajax.data, {
                    page: this.page,
                    count: this.count,
                    nums: this.nums
                });

                $.ajax({
                    url: ajax.url,
                    data: ajax.data,
                    cache: ajax.cache,
                    type: ajax.type,
                    success: function (html) {
                        if (_this.ajaxContainer === null) {
                            if (typeof ajax['end'] == 'function') {
                                ajax['end'].call(_this);
                            }
                            return;
                        }
                        _this.ajaxContainer.html(html);
                        _this.pageContainer.html(_pageString);
                        if (typeof ajax['end'] == 'function') {
                            ajax['end'].call(_this);
                        }
                        _this = _pageString = ajax = null;
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        _this = _pageString = ajax = null;
                        alert(textStatus + ":" + errorThrown);
                        throw '';
                    }
                });
            } else {
                this.pageContainer.html(_pageString);
            }

            return this;
        };

        this.getRegisterKey = function () {
            for (var i in $._pagers) {
                if ($._pagers[i] == this) {
                    return i;
                }
            }
            return null;
        };

        this.hide = function () {
            this.pageContainer.hide();
            return this;
        };

        this.show = function () {
            this.pageContainer.show();
            return this;
        };

        this.remove = function () {
            var k = this.getRegisterKey();
            if (k !== null) {
                try {
                    delete $._pagers[k];
                } catch (e) {
                    $._pagers[k] = null;
                }
            }
            this.pageContainer.html('');

            for (k in this) {
                this[k] = null;
            }
        };

        this.appendTo = function (obj) {
            if (_isAppend) {
                //ֵ�������appendToһ��
                throw '\u53ea\u5141\u8bb8\u5904\u7406\u4e00\u6b21';
            }
            _isAppend = true;
            $._pagers.push(this);
            return this.setOption('pageContainer', obj);
        };

        param = param || {};

        var t;

        $.extend(true, this, $.pagerSettings);

        //���û������ÿҳ��ʾ����������ô���ж��Ƿ������COOKIE����ȡֵ
        if (!param.nums && this.numsFromCookie) {
            t = $.cookie(this.numsCookieName);
            if (t) {
                param.nums = t;
            }
        }

        if (t = param.pageContainer) {//���ָ������ʾ��λ�ã��Ȳ���������Ϻ�Ž�����ʾ
            delete param.pageContainer;
        }

        for (var i in param) {
            this.setOption(i, param[i]);
        }

        if (t) {
            this.setOption('pageContainer', t);
        }
    },
    pagerSettings: {
        page: 1,
        nums: 20,
        pages: 1,
        count: 0,
        numsOption: [10, 20, 50, 100],
        showNumsOption: false, //��ʾÿҳ��ʾ�� ѡ��
        showPagerInfo: false, //��ʾ��ǰ��¼��Ϣ
        numsFromCookie: null,
        numsCookieName: '_n',
        numsCookieAuto: false,  //ÿҳ��ʾ�������Ƿ��Զ����ݵ�ǰ�ĸı���ı�
        numberOfButtons: 5,
        showButtons: true,
        showCurrentPageInfo:false,
        styleType: 0,            //��ʽ���ͣ������ṩ��������ʾģʽ
        ajax: {
            url: document.URL, //���̨�����URL
            type: 'get',       //����ģʽ
            data: {},          //��������
            cache: true,       //�Ƿ񻺴�
            before: null,      //��ҳ���ı�ǰ���õĺ���,ע�⣺����Ĳ����ǣ���ҳ�����в������ı�ǰ
            end: null	        //��ҳ���ı����õĺ���
        },
        ajaxContainer: null,
        pageContainer: null,
        onchange: null       //ע�⣺onchange�¼���Ĭ�ϵĴ����¼�����ֻ�����ѡһ,�����ȼ���
    },
    _pagers: [],
    pagerChange: function (key, page, nums, count) {
        var pager = $._pagers[key];
        if (pager) {
            var isChange = page && pager.page != page ||
			 nums && pager.nums != nums ||
			 count && pager.count;
            var dochange = function () {
                if (page && pager.page != page) {
                    pager.page = page;
                }
                if (nums && pager.nums != nums) {
                    pager.nums = nums;
                    if (pager.numsCookieAuto) {
                        $.cookie(pager.numsCookieName, nums);
                    }
                }
                if (count && pager.count != count) {
                    pager.count = count;
                }
                return pager;
            };

            if (isChange) {
                if ($.isFunction(pager.onchange)) {
                    dochange().reload().onchange.apply(pager);
                } else if (pager.ajaxContainer) {
                    if ($.isFunction(pager.ajax.start) && pager.ajax.start.call(pager, page) === false) {
                        return false;
                    }
                    dochange().reload();
                } else {
                    dochange().pageContainer.html(pager.toString());
                }
            }
        }
        return false;
    }
});