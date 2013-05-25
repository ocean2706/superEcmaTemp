using System;

namespace ReplaceInJavaFile
{
	public delegate String ReplaceCallback(String incomming, bool isRegexp,String search, String replacement);
	public class ReplaceListItem {
	}
	public class ReplaceItem
	{
		public ReplaceCallback DelegateReplace {get;set;}
		public ReplaceItem ()
		{
			
			
		}
		
		public List<ReplaceListItem> ReplaceList=new List<ReplaceListItem>();
	}
}

