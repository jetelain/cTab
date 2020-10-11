#include "script_component.hpp"

private _data = [];

{
	_data pushBack ['v', [_x select 0] call FUNC(getId), _x select 1, _x select 2, _x select 3, _x select 4, _x select 5, _x select 6];
} forEach cTabBFTvehicles;

{
	private _leader = (_x select 0);
	private _vehicle = vehicle _leader;
	private _vehicleId = '';
	if ( _vehicle != _leader ) then {
		_vehicleId = [vehicle (_x select 0)] call FUNC(getId);
	};
	_data pushBack ['g', [group _leader] call FUNC(getId), _x select 1, _x select 2, _x select 3, _x select 4, _x select 5, 0, _vehicleId];
} forEach cTabBFTgroups;

{
	private _markerData = _x select 1;
	_data pushBack ['u', format ["m%1", _x select 0], _markerData select 1, _markerData select 2, _markerData select 5, '', _markerData select 0, _markerData select 3];
} forEach cTabUserMarkerList;

"cTabExtension" callExtension ["UpdateMarkers", _data];