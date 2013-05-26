/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript { //

using java.lang.ref.SoftReference;
using java.lang.reflect.UndeclaredThrowableException;
using java.security.AccessController;
using java.security.CodeSource;
using java.security.Policy;
using java.security.PrivilegedAction;
using java.security.PrivilegedActionException;
using java.security.PrivilegedExceptionAction;
using java.security.SecureClassLoader;
using java.util.Map;
using java.util.WeakHashMap;

using org.mozilla.classfile.ByteCode;
using org.mozilla.classfile.ClassFileWriter;

/**
 * A security controller relying on Java {@link Policy} in effect. When you use
 * this security controller, your securityDomain objects must be instances of
 * {@link CodeSource} representing the location from where you load your
 * scripts. Any Java policy "grant" statements matching the URL and certificate
 * in code sources will apply to the scripts. If you specify any certificates
 * within your {@link CodeSource} objects, it is your responsibility to verify
 * (or not) that the script source files are signed in whatever
 * implementation-specific way you're using.
 * @author Attila Szegedi
 */
public class PolicySecurityController : SecurityController
{
    private static   Byte[] secureCallerImplBytecode = loadBytecode();

    // We're storing a CodeSource -> (ClassLoader -> SecureRenderer), since we
    // need to have one renderer per class loader. We're using weak hash maps
    // and soft references all the way, since we don't want to interfere with
    // cleanup of either CodeSource or ClassLoader objects.
    private static   Map<CodeSource,Map<ClassLoader,SoftReference<SecureCaller>>>
        callers =
            new WeakHashMap<CodeSource,Map<ClassLoader,SoftReference<SecureCaller>>>();

    //@Override
    public Class<?> getStaticSecurityDomainClassInternal() {
        return CodeSource.class;
    }

    private static class Loader : SecureClassLoader
    implements GeneratedClassLoader
    {
        private   CodeSource codeSource;

        Loader(ClassLoader parent, CodeSource codeSource)
        {
            base(parent);
            this.codeSource = codeSource;
        }

        public Class<?> defineClass(String name, Byte[] data)
        {
            return defineClass(name, data, 0, data.length, codeSource);
        }

        public void linkClass(Class<?> cl)
        {
            resolveClass(cl);
        }
    }

    //@Override
    public GeneratedClassLoader createClassLoader(final ClassLoader parent,
              Object securityDomain)
    {
        return (Loader)AccessController.doPrivileged(
            new PrivilegedAction<Object>()
            {
                public Object run()
                {
                    return new Loader(parent, (CodeSource)securityDomain);
                }
            });
    }

    //@Override
    public Object getDynamicSecurityDomain(Object securityDomain)
    {
        // No separate notion of dynamic security domain - just return what was
        // passed in.
        return securityDomain;
    }

    //@Override
    public Object callWithDomain(final Object securityDomain,   Context cx,
            Callable callable, Scriptable scope, Scriptable thisObj,
            Object[] args)
    {
        // Run in doPrivileged as we might be checked for "getClassLoader"
        // runtime permission
          ClassLoader classLoader = (ClassLoader)AccessController.doPrivileged(
            new PrivilegedAction<Object>() {
                public Object run() {
                    return cx.getApplicationClassLoader();
                }
            });
          CodeSource codeSource = (CodeSource)securityDomain;
        Map<ClassLoader,SoftReference<SecureCaller>> classLoaderMap;
        synchronized (callers) {
            classLoaderMap = callers.get(codeSource);
            if(classLoaderMap == null) {
                classLoaderMap = new WeakHashMap<ClassLoader,SoftReference<SecureCaller>>();
                callers.put(codeSource, classLoaderMap);
            }
        }
        SecureCaller caller;
        synchronized (classLoaderMap) {
            SoftReference<SecureCaller> ref = classLoaderMap.get(classLoader);
            if (ref != null) {
                caller = ref.get();
            } else {
                caller = null;
            }
            if (caller == null)
            {
                try
                {
                    // Run in doPrivileged as we'll be checked for
                    // "createClassLoader" runtime permission
                    caller = (SecureCaller)AccessController.doPrivileged(
                            new PrivilegedExceptionAction<Object>()
                    {
                        public Object run() throws Exception
                        {
                            Loader loader = new Loader(classLoader,
                                    codeSource);
                            Class<?> c = loader.defineClass(
                                    SecureCaller.class.getName() + "Impl",
                                    secureCallerImplBytecode);
                            return c.newInstance();
                        }
                    });
                    classLoaderMap.put(classLoader, new SoftReference<SecureCaller>(caller));
                }
                catch(PrivilegedActionException ex)
                {
                    throw new UndeclaredThrowableException(ex.getCause());
                }
            }
        }
        return caller.call(callable, cx, scope, thisObj, args);
    }

    public abstract static class SecureCaller
    {
        public abstract Object call(Callable callable, Context cx, Scriptable scope,
                Scriptable thisObj, Object[] args);
    }


    private static Byte[] loadBytecode()
    {
        String secureCallerClassName = SecureCaller.class.getName();
        ClassFileWriter cfw = new ClassFileWriter(
                secureCallerClassName + "Impl", secureCallerClassName,
                "<generated>");
        cfw.startMethod("<init>", "()V", ClassFileWriter.ACC_PUBLIC);
        cfw.addALoad(0);
        cfw.addInvoke(ByteCode.INVOKESPECIAL, secureCallerClassName,
                "<init>", "()V");
        cfw.add(ByteCode.RETURN);
        cfw.stopMethod((short)1);
        String callableCallSig =
            "Lorg/mozilla/javascript/Context;" +
            "Lorg/mozilla/javascript/Scriptable;" +
            "Lorg/mozilla/javascript/Scriptable;" +
            "[Ljava/lang/Object;)Ljava/lang/Object;";

        cfw.startMethod("call",
                "(Lorg/mozilla/javascript/Callable;" + callableCallSig,
                (short)(ClassFileWriter.ACC_PUBLIC
                        | ClassFileWriter.ACC_FINAL));
        for(int i = 1; i < 6; ++i) {
            cfw.addALoad(i);
        }
        cfw.addInvoke(ByteCode.INVOKEINTERFACE,
                "org/mozilla/javascript/Callable", "call",
                "(" + callableCallSig);
        cfw.add(ByteCode.ARETURN);
        cfw.stopMethod((short)6);
        return cfw.toByteArray();
    }
}
}