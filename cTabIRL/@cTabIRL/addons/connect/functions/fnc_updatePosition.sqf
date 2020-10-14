#include "script_component.hpp"

if ( GVAR(deviceLevel) == 0 ) exitWith { };

private _vehicle = vehicle player;
private _data = getPosASL player;
_data pushBack direction player;
_data pushBack date;
_data pushBack ([group player] call FUNC(getId));
if ( _vehicle != player ) then {
	_data pushBack ([_vehicle] call FUNC(getId));
};

"cTabExtension" callExtension ["UpdatePosition", _data];