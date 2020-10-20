#include "script_component.hpp"

params ["_unit"];
private _id = _unit getVariable [QGVAR(id),""];
if (_id == "") then {
	_id = format ['o%1',GVAR(nextId)];
	GVAR(nextId) = GVAR(nextId) + 1;
	_unit setVariable [QGVAR(id), _id];
};
_id