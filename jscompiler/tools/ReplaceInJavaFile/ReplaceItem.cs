using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReplaceInJavaFile
{
	public delegate String ReplaceCallback(String incomming,ReplaceListItem r);


    public class CommonReplaceDelegate
    {
        public static String Replace1(String incomming,ReplaceListItem r)
        {


            if(!r.IsRegexp && incomming.ToLower().Contains(r.ToReplaceInMatch)){
                return incomming.Replace(r.ToReplaceInMatch,r.Replacement); // simple replacement
            }
            String ret = incomming;
            MatchCollection foundList = Regex.Matches(incomming, search);
            foreach (Match m in foundList)
            {
                String toReplace = m.Value.Replace(iSearch, replacement);
                ret = ret.Replace(m.Value, toReplace);
            }
            return ret;
        }
    }
	public class ReplaceListItem {
        public ReplaceCallback ExecuteCallBack = null;
        public String Regexp { get; set; }
        public String ToReplaceInMatch { get; set; }
        public String Replacement { get; set; }

        public String Prefix { get; set; }
        public String Suffix { get; set; }
        /// <summary>
        /// if prefix suffix then the replaced expression will be left intact 
        /// but prefix and suffix will be added
        /// </summary>
        public bool IsPrefixSuffix = false;
        public bool IsRegexp = false;
	}
	public class ReplaceItem
	{
		
		public ReplaceItem ()
		{
			
			
		}
		
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

