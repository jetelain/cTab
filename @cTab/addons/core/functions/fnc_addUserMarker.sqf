#include "script_component.hpp"
/*
	Name: cTab_fnc_addUserMarker
	
	Author(s):
		Gundy
	
	Description:
		Add a new user marker to the list and broadcast it. This function is called on the server.
	
	Parameters:
		0: STRING  - Encryption Key for this marker
		1: ARRAY   - markerData
	Optional:
		2: INTEGER - Transaction ID
	
	Returns:
		BOOLEAN - Always TRUE
	
	Example:
		// Client requesting marker addition and server receiving request
		["bluefor",[[1714.35,5716.82],0,0,0,"12:00",player]]call cTab_fnc_addUserMarker;
		
		// Client receiving marker addition (from server)
		["bluefor",[21,[[1714.35,5716.82],0,0,0,"12:00",player]],157]call cTab_fnc_addUserMarker;
*/

params ["_encryptionKey","_markerData",["_transactionId",-1]];

call {
	// If received on the server
	if (isServer) exitWith {
		if (_transactionId == -1) then {
			// Increase transaction ID
			cTab_userMarkerTransactionId = cTab_userMarkerTransactionId + 1;
			_transactionId = cTab_userMarkerTransactionId;
			
			// Convert to ASL if 2D coordinates
			private _pos = _markerData select 0;
			if ( count _pos == 2 ) then {
				_markerData set [0, ATLToASL [_pos select 0, _pos select 1, 0]];
			};

			// Add marker data to list
			[cTab_userMarkerLists,_encryptionKey,[[_transactionId,_markerData]]] call cTab_fnc_addToPairs;
			
			// Send addUserMarker command to all clients
			[[_encryptionKey,[_transactionId,_markerData],_transactionId],"cTab_fnc_addUserMarker",true,false,true] call bis_fnc_MP;
			
			// If this was run on a client-server (i.e. in single player or locally hosted), update the marker list
			if (hasInterface && {_encryptionKey == call cTab_fnc_getPlayerEncryptionKey}) then {
				call cTab_fnc_updateUserMarkerList;
				if ((_markerData select 5) != cTab_player) then {
					["BFT",format [LLSTRING(newMarker), [_pos] call FUNC(gridPosition)],20] call cTab_fnc_addNotification;
				} else {
					["BFT",LLSTRING(markerAdded),3] call cTab_fnc_addNotification;
				};
			};
		};
	};

	// If received on a client, sent by the server
	if (hasInterface && _transactionId != -1) exitWith {
		call {
			if (cTab_userMarkerTransactionId == _transactionId) exitWith {};
			if (cTab_userMarkerTransactionId != (_transactionId -1)) exitWith {
				// get full list
				["Transaction ID check failed! Had %1, received %2. Requesting user marker list.",cTab_userMarkerTransactionId,_transactionId] call bis_fnc_error;
				[] call cTab_fnc_getUserMarkerList;
			};

			// Convert to ASL if 2D coordinates (should not happen)
			private _pos = _markerData select 1 select 0;
			if ( count _pos == 2 ) then {
				WARNING_2("2D position %1 of marker %2 received from server. Is server using an older version of ctab ?", _pos, _transactionId);
				(_markerData select 1) set [0, ATLToASL [_pos select 0, _pos select 1, 0]];
			};

			cTab_userMarkerTransactionId = _transactionId;
			[cTab_userMarkerLists,_encryptionKey,[_markerData]] call cTab_fnc_addToPairs;
			// only update the user marker list if the marker was added to the player's side
			if (_encryptionKey == call cTab_fnc_getPlayerEncryptionKey) then {
				call cTab_fnc_updateUserMarkerList;
				
				// add notification if marker was issued by someone else
				if ((_markerData select 1 select 5) != cTab_player) then {
					["BFT",format [LLSTRING(newMarker),[_pos] call FUNC(gridPosition)],20] call cTab_fnc_addNotification;
				} else {
					["BFT",LLSTRING(markerAdded),3] call cTab_fnc_addNotification;
				};
			};
		};
	};

	// If received on a client, to be sent to the server
	if (hasInterface) then {
		[_this,"cTab_fnc_addUserMarker",false,false,true] call bis_fnc_MP;
	};
};

true