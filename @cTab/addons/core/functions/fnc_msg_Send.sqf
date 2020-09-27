/*
	(previously cTab_msg_Send in player_init.sqf)
*/
#include "\cTab\shared\cTab_gui_macros.hpp"

private ["_return","_display","_plrLBctrl","_msgBodyctrl","_plrList","_indices","_time","_msgTitle","_msgBody","_recip","_recipientNames","_msgArray","_playerEncryptionKey"];
disableSerialization;
_return = true;
_display = uiNamespace getVariable (cTabIfOpen select 1);
_playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
_plrLBctrl = _display displayCtrl IDC_CTAB_MSG_RECIPIENTS;
_msgBodyctrl = _display displayCtrl IDC_CTAB_MSG_COMPOSE;
_plrList = (uiNamespace getVariable "cTab_msg_playerList");

_indices = lbSelection _plrLBctrl;

if (_indices isEqualTo []) exitWith {false};

_time = call cTab_fnc_currentTime;
_msgTitle = format ["%1 - %2:%3 (%4)",_time,groupId group cTab_player,[cTab_player] call CBA_fnc_getGroupIndex,name cTab_player];
_msgBody = ctrlText _msgBodyctrl;
if (_msgBody isEqualTo "") exitWith {false};
_recipientNames = "";

{
	_data = _plrLBctrl lbData _x;
	_recip = objNull;
	{
		if (_data == str _x) exitWith {_recip = _x;};
	} count _plrList;
	
	if !(IsNull _recip) then {
		if (_recipientNames isEqualTo "") then {
			_recipientNames = format ["%1:%2 (%3)",groupId group _recip,[_recip] call CBA_fnc_getGroupIndex,name _recip];
		} else {
			_recipientNames = format ["%1; %2",_recipientNames,name _recip];
		};
		
		["cTab_msg_receive",[_recip,_msgTitle,_msgBody,_playerEncryptionKey,cTab_player]] call CBA_fnc_whereLocalEvent;
	};
} forEach _indices;

// If the message was sent
if (_recipientNames != "") then {
	_msgArray = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];
	_msgArray pushBack [format ["%1 - %2",_time,_recipientNames],_msgBody,2];
	cTab_player setVariable [format ["cTab_messages_%1",_playerEncryptionKey],_msgArray];

	if (!isNil "cTabIfOpen" && {[cTabIfOpen select 1,"mode"] call cTab_fnc_getSettings == "MESSAGE"}) then {
		call cTab_msg_gui_load;
	};
	
	// add a notification
	["MSG","Message sent successfully",3] call cTab_fnc_addNotification;
	playSound "cTab_mailSent";
	// remove message body
	_msgBodyctrl ctrlSetText "";
	// clear selected recipients
	_plrLBctrl lbSetCurSel -1;
};

_return;