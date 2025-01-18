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

		EGVAR(core,bftDrawHandlers) pushBack {
			if ( !GVAR(isActive) ) exitWith { };
			params ['_ctrl'];
			{
				_x params ['','','_point','_radius'];
				_ctrl drawEllipse  [_point, _radius, _radius, 0, cTabColorRed, "#(argb,8,8,3)color(1,1,1,0.5)"];
			} forEach GVAR(detectedShots);
		};

		GVAR(isInitialized) = true;
	};
};

call FUNC(updateActiveState);
