/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

// API class

namespace org.mozilla.javascript.serialize { //

using java.io.*;

using org.mozilla.javascript.*;

/**
 * Class ScriptableInputStream is used to read in a JavaScript
 * JObject or function previously serialized with a ScriptableOutputStream.
 * References to names in the exclusion list
 * replaced with references to the top-level scope specified during
 * creation of the ScriptableInputStream.
 *
 * @author Norris Boyd
 */

public class ScriptableInputStream : ObjectInputStream {

    /**
     * Create a ScriptableInputStream.
     * @param in the InputStream to read from.
     * @param scope the top-level scope to create the JObject in.
     */
    public ScriptableInputStream(InputStream in, Scriptable scope)
        throws IOException
    {
        base(in);
        this.scope = scope;
        enableResolveObject(true);
        Context cx = Context.getCurrentContext();
        if (cx != null) {
            this.classLoader = cx.getApplicationClassLoader();
        }
    }

    //@Override
    protected Class<?> resolveClass(ObjectStreamClass desc)
        throws IOException, ClassNotFoundException
    {
        String name = desc.getName();
        if (classLoader != null) {
            try {
                return classLoader.loadClass(name);
            } catch (ClassNotFoundException ex) {
                // fall through to default loading
            }
        }
        return base.resolveClass(desc);
    }

    //@Override
    protected Object resolveObject(Object obj)
        throws IOException
    {
        if (obj is ScriptableOutputStream.PendingLookup) {
            String name = ((ScriptableOutputStream.PendingLookup)obj).getName();
            obj = ScriptableOutputStream.lookupQualifiedName(scope, name);
            if (obj == Scriptable.NOT_FOUND) {
                throw new IOException("Object " + name + " not found upon " +
                                      "deserialization.");
            }
        }else if (obj is UniqueTag) {
            obj = ((UniqueTag)obj).readResolve();
        }else if (obj is Undefined) {
            obj = ((Undefined)obj).readResolve();
        }
        return obj;
    }

    private Scriptable scope;
    private ClassLoader classLoader;
}

}