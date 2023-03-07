#include "\x\cba\addons\main\script_macros_common.hpp"

#define DFUNC(var1) TRIPLES(ADDON,fnc,var1)
#define DFUNCMAIN(var1) TRIPLES(PREFIX,fnc,var1)

#ifdef DISABLE_COMPILE_CACHE
    #undef PREP
    #define PREP(fncName) DFUNC(fncName) = compile preprocessFileLineNumbers QPATHTOF(functions\DOUBLES(fnc,fncName).sqf)
    #undef PREPMAIN
    #define PREPMAIN(fncName) DFUNCMAIN(fncName) = compile preprocessFileLineNumbers QPATHTOF(functions\DOUBLES(fnc,fncName).sqf)
#else
    #undef PREP
    #define PREP(fncName) [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf), QFUNC(fncName)] call CBA_fnc_compileFunction
    #undef PREPMAIN
    #define PREPMAIN(fncName) [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf), QFUNCMAIN(fncName)] call CBA_fnc_compileFunction
#endif

#ifdef PROFILER
    #define PREP_RET(fncName)         [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),true,false] call bsp_fnc_compileFunction
    #define PREP_VOID(fncName)        [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),false,false] call bsp_fnc_compileFunction
    #define PREP_RET_HF(fncName)      [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),true,true] call bsp_fnc_compileFunction
    #define PREP_VOID_HF(fncName)     [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),false,true] call bsp_fnc_compileFunction
    #define PREPMAIN_RET(fncName)     [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNCMAIN(fncName),true,false] call bsp_fnc_compileFunction
    #define PREPMAIN_VOID(fncName)    [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNCMAIN(fncName),false,false] call bsp_fnc_compileFunction
    #define PREPMAIN_RET_HF(fncName)  [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNCMAIN(fncName),true,true] call bsp_fnc_compileFunction
    #define PREPMAIN_VOID_HF(fncName) [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNCMAIN(fncName),false,true] call bsp_fnc_compileFunction
#else
    #define PREP_RET(fncName)         PREP(fncName)
    #define PREP_VOID(fncName)        PREP(fncName)
    #define PREP_RET_HF(fncName)      PREP(fncName)
    #define PREP_VOID_HF(fncName)     PREP(fncName)
    #define PREPMAIN_RET(fncName)     PREPMAIN(fncName)
    #define PREPMAIN_VOID(fncName)    PREPMAIN(fncName)
    #define PREPMAIN_RET_HF(fncName)  PREPMAIN(fncName)
    #define PREPMAIN_VOID_HF(fncName) PREPMAIN(fncName)
#endif
