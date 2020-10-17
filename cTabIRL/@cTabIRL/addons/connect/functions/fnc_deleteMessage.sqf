#include "script_component.hpp"

params ['_messageId'];

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
private _msgArray = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];
private _index = _msgArray findIf { count _x == 4 && { (_x select 3) == _messageId } };
if ( _index != -1 ) then {
	[[_index]] call ctab_core_fnc_deleteMessages;
};
