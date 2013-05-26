/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript { //

using java.io.Serializable;

/**
 * Generic notion of reference JObject that know how to query/modify the
 * target objects based on some property/index.
 */
public abstract class Ref implements Serializable
{
    
    static   long serialVersionUID = 4044540354730911424L;
    
    public boolean has(Context cx)
    {
        return true;
    }

    public abstract Object Get(Context cx);

    public abstract Object Set(Context cx, Object value);

    public boolean delete(Context cx)
    {
        return false;
    }

}


}