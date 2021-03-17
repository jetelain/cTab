#include "script_component.hpp"

params ['_id', '_x', '_y', '_icon', '_color', '_text', '_rotate'];
private _marker = createMarkerLocal [ format ['_USER_DEFINED #0/tacmap%1/-1', _id], [_x, _y]];
_marker setMarkerPosLocal [_x, _y];
_marker setMarkerShapeLocal 'ICON';
_marker setMarkerDirLocal _rotate;
_marker setMarkerColorLocal _color; 
_marker setMarkerTextLocal _text;
_marker setMarkerTypeLocal _icon;
GVAR(allMarkersLocal) pushBack _marker;