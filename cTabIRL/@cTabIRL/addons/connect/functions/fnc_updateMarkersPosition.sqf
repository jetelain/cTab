#include "script_component.hpp"

params ['_send'];

private _data = [];

{
	private _vehicle = (_x select 0);
	_x set [5, getPosASL _vehicle];
	_x set [6, direction _vehicle];
	if ( _send ) then {
		_data pushBack [_vehicle call FUNC(getId), _x select 5, _x select 6];
	};
} forEach cTabBFTvehicles;

{
	private _leader = (_x select 0);
	_x set [5, getPosASL _leader];
	if ( _send ) then {
		_data pushBack [[group _leader] call FUNC(getId), _x select 5, 0];
	};
} forEach cTabBFTgroups;

if ( _send ) then {
	"cTabExtension" callExtension ["UpdateMarkersPosition", _data];
};