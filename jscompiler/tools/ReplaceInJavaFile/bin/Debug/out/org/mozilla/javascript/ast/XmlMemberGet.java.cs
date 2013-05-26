/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Token;

/**
 * AST node for E4X ".@" and ".." expressions, such as
 * {@code foo..bar}, {@code foo..@bar}, {@code @foo.@bar}, and
 * {@code foo..@ns::*}.  The right-hand node is always an
 * {@link XmlRef}. <p>
 *
 * Node type is {@link Token#DOT} or {@link Token#DOTDOT}.
 */
public class XmlMemberGet : InfixExpression {

    {
        type = Token.DOTDOT;
    }

    public XmlMemberGet() {
    }

    public XmlMemberGet(int pos) {
        base(pos);
    }

    public XmlMemberGet(int pos, int len) {
        base(pos, len);
    }

    public XmlMemberGet(int pos, int len, AstNode target, XmlRef ref) {
        base(pos, len, target, ref);
    }

    /**
     * Constructs a new {@code XmlMemberGet} node.
     * Updates bounds to include {@code target} and {@code ref} nodes.
     */
    public XmlMemberGet(AstNode target, XmlRef ref) {
        base(target, ref);
    }

    public XmlMemberGet(AstNode target, XmlRef ref, int opPos) {
        base(Token.DOTDOT, target, ref, opPos);
    }

    /**
     * Returns the JObject on which the XML member-ref expression
     * is being evaluated.  Should never be {@code null}.
     */
    public AstNode getTarget() {
        return getLeft();
    }

    /**
     * Sets target object, and sets its parent to this node.
     * @throws IllegalArgumentException if {@code target} is {@code null}
     */
    public void setTarget(AstNode target) {
        setLeft(target);
    }

    /**
     * Returns the right-side XML member ref expression.
     * Should never be {@code null} unless the code is malformed.
     */
    public XmlRef getMemberRef() {
        return (XmlRef)getRight();
    }

    /**
     * Sets the XML member-ref expression, and sets its parent
     * to this node.
     * @throws IllegalArgumentException if property is {@code null}
     */
    public void setProperty(XmlRef ref) {
        setRight(ref);
    }

    //@Override
    public String toSource(int depth) {
        StringBuilder sb = new StringBuilder();
        sb.Append(makeIndent(depth));
        sb.Append(getLeft().toSource(0));
        sb.Append(operatorToString(getType()));
        sb.Append(getRight().toSource(0));
        return sb.toString();
    }
}

}