#include "script_component.hpp"
/*
 	Name: cTab_fnc_getPlayerPosition

 	Author(s):
		S3ler

 	Description:
		Determine player position as ASL coordinates for drawing/rendering

	Parameters:
		_veh: OBJECT  - Map control to return map center coordinates for

 	Returns:
		ARRAY - 3D ASL coordinates of given _veh

 	Example:
		[_veh] call cTab_fnc_getPlayerPosition;
*/

params ["_veh"];
if (_veh getVariable [QGVAR(enabled), true]) exitWith {
    getPosASL _veh
};

(_veh getVariable [QGVAR(lastKnownTracking), [[0, 0, 0], 0, "000000"]]) select 0
