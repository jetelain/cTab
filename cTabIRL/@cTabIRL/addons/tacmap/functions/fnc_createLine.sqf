#include "script_component.hpp"

params ['_id', '_points', '_color'];

private _existing = GVAR(allLineMarkers) get _id;
if ( !isNil "_existing" ) then {
	deleteMarker (_existing#0);
};

_points params ['_x', '_y'];
private _marker = createMarkerLocal [ format ['_USER_DEFINED #%1/tacmap%2/%3', clientOwner, _id, GVAR(channel)], [_x, _y], GVAR(channel), player];
_marker setMarkerShapeLocal 'polyline';
_marker setMarkerPolylineLocal _points;
if ( GVAR(channel) == -1 ) then {
	_marker setMarkerColorLocal _color; 
} else {
	_marker setMarkerColor _color;
	// Multiplayer optimisation: Global marker commands always broadcast the entire marker state over the network. 
	// As such, the number of network messages exchanged when creating or editing a marker can be reduced by performing 
	// all but the last operation using local marker commands, then using a global marker command for the last change 
	// (and subsequent global broadcast of all changes applied to the marker).
};
GVAR(allLineMarkers) set [_id, [_marker, _this]];
