#include "script_component.hpp"

params ['_targetId','_msgBody',['_data',[]]];

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
private _time = call cTab_fnc_currentTime;
private _msgTitle = format ["%1 - %2:%3 (%4)",_time,groupId group cTab_player,[cTab_player] call CBA_fnc_getGroupIndex,name cTab_player];

private _recipList = [];

private _groupIndex = cTabBFTgroups findIf { ([group (_x select 0)] call FUNC(getId)) == _targetId };
if ( _groupIndex != -1 ) then {
	_recipList = [(cTabBFTgroups select _groupIndex) select 0];
};

if ( count _recipList > 0 ) then {

	if !(isNil "ctab_messaging_fnc_sendMessage") exitWith {
		// New messaging system is available

		if (count _data == 0) then {
			// WebApp is not up-to-date, generate new system format
			_data = ["", null, 0, []];
		};

		// Data should be ["_title", "_message", "_messageType", "_attachments"];
		// but to keep compatibility with old version, server has sent ["_title", null, "_messageType", "_attachments"] and _message as _msgBody
		// so set _message with content of _msgBody to respect the new format
		_data set [1, _msgBody];

		[_data, _recipList] call ctab_messaging_fnc_sendMessage;
	};

	// Old messaging system
	[_msgBody, _recipList] call ctab_core_fnc_sendMessage;
};
