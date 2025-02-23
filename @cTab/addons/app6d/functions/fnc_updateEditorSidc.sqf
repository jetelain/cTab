#include "script_component.hpp"
params ["_num", "_pos"];
private _textField = (findDisplay 54) displayCtrl 101;
private _idc = ctrlText _textField;
if (count _idc < 20 ) then {
	_idc = DEFAULT_SIDC;
};
private _len = count _num;
private _newIdc = (_idc select [0, _pos]) + _num + (_idc select [_pos + _len, 20 - _pos - _len]);
if ( _newIdc != _idc ) then {
	_textField ctrlSetText _newIdc;
	_textField ctrlCommit 0;
};
