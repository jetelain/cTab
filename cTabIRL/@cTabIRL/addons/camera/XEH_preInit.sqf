#include "script_component.hpp"

if (!hasInterface) exitWith { };

[QGVAR(mode), "LIST", [LLSTRING(mode), LLSTRING(modeTooltip)], ["cTab",LELSTRING(connect,modName)], [[0, 1, 2], [LLSTRING(mode0),LLSTRING(mode1),LLSTRING(mode2)], 0]] call CBA_fnc_addSetting;
