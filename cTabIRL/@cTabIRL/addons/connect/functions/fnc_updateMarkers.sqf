#include "script_component.hpp"

if ( GVAR(deviceLevel) == 0 ) exitWith { };

if ( ctab_core_bft_mode == 1 ) then {
	// If real-time markers, update position before broadcast
	[false] call FUNC(updateMarkersPosition);
};

private _data = [];
// [0:kind,1:id,2:iconA,3:IconB,4:text,5:(???),6:pos,7:dir,8:vhl or grp]

{
	_data pushBack ['v', [_x select 0] call FUNC(getId), _x select 1, _x select 2, _x select 3, _x select 4, _x select 5, _x select 6];
} forEach cTabBFTvehicles;

{
	private _leader = (_x select 0);
	private _vehicle = vehicle _leader;
	private _vehicleId = '';
	if ( _vehicle != _leader ) then {
		_vehicleId = _vehicle call FUNC(getId);
	};
	_data pushBack ['g', [group _leader] call FUNC(getId), _x select 1, _x select 2, _x select 3, _x select 4, _x select 5, 0, _vehicleId];
} forEach cTabBFTgroups;

{
	_x params ["_mid","_markerData","_markerRawData"];
	// _markerData =>    [0:_pos,1:_texture1,  2:_texture2,  3:_dir,      4:_color,5:_text, 6:_align]
	// _markerRawData => [0:_pos,1:_markerIcon,2:_markerSize,3:_markerDir,4:_text, 5:_player]
	if (isNil "_markerRawData") then {
		_data pushBack ['u', format ["m%1", _mid], _markerData select 1, _markerData select 2, _markerData select 5, '', _markerData select 0, _markerData select 3];
	} else {
		_data pushBack ['u', format ["m%1", _mid], _markerData select 1, _markerData select 2, _markerData select 5, '', _markerData select 0, _markerData select 3, [group (_markerRawData select 5)] call FUNC(getId)];
	};
} forEach cTabUserMarkerList;

"cTabExtension" callExtension ["UpdateMarkers", _data];