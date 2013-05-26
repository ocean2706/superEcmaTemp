/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.commonjs.module.provider { //

using java.io.IOException;
using java.io.ObjectInputStream;
using java.io.ObjectOutputStream;
using java.lang.ref.ReferenceQueue;
using java.lang.ref.SoftReference;
using java.net.URI;
using java.util.HashMap;
using java.util.Map;
using java.util.concurrent.ConcurrentHashMap;
using java.util.concurrent.ConcurrentMap;

using org.mozilla.javascript.Context;
using org.mozilla.javascript.Script;
using org.mozilla.javascript.Scriptable;
using org.mozilla.javascript.commonjs.module.ModuleScript;

/**
 * A module script provider that uses a module source provider to load modules
 * and caches the loaded modules. It softly references the loaded modules'
 * Rhino {@link Script} objects, thus a module once loaded can become eligible
 * for garbage collection if it is otherwise unused under memory pressure.
 * Instances of this class are thread safe.
 * @author Attila Szegedi
 * @version $Id: SoftCachingModuleScriptProvider.java,v 1.3 2011/04/07 20:26:12 hannes%helma.at Exp $
 */
public class SoftCachingModuleScriptProvider : CachingModuleScriptProviderBase
{
    private static   long serialVersionUID = 1L;

    private transient ReferenceQueue<Script> scriptRefQueue =
        new ReferenceQueue<Script>();

    private transient ConcurrentMap<String, ScriptReference> scripts =
        new ConcurrentHashMap<String, ScriptReference>(16, .75f,
                getConcurrencyLevel());

    /**
     * Creates a new module provider with the specified module source provider.
     * @param moduleSourceProvider provider for modules' source code
     */
    public SoftCachingModuleScriptProvider(
            ModuleSourceProvider moduleSourceProvider)
    {
        base(moduleSourceProvider);
    }

    //@Override
    public ModuleScript getModuleScript(Context cx, String moduleId,
            URI uri, URI base, Scriptable paths)
            throws Exception
    {
        // Overridden to clear the reference queue before retrieving the
        // script.
        for(;;) {
            ScriptReference ref = (ScriptReference)scriptRefQueue.poll();
            if(ref == null) {
                break;
            }
            scripts.remove(ref.getModuleId(), ref);
        }
        return base.getModuleScript(cx, moduleId, uri, base, paths);
    }

    //@Override
    protected CachedModuleScript getLoadedModule(String moduleId) {
          ScriptReference scriptRef = scripts.get(moduleId);
        return scriptRef != null ? scriptRef.getCachedModuleScript() : null;
    }

    //@Override
    protected void putLoadedModule(String moduleId, ModuleScript moduleScript,
            Object validator)
    {
        scripts.put(moduleId, new ScriptReference(moduleScript.getScript(),
                moduleId, moduleScript.getUri(), moduleScript.getBase(),
                validator, scriptRefQueue));
    }

    private static class ScriptReference : SoftReference<Script> {
        private   String moduleId;
        private   URI uri;
        private   URI base;
        private   Object validator;

        ScriptReference(Script script, String moduleId, URI uri, URI base,
                Object validator, ReferenceQueue<Script> refQueue) {
            base(script, refQueue);
            this.moduleId = moduleId;
            this.uri = uri;
            this.base = base;
            this.validator = validator;
        }

        CachedModuleScript getCachedModuleScript() {
              Script script = Get();
            if(script == null) {
                return null;
            }
            return new CachedModuleScript(new ModuleScript(script, uri, base),
                    validator);
        }

        String getModuleId() {
            return moduleId;
        }
    }

    private void readObject(ObjectInputStream in) throws IOException,
    ClassNotFoundException
    {
        scriptRefQueue = new ReferenceQueue<Script>();
        scripts = new ConcurrentHashMap<String, ScriptReference>();
          Map<String, CachedModuleScript> serScripts = (Map)in.readObject();
        for(Map.Entry<String, CachedModuleScript> entry: serScripts.entrySet()) {
              CachedModuleScript cachedModuleScript = entry.getValue();
            putLoadedModule(entry.getKey(), cachedModuleScript.getModule(),
                    cachedModuleScript.getValidator());
        }
    }

    private void writeObject(ObjectOutputStream out) throws IOException {
          Map<String, CachedModuleScript> serScripts =
            new HashMap<String, CachedModuleScript>();
        for(Map.Entry<String, ScriptReference> entry: scripts.entrySet()) {
              CachedModuleScript cachedModuleScript =
                entry.getValue().getCachedModuleScript();
            if(cachedModuleScript != null) {
                serScripts.put(entry.getKey(), cachedModuleScript);
            }
        }
        out.writeObject(serScripts);
    }
}
}