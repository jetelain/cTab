#include "script_component.hpp"
/*
	(previously cTab_msg_delete_all in player_init.sqf)
*/
_playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
cTab_player setVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];
[QGVARMAIN(messagesUpdated)] call CBA_fnc_localEvent;
