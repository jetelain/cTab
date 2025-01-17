#include "script_component.hpp"
#include "../defines.hpp"

params ["_button"];

private _display = ctrlParent _button;

[cTabIfOpen select 1, [['mode','TEMPLATELIST']]] call cTab_fnc_setSettings;

private _rect = ctrlPosition (_display displayCtrl IDC_CTAB_GROUP_MESSAGE);

private _targetControl =  _display displayCtrl IDC_TEMPLATELIST;
if !( isNull _targetControl ) then {
	ctrlDelete _targetControl;
};

private _width = (_rect#2) / 2;

_targetControl = _display ctrlCreate ["RscControlsGroup", IDC_TEMPLATELIST];
_targetControl ctrlSetPosition [_rect#0 + (_width/2), _rect#1, _width, _rect#3];
_targetControl ctrlCommit 0;

private _posY = GRID_H;

{
	private _index = _foreachIndex;
	_x params ["", "", "_title"];
	
    private _ctrl = _display ctrlCreate ["ctab_RscButton", IDC_TEMPLATEBUTTON + _index, _targetControl];
    _ctrl ctrlSetPosition [ 0, _posY, _width, (10*GRID_H) ];
	_ctrl ctrlSetText _title;
	_ctrl ctrlAddEventHandler ["ButtonClick", 
	{
		params ["_ctrl"];
		[ctrlParent _ctrl, GVAR(templates) select ((ctrlIDC _ctrl)-IDC_TEMPLATEBUTTON) ] call FUNC(showTemplateUI);
	}];
    _ctrl ctrlCommit 0;
    _posY = _posY + (12*GRID_H); 

} forEach GVAR(templates);

private _ctrl = _display ctrlCreate ["ctab_RscButton", -1, _targetControl];
_ctrl ctrlSetPosition [ _width/4, _posY + (6*GRID_H), _width/2, (10*GRID_H) ];
_ctrl ctrlSetText LLSTRING(Cancel);
_ctrl ctrlAddEventHandler ["ButtonClick", 
{
	params ["_ctrl"];
	[_ctrl, false] call FUNC(closeTemplateUI);
}];
_ctrl ctrlCommit 0;


_ctrl = _display ctrlCreate ["RscStructuredText", -1, _targetControl];
_ctrl ctrlSetPosition [ 0, _posY + (18*GRID_H), _width, (10*GRID_H) ];
_ctrl ctrlSetStructuredText parseText LLSTRING(HowToSetupTemplates);
_ctrl ctrlCommit 0;