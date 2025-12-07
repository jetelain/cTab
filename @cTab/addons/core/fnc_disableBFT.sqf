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

 	Returns:
		NOTHING

 	Example:
		[cursorTarget] call cTab_fnc_disableBFT;
		[_vehicle] call cTab_fnc_disableBFT;
*/

params ["_bftObject"];
_bftObject setVariable [QGVAR(lastKnownTracking), [(getPosASL _bftObject), (direction _bftObject), (mapGridPosition _bftObject)], true];
_bftObject setVariable [QGVAR(enabled), false, true];
