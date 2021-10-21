#include "script_component.hpp"
#include "\cTab\shared\cTab_gui_macros.hpp"
/*
    Function called when DELETE button is pressed in messaging mode
    Parameter 0: Name of uiNameSpace variable of display
    Returns false if nothing was selected for deletion, else returns true

    (previously in player_init.sqf)
*/


private _displayName = _this select 0;
private _display = uiNamespace getVariable _displayName;
private _msgLbCtrl = _display displayCtrl IDC_CTAB_MSG_LIST;
private _msgLbSelection = lbSelection _msgLbCtrl;

if (count _msgLbSelection == 0) exitWith {false};

[_msgLbSelection] call FUNC(deleteMessages);

private _msgTextCtrl = _display displayCtrl IDC_CTAB_MSG_CONTENT;
_msgTextCtrl ctrlSetText LLSTRING(NoMessageSelected);

true