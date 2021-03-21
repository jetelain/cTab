#include "script_component.hpp"

params ['_id', '_points', '_color'];
_points params ['_x', '_y'];
private _marker = createMarker [ format ['_USER_DEFINED #0/tacmap%1/0', _id], [_x, _y]];
_marker setMarkerShape 'polyline';
_marker setMarkerPolyline _points;
_marker setMarkerColor _color; 
GVAR(allMarkersGlobal) pushBack _marker;