/*
	(previously cTab_msg_Send in player_init.sqf)
*/
#include "script_component.hpp"
#include "\cTab\shared\cTab_gui_macros.hpp"

disableSerialization;

private _display = uiNamespace getVariable (cTabIfOpen select 1);
private _msgBodyctrl = _display displayCtrl IDC_CTAB_MSG_COMPOSE;
private _plrLBctrl = _display displayCtrl IDC_CTAB_MSG_RECIPIENTS;

private _msgBody = ctrlText _msgBodyctrl;
if (_msgBody isEqualTo "") exitWith {false};

private _recipList = [_plrLBctrl] call EFUNC(messaging,getSelectedRecipients);

// If the message was sent
if ( count _recipList >  0) then {

	[_msgBody, _recipList] call FUNC(sendMessage);

	// remove message body
	_msgBodyctrl ctrlSetText "";
	// clear selected recipients
	_plrLBctrl lbSetCurSel -1;
};

true