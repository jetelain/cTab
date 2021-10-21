#include "script_component.hpp"
#include "\cTab\shared\cTab_gui_macros.hpp"
/*
    (previously cTab_msg_Send in player_init.sqf)
*/


private ["_display","_plrLBctrl","_msgBodyctrl","_plrList","_indices","_msgBody"];
disableSerialization;

_display = uiNamespace getVariable (cTabIfOpen select 1);
_plrLBctrl = _display displayCtrl IDC_CTAB_MSG_RECIPIENTS;
_msgBodyctrl = _display displayCtrl IDC_CTAB_MSG_COMPOSE;
_plrList = (uiNamespace getVariable "cTab_msg_playerList");

_indices = lbSelection _plrLBctrl;
if (_indices isEqualTo []) exitWith {false};

_msgBody = ctrlText _msgBodyctrl;
if (_msgBody isEqualTo "") exitWith {false};

private _recipList = [];

{
    private _data = _plrLBctrl lbData _x;
    private _recip = objNull;
    {
        if (_data == str _x) exitWith {_recip = _x;};
    } count _plrList;
    
    if !(isNull _recip) then {

        _recipList pushBack _recip;
    };
} forEach _indices;

// If the message was sent
if ( count _recipList >  0) then {

    [_msgBody, _recipList] call FUNC(sendMessage);

    // remove message body
    _msgBodyctrl ctrlSetText "";
    // clear selected recipients
    _plrLBctrl lbSetCurSel -1;
};

true