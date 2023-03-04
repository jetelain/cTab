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

		GVAR(red1) = +cTabColorRed;
		GVAR(red2) = +cTabColorRed;
		GVAR(red2) set [3,(GVAR(red2)#3)/2];

		EGVAR(core,bftDrawHandlers) pushBack {
			if ( !GVAR(isActive) ) exitWith { };
			params ['_ctrl'];
			{
				_x params ['_time','','_point','_radius'];
				_ctrl drawEllipse  [_point, _radius, _radius, 0, if ((diag_tickTime-_time)<2.5) then {GVAR(red1)} else {GVAR(red2)}, "#(argb,8,8,3)color(1,1,1,0.5)"];
			} forEach GVAR(detectedShots);
		};

		GVAR(isInitialized) = true;
	};
};

call FUNC(updateActiveState);
