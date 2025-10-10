#include "script_component.hpp"
/*
 	Name: cTab_fnc_setBftState
 	
 	Author(s):
		S3ler

 	Description:
		Set BFT tracking state for an object and save current position and direction.
		Propagates all values over the network.

	Parameters:
		0: OBJECT  - Object to set BFT tracking state
        1: BOOLEAN - Set BFT tracking state, true = enable, false = disable
		2: BOOLEAN - Save current position and direction, true = save position, false = do not save position
 	
 	Returns:
		BOOLEAN - Always TRUE
 	
 	Example:
		[cursorTarget, true, false] call cTab_fnc_setBftState;
		[_vehicle, false, true] call cTab_fnc_setBftState;
*/

params ["_bftObject", "_bftTrackingEnabled", "_saveLasKnownTracking"];


_bftObject setVariable ["CTAB_Has_BFT_Enabled", _bftTrackingEnabled, true];
_bftObject setVariable ["CTAB_Has_Last_Known_Tracking", _saveLasKnownTracking, true];

if (_saveLasKnownTracking) then {
	_bftObject setVariable ["CTAB_Tracking", [_bftTrackingEnabled,_saveLasKnownTracking,(getPosASL _bftObject),(getDir _bftObject), (direction _bftObject), (mapGridPosition _bftObject)], true];
} else {
	_bftObject setVariable ["CTAB_Tracking", [_bftTrackingEnabled,_saveLasKnownTracking,[0,0,0],0, 0, "000000"], true];
};

// return
true
