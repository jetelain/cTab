#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

GVAR(channel) = -1; 
GVAR(allIconMarkers) = createHashMap;
GVAR(allLineMarkers) = createHashMap;
GVAR(allMetisMarkers) = createHashMap;

["tacmap", {
    params ["_mode"];
	switch (_mode) do {
		case "global": { [0] call FUNC(setGlobal); };
		case "side":   { [1] call FUNC(setGlobal); };
		case "group" : { [3] call FUNC(setGlobal); };
		default       { [-1] call FUNC(setGlobal); };
	};
}, "admin"] call CBA_fnc_registerChatCommand;