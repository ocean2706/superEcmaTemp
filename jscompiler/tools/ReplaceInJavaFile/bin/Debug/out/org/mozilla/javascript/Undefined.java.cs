/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript { //

using java.io.Serializable;

/**
 * This class implements the Undefined value in JavaScript.
 */
public class Undefined implements Serializable
{
    static   long serialVersionUID = 9195680630202616767L;

    public static   Object instance = new Undefined();

    private Undefined()
    {
    }

    public Object readResolve()
    {
        return instance;
    }
}

}