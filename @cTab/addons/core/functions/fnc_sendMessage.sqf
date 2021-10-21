#include "script_component.hpp"

params ["_msgBody", "_recipList"];

if (_msgBody isEqualTo "") exitWith {false};
if (_recipList isEqualTo []) exitWith {false};

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
private _time = call cTab_fnc_currentTime;
private _msgTitle = format ["%1 - %2:%3 (%4)",_time,groupId group cTab_player,[cTab_player] call CBA_fnc_getGroupIndex,name cTab_player];
private _recipientNames = "";

{
    private _recip = _x;
    if (_recipientNames isEqualTo "") then {
        _recipientNames = format ["%1:%2 (%3)",groupId group _recip,[_recip] call CBA_fnc_getGroupIndex,name _recip];
    } else {
        _recipientNames = format ["%1; %2",_recipientNames,name _recip];
    };
    ["cTab_msg_receive",[_recip,_msgTitle,_msgBody,_playerEncryptionKey,cTab_player]] call CBA_fnc_whereLocalEvent;
} forEach _recipList;

private _msgArray = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];
_msgArray pushBack [format ["%1 - %2",_time,_recipientNames],_msgBody,2];
cTab_player setVariable [format ["cTab_messages_%1",_playerEncryptionKey],_msgArray];

if (!isNil "cTabIfOpen" && {[cTabIfOpen select 1,"mode"] call cTab_fnc_getSettings == "MESSAGE"}) then {
    call cTab_msg_gui_load;
};

// add a notification
["MSG",LLSTRING(MessageSent),3] call cTab_fnc_addNotification;
playSound "cTab_mailSent";

[QGVARMAIN(messagesUpdated)] call CBA_fnc_localEvent;

true