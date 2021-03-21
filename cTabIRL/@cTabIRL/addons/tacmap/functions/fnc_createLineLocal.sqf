#include "script_component.hpp"

params ['_id', '_points', '_color'];
_points params ['_x', '_y'];
private _marker = createMarkerLocal [ format ['_USER_DEFINED #0/tacmap%1/-1', _id], [_x, _y]];
_marker setMarkerShapeLocal 'polyline';
_marker setMarkerPolylineLocal _points;
_marker setMarkerColorLocal _color; 
GVAR(allMarkersLocal) pushBack _marker;