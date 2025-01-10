#include "script_component.hpp"
#include "../defines.hpp"

params ["_button", ["_messageSent", false]];

private _display = ctrlParent _button;

private _targetControl =  _display displayCtrl IDC_TEMPLATEBLOCK;
if !( isNull _targetControl ) then {
	ctrlDelete _targetControl;
};

private _displayName = cTabIfOpen select 1;
if ( _displayName == "cTab_Tablet_dlg" ) then {
	['cTab_Tablet_dlg',[['mode','MESSAGE']]] call cTab_fnc_setSettings;
} else{
	if ( _messageSent ) then {
		['cTab_Android_dlg',[['mode','BFT']]] call cTab_fnc_setSettings;
	} else {
		['cTab_Android_dlg',[['mode','COMPOSE']]] call cTab_fnc_setSettings;
	};
};
