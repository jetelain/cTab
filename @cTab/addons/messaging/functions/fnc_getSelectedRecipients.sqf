#include "script_component.hpp"
#include "../defines.hpp"

params ["_control"];

private _plrList = playableUnits;
if (_plrList isEqualTo []) then {
	_plrList pushBack cTab_player
};
private _selectedData = (lbSelection _control) apply { _control lbData _x };

_plrList select { (str _x) in _selectedData }