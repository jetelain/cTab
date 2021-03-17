#include "script_component.hpp"

params ['_id', '_x', '_y', '_icon', '_color', '_text', '_rotate'];
private _marker = createMarker [ format ['_USER_DEFINED #0/tacmap%1/0', _id], [_x, _y]];
_marker setMarkerPos [_x, _y];
_marker setMarkerShape 'ICON';
_marker setMarkerDir _rotate;
_marker setMarkerColor _color; 
_marker setMarkerText _text;
_marker setMarkerType _icon;
GVAR(allMarkersGlobal) pushBack _marker;