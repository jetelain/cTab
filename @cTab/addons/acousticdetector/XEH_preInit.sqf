#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

[QGVAR(enable), "CHECKBOX", [LLSTRING(enable), LLSTRING(enableDetails)], ["cTab", LLSTRING(acousticdetector)], false, 0, FUNC(applySettings)] call CBA_fnc_addSetting;
[QGVAR(shotsTrackTimeLimit),  "TIME", [LLSTRING(timeLimit), LLSTRING(timeLimitDetails)],  ["cTab", LLSTRING(acousticdetector)], [10, 120, 30]] call CBA_fnc_addSetting;

