using System;
using System.IO;

namespace java.lang.emul.io
{
	public class InputStream:Stream{
		#region implemented abstract members of System.IO.Stream
		public override void Flush ()
		{
			throw new System.NotImplementedException ();
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			throw new System.NotImplementedException ();
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new System.NotImplementedException ();
		}

		public override void SetLength (long value)
		{
			throw new System.NotImplementedException ();
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new System.NotImplementedException ();
		}

		public override bool CanRead {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public override bool CanSeek {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public override bool CanWrite {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public override long Length {
			get {
				throw new System.NotImplementedException ();
			}
		}

		public override long Position {
			get {
				throw new System.NotImplementedException ();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}
		#endregion
	}
	public class FileInputStream:InputStream
	{
		public FileInputStream (FileInfo file)
		{
			throw new NotImplementedException ();
		}
	}
}

