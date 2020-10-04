#include "script_component.hpp"
/*
 	Name: cTab_fnc_degreeToOctant
 	
 	Author(s):
		Gundy

 	Description:
		Calculate octant from direction
	
	Parameters:
		0: INTEGER - Degree value to convert 0 to 360.
 	
 	Returns:
		STRING - Matching octant as either N, NE, E, SE, S, SW, W, NW
 	
 	Example:
		_octant = [direction player] call cTab_fnc_degreeToOctant;
*/

private ["_dir","_octant"];

_dir = _this select 0;
_octant = round (_dir / 45);

[LLSTRING(North),
LLSTRING(NorthEast),
LLSTRING(East),
LLSTRING(SouthEast),
LLSTRING(South),
LLSTRING(SouthWest),
LLSTRING(West),
LLSTRING(NorthWest),
LLSTRING(North)] select _octant