using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReplaceInJavaFile
{
	public delegate String ReplaceCallback(String incomming,ReplaceListItem r);


    public class CommonReplaceDelegate
    {
		/// <summary>
		/// default delegate for replacement
		/// </summary>
		/// <param name="incomming">Incomming.</param>
		/// <param name="r">The red component.</param>
        public static String Replace1(String incomming,ReplaceListItem r)
        {


            if((!r.IsRegexp) && (incomming.ToLower().Contains(r.ToReplaceInMatch.ToLower()))){
               incomming= incomming.Replace(r.ToReplaceInMatch,r.ReplaceWith); // simple replacement
				incomming=incomming.Replace(r.ToReplaceInMatch.ToLower(),r.ReplaceWith);
				return incomming;
            }
            String ret = incomming;
            MatchCollection foundList = Regex.Matches(ret, r.Regexp);
			List<String> replaced=new List<String>();
			if (!r.IsPrefixSuffix) {
            foreach (Match m in foundList)
            {
					if(!replaced.Contains(m.Value.Trim ())){
					// replace in founded string 
					String toReplace = m.Value.Replace (r.ToReplaceInMatch, r.ReplaceWith);
					ret = ret.Replace (m.Value, toReplace);
						replaced.Add(m.Value);
					}
				}
			}
			else {

				// in this seq we just add prefix and suffix to the original string..so it can mach multiple times...
				//
				//this must be fixed
				foreach (Match m in foundList)
				{
					if (!replaced.Contains (m.Value.Trim ())) {
						ret = r.Prefix + m.Value + r.Suffix;
						replaced.Add(m.Value);
					}
				}
            }
            return ret;
        }
    }
	public class ReplaceListItem {
        public ReplaceCallback ExecuteCallBack = null;
        public String Regexp { get; set; }
        public String ToReplaceInMatch { get; set; }
        public String ReplaceWith { get; set; }

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

