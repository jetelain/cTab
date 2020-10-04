#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

[QGVAR(sync_time),      "TIME", [LLSTRING(sync_time), LLSTRING(sync_timeDetails)], "cTab", [15, 120, 30], 1, {}, false] call CBA_fnc_addSetting;
[QGVAR(bft_mode),       "LIST", [LLSTRING(bft_mode), LLSTRING(bft_modeDetails)], "cTab", [[0, 1, 2], [LLSTRING(disabled), LLSTRING(realTime), LLSTRING(atSync)], 1], 0, {}, false] call CBA_fnc_addSetting;
[QGVAR(helmetcam_mode), "LIST", [LLSTRING(helmetcam_mode), LLSTRING(helmetcam_modeDetails)], "cTab", [[0, 1], [LLSTRING(disabled), LLSTRING(enabled)], 1], 0, {}, false] call CBA_fnc_addSetting;
[QGVAR(uav_mode),       "LIST", [LLSTRING(uav_mode), LLSTRING(uav_modeDetails)], "cTab", [[0, 1], [LLSTRING(disabled), LLSTRING(enabled)], 1], 0, {}, false] call CBA_fnc_addSetting;

if (isClass (configFile >> "CfgWeapons" >> "ACE_microDAGR")) then {
	[QGVAR(useAceMicroDagr), "CHECKBOX", [LLSTRING(useAceMicroDagr), LLSTRING(useAceMicroDagrDetails)], "cTab", true, 0, {}, true] call CBA_fnc_addSetting;
} else {
	GVAR(useAceMicroDagr) = false;
};
