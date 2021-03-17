#include "script_component.hpp"

params ['_id', '_x', '_y', '_icon', '_color', '_text', '_rotate'];
private _name = format ['_USER_DEFINED #0/tacmap%1/0', _id];
private _marker = createMarker [_name, [_x, _y]];
_name setMarkerPos [_x, _y];
_name setMarkerShape 'ICON';
_name setMarkerDir _rotate;
_name setMarkerColor _color; 
_name setMarkerText _text;
_name setMarkerType _icon;
if ( _name isEqualTo _marker) then {
	GVAR(allMarkersGlobal) pushBack _marker;
};