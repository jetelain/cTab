/*
	(previously cTab_msg_get_mailTxt in player_init.sqf)
*/
#include "script_component.hpp"
#include "\cTab\shared\cTab_gui_macros.hpp"

params ["_control", "_index"];

private _display = ctrlParent _control;

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
private _messageList = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];
private _message = (_messageList select _index);

_message params ["", "_msgtxt", "_msgState"];

if (_msgState == 0) then {
    _message set [2, 1];
	(_display displayCtrl IDC_CTAB_MSG_LIST) lbSetPicture [_index,"\cTab\img\icoOpenmail.paa"];
    [QGVARMAIN(messagesUpdated)] call CBA_fnc_localEvent;
};

(_display displayCtrl IDC_CTAB_MSG_CONTENT) ctrlSetText _msgtxt;
