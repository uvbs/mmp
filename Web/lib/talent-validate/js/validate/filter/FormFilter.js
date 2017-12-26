tt.f.Form = tt.f.Dft.ext({
	f:function(e)
	{
		return e.form && this.p.ttCons(e.form.getAttribute('name'));
	}
});