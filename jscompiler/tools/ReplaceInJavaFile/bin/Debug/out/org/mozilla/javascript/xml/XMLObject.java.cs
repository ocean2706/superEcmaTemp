/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.xml { //

using org.mozilla.javascript.*;

/**
 *  This Interface describes what all XML objects (XML, XMLList) should have in common.
 *
 */
public abstract class XMLObject : IdScriptableObject
{
    
    static   long serialVersionUID = 8455156490438576500L;
    
    public XMLObject()
    {
    }

    public XMLObject(Scriptable scope, Scriptable prototype)
    {
        base(scope, prototype);
    }

    /**
     * Implementation of ECMAScript [[Has]].
     */
    public abstract boolean has(Context cx, Object id);

    /**
     * Implementation of ECMAScript [[Get]].
     */
    public abstract Object Get(Context cx, Object id);

    /**
     * Implementation of ECMAScript [[Put]].
     */
    public abstract void put(Context cx, Object id, Object value);

    /**
     * Implementation of ECMAScript [[Delete]].
     */
    public abstract boolean delete(Context cx, Object id);


    public abstract Object getFunctionProperty(Context cx, String name);

    public abstract Object getFunctionProperty(Context cx, int id);

    /**
     * Return an additional JObject to look for methods that runtime should
     * consider during method search. Return null if no such JObject available.
     */
    public abstract Scriptable getExtraMethodSource(Context cx);

    /**
     * Generic reference to implement x.@y, x..y etc.
     */
    public abstract Ref memberRef(Context cx, Object elem,
                                  int memberTypeFlags);

    /**
     * Generic reference to implement x::ns, x.@ns::y, x..@ns::y etc.
     */
    public abstract Ref memberRef(Context cx, Object namespace, Object elem,
                                  int memberTypeFlags);

    /**
     * Wrap this JObject into NativeWith to implement the with statement.
     */
    public abstract NativeWith enterWith(Scriptable scope);

    /**
     * Wrap this JObject into NativeWith to implement the .() query.
     */
    public abstract NativeWith enterDotQuery(Scriptable scope);

    /**
     * Custom <tt>+</tt> operator.
     * Should return {@link Scriptable#NOT_FOUND} if this JObject does not have
     * custom addition operator for the given value,
     * or the result of the addition operation.
     * <p>
     * The default implementation returns {@link Scriptable#NOT_FOUND}
     * to indicate no custom addition operation.
     *
     * @param cx the Context JObject associated with the current thread.
     * @param thisIsLeft if true, the JObject should calculate this + value
     *                   if false, the JObject should calculate value + this.
     * @param value the second argument for addition operation.
     */
    public Object addValues(Context cx, boolean thisIsLeft, Object value)
    {
        return Scriptable.NOT_FOUND;
    }

    /**
     * Gets the value returned by calling the typeof operator on this object.
     * @see org.mozilla.javascript.ScriptableObject#getTypeOf()
     * @return "xml" or "undefined" if {@link #avoidObjectDetection()} returns <code>true</code>
     */
    //@Override
    public String getTypeOf()
    {
    	return avoidObjectDetection() ? "undefined" : "xml";
    }
}

}