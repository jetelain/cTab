#include "script_component.hpp"
/*
 	Name: cTab_fnc_enableBFT

 	Author(s):
		S3ler

 	Description:
		Set BFT tracking state for an object to enabled.
		Propagates value over the network.

	Parameters:
		0: OBJECT  - Object to set BFT tracking state to enabled

 	Returns:
		NOTHING

 	Example:
		[cursorTarget] call cTab_fnc_enableBFT;
		[_vehicle] call cTab_fnc_enableBFT;
*/

params ["_bftObject"];
_bftObject setVariable [QGVAR(enabled), true, true];
