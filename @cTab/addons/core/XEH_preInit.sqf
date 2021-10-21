#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

[QGVAR(sync_time),     "TIME", [LLSTRING(sync_time), LLSTRING(sync_timeDetails)], "cTab", [15, 120, 30], 1, {}, false] call CBA_fnc_addSetting;

[QGVAR(bft_mode),      "LIST", [LLSTRING(bft_mode), LLSTRING(bft_modeDetails)], ["cTab", LLSTRING(mapCategory)], [[0, 1, 2], [LLSTRING(disabled), LLSTRING(realTime), LLSTRING(atSync)], 1], 0, {}, false] call CBA_fnc_addSetting;
[QGVAR(useMils),       "LIST", [LLSTRING(useMils), LLSTRING(useMilsDetails)], ["cTab", LLSTRING(mapCategory)], [[false, true], [LLSTRING(useMils_false), LLSTRING(useMils_true)], 0]] call CBA_fnc_addSetting;
[QGVAR(defMapStyle),   "LIST", [LLSTRING(defMapStyle), LLSTRING(defMapStyleDetails)], ["cTab", LLSTRING(mapCategory)], [["SAT", "TOPO"], [LLSTRING(satStyle), LLSTRING(topoStyle)], 0], 0, {}, true] call CBA_fnc_addSetting;
[QGVAR(useArmaMarker), "LIST", [LLSTRING(useArmaMarker), LLSTRING(useArmaMarkerDetails)], ["cTab", LLSTRING(mapCategory)], [[true, false], [LLSTRING(useArmaMarker_true), LLSTRING(useArmaMarker_false)], 0]] call CBA_fnc_addSetting;
[QGVAR(gridPrecision), "LIST", [LLSTRING(gridPrecision), LLSTRING(gridPrecisionDetails)], ["cTab", LLSTRING(mapCategory)], [[0, 1, 2], [LLSTRING(gridPrecision0), LLSTRING(gridPrecision1), LLSTRING(gridPrecision2)], 0]] call CBA_fnc_addSetting;

if (isClass (configFile >> "CfgWeapons" >> "ACE_microDAGR")) then {
    [QGVAR(useAceMicroDagr), "CHECKBOX", [LLSTRING(useAceMicroDagr), LLSTRING(useAceMicroDagrDetails)], ["cTab", LLSTRING(mapCategory)], true, 0, {}, true] call CBA_fnc_addSetting;
} else {
    GVAR(useAceMicroDagr) = false;
};


[QGVAR(helmetcam_mode), "LIST", [LLSTRING(helmetcam_mode), LLSTRING(helmetcam_modeDetails)], ["cTab", LLSTRING(videoCategory)], [[0, 1], [LLSTRING(disabled), LLSTRING(enabled)], 1], 0, {}, false] call CBA_fnc_addSetting;
[QGVAR(uav_mode),       "LIST", [LLSTRING(uav_mode), LLSTRING(uav_modeDetails)], ["cTab", LLSTRING(videoCategory)], [[0, 1], [LLSTRING(disabled), LLSTRING(enabled)], 1], 0, {}, false] call CBA_fnc_addSetting;

