/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Token;

/**
 * AST node representing an E4X {@code foo.(bar)} query expression.
 * The node type (operator) is {@link Token#DOTQUERY}.
 * Its {@code getLeft} node is the target ("foo" in the example),
 * and the {@code getRight} node is the filter expression node.<p>
 *
 * This class exists separately from {@link InfixExpression} largely because it
 * has different printing needs.  The position of the left paren is just after
 * the dot (operator) position, and the right paren is the   position in the
 * bounds of the node.  If the right paren is missing, the node ends at the end
 * of the filter expression.
 */
public class XmlDotQuery : InfixExpression {

    private int rp = -1;

    {
        type = Token.DOTQUERY;
    }

    public XmlDotQuery() {
    }

    public XmlDotQuery(int pos) {
        base(pos);
    }

    public XmlDotQuery(int pos, int len) {
        base(pos, len);
    }

    /**
     * Returns right-paren position, -1 if missing.<p>
     *
     * Note that the left-paren is automatically the character
     * immediately after the "." in the operator - no whitespace is
     * permitted between the dot and lp by the scanner.
     */
    public int getRp() {
        return rp;
    }

    /**
     * Sets right-paren position
     */
    public void setRp(int rp) {
        this.rp = rp;
    }

    //@Override
    public String toSource(int depth) {
        StringBuilder sb = new StringBuilder();
        sb.Append(makeIndent(depth));
        sb.Append(getLeft().toSource(0));
        sb.Append(".(");
        sb.Append(getRight().toSource(0));
        sb.Append(")");
        return sb.toString();
    }
}

}