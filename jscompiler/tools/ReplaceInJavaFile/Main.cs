using System;
using System.Collections.Generic;
using System.IO;

namespace ReplaceInJavaFile
{

	class MainClass
	{
		public static Dictionary<String,String> ToReplace=new Dictionary<String,String>(){
			{" get("," Get("},
			{" set("," Set("},
			{" put("," Put("},
			{

		};
		public static void Main (string[] args)
		{
			//Console.WriteLine ("Hello World!");
			String outDir = Environment.CurrentDirectory + "/../out";
			DirectoryInfo di = new DirectoryInfo (Environment.CurrentDirectory);
			List<FileInfo> fi = new List<FileInfo> (di.GetFiles ("*.java", SearchOption.AllDirectories));
			Directory.CreateDirectory (outDir);
			fi.ForEach (delegate(FileInfo obj) {

				String content = File.ReadAllText (obj.FullName);
		
				foreach (String key in ToReplace.Keys) {
					content = content.Replace (key, ToReplace [key]);
				}
				String outfile = outDir + "/" + Path.GetFileName (obj.FullName);
				File.WriteAllText (outfile, content);
				Console.WriteLine ("Generated " + outfile);
			});

		}
	}
}
