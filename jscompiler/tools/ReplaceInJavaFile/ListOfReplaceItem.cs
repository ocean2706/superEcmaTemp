using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReplaceInJavaFile
{
	
	public class ListOfReplaceItem
	{
		
		public List<ReplaceListItem> ReplaceList=new List<ReplaceListItem>();

        public String DoRegexp(String incomming)
        {
            String outc = incomming;
            ReplaceList.ForEach(delegate(ReplaceListItem r)
            {
                r.ExecuteCallBack(incomming, r);
            });
            return outc;
        }
	}
}
