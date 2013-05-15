using System;
using java.lang.emul;

namespace org.mozilla.javascript.commonjs.module.provider
{
//	/*
//	 * public public class  ParsedContentType implements Serializable
//{
//    private static readonly long serialVersionUID = 1L;
//
//    private String contentType;
//    private String encoding;

//    /**
//     * Creates a new parsed content type.
//     * @param mimeType the full MIME type; typically the value of the
//     * "Content-Type" header of some MIME-compliant message. Can be null.
//     */
//    public ParsedContentType(String mimeType) {
//        String contentType = null;
//        String encoding = null;
//        if(mimeType != null) {
//            StringTokenizer tok = new StringTokenizer(mimeType, ";");
//            if(tok.hasMoreTokens()) {
//                contentType = tok.nextToken().trim();
//                while(tok.hasMoreTokens()) {
//                    String param = tok.nextToken().trim();
//                    if(param.startsWith("charset=")) {
//                        encoding = param.substring(8).trim();
//                        int l = encoding.length();
//                        if(l > 0) {
//                            if(encoding.charAt(0) == '"') {
//                                encoding = encoding.substring(1);
//                            }
//                            if(encoding.charAt(l - 1) == '"') {
//                                encoding = encoding.substring(0, l - 1);
//                            }
//                        }
//                        break;
//                    }
//                }
//            }
//        }
//        this.contentType = contentType;
//        this.encoding = encoding;
//    }
//
//    /**
//     * Returns the content type (without charset declaration) of the MIME type.
//     * @return the content type (without charset declaration) of the MIME type.
//     * Can be null if the MIME type was null.
//     */
//    public String getContentType() {
//        return contentType;
//    }
//
//    /**
//     * Returns the character encoding of the MIME type.
//     * @return the character encoding of the MIME type. Can be null when it is
//     * not specified.
//     */
//    public String getEncoding() {
//        return encoding;
//    }
//}*/
	public class ParsedContentType:Serializable
	{
		string contentType {
			get;
			set;
		}

		string encoding {
			get;
			set;
		}
		public ParsedContentType(String mimeType) {
        String contentType = null;
        String encoding = null;
        if(mimeType != null) {
            StringTokenizer tok = new StringTokenizer(mimeType, ";");
            if(tok.hasMoreTokens()) {
                contentType = tok.nextToken().Trim();
                while(tok.hasMoreTokens()) {
                    String param = tok.nextToken().Trim();
                    if(param.StartsWith("charset=")) {
                        encoding = param.Substring(8).Trim();
                        int l = encoding.Length;
                        if(l > 0) {
                            if(encoding[0] == '"') {
                                encoding = encoding.Substring(1);
                            }
                            if(encoding[l - 1] == '"') {
                                encoding = encoding.Substring(0, l - 1);
                            }
                        }
                        break;
                    }
                }
            }
        }
			this.contentType = contentType;
			this.encoding = encoding;
    }
		public string getEncoding ()
		{
			return encoding;
		}

		public string getContentType ()
		{
			return contentType;
		}



		public ParsedContentType ()
		{
		}
	}
}

