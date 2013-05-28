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


		public static String SearchAndDestroyRecursive(String levelDir ){
			ListOfReplaceItem listOfSearches = new ListOfReplaceItem ();
			String outputDir = levelDir.Replace (sourceDir, sourceDir.Replace ("/src/", "/out/"));

			List<String> fi = new List<string> (Directory.EnumerateFiles(levelDir, "*.java",SearchOption.TopDirectoryOnly));
			fi.ForEach (delegate(String path){
				String incomming=File.ReadAllText(path);
				String gen=listOfSearches.DoRegexp(incomming);
				String outF=path.Replace (levelDir,outputDir);
				outF=Path.GetFileNameWithoutExtension(outF)+".cs";
				Directory.CreateDirectory(Path.GetDirectoryName(outF));
				File.WriteAllText(outf,gen);
			});
			// recursive execute oprerations
			fi = new List<String> (Directory.EnumerateDirectories(levelDir,"*",SearchOption.TopDirectoryOnly));
			fi.ForEach(delegate (String ddr){
				if(Path.GetFileName(ddr)!="." && Path.GetFileName(ddr)!=".."){
				SearchAndDestroyRecursive(ddr);
				}
			});
		}
		// recurse in directoryes


				public static String sourceDir="";
		public static void Main (string[] args)
		{

			//Console.WriteLine ("Hello World!");
			sourceDir = Environment.CurrentDirectory + "/src/";
			//String outDir = Environment.CurrentDirectory + "/out";
			//DirectoryInfo di = new DirectoryInfo (Environment.CurrentDirectory);
			SearchAndDestroyRecursive (sourceDir);


		}
	}
}
