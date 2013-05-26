using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace TestRegex
{
	internal class MainClass
	{
		public MainClass()
		{
		}

		public static void Main(string[] args)
		{
			Console.WriteLine("Enter data to match");
			string str = Console.ReadLine().Trim();
			string str1 = "/* This Source Code Form is subject to the terms of the Mozilla Public\n * License, v. 2.0. If a copy of the MPL was not distributed with this\n * file, You can obtain one at http://mozilla.org/MPL/2.0/. */\n\npackage org.mozilla.javascript;\n\nimport java.io.Serializable;\n\n\n\tbyte[] b \n\tbyte() x\n\tbbbbyte yz\n\n/**\n * This class represents an element on the script execution stack.\n * @see RhinoException#getScriptStack()\n * @author Hannes Wallnoefer\n * @since 1.7R3\n */";
			string str2 = str;
			Console.WriteLine("Enter regexp");
			string str3 = Console.ReadLine().Trim();
			for (Match i = Regex.Match(str1, str3); i.Success; i = i.NextMatch())
			{
				IEnumerator enumerator = i.Groups.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Group current = (Group)enumerator.Current;
						Console.WriteLine(string.Concat("found ", current.ToString(), i.ToString()));
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					IDisposable disposable1 = disposable;
					if (disposable != null)
					{
						disposable1.Dispose();
					}
				}
			}
		}
	}
}