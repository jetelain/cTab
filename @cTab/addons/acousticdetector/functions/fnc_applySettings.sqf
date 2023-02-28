#include "script_component.hpp"

if ( !hasInterface ) exitWith {};

TRACE_2("applySettings", GVAR(enable), GVAR(isInitialized));

if ( GVAR(enable) ) then {
	if ( !GVAR(isInitialized) ) then {
		LOG("Initialize events for Acoustic gunshot detector");
		["CAManBase", "firedMan", FUNC(firedManEH)] call CBA_fnc_addClassEventHandler;
		["unit", FUNC(updateActiveState)] call CBA_fnc_addPlayerEventHandler;
		["turret", FUNC(updateActiveState)] call CBA_fnc_addPlayerEventHandler;
		["vehicle", FUNC(updateActiveState)] call CBA_fnc_addPlayerEventHandler;
		GVAR(isInitialized) = true;
	};
};

call FUNC(updateActiveState);
