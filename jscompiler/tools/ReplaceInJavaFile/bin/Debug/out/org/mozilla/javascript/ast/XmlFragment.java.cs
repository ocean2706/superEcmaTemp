/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Token;

/**
 * Abstract base type for components that comprise an {@link XmlLiteral}
 * object. Node type is {@link Token#XML}.<p>
 */
public abstract class XmlFragment : AstNode {

    {
        type = Token.XML;
    }

    public XmlFragment() {
    }

    public XmlFragment(int pos) {
        base(pos);
    }

    public XmlFragment(int pos, int len) {
        base(pos, len);
    }
}

}