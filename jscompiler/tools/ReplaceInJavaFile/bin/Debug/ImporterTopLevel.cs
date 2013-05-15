/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

// API class

//package org.mozilla.javascript;

/**
 * Class ImporterTopLevel
 *
 * This class defines a ScriptableObject that can be instantiated
 * as a top-level ("global") object to provide functionality similar
 * to Java's "import" statement.
 * <p>
 * This class can be used to create a top-level scope using the following code:
 * <pre>
 *  Scriptable scope = new ImporterTopLevel(cx);
 * </pre>
 * Then JavaScript code will have access to the following methods:
 * <ul>
 * <li>importClass - will "import" a class by making its unqualified name
 *                   available as a property of the top-level scope
 * <li>import//package - will "import" all the classes of the //package by
 *                     searching for unqualified names as classes qualified
 *                     by the given //package.
 * </ul>
 * The following code from the shell illustrates this use:
 * <pre>
 * js> importClass(java.io.File)
 * js> f = new File('help.txt')
 * help.txt
 * js> import//package(java.util)
 * js> v = new Vector()
 * []
 *
 * @author Norris Boyd
 */
public class ImporterTopLevel  :  TopLevel {
    static readonly long serialVersionUID = -9095380847465315412L;

    private static readonly Object IMPORTER_TAG = "Importer";

    public ImporterTopLevel() { }

    public ImporterTopLevel(Context cx) {
        this(cx, false);
    }

    public ImporterTopLevel(Context cx, bool  sealed)
    {
        initStandardObjects(cx, sealed);
    }

    //@Override
    public String getClassName()
    {
        return (topScopeFlag) ? "global" : "JavaImporter";
    }

    public static void init(Context cx, Scriptable scope, bool  sealed)
    {
        ImporterTopLevel obj = new ImporterTopLevel();
        obj.exportAsJSClass(MAX_PROTOTYPE_ID, scope, sealed);
    }

    public void initStandardObjects(Context cx, bool  sealed)
    {
        // Assume that Context.initStandardObjects initialize JavaImporter
        // property lazily so the above init call is not yet called
        cx.initStandardObjects(this, sealed);
        topScopeFlag = true;
        // If seal is true then exportAsJSClass(cx, seal) would seal
        // this obj. Since this is scope as well, it would not allow
        // to add variables.
        IdFunctionObject ctor = exportAsJSClass(MAX_PROTOTYPE_ID, this, false);
        if (sealed) {
            ctor.sealObject();
        }
        // delete "constructor" defined by exportAsJSClass so "constructor"
        // name would refer to Object.constructor
        // and not to JavaImporter.prototype.constructor.
        delete("constructor");
    }

    //@Override
    public bool  has(String name, Scriptable start) {
        return base.has(name, start)
               || get//packageProperty(name, start) != NOT_FOUND;
    }

    //@Override
    public Object get(String name, Scriptable start) {
        Object result = base.get(name, start);
        if (result != NOT_FOUND)
            return result;
        result = get//packageProperty(name, start);
        return result;
    }

    private Object get//packageProperty(String name, Scriptable start) {
        Object result = NOT_FOUND;
        Object[] elements;
        synchronized (imported//packages) {
            elements = imported//packages.toArray();
        }
        for (int i=0; i < elements.length; i++) {
            NativeJava//package p = (NativeJava//package) elements[i];
            Object v = p.getPkgProperty(name, start, false);
            if (v != null && !(v is NativeJava//package)) {
                if (result == NOT_FOUND) {
                    result = v;
                } else {
                    throw Context.reportRuntimeError2(
                        "msg.ambig.import", result.toString(), v.toString());
                }
            }
        }
        return result;
    }

    /**
     * @deprecated Kept only for compatibility.
     */
    public void import//package(Context cx, Scriptable thisObj, Object[] args,
                              Function funObj)
    {
        js_import//package(args);
    }

    private Object js_construct(Scriptable scope, Object[] args)
    {
        ImporterTopLevel result = new ImporterTopLevel();
        for (int i = 0; i != args.length; ++i) {
            Object arg = args[i];
            if (arg is NativeJavaClass) {
                result.importClass((NativeJavaClass)arg);
            } else if (arg is NativeJava//package) {
                result.import//package((NativeJava//package)arg);
            } else {
                throw Context.reportRuntimeError1(
                    "msg.not.class.not.pkg", Context.toString(arg));
            }
        }
        // set explicitly prototype and scope
        // as otherwise in top scope mode BaseFunction.construct
        // would keep them set to null. It also allow to use
        // JavaImporter without new and still get properly
        // initialized object.
        result.setParentScope(scope);
        result.setPrototype(this);
        return result;
    }

    private Object js_importClass(Object[] args)
    {
        for (int i = 0; i != args.length; i++) {
            Object arg = args[i];
            if (!(arg is NativeJavaClass)) {
                throw Context.reportRuntimeError1(
                    "msg.not.class", Context.toString(arg));
            }
            importClass((NativeJavaClass)arg);
        }
        return Undefined.instance;
    }

    private Object js_import//package(Object[] args)
    {
        for (int i = 0; i != args.length; i++) {
            Object arg = args[i];
            if (!(arg is NativeJava//package)) {
                throw Context.reportRuntimeError1(
                    "msg.not.pkg", Context.toString(arg));
            }
            import//package((NativeJava//package)arg);
        }
        return Undefined.instance;
    }

    private void import//package(NativeJava//package pkg)
    {
        if(pkg == null) {
            return;
        }
        synchronized (imported//packages) {
            for (int j = 0; j != imported//packages.size(); j++) {
                if (pkg.equals(imported//packages.get(j))) {
                    return;
                }
            }
            imported//packages.add(pkg);
        }
    }

    private void importClass(NativeJavaClass cl)
    {
        String s = cl.getClassObject().getName();
        String n = s.substring(s.lastIndexOf('.')+1);
        Object val = get(n, this);
        if (val != NOT_FOUND && val != cl) {
            throw Context.reportRuntimeError1("msg.prop.defined", n);
        }
        //defineProperty(n, cl, DONTENUM);
        put(n, this, cl);
    }

    //@Override
    protected void initPrototypeId(int id)
    {
        String s;
        int arity;
        switch (id) {
          case Id_constructor:   arity=0; s="constructor";   break;
          case Id_importClass:   arity=1; s="importClass";   break;
          case Id_import//package: arity=1; s="import//package"; break;
          default: throw new IllegalArgumentException(String.valueOf(id));
        }
        initPrototypeMethod(IMPORTER_TAG, id, s, arity);
    }

    //@Override
    public Object execIdCall(IdFunctionObject f, Context cx, Scriptable scope,
                             Scriptable thisObj, Object[] args)
    {
        if (!f.hasTag(IMPORTER_TAG)) {
            return base.execIdCall(f, cx, scope, thisObj, args);
        }
        int id = f.methodId();
        switch (id) {
          case Id_constructor:
            return js_construct(scope, args);

          case Id_importClass:
            return realThis(thisObj, f).js_importClass(args);

          case Id_import//package:
            return realThis(thisObj, f).js_import//package(args);
        }
        throw new IllegalArgumentException(String.valueOf(id));
    }

    private ImporterTopLevel realThis(Scriptable thisObj, IdFunctionObject f)
    {
        if (topScopeFlag) {
            // when used as top scope import//package and importClass are global
            // function that ignore thisObj
            return this;
        }
        if (!(thisObj is ImporterTopLevel))
            throw incompatibleCallError(f);
        return (ImporterTopLevel)thisObj;
    }

// #string_id_map#

    //@Override
    protected int findPrototypeId(String s)
    {
        int id;
// #generated# Last update: 2007-05-09 08:15:24 EDT
        L0: { id = 0; String X = null; int c;
            int s_length = s.length();
            if (s_length==11) {
                c=s.charAt(0);
                if (c=='c') { X="constructor";id=Id_constructor; }
                else if (c=='i') { X="importClass";id=Id_importClass; }
            }
            else if (s_length==13) { X="import//package";id=Id_import//package; }
            if (X!=null && X!=s && !X.equals(s)) id = 0;
            break L0;
        }
// #/generated#
        return id;
    }

    private static readonly int
        Id_constructor          = 1,
        Id_importClass          = 2,
        Id_import//package        = 3,
        MAX_PROTOTYPE_ID        = 3;

// #/string_id_map#

    private ObjArray imported//packages = new ObjArray();
    private bool  topScopeFlag;
}
