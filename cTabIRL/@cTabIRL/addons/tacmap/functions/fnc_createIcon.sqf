#include "script_component.hpp"

params ['_id', '_x', '_y', '_icon', '_color', '_text', '_rotate'];

private _existing = GVAR(allIconMarkers) get _id;
if ( !isNil "_existing" ) then {
	deleteMarker (_existing # 0);
};

private _marker = createMarkerLocal [format ['_USER_DEFINED #%1/tacmap%2/%3', clientOwner, _id, GVAR(channel)], [_x, _y], GVAR(channel), player];
_marker setMarkerPosLocal [_x, _y];
_marker setMarkerShapeLocal 'ICON';
_marker setMarkerDirLocal _rotate;
_marker setMarkerColorLocal _color; 
_marker setMarkerTextLocal _text;
if ( GVAR(channel) == -1 ) then {
	_marker setMarkerTypeLocal _icon;
} else {
	_marker setMarkerType _icon; 
	// Multiplayer optimisation: Global marker commands always broadcast the entire marker state over the network. 
	// As such, the number of network messages exchanged when creating or editing a marker can be reduced by performing 
	// all but the last operation using local marker commands, then using a global marker command for the last change 
	// (and subsequent global broadcast of all changes applied to the marker).
};
GVAR(allIconMarkers) set [_id, [_marker, _this]];
