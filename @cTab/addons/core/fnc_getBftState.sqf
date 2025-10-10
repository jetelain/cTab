#include "script_component.hpp"
/*
 	Name: cTab_fnc_getBftState

 	Author(s):
		S3ler

 	Description:
		Get state of BFT position tracking for an object

	Parameters:
		0: OBJECT  - player or vehicle to get last known BFT tracking for

 	Returns:
		BOOLEAN - true if tracking is enabled else false

 	Example:
		[cursorTarget] call cTab_fnc_getBftState;
		[_vehicle] call cTab_fnc_getBftState;
*/

params ["_bftObject"];

_bftObject getVariable ["CTAB_Has_BFT_Enabled", true];
