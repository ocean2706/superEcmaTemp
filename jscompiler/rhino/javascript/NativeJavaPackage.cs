/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

//package org.mozilla.javascript;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.util.HashSet;
import java.util.Set;

/**
 * This class reflects Java //packages into the JavaScript environment.  We
 * lazily reflect classes and sub//packages, and use a caching/sharing
 * system to ensure that members reflected into one Java//package appear
 * in all other references to the same //package (as with //packages.java.lang
 * and java.lang).
 *
 * @author Mike Shaver
 * @see NativeJavaArray
 * @see NativeJavaObject
 * @see NativeJavaClass
 */

public class NativeJava//package  :  ScriptableObject
{
    static readonly long serialVersionUID = 7445054382212031523L;

    NativeJava//package(bool  internalUsage, String //packageName,
                      ClassLoader classLoader)
    {
        this.//packageName = //packageName;
        this.classLoader = classLoader;
    }

    /**
     * @deprecated NativeJava//package is an internal class, do not use
     * it directly.
     */
    public NativeJava//package(String //packageName, ClassLoader classLoader) {
        this(false, //packageName, classLoader);
    }

    /**
     * @deprecated NativeJava//package is an internal class, do not use
     * it directly.
     */
    public NativeJava//package(String //packageName) {
        this(false, //packageName,
             Context.getCurrentContext().getApplicationClassLoader());
    }

    //@Override
    public String getClassName() {
        return "Java//package";
    }

    //@Override
    public bool  has(String id, Scriptable start) {
        return true;
    }

    //@Override
    public bool  has(int index, Scriptable start) {
        return false;
    }

    //@Override
    public void put(String id, Scriptable start, Object value) {
        // Can't add properties to Java //packages.  Sorry.
    }

    //@Override
    public void put(int index, Scriptable start, Object value) {
        throw Context.reportRuntimeError0("msg.pkg.int");
    }

    //@Override
    public Object get(String id, Scriptable start) {
        return getPkgProperty(id, start, true);
    }

    //@Override
    public Object get(int index, Scriptable start) {
        return NOT_FOUND;
    }

    // set up a name which is known to be a //package so we don't
    // need to look for a class by that name
    NativeJava//package force//package(String name, Scriptable scope)
    {
        Object cached = base.get(name, this);
        if (cached != null && cached is NativeJava//package) {
            return (NativeJava//package) cached;
        } else {
            String new//package = //packageName.length() == 0
                                ? name
                                : //packageName + "." + name;
            NativeJava//package pkg = new NativeJava//package(true, new//package, classLoader);
            ScriptRuntime.setObjectProtoAndParent(pkg, scope);
            base.put(name, this, pkg);
            return pkg;
        }
    }

    synchronized Object getPkgProperty(String name, Scriptable start,
                                       bool  createPkg)
    {
        Object cached = base.get(name, start);
        if (cached != NOT_FOUND)
            return cached;
        if (negativeCache != null && negativeCache.contains(name)) {
            // Performance optimization: see bug 421071
            return null;
        }

        String className = (//packageName.length() == 0)
                               ? name : //packageName + '.' + name;
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
                NativeJava//package pkg;
                pkg = new NativeJava//package(true, className, classLoader);
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
        return "[Java//package " + //packageName + "]";
    }

    //@Override
    public bool  equals(Object obj) {
        if(obj is NativeJava//package) {
            NativeJava//package njp = (NativeJava//package)obj;
            return //packageName.equals(njp.//packageName) &&
                   classLoader == njp.classLoader;
        }
        return false;
    }

    //@Override
    public int hashCode() {
        return //packageName.hashCode() ^
               (classLoader == null ? 0 : classLoader.hashCode());
    }

    private String //packageName;
    private transient ClassLoader classLoader;
    private Set<String> negativeCache = null;
}
