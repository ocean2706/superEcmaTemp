/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Token;

/**
 * New expression. Node type is {@link Token#NEW}.<p>
 *
 * <pre><i>NewExpression</i> :
 *      MemberExpression
 *      <b>new</b> NewExpression</pre>
 *
 * This node is a subtype of {@link FunctionCall}, mostly for internal code
 * sharing.  Structurally a {@code NewExpression} node is very similar to a
 * {@code FunctionCall}, so it made a certain amount of sense.
 */
public class NewExpression : FunctionCall {

    private ObjectLiteral initializer;

    {
        type = Token.NEW;
    }

    public NewExpression() {
    }

    public NewExpression(int pos) {
        base(pos);
    }

    public NewExpression(int pos, int len) {
        base(pos, len);
    }

    /**
     * Returns initializer object, if any.
     * @return extra initializer object-literal expression, or {@code null} if
     * not specified.
     */
    public ObjectLiteral getInitializer() {
      return initializer;
    }

    /**
     * Sets initializer object.  Rhino supports an experimental syntax
     * of the form {@code new expr [ ( arglist ) ] [initializer]},
     * in which initializer is an JObject literal that is used to set
     * additional properties on the newly-created {@code expr} object.
     *
     * @param initializer extra initializer object.
     * Can be {@code null}.
     */
    public void setInitializer(ObjectLiteral initializer) {
      this.initializer = initializer;
      if (initializer != null)
          initializer.setParent(this);
    }

    //@Override
    public String toSource(int depth) {
        StringBuilder sb = new StringBuilder();
        sb.Append(makeIndent(depth));
        sb.Append("new ");
        sb.Append(target.toSource(0));
        sb.Append("(");
        if (arguments != null) {
            printList(arguments, sb);
        }
        sb.Append(")");
        if (initializer != null) {
            sb.Append(" ");
            sb.Append(initializer.toSource(0));
        }
        return sb.toString();
    }

    /**
     * Visits this node, the target, and each argument.  If there is
     * a trailing initializer node, visits that last.
     */
    //@Override
    public void visit(NodeVisitor v) {
        if (v.visit(this)) {
            target.visit(v);
            for (AstNode arg : getArguments()) {
                arg.visit(v);
            }
            if (initializer != null) {
                initializer.visit(v);
            }
        }
    }
}

}