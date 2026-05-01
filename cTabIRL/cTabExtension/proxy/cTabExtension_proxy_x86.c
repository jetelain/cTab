#include <windows.h>
#include <stdio.h>
/* 
    Native AOT is not able to export the symbols as expected by RvEngine/Arma3 

    This proxy DLL loads the actual implementation (cTabExtension_x86.dll) and forwards the calls to it.
*/

/* Function pointer types — AOT exports are stdcall on x86 */
typedef void (__stdcall *fn_RVExtensionVersion)          (char *output, int outputSize);
typedef void (__stdcall *fn_RVExtension)                 (char *output, int outputSize, const char *function);
typedef int  (__stdcall *fn_RVExtensionArgs)             (char *output, int outputSize, const char *function, const char **args, int argCount);
typedef void (__stdcall *fn_RVExtensionRegisterCallback) (void *callback);

/* ---- Loaded function pointers --------------------------------------- */
static fn_RVExtensionVersion          g_Version   = NULL;
static fn_RVExtension                 g_Ext       = NULL;
static fn_RVExtensionArgs             g_ExtArgs   = NULL;
static fn_RVExtensionRegisterCallback g_RegCb     = NULL;
static HMODULE                        g_hImpl     = NULL;

/* ---- DLL entry point ------------------------------------------------ */
BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
    (void)lpvReserved;
    if (fdwReason == DLL_PROCESS_ATTACH)
    {
        char path[MAX_PATH];
        GetModuleFileNameA(hinstDLL, path, MAX_PATH);
        /* Replace filename with cTabExtension_x86.dll in same directory */
        char *slash = strrchr(path, '\\');
        if (slash) strcpy_s(slash + 1, MAX_PATH - (slash - path) - 1, "cTabExtension_x86.dll");
        else       strcpy_s(path, MAX_PATH, "cTabExtension_x86.dll");

        g_hImpl = LoadLibraryA(path);
        if (!g_hImpl) return FALSE;

        g_Version = (fn_RVExtensionVersion)         GetProcAddress(g_hImpl, "RVExtensionVersion");
        g_Ext     = (fn_RVExtension)                GetProcAddress(g_hImpl, "RVExtension");
        g_ExtArgs = (fn_RVExtensionArgs)            GetProcAddress(g_hImpl, "RVExtensionArgs");
        g_RegCb   = (fn_RVExtensionRegisterCallback)GetProcAddress(g_hImpl, "RVExtensionRegisterCallback");

        if (!g_Version || !g_Ext || !g_ExtArgs || !g_RegCb)
        {
            FreeLibrary(g_hImpl);
            return FALSE;
        }
    }
    else if (fdwReason == DLL_PROCESS_DETACH)
    {
        if (g_hImpl) FreeLibrary(g_hImpl);
    }
    return TRUE;
}

__declspec(dllexport) void __stdcall RVExtensionVersion(char *output, int outputSize)
{
    if (g_Version) g_Version(output, outputSize);
}

__declspec(dllexport) void __stdcall RVExtension(char *output, int outputSize, const char *function)
{
    if (g_Ext) g_Ext(output, outputSize, function);
}

__declspec(dllexport) int __stdcall RVExtensionArgs(char *output, int outputSize, const char *function, const char **args, int argCount)
{
    if (g_ExtArgs) return g_ExtArgs(output, outputSize, function, args, argCount);
    return 0;
}

__declspec(dllexport) void __stdcall RVExtensionRegisterCallback(void *callback)
{
    if (g_RegCb) g_RegCb(callback);
}
