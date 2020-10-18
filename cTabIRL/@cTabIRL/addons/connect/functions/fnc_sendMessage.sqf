#include "script_component.hpp"

params ['_targetId','_msgBody'];

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
private _time = call cTab_fnc_currentTime;
private _msgTitle = format ["%1 - %2:%3 (%4)",_time,groupId group cTab_player,[cTab_player] call CBA_fnc_getGroupIndex,name cTab_player];

private _recipList = [];

private _groupIndex = cTabBFTgroups findIf { ([group (_x select 0)] call FUNC(getId)) == _targetId };
if ( _groupIndex != -1 ) then {
	_recipList = [(cTabBFTgroups select _groupIndex) select 0];
};

if ( count _recipList > 0 ) then {
	[_msgBody, _recipList] call ctab_core_fnc_sendMessage;
};
