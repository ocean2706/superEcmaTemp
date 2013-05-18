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
			{"@Override","//@Override"},
			{"synchronized",@"//synchronized"}


		};
		public static void Main (string[] args)
		{
			//Console.WriteLine ("Hello World!");
			String outDir = Environment.CurrentDirectory + "/../out";
			//DirectoryInfo di = new DirectoryInfo (Environment.CurrentDirectory);
			List<String> fi =new List<string>( Directory.EnumerateFiles(Environment.CurrentDirectory, "*.java",SearchOption.AllDirectories));
			Directory.CreateDirectory (outDir);
			fi.ForEach (delegate(String path) {

				String content = File.ReadAllText (path);
		
				foreach (String key in ToReplace.Keys) {
					content = content.Replace (key, ToReplace[key]);
				}
				String outfile = outDir + "/" + Path.GetFileName (path);
				File.WriteAllText (outfile, content);
				Console.WriteLine ("Generated " + outfile);
			});

		}
	}
}
