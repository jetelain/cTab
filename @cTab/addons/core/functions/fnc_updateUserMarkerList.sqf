#include "script_component.hpp"
/*
	Name: cTab_fnc_updateUserMarkerList
	
	Author(s):
		Gundy

	Description:
		Update lists of user markers by finding extracting all the user markers with the right encryption key and then translate the marker data in to a format so that it can be drawn quicker.
		
	Parameters:
		NONE
	
	Returns:
		BOOLEAN - Always TRUE
	
	Example:
		call cTab_fnc_updateUserMarkerList;
*/

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;

private _rawMarkersList = [cTab_userMarkerLists,_playerEncryptionKey,[]] call cTab_fnc_getFromPairs;

cTabUserMarkerList = _rawMarkersList apply {  [_x select 0, _x select 1 call cTab_fnc_translateUserMarker, _x select 1] };

[QGVARMAIN(userMarkerListUpdated)] call CBA_fnc_localEvent;

true