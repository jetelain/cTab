#include "script_component.hpp"

if ( GVAR(deviceLevel) < 3 ) exitWith { };

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
private _msgArray = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];

// Allocate an id to each message that do not have already one
{
	if ( count _x == 3 ) then {
		_x pushBack format ['m%1', GVAR(nextMessageId)];
		GVAR(nextMessageId) = GVAR(nextMessageId) + 1;
	};
} forEach _msgArray;

"cTabExtension" callExtension ["UpdateMessages", _msgArray];