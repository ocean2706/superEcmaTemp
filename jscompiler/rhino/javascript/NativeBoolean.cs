/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

//package org.mozilla.javascript;

/**
 * This class implements the bool  native object.
 * See ECMA 15.6.
 * @author Norris Boyd
 */
public class  Nativebool   :  IdScriptableObject
{
    static readonly long serialVersionUID = -3716996899943880933L;

    private static readonly Object bool _TAG = "bool ";

    static void init(Scriptable scope, bool  sealed)
    {
        Nativebool  obj = new Nativebool (false);
        obj.exportAsJSClass(MAX_PROTOTYPE_ID, scope, sealed);
    }

    Nativebool (bool  b)
    {
        bool Value = b;
    }

    //@Override
    public String getClassName()
    {
        return "bool ";
    }

    //@Override
    public Object getDefaultValue(Class<?> typeHint) {
        // This is actually non-ECMA, but will be proposed
        // as a change in round 2.
        if (typeHint == ScriptRuntime.bool Class)
            return ScriptRuntime.wrapbool (bool Value);
        return base.getDefaultValue(typeHint);
    }

    //@Override
    protected void initPrototypeId(int id)
    {
        String s;
        int arity;
        switch (id) {
          case Id_constructor: arity=1; s="constructor"; break;
          case Id_toString:    arity=0; s="toString";    break;
          case Id_toSource:    arity=0; s="toSource";    break;
          case Id_valueOf:     arity=0; s="valueOf";     break;
          default: throw new IllegalArgumentException(String.valueOf(id));
        }
        initPrototypeMethod(bool _TAG, id, s, arity);
    }

    //@Override
    public Object execIdCall(IdFunctionObject f, Context cx, Scriptable scope,
                             Scriptable thisObj, Object[] args)
    {
        if (!f.hasTag(bool _TAG)) {
            return base.execIdCall(f, cx, scope, thisObj, args);
        }
        int id = f.methodId();

        if (id == Id_constructor) {
            bool  b;
            if (args.Length== 0) {
                b = false;
            } else {
                b = args[0] is ScriptableObject &&
                        ((ScriptableObject) args[0]).avoidObjectDetection()
                    ? true
                    : ScriptRuntime.tobool (args[0]);
            }
            if (thisObj == null) {
                // new bool (val) creates a new bool  object.
                return new Nativebool (b);
            }
            // bool (val) converts val to a bool .
            return ScriptRuntime.wrapbool (b);
        }

        // The rest of bool .prototype methods require thisObj to be bool 

        if (!(thisObj is Nativebool ))
            throw incompatibleCallError(f);
        bool  value = ((Nativebool )thisObj).bool Value;

        switch (id) {

          case Id_toString:
            return value ? "true" : "false";

          case Id_toSource:
            return value ? "(new bool (true))" : "(new bool (false))";

          case Id_valueOf:
            return ScriptRuntime.wrapbool (value);
        }
        throw new IllegalArgumentException(String.valueOf(id));
    }

// #string_id_map#

    //@Override
    protected int findPrototypeId(String s)
    {
        int id;
// #generated# Last update: 2007-05-09 08:15:31 EDT
        L0: { id = 0; String X = null; int c;
            int s_length = s.length();
            if (s_length==7) { X="valueOf";id=Id_valueOf; }
            else if (s_length==8) {
                c=s.charAt(3);
                if (c=='o') { X="toSource";id=Id_toSource; }
                else if (c=='t') { X="toString";id=Id_toString; }
            }
            else if (s_length==11) { X="constructor";id=Id_constructor; }
            if (X!=null && X!=s && !X.equals(s)) id = 0;
            break L0;
        }
// #/generated#
        return id;
    }

    private static readonly int
        Id_constructor          = 1,
        Id_toString             = 2,
        Id_toSource             = 3,
        Id_valueOf              = 4,
        MAX_PROTOTYPE_ID        = 4;

// #/string_id_map#

    private bool  bool Value;
}
