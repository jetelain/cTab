#include "script_component.hpp"
/*
 	Name: cTab_fnc_getBftLastKnownTracking
 	
 	Author(s):
		S3ler

 	Description:
		Get last known BFT position and direction for an object

	Parameters:
		0: OBJECT  - player or vehicle to get last known BFT tracking for

 	Returns:
		BOOLEAN: true if if object has BFT tracking enabled, false otherwise
		BOOLEAN: true if object has last known BFT tracking, false otherwise
		ARRAY: [0,0,0] - last known position
		NUMBER: 0 - last known direction
 	
 	Example:
		[_player] call cTab_fnc_getBftLastKnownTracking;
		[_vehicle] call cTab_fnc_getBftLastKnownTracking;
*/

params ["_bftObject"];
_bftObject getVariable ["CTAB_Tracking", [true, false, [0,0,0], 0, 0]]
