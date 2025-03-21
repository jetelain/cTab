using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace cTabExtension
{
    public static class Extension
    {
        private static ExtensionCallback? callback;
        private static bool debugCallback;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int ExtensionCallback([MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string function, [MarshalAs(UnmanagedType.LPStr)] string data);


        [UnmanagedCallersOnly(EntryPoint = "RVExtensionRegisterCallback")]
        public static void RVExtensionRegisterCallback(nint func)
        {
            callback = Marshal.GetDelegateForFunctionPointer<ExtensionCallback>(func);
        }

        [UnmanagedCallersOnly(EntryPoint = "RVExtensionVersion")]
        public static void RvExtensionVersion(nint output, int outputSize)
        {
            Output(output, outputSize, "cTabExtension 2.1");
        }

        private static void Output(nint output, int outputSize, string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            Marshal.Copy(bytes, 0, output, Math.Min(bytes.Length, outputSize));
        }

        [UnmanagedCallersOnly(EntryPoint = "RVExtension")]
        public static void RvExtension(nint output, int outputSize, nint function)
        {
            var functionString = Marshal.PtrToStringUTF8(function);
            RvExtensionArgsImpl(functionString, new string[0]);
            Output(output, outputSize, "");
        }

        [UnmanagedCallersOnly(EntryPoint = "RVExtensionArgs")]
        public static int RvExtensionArgs(nint output, int outputSize, nint function, nint args, int argCount)
        {
            var functionString = Marshal.PtrToStringUTF8(function);
            var argsString = new string?[argCount];
            for (int i = 0; i < argCount; i++)
            {
                argsString[i] = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(args + (i * Marshal.SizeOf<nint>())));
            }

            RvExtensionArgsImpl(functionString, argsString);
            Output(output, outputSize, "");
            return 0;
        }

        public static int RvExtensionArgsImpl(string? function, string?[] args)
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
                Worker.Message(function ?? string.Empty, args);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    ErrorMessage($"{e.GetType().Name} {e.Message}.");
                }
            }
            catch (Exception e)
            {
                ErrorMessage($"{e.GetType().Name} {e.Message}.");
            }
            DebugMessage($"{function}: {sw.ElapsedTicks} ticks in RvExtensionArgs.");
            return 0;
        }

        public static Task Callback(string function, string data)
        {
            if (data == null)
            {
                data = "";
            }
            if (function.Length > 64 || data.Length > 20000)
            {
                return Task.CompletedTask;
            }
            if (callback != null)
            {
                return Task.Factory.StartNew(() => callback("ctab", function, data));
            }
            return Task.CompletedTask;
        }

        public static void DebugMessage(string message)
        {
            Trace.WriteLine(message);
            if (callback != null && debugCallback)
            {
                callback("ctab", "Debug", message);
            }
        }

        public static void ErrorMessage(string message)
        {
            Trace.TraceError("%1", message);
            if (callback != null)
            {
                callback("ctab", "Error", message);
            }
        }
    }
}
