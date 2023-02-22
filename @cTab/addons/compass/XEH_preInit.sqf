#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

[QGVAR(enable), "CHECKBOX", [LLSTRING(enable), LLSTRING(enableDetails)], ["cTab", LLSTRING(compassCategory)], true] call CBA_fnc_addSetting;
