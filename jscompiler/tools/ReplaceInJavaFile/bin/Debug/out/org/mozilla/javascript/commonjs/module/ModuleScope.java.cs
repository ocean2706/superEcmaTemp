/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.commonjs.module { //

using org.mozilla.javascript.Scriptable;
using org.mozilla.javascript.TopLevel;

using java.net.URI;

/**
 * A top-level module scope. This class provides methods to retrieve the
 * module's source and base URIs in order to resolve relative module IDs
 * and check sandbox constraints.
 */
public class ModuleScope : TopLevel {

    private static   long serialVersionUID = 1L;

    private   URI uri;
    private   URI base;

    public ModuleScope(Scriptable prototype, URI uri, URI base) {
        this.uri = uri;
        this.base = base;
        setPrototype(prototype);
        cacheBuiltins();
    }

    public URI getUri() {
        return uri;
    }

    public URI getBase() {
        return base;
    }
}

}