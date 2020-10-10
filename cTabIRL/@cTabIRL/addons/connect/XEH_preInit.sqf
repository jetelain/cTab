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

	};
}];

GVAR(nextId) = 1;