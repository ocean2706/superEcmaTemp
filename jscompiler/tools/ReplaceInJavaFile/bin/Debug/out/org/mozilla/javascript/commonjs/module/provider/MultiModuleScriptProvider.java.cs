/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.commonjs.module.provider { //

using java.net.URI;
using java.util.LinkedList;
using java.util.List;

using org.mozilla.javascript.Context;
using org.mozilla.javascript.Scriptable;
using org.mozilla.javascript.commonjs.module.ModuleScript;
using org.mozilla.javascript.commonjs.module.ModuleScriptProvider;

/**
 * A multiplexer for module script providers.
 * @author Attila Szegedi
 * @version $Id: MultiModuleScriptProvider.java,v 1.4 2011/04/07 20:26:12 hannes%helma.at Exp $
 */
public class MultiModuleScriptProvider implements ModuleScriptProvider
{
    private   ModuleScriptProvider[] providers;

    /**
     * Creates a new multiplexing module script provider tht gathers the
     * specified providers
     * @param providers the providers to multiplex.
     */
    public MultiModuleScriptProvider(Iterable<? : ModuleScriptProvider> providers) {
          List<ModuleScriptProvider> l = new LinkedList<ModuleScriptProvider>();
        for (ModuleScriptProvider provider : providers) {
            l.add(provider);
        }
        this.providers = l.toArray(new ModuleScriptProvider[l.size()]);
    }

    public ModuleScript getModuleScript(Context cx, String moduleId, URI uri,
                                        URI base, Scriptable paths) throws Exception {
        for (ModuleScriptProvider provider : providers) {
              ModuleScript script = provider.getModuleScript(cx, moduleId,
                    uri, base, paths);
            if(script != null) {
                return script;
            }
        }
        return null;
    }
}

}