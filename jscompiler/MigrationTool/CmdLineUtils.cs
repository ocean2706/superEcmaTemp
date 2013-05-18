using System;
using System.Collections.Generic;

namespace MigrationTool
{
	public class CmdLineUtils
	{
		public CmdLineUtils ()
		{
		}
		public static String GetArgument(String name){
			System.Collections.Generic.List<String> lines=new List<String>(Environment.GetCommandLineArgs());
			String val = "";
			lines.ForEach (delegate(String l) {
				if(l.ToLower().StartsWith(name.ToLower()+"=")){
					val= l.ToLower().Replace(name.ToLower()+"=").Trim ('"').Trim ("'").Trim ();
				}else if(l.ToLower()==name.ToLower()){
					val="True";
				}
			});
		}
	}
}

