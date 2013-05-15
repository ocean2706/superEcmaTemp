using System;

namespace java.lang.emul.net
{
	public class URL:UriBuilder
	{
		public URLConnection openConnection ()
		{
			throw new NotImplementedException ();
		}
		public URL (string path):base(path)
		{
//			throw new NotImplementedException ();
		}

		public URL ()
		{
		}
	}
}

