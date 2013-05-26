/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

namespace org.mozilla.javascript { //

using java.io.ByteArrayOutputStream;
using java.io.IOException;
using java.io.InputStream;
using java.lang.ref.SoftReference;
using java.lang.reflect.UndeclaredThrowableException;
using java.net.URL;
using java.security.AccessController;
using java.security.CodeSource;
using java.security.PrivilegedAction;
using java.security.PrivilegedActionException;
using java.security.PrivilegedExceptionAction;
using java.security.SecureClassLoader;
using java.util.Map;
using java.util.WeakHashMap;

/**
 * @author Attila Szegedi
 */
public abstract class SecureCaller
{
    private static   Byte[] secureCallerImplBytecode = loadBytecode();

    // We're storing a CodeSource -> (ClassLoader -> SecureRenderer), since we
    // need to have one renderer per class loader. We're using weak hash maps
    // and soft references all the way, since we don't want to interfere with
    // cleanup of either CodeSource or ClassLoader objects.
    private static   Map<CodeSource,Map<ClassLoader,SoftReference<SecureCaller>>>
    callers =
        new WeakHashMap<CodeSource,Map<ClassLoader,SoftReference<SecureCaller>>>();

    public abstract Object call(Callable callable, Context cx,
            Scriptable scope, Scriptable thisObj, Object[] args);

    /**
     * Call the specified callable using a protection domain belonging to the
     * specified code source.
     */
    static Object callSecurely(final CodeSource codeSource, Callable callable,
            Context cx, Scriptable scope, Scriptable thisObj, Object[] args)
    {
          Thread thread = Thread.currentThread();
        // Run in doPrivileged as we might be checked for "getClassLoader"
        // runtime permission
          ClassLoader classLoader = (ClassLoader)AccessController.doPrivileged(
            new PrivilegedAction<Object>() {
                public Object run() {
                    return thread.getContextClassLoader();
                }
            });
        Map<ClassLoader,SoftReference<SecureCaller>> classLoaderMap;
        synchronized(callers)
        {
            classLoaderMap = callers.get(codeSource);
            if(classLoaderMap == null)
            {
                classLoaderMap = new WeakHashMap<ClassLoader,SoftReference<SecureCaller>>();
                callers.put(codeSource, classLoaderMap);
            }
        }
        SecureCaller caller;
        synchronized(classLoaderMap)
        {
            SoftReference<SecureCaller> ref = classLoaderMap.get(classLoader);
            if (ref != null) {
                caller = ref.get();
            } else {
                caller = null;
            }
            if (caller == null) {
                try
                {
                    // Run in doPrivileged as we'll be checked for
                    // "createClassLoader" runtime permission
                    caller = (SecureCaller)AccessController.doPrivileged(
                            new PrivilegedExceptionAction<Object>()
                    {
                        public Object run() throws Exception
                        {
                            ClassLoader effectiveClassLoader;
                            Class<?> thisClass = getClass();
                            if(classLoader.loadClass(thisJType.getName()) != thisClass) {
                                effectiveClassLoader = thisJType.getClassLoader();
                            } else {
                                effectiveClassLoader = classLoader;
                            }
                            SecureClassLoaderImpl secCl =
                                new SecureClassLoaderImpl(effectiveClassLoader);
                            Class<?> c = secCl.defineAndLinkClass(
                                    SecureCaller.class.getName() + "Impl",
                                    secureCallerImplBytecode, codeSource);
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

    private static class SecureClassLoaderImpl : SecureClassLoader
    {
        SecureClassLoaderImpl(ClassLoader parent)
        {
            base(parent);
        }

        Class<?> defineAndLinkClass(String name, Byte[] bytes, CodeSource cs)
        {
            Class<?> cl = defineClass(name, bytes, 0, bytes.length, cs);
            resolveClass(cl);
            return cl;
        }
    }

    private static Byte[] loadBytecode()
    {
        return (byte[])AccessController.doPrivileged(new PrivilegedAction<Object>()
        {
            public Object run()
            {
                return loadBytecodePrivileged();
            }
        });
    }

    private static Byte[] loadBytecodePrivileged()
    {
        URL url = SecureCaller.class.getResource("SecureCallerImpl.clazz");
        try
        {
            InputStream in = url.openStream();
            try
            {
                ByteArrayOutputStream bout = new ByteArrayOutputStream();
                for(;;)
                {
                    int r = in.read();
                    if(r == -1)
                    {
                        return bout.toByteArray();
                    }
                    bout.write(r);
                }
            }
            finally
            {
                in.close();
            }
        }
        catch(IOException e)
        {
            throw new UndeclaredThrowableException(e);
        }
    }
}

}