#include "script_component.hpp"
/*
	(previously cTab_msg_gui_load in player_init.sqf)
*/
#include "\cTab\shared\cTab_gui_macros.hpp"

disableSerialization;
_return = true;
_display = uiNamespace getVariable (cTabIfOpen select 1);
_playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
_msgArray = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];
_msgControl = _display displayCtrl IDC_CTAB_MSG_LIST;
_plrlistControl = _display displayCtrl IDC_CTAB_MSG_RECIPIENTS;
lbClear _msgControl;
lbClear _plrlistControl;
_plrList = playableUnits;
// since playableUnits will return an empty array in single player, add the player if array is empty
if (_plrList isEqualTo []) then {_plrList pushBack cTab_player};
_validSides = call cTab_fnc_getPlayerSides;

// turn this on for testing, otherwise not really usefull, since sending to an AI controlled, but switchable unit will send the message to the player himself
/*if (count _plrList < 1) then { _plrList = switchableUnits;};*/

uiNamespace setVariable ['cTab_msg_playerList', _plrList];
// Messages
{
	_title =  _x select 0;
	_msgState = _x select 2;
	_img = call {
		if (_msgState == 0) exitWith {"\cTab\img\icoUnopenedmail.paa"};
		if (_msgState == 1) exitWith {"\cTab\img\icoOpenmail.paa"};
		if (_msgState == 2) exitWith {"\cTab\img\icon_sentMail_ca.paa"};
	};
	_index = _msgControl lbAdd _title;
	_msgControl lbSetPicture [_index,_img];
	_msgControl lbSetTooltip [_index,_title];
} count _msgArray;

{
	if ((side _x in _validSides) && {isPlayer _x} && {[_x,GVAR(leaderDevices)] call cTab_fnc_checkGear}) then {
		_index = _plrlistControl lbAdd format ["%1:%2 (%3)",groupId group _x,groupId _x,name _x];
		_plrlistControl lbSetData [_index,str _x];
	};
} count _plrList;

lbSort [_plrlistControl, "ASC"];

_return;