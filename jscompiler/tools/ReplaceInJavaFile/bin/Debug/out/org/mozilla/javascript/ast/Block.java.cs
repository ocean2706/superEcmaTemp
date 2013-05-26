/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Node;
using org.mozilla.javascript.Token;

/**
 * A block statement delimited by curly braces.  The node position is the
 * position of the open-curly, and the length : to the position of
 * the close-curly.  Node type is {@link Token#BLOCK}.
 *
 * <pre><i>Block</i> :
 *     <b>{</b> Statement* <b>}</b></pre>
 */
public class Block : AstNode {

    {
        this.type = Token.BLOCK;
    }

    public Block() {
    }

    public Block(int pos) {
        base(pos);
    }

    public Block(int pos, int len) {
        base(pos, len);
    }

    /**
     * Alias for {@link #addChild}.
     */
    public void addStatement(AstNode statement) {
        addChild(statement);
    }

    //@Override
    public String toSource(int depth) {
        StringBuilder sb = new StringBuilder();
        sb.Append(makeIndent(depth));
        sb.Append("{\n");
        for (Node kid : this) {
            sb.Append(((AstNode)kid).toSource(depth+1));
        }
        sb.Append(makeIndent(depth));
        sb.Append("}\n");
        return sb.toString();
    }

    //@Override
    public void visit(NodeVisitor v) {
        if (v.visit(this)) {
            for (Node kid : this) {
                ((AstNode)kid).visit(v);
            }
        }
    }
}

}