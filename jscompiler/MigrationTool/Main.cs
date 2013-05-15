using System;

namespace MigrationTool
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!,Press a key");
			System.Console.ReadKey();
			Java2CsMove.MoveJavaFiles("/bkp/home/george/Desktop/jsc-src/eclipse-workspace/mozilla-rhino/src/org/mozilla/javascript/","/bkp/home/george/Desktop/jsc-src/jscompiler/rhino/javascript");
		}
	}
}
