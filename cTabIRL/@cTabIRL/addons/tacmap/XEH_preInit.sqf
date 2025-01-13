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
		case "global": { if ( IS_ADMIN || isServer ) then { [0] call FUNC(setGlobal); }; };
		case "side":   { if ( IS_ADMIN || isServer ) then { [1] call FUNC(setGlobal); }; };
		case "group" : { if ( leader player == player || { IS_ADMIN || isServer } ) then { [3] call FUNC(setGlobal); }; };
		default       { [-1] call FUNC(setGlobal); };
	};
}, "all"] call CBA_fnc_registerChatCommand;