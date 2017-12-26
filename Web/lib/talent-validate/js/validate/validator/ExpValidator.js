tt.ExpV = tt.BV.ext({
			set : function(exp) {
				this.exp = exp;
				return this;
			},
			v : function(str, index, es, f) {
				try {
					var _exp = tt.getExp(this.exp, index);
					this.i18ps[0] = _exp;
					return eval(_exp);
				} catch (e) {
					return false;
				}
			}
		});