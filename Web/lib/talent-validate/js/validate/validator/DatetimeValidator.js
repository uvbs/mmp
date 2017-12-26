tt.DV = tt.BV.ext({
    set:function(pattern)
    {
    	this.pattern = pattern;
    	return this;
    },
    /**
     * 说明:日期的验证代码是直接摘自validation-framework.js,但略作修改
     */
    v:function(s)
    {
        if (!s)
        {
           return true;//不验证为空的串
        }
        this.initI18n(tt.i18Datetime);
        
        var dateP = this.pattern;//params[0];
        dateP = dateP ? dateP : "yyyy-mm-dd";
        this.i18ps[0] = dateP;
        this.i18ps[1] = dateP.replace("yyyy","2011").replace("mm","08").replace("dd","08");
        
        var MONTH = "mm";
        var DAY = "dd";
        var YEAR = "yyyy";
        var orderMonth = dateP.indexOf(MONTH);
        var orderDay = dateP.indexOf(DAY);
        var orderYear = dateP.indexOf(YEAR);
        var f = true;
        var dateReg = null;
        
        if ((orderDay < orderYear && orderDay > orderMonth)) {
            var iDelim1 = orderMonth + MONTH.length;
               var iDelim2 = orderDay + DAY.length;
               var delim1 = dateP.substring(iDelim1, iDelim1 + 1);
               var delim2 = dateP.substring(iDelim2, iDelim2 + 1);
               if (iDelim1 == orderDay && iDelim2 == orderYear) {
                dateReg = /^(\\d{2})(\\d{2})(\\d{4})$/;
               } else if (iDelim1 == orderDay) {
                dateReg = new RegExp("^(\\d{2})(\\d{2})[" + delim2 + "](\\d{4})$");
               } else if (iDelim2 == orderYear) {
                dateReg = new RegExp("^(\\d{2})[" + delim1 + "](\\d{2})(\\d{4})$");
               } else {
                dateReg = new RegExp("^(\\d{2})[" + delim1 + "](\\d{2})[" + delim2 + "](\\d{4})$");
               }
        
               var matched = dateReg.exec(s);
               if(matched != null) {
                if (!this._myc(matched[2], matched[1], matched[3])) {
                       f =  false;
                }
               } else {
                   f =  false;
               }
           } else if ((orderMonth < orderYear && orderMonth > orderDay)) { 
            var iDelim1 = orderDay + DAY.length;
               var iDelim2 = orderMonth + MONTH.length;
               var delim1 = dateP.substring(iDelim1, iDelim1 + 1);
               var delim2 = dateP.substring(iDelim2, iDelim2 + 1);
               if (iDelim1 == orderMonth && iDelim2 == orderYear) {
                dateReg = /^(\\d{2})(\\d{2})(\\d{4})$/;
               } else if (iDelim1 == orderMonth) {
                dateReg = new RegExp("^(\\d{2})(\\d{2})[" + delim2 + "](\\d{4})$");
               } else if (iDelim2 == orderYear) {
                dateReg = new RegExp("^(\\d{2})[" + delim1 + "](\\d{2})(\\d{4})$");
               } else {
                dateReg = new RegExp("^(\\d{2})[" + delim1 + "](\\d{2})[" + delim2 + "](\\d{4})$");
               }
               var matched = dateReg.exec(s);
            if(matched != null) {
                if (!this._myc(matched[1], matched[2], matched[3])) {
                    f = false;
                   }
               } else {
                f = false;
               }
           } else if ((orderMonth > orderYear && orderMonth < orderDay)) {
            var iDelim1 = orderYear + YEAR.length;
               var iDelim2 = orderMonth + MONTH.length;
               var delim1 = dateP.substring(iDelim1, iDelim1 + 1);
        
               var delim2 = dateP.substring(iDelim2, iDelim2 + 1);
               if (iDelim1 == orderMonth && iDelim2 == orderDay) {
                dateReg = /^(\\d{4})(\\d{2})(\\d{2})$/;
               } else if (iDelim1 == orderMonth) {
                dateReg = new RegExp("^(\\d{4})(\\d{2})[" + delim2 + "](\\d{2})$");
               } else if (iDelim2 == orderDay) {
                dateReg = new RegExp("^(\\d{4})[" + delim1 + "](\\d{2})(\\d{2})$");
               } else {
                dateReg = new RegExp("^(\\d{4})[" + delim1 + "](\\d{2})[" + delim2 + "](\\d{2})$");
               }
            var matched = dateReg.exec(s);
               if(matched != null) {
                if (!this._myc(matched[3], matched[2], matched[1])) {
                       f =  false;
                   }
               } else {
                   f =  false;
               }
           } else {
               f =  false;
           }
        return f;
    },
    _myc:function(d, m, y)
    {
		if (d < 1 || d > 31)
		{
			return false;
		}
		if (m < 1 || m > 12)
		{
			return false;
		}
		
		if ([4,6,9,11].ttCons(m) && (d > 30))
		{
			return false;
		}
		if (m == 2)
		{
			var leap = (y % 4 == 0 && (y % 100 != 0 || y % 400 == 0));
			if (d>29 || (d == 29 && !leap))
			{
				return false;
			} 
	    }
		return true;
	}
});