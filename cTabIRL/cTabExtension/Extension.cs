using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cTabExtension
{
    public class Extension
    {
        private static bool debugCallback;
        private static int isInit;

        public static ExtensionCallback callback;
        public delegate int ExtensionCallback([MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string function, [MarshalAs(UnmanagedType.LPStr)] string data);

#if WIN64
        [DllExport("RVExtensionRegisterCallback", CallingConvention = CallingConvention.Winapi)]
#else
        [DllExport("_RVExtensionRegisterCallback@4", CallingConvention = CallingConvention.Winapi)]
#endif
        public static void RVExtensionRegisterCallback([MarshalAs(UnmanagedType.FunctionPtr)] ExtensionCallback func)
        {
            callback = func;
        }

#if WIN64
        [DllExport("RVExtensionVersion", CallingConvention = CallingConvention.Winapi)]
#else
        [DllExport("_RVExtensionVersion@8", CallingConvention = CallingConvention.Winapi)]
#endif
        public static void RvExtensionVersion(StringBuilder output, int outputSize)
        {
            output.Append("cTabExtension v1.0");
        }

#if WIN64
        [DllExport("RVExtension", CallingConvention = CallingConvention.Winapi)]
#else
        [DllExport("_RVExtension@12", CallingConvention = CallingConvention.Winapi)]
#endif
        public static void RvExtension(StringBuilder output, int outputSize,
            [MarshalAs(UnmanagedType.LPStr)] string function)
        {
            if (function == "Debug")
            {
                debugCallback = true;
            }
            EnsureInit();
        }

        private static void EnsureInit()
        {
            if (Interlocked.CompareExchange(ref isInit, 1, 0) == 0)
            {
                AppDomain.CurrentDomain.AssemblyResolve += Resolve;
            }
        }

        private static System.Reflection.Assembly Resolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);
            var path = Path.GetDirectoryName(typeof(Extension).Assembly.Location);
            var file = Path.Combine(path, name.Name + ".dll");
            DebugMessage($"Load '{args.Name}' from {file}");
            return Assembly.LoadFrom(file);
        }

#if WIN64
        [DllExport("RVExtensionArgs", CallingConvention = CallingConvention.Winapi)]
#else
        [DllExport("_RVExtensionArgs@20", CallingConvention = CallingConvention.Winapi)]
#endif
        public static int RvExtensionArgs(StringBuilder output, int outputSize,
            [MarshalAs(UnmanagedType.LPStr)] string function,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 4)] string[] args, int argCount)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (function == "Debug")
                {
                    debugCallback = args.Length == 0 || args[0] == "true";
                    return 0;
                }
                if (function == "Warmup")
                {
                    return 0;
                }
                EnsureInit();
                Worker.Message(function, args);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    DebugMessage($"{e.GetType().Name} {e.Message}.");
                }
            }
            catch (Exception e)
            {
                DebugMessage($"{e.GetType().Name} {e.Message}.");
            }
            DebugMessage($"{function}: {sw.ElapsedMilliseconds} msec in RvExtensionArgs.");
            return 0;
        }

        public static Task Callback(string function, string data)
        {
            if (callback != null)
            {
                return Task.Factory.StartNew(() => callback("ctab", function, data));
            }
            return Task.CompletedTask;
        }

        public static void DebugMessage(string message)
        {
            Debug.WriteLine(message);
            if (callback != null && debugCallback)
            {
                callback("ctab", "Debug", message);
            }
        }
    }
}
