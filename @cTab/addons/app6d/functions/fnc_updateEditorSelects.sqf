#include "script_component.hpp"

if (!GVAR(editorActive)) exitWith {};

GVAR(isFillingLB) = true;

params ["_ctrl"];
private _idc = ctrlText _ctrl;
private _display = ctrlParent _ctrl;
private _stdid = -1;

private _ensureSet = {
	params ["_set"];
	if (_set != GVAR(editingSet)) then {
		private _setIndex = GVAR(sets) findIf { (_x # 0) isEqualTo _set };
		if (_setIndex != -1) then {
			private _setData = GVAR(sets) select _setIndex;
			{
				private _items = _setData select (_forEachIndex + 2);
				private _lb = _display displayCtrl _x;
				lbClear _lb;
				{
					_lb lbAdd (_x # 1);
					_lb lbSetData [_forEachIndex, _x # 0];
				} forEach _items;
			} forEach [IDC_LB_AMP,IDC_LB_MOD1,IDC_LB_MOD2,IDC_LB_ICON];
		};
	};
};

if ( count _idc >= 20 ) then {
	private _updateIndex = {
		params ["_ctrl","_index"];
		if ( lbCurSel _ctrl != _index && { _index < lbSize _ctrl } ) then { _ctrl lbSetCurSel _index; _ctrl ctrlCommit 0; };
	};
	private _updateData = {
		params ["_ctrl","_value"];
		private _index = -1;
		for "_i" from 0 to ((lbSize _ctrl)-1) do { 
			if ( _ctrl lbData _i == _value ) exitWith { _index = _i; };
		};
		if ( _index != -1 && lbCurSel _ctrl != _index ) then { _ctrl lbSetCurSel _index; _ctrl ctrlCommit 0; };
	};

	[_display displayCtrl 9910, parseNumber (_idc select [6, 1]) ] call _updateIndex;
	[_display displayCtrl 9911, parseNumber (_idc select [7, 1]) ] call _updateIndex;
	[_idc select [4, 2]] call _ensureSet;
	[_display displayCtrl IDC_LB_SET, _idc select [4, 2]] call _updateData;
	[_display displayCtrl IDC_LB_AMP, _idc select [8, 2]] call _updateData;
	[_display displayCtrl IDC_LB_MOD1, _idc select [16, 2]] call _updateData;
	[_display displayCtrl IDC_LB_MOD2, _idc select [18, 2]] call _updateData;
	[_display displayCtrl IDC_LB_ICON, _idc select [10, 6]] call _updateData;

	_stdid = parseNumber(_idc select [3, 1]);
} else {
	["10"] call _ensureSet;
};

{(_display displayCtrl _x) ctrlShow (_forEachIndex == _stdid);} forEach [9710,9711,9712,9713,9714,9715,9716];

GVAR(isFillingLB) = false;