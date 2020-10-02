#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

[QGVAR(sync_time),      "TIME", ["Synchronization rate", "Time required to discover new devices and units"], "cTab", [15, 120, 30], 1, {}, false] call CBA_fnc_addSetting;
[QGVAR(bft_mode),       "LIST", ["Blue Force Tracking", "Allow tracking of units equiped with cTab items. Position may be updated at each synchronization, or can be real time"], "cTab", [[0, 1, 2], ["Disabled", "Real time", "At each synchronization"], 1], 0, {}, false] call CBA_fnc_addSetting;
[QGVAR(helmetcam_mode), "LIST", ["Helmet cam", "Allow usage of helmet cam on tablet"], "cTab", [[0, 1], ["Disabled", "Enabled"], 1], 0, {}, false] call CBA_fnc_addSetting;
[QGVAR(uav_mode),       "LIST", ["UAV feed", "Allow usage of UAV feed on tablet"], "cTab", [[0, 1], ["Disabled", "Enabled"], 1], 0, {}, false] call CBA_fnc_addSetting;
