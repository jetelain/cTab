#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

addMissionEventHandler ["ExtensionCallback", {
	params ["_name", "_function", "_data"];
	if ( _name == "ctab" ) then {

#ifdef DEBUG_MODE_FULL
		if( _function == "Debug" ) exitWith {
			LOG(_data);
		};
#endif
		if( _function == "AddUserMarker" ) exitWith {
			cTabUserSelIcon = parseSimpleArray _data;
			cTabUserSelIcon pushBack (call cTab_fnc_currentTime);
			cTabUserSelIcon pushBack cTab_player;
			TRACE_2("AddUserMarker", _data, cTabUserSelIcon);
			[call cTab_fnc_getPlayerEncryptionKey, cTabUserSelIcon] call cTab_fnc_addUserMarker;
		};
	};
}];

GVAR(nextId) = 1;
GVAR(deviceLevel) = 0;
