#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

GVAR(global) = false; 
GVAR(allMarkersGlobal) = [];
GVAR(allMarkersLocal) = [];
GVAR(allMetisMarkersGlobal) = createHashMap;;
GVAR(allMetisMarkersLocal) = createHashMap;

["tacmap", {
    params ["_mode"];
	[_mode == "global"] call FUNC(setGlobal);
}, "admin"] call CBA_fnc_registerChatCommand;