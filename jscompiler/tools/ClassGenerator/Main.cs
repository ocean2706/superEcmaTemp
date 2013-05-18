using System;
using System.IO;
using System.Collections.Generic;

namespace ClassGenerator
{
	class MainClass
	{

		static String List=@"Arguments.java
BaseFunction.java
BoundFunction.java
Callable.java
ClassCache.java
ClassShutter.java
CodeGenerator.java
CompilerEnvirons.java
ConsString.java
ConstProperties.java
ContextAction.java
ContextFactory.java
Context.java
ContextListener.java
ContinuationPending.java
Decompiler.java
DefaultErrorReporter.java
DefiningClassLoader.java
Delegator.java
DToA.java
EcmaError.java
ErrorReporter.java
EvaluatorException.java
Evaluator.java
Function.java
FunctionObject.java
GeneratedClassLoader.java
Icode.java
IdFunctionCall.java
IdFunctionObject.java
IdScriptableObject.java
ImporterTopLevel.java
InterfaceAdapter.java
InterpretedFunction.java
InterpreterData.java
Interpreter.java
IRFactory.java
JavaAdapter.java
JavaMembers.java
JavaScriptException.java
Kit.java
LazilyLoadedCtor.java
MemberBox.java
NativeArray.java
NativeBoolean.java
NativeCall.java
NativeContinuation.java
NativeDate.java
NativeError.java
NativeFunction.java
NativeGenerator.java
NativeGlobal.java
NativeIterator.java
NativeJavaArray.java
NativeJavaClass.java
NativeJavaConstructor.java
NativeJavaMethod.java
NativeJavaObject.java
NativeJavaPackage.java
NativeJavaTopPackage.java
NativeJSON.java
NativeMath.java
NativeNumber.java
NativeObject.java
NativeScript.java
NativeString.java
NativeWith.java
Node.java
NodeTransformer.java
ObjArray.java
ObjToIntMap.java
Parser.java
PolicySecurityController.java
RefCallable.java
Ref.java
RegExpProxy.java
RhinoException.java
RhinoSecurityManager.java
Scriptable.java
ScriptableObject.java
Script.java
ScriptRuntime.java
ScriptStackElement.java
SecureCaller.java
SecurityController.java
SecurityUtilities.java
SpecialRef.java
Synchronizer.java
Token.java
TokenStream.java
TopLevel.java
UintMap.java
Undefined.java
UniqueTag.java
VMBridge.java
WrapFactory.java
WrappedException.java
Wrapper.java";
		static List<String> files = new List<String> ();
		public static void Main (string[] args)
		{
			File.WriteAllText("data",List);
			files= new List<string>(File.ReadAllLines ("data"));
			files.ForEach (delegate(String fname) {
				String clas=Path.GetFileNameWithoutExtension(fname);
				String code=@"using System;
using System.Data;
using java.lang.emul;
namespace ro.integrator.javascript.rhino.core{
public class "+clas+@" {
	public "+clas+@"(){
	//@do constructor code
}
}
}
";

				File.WriteAllText(clas+".cs",code);
			});



		}
	}
}
