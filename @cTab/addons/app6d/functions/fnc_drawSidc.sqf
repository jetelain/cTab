#include "script_component.hpp"

params ["_map", "_sidc", "_position", ["_size", 96]];

private _images = [_sidc] call FUNC(getSidcImagesCached);
{
	_map drawIcon [_x,[1,1,1,1],_position,_size,_size,0];
} forEach _images;

