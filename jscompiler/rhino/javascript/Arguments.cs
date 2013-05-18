using System;
using System.Data;
using java.lang.emul;
namespace ro.integrator.javascript.rhino.core{
public class Arguments {
	public Arguments(){
	//@do constructor code
}

		NativeCall activation {
			get;
			set;
		}

		void setParentScope (Scriptable parent)
		{
			throw new NotImplementedException ();
		}

		void setPrototype (object obj)
		{
			throw new NotImplementedException ();
		}

		object[] args {
			get;
			set;
		}

		int lengthObj {
			get;
			set;
		}

		NativeFunction calleeObj {
			get;
			set;
		}

		Scriptable getTopLevelScope (Scriptable parent)
		{
			throw new NotImplementedException ();
		}

		object getProperty (Scriptable topLevel, string @object)
		{
			throw new NotImplementedException ();
		}

		object constructor {
			get;
			set;
		}

		@null callerObj {
			get;
			set;
		}

		object NOT_FOUND {
			get;
			set;
		}

		public Arguments(NativeCall activation){
			this.activation = activation;
		
		Scriptable parent = activation.getParentScope ();
			setParentScope (parent);
			setPrototype (ScriptableObject.getObjectPrototype (parent));
		
			args = activation.originalArgs;
			lengthObj = Integer.valueOf (args.Length);
		
		NativeFunction f = activation.function;
			calleeObj = f;
		
			Scriptable topLevel = getTopLevelScope (parent);
			constructor = getProperty (topLevel, "Object");
		
		int version = f.getLanguageVersion ();
		if (version <= Context.VERSION_1_3
			&& version != Context.VERSION_DEFAULT) {
				callerObj = null;
		} else {
				callerObj = NOT_FOUND;
		}
	}
}
}
