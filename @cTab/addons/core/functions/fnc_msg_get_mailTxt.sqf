#include "script_component.hpp"
#include "\cTab\shared\cTab_gui_macros.hpp"
/*
	(previously cTab_msg_get_mailTxt in player_init.sqf)
*/


disableSerialization;
_return = true;
_index = _this select 1;
_display = uiNamespace getVariable (cTabIfOpen select 1);
_playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
_msgArray = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];
_msgName = (_msgArray select _index) select 0;
_msgtxt = (_msgArray select _index) select 1;
_msgState = (_msgArray select _index) select 2;
if (_msgState == 0) then {
    _msgArray set [_index,[_msgName,_msgtxt,1]];
    cTab_player setVariable [format ["cTab_messages_%1",_playerEncryptionKey],_msgArray];
    [QGVARMAIN(messagesUpdated)] call CBA_fnc_localEvent;
};

_nop = [] call cTab_msg_gui_load;

_txtControl = _display displayCtrl IDC_CTAB_MSG_CONTENT;

_nul = _txtControl ctrlSetText  _msgtxt;

_return;