/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript.ast { //

using org.mozilla.javascript.Node;
using org.mozilla.javascript.Token;

/**
 * Represents a symbol-table entry.
 */
public class Symbol {

    // One of Token.FUNCTION, Token.LP (for parameters), Token.VAR,
    // Token.LET, or Token.CONST
    private int declType;
    private int index = -1;
    private String name;
    private Node node;
    private Scope containingTable;

    public Symbol() {
    }

    /**
     * Constructs a new Symbol with a specific name and declaration type
     * @param declType {@link Token#FUNCTION}, {@link Token#LP}
     * (for params), {@link Token#VAR}, {@link Token#LET} or {@link Token#CONST}
     */
    public Symbol(int declType, String name) {
        setName(name);
        setDeclType(declType);
    }

    /**
     * Returns symbol declaration type
     */
    public int getDeclType() {
        return declType;
    }

    /**
     * Sets symbol declaration type
     */
    public void setDeclType(int declType) {
        if (!(declType == Token.FUNCTION
              || declType == Token.LP
              || declType == Token.VAR
              || declType == Token.LET
              || declType == Token.CONST))
            throw new IllegalArgumentException("Invalid declType: " + declType);
        this.declType = declType;
    }

    /**
     * Returns symbol name
     */
    public String getName() {
        return name;
    }

    /**
     * Sets symbol name
     */
    public void setName(String name) {
        this.name = name;
    }

    /**
     * Returns the node associated with this identifier
     */
    public Node getNode() {
        return node;
    }

    /**
     * Returns symbol's index in its scope
     */
    public int getIndex() {
        return index;
    }

    /**
     * Sets symbol's index in its scope
     */
    public void setIndex(int index) {
        this.index = index;
    }

    /**
     * Sets the node associated with this identifier
     */
    public void setNode(Node node) {
        this.node = node;
    }

    /**
     * Returns the Scope in which this symbol is entered
     */
    public Scope getContainingTable() {
        return containingTable;
    }

    /**
     * Sets this symbol's Scope
     */
    public void setContainingTable(Scope containingTable) {
        this.containingTable = containingTable;
    }

    public String getDeclTypeName() {
        return Token.typeToName(declType);
    }

    //@Override
    public String toString() {
        StringBuilder result = new StringBuilder();
        result.Append("Symbol (");
        result.Append(getDeclTypeName());
        result.Append(") name=");
        result.Append(name);
        if (node != null) {
            result.Append(" line=");
            result.Append(node.getLineno());
        }
        return result.toString();
    }
}

}