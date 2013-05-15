//
//
//namespace org.mozilla
//{
//	public class SourceReader
//	{
//		public SourceReader ()
//		{
//		}
//	}
//}
using System;
using java.lang.emul; //URL
using System.IO;
using java.lang.emul.io;
using java.lang.emul.net;
using org.mozilla.javascript.commonjs.module.provider;
using System.Text;


namespace org.mozilla.javascript.tools
{

 //*//package org.mozilla.javascript.tools;

//import java.io.File;
//import java.io.FileInputStream;
//import java.io.IOException;
//import java.io.InputStream;
//import java.net.MalformedURLException;
//import java.net.URL;
//import java.net.URLConnection;
//
//import org.mozilla.javascript.Kit;
//import org.mozilla.javascript.commonjs.module.provider.ParsedContentType;


/**
 * @author Attila Szegedi
 * @version $Id: SourceReader.java,v 1.2 2010/02/15 19:31:17 szegedia%freemail.hu Exp $
 */
public class SourceReader
{
    public static URL toUrl(String path) {
        // Assume path is URL if it contains a colon and there are at least
        // 2 characters in the protocol part. The later allows under Windows
        // to interpret paths with driver letter as file, not URL.
        if (path.IndexOf(':') >= 2) {
            try {
                return new URL(path);
            } catch (Exception ex) {
                // not a URL
            }
        }
        return null;
    }

    public static Object readFileOrUrl (String path, bool  convertToString,
            String defaultEncoding)
		{
			URL url = toUrl (path);
			Stream ins = null;
			int capacityHint = 0;
			String encoding="";
			String contentType="";
			byte[] data=new byte[]{};
			try {
				if (url == null) {
					FileInfo file = new FileInfo (path);
					contentType = encoding = null;
					capacityHint = (int)file.Length;
					ins = new FileInputStream (file);
				} else {
					URLConnection uc = url.openConnection ();
					ins = uc.getInputStream ();
					if (convertToString) {
						ParsedContentType pct = new ParsedContentType (uc.getContentType ());
						contentType = pct.getContentType ();
						encoding = pct.getEncoding ();
					} else {
						contentType = encoding = null;
					}
					capacityHint = uc.getContentLength ();
					// Ignore insane values for Content-Length
					if (capacityHint > (1 << 20)) {
						capacityHint = -1;
					}
				}
				if (capacityHint <= 0) {
					capacityHint = 4096;
				}

				data = Kit.readStream (ins, capacityHint);
			} catch (Exception ex) {
			} finally {
				if (ins != null) {
					ins.Close ();
				}
			}

			Object result;
			if (!convertToString) {
				result = data;
			} else {
				if (encoding == null) {
					// None explicitly specified in Content-type header. Use RFC-4329
					// 4.2.2 section to autodetect
					if (data.Length> 3 && data [0] == -1 && data [1] == -2 && data [2] == 0 && data [3] == 0) {
						encoding = "UTF-32LE";
					} else if (data.Length> 3 && data [0] == 0 && data [1] == 0 && data [2] == -2 && data [3] == -1) {
						encoding = "UTF-32BE";
					} else if (data.Length> 2 && data [0] == -17 && data [1] == -69 && data [2] == -65) {
						encoding = "UTF-8";
					} else if (data.Length> 1 && data [0] == -1 && data [1] == -2) {
						encoding = "UTF-16LE";
					} else if (data.Length> 1 && data [0] == -2 && data [1] == -1) {
						encoding = "UTF-16BE";
					} else {
						// No autodetect. See if we have explicit value on command line
						encoding = defaultEncoding;
						if (encoding == null) {
							// No explicit encoding specification
							if (url == null) {
								// Local files default to system encoding
								// encoding = System.getProperty("file.encoding");
								encoding = "UTF-8";
							} else if (contentType != null && contentType.StartsWith ("application/")) {
								// application/* types default to UTF-8
								encoding = "UTF-8";
							} else {
								// text/* MIME types default to US-ASCII
								encoding = "US-ASCII";
							}
						}
					}
				}
				// String strResult = Encoding.UTF8. new String(data, encoding);
				String strResult = Encoding.UTF8.GetString (data);//  = "UTF-8";
				// Skip BOM
				if (strResult.Length> 0 && strResult [0] == '\uFEFF') {
					strResult = strResult.Substring (1);
				}
				result = strResult;
			}
			return result;
		}
	}
}

