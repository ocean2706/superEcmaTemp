/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Token;

/**
 * Throw statement.  Node type is {@link Token#THROW}.<p>
 *
 * <pre><i>ThrowStatement</i> :
 *      <b>throw</b> [<i>no LineTerminator here</i>] Expression ;</pre>
 */
public class ThrowStatement : AstNode {

    private AstNode expression;

    {
        type = Token.THROW;
    }

    public ThrowStatement() {
    }

    public ThrowStatement(int pos) {
        base(pos);
    }

    public ThrowStatement(int pos, int len) {
        base(pos, len);
    }

    public ThrowStatement(AstNode expr) {
        setExpression(expr);
    }

    public ThrowStatement(int pos, AstNode expr) {
        base(pos, expr.getLength());
        setExpression(expr);
    }

    public ThrowStatement(int pos, int len, AstNode expr) {
        base(pos, len);
        setExpression(expr);
    }

    /**
     * Returns the expression being thrown
     */
    public AstNode getExpression() {
        return expression;
    }

    /**
     * Sets the expression being thrown, and sets its parent
     * to this node.
     * @throws IllegalArgumentException} if expression is {@code null}
     */
    public void setExpression(AstNode expression) {
        assertNotNull(expression);
        this.expression = expression;
        expression.setParent(this);
    }

    //@Override
    public String toSource(int depth) {
        StringBuilder sb = new StringBuilder();
        sb.Append(makeIndent(depth));
        sb.Append("throw");
        sb.Append(" ");
        sb.Append(expression.toSource(0));
        sb.Append(";\n");
        return sb.toString();
    }

    /**
     * Visits this node, then the thrown expression.
     */
    //@Override
    public void visit(NodeVisitor v) {
        if (v.visit(this)) {
            expression.visit(v);
        }
    }
}

}