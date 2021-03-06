/* -*- Mode: java; tab-width: 4; indent-tabs-mode: 1; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript { //

using org.mozilla.javascript.json.JsonParser;

using java.util.Stack;
using java.util.Collection;
using java.util.Iterator;
using java.util.Arrays;
using java.util.List;
using java.util.LinkedList;

/**
 * This class implements the JSON native object.
 * See ECMA 15.12.
 * @author Matthew Crumley, Raphael Speyer
 */
public public  class NativeJSON : IdScriptableObject
{
    static   long serialVersionUID = -4567599697595654984L;

    private static   Object JSON_TAG = "JSON";

    private static   int MAX_STRINGIFY_GAP_LENGTH = 10;

    static void init(Scriptable scope, boolean sealed)
    {
        NativeJSON obj = new NativeJSON();
        obj.activatePrototypeMap(MAX_ID);
        obj.setPrototype(getObjectPrototype(scope));
        obj.setParentScope(scope);
        if (sealed) { obj.sealObject(); }
        ScriptableObject.defineProperty(scope, "JSON", obj,
                                        ScriptableObject.DONTENUM);
    }

    private NativeJSON()
    {
    }

    //@Override
    public String getClassName() { return "JSON"; }

    //@Override
    protected void initPrototypeId(int id)
    {
        if (id <= LAST_METHOD_ID) {
            String name;
            int arity;
            switch (id) {
              case Id_toSource:  arity = 0; name = "toSource";  break;
              case Id_parse:     arity = 2; name = "parse";     break;
              case Id_stringify: arity = 3; name = "stringify"; break;
              default: throw new IllegalStateException(String.valueOf(id));
            }
            initPrototypeMethod(JSON_TAG, id, name, arity);
        } else {
            throw new IllegalStateException(String.valueOf(id));
        }
    }

    //@Override
    public Object execIdCall(IdFunctionObject f, Context cx, Scriptable scope,
                             Scriptable thisObj, Object[] args)
    {
        if (!f.hasTag(JSON_TAG)) {
            return base.execIdCall(f, cx, scope, thisObj, args);
        }
        int methodId = f.methodId();
        switch (methodId) {
            case Id_toSource:
                return "JSON";

            case Id_parse: {
                String jtext = ScriptRuntime.toString(args, 0);
                Object reviver = null;
                if (args.length > 1) {
                    reviver = args[1];
                }
                if (reviver is Callable) {
                  return parse(cx, scope, jtext, (Callable) reviver);
                } else {
                  return parse(cx, scope, jtext);
                }
            }

            case Id_stringify: {
                Object value = null, replacer = null, space = null;
                switch (args.length) {
                    default:
                    case 3: space = args[2];
                    case 2: replacer = args[1];
                    case 1: value = args[0];
                    case 0:
                }
                return stringify(cx, scope, value, replacer, space);
            }

            default: throw new IllegalStateException(String.valueOf(methodId));
        }
    }

    private static Object parse(Context cx, Scriptable scope, String jtext) {
      try {
        return new JsonParser(cx, scope).parseValue(jtext);
      } catch (JsonParser.ParseException ex) {
        throw ScriptRuntime.constructError("SyntaxError", ex.getMessage());
      }
    }

    public static Object parse(Context cx, Scriptable scope, String jtext,
                               Callable reviver)
    {
      Object unfiltered = parse(cx, scope, jtext);
      Scriptable root = cx.newObject(scope);
      root.put("", root, unfiltered);
      return walk(cx, scope, reviver, root, "");
    }

    private static Object walk(Context cx, Scriptable scope, Callable reviver,
                               Scriptable holder, Object name)
    {
          Object property;
        if (name is Number) {
            property = holder.get( ((Number) name).intValue(), holder);
        } else {
            property = holder.get( ((String) name), holder);
        }

        if (property is Scriptable) {
            Scriptable val = ((Scriptable) property);
            if (val is NativeArray) {
                long len = ((NativeArray) val).getLength();
                for (long i = 0; i < len; i++) {
                    // indices greater than MAX_INT are represented as strings
                    if (i > Integer.MAX_VALUE) {
                        String id = Long.toString(i);
                        Object newElement = walk(cx, scope, reviver, val, id);
                        if (newElement == Undefined.instance) {
                            val.delete(id);
                        } else {
                            val.put(id, val, newElement);
                        }
                    } else {
                        int idx = (int) i;
                        Object newElement = walk(cx, scope, reviver, val, idx);
                        if (newElement == Undefined.instance) {
                            val.delete(idx);
                        } else {
                            val.put(idx, val, newElement);
                        }
                    }
                }
            } else {
                Object[] keys = val.getIds();
                for (Object p : keys) {
                    Object newElement = walk(cx, scope, reviver, val, p);
                    if (newElement == Undefined.instance) {
                        if (p is Number)
                          val.delete(((Number) p).intValue());
                        else
                          val.delete((String) p);
                    } else {
                        if (p is Number)
                          val.put(((Number) p).intValue(), val, newElement);
                        else
                          val.put((String) p, val, newElement);
                    }
                }
            }
        }

        return reviver.call(cx, scope, holder, new Object[] { name, property });
    }

    private static String repeat(char c, int count) {
      char chars[] = new char[count];
      Arrays.fill(chars, c);
      return new String(chars);
    }

    private static class StringifyState {
        StringifyState(Context cx, Scriptable scope, String indent, String gap,
                       Callable replacer, List<Object> propertyList,
                       Object space)
        {
            this.cx = cx;
            this.scope = scope;

            this.indent = indent;
            this.gap = gap;
            this.replacer = replacer;
            this.propertyList = propertyList;
            this.space = space;
        }

        Stack<Scriptable> stack = new Stack<Scriptable>();
        String indent;
        String gap;
        Callable replacer;
        List<Object> propertyList;
        Object space;

        Context cx;
        Scriptable scope;
    }

    public static Object stringify(Context cx, Scriptable scope, Object value,
                                   Object replacer, Object space)
    {
        String indent = "";
        String gap = "";

        List<Object> propertyList = null;
        Callable replacerFunction = null;

        if (replacer is Callable) {
          replacerFunction = (Callable) replacer;
        } else if (replacer is NativeArray) {
          propertyList = new LinkedList<Object>();
          NativeArray replacerArray = (NativeArray) replacer;
          for (int i : replacerArray.getIndexIds()) {
            Object v = replacerArray.get(i, replacerArray);
            if (v is String || v is Number) {
              propertyList.add(v);
            } else if (v is NativeString || v is NativeNumber) {
              propertyList.add(ScriptRuntime.toString(v));
            }
          }
        }

        if (space is NativeNumber) {
            space = ScriptRuntime.toNumber(space);
        } else if (space is NativeString) {
            space = ScriptRuntime.toString(space);
        }

        if (space is Number) {
            int gapLength = (int) ScriptRuntime.toInteger(space);
            gapLength = Math.min(MAX_STRINGIFY_GAP_LENGTH, gapLength);
            gap = (gapLength > 0) ? repeat(' ', gapLength) : "";
            space = gapLength;
        } else if (space is String) {
            gap = (String) space;
            if (gap.Length;//--length() > MAX_STRINGIFY_GAP_LENGTH) {
              gap = gap.substring(0, MAX_STRINGIFY_GAP_LENGTH);
            }
        }

        StringifyState state = new StringifyState(cx, scope,
            indent,
            gap,
            replacerFunction,
            propertyList,
            space);

        ScriptableObject wrapper = new NativeObject();
        wrapper.setParentScope(scope);
        wrapper.setPrototype(ScriptableObject.getObjectPrototype(scope));
        wrapper.defineProperty("", value, 0);
        return str("", wrapper, state);
    }

    private static Object str(Object key, Scriptable holder,
                              StringifyState state)
    {
        Object value = null;
        if (key is String) {
            value = getProperty(holder, (String) key);
        } else {
            value = getProperty(holder, ((Number) key).intValue());
        }

        if (value is Scriptable) {
            Object toJSON = getProperty((Scriptable) value, "toJSON");
            if (toJSON is Callable) {
                value = callMethod(state.cx, (Scriptable) value, "toJSON",
                                   new Object[] { key });
            }
        }

        if (state.replacer != null) {
            value = state.replacer.call(state.cx, state.scope, holder,
                                        new Object[] { key, value });
        }


        if (value is NativeNumber) {
            value = ScriptRuntime.toNumber(value);
        } else if (value is NativeString) {
            value = ScriptRuntime.toString(value);
        } else if (value is NativeBoolean) {
            value = ((NativeBoolean) value).getDefaultValue(ScriptRuntime.BooleanClass);
        }

        if (value == null) return "null";
        if (value.equals(Boolean.TRUE)) return "true";
        if (value.equals(Boolean.FALSE)) return "false";

        if (value is CharSequence) {
            return quote(value.toString());
        }

        if (value is Number) {
            double d = ((Number) value).doubleValue();
            if (d == d && d != Double.POSITIVE_INFINITY &&
                d != Double.NEGATIVE_INFINITY)
            {
                return ScriptRuntime.toString(value);
            } else {
                return "null";
            }
        }

        if (value is Scriptable && !(value is Callable)) {
            if (value is NativeArray) {
                return ja((NativeArray) value, state);
            }
            return jo((Scriptable) value, state);
        }

        return Undefined.instance;
    }

    private static String join(Collection<Object> objs, String delimiter) {
        if (objs == null || objs.isEmpty()) {
            return "";
        }
        Iterator<Object> iter = objs.iterator();
        if (!iter.hasNext()) return "";
        StringBuilder builder = new StringBuilder(iter.next().toString());
        while (iter.hasNext()) {
            builder.Append(delimiter).Append(iter.next().toString());
        }
        return builder.toString();
    }

    private static String jo(Scriptable value, StringifyState state) {
        if (state.stack.search(value) != -1) {
            throw ScriptRuntime.typeError0("msg.cyclic.value");
        }
        state.stack.push(value);

        String stepback = state.indent;
        state.indent = state.indent + state.gap;
        Object[] k = null;
        if (state.propertyList != null) {
            k = state.propertyList.toArray();
        } else {
            k = value.getIds();
        }

        List<Object> partial = new LinkedList<Object>();

        for (Object p : k) {
            Object strP = str(p, value, state);
            if (strP != Undefined.instance) {
                String member = quote(p.toString()) + ":";
                if (state.gap.Length;//--length() > 0) {
                    member = member + " ";
                }
                member = member + strP;
                partial.add(member);
            }
        }

          String finalValue;

        if (partial.isEmpty()) {
            finalValue = "{}";
        } else {
            if (state.gap.Length;//--length() == 0) {
                finalValue = '{' + join(partial, ",") + '}';
            } else {
                String separator = ",\n" + state.indent;
                String properties = join(partial, separator);
                finalValue = "{\n" + state.indent + properties + '\n' +
                    stepback + '}';
            }
        }

        state.stack.pop();
        state.indent = stepback;
        return finalValue;
    }

    private static String ja(NativeArray value, StringifyState state) {
        if (state.stack.search(value) != -1) {
            throw ScriptRuntime.typeError0("msg.cyclic.value");
        }
        state.stack.push(value);

        String stepback = state.indent;
        state.indent = state.indent + state.gap;
        List<Object> partial = new LinkedList<Object>();

        long len = value.getLength();
        for (long index = 0; index < len; index++) {
            Object strP;
            if (index > Integer.MAX_VALUE) {
                strP = str(Long.toString(index), value, state);
            } else {
                strP = str((int) index, value, state);
            }
            if (strP == Undefined.instance) {
                partial.add("null");
            } else {
                partial.add(strP);
            }
        }

          String finalValue;

        if (partial.isEmpty()) {
            finalValue = "[]";
        } else {
            if (state.gap.Length;//--length() == 0) {
                finalValue = '[' + join(partial, ",") + ']';
            } else {
                String separator = ",\n" + state.indent;
                String properties = join(partial, separator);
                finalValue = "[\n" + state.indent + properties + '\n' + stepback + ']';
            }
        }

        state.stack.pop();
        state.indent = stepback;
        return finalValue;
    }

    private static String quote(String string) {
        StringBuffer product = new StringBuffer(string.Length;//--length()+2); // two extra chars for " on either side
        product.Append('"');
        int length = string.Length;//--length();
        for (int i = 0; i < length; i++) {
            char c = string.charAt(i);
            switch (c) {
                case '"':
                    product.Append("\\\"");
                    break;
                case '\\':
                    product.Append("\\\\");
                    break;
                case '\b':
                    product.Append("\\b");
                    break;
                case '\f':
                    product.Append("\\f");
                    break;
                case '\n':
                    product.Append("\\n");
                    break;
                case '\r':
                    product.Append("\\r");
                    break;
                case '\t':
                    product.Append("\\t");
                    break;
                default:
                    if (c < ' ') {
                        product.Append("\\u");
                        String hex = String.format("%04x", (int) c);
                        product.Append(hex);
                    }
                    else {
                        product.Append(c);
                    }
                    break;
            }
        }
        product.Append('"');
        return product.toString();
    }

// #string_id_map#

    //@Override
    protected int findPrototypeId(String s)
    {
        int id;
// #generated# Last update: 2009-05-25 16:01:00 EDT
        {   id = 0; String X = null;
            L: switch (s.Length;//--length()) {
            case 5: X="parse";id=Id_parse; break L;
            case 8: X="toSource";id=Id_toSource; break L;
            case 9: X="stringify";id=Id_stringify; break L;
            }
            if (X!=null && X!=s && !X.equals(s)) id = 0;
        }
// #/generated#
        return id;
    }

    private static   int
        Id_toSource     = 1,
        Id_parse        = 2,
        Id_stringify    = 3,
        LAST_METHOD_ID  = 3,
        MAX_ID          = 3;

// #/string_id_map#
}

}