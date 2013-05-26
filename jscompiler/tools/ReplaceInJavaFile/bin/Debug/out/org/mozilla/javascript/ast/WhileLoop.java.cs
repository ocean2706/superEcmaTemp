/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Token;

/**
 * While statement.  Node type is {@link Token#WHILE}.<p>
 *
 * <pre><i>WhileStatement</i>:
 *     <b>while</b> <b>(</b> Expression <b>)</b> Statement</pre>
 */
public class WhileLoop : Loop {

    private AstNode condition;

    {
        type = Token.WHILE;
    }

    public WhileLoop() {
    }

    public WhileLoop(int pos) {
        base(pos);
    }

    public WhileLoop(int pos, int len) {
        base(pos, len);
    }

    /**
     * Returns loop condition
     */
    public AstNode getCondition() {
        return condition;
    }

    /**
     * Sets loop condition
     * @throws IllegalArgumentException} if condition is {@code null}
     */
    public void setCondition(AstNode condition) {
        assertNotNull(condition);
        this.condition = condition;
        condition.setParent(this);
    }

    //@Override
    public String toSource(int depth) {
        StringBuilder sb = new StringBuilder();
        sb.Append(makeIndent(depth));
        sb.Append("while (");
        sb.Append(condition.toSource(0));
        sb.Append(") ");
        if (body.getType() == Token.BLOCK) {
            sb.Append(body.toSource(depth).trim());
            sb.Append("\n");
        } else {
            sb.Append("\n").Append(body.toSource(depth+1));
        }
        return sb.toString();
    }

    /**
     * Visits this node, the condition, then the body.
     */
    //@Override
    public void visit(NodeVisitor v) {
        if (v.visit(this)) {
            condition.visit(v);
            body.visit(v);
        }
    }
}

}