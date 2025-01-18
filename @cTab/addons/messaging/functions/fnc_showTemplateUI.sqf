#include "script_component.hpp"
#include "../defines.hpp"

params ["_display", "_data"];

private _rect = ctrlPosition (_display displayCtrl IDC_CTAB_GROUP_MESSAGE);

[cTabIfOpen select 1, [['mode','TEMPLATE']]] call cTab_fnc_setSettings;

private _targetControl =  _display displayCtrl IDC_TEMPLATEBLOCK;
if !( isNull _targetControl ) then {
	ctrlDelete _targetControl;
};

_targetControl = _display ctrlCreate ["RscControlsGroup", IDC_TEMPLATEBLOCK];
_targetControl ctrlSetPosition [_rect#0 + GRID_W, _rect#1, _rect#2 - (2 * GRID_W), _rect#3];
_targetControl ctrlCommit 0;

[_display, _data, _targetControl] call FUNC(generateTemplateUI);

