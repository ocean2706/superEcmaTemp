/* -*- Mode: java; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 4 -*-
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License; v. 2.0. If a copy of the MPL was not distributed with this
 * file; You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.classfile 
{

/**
 * This class provides opcode values expected by the JVM in Java class files.
 *
 * It also provides tables for internal use by the ClassFileWriter.
 *
 * @author Roger Lawrence
 */
public class ByteCode {

    /**
     * The byte opcodes defined by the Java Virtual Machine.
     */


		public const   int NOP = 0x00;
         public const   int ACONST_NULL = 0x01;
         public const   int ICONST_M1 = 0x02;
  public const   int       ICONST_0 = 0x03;
  public const   int       ICONST_1 = 0x04;
  public const   int       ICONST_2 = 0x05;
 public const   int        ICONST_3 = 0x06;
 public const   int        ICONST_4 = 0x07;
 public const   int        ICONST_5 = 0x08;
 public const   int        LCONST_0 = 0x09;
 public const   int        LCONST_1 = 0x0A;
 public const   int        FCONST_0 = 0x0B;
 public const   int        FCONST_1 = 0x0C;
 public const   int        FCONST_2 = 0x0D;
 public const   int        DCONST_0 = 0x0E;
 public const   int        DCONST_1 = 0x0F;
 public const   int        BIPUSH = 0x10;
 public const   int        SIPUSH = 0x11;
 public const   int        LDC = 0x12;
 public const   int        LDC_W = 0x13;
 public const   int        LDC2_W = 0x14;
 public const   int        ILOAD = 0x15;
 public const   int        LLOAD = 0x16;
 public const   int        FLOAD = 0x17;
 public const   int        DLOAD = 0x18;
 public const   int        ALOAD = 0x19;
 public const   int        ILOAD_0 = 0x1A;
 public const   int        ILOAD_1 = 0x1B;
 public const   int        ILOAD_2 = 0x1C;
 public const   int        ILOAD_3 = 0x1D;
 public const   int        LLOAD_0 = 0x1E;
 public const   int        LLOAD_1 = 0x1F;
  public const   int       LLOAD_2 = 0x20;
  public const   int       LLOAD_3 = 0x21;
 public const   int        FLOAD_0 = 0x22;
 public const   int        FLOAD_1 = 0x23;
 public const   int        FLOAD_2 = 0x24;
 public const   int        FLOAD_3 = 0x25;
 public const   int        DLOAD_0 = 0x26;
 public const   int        DLOAD_1 = 0x27;
 public const   int        DLOAD_2 = 0x28;
 public const   int        DLOAD_3 = 0x29;
 public const   int        ALOAD_0 = 0x2A;
 public const   int        ALOAD_1 = 0x2B;
 public const   int        ALOAD_2 = 0x2C;
 public const   int        ALOAD_3 = 0x2D;
 public const   int        IALOAD = 0x2E;
public const   int         LALOAD = 0x2F;
public const   int         FALOAD = 0x30;
public const   int         DALOAD = 0x31;
public const   int         AALOAD = 0x32;
public const   int         BALOAD = 0x33;
public const   int         CALOAD = 0x34;
public const   int         SALOAD = 0x35;
public const   int         ISTORE = 0x36;
public const   int         LSTORE = 0x37;
public const   int         FSTORE = 0x38;
public const   int         DSTORE = 0x39;
public const   int         ASTORE = 0x3A;
public const   int         ISTORE_0 = 0x3B;
public const   int         ISTORE_1 = 0x3C;
public const   int         ISTORE_2 = 0x3D;
public const   int         ISTORE_3 = 0x3E;
 public const   int        LSTORE_0 = 0x3F;
public const   int         LSTORE_1 = 0x40;
public const   int         LSTORE_2 = 0x41;
public const   int         LSTORE_3 = 0x42;
public const   int         FSTORE_0 = 0x43;
        public const   int FSTORE_1 = 0x44;
        public const   int FSTORE_2 = 0x45;
        public const   int FSTORE_3 = 0x46;
        public const   int DSTORE_0 = 0x47;
        public const   int DSTORE_1 = 0x48;
        public const   int DSTORE_2 = 0x49;
        public const   int DSTORE_3 = 0x4A;
        public const   int ASTORE_0 = 0x4B;
        public const   int ASTORE_1 = 0x4C;
        public const   int ASTORE_2 = 0x4D;
        public const   int ASTORE_3 = 0x4E;
        public const   int IASTORE = 0x4F;
        public const   int LASTORE = 0x50;
        public const   int FASTORE = 0x51;
        public const   int DASTORE = 0x52;
        public const   int AASTORE = 0x53;
        public const   int BASTORE = 0x54;
        public const   int CASTORE = 0x55;
        public const   int SASTORE = 0x56;
        public const   int POP = 0x57;
        public const   int POP2 = 0x58;
        public const   int DUP = 0x59;
        public const   int DUP_X1 = 0x5A;
        public const   int DUP_X2 = 0x5B;
        public const   int DUP2 = 0x5C;
        public const   int DUP2_X1 = 0x5D;
        public const   int DUP2_X2 = 0x5E;
        public const   int SWAP = 0x5F;
        public const   int IADD = 0x60;
        public const   int LADD = 0x61;
        public const   int FADD = 0x62;
        public const   int DADD = 0x63;
        public const   int ISUB = 0x64;
        public const   int LSUB = 0x65;
        public const   int FSUB = 0x66;
        public const   int DSUB = 0x67;
        public const   int IMUL = 0x68;
        public const   int LMUL = 0x69;
        public const   int FMUL = 0x6A;
        public const   int DMUL = 0x6B;
        public const   int IDIV = 0x6C;
        public const   int LDIV = 0x6D;
        public const   int FDIV = 0x6E;
        public const   int DDIV = 0x6F;
        public const   int IREM = 0x70;
        public const   int LREM = 0x71;
        public const   int FREM = 0x72;
        public const   int DREM = 0x73;
        public const   int INEG = 0x74;
        public const   int LNEG = 0x75;
        public const   int FNEG = 0x76;
        public const   int DNEG = 0x77;
        public const   int ISHL = 0x78;
        public const   int LSHL = 0x79;
        public const   int ISHR = 0x7A;
        public const   int LSHR = 0x7B;
        public const   int IUSHR = 0x7C;
        public const   int LUSHR = 0x7D;
        public const   int IAND = 0x7E;
        public const   int LAND = 0x7F;
        public const   int IOR = 0x80;
        public const   int LOR = 0x81;
        public const   int IXOR = 0x82;
        public const   int LXOR = 0x83;
        public const   int IINC = 0x84;
        public const   int I2L = 0x85;
        public const   int I2F = 0x86;
        public const   int I2D = 0x87;
        public const   int L2I = 0x88;
        public const   int L2F = 0x89;
        public const   int L2D = 0x8A;
        public const   int  F2I = 0x8B;
        public const   int F2L = 0x8C;
        public const   int F2D = 0x8D;
        public const   int D2I = 0x8E;
        public const   int D2L = 0x8F;
        public const   int D2F = 0x90;
        public const   int I2B = 0x91;
        public const   int I2C = 0x92;
        public const   int I2S = 0x93;
        public const   int LCMP = 0x94;
        public const   int FCMPL = 0x95;
        public const   int FCMPG = 0x96;
        public const   int DCMPL = 0x97;
        public const   int DCMPG = 0x98;
        public const   int IFEQ = 0x99;
        public const   int IFNE = 0x9A;
        public const   int IFLT = 0x9B;
        public const   int IFGE = 0x9C;
        public const   int IFGT = 0x9D;
        public const   int IFLE = 0x9E;
        public const   int IF_ICMPEQ = 0x9F;
        public const   int IF_ICMPNE = 0xA0;
        public const   int IF_ICMPLT = 0xA1;
        public const   int IF_ICMPGE = 0xA2;
        public const   int IF_ICMPGT = 0xA3;
        public const   int IF_ICMPLE = 0xA4;
        public const   int IF_ACMPEQ = 0xA5;
        public const   int IF_ACMPNE = 0xA6;
        public const   int GOTO = 0xA7;
        public const   int JSR = 0xA8;
        public const   int RET = 0xA9;
        public const   int TABLESWITCH = 0xAA;
        public const   int LOOKUPSWITCH = 0xAB;
        public const   int IRETURN = 0xAC;
        public const   int LRETURN = 0xAD;
        public const   int FRETURN = 0xAE;
        public const   int DRETURN = 0xAF;
        public const   int ARETURN = 0xB0;
        public const   int RETURN = 0xB1;
        public const   int GETSTATIC = 0xB2;
        public const   int PUTSTATIC = 0xB3;
        public const   int GETFIELD = 0xB4;
        public const   int PUTFIELD = 0xB5;
        public const   int INVOKEVIRTUAL = 0xB6;
        public const   int INVOKESPECIAL = 0xB7;
        public const   int INVOKESTATIC = 0xB8;
        public const   int INVOKEINTERFACE = 0xB9;
        public const   int NEW = 0xBB;
        public const   int NEWARRAY = 0xBC;
        public const   int ANEWARRAY = 0xBD;
        public const   int ARRAYLENGTH = 0xBE;
        public const   int ATHROW = 0xBF;
        public const   int CHECKCAST = 0xC0;
        public const   int INSTANCEOF = 0xC1;
        public const   int MONITORENTER = 0xC2;
        public const   int MONITOREXIT = 0xC3;
        public const   int WIDE = 0xC4;
        public const   int MULTIANEWARRAY = 0xC5;
        public const   int IFNULL = 0xC6;
        public const   int IFNONNULL = 0xC7;
        public const   int GOTO_W = 0xC8;
        public const   int JSR_W = 0xC9;
        public const   int BREAKPOINT = 0xCA;

public const   int         IMPDEP1 = 0xFE;
		public const   int IMPDEP2 = 0xFF;
		

        public const   byte      T_bool  = 4;
        public const   byte    T_CHAR = 5;
            public const   byte T_FLOAT = 6;
            public const   byte T_DOUBLE = 7;
             public const   byte T_SHORT = 9;
            public const   byte T_INT = 10;
		public const   byte  T_LONG = 11;



	}
}

