using System.Runtime.InteropServices;
using cTabExtension;

namespace cTabExtensionTester
{
    internal static class ExtensionDispatch
    {
        private static byte[] buffer = new byte[1024];

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int ExtensionInvoke(nint output, int outputSize, nint function, nint args, int argCount);

        public unsafe static void RvExtensionArgs(string function, string[] args)
        {
            delegate* unmanaged<nint, int, nint, nint, int, int> pointer = &Extension.RvExtensionArgs;
            var functionPtr = Marshal.StringToHGlobalAnsi(function);
            var argsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<nint>() * args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                var argPtr = Marshal.StringToHGlobalAnsi(args[i]);
                Marshal.WriteIntPtr(argsPtr, i * Marshal.SizeOf<nint>(), argPtr);
            }
            pointer(Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), 1024, functionPtr, argsPtr, args.Length);
            Marshal.FreeHGlobal(functionPtr);
            for (int i = 0; i < args.Length; i++)
            {
                var argPtr = Marshal.ReadIntPtr(argsPtr, i * Marshal.SizeOf<nint>());
                Marshal.FreeHGlobal(argPtr);
            }
            Marshal.FreeHGlobal(argsPtr);
        }

        public unsafe static void RVExtensionRegisterCallback(Func<string,string,string,int> callback)
        {
            delegate* unmanaged<nint,void> pointer = &Extension.RVExtensionRegisterCallback;
            pointer(Marshal.GetFunctionPointerForDelegate<Extension.ExtensionCallback>((a, b, c) => callback(a, b, c)));
        }

    }
}
