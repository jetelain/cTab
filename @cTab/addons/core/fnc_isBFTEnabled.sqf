#include "script_component.hpp"
/*
 	Name: cTab_fnc_isBFTEnabled

 	Author(s):
		S3ler

 	Description:
		get BFT tracking state for an object.

	Parameters:
		0: OBJECT  - Object to set BFT tracking state

 	Returns:
		BOOLEAN - TRUE when tracking is enabled (or not set yet). False if tracking is disabled.

 	Example:
		[cursorTarget] call cTab_fnc_isBFTEnabled;
		[_vehicle] call cTab_fnc_isBFTEnabled;
*/

params ["_bftObject"];
_bftObject getVariable [QGVAR(enabled), true];
