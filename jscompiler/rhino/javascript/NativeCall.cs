using System;
using System.Data;
using java.lang.emul;
namespace ro.integrator.javascript.rhino.core{
public class NativeCall {
		public NativeFunction function {
			get;
			set;
		}

		public object[] originalArgs {
			get;
			set;
		}

		public Scriptable getParentScope ()
		{
			throw new NotImplementedException ();
		}

	public NativeCall(){
	//@do constructor code
}
}
}
