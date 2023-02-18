#include "script_component.hpp"

if ( GVAR(deviceLevel) == 0 ) exitWith { };

private _vehicle = vehicle player;
private _data = getPosASL player;
_data pushBack direction player;
_data pushBack date;
_data pushBack ([group player] call FUNC(getId));
if ( _vehicle != player ) then {
	_data pushBack ([_vehicle] call FUNC(getId));

	if ( GVAR(vehicleMode) > 0 ) then {
		_data pushBack (vectorDir _vehicle);
		_data pushBack (vectorUp _vehicle);
		_data pushBack (velocity _vehicle);
		_data pushBack (getPosASL _vehicle);
		_data pushBack (wind);
	};
};

"cTabExtension" callExtension ["UpdatePosition", _data];

if ( ctab_core_bft_mode == 1 && { diag_tickTime > GVAR(nextMPU) } ) then {
	GVAR(nextMPU) = diag_tickTime + 1.5;
	[true] call FUNC(updateMarkersPosition);
};

if ( GVAR(mapMarkersNeedsUpdate) && GVAR(syncMap) ) then {
	GVAR(mapMarkersNeedsUpdate) = false;
	[] call FUNC(updateMapMarkers);
};
