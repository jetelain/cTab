#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

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
INFO_2("%1: %2",_function,_data);

		if( _function == "AddUserMarker" ) exitWith {
			cTabUserSelIcon = parseSimpleArray _data;
			cTabUserSelIcon pushBack (call cTab_fnc_currentTime);
			cTabUserSelIcon pushBack cTab_player;
			[call cTab_fnc_getPlayerEncryptionKey, cTabUserSelIcon] call cTab_fnc_addUserMarker;
		};
		if( _function == "DeleteUserMarker" ) exitWith {
			(parseSimpleArray _data) params ['_markerId'];
			TRACE_2("DeleteUserMarker", _data, _markerId);
			if (_markerId select [0,1] == "m") then {
				private _markerIndex = parseNumber (_markerId select [1]);
				TRACE_1("call deleteUserMarker", _markerIndex);
				[call cTab_fnc_getPlayerEncryptionKey, _markerIndex] call cTab_fnc_deleteUserMarker;
			};
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
		if( _function == "Error" ) exitWith {
			ERROR(_data);
		};
	};
}];

GVAR(nextId) = 1;
GVAR(nextMessageId) = 1;
GVAR(deviceLevel) = 0;

[QGVAR(enabled), "CHECKBOX", [LLSTRING(enabled), LLSTRING(enabledDetails)], ["cTab",LLSTRING(modName)], true, 0, {}, true] call CBA_fnc_addSetting;
[QGVAR(uri),     "EDITBOX",  [LLSTRING(uri),     LLSTRING(uriDetails)],     ["cTab",LLSTRING(modName)], "http://localhost:5000/hub", 0, {}, true] call CBA_fnc_addSetting;
[QGVAR(key),     "EDITBOX",  [LLSTRING(key),     LLSTRING(keyDetails)],     ["cTab",LLSTRING(modName)], "", 0, {}, true] call CBA_fnc_addSetting;
