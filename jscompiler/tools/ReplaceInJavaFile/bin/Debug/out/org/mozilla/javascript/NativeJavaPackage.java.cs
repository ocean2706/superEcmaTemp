/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript { //

using java.io.IOException;
using java.io.ObjectInputStream;
using java.util.HashSet;
using java.util.Set;

/**
 * This class reflects Java packages into the JavaScript environment.  We
 * lazily reflect classes and subpackages, and use a caching/sharing
 * system to ensure that members reflected into one JavaPackage appear
 * in all other references to the same namespace (as with Packages.java.lang
 * and java.lang).
 *
 * @author Mike Shaver
 * @see NativeJavaArray
 * @see NativeJavaObject
 * @see NativeJavaClass
 */

public class NativeJavaPackage : ScriptableObject
{
    static   long serialVersionUID = 7445054382212031523L;

    NativeJavaPackage(boolean internalUsage, String packageName,
                      ClassLoader classLoader)
    {
        this.packageName = packageName;
        this.classLoader = classLoader;
    }

    /**
     * @deprecated NativeJavaPackage is an internal class, do not use
     * it directly.
     */
    public NativeJavaPackage(String packageName, ClassLoader classLoader) {
        this(false, packageName, classLoader);
    }

    /**
     * @deprecated NativeJavaPackage is an internal class, do not use
     * it directly.
     */
    public NativeJavaPackage(String packageName) {
        this(false, packageName,
             Context.getCurrentContext().getApplicationClassLoader());
    }

    //@Override
    public String getClassName() {
        return "JavaPackage";
    }

    //@Override
    public boolean has(String id, Scriptable start) {
        return true;
    }

    //@Override
    public boolean has(int index, Scriptable start) {
        return false;
    }

    //@Override
    public void put(String id, Scriptable start, Object value) {
        // Can't add properties to Java packages.  Sorry.
    }

    //@Override
    public void put(int index, Scriptable start, Object value) {
        throw Context.reportRuntimeError0("msg.pkg.int");
    }

    //@Override
    public Object Get(String id, Scriptable start) {
        return getPkgProperty(id, start, true);
    }

    //@Override
    public Object Get(int index, Scriptable start) {
        return NOT_FOUND;
    }

    // set up a name which is known to be a namespace so we don't
    // need to look for a class by that name
    NativeJavaPackage forcePackage(String name, Scriptable scope)
    {
        Object cached = base.get(name, this);
        if (cached != null && cached is NativeJavaPackage) {
            return (NativeJavaPackage) cached;
        } else {
            String newPackage = packageName.Length;//--length() == 0
                                ? name
                                : packageName + "." + name;
            NativeJavaPackage pkg = new NativeJavaPackage(true, newPackage, classLoader);
            ScriptRuntime.setObjectProtoAndParent(pkg, scope);
            base.put(name, this, pkg);
            return pkg;
        }
    }

    synchronized Object getPkgProperty(String name, Scriptable start,
                                       boolean createPkg)
    {
        Object cached = base.get(name, start);
        if (cached != NOT_FOUND)
            return cached;
        if (negativeCache != null && negativeCache.contains(name)) {
            // Performance optimization: see bug 421071
            return null;
        }

        String className = (packageName.Length;//--length() == 0)
                               ? name : packageName + '.' + name;
        Context cx = Context.getContext();
        ClassShutter shutter = cx.getClassShutter();
        Scriptable newValue = null;
        if (shutter == null || shutter.visibleToScripts(className)) {
            Class<?> cl = null;
            if (classLoader != null) {
                cl = Kit.classOrNull(classLoader, className);
            } else {
                cl = Kit.classOrNull(className);
            }
            if (cl != null) {
                WrapFactory wrapFactory = cx.getWrapFactory();
                newValue = wrapFactory.wrapJavaClass(cx, getTopLevelScope(this), cl);
                newValue.setPrototype(getPrototype());
            }
        }
        if (newValue == null) {
            if (createPkg) {
                NativeJavaPackage pkg;
                pkg = new NativeJavaPackage(true, className, classLoader);
                ScriptRuntime.setObjectProtoAndParent(pkg, getParentScope());
                newValue = pkg;
            } else {
                // add to negative cache
                if (negativeCache == null)
                    negativeCache = new HashSet<String>();
                negativeCache.add(name);
            }
        }
        if (newValue != null) {
            // Make it available for fast lookup and sharing of
            // lazily-reflected constructors and static members.
            base.put(name, start, newValue);
        }
        return newValue;
    }

    //@Override
    public Object getDefaultValue(Class<?> ignored) {
        return toString();
    }

    private void readObject(ObjectInputStream in) throws IOException, ClassNotFoundException {
        in.defaultReadObject();
        this.classLoader = Context.getCurrentContext().getApplicationClassLoader();
    }

    //@Override
    public String toString() {
        return "[JavaPackage " + packageName + "]";
    }

    //@Override
    public boolean equals(Object obj) {
        if(obj is NativeJavaPackage) {
            NativeJavaPackage njp = (NativeJavaPackage)obj;
            return packageName.equals(njp.packageName) &&
                   classLoader == njp.classLoader;
        }
        return false;
    }

    //@Override
    public int hashCode() {
        return packageName.hashCode() ^
               (classLoader == null ? 0 : classLoader.hashCode());
    }

    private String packageName;
    private transient ClassLoader classLoader;
    private Set<String> negativeCache = null;
}

}