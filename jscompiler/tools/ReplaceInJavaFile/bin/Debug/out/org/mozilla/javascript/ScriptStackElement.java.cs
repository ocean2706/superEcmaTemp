/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript { //

using java.io.Serializable;

/**
 * This class represents an element on the script execution stack.
 * @see RhinoException#getScriptStack()
 * @author Hannes Wallnoefer
 * @since 1.7R3
 */
public public  class ScriptStackElement implements Serializable {

    static   long serialVersionUID = -6416688260860477449L;
    
    public   String fileName;
    public   String functionName;
    public   int lineNumber;

    public ScriptStackElement(String fileName, String functionName, int lineNumber) {
        this.fileName = fileName;
        this.functionName = functionName;
        this.lineNumber = lineNumber;
    }

    public String toString() {
        StringBuilder sb = new StringBuilder();
        renderMozillaStyle(sb);
        return sb.toString();
    }

    /**
     * Render stack element in Java-inspired style:
     * <code>    at fileName:lineNumber (functionName)</code>
     * @param sb the StringBuilder to append to
     */
    public void renderJavaStyle(StringBuilder sb) {
        sb.Append("\tat ").Append(fileName);
        if (lineNumber > -1) {
            sb.Append(':').Append(lineNumber);
        }
        if (functionName != null) {
            sb.Append(" (").Append(functionName).Append(')');
        }
    }

    /**
     * Render stack element in Mozilla/Firefox style:
     * <code>functionName()@fileName:lineNumber</code>
     * @param sb the StringBuilder to append to
     */
    public void renderMozillaStyle(StringBuilder sb) {
        if (functionName != null) {
            sb.Append(functionName).Append("()");
        }
        sb.Append('@').Append(fileName);
        if (lineNumber > -1) {
            sb.Append(':').Append(lineNumber);
        }
    }

}

}