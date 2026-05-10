#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

[QGVAR(enable), "CHECKBOX", [LLSTRING(enable), LLSTRING(enableDetails)], ["cTab", LLSTRING(acousticdetector)], false, 0, FUNC(applySettings)] call CBA_fnc_addSetting;
[QGVAR(shotsTrackTimeLimit),  "TIME", [LLSTRING(timeLimit), LLSTRING(timeLimitDetails)],  ["cTab", LLSTRING(acousticdetector)], [10, 60, 20]] call CBA_fnc_addSetting;
[QGVAR(shotsCountLimit), "SLIDER", [LLSTRING(countLimit), LLSTRING(countLimitDetails)], ["cTab", LLSTRING(acousticdetector)], [10, 510, 150, 0]] call CBA_fnc_addSetting;
[QGVAR(filterDistance), "SLIDER", [LLSTRING(filterDistance), LLSTRING(filterDistanceDetails)], ["cTab", LLSTRING(acousticdetector)], [0, 100, 50, 0]] call CBA_fnc_addSetting;