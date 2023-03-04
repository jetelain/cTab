#include "\x\cba\addons\main\script_macros_common.hpp"

#define DFUNC(var1) TRIPLES(ADDON,fnc,var1)

#ifdef DISABLE_COMPILE_CACHE
    #undef PREP
    #define PREP(fncName) DFUNC(fncName) = compile preprocessFileLineNumbers QPATHTOF(functions\DOUBLES(fnc,fncName).sqf)
#else
    #undef PREP
    #define PREP(fncName) [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf), QFUNC(fncName)] call CBA_fnc_compileFunction
#endif

#ifdef PROFILER
    #define PREP_RET(fncName)         [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),true,false] call ctab_fnc_compileFunction
    #define PREP_VOID(fncName)        [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),false,false] call ctab_fnc_compileFunction
    #define PREP_RET_HF(fncName)      [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),true,true] call ctab_fnc_compileFunction
    #define PREP_VOID_HF(fncName)     [QPATHTOF(functions\DOUBLES(fnc,fncName).sqf),QFUNC(fncName),false,true] call ctab_fnc_compileFunction
#else
    #define PREP_RET(fncName)         PREP(fncName)
    #define PREP_VOID(fncName)        PREP(fncName)
    #define PREP_RET_HF(fncName)      PREP(fncName)
    #define PREP_VOID_HF(fncName)     PREP(fncName)
#endif
