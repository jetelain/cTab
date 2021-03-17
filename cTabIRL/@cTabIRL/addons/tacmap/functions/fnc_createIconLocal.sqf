#include "script_component.hpp"

params ['_id', '_x', '_y', '_icon', '_color', '_text', '_rotate'];
private _name = format ['_USER_DEFINED #0/tacmap%1/-1', _id];
private _marker = createMarkerLocal [_name, [_x, _y]];
_name setMarkerPosLocal [_x, _y];
_name setMarkerShapeLocal 'ICON';
_name setMarkerDirLocal _rotate;
_name setMarkerColorLocal _color; 
_name setMarkerTextLocal _text;
_name setMarkerTypeLocal _icon;
if ( _name isEqualTo _marker) then {
	GVAR(allMarkersLocal) pushBack _marker;
};