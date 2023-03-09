#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

if (isServer) then {
	// Generate an (almost) unique identifier for game session
	private _chars = ['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9']; 
	private _data = systemTimeUTC apply { _chars select (_x % (count _chars))}; 
	for "_i" from 1 to 33 do { // => total of 40 chars
		_data pushBack (selectRandom  _chars); 
	}; 
	GVAR(sessionId) = _data joinString ""; 
	publicVariable QGVAR(sessionId);
};

if (!hasInterface) exitWith { };

addMissionEventHandler ["ExtensionCallback", {
	params ["_name", "_function", "_data"];
	if ( _name == "ctab" ) then {

#ifdef DEBUG_MODE_FULL
		if( _function == "Log" ) exitWith {
			LOG(_data);
		};
#endif
		TRACE_2("ExtensionCallback", _function, _data);
		if( _function == "AddUserMarker" ) exitWith {
			cTabUserSelIcon = parseSimpleArray _data;
			cTabUserSelIcon pushBack (call cTab_fnc_currentTime);
			cTabUserSelIcon pushBack cTab_player;
			[call cTab_fnc_getPlayerEncryptionKey, cTabUserSelIcon] call cTab_fnc_addUserMarker;
		};
		if( _function == "DeleteUserMarker" ) exitWith {
			(parseSimpleArray _data) params ['_markerId'];
			if (_markerId select [0,1] == "m") then {
				private _markerIndex = parseNumber (_markerId select [1]);
				[call cTab_fnc_getPlayerEncryptionKey, _markerIndex] call cTab_fnc_deleteUserMarker;
			};
		};
		if( _function == "TicAlert" ) exitWith {
			(parseSimpleArray _data) call ctab_core_fnc_ticAlert;
		};
		if( _function == "SendMessage" ) exitWith {
			(parseSimpleArray _data) call FUNC(sendMessage);
		};
		if( _function == "Connected" ) exitWith {
			private _args = parseSimpleArray _data;
			_args call FUNC(onConnected);
		};
		if(_function == "MessageRead") exitWith {
			(parseSimpleArray _data) call FUNC(markMessageRead);
		};
		if(_function == "DeleteMessage") exitWith {
			(parseSimpleArray _data) call FUNC(deleteMessage);
		};
		if ( _function == "Reconnecting") exitWith {
			systemChat LLSTRING(reconnecting);
		};
		if ( _function == "Disconnected") exitWith {
			systemChat LLSTRING(disconnected);
		};
		if ( _function == "ClearTacMapMarkers") exitWith {
			call EFUNC(tacmap,clear);
		};
		if ( _function == "AddTacMapMarker") exitWith {
			(parseSimpleArray _data) call EFUNC(tacmap,create);
		};
		if ( _function == "RemoveTacMapMarker") exitWith {
			(parseSimpleArray _data) call EFUNC(tacmap,delete);
		};
		if ( _function == "Disconnected") exitWith {
			systemChat LLSTRING(disconnected);
		};
		if( _function == "Error" ) exitWith {
			ERROR(_data);
		};
	};
}];

GVAR(nextId) = 1;
GVAR(nextMessageId) = 1;
GVAR(deviceLevel) = 0;
GVAR(vehicleMode) = 0;
GVAR(mapMarkersNeedsUpdate) = true;
GVAR(trackDevices) = ["ItemcTab", "ItemAndroid"];

[QGVAR(enabled), "CHECKBOX", [LLSTRING(enabled), LLSTRING(enabledDetails)], ["cTab",LLSTRING(modName)], true, 0, {}, true] call CBA_fnc_addSetting;
#ifdef DEBUG_BACKEND
[QGVAR(uri),     "EDITBOX",  [LLSTRING(uri),     LLSTRING(uriDetails)],     ["cTab",LLSTRING(modName)], "http://localhost:5000/hub", 0, {}, true] call CBA_fnc_addSetting;
#else
[QGVAR(uri),     "EDITBOX",  [LLSTRING(uri),     LLSTRING(uriDetails)],     ["cTab",LLSTRING(modName)], "https://ctab.plan-ops.fr/hub", 0, {}, true] call CBA_fnc_addSetting;
#endif
[QGVAR(key),     "EDITBOX",  [LLSTRING(key),     LLSTRING(keyDetails)],     ["cTab",LLSTRING(modName)], "", 0, {}, true] call CBA_fnc_addSetting;

[QGVAR(syncMap), "CHECKBOX", [LLSTRING(syncMap), LLSTRING(syncMapDetails)], ["cTab",LLSTRING(modName)], true] call CBA_fnc_addSetting;
