#include "script_component.hpp"
/*
 	Name: cTab_fnc_disableBFT

 	Author(s):
		S3ler

 	Description:
		Set BFT tracking state for an object to disabled and save current position and direction.
		Propagates all values over the network.

	Parameters:
		0: OBJECT  - Object to set BFT tracking state
        1: ARRAY   - Optional last known tracking information to use instead of the current position
 	Returns:
		NOTHING

 	Example:
		[cursorTarget] call cTab_fnc_disableBFT;
		[_vehicle, [[-2000, -2000, 45], 0, "000000"]]] call cTab_fnc_disableBFT;
*/

params ["_bftObject", "_lastKnownTracking"];
if (isNil "_lastKnownTracking") then {
    _bftObject setVariable [QGVAR(lastKnownTracking), [(getPosASL _bftObject), (direction _bftObject), [_bftObject] call FUNC(gridPosition)], true];
} else {
    _bftObject setVariable [QGVAR(lastKnownTracking), _lastKnownTracking, true];
};
_bftObject setVariable [QGVAR(enabled), false, true];
