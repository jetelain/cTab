#include "script_component.hpp"

params ['_isAlert'];

private _key = call cTab_fnc_getPlayerEncryptionKey;

// Remove any previous player "TIC" marker
private _rawMarkersList = [cTab_userMarkerLists,_key,[]] call cTab_fnc_getFromPairs;
private _previousMarker = _rawMarkersList findIf {
	((_x select 1) select 1 == 90) && { ((_x select 1) select 5) == cTab_player  }
};
if ( _previousMarker != -1 ) then {
	[_key, (_rawMarkersList select _previousMarker) select 0] call cTab_fnc_deleteUserMarker;
};

// Add "TIC" marker on player position
if ( _isAlert ) then {
	private _markerData = [
		getPosASL cTab_player,
		90,
		0,
		0,
		(call cTab_fnc_currentTime),
		cTab_player
	];
	[_key, _markerData] call cTab_fnc_addUserMarker;
};
