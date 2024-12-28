#include "script_component.hpp"
#include "../defines.hpp"

params ["_button"];

private _display = ctrlParent _button;
private _control = (_display displayCtrl IDC_RECIPIENTS);

private _plrList = playableUnits;
if (_plrList isEqualTo []) then {
	_plrList pushBack cTab_player
};
private _selectedData = (lbSelection _control) apply { _control lbData _x };
private _recipList = _plrList select { (str _x) in _selectedData };

if ( count _recipList >  0) then {
	private _result = [_display] call FUNC(generateTemplateText);
	[_result, _recipList] call FUNC(sendMessage);
	closeDialog 1;
};
