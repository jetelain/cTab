#include "script_component.hpp"
#include "../defines.hpp"

params ["_display"];

private _targetControl =  _display displayCtrl IDC_INTELFEED_BLOCK;
private _list = controlNull;

if ( isNull _targetControl ) then {

	private _rect = ctrlPosition (_display displayCtrl IDC_CTAB_GROUP_MESSAGE);
	private _width = _rect#2 - (2 * GRID_W);
	private _height = _rect#3 - (2 * GRID_H);

	_targetControl = _display ctrlCreate ["RscControlsGroup", IDC_INTELFEED_BLOCK];
	_targetControl ctrlSetPosition [_rect#0 + GRID_W, _rect#1 + GRID_H, _rect#2 - (2 * GRID_W), _rect#3];
	_targetControl ctrlCommit 0;

	_list = _display ctrlCreate ["RscListBox", IDC_INTELFEED_LIST, _targetControl];
	_list ctrlSetPosition [0, 0, (_width/3)-GRID_W, _height- (9*GRID_H)];
	_list ctrlCommit 0;
	_list ctrlAddEventHandler ["LBSelChanged",
	{
		params ["_ctrl"];
		private _index = lbCurSel _ctrl;
		private _id = parseNumber (_ctrl lbData _index);
		[ctrlParent _ctrl, _id] call FUNC(showIntelFeedDetails);
	}];

	private _ctrl = _display ctrlCreate ["RscHTML", IDC_INTELFEED_HTMLVIEW, _targetControl];
	_ctrl ctrlSetPosition [_width/3, 0, (_width/3)*2, _height];
	_ctrl ctrlCommit 0;

	_ctrl = _display ctrlCreate ["cTab_RscButton_Danger", -1, _targetControl];
	_ctrl ctrlSetText "Delete";
	_ctrl ctrlSetPosition [0, _height - (8*GRID_H), (_width/3)-GRID_W, GRID_H*8];
	_ctrl ctrlCommit 0;
	_ctrl ctrlAddEventHandler ["ButtonClick",
	{
		params ["_ctrl"];
		private _list = (ctrlParent _ctrl) displayCtrl IDC_INTELFEED_LIST;
		private _index = lbCurSel _list;
		if ( _index != -1 ) then {
			[_list lbData _index] call FUNC(removeItem);
		};
	}];

	if (isNil QGVAR(uiHandler)) then {
		GVAR(uiHandler) = [QGVAR(feed), {
			private _list = uiNamespace getVariable [QGVAR(feedList), controlNull]; 
			if ( isNull _list ) exitWith {};
			[_list] call FUNC(fillIntelFeedList);
		}] call CBA_fnc_addEventHandler;
	};
} else {
	_list = _display displayCtrl IDC_INTELFEED_LIST;  
};

[_list] call FUNC(fillIntelFeedList);

uiNamespace setVariable [QGVAR(feedList), _list];
