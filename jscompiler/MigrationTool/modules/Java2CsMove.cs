using System;
using System.IO;
using System.Collections.Generic;

namespace MigrationTool
{
	public class Java2CsMove
	{
		public Java2CsMove ()
		{
		}

		public static void MoveJavaFiles(String fromDirectory, String destDirectory){
			DirectoryInfo di=new DirectoryInfo(fromDirectory);
			Directory.CreateDirectory(destDirectory);
			List<FileInfo> fl=new List<FileInfo>( di.EnumerateFiles("*.java"));
			fl.ForEach(delegate(FileInfo obj) {
				try{
				File.Copy(obj.FullName,destDirectory+"/"+Path.GetFileNameWithoutExtension(obj.FullName)+".cs");
				}catch{
				}
			});

			System.Console.WriteLine("MoveJavaFiles Completed");
		}
	}
}

