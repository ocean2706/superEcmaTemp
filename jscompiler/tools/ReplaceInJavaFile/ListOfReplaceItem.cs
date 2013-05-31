using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReplaceInJavaFile
{
	
	public class ListOfReplaceItem
	{
		
		public  List<ReplaceListItem> ReplaceList = new List<ReplaceListItem> ();

		public ListOfReplaceItem(){
		ReplaceList.Clear ();
			ReplaceList.AddRange (new ReplaceListItem[]{
				new ReplaceListItem(){
					ExecuteCallBack=CommonReplaceDelegate.Replace1,
					IsRegexp=true,
					Regexp=@"(\s?|^)package\s",
					ToReplaceInMatch="package",
					ReplaceWith="namespace ",
				},
			});
		}

        public  String DoRegexp(String incomming)
        {
            String outc = incomming;
            ReplaceList.ForEach(delegate(ReplaceListItem r)
            {
               outc= r.ExecuteCallBack(outc, r);
            });
            return outc;
        }
	}
}
