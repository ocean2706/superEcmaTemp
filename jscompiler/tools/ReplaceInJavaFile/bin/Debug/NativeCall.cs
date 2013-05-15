using System;

namespace org.mozilla.javascript
{
	public  class NativeCall : IdScriptableObject
{
    //static readonly long serialVersionUID = -7471457301304454454L;

    private static readonly Object CALL_TAG = "Call";

    static void init(Scriptable scope, bool IsSealed)
    {
        NativeCall obj = new NativeCall();
        obj.exportAsJSClass(MAX_PROTOTYPE_ID, scope, IsSealed);
    }

    NativeCall() { }

    NativeCall(NativeFunction function, Scriptable scope, Object[] args)
    {
        this.function = function;

        setParentScope(scope);
        // leave prototype null

        this.originalArgs = (args == null) ? ScriptRuntime.emptyArgs : args;

        // initialize values of arguments
        int paramAndVarCount = function.getParamAndVarCount();
        int paramCount = function.getParamCount();
        if (paramAndVarCount != 0) {
            for (int i = 0; i < paramCount; ++i) {
                String name = function.getParamOrVarName(i);
                Object val = i < args.Length? args[i]
                                             : Undefined.instance;
                defineProperty(name, val, PERMANENT);
            }
        }

        // initialize "arguments" property but only if it was not overridden by
        // the parameter with the same name
        if (!base.has("arguments", this)) {
            defineProperty("arguments", new Arguments(this), PERMANENT);
        }

        if (paramAndVarCount != 0) {
            for (int i = paramCount; i < paramAndVarCount; ++i) {
                String name = function.getParamOrVarName(i);
                if (!base.has(name, this)) {
                    if (function.getParamOrVarConst(i)){
                        defineProperty(name, Undefined.instance, CONST);
						}
                    else{
                        defineProperty(name, Undefined.instance, PERMANENT);
						}
                }
            }
        }
    }

    //////@Override 
    public String getClassName()
    {
        return "Call";
    }

    //////@Override 
    protected int findPrototypeId(String s)
    {
        return s.equals("constructor") ? Id_constructor : 0;
    }

    ////@Override 
    protected void initPrototypeId(int id)
    {
        String s;
        int arity;
        if (id == Id_constructor) {
            arity=1; s="constructor";
        } else {
            throw new IllegalArgumentException(String.valueOf(id));
        }
        initPrototypeMethod(CALL_TAG, id, s, arity);
    }

    ////@Override 
    public Object execIdCall(IdFunctionObject f, Context cx, Scriptable scope,
                             Scriptable thisObj, Object[] args)
    {
        if (!f.hasTag(CALL_TAG)) {
            return base.execIdCall(f, cx, scope, thisObj, args);
        }
        int id = f.methodId();
        if (id == Id_constructor) {
            if (thisObj != null) {
                throw Context.reportRuntimeError1("msg.only.from.new", "Call");
            }
            ScriptRuntime.checkDeprecated(cx, "Call");
            NativeCall result = new NativeCall();
            result.setPrototype(getObjectPrototype(scope));
            return result;
        }
        throw new IllegalArgumentException(String.valueOf(id));
    }

		public const int        Id_constructor   = 1;
        public const int MAX_PROTOTYPE_ID = 1;

    NativeFunction function;
    Object[] originalArgs;

     NativeCall parentActivationCall;
}
}

